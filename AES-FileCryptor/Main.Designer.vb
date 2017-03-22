Imports System.ComponentModel
Imports Microsoft.VisualBasic.CompilerServices

<DesignerGenerated()> _
Partial Class Main
    Inherits Form

    'Form overrides dispose to clean up the component list.
    <DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Main))
        Me.Label_Sprache = New System.Windows.Forms.Label()
        Me.RadioButton_Englisch = New System.Windows.Forms.RadioButton()
        Me.RadioButton_Deutsch = New System.Windows.Forms.RadioButton()
        Me.Button_Alle_Resetten = New System.Windows.Forms.Button()
        Me.RichTextBox_Passwort = New System.Windows.Forms.RichTextBox()
        Me.Label_Passwort = New System.Windows.Forms.Label()
        Me.RichTextBox_Salt = New System.Windows.Forms.RichTextBox()
        Me.Label_Salt = New System.Windows.Forms.Label()
        Me.Button_Entschluesseln = New System.Windows.Forms.Button()
        Me.Button_Verschluesseln = New System.Windows.Forms.Button()
        Me.Label_Ausgabe = New System.Windows.Forms.Label()
        Me.Label_Eingabe = New System.Windows.Forms.Label()
        Me.Label_Art = New System.Windows.Forms.Label()
        Me.ComboBox_Art = New System.Windows.Forms.ComboBox()
        Me.OpenFileDialog_Eingabe = New System.Windows.Forms.OpenFileDialog()
        Me.Button_Eingabe = New System.Windows.Forms.Button()
        Me.Label_Eingabedatei = New System.Windows.Forms.Label()
        Me.Label_Ausgabedatei = New System.Windows.Forms.Label()
        Me.SaveFileDialog_Ausgabe = New System.Windows.Forms.SaveFileDialog()
        Me.Button_Ausgabe = New System.Windows.Forms.Button()
        Me.ProgressBar_Verschluesseln = New System.Windows.Forms.ProgressBar()
        Me.SuspendLayout()
        '
        'Label_Sprache
        '
        Me.Label_Sprache.AutoSize = True
        Me.Label_Sprache.Location = New System.Drawing.Point(744, 30)
        Me.Label_Sprache.Name = "Label_Sprache"
        Me.Label_Sprache.Size = New System.Drawing.Size(0, 13)
        Me.Label_Sprache.TabIndex = 33
        '
        'RadioButton_Englisch
        '
        Me.RadioButton_Englisch.AutoSize = True
        Me.RadioButton_Englisch.Location = New System.Drawing.Point(819, 50)
        Me.RadioButton_Englisch.Name = "RadioButton_Englisch"
        Me.RadioButton_Englisch.Size = New System.Drawing.Size(14, 13)
        Me.RadioButton_Englisch.TabIndex = 32
        Me.RadioButton_Englisch.TabStop = True
        Me.RadioButton_Englisch.UseVisualStyleBackColor = True
        '
        'RadioButton_Deutsch
        '
        Me.RadioButton_Deutsch.AutoSize = True
        Me.RadioButton_Deutsch.Location = New System.Drawing.Point(747, 50)
        Me.RadioButton_Deutsch.Name = "RadioButton_Deutsch"
        Me.RadioButton_Deutsch.Size = New System.Drawing.Size(14, 13)
        Me.RadioButton_Deutsch.TabIndex = 31
        Me.RadioButton_Deutsch.TabStop = True
        Me.RadioButton_Deutsch.UseVisualStyleBackColor = True
        '
        'Button_Alle_Resetten
        '
        Me.Button_Alle_Resetten.Location = New System.Drawing.Point(386, 25)
        Me.Button_Alle_Resetten.Name = "Button_Alle_Resetten"
        Me.Button_Alle_Resetten.Size = New System.Drawing.Size(87, 23)
        Me.Button_Alle_Resetten.TabIndex = 30
        Me.Button_Alle_Resetten.UseVisualStyleBackColor = True
        '
        'RichTextBox_Passwort
        '
        Me.RichTextBox_Passwort.Location = New System.Drawing.Point(14, 171)
        Me.RichTextBox_Passwort.Name = "RichTextBox_Passwort"
        Me.RichTextBox_Passwort.Size = New System.Drawing.Size(707, 58)
        Me.RichTextBox_Passwort.TabIndex = 28
        Me.RichTextBox_Passwort.Text = ""
        '
        'Label_Passwort
        '
        Me.Label_Passwort.AutoSize = True
        Me.Label_Passwort.Location = New System.Drawing.Point(15, 155)
        Me.Label_Passwort.Name = "Label_Passwort"
        Me.Label_Passwort.Size = New System.Drawing.Size(0, 13)
        Me.Label_Passwort.TabIndex = 27
        '
        'RichTextBox_Salt
        '
        Me.RichTextBox_Salt.Location = New System.Drawing.Point(15, 82)
        Me.RichTextBox_Salt.Name = "RichTextBox_Salt"
        Me.RichTextBox_Salt.Size = New System.Drawing.Size(707, 58)
        Me.RichTextBox_Salt.TabIndex = 26
        Me.RichTextBox_Salt.Text = ""
        '
        'Label_Salt
        '
        Me.Label_Salt.AutoSize = True
        Me.Label_Salt.Location = New System.Drawing.Point(15, 64)
        Me.Label_Salt.Name = "Label_Salt"
        Me.Label_Salt.Size = New System.Drawing.Size(0, 13)
        Me.Label_Salt.TabIndex = 25
        '
        'Button_Entschluesseln
        '
        Me.Button_Entschluesseln.Location = New System.Drawing.Point(293, 25)
        Me.Button_Entschluesseln.Name = "Button_Entschluesseln"
        Me.Button_Entschluesseln.Size = New System.Drawing.Size(87, 23)
        Me.Button_Entschluesseln.TabIndex = 24
        Me.Button_Entschluesseln.UseVisualStyleBackColor = True
        '
        'Button_Verschluesseln
        '
        Me.Button_Verschluesseln.Location = New System.Drawing.Point(200, 25)
        Me.Button_Verschluesseln.Name = "Button_Verschluesseln"
        Me.Button_Verschluesseln.Size = New System.Drawing.Size(87, 23)
        Me.Button_Verschluesseln.TabIndex = 23
        Me.Button_Verschluesseln.UseVisualStyleBackColor = True
        '
        'Label_Ausgabe
        '
        Me.Label_Ausgabe.AutoSize = True
        Me.Label_Ausgabe.Location = New System.Drawing.Point(12, 334)
        Me.Label_Ausgabe.Name = "Label_Ausgabe"
        Me.Label_Ausgabe.Size = New System.Drawing.Size(0, 13)
        Me.Label_Ausgabe.TabIndex = 22
        '
        'Label_Eingabe
        '
        Me.Label_Eingabe.AutoSize = True
        Me.Label_Eingabe.Location = New System.Drawing.Point(12, 248)
        Me.Label_Eingabe.Name = "Label_Eingabe"
        Me.Label_Eingabe.Size = New System.Drawing.Size(0, 13)
        Me.Label_Eingabe.TabIndex = 19
        '
        'Label_Art
        '
        Me.Label_Art.AutoSize = True
        Me.Label_Art.Location = New System.Drawing.Point(12, 9)
        Me.Label_Art.Name = "Label_Art"
        Me.Label_Art.Size = New System.Drawing.Size(0, 13)
        Me.Label_Art.TabIndex = 18
        '
        'ComboBox_Art
        '
        Me.ComboBox_Art.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox_Art.FormattingEnabled = True
        Me.ComboBox_Art.Location = New System.Drawing.Point(12, 25)
        Me.ComboBox_Art.Name = "ComboBox_Art"
        Me.ComboBox_Art.Size = New System.Drawing.Size(182, 21)
        Me.ComboBox_Art.TabIndex = 17
        '
        'OpenFileDialog_Eingabe
        '
        Me.OpenFileDialog_Eingabe.FilterIndex = 2
        Me.OpenFileDialog_Eingabe.InitialDirectory = "C:\"
        Me.OpenFileDialog_Eingabe.RestoreDirectory = True
        '
        'Button_Eingabe
        '
        Me.Button_Eingabe.Location = New System.Drawing.Point(15, 279)
        Me.Button_Eingabe.Name = "Button_Eingabe"
        Me.Button_Eingabe.Size = New System.Drawing.Size(122, 23)
        Me.Button_Eingabe.TabIndex = 34
        Me.Button_Eingabe.UseVisualStyleBackColor = True
        '
        'Label_Eingabedatei
        '
        Me.Label_Eingabedatei.AutoSize = True
        Me.Label_Eingabedatei.Location = New System.Drawing.Point(155, 288)
        Me.Label_Eingabedatei.Name = "Label_Eingabedatei"
        Me.Label_Eingabedatei.Size = New System.Drawing.Size(0, 13)
        Me.Label_Eingabedatei.TabIndex = 36
        '
        'Label_Ausgabedatei
        '
        Me.Label_Ausgabedatei.AutoSize = True
        Me.Label_Ausgabedatei.Location = New System.Drawing.Point(155, 376)
        Me.Label_Ausgabedatei.Name = "Label_Ausgabedatei"
        Me.Label_Ausgabedatei.Size = New System.Drawing.Size(0, 13)
        Me.Label_Ausgabedatei.TabIndex = 37
        '
        'SaveFileDialog_Ausgabe
        '
        Me.SaveFileDialog_Ausgabe.FilterIndex = 2
        Me.SaveFileDialog_Ausgabe.InitialDirectory = "C:\"
        '
        'Button_Ausgabe
        '
        Me.Button_Ausgabe.Location = New System.Drawing.Point(15, 371)
        Me.Button_Ausgabe.Name = "Button_Ausgabe"
        Me.Button_Ausgabe.Size = New System.Drawing.Size(122, 23)
        Me.Button_Ausgabe.TabIndex = 38
        Me.Button_Ausgabe.UseVisualStyleBackColor = True
        '
        'ProgressBar_Verschluesseln
        '
        Me.ProgressBar_Verschluesseln.Location = New System.Drawing.Point(621, 279)
        Me.ProgressBar_Verschluesseln.Maximum = 2147483647
        Me.ProgressBar_Verschluesseln.Name = "ProgressBar_Verschluesseln"
        Me.ProgressBar_Verschluesseln.Size = New System.Drawing.Size(100, 23)
        Me.ProgressBar_Verschluesseln.Step = 1
        Me.ProgressBar_Verschluesseln.TabIndex = 39
        '
        'Main
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(934, 495)
        Me.Controls.Add(Me.ProgressBar_Verschluesseln)
        Me.Controls.Add(Me.Button_Ausgabe)
        Me.Controls.Add(Me.Label_Ausgabedatei)
        Me.Controls.Add(Me.Label_Eingabedatei)
        Me.Controls.Add(Me.Button_Eingabe)
        Me.Controls.Add(Me.Label_Sprache)
        Me.Controls.Add(Me.RadioButton_Englisch)
        Me.Controls.Add(Me.RadioButton_Deutsch)
        Me.Controls.Add(Me.Button_Alle_Resetten)
        Me.Controls.Add(Me.RichTextBox_Passwort)
        Me.Controls.Add(Me.Label_Passwort)
        Me.Controls.Add(Me.RichTextBox_Salt)
        Me.Controls.Add(Me.Label_Salt)
        Me.Controls.Add(Me.Button_Entschluesseln)
        Me.Controls.Add(Me.Button_Verschluesseln)
        Me.Controls.Add(Me.Label_Ausgabe)
        Me.Controls.Add(Me.Label_Eingabe)
        Me.Controls.Add(Me.Label_Art)
        Me.Controls.Add(Me.ComboBox_Art)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "Main"
        Me.Text = "Form1"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label_Sprache As Label
    Friend WithEvents RadioButton_Englisch As RadioButton
    Friend WithEvents RadioButton_Deutsch As RadioButton
    Friend WithEvents Button_Alle_Resetten As Button
    Friend WithEvents RichTextBox_Passwort As RichTextBox
    Friend WithEvents Label_Passwort As Label
    Friend WithEvents RichTextBox_Salt As RichTextBox
    Friend WithEvents Label_Salt As Label
    Friend WithEvents Button_Entschluesseln As Button
    Friend WithEvents Button_Verschluesseln As Button
    Friend WithEvents Label_Ausgabe As Label
    Friend WithEvents Label_Eingabe As Label
    Friend WithEvents Label_Art As Label
    Friend WithEvents ComboBox_Art As ComboBox
    Friend WithEvents OpenFileDialog_Eingabe As OpenFileDialog
    Friend WithEvents Button_Eingabe As Button
    Friend WithEvents Label_Eingabedatei As Label
    Friend WithEvents Label_Ausgabedatei As Label
    Friend WithEvents SaveFileDialog_Ausgabe As SaveFileDialog
    Friend WithEvents Button_Ausgabe As Button
    Friend WithEvents ProgressBar_Verschluesseln As ProgressBar

End Class
