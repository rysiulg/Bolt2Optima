<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="text" encoding="utf-8" />

  <xsl:param name="delim" select="','" />
  <xsl:param name="quote" select="'&quot;'" />
  <xsl:param name="break" select="'&#xA;'" />
<xsl:param name="quotehex" select="'&#x22;'" />  

<!-- change cr+lf to br -->
<xsl:template name="breakht">
  <xsl:param name="text" select="string(.)"/>
  <xsl:choose>
    <xsl:when test="contains($text, '&#xa;')">
      <xsl:value-of select="normalize-space(substring-before($text, '&#xa;'))"/>
      <xsl:text>&lt;br&gt;</xsl:text>
      <xsl:call-template name="breakht">
        <xsl:with-param name="text" select="substring-after($text, '&#xa;')"/>
      </xsl:call-template>
    </xsl:when>
    <xsl:otherwise>
      <xsl:value-of select="normalize-space($text)"/>
    </xsl:otherwise>
  </xsl:choose>
</xsl:template>
  <!-- change nbsp to spc -->
  <xsl:template name="nbsp">
    <xsl:param name="text" select="string(.)"/>
    <xsl:choose>
      <xsl:when test="contains($text, '&#x7f;')">
        <xsl:value-of select="normalize-space(substring-before($text, '&#x7f;'))"/>
        <xsl:text> </xsl:text>
        <xsl:call-template name="nbsp">
          <xsl:with-param name="text" select="substring-after($text, '&#x7f;')"/>
        </xsl:call-template>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="normalize-space($text)"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
<!-- change quot to doublequot -->
<xsl:template name="doublequot">
  <xsl:param name="text" select="string(.)"/>
  <xsl:choose>
    <xsl:when test="contains($text, '&#x22;')">
      <xsl:value-of select="(substring-before($text, '&#x22;'))"/>
      <xsl:value-of select="(concat($quote,$quote))"/>
      <xsl:call-template name="doublequot">
        <xsl:with-param name="text" select="substring-after($text, '&#x22;')"/>
      </xsl:call-template>
    </xsl:when>
    <xsl:otherwise>
      <xsl:value-of select="normalize-space($text)"/>
    </xsl:otherwise>
  </xsl:choose>
</xsl:template>

  <xsl:template match="/">
<!--TITLE-->
	<xsl:for-each select="*/*[1]/*">
	
		<xsl:value-of select="$quote" />
	  <xsl:value-of select="name()" />
	  <xsl:value-of select="$quote" />				
		
		<xsl:if test="position() != last()"><xsl:value-of select="$delim" /> </xsl:if>
		<xsl:if test="position() = last()"> <xsl:value-of select="$break" /> </xsl:if>
	</xsl:for-each>

    <xsl:apply-templates select="Products/product" />
    <xsl:apply-templates select="Items/item" />
  </xsl:template>

 
  <xsl:template match="product">
    <xsl:apply-templates />
    <xsl:if test="following-sibling::*"> <xsl:value-of select="$break" /> </xsl:if>
  </xsl:template>
  <xsl:template match="item">
    <xsl:apply-templates />
    <xsl:if test="following-sibling::*"> <xsl:value-of select="$break" /> </xsl:if>
  </xsl:template>

  <xsl:template match="*">
    <!-- remove normalize-space() to . if you want keep white-space at it is --> 
    <!-- added checkin that opis or description fields to change chr(10) to <br>-->


    <xsl:value-of select="$quote" />



<xsl:choose>
    <xsl:when test="count(child::node())>1">
      <xsl:for-each select="child::node()">
        <xsl:if test="name(.)!=''">
          <xsl:value-of select="name(.)"/>
          <xsl:value-of select="'='"/>
          <xsl:value-of select="$quote" />
          <xsl:value-of select="$quote" />
          
          <xsl:choose>
            <xsl:when test="contains(normalize-space(.),'&#x22;') ">
              <xsl:call-template name="doublequot">
                <!--<xsl:with-param name="text" select="$namenbsp" />-->
                <xsl:with-param name="text" select="normalize-space(.)" />
              </xsl:call-template>
            </xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="normalize-space(.)"/>
            </xsl:otherwise>
          </xsl:choose>
          
          
          <xsl:value-of select="$quote" />
          <xsl:value-of select="$quote" />
          <xsl:if test="following-sibling::*">
            <xsl:value-of select="$delim" />
          </xsl:if>
        </xsl:if>
      </xsl:for-each>
      
    </xsl:when>
    <xsl:otherwise>
      <xsl:choose>
        <xsl:when test="contains(normalize-space(.),'&#x22;') ">
          <xsl:call-template name="doublequot">
            <!--<xsl:with-param name="text" select="$namenbsp" />-->
            <xsl:with-param name="text" select="normalize-space(.)" />
          </xsl:call-template>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="normalize-space(.)"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:otherwise>
</xsl:choose>
    <xsl:value-of select="$quote" />			




    <xsl:if test="following-sibling::*">
      <xsl:value-of select="$delim" />
    </xsl:if>
  </xsl:template>

  <xsl:template match="text()" />
</xsl:stylesheet>


