# Project rules for Claude

## What this is

AES-FileCryptor is a small Windows Forms application written in **Visual Basic** that encrypts and
decrypts arbitrary files with AES. It is a program, **not** a library: no `GeneratePackageOnBuild`,
no NuGet push script. It ships as an Inno Setup installer that is committed into this repository.

One solution `src/AESFileCryptor.sln` with exactly one project:

- `src/AESFileCryptor/AESFileCryptor.vbproj`, `OutputType` `WinExe`, `StartupObject`
  `AESFileCryptor.Main`. There is no `Program.vb` and no `My Project` folder, the form itself is the
  startup object.

Layout inside `src/AESFileCryptor`:

- `Main.vb`: everything. Loading the language from `Config.ini`, the two translation subs
  `AllesAufDeutsch` and `AllesAufEnglisch`, the file dialogs, `PrepareCryptoRun` (input validation
  plus key size and salt), `EncryptFile` and `DecryptFile` (the streaming loop), `CreateAes` (key
  derivation), `ResetProgressBar` and `WriteToLog`.
- `Main.Designer.vb` and `Main.resx`: designer generated, do not hand edit the control layout.
- `Module1.vb`: public module level variables shared between the subs.
- `Config.ini`: one line, `DE` or `EN`, copied to the output directory with
  `CopyToOutputDirectory=Always`.
- `License.txt` and `AES.ico`: also copied to the output, the icon is the `ApplicationIcon`.

Repository root: `README.md` (the only user documentation), `Changelog.md`, `License.txt` (MIT),
`Screenshot_DE.PNG`, `Screenshot_EN.PNG`, `.gitattributes` and `.gitignore`. `Setup/` holds the
Inno Setup script `AES-FileCryptor-Skript.iss`, the publish helper `build-setup-files.bat` and the
committed installer `AES-FileCryptor-Setup.exe`. There is no test project, no `Directory.Build.props`
and no `.github` folder.

## Build

```powershell
dotnet build src/AESFileCryptor.sln -c Release
```

- Single target framework `net10.0-windows`, `RuntimeIdentifiers` `win-x64`, no multi-targeting.
- All build properties live directly in `AESFileCryptor.vbproj`. There is **no**
  `Directory.Build.props` in this repository.
- `TreatWarningsAsErrors` is enabled, so every warning breaks the build, NuGet warnings (`NU****`)
  from restore included. A clean build reports zero warnings, keep it that way. This was switched on
  in version 1.0.8.0 after the 22 warnings that existed until 1.0.7.0 were fixed.
- `NU1803` (HTTP source usage during restore) is the one warning suppressed via `NoWarn`. Fix
  warnings instead of extending that list. `NuGetAudit` and `NuGetAuditMode=all` are on, so a
  vulnerable transitive package fails the build too.
- Versions come from GitVersion.MsBuild out of the git tags, for example `1.0.8-1` for the first
  commit after tag `1.0.8`. Never edit a version property or an assembly version by hand.
- Restore needs nuget.org. If a private feed is configured globally on the machine and answers 404
  for public packages, restore fails with `NU1301`. Then build with an explicit source:
  `dotnet build src/AESFileCryptor.sln --source https://api.nuget.org/v3/index.json`.
- **There are no automated tests.** A behaviour change in the crypto path is verified by an
  encrypt and decrypt roundtrip with files on both sides of the 1 MB block boundary, at least
  100 bytes, exactly 1048576 bytes, 1500000 bytes and 3000000 bytes. Sizes that are an exact
  multiple of 1048576 are the interesting ones, that is where the format used to break. Never claim
  a roundtrip happened without running it.
- The installer is built by `Setup/build-setup-files.bat` (deletes `bin` and `obj`, publishes self
  contained for `win-x64`, deletes the `*.pdb`) followed by `ISCC.exe` on
  `Setup/AES-FileCryptor-Skript.iss`. In this environment `NoDefaultCurrentDirectoryInExePath` is
  set, so the batch file has to be started as `call .\build-setup-files.bat` from within `Setup`,
  because its `cd ..\src` is relative to the start directory.

