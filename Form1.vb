'Imports System
Imports System.IO
Imports System.Reflection
Imports System.Text
Imports System.Xml
Imports Microsoft.VisualBasic.FileIO
Imports Ionic
'Imports System.IO.Compression
'Imports System.Net.Mail

Public Class Bolt2Optima
    'Dim ds As New DataSet
    Dim Debugflag As Boolean = vbFalse
    Dim starttime As Date = Now()
    Dim filepath As String = ""

    Private Sub CsvToXml(_inputFile As String, _dataName As String, _separator As Char, _outputFile As String, xsltfileinresource As String, Optional _firstrowdata As Boolean = vbTrue, Optional _fieldnames() As String = Nothing, Optional encodingtext As String = "utf-8")
        starttime = Now()
        Dim dt As New DataTable(_dataName)

        Dim utf8 As Encoding = Encoding.GetEncoding(encodingtext)
        Dim parser As TextFieldParser = New TextFieldParser(_inputFile, utf8, True)
        parser.Delimiters = New String() {_separator}
        parser.HasFieldsEnclosedInQuotes = True
        parser.TrimWhiteSpace = True

        Dim firstRow As Boolean = True
        Dim secrow As UInteger = 0
        Dim sumanet As Double = 0
        Dim sumavat As Double = 0
        Dim sumabrut As Double = 0

        'Using sr As New StreamReader(_inputFile, True)

        While Not parser.EndOfData
            Dim fields As String() = parser.ReadFields()
            'Not parser.EndOfData

            Application.DoEvents()

            '      Dim fields() As String = sr.ReadLine.Split(_separator)

            If firstRow Then
                For ii As Integer = 0 To fields.Count - 1
                    Dim _fName As String = ""
                    If (IsNothing(_fieldnames) And _firstrowdata = vbFalse) Then
                        _fName = "Field" & ii.ToString("000")
                    Else
                        If (_firstrowdata = vbFalse) Then
                            _fName = _fieldnames(ii)
                        Else
                            _fName = Replace(Replace(Replace(Replace(Replace(fields(ii), Chr(34), ""), " ", "_"), "(", ""), ")", ""), ",", "")
                        End If
                    End If
                    dt.Columns.Add(_fName)
                Next
                firstRow = False
            End If
            sumanet += Val(fields(13))
            sumavat += Val(fields(14))
            sumabrut += Val(fields(15))
            If Not (secrow = 0 And _firstrowdata = vbTrue) Then dt.Rows.Add(fields)
            If (secrow = 2) Then
                statusbox.AppendText("Dane Kierowcy: " + fields(9) + " " + fields(10) + vbCrLf)
                statusbox.AppendText("NIP: " + fields(11) + " REGON: " + fields(12) + vbCrLf)
            End If
            secrow += 1
        End While
        parser.Close()
        statusbox.AppendText("Kolumn: " + dt.Columns.Count.ToString + vbCrLf)
        statusbox.AppendText("Wierszy: " + dt.Rows.Count.ToString + vbCrLf)
        statusbox.AppendText("Sumy: " + "Netto: " + sumanet.ToString("0.00") + "   VAT: " + sumavat.ToString("0.00") + "   Brutto: " + sumabrut.ToString("0.00") + vbCrLf)
        If (Debugflag = True) Then dt.WriteXml(_outputFile)

        'Load XSLT from resources
        Dim xsltStream As System.IO.Stream = New System.IO.MemoryStream()
        xsltStream.Position = 0
        Try
            xsltStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(xsltfileinresource)
            ' Use an XmlReader And pass in a Stream as the Source
            Dim xsltXR As System.Xml.XmlReader = New System.Xml.XmlTextReader(xsltStream)
            ' Create the XslTransform.
            Dim xslt As System.Xml.Xsl.XslCompiledTransform = New System.Xml.Xsl.XslCompiledTransform()
            ' Load the stylesheet.
            xslt.Load(xsltXR)
            'Parse parameters to xslt
            Dim xsltparam As New Xsl.XsltArgumentList()
            xsltparam.AddParam("pOptZrd", "", IDOptksieg.Text)
            xsltparam.AddParam("pOptDoc", "", IDSender.Text)
            xsltparam.AddParam("pFixedKod", "", Mid(defaultPostCode.Text, 1, 2) + "-" + Mid(defaultPostCode.Text, 4, 3))
            xsltparam.AddParam("pFixedMiasto", "", defaultCity.Text)
            xsltparam.AddParam("pNIPclean", "", cb_usun_minus_nip.Checked.ToString.ToUpper)
            If (Debugflag = vbTrue) Then Dim a = MsgBox(cb_usun_minus_nip.Checked.ToString.ToUpper + vbCrLf + defaultCity.Text)
            Dim myxml As System.IO.Stream = New System.IO.MemoryStream()
            myxml.Position = 0
            dt.WriteXml(myxml)
            myxml.Position = 0
            Dim docxmlr As System.Xml.XmlReader = New System.Xml.XmlTextReader(myxml)
            Dim writer As System.Xml.XmlTextWriter = New System.Xml.XmlTextWriter(_outputFile + "-OPTIMA.xml", System.Text.Encoding.UTF8)
            xslt.Transform(docxmlr, xsltparam, writer)
            writer.Flush()
            writer.Close()
            docxmlr.Close()
        Catch
            Dim a = MessageBox.Show("Bu...Bu... Error accessing resources in Style!")
            Environment.Exit(0)
        End Try

        '      xsltStream = Properties.Resources.ResourceManager.GetObject(xsltfileinresource)

    End Sub

    Private Sub OpenCSV_Click(sender As Object, e As EventArgs) Handles OpenCSV.Click
        OpenFileDialogAll()
    End Sub

    Private Sub exitok()
        statusbox.AppendText("Upłynęło Czasu: " + (Now().Subtract(starttime).TotalSeconds.ToString("0.0000")).ToString + "s" + vbCrLf)
        Dim a = MsgBox("OK Zrobione" + vbCrLf + "Upłynęło: " + (Now().Subtract(starttime).TotalSeconds.ToString("0.0000")).ToString + "s", vbOKOnly)
        Dim axs() = Split(filepath, "\")
        Dim pathpa As String = ""
        Dim fname As String = ""
        For x = 0 To UBound(axs)
            If x <> UBound(axs) Then If pathpa = "" Then pathpa = axs(x) + "\" Else pathpa += axs(x) + "\"
            If x = UBound(axs) Then fname = axs(x)
        Next x
        Process.Start("Explorer.exe", ControlChars.Quote & IO.Path.Combine(pathpa) & ControlChars.Quote) 'Open Explorer WIndow
        Dim filenames As String() = {IO.Path.Combine(pathpa) + fname, IO.Path.Combine(pathpa) + fname + ".xml-OPTIMA.xml"}
        Using zip As New Zip.ZipFile()
            zip.AddFiles(filenames, "/")
            zip.Save(pathpa & fname & ".zip")

        End Using
    End Sub

    Private Sub OpenFileDialogAll()
        Dim ofd As OpenFileDialog = New OpenFileDialog
        ofd.DefaultExt = "csv"
        ofd.FileName = "Bolt Faktury za przejazd"
        ofd.InitialDirectory = "%USERPROFILE%"
        ofd.Filter = "Pliki Bolt-CSV|*.csv|All files|*.*"
        ofd.Title = "Wskaż plik csv do dostawcy BOLT"
        If ofd.ShowDialog() = DialogResult.Cancel Then Exit Sub

        filepath = ofd.FileName
        statusbox.Clear()
        CsvToXml(filepath, "Bolt", ",", filepath + ".xml", "Bolt2Optim.Resources.XSLTFile1.xslt")
        exitok()
    End Sub

    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click
        Process.Start("https://www.marm.pl")
    End Sub

    Private Sub Bolt2Optima_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.AllowDrop = True
        Me.Text += " v." + Application.ProductVersion + " (c) MARM.pl Sp. z o.o."
    End Sub

    Private Sub Bolt2Optima_DragDrop(sender As Object, e As DragEventArgs) Handles Me.DragDrop
        Dim files() As String = e.Data.GetData(DataFormats.FileDrop)
        For Each path In files
            If (Debugflag = vbTrue) Then MsgBox(path)
            statusbox.Clear()
            filepath = path
            CsvToXml(path, "Bolt", ",", path + ".xml", "Bolt2Optim.Resources.XSLTFile1.xslt")
        Next
        exitok()
    End Sub

    Private Sub Bolt2Optima_DragEnter(sender As Object, e As DragEventArgs) Handles Me.DragEnter
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            e.Effect = DragDropEffects.Copy
        End If
    End Sub

    Private Sub statusbox_Click(sender As Object, e As EventArgs) Handles statusbox.Click
        OpenFileDialogAll()
    End Sub

    Private Sub Bolt2Optima_Click(sender As Object, e As EventArgs) Handles Me.Click
        OpenFileDialogAll()
    End Sub
End Class
