Imports System.IO
Imports System.Reflection
Imports System.Text
Imports System.Xml
Imports Microsoft.VisualBasic.FileIO
Imports Ionic
Public Class ChangelogInfo
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles btn_OK.Click
        Me.Hide()
        Me.Close()
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        Upload_infobox()
    End Sub

    Private Sub ChangelogInfo_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            ComboBox1.Items.Clear()
            ComboBox1.Text = ""
            For i As Integer = 0 To Bolt2Optima.fname.Length - 1
                ComboBox1.Items.Add(Bolt2Optima.fname(i))
                If ComboBox1.Text = "" Then ComboBox1.Text = Bolt2Optima.fname(i)
            Next i
        Catch
            Bolt2Optima.result = MsgBox("Error filenames chl/read")
        End Try
        Upload_infobox()
    End Sub

    Private Sub Upload_infobox()
        Try
            If Bolt2Optima.Debugflag Then
                Dim resource_data As String() = Assembly.GetExecutingAssembly().GetManifestResourceNames()
                Bolt2Optima.result = MsgBox(resource_data.ToString)
            End If
            If Bolt2Optima.Debugflag Then Bolt2Optima.result = MsgBox(ComboBox1.SelectedIndex)
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error Getting Text f contents...")
        End Try
        Try
            Dim reader As StreamReader = New StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream(Me.GetType().Namespace + "." + ComboBox1.SelectedItem().ToString))
            InfoBox_details.Text = reader.ReadToEnd()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error Getting Text f contents...")
        End Try
        InfoBox_details.Update()
    End Sub

End Class