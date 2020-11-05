<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
>
    <xsl:output indent="yes" method="xml" omit-xml-declaration="no" version="1.0" encoding="UTF-8" />
    <xsl:strip-space elements="*" />
    <xsl:variable name="OptZrd">MARSP</xsl:variable>
    <xsl:variable name="OptDoc">MARMP</xsl:variable>
    <xsl:variable name="OptimaNazwalimit">50</xsl:variable>
    <xsl:variable name="FixedKod">35-205</xsl:variable>
    <xsl:variable name="FixedMiasto">Rzeszów</xsl:variable>
    
    <xsl:variable name="ucase" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZACELNOSZZACELNOSZZ_'">	</xsl:variable>
    <xsl:variable name="lcase" select="'abcdefghijklmnopqrstuvwxyząćęłńóśżźĄĆĘŁŃÓŚŻŹ '">	</xsl:variable>
    

    
    <xsl:template match="DocumentElement">
        <xsl:text disable-output-escaping="yes">&lt;ROOT xmlns=&quot;http://www.comarch.pl/cdn/optima/offline&quot;&gt;</xsl:text> 
        <xsl:element name="DEFINICJE_DOKUMENTOW">
            <xsl:element name="WERSJA"><xsl:text>2.00</xsl:text></xsl:element>
            <xsl:element name="BAZA_ZRD_ID"><xsl:value-of select="$OptZrd"/></xsl:element>
            <xsl:element name="BAZA_DOC_ID"><xsl:value-of select="$OptDoc"/></xsl:element> 
        </xsl:element>
        <xsl:element name="KONTRAHENCI">
            <xsl:element name="WERSJA"><xsl:text>2.00</xsl:text></xsl:element>
            <xsl:element name="BAZA_ZRD_ID"><xsl:value-of select="$OptZrd"/></xsl:element>
            <xsl:element name="BAZA_DOC_ID"><xsl:value-of select="$OptDoc"/></xsl:element> 
        <xsl:call-template name="ParseDataKontah" />
        </xsl:element>
        <xsl:element name="REJESTRY_SPRZEDAZY_VAT">
            <xsl:element name="WERSJA"><xsl:text>2.00</xsl:text></xsl:element>
            <xsl:element name="BAZA_ZRD_ID"><xsl:value-of select="$OptZrd"/></xsl:element>
            <xsl:element name="BAZA_DOC_ID"><xsl:value-of select="$OptDoc"/></xsl:element> 
        <xsl:call-template name="ParseDataSprzedazy" />
        </xsl:element>

        <xsl:text>&lt;/ROOT&gt;</xsl:text>
    </xsl:template>
    
    <xsl:template name="ParseDataSprzedazy" >
        <xsl:element name="REJESTR_SPRZEDAZY_VAT">
            <xsl:for-each select="Bolt/Nazwa_Firmy_Kierowca">
                <xsl:element name="ID_ZRODLA">A0D739EE-2807-CD5B-533C-C61A74CB22C0</xsl:element>
                <xsl:element name="MODUL">Handel</xsl:element>
                <xsl:element name="TYP">Rejestr sprzedazy</xsl:element>
                <xsl:element name="REJESTR">SPRZEDAŻ</xsl:element>

                <xsl:element name="DATA_WYSTAWIENIA">
                    <xsl:value-of select="concat(substring(normalize-space(ancestor-or-self::node()/Data),7,4),'-',substring(normalize-space(ancestor-or-self::node()/Data),4,2),'-',substring(normalize-space(ancestor-or-self::node()/Data),1,2))"/></xsl:element>
                <xsl:element name="DATA_SPRZEDAZY"><xsl:value-of select="concat(substring(normalize-space(ancestor-or-self::node()/Data_przejazdu),7,4),'-',substring(normalize-space(ancestor-or-self::node()/Data_przejazdu),4,2),'-',substring(normalize-space(ancestor-or-self::node()/Data_przejazdu),1,2))"/></xsl:element>
                <xsl:element name="TERMIN"><xsl:value-of select="concat(substring(normalize-space(ancestor-or-self::node()/Data_przejazdu),7,4),'-',substring(normalize-space(ancestor-or-self::node()/Data_przejazdu),4,2),'-',substring(normalize-space(ancestor-or-self::node()/Data_przejazdu),1,2))"/></xsl:element>
                <xsl:element name="NUMER"><xsl:value-of select="normalize-space(ancestor-or-self::node()/Numer_faktury)"/></xsl:element>
                
                <xsl:element name="WEWNETRZNA">Nie</xsl:element>
                <xsl:element name="DETALICZNA">                    
                    <xsl:if test="normalize-space(ancestor-or-self::node()/NIP_odbiorcy) != ''">Nie</xsl:if>
                    <xsl:if test="normalize-space(ancestor-or-self::node()/NIP_odbiorcy) = ''">Tak</xsl:if>
                </xsl:element>
  <!--              <xsl:element name="IDENTYFIKATOR_KSIEGOWY">FZ111</xsl:element>-->
                <xsl:element name="TYP_PODMIOTU">kontrahent</xsl:element><!--!!!!