## Code conventions

Follow the surrounding code:

- Visual Basic, `Me.` qualification for every field, property and method.
- Inline comments sit at the end of the line and are **German**, the language of the existing
  comments. Longer explanations go on their own line above the code, also German.
- No `Option Strict`. Until 1.0.7.0 the code needed that, `WriteToLog` built its date strings from
  untyped `Object` variables. Since 1.0.8.0 the project compiles with `<OptionStrict>On</OptionStrict>`
  without a single error, so switching it on is a real option, it is simply not done yet.
- `Nullable`, `ImplicitUsings` and `LangVersion latest` are enabled, unusual for a VB project but
  intentional and inherited from the sibling repositories.
- New `Imports` go to the top of the file, VB has no global usings file here.
- `src/.editorconfig` sets CRLF, four spaces, UTF-8 and `IDE0005` as warning. Everything else in it
  is a `csharp_*` or `dotnet_style_*` rule that does not apply to this VB project, it is a copy of
  the file the sibling repositories use. Leave it alone.

## Known quirks

Do not silently "clean up" these, they are existing behaviour:

- **The assembly is `AESFileCryptor.exe`, the product is `AES-FileCryptor`.** The repository, the
  installer and the README use the spelling with the hyphen, the project and therefore the built
  executable do not. Until version 1.0.7.0 the Inno Setup script defined
  `MyAppExeName "AES-FileCryptor.exe"`, so every start menu entry, the desktop icon, the uninstall
  icon and the post install launch pointed at a file that never existed. Fixed in 1.0.8.0 by
  correcting the script, not by renaming the assembly.
- **One CryptoStream per file, not per block.** `EncryptFile` and `DecryptFile` read the file in
  1 MB chunks so that the progress bar moves and `Application.DoEvents` keeps the form alive, but
  all chunks go through a single `CryptoStream`, and `FlushFinalBlock` is called exactly once at
  the end. Until 1.0.7.0 every chunk got its own `Aes` object with the same key and the same IV.
  That restarted the CBC chain per chunk and, on decryption, left the 16 bytes that the decryptor
  holds back for depadding unwritten. The result was a crash
  (`Padding is invalid and cannot be removed`) for files whose size is a multiple of 1 MB, and
  silent loss of 16 bytes per block boundary otherwise. Do not reintroduce per block cipher
  objects.
- **Files above 1 MB written before 1.0.8.0 are not readable.** That is not a regression, those
  files could not be decrypted correctly by 1.0.7.0 either. Below and at 1048576 bytes the format
  is unchanged and byte identical, so old files of that size still decrypt.
- **An empty input file now produces a 16 byte output.** Before 1.0.8.0 the loop never ran and the
  padding block was never written, so encrypting an empty file produced an empty file. Both
  versions decrypt back to empty.
- **Key derivation must stay bit compatible.** `CreateAes` derives key and IV in one
  `Rfc2898DeriveBytes.Pbkdf2` call of `keyLength + ivLength` bytes. That produces exactly the same
  bytes as the two consecutive `GetBytes` calls on an `Rfc2898DeriveBytes` instance that the code
  used until 1.0.7.0, whose constructors are obsolete since .NET 10 (`SYSLIB0060`). Changing the
  iteration count (600000), the hash (SHA256) or the salt encoding (`Encoding.UTF32`) makes every
  existing file unreadable.
- **The salt comes from the user and is not stored.** The user has to type the same salt again to
  decrypt, the file contains neither salt nor IV nor a header. A wrong salt or password fails in
  `FlushFinalBlock` with `Padding is invalid and cannot be removed` and leaves a partially written
  output file behind. Until 1.0.7.0 `DecryptAes` caught that exception itself, so the user got an
  error dialog **and** afterwards the "successfully decrypted" dialog. The exception now reaches the
  button handler, which logs it and shows it, and no success message follows.
