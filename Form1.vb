Imports System.IO
Imports System.Text
Imports System.Xml

Imports DataStreams.Csv




Public Class Form1
    Dim ds As New DataSet

    Private Sub CsvToXml(_inputFile As String, _dataName As String, _separator As Char, _outputFile As String, Optional _firstrowdata As Boolean = vbTrue, Optional _fieldnames() As String = Nothing)
        Dim dt As New DataTable(_dataName)

        Dim firstRow As Boolean = True

        Using sr As New StreamReader(_inputFile)
            While Not (sr.EndOfStream)
                Dim fields() As String = sr.ReadLine.Split(_separator)

                If firstRow Then
                    For ii As Integer = 0 To fields.Count - 1
                        Dim _fName As String = ""
                        If (IsNothing(_fieldnames) And _firstrowdata = vbFalse) Then
                            _fName = "Field" & ii.ToString("000")
                        Else
                            If (_firstrowdata = vbFalse) Then
                                _fName = _fieldnames(ii)
                            Else
                                _fName = Replace(Replace(Replace(Replace(fields(ii), Chr(34), ""), " ", "_"), "(", ""), ")", "")
                            End If
                        End If
                        dt.Columns.Add(_fName)
                    Next
                    firstRow = False
                End If

                dt.Rows.Add(fields)
            End While

            dt.WriteXml(_outputFile)
            dt.TableName = "Bolt"
            DataGridView1.DataSource.Tables = ds.Tables
            ds.Tables.Add(dt)
            dt.Dispose()
        End Using
    End Sub

    Private Sub OpenCSV_Click(sender As Object, e As EventArgs) Handles OpenCSV.Click
        Dim ofd As OpenFileDialog = New OpenFileDialog
        Dim filepath As String
        ofd.DefaultExt = "csv"
        ofd.FileName = "Bolt"
        ofd.InitialDirectory = "%USERPROFILE%"
        ofd.Filter = "Pliki Bolt-CSV|*.csv|All files|*.*"
        ofd.Title = "Wskaż plik csv do dostawcy BOLT"
        If ofd.ShowDialog() = DialogResult.Cancel Then Exit Sub

        filepath = ofd.FileName
        CsvToXml(filepath, "Bolt", ",", filepath + ".xml")


    End Sub


End Class
