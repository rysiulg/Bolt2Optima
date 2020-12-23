<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
>
    <xsl:output indent="yes" method="xml" omit-xml-declaration="no" version="1.0" encoding="UTF-8" />
    <xsl:strip-space elements="*" />
    <xsl:param name="pOptZrd"/>
    <xsl:param name="pOptDoc"/>
    <xsl:param name="pFixedKod"/>
    <xsl:param name="pFixedMiasto"/>
    <xsl:param name="pNIPclean"/>
    <xsl:param name="pKategoria"/>
    <xsl:param name="puslugi"/>
    <xsl:param name="pWieleKierowcow"/>
  
    <xsl:param name="pkategoriaOPIS"></xsl:param>

    <xsl:variable name="OptimaNazwalimit">50</xsl:variable>
    <xsl:variable name="numb" select="'0123456789'"/>
    <xsl:variable name="nipn" select="'0123456789-'"/>
    <xsl:variable name="ucase" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZACELNOSZZACELNOSZZ_ILGSCJDBBGNOWMKAYOPEIKAEKTUGSASAKOZAEWALILIJZEMLJNANASTACIJOWALCZUK_'"/>
    <xsl:variable name="lcase" select="'abcdefghijklmnopqrstuvwxyząćęłńóśżźĄĆĘŁŃÓŚŻŹ илГшчйДбБгновМкауOреікАектугСашаКозаеваЛилияЗемлянанастасияовальчук.'"/> 
  <!--  Микола_Глушко Даша_ГречишніковаАйбек_Бектурганов -->

    <xsl:template match="DocumentElement">
        <xsl:text disable-output-escaping="yes">&lt;ROOT xmlns=&quot;http://www.comarch.pl/cdn/optima/offline&quot;&gt;</xsl:text> 
        <xsl:element name="DEFINICJE_DOKUMENTOW">
            <xsl:element name="WERSJA"><xsl:text>2.00</xsl:text></xsl:element>
            <xsl:element name="BAZA_ZRD_ID"><xsl:value-of select="$pOptZrd"/></xsl:element>
            <xsl:element name="BAZA_DOC_ID"><xsl:value-of select="$pOptDoc"/></xsl:element>
        </xsl:element>
        <xsl:if test="$pKategoria!=''">
              <xsl:element name="KATEGORIE">
                  <xsl:element name="WERSJA"><xsl:text>2.00</xsl:text></xsl:element>
                  <xsl:element name="BAZA_ZRD_ID"><xsl:value-of select="$pOptZrd"/></xsl:element>
                  <xsl:element name="BAZA_DOC_ID"><xsl:value-of select="$pOptDoc"/></xsl:element>
                  <xsl:element name="KATEGORIA">
                      <xsl:element name="KOD_OGOLNY">
                          <xsl:value-of select="translate($pKategoria,$lcase,$ucase)"/>
                      </xsl:element>
                      <xsl:element name="KOD">
                          <xsl:value-of select="translate($pKategoria,$lcase,$ucase)"/>
                      </xsl:element>
                      <xsl:element name="TYP">
                          <xsl:value-of select="'przychód'"/>
                      </xsl:element>
                      <xsl:element name="OPIS">
                          <xsl:value-of select="$pkategoriaOPIS"/>
                      </xsl:element>
                      <xsl:element name="NIEAKTYWNA">Nie</xsl:element>
                    
                  </xsl:element>
              </xsl:element>
        </xsl:if>
        <xsl:element name="KONTRAHENCI">
        <xsl:call-template name="ParseDataKontah"/>
        </xsl:element>
        <xsl:element name="REJESTRY_SPRZEDAZY_VAT">
        <xsl:call-template name="ParseDataSprzedazy"/>
        </xsl:element>
        <xsl:text disable-output-escaping="yes">&lt;/ROOT&gt;</xsl:text>
    </xsl:template>
    
    <xsl:template name="ParseDataSprzedazy">
        <xsl:element name="WERSJA">
            <xsl:text>2.00</xsl:text>
        </xsl:element>
        <xsl:element name="BAZA_ZRD_ID">
            <xsl:value-of select="$pOptZrd"/>
        </xsl:element>
        <xsl:element name="BAZA_DOC_ID">
            <xsl:value-of select="$pOptDoc"/>
        </xsl:element>
        <xsl:for-each select="Bolt/Nazwa_Firmy_Kierowca">
            <xsl:variable name="metplat"><xsl:value-of select="translate(substring(normalize-space(ancestor-or-self::node()/Metoda_płatności),1,3),$ucase,$lcase)"/></xsl:variable>
            <xsl:variable name="nipodb"><xsl:value-of select="normalize-space(ancestor-or-self::node()/NIP_odbiorcy)"/></xsl:variable>
            <xsl:variable name="netto"><xsl:value-of select="normalize-space(ancestor-or-self::node()/Cena_bez_VAT)"/></xsl:variable>
            <xsl:variable name="brutto"><xsl:value-of select="normalize-space(ancestor-or-self::node()/Suma)"/></xsl:variable>
            <xsl:variable name="vat"><xsl:value-of select="normalize-space(ancestor-or-self::node()/VAT)"/></xsl:variable>
            <xsl:variable name="data"><xsl:value-of select="concat(substring(normalize-space(ancestor-or-self::node()/Data),7,4),'-',substring(normalize-space(ancestor-or-self::node()/Data),4,2),'-',substring(normalize-space(ancestor-or-self::node()/Data),1,2))"/></xsl:variable>
            <xsl:variable name="data_przejazdu"><xsl:value-of select="concat(substring(normalize-space(ancestor-or-self::node()/Data_przejazdu),7,4),'-',substring(normalize-space(ancestor-or-self::node()/Data_przejazdu),4,2),'-',substring(normalize-space(ancestor-or-self::node()/Data_przejazdu),1,2))"/></xsl:variable>
            <xsl:element name="REJESTR_SPRZEDAZY_VAT">
                <!--<xsl:element name="ID_ZRODLA">A0D739EE-2807-CD5B-533C-C61A74CB22C0</xsl:element>  -->
                <xsl:element name="MODUL">Handel</xsl:element>
                <xsl:element name="TYP">Rejestr sprzedazy</xsl:element>
                <xsl:element name="REJESTR">SPRZEDAŻ</xsl:element>
                <xsl:element name="DATA_WYSTAWIENIA">
                    <xsl:value-of select="$data"/>
                </xsl:element>
                <xsl:element name="DATA_SPRZEDAZY">
                    <xsl:value-of select="$data"/>
                </xsl:element>
                <xsl:element name="TERMIN">
                    <!--RRRR-MM-DD Termin płatności.-->
                    <xsl:value-of select="$data"/>
                </xsl:element>
                <xsl:element name="NUMER">
                    <!--STRING(30) Numer faktury. Węzeł wymagany.-->
                    <xsl:value-of select="normalize-space(ancestor-or-self::node()/Numer_faktury)"/>
                </xsl:element>

                <xsl:element name="WEWNETRZNA">
                    <!--Czy faktura jest fakturą wewnętrzną sprzedaŜy.Przyjmuje wartości Tak lub Nie.-->
                    <xsl:text>Nie</xsl:text>
                </xsl:element>
                <xsl:element name="DETALICZNA">
                    <!--Czy jest to sprzedaŜ detaliczna (nie fakturowana).Przyjmuje wartości Tak lub Nie.-->
                    <xsl:if test="$nipodb != ''">Nie</xsl:if>
                    <xsl:if test="$nipodb = ''">Tak</xsl:if>
                </xsl:element>
                <xsl:element name="FINALNY">
                    <!--Czy jest to sprzedaŜ dla odbiorcy finalnego (niebędącego podmiotem gospodarczym). Przyjmuje wartości Tak lub Nie.-->
                    <xsl:if test="$nipodb != ''">Nie</xsl:if>
                    <xsl:if test="$nipodb = ''">Tak</xsl:if>
                </xsl:element>
                <!--<xsl:element name="IDENTYFIKATOR_KSIEGOWY">FZ111</xsl:element>-->
                <xsl:element name="TYP_PODMIOTU">kontrahent</xsl:element>
                <!--!!!!