- **The language is a single line in `Config.ini`.** `Main_Load` reads the **last** line of the
  file, valid values are `DE` and `EN`, anything else falls through to English. Switching the radio
  button changes the UI language but does not write the file back, the setting is only persistent
  if the file is edited by hand. That is what the README documents.
- **`src/Config.ini` and `src/AES.ico` are leftovers.** They sit next to the solution, outside the
  project folder, are not referenced by anything and are not copied to the output. `src/AES.ico` is
  byte identical to the project icon, `src/Config.ini` says `EN` while the project one says `DE`.
  They are tracked, leave them alone.
- **`Module1.vb` holds dead variables.** `FailureWithEncryption` is never read or written,
  `LastBlockFlushed` is written but never read. `Blocksize` is not a constant, it is set to
  1048576 in `Main_Load`.
- **`WriteToLog` writes into `log\` next to the executable**, one file per day named
  `yyyy_MM_dd_.txt`, note the trailing underscore. Every caught exception goes there and into a
  `MessageBox` showing the full stack trace.
- **AppVeyor badge without CI in the repository.** `README.md` links an AppVeyor build that is
  configured outside of this repository. There is no pipeline file here.
- **`.gitattributes` sets `* text=auto`** and every rule of the Visual Studio template below it is
  commented out. A new binary file needs its own rule.
- **The installer is committed.** `Setup/AES-FileCryptor-Setup.exe` is tracked although `.gitignore`
  excludes `*.exe`, so it needs `git add -f`. Self contained since 1.0.8.0, which grows the
  repository by roughly 35 MB per release.

## Releasing

1. Make the change.
2. Add an entry at the top of `Changelog.md` in the existing format:
   `* **Version 1.0.8.0 (2026-08-10)** : Short description.`
3. Set `MyAppVersion` in `Setup/AES-FileCryptor-Skript.iss` to the same four part version. The file
   is UTF-8 **with** BOM since 1.0.8.0, keep the BOM and the CRLF line endings, otherwise Inno Setup
   falls back to the system code page and mangles `Hämmer Electronics` in the installer metadata.
4. Commit that.
5. Tag the commit with the plain version number, no `v` prefix (`1.0.8`, `1.0.7`, ...). The existing
   tags are lightweight tags, create new ones the same way.
6. **Only then** build the installer: `Setup/build-setup-files.bat`, then `ISCC.exe` on the script.
   Tag first, otherwise GitVersion burns a prerelease version into the shipped executable.
7. `git add -f Setup/AES-FileCryptor-Setup.exe` and commit it.
8. Push the commits and the tag.

The version in the `Changelog.md` has four parts (`1.0.8.0`), the tag has three (`1.0.8`).
GitVersion turns the tag into the assembly version, so an untagged commit produces something like
`1.0.8-1+Branch.master.Sha...`.

## Git

- **Never amend a commit.** No `git commit --amend`, not for a typo in the message, not to add a
  forgotten file, not even when the commit is still local. Write a follow-up commit instead. The
  release versions come from tags on exact commits, an amended commit leaves its tag pointing at a
  commit that no longer exists in the branch.

## Writing style

- Commit messages are written **in English only**: short, precise subject line, explanatory body
  when needed.
- Comments in project files such as `.vbproj` are English. The comments in `Main.vb` are German,
  that is the existing style of that file, keep it.
- **No em dashes or en dashes** (`—`, `–`), neither in prose, commit messages, code comments nor
  documentation. Use a regular hyphen, comma, colon, parentheses or a separate sentence.
- German texts (documentation, chat replies, the comments in `Main.vb`) always use real umlauts and
  ß, never ASCII transliterations such as `ae`, `oe`, `ue` or `ss`.