dane podmiotu-->
                       <xsl:element name="STATUS">aktualny</xsl:element>
                <xsl:element name="PODMIOT"><xsl:value-of select="translate(normalize-space(ancestor-or-self::node()/Odbiorca),$lcase,$ucase)"/></xsl:element>
                <xsl:element name="NAZWA1">
                    <xsl:if test="string-length(normalize-space(ancestor-or-self::node()/Odbiorca))&lt;$OptimaNazwalimit"><xsl:value-of select="normalize-space(ancestor-or-self::node()/Odbiorca)"/></xsl:if>
                    <xsl:if test="string-length(normalize-space(ancestor-or-self::node()/Odbiorca))&gt;$OptimaNazwalimit"><xsl:value-of select="substring(normalize-space(ancestor-or-self::node()/Odbiorca),1,$OptimaNazwalimit)"/></xsl:if>
                </xsl:element>
                <xsl:element name="NAZWA2">
                    <xsl:if test="string-length(normalize-space(ancestor-or-self::node()/Odbiorca))&gt;$OptimaNazwalimit"><xsl:value-of select="substring(normalize-space(ancestor-or-self::node()/Odbiorca),$OptimaNazwalimit+1,$OptimaNazwalimit)"/></xsl:if>
                </xsl:element>
                <xsl:element name="KRAJ">Polska</xsl:element>
                <xsl:element name="ULICA">
                    <xsl:value-of select="normalize-space(ancestor-or-self::node()/Adres_odbiorcy)"/>
                </xsl:element>
                <xsl:element name="MIASTO"><xsl:value-of select="$FixedMiasto"/></xsl:element>
                <xsl:element name="KOD_POCZTOWY"><xsl:value-of select="$FixedKod"/></xsl:element>
                <xsl:element name="NIP">
                    <xsl:value-of select="normalize-space(ancestor-or-self::node()/NIP_odbiorcy)"/>
                </xsl:element>
                <xsl:element name="REGON">
                    <xsl:value-of select="normalize-space(ancestor-or-self::node()/Numer_REGON)"/>
                </xsl:element>
                
                

                <xsl:element name="FINALNY">
                    <xsl:if test="normalize-space(ancestor-or-self::node()/NIP_odbiorcy) != ''">Nie</xsl:if>
                    <xsl:if test="normalize-space(ancestor-or-self::node()/NIP_odbiorcy) = ''">Tak</xsl:if>
                </xsl:element>
                
                <xsl:element name="OPIS"><xsl:value-of select="concat('Adres odbioru: ',normalize-space(ancestor-or-self::node()/Adres_odbioru))"/></xsl:element>
                <xsl:element name="FORMA_PLATNOSCI">
                    <xsl:if test="normalize-space(ancestor-or-self::node()/Metoda_płatności)!='gotówka'"><xsl:text>przelew</xsl:text></xsl:if>
                    <xsl:if test="normalize-space(ancestor-or-self::node()/Metoda_płatności)='gotówka'"><xsl:text>gotówka</xsl:text></xsl:if>
                </xsl:element>

                <xsl:element name="NOTOWANIE_WALUTY_ILE">1</xsl:element>
                <xsl:element name="NOTOWANIE_WALUTY_ZA_ILE">1</xsl:element>
                <xsl:element name="WARTOSC_SPRZEDAZY"><xsl:value-of select="normalize-space(ancestor-or-self::node()/Cena_z_VAT)"/></xsl:element>
                
                <xsl:element name="POZYCJE">
                    <xsl:element name="POZYCJA">
                        <xsl:element name="LP">1</xsl:element>
                        <xsl:element name="STAWKA_VAT">23.0</xsl:element>
                        <xsl:element name="STATUS_VAT">opodatkowana</xsl:element>
                        <xsl:element name="NETTO"><xsl:value-of select="normalize-space(ancestor-or-self::node()/Cena_bez_VAT)"/></xsl:element>
                        <xsl:element name="VAT"><xsl:value-of select="normalize-space(ancestor-or-self::node()/VAT)"/></xsl:element>
                        <xsl:element name="NETTO_SYS"><xsl:value-of select="normalize-space(ancestor-or-self::node()/Cena_bez_VAT)"/></xsl:element>
                        <xsl:element name="VAT_SYS"><xsl:value-of select="normalize-space(ancestor-or-self::node()/VAT)"/></xsl:element>
                        <xsl:element name="RODZAJ_SPRZEDAZY">uslugi</xsl:element>
                    </xsl:element>
                </xsl:element>
                <xsl:element name="PLATNOSCI">
                    <xsl:element name="PLATNOSC">
                        <xsl:element name="TERMIN_PLAT"><xsl:value-of select="concat(substring(normalize-space(ancestor-or-self::node()/Data),7,4),'-',substring(normalize-space(ancestor-or-self::node()/Data),4,2),'-',substring(normalize-space(ancestor-or-self::node()/Data),1,2))"/></xsl:element>
                    <xsl:element name="FORMA_PLATNOSCI_PLAT">
                        <xsl:if test="normalize-space(ancestor-or-self::node()/Metoda_płatności)!='gotówka'"><xsl:text>przelew</xsl:text></xsl:if>
                        <xsl:if test="normalize-space(ancestor-or-self::node()/Metoda_płatności)='gotówka'"><xsl:text>gotówka</xsl:text></xsl:if>
                    </xsl:element>
                        <xsl:element name="KWOTA_PLN_PLAT"><xsl:value-of select="normalize-space(ancestor-or-self::node()/Suma)"/></xsl:element>
                        <xsl:element name="KWOTA_PLAT"><xsl:value-of select="normalize-space(ancestor-or-self::node()/Suma)"/></xsl:element>
                        <xsl:element name="KIERUNEK">przychód</xsl:element>
                    <xsl:element name="PLATNOSC_TYP_PODMIOTU">kontrahent</xsl:element>
                        <xsl:element name="PODLEGA_ROZLICZENIU">tak</xsl:element>
                        <xsl:element name="WALUTA_DOK">PLN</xsl:element>
                    </xsl:element>
                </xsl:element>
                    
                    
            </xsl:for-each>
        </xsl:element>
    </xsl:template>
    
    <xsl:template name="ParseDataKontah" >
        <xsl:element name="KONTRAHENT">
            <xsl:for-each select="Bolt/Nazwa_Firmy_Kierowca">
                <xsl:element name="ID_ZRODLA">A0D739EE-2807-CD5B-533C-C61A74CB22C0</xsl:element>
                <xsl:element name="AKRONIM"><xsl:value-of select="translate(normalize-space(ancestor-or-self::node()/Odbiorca),$lcase,$ucase)"/></xsl:element>
                <xsl:element name="RODZAJ">dostawca odbiorca</xsl:element>
                <xsl:element name="FINALNY">
                    <xsl:if test="normalize-space(NIP_odbiorcy) != ''">Nie</xsl:if>
                    <xsl:if test="normalize-space(NIP_odbiorcy) = ''">Tak</xsl:if>
                </xsl:element>
                <xsl:element name="NIEAKTYWNY">Nie</xsl:element>
                <xsl:element name="NIE_PODLEGA_ROZLICZENIU">Nie</xsl:element>
                <xsl:element name="PLATNIK_VAT">
                    <xsl:if test="normalize-space(NIP_odbiorcy) != ''">Tak</xsl:if>
                    <xsl:if test="normalize-space(NIP_odbiorcy) = ''">Nie</xsl:if>
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
                            <xsl:value-of select="normalize-space(ancestor-or-self::node()/Adres_odbiorcy)"/>
                        </xsl:element>
                        <xsl:element name="MIASTO"><xsl:value-of select="$FixedMiasto"/></xsl:element>
                        <xsl:element name="KOD_POCZTOWY"><xsl:value-of select="$FixedKod"/></xsl:element>
                        <xsl:element name="NIP">
                            <xsl:value-of select="normalize-space(ancestor-or-self::node()/NIP_odbiorcy)"/>
                        </xsl:element>
                        <xsl:element name="REGON">
                            <xsl:value-of select="normalize-space(ancestor-or-self::node()/Numer_REGON)"/>
                        </xsl:element>
                    </xsl:element>
                </xsl:element>
                <xsl:element name="GRUPY"/>
            </xsl:for-each>
    </xsl:element>
    </xsl:template>
    
</xsl:stylesheet>