dane podmiotu-->
                <xsl:element name="STATUS">aktualny</xsl:element>
                <xsl:element name="PODMIOT">
                    <!--STRING(20) Akronim podmiotu (np. kontrahenta). Po tym polu będzie rozpoznawany kontrahent w bazie danych CDNOPT!MA.-->
                    <xsl:value-of select="substring(translate(normalize-space(ancestor-or-self::node()/Odbiorca), $lcase, $ucase), 1, 20)"/>
                </xsl:element>
                <xsl:element name="NAZWA1">
                    <!--STRING(50) Pierwsza linia nazwy podmiotu (kontrahenta).-->
                    <xsl:if test="string-length(normalize-space(ancestor-or-self::node()/Odbiorca)) &lt; $OptimaNazwalimit">
                        <xsl:value-of select="normalize-space(ancestor-or-self::node()/Odbiorca)"/>
                    </xsl:if>
                    <xsl:if test="string-length(normalize-space(ancestor-or-self::node()/Odbiorca)) &gt; $OptimaNazwalimit">
                        <xsl:value-of select="substring(normalize-space(ancestor-or-self::node()/Odbiorca), 1, $OptimaNazwalimit)"/>
                    </xsl:if>
                </xsl:element>
                <xsl:element name="NAZWA2">
                    <xsl:if test="string-length(normalize-space(ancestor-or-self::node()/Odbiorca)) &gt; $OptimaNazwalimit">
                        <xsl:value-of select="substring(normalize-space(ancestor-or-self::node()/Odbiorca), $OptimaNazwalimit + 1, $OptimaNazwalimit)" />
                    </xsl:if>
                </xsl:element>
                <xsl:element name="KRAJ">Polska</xsl:element>
                <xsl:element name="ULICA"> </xsl:element>
                <xsl:element name="MIASTO">
                    <xsl:value-of select="$pFixedMiasto"/>
                </xsl:element>
                <xsl:element name="KOD_POCZTOWY">
                    <xsl:value-of select="$pFixedKod"/>
                </xsl:element>
                <xsl:element name="NIP">
                    <xsl:if test="$pNIPclean != 'TRUE'">
                        <xsl:value-of select="$nipodb"/>
                    </xsl:if>
                    <xsl:if test="$pNIPclean = 'TRUE'">
                        <xsl:value-of select="translate($nipodb, $nipn, $numb)"/>
                    </xsl:if>
                </xsl:element>
                <xsl:element name="REGON">
                    <xsl:value-of select="normalize-space(ancestor-or-self::node()/Numer_REGON)"/>
                </xsl:element>

              <xsl:if test="$pKategoria!=''">
                <xsl:element name="KATEGORIA">
                  <!--STRING(20) Kod kategorii dokumentu. Po tym polu będzie rozpoznawana kategoria w bazie danych CDN OPT!MA.-->
                  <xsl:value-of select="substring(translate($pKategoria,$lcase,$ucase),1,20)"/>
                </xsl:element>
              </xsl:if>

                <xsl:element name="OPIS">
                    <!--STRING(50) Opis dokumentu.-->
                    <xsl:value-of select="concat('Data_przejazdu: ', normalize-space(ancestor-or-self::node()/Data_przejazdu), ' ')"/>
                </xsl:element>
                <xsl:element name="DODATKOWE">
                    <!--STRING(40) Dodatkowe dane adresowe dokumentu.-->
                    <xsl:value-of select="concat('ź/d:', normalize-space(ancestor-or-self::node()/Adres_odbioru))"/>
                    <xsl:value-of select="concat(' / ', normalize-space(ancestor-or-self::node()/Adres_odbiorcy))"/>
                </xsl:element>
                
                <xsl:element name="FORMA_PLATNOSCI">
                    <xsl:if test="$metplat != 'got'">
                        <xsl:text>przelew</xsl:text>
                    </xsl:if>
                    <xsl:if test="$metplat = 'got'">
                        <xsl:text>gotówka</xsl:text>
                    </xsl:if>
                </xsl:element>
                <xsl:element name="NOTOWANIE_WALUTY_ILE">1</xsl:element>
                <xsl:element name="NOTOWANIE_WALUTY_ZA_ILE">1</xsl:element>
                <xsl:element name="WARTOSC_SPRZEDAZY">
                    <xsl:value-of select="format-number($brutto, '#0.00')"/>
                </xsl:element>

                <xsl:element name="POZYCJE">
                    <xsl:element name="POZYCJA">
                        <xsl:element name="LP">1</xsl:element>
                        <xsl:element name="STAWKA_VAT">
                          <xsl:variable name="stvat1" select="(($brutto div $netto) - 1) * 100"/>  <!-- //format-number(((($brutto div $netto) - 1) * 100) , '#0.0')"/> -->
                          <xsl:choose>
                            <xsl:when test="$stvat1 &lt; 5" >
                              <xsl:value-of select="'0.0'"/>
                            </xsl:when>
                            <xsl:when test="$stvat1 > 4.9 and $stvat1 &lt; 7.4">
                              <xsl:value-of select="'5'"/>
                            </xsl:when>
                            <xsl:when test="$stvat1 > 7.4 and $stvat1 &lt; 7.9">
                              <xsl:value-of select="'7.5'"/>
                            </xsl:when>
                            <xsl:when test="$stvat1 > 7.9 and $stvat1 &lt; 21.9">
                              <xsl:value-of select="'8.0'"/>
                            </xsl:when>
                            <xsl:when test="$stvat1 > 21.9 and $stvat1 &lt; 22.9">
                              <xsl:value-of select="'22.0'"/>
                            </xsl:when>
                            <xsl:when test="$stvat1 > 22.9 and $stvat1 &lt; 50">
                              <xsl:value-of select="'23.0'"/>
                            </xsl:when>
                            <xsl:otherwise >
                              <xsl:value-of select="'0.0'"/>
                            </xsl:otherwise>
                          </xsl:choose>
                          
                          
                          <!--<xsl:value-of                                select="format-number(((($brutto div $netto) - 1) * 100) , '#0.0')"/> -->
                        </xsl:element>
                        <xsl:if test="translate($pKategoria,$lcase,$ucase)!='' or $pWieleKierowcow = 'TRUE'">
                            <xsl:element name="KATEGORIA_POS">
                              <xsl:if test="translate($pKategoria,$lcase,$ucase)!=''">
                                <xsl:value-of select="translate($pKategoria,$lcase,$ucase)"/>
                              </xsl:if>
                              <xsl:if test="translate($pKategoria,$lcase,$ucase)!='' and $pWieleKierowcow = 'TRUE'">
                                <xsl:value-of select="'_'"/>
                              </xsl:if>
                                <xsl:if test="$pWieleKierowcow = 'TRUE'">
                                  <xsl:value-of select="translate(normalize-space(ancestor-or-self::node()/Kierowca),$lcase,$ucase)"/>
                                  <xsl:value-of select="concat('_',translate(normalize-space(ancestor-or-self::node()/NIP_Firmy), $nipn, $numb))"/>
                                </xsl:if>
                            </xsl:element>
                        </xsl:if>
                        <xsl:element name="STATUS_VAT">
                            <xsl:if test="$vat > 0">
                                <xsl:text>opodatkowana</xsl:text>
                            </xsl:if>
                            <xsl:if test="$vat = 0">
                                <xsl:text>nieopodatkowana</xsl:text>
                            </xsl:if>
                        </xsl:element>
                        <xsl:element name="NETTO">
                            <xsl:value-of select="format-number($netto, '#0.00')"/>
                        </xsl:element>
                        <xsl:element name="VAT">
                            <xsl:value-of select="format-number($vat, '#0.00')"/>
                        </xsl:element>
                        <xsl:element name="NETTO_SYS">
                            <xsl:value-of select="format-number($netto, '#0.00')"/>
                        </xsl:element>
                        <xsl:element name="VAT_SYS">
                            <xsl:value-of select="format-number($vat, '#0.00')"/>
                        </xsl:element>
                        <xsl:element name="RODZAJ_SPRZEDAZY"><xsl:value-of select="$puslugi"/></xsl:element>
                    </xsl:element>
                </xsl:element>
                <xsl:element name="PLATNOSCI">
                    <xsl:element name="PLATNOSC">
                        <xsl:element name="TERMIN_PLAT">
                            <xsl:value-of select="concat(substring(normalize-space(ancestor-or-self::node()/Data), 7, 4), '-', substring(normalize-space(ancestor-or-self::node()/Data), 4, 2), '-', substring(normalize-space(ancestor-or-self::node()/Data), 1, 2))"/>
                        </xsl:element>
                        <xsl:element name="FORMA_PLATNOSCI_PLAT">
                            <xsl:if test="$metplat != 'got'">
                                <xsl:text>przelew</xsl:text>
                            </xsl:if>
                            <xsl:if test="$metplat = 'got'">
                                <xsl:text>gotówka</xsl:text>
                            </xsl:if>
                        </xsl:element>
                        <xsl:element name="KWOTA_PLN_PLAT">
                            <xsl:value-of select="$brutto"/>
                        </xsl:element>
                        <xsl:element name="KWOTA_PLAT">
                            <xsl:value-of select="format-number($brutto, '#0.00')"/>
                        </xsl:element>
                        <xsl:element name="KIERUNEK">
                            <xsl:if test="$netto &gt; 0">
                                <xsl:text>przychód</xsl:text>
                            </xsl:if>
                            <xsl:if test="$netto &lt; 0">
                                <xsl:text>rozchód</xsl:text>
                            </xsl:if>
                        </xsl:element>
                        <xsl:element name="PLATNOSC_TYP_PODMIOTU">kontrahent</xsl:element>
                        <xsl:element name="PODLEGA_ROZLICZENIU">tak</xsl:element>
                        <xsl:element name="WALUTA_DOK">PLN</xsl:element>
                    </xsl:element>
                </xsl:element>
            </xsl:element>
        </xsl:for-each>
    </xsl:template>
    
    <xsl:template name="ParseDataKontah" >
        <xsl:element name="WERSJA">
            <xsl:text>2.00</xsl:text>
        </xsl:element>
        <xsl:element name="BAZA_ZRD_ID">
            <xsl:value-of select="$pOptZrd"/>
        </xsl:element>
        <xsl:element name="BAZA_DOC_ID">
            <xsl:value-of select="$pOptDoc"/>
        </xsl:element>
        <xsl:for-each select="Bolt/Nazwa_Firmy_Kierowca">
            <xsl:variable name="nipodb"><xsl:value-of select="normalize-space(ancestor-or-self::node()/NIP_odbiorcy)"/></xsl:variable>
            <xsl:element name="KONTRAHENT">
                <!--    <xsl:element name="ID_ZRODLA">A0D739EE-2807-CD5B-533C-C61A74CB22C0</xsl:element> -->
                <xsl:element name="AKRONIM"><xsl:value-of select="substring(translate(normalize-space(ancestor-or-self::node()/Odbiorca),$lcase,$ucase),1,20)"/></xsl:element>
                <xsl:element name="RODZAJ">dostawca odbiorca</xsl:element>
                <xsl:element name="FINALNY">
                    <xsl:if test="$nipodb != ''">Nie</xsl:if>
                    <xsl:if test="$nipodb = ''">Tak</xsl:if>
                </xsl:element>
                <xsl:element name="NIEAKTYWNY">Nie</xsl:element>
                <xsl:element name="NIE_PODLEGA_ROZLICZENIU">Nie</xsl:element>
                <xsl:element name="PLATNIK_VAT">
                    <xsl:if test="$nipodb != ''">Tak</xsl:if>
                    <xsl:if test="$nipodb = ''">Nie</xsl:if>
                </xsl:element>
                <xsl:element name="ALGORYTM">brutto</xsl:element>
                <xsl:element name="NIEUWZGLVATZD">Nie</xsl:element>
                <xsl:element name="KRAJ_ISO">PL</xsl:element>
                <xsl:element name="ADRESY">
                    <xsl:element name="ADRES">
                        <xsl:element name="STATUS">aktualny</xsl:element>
                        <xsl:element name="NAZWA1">
                            <xsl:if test="string-length(normalize-space(ancestor-or-self::node()/Odbiorca))&lt;$OptimaNazwalimit"><xsl:value-of select="normalize-space(ancestor-or-self::node()/Odbiorca)"/></xsl:if>
                            <xsl:if test="string-length(normalize-space(ancestor-or-self::node()/Odbiorca))&gt;$OptimaNazwalimit"><xsl:value-of select="substring(normalize-space(ancestor-or-self::node()/Odbiorca),1,$OptimaNazwalimit)"/></xsl:if>
                        </xsl:element>
                        <xsl:element name="NAZWA2">
                            <xsl:if test="string-length(normalize-space(ancestor-or-self::node()/Odbiorca))&gt;$OptimaNazwalimit"><xsl:value-of select="substring(normalize-space(ancestor-or-self::node()/Odbiorca),$OptimaNazwalimit+1,$OptimaNazwalimit)"/></xsl:if>
                        </xsl:element>
                            <xsl:element name="KRAJ">Polska</xsl:element>
                        <xsl:element name="ULICA">
                        </xsl:element>
                        <xsl:element name="MIASTO"><xsl:value-of select="$pFixedMiasto"/></xsl:element>
                        <xsl:element name="KOD_POCZTOWY"><xsl:value-of select="$pFixedKod"/></xsl:element>
                        <xsl:element name="NIP">
                          <xsl:if test="$pNIPclean!='TRUE'">
                              <xsl:value-of select="$nipodb"/>
                          </xsl:if>
                          <xsl:if test="$pNIPclean='TRUE'">
                              <xsl:value-of select="translate($nipodb,$nipn,$numb)"/>
                          </xsl:if>
                        </xsl:element>
                        <xsl:element name="REGON">
                            <xsl:value-of select="normalize-space(ancestor-or-self::node()/Numer_REGON)"/>
                        </xsl:element>
                    </xsl:element>
                </xsl:element>
                <xsl:element name="GRUPY"/>
            </xsl:element>
        </xsl:for-each>
    </xsl:template>
</xsl:stylesheet>
