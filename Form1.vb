Imports System.IO
Imports System.Reflection
Imports System.Text
Imports System.Xml
Imports Microsoft.VisualBasic.FileIO

'Imports DataStreams.Csv




Public Class Form1
    Dim ds As New DataSet



    Private Sub CsvToXml(_inputFile As String, _dataName As String, _separator As Char, _outputFile As String, Optional _firstrowdata As Boolean = vbTrue, Optional _fieldnames() As String = Nothing, Optional encodingtext As String = "utf-8")
        Dim dt As New DataTable(_dataName)

        Dim utf8 As Encoding = Encoding.GetEncoding(encodingtext)
        Dim parser As TextFieldParser = New TextFieldParser(_inputFile, utf8, True)
        parser.Delimiters = New String() {_separator}
        parser.HasFieldsEnclosedInQuotes = True
        parser.TrimWhiteSpace = True

        Dim firstRow As Boolean = True
        Dim secrow As UInteger = 0

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
            If Not (secrow = 0 And _firstrowdata = vbTrue) Then dt.Rows.Add(fields)
            secrow += 1
        End While
        parser.Close()

        dt.WriteXml(_outputFile)
        dt.Dispose()
        'dt.TableName = "Bolt"


        'Load XSLT from resources
        Dim xsltStream As Stream
        xsltStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("BOLT2OPTIMA.XSLTFile1.xslt")
        Dim input = New XmlDocument()
        Dim output = New StringWriter()


        ' Load the String into a TextReader
        ' System.IO.TextReader
        Dim tr As System.IO.TextReader = New System.IO.StreamReader(xsltStream, System.Text.Encoding.UTF8, True)
        ' Use that TextReader as the Source for the XmlTextReader
        ' 
        Dim xr As System.Xml.XmlReader = New System.Xml.XmlTextReader(tr)
        ' Create a New XslTransform class
        Dim trans As System.Xml.Xsl.XslTransform = New System.Xml.Xsl.XslTransform()
        ' Load the XmlReader StyleSheet into the XslTransform class
        trans.Load(xr)
        'transformer.Load(xsltStream)

        input.Load(_outputFile)
        ''# create navigator
        Dim navigator = input.CreateNavigator

        trans.Transform(navigator, Nothing, output)

        '     Dim transform = New XmlDocument()
        '    transformer.Load(transform, vbNull, New embeddedResourcexsltresolver()))
        ''# transform XML data
        '   transformer.Transform(input, Nothing, output)

        Console.WriteLine(output.ToString)
        ''# write transformation result to disk
        'Dim stream As FileStream = New FileStream(_outputFile + "-Optima.xml", FileMode.Create)


        Dim writer = New StreamWriter(_outputFile + "-Optima.xml", True, System.Text.Encoding.UTF8)

        writer.Write(output)

        ''# close streams
        writer.Close()
        output.Close()



        'DataGridView1.DataSource.Tables 
        '              ds.Tables.Add(dt)
        '       dt.Dispose()
        'End Using
    End Sub

    Private Sub OpenCSV_Click(sender As Object, e As EventArgs) Handles OpenCSV.Click
        Dim ofd As OpenFileDialog = New OpenFileDialog
        Dim filepath As String
        ofd.DefaultExt = "csv"
        ofd.FileName = "Bolt Faktury za przejazd"
        ofd.InitialDirectory = "%USERPROFILE%"
        ofd.Filter = "Pliki Bolt-CSV|*.csv|All files|*.*"
        ofd.Title = "Wskaż plik csv do dostawcy BOLT"
        If ofd.ShowDialog() = DialogResult.Cancel Then Exit Sub

        filepath = ofd.FileName
        CsvToXml(filepath, "Bolt", ",", filepath + ".xml")

        dim a =MsgBox("OK Zrobione",vbOKOnly)

    End Sub


End Class
