﻿'------------------------------------------------------------------------------
' <auto-generated>
'     Dieser Code wurde von einem Tool generiert.
'     Laufzeitversion:4.0.30319.42000
'
'     Änderungen an dieser Datei können falsches Verhalten verursachen und gehen verloren, wenn
'     der Code erneut generiert wird.
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict On
Option Explicit On


Namespace My
    
    'HINWEIS: Diese Datei wird automatisch generiert und darf nicht direkt bearbeitet werden. Wenn Sie Änderungen vornehmen möchten,
    ' oder bei in dieser Datei auftretenden Buildfehlern wechseln Sie zum Projekt-Designer.
    ' (Wechseln Sie dazu zu den Projekteigenschaften, oder doppelklicken Sie auf den Knoten "Mein Projekt" im
    ' Projektmappen-Explorer). Nehmen Sie auf der Registerkarte "Anwendung" entsprechende Änderungen vor.
    '
    Partial Friend Class MyApplication
        
        <Global.System.Diagnostics.DebuggerStepThroughAttribute()>  _
        Public Sub New()
            MyBase.New(Global.Microsoft.VisualBasic.ApplicationServices.AuthenticationMode.Windows)
            Me.IsSingleInstance = false
            Me.EnableVisualStyles = true
            Me.SaveMySettingsOnExit = true
            Me.ShutDownStyle = Global.Microsoft.VisualBasic.ApplicationServices.ShutdownMode.AfterMainFormCloses
        End Sub
        
        <Global.System.Diagnostics.DebuggerStepThroughAttribute()>  _
        Protected Overrides Sub OnCreateMainForm()
            Me.MainForm = Global.AES_FileCryptor.Main
        End Sub
    End Class
End Namespace
