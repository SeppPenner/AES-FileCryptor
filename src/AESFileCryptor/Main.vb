Imports System.IO
Imports System.Reflection
Imports System.Security.Cryptography
Imports System.Text
Imports System.Windows.Forms

Public Class Main
    Dim _salt() As Byte 'Saltwert erzeugen
    Dim _sprache As String 'Sprache erzeugen

    Private Sub Main_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Call Me.Button_Alle_Resetten_Click(sender, e) 'Alles resetten
        GC.Collect() 'Garbage-Collector aufrufen
        Blocksize = 1048576 'Blocksize auf 1 MB setzen
        'Datei öffnen und Sprache auslesen:
        Try
            'Datei öffnen
            Dim directoryLocation As String = Assembly.GetExecutingAssembly().Location
            Dim configFile = ""
            If (directoryLocation <> Nothing) Then
                configFile = Path.Combine(Directory.GetParent(directoryLocation).FullName, "Config.ini")
            End If
            Dim fs = New FileStream(configFile, FileMode.OpenOrCreate, FileAccess.ReadWrite)
            'Stream öffnen
            Dim r = New StreamReader(fs)
            'Zeiger auf den Anfang
            r.BaseStream.Seek(0, SeekOrigin.Begin)
            'Alle Zeilen lesen und an Console ausgeben
            While r.Peek() > -1
                Me._sprache = r.ReadLine() 'Sprache festsetzen
            End While
            'Reader und Stream schließen
            r.Close()
            fs.Close()
        Catch ex As Exception
            Me.WriteToLog(ex.ToString) 'Fehler ausgeben in Log-Datei
            MessageBox.Show(ex.ToString) 'Fehlermeldung ausgeben
        End Try
        'Verschlüsselungsarten anzeigen:
        Me.ComboBox_Art.Items.Add("AES-256") 'AES-256 als Verschlüsselungsart hinzufügen
        Me.ComboBox_Art.Items.Add("AES-128") 'AES-128 als Verschlüsselungsart hinzufügen
        Me.ComboBox_Art.SelectedIndex = 0 'Vorauswahl setzen, dass Combobox nicht leer
        'Sprache anpassen:
        Select Case Me._sprache
            Case "DE"
                Me.RadioButton_Deutsch.Checked = True 'RadioButton_Deutsch auswählen
            Case Else
                Me.RadioButton_Englisch.Checked = True 'RadioButton_Englisch auswählen
        End Select
    End Sub

    Private Sub EncryptFile(file As String) 'Eingangsdatei als Byte-Array einlesen
        GC.Collect() 'Garbage-Collector aufrufen
        BytesBereitsGelesen = 0 'BytesBereitsGelesen zurücksetzen
        Dim fInfo As New FileInfo(file) 'FileInfo anlegen
        Dim numBytes As Long = fInfo.Length 'Bytezahl auslesen
        Dim inStream As New FileStream(file, FileMode.Open, FileAccess.Read) 'FileStream (für Input) öffnen
        Dim br As New BinaryReader(inStream) 'Reader öffnen
        Dim data As Byte() 'Datenarray anlegen
        Dim outStream = New FileStream(Ausgabedateipfad, FileMode.Create) 'FileStream (für Output) öffnen
        Me.ProgressBar_Verschluesseln.Maximum = numBytes / Blocksize 'Verhältnis ausrechnen
        If Me.ProgressBar_Verschluesseln.Maximum = 0 Then
            Me.ProgressBar_Verschluesseln.Maximum = 1
        End If
        While BytesBereitsGelesen < numBytes
            Application.DoEvents() 'Dass Form nicht einfriert
            If (numBytes - BytesBereitsGelesen) > Blocksize Then
                Application.DoEvents() 'Dass Form nicht einfriert
                data = br.ReadBytes(Blocksize) 'Genau Blocksize lesen
                Application.DoEvents() 'Dass Form nicht einfriert
                BytesBereitsGelesen = BytesBereitsGelesen + Blocksize 'BytesBereitsGelesen um Blocksize erhöhen
                Application.DoEvents() 'Dass Form nicht einfriert
                Call Me.EncryptAes(AesSize, data, Me.RichTextBox_Passwort.Text, False) 'Verschlüsselung aufrufen
                Application.DoEvents() 'Dass Form nicht einfriert
                outStream.Write(Me._encryptedString, 0, Me._encryptedString.Length) 'Daten schreiben
                Application.DoEvents() 'Dass Form nicht einfriert
                Me.ProgressBar_Verschluesseln.PerformStep() 'Next step
            Else
                Application.DoEvents() 'Dass Form nicht einfriert
                data = br.ReadBytes(numBytes - BytesBereitsGelesen) 'Genau Blocksize lesen
                Application.DoEvents() 'Dass Form nicht einfriert
                BytesBereitsGelesen = numBytes 'BytesBereitsGelesen um Blocksize erhöhen
                Application.DoEvents() 'Dass Form nicht einfriert
                Call Me.EncryptAes(AesSize, data, Me.RichTextBox_Passwort.Text, True) 'Verschlüsselung aufrufen
                Application.DoEvents() 'Dass Form nicht einfriert
                outStream.Write(Me._encryptedString, 0, Me._encryptedString.Length) 'Daten schreiben
                Application.DoEvents() 'Dass Form nicht einfriert
                Me.ProgressBar_Verschluesseln.PerformStep() 'Next step
            End If
            Application.DoEvents() 'Dass Form nicht einfriert
        End While
        outStream.Close() 'FileStream (für Output) schließen
        inStream.Close() 'FileStream (für Input) schließen
        br.Close() 'Reader schließen
        Select Case Me._sprache
            Case "DE"
                MessageBox.Show("Datei " & Eingabedateipfad & " erfolgreich verschlüsselt!",
                                "Datei erfolgreich verschlüsselt!", MessageBoxButtons.OK, MessageBoxIcon.Information) _
                'Meldung ausgeben
            Case Else
                MessageBox.Show("File " & Eingabedateipfad & " successfully encrypted!", "File successfully encrypted!",
                                MessageBoxButtons.OK, MessageBoxIcon.Information) 'Meldung ausgeben
        End Select
        Application.DoEvents() 'Dass Form nicht einfriert
    End Sub

    Private Sub DecryptFile(file As String) 'Eingangsdatei als Byte-Array einlesen
        GC.Collect() 'Garbage-Collector aufrufen
        BytesBereitsGelesen = 0 'BytesBereitsGelesen zurücksetzen
        Dim fInfo As New FileInfo(file) 'FileInfo anlegen
        Dim numBytes As Long = fInfo.Length 'Bytezahl auslesen
        Dim inStream As New FileStream(file, FileMode.Open, FileAccess.Read) 'FileStream (für Input) öffnen
        Dim br As New BinaryReader(inStream) 'Reader öffnen
        Dim data As Byte() 'Datenarray anlegen
        Dim outStream = New FileStream(Ausgabedateipfad, FileMode.Create) 'FileStream (für Output) öffnen
        Me.ProgressBar_Verschluesseln.Maximum = numBytes / Blocksize 'Verhältnis ausrechnen
        If Me.ProgressBar_Verschluesseln.Maximum = 0 Then
            Me.ProgressBar_Verschluesseln.Maximum = 1
        End If
        While BytesBereitsGelesen < numBytes
            Application.DoEvents() 'Dass Form nicht einfriert
            If (numBytes - BytesBereitsGelesen) > Blocksize Then
                Application.DoEvents() 'Dass Form nicht einfriert
                data = br.ReadBytes(Blocksize) 'Genau Blocksize lesen
                Application.DoEvents() 'Dass Form nicht einfriert
                BytesBereitsGelesen = BytesBereitsGelesen + Blocksize 'BytesBereitsGelesen um Blocksize erhöhen
                Application.DoEvents() 'Dass Form nicht einfriert
                Call Me.DecryptAes(AesSize, data, Me.RichTextBox_Passwort.Text, False) 'Verschlüsselung aufrufen
                Application.DoEvents() 'Dass Form nicht einfriert
                outStream.Write(Me._decryptedString, 0, Me._decryptedString.Length) 'Daten schreiben
                Application.DoEvents() 'Dass Form nicht einfriert
                Me.ProgressBar_Verschluesseln.PerformStep() 'Next step
            Else
                Application.DoEvents() 'Dass Form nicht einfriert
                data = br.ReadBytes(numBytes - BytesBereitsGelesen) 'Genau Blocksize lesen
                Application.DoEvents() 'Dass Form nicht einfriert
                BytesBereitsGelesen = numBytes 'BytesBereitsGelesen um Blocksize erhöhen
                Application.DoEvents() 'Dass Form nicht einfriert
                Call Me.DecryptAes(AesSize, data, Me.RichTextBox_Passwort.Text, True) 'Verschlüsselung aufrufen
                Application.DoEvents() 'Dass Form nicht einfriert
                outStream.Write(Me._decryptedString, 0, Me._decryptedString.Length) 'Daten schreiben
                Application.DoEvents() 'Dass Form nicht einfriert
                Me.ProgressBar_Verschluesseln.PerformStep() 'Next step
            End If
            Application.DoEvents() 'Dass Form nicht einfriert
        End While
        outStream.Close() 'FileStream (für Output) schließen
        inStream.Close() 'FileStream (für Input) schließen
        br.Close() 'Reader schließen
        Select Case Me._sprache
            Case "DE"
                MessageBox.Show("Datei " & Eingabedateipfad & " erfolgreich entschlüsselt!",
                                "Datei erfolgreich entschlüsselt!", MessageBoxButtons.OK, MessageBoxIcon.Information) _
                'Meldung ausgeben
            Case Else
                MessageBox.Show("File " & Eingabedateipfad & " successfully decrypted!", "File successfully decrypted!",
                                MessageBoxButtons.OK, MessageBoxIcon.Information) 'Meldung ausgeben
        End Select
        Application.DoEvents() 'Dass Form nicht einfriert
    End Sub

    Private Sub RadioButton_Deutsch_CheckedChanged(sender As Object, e As EventArgs) _
        Handles RadioButton_Deutsch.CheckedChanged
        If Me.RadioButton_Deutsch.Checked = True Then
            Call Me.AllesAufDeutsch() 'Alles auf Deutsch übersetzen
        Else
            Call Me.AllesAufEnglisch() 'Alles auf Englisch übersetzen
        End If
    End Sub

    Private Sub RadioButton_Englisch_CheckedChanged(sender As Object, e As EventArgs) _
        Handles RadioButton_Englisch.CheckedChanged
        If Me.RadioButton_Englisch.Checked = True Then
            Call Me.AllesAufEnglisch() 'Alles auf Englisch übersetzen
        Else
            Call Me.AllesAufDeutsch() 'Alles auf Deutsch übersetzen
        End If
    End Sub

    Private Sub AllesAufDeutsch() 'Alles auf Deutsch übersetzen
        Me.Text = "AES Dateiverschlüsselung" 'Text von Form setzen
        Me.Label_Art.Text = "Bitte Verschlüsselungsart auswählen:" 'Label_Art Text setzen
        Me.Label_Salt.Text = "Salteingabe:" 'Label_Salt Text setzen
        Me.Label_Passwort.Text = "Passworteingabe:" 'Label_Passwort Text setzen
        Me.Label_Eingabe.Text = "Dateieingabe:" 'Label_Eingabe Text setzen
        Me.Label_Ausgabe.Text = "Dateiausgabe:" 'Label_Ausgabe Text setzen
        Me.Button_Verschluesseln.Text = "Verschlüsseln" 'Button_Verschluesseln Text setzen
        Me.Button_Entschluesseln.Text = "Entschlüsseln" 'Button_Entschluesseln Text setzen
        Me.Button_Alle_Resetten.Text = "Alles löschen" 'Button_Alle_Resetten Text setzen
        Me.Label_Sprache.Text = "Sprache auswählen:" 'Label_Sprache Text setzen
        Me.RadioButton_Deutsch.Text = "Deutsch" 'RadioButton_Deutsch Text setzen
        Me.RadioButton_Englisch.Text = "Englisch" 'RadioButton_Englisch Text setzen
        Me._sprache = "DE" 'Sprache festlegen
        Me.OpenFileDialog_Eingabe.Filter = "Alle Dateien (*.*)|*.*" 'OpenFileDialog_Eingabe Filter setzen
        Me.OpenFileDialog_Eingabe.Title = "Eingabedatei auswählen" 'OpenFileDialog_Eingabe Titel setzen
        Me.SaveFileDialog_Ausgabe.Filter = "Alle Dateien (*.*)|*.*" 'SaveFileDialog_Ausgabe Filter setzen
        Me.SaveFileDialog_Ausgabe.Title = "Ausgabedatei auswählen" 'SaveFileDialog_Ausgabe Titel setzen
        Me.Button_Eingabe.Text = "Eingabedatei auswählen" 'Button_Eingabe Text setzen
        Me.Button_Ausgabe.Text = "Ausgabedatei auswählen" 'Button_Ausgabe Text setzen
        Me.Label_Ausgabe.Text = "Dateiausgabe: " 'Label_Ausgabe Text setzen
    End Sub

    Private Sub AllesAufEnglisch() 'Alles auf Englisch übersetzen
        Me.Text = "AES File Cryptor" 'Text von Form setzen
        Me.Label_Art.Text = "Choose encryption method:" 'Label_Art Text setzen
        Me.Label_Salt.Text = "Salt input:" 'Label_Salt Text setzen
        Me.Label_Passwort.Text = "Password input:" 'Label_Passwort Text setzen
        Me.Label_Eingabe.Text = "File input:" 'Label_Eingabe Text setzen
        Me.Label_Ausgabe.Text = "File output:" 'Label_Ausgabe Text setzen
        Me.Button_Verschluesseln.Text = "Encrypt" 'Button_Verschluesseln Text setzen
        Me.Button_Entschluesseln.Text = "Decrypt" 'Button_Entschluesseln Text setzen
        Me.Button_Alle_Resetten.Text = "Clear all" 'Button_Alle_Resetten Text setzen
        Me.Label_Sprache.Text = "Choose language:" 'Label_Sprache Text setzen
        Me.RadioButton_Deutsch.Text = "German" 'RadioButton_Deutsch Text setzen
        Me.RadioButton_Englisch.Text = "English" 'RadioButton_Englisch Text setzen
        Me._sprache = "EN" 'Sprache festlegen
        Me.OpenFileDialog_Eingabe.Filter = "All files (*.*)|*.*" 'OpenFileDialog_Eingabe Filter setzen
        Me.OpenFileDialog_Eingabe.Title = "Select input file" 'OpenFileDialog_Eingabe Titel setzen
        Me.SaveFileDialog_Ausgabe.Filter = "All files (*.*)|*.*" 'SaveFileDialog_Ausgabe Filter setzen
        Me.SaveFileDialog_Ausgabe.Title = "Select output file" 'SaveFileDialog_Ausgabe Titel setzen
        Me.Button_Eingabe.Text = "Select input file" 'Button_Eingabe Text setzen
        Me.Button_Ausgabe.Text = "Select output file" 'Button_Ausgabe Text setzen
        Me.Label_Ausgabe.Text = "File output:" 'Label_Ausgabe Text setzen
    End Sub

    Private Sub Button_Verschluesseln_Click(sender As Object, e As EventArgs) Handles Button_Verschluesseln.Click _
        'Text verschlüsseln
        Try
            LastBlockFlushed = False 'LastBlockFlushed auf false setzen
            Me.ProgressBar_Verschluesseln.Value = 0 'ProgressBar_Verschluesseln zurücksetzen
            Select Case Me.ComboBox_Art.SelectedIndex
                Case 0 'AES-256 ausgewählt
                    AesSize = 256 'AESSize auf 256 setzen
                    If Me.RichTextBox_Passwort.Text = "" Or Me.Label_Eingabedatei.Text = "" Then 'Wenn Felder leer sind
                        Select Case Me._sprache
                            Case "DE"
                                MessageBox.Show("Passwort oder Dateieingabe ist leer") 'Fehlermeldung ausgeben
                            Case Else
                                MessageBox.Show("Password or file input is empty") 'Fehlermeldung ausgeben
                        End Select
                    Else 'Wenn Felder gefüllt sind
                        If Me.RichTextBox_Salt.TextLength < 8 Then 'Wenn Saltwert zu klein ist
                            Select Case Me._sprache
                                Case "DE"
                                    MessageBox.Show("Saltwert muss mindestens 8 Zeichen enthalten") _
                                    'Fehlermeldung ausgeben
                                Case Else
                                    MessageBox.Show("Salt value must contain at least 8 characters") _
                                    'Fehlermeldung ausgeben
                            End Select
                        Else
                            Me._salt = Encoding.UTF32.GetBytes(Me.RichTextBox_Salt.Text) 'Salt aus Benutzereingabe auslesen
                            Call Me.EncryptFile(Me.Label_Eingabedatei.Text) 'Datei verschlüsseln
                        End If
                    End If
                Case 1 'AES-128 ausgewählt
                    AesSize = 128 'AESSize auf 128 setzen
                    If Me.RichTextBox_Passwort.Text = "" Or Me.Label_Eingabedatei.Text = "" Then 'Wenn Felder leer sind
                        Select Case Me._sprache
                            Case "DE"
                                MessageBox.Show("Passwort oder Dateieingabe ist leer") 'Fehlermeldung ausgeben
                            Case Else
                                MessageBox.Show("Password or file input is empty") 'Fehlermeldung ausgeben
                        End Select
                    Else 'Wenn Felder gefüllt sind

                        If Me.RichTextBox_Salt.TextLength < 8 Then 'Wenn Saltwert zu klein ist
                            Select Case Me._sprache
                                Case "DE"
                                    MessageBox.Show("Saltwert muss mindestens 8 Zeichen enthalten") _
                                    'Fehlermeldung ausgeben
                                Case Else
                                    MessageBox.Show("Salt value must contain at least 8 characters") _
                                    'Fehlermeldung ausgeben
                            End Select
                        Else
                            Me._salt = Encoding.UTF32.GetBytes(Me.RichTextBox_Salt.Text) 'Salt aus Benutzereingabe auslesen
                            Call Me.EncryptFile(Me.Label_Eingabedatei.Text) 'Datei verschlüsseln
                        End If
                    End If
            End Select
        Catch ex As Exception
            Me.WriteToLog(ex.ToString) 'Fehler ausgeben in Log-Datei
            MessageBox.Show(ex.ToString) 'Fehlermeldung ausgeben
        End Try
    End Sub

    Private Sub Button_Entschluesseln_Click(sender As Object, e As EventArgs) Handles Button_Entschluesseln.Click
        Try
            LastBlockFlushed = False 'LastBlockFlushed auf false setzen
            Me.ProgressBar_Verschluesseln.Value = 0 'ProgressBar_Verschluesseln zurücksetzen
            Select Case Me.ComboBox_Art.SelectedIndex
                Case 0 'AES-256 ausgewählt
                    AesSize = 256 'AESSize auf 256 setzen
                    If Me.RichTextBox_Passwort.Text = "" Or Me.Label_Eingabedatei.Text = "" Then 'Wenn Felder leer sind
                        Select Case Me._sprache
                            Case "DE"
                                MessageBox.Show("Passwort oder Dateieingabe ist leer") 'Fehlermeldung ausgeben
                            Case Else
                                MessageBox.Show("Password or file input is empty") 'Fehlermeldung ausgeben
                        End Select
                    Else 'Wenn Felder gefüllt sind

                        If Me.RichTextBox_Salt.TextLength < 8 Then 'Wenn Saltwert zu klein ist
                            Select Case Me._sprache
                                Case "DE"
                                    MessageBox.Show("Saltwert muss mindestens 8 Zeichen enthalten") _
                                    'Fehlermeldung ausgeben
                                Case Else
                                    MessageBox.Show("Salt value must contain at least 8 characters") _
                                    'Fehlermeldung ausgeben
                            End Select
                        Else
                            Me._salt = Encoding.UTF32.GetBytes(Me.RichTextBox_Salt.Text) 'Salt aus Benutzereingabe auslesen
                            Call Me.DecryptFile(Me.Label_Eingabedatei.Text) 'Datei entschlüsseln
                        End If
                    End If
                Case 1 'AES-128 ausgewählt
                    AesSize = 128 'AESSize auf 128 setzen
                    If Me.RichTextBox_Passwort.Text = "" Or Me.Label_Eingabedatei.Text = "" Then 'Wenn Felder leer sind
                        Select Case Me._sprache
                            Case "DE"
                                MessageBox.Show("Passwort oder Dateieingabe ist leer") 'Fehlermeldung ausgeben
                            Case Else
                                MessageBox.Show("Password or file input is empty") 'Fehlermeldung ausgeben
                        End Select
                    Else 'Wenn Felder gefüllt sind

                        If Me.RichTextBox_Salt.TextLength < 8 Then 'Wenn Saltwert zu klein ist
                            Select Case Me._sprache
                                Case "DE"
                                    MessageBox.Show("Saltwert muss mindestens 8 Zeichen enthalten") _
                                    'Fehlermeldung ausgeben
                                Case Else
                                    MessageBox.Show("Salt value must contain at least 8 characters") _
                                    'Fehlermeldung ausgeben
                            End Select
                        Else
                            Me._salt = Encoding.UTF32.GetBytes(Me.RichTextBox_Salt.Text) 'Salt aus Benutzereingabe auslesen
                            Call Me.DecryptFile(Me.Label_Eingabedatei.Text) 'Datei entschlüsseln
                        End If
                    End If
            End Select
        Catch ex As Exception
            Me.WriteToLog(ex.ToString) 'Fehler ausgeben in Log-Datei
            MessageBox.Show(ex.ToString) 'Fehlermeldung ausgeben
        End Try
    End Sub

    Private Sub Button_Alle_Resetten_Click(sender As Object, e As EventArgs) Handles Button_Alle_Resetten.Click
        Me.RichTextBox_Salt.Clear() 'RichTextBox_Salt leeren
        Me.RichTextBox_Passwort.Clear() 'RichTextBox_Passwort leeren
        Me.Label_Eingabedatei.Text = "" 'Label_Eingabedatei leeren
        Me.Label_Ausgabedatei.Text = "" 'Label_Ausgabedatei leeren
        Me.ProgressBar_Verschluesseln.Value = 0 'ProgressBar_Verschluesseln resetten
    End Sub

    Private _encryptedString() As Byte
    Private _decryptedString() As Byte

    ' Verschlüsseln
    Private Sub EncryptAes(aesKeySize As Integer, decryptedString() As Byte, password As String, isLastBlock As Boolean)

        Dim generierterKey As New Rfc2898DeriveBytes(password, Me._salt, 600000, HashAlgorithmName.SHA256)
        Dim aes As Aes = Aes.Create()
        aes.KeySize = aesKeySize ' möglich sind 128 oder 256 bit
        aes.BlockSize = 128

        ' Algorithmus initialisieren:
        aes.Key = generierterKey.GetBytes(aes.KeySize \ 8)
        aes.IV = generierterKey.GetBytes(aes.BlockSize \ 8)

        ' Memory-Stream und Crypto-Stream erzeugen -> CreateEncryptor()
        Dim ms As New MemoryStream
        Dim cs As New CryptoStream(ms, aes.CreateEncryptor(),
                                   CryptoStreamMode.Write)

        ' Daten verschlüsseln:
        Dim data() As Byte
        data = decryptedString
        cs.Write(data, 0, data.Length)
        If isLastBlock = True Then
            cs.FlushFinalBlock()
            cs.Close()
            LastBlockFlushed = True
        End If

        ' Verschlüsselte Daten als Byte ausgeben: 
        Me._encryptedString = ms.ToArray
        ms.Close()

        aes.Clear()
    End Sub

    ' Entschlüsseln
    Private Sub DecryptAes(aesKeySize As Int32, encryptedString() As Byte, password As String, isLastBlock As Boolean)

        Dim generierterKey As New Rfc2898DeriveBytes(password, Me._salt, 600000, HashAlgorithmName.SHA256)
        ' Instanzierung des AES-Algorithmus-Objekts:
        Dim aes As Aes = Aes.Create()
        ' Ein mit 256 bit verschlüsseltes Byte kann 
        ' auch nur mit 256 bit entschlüsselt werden!
        aes.KeySize = aesKeySize ' möglich sind 128 oder 256 bit
        aes.BlockSize = 128

        ' Algorithmus initialisieren:
        aes.Key = generierterKey.GetBytes(aes.KeySize \ 8)
        aes.IV = generierterKey.GetBytes(aes.BlockSize \ 8)

        ' Memory-Stream und Crypto-Stream erzeugen -> CreateDecryptor()
        Dim ms As New MemoryStream
        Dim cs As New CryptoStream(ms, aes.CreateDecryptor(),
                                   CryptoStreamMode.Write)

        Try ' Daten entschlüsseln:
            Dim data() As Byte
            data = encryptedString
            cs.Write(data, 0, data.Length)
            If isLastBlock = True Then
                cs.FlushFinalBlock()
                cs.Close()
                LastBlockFlushed = True
            End If

            ' Die entschlüsselten Daten als Byte ausgeben: 
            Me._decryptedString = ms.ToArray
            ms.Close()

            aes.Clear()
        Catch ex As Exception
            Me.WriteToLog(ex.ToString) 'Fehler ausgeben in Log-Datei
            MessageBox.Show(ex.ToString)
        End Try
    End Sub

    Private Sub Button_Eingabe_Click(sender As Object, e As EventArgs) Handles Button_Eingabe.Click
        If Me.OpenFileDialog_Eingabe.ShowDialog() = DialogResult.OK Then
            Eingabedateipfad = Me.OpenFileDialog_Eingabe.FileName 'Eingabedateipfad speichern
            Me.Label_Eingabedatei.Text = Eingabedateipfad 'Label_Eingabedatei Text ausgeben
            Select Case Me._sprache
                Case "DE"
                    Me.Label_Ausgabe.Text = "Dateiausgabe: " 'Label_Ausgabe Text setzen
                Case Else
                    Me.Label_Ausgabe.Text = "File output:" 'Label_Ausgabe Text setzen
            End Select
        End If
    End Sub

    Private Sub Button_Ausgabe_Click(sender As Object, e As EventArgs) Handles Button_Ausgabe.Click
        If Me.SaveFileDialog_Ausgabe.ShowDialog() = DialogResult.OK Then
            Ausgabedateipfad = Me.SaveFileDialog_Ausgabe.FileName 'Ausgabedateipfad speichern
            Me.Label_Ausgabedatei.Text = Ausgabedateipfad 'Label_Ausgabedatei Text ausgeben
            Select Case Me._sprache
                Case "DE"
                    Me.Label_Ausgabe.Text = "Dateiausgabe: " 'Label_Ausgabe Text setzen
                Case Else
                    Me.Label_Ausgabe.Text = "File output:" 'Label_Ausgabe Text setzen
            End Select
        End If
    End Sub

    Private Sub WriteToLog(textParam As String)
        Try
            If Directory.Exists(AppDomain.CurrentDomain.BaseDirectory() & "log\") = False Then
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory() & "log\") 'Verzeichnis erstellen
            End If
            'Datum anpassen
            Dim currentDateMonth
            If Date.Today.Month < 10 Then
                currentDateMonth = "0" & Date.Today.Month
            Else
                currentDateMonth = Date.Today.Month
            End If
            Dim currentDateDay
            If Date.Today.Day < 10 Then
                currentDateDay = "0" & Date.Today.Day
            Else
                currentDateDay = Date.Today.Day
            End If
            'Dateipfad anlegen:
            Dim dateipfad As String = AppDomain.CurrentDomain.BaseDirectory() & "log\" & Date.Today.Year & "_" &
                                      currentDateMonth & "_" & currentDateDay & "_" & ".txt"
            If File.Exists(dateipfad) = False Then
                Using logFile As FileStream = File.Create(dateipfad, 200, FileOptions.Asynchronous)
                    logFile.Close()
                End Using
            End If
            'Datei öffnen
            Dim fs = New FileStream(dateipfad, FileMode.Append, FileAccess.Write)
            'Stream öffnen
            Dim w = New StreamWriter(fs)
            'Anfügen am Ende
            w.BaseStream.Seek(0, SeekOrigin.End)
            'Zeilen schreiben
            w.WriteLine(
                "------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------")
            'Daten anpassen für Log:
            w.WriteLine(currentDateDay & "." & currentDateMonth & "." & Date.Today.Year & "-" & TimeOfDay)
            w.Write(textParam)
            w.WriteLine()
            'Writer und Stream schließen
            w.Close()
            fs.Close()
        Catch ex As Exception
            MessageBox.Show(ex.ToString) 'Fehlermeldung ausgeben
        End Try
    End Sub
End Class
