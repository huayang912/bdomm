<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns="http://schemas.microsoft.com/sqlserver/reporting/2005/01/reportdefinition"
    xmlns:rd="http://schemas.microsoft.com/SQLServer/reporting/reportdesigner"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:bo="urn:schemas-codeontime-com:business-objects"
    xmlns:a="urn:schemas-codeontime-com:data-aquarium"
    exclude-result-prefixes="msxsl"
>
	<xsl:output method="xml" indent="yes"/>
	<xsl:param name="ViewName" select="'grid1'"/>
	<xsl:variable name="DefaultFieldColumns" select="40"/>
	<xsl:variable name="MinimumFieldColumns" select="16"/>
	<xsl:variable name="PortraitReportMaxFieldCount" select="8"/>

	<xsl:template match="a:dataController">
		<xsl:variable name="Self" select="."/>
		<xsl:variable name="Fields" select="a:fields/a:field[not(@onDemand='true')]"/>
		<xsl:variable name="DataFields" select="a:views/a:view[@id=$ViewName][1]//a:dataField[@fieldName=$Fields/@name and not(@hidden='true')]"/>
		<xsl:variable name="ReportWidth">
			<xsl:choose>
				<xsl:when test="count($DataFields)&gt;$PortraitReportMaxFieldCount">
					<xsl:value-of select="10"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="7.5"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="ReportHeight">
			<xsl:choose>
				<xsl:when test="count($DataFields)&gt;$PortraitReportMaxFieldCount">
					<xsl:value-of select="7.5"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="10"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<Report xmlns="http://schemas.microsoft.com/sqlserver/reporting/2005/01/reportdefinition" xmlns:rd="http://schemas.microsoft.com/SQLServer/reporting/reportdesigner">
			<DataSources>
				<DataSource Name="{$Self/@name}DataSource">
					<rd:DataSourceID>0e04e19a-b1dd-4ad1-bd43-174d0d295397</rd:DataSourceID>
					<ConnectionProperties>
						<DataProvider>OLEDB</DataProvider>
						<ConnectString />
					</ConnectionProperties>
				</DataSource>
			</DataSources>
			<InteractiveHeight>
				<xsl:value-of select="$ReportHeight + 1"/>
				<xsl:text>in</xsl:text>
			</InteractiveHeight>
			<ReportParameters>
				<ReportParameter Name="ShowLines">
					<DataType>Boolean</DataType>
					<DefaultValue>
						<Values>
							<Value>=True</Value>
						</Values>
					</DefaultValue>
					<AllowBlank>true</AllowBlank>
					<Prompt>Report_Parameter_0</Prompt>
					<Hidden>true</Hidden>
				</ReportParameter>
			</ReportParameters>
			<rd:DrawGrid>false</rd:DrawGrid>
			<InteractiveWidth>
				<xsl:value-of select="$ReportWidth + 1"/>
				<xsl:text>in</xsl:text>
			</InteractiveWidth>
			<rd:SnapToGrid>false</rd:SnapToGrid>
			<RightMargin>0.5in</RightMargin>
			<LeftMargin>0.5in</LeftMargin>
			<PageHeader>
				<PrintOnFirstPage>true</PrintOnFirstPage>
				<ReportItems>
					<Textbox Name="ReportTitleTextbox">
						<rd:DefaultName>textbox5</rd:DefaultName>
						<Style>
							<FontFamily>Arial Unicode MS</FontFamily>
							<FontSize>18pt</FontSize>
							<TextAlign>Center</TextAlign>
							<PaddingLeft>2pt</PaddingLeft>
							<PaddingRight>2pt</PaddingRight>
							<PaddingTop>2pt</PaddingTop>
							<PaddingBottom>2pt</PaddingBottom>
						</Style>
						<CanGrow>true</CanGrow>
						<Height>0.375in</Height>
						<Value>
							<xsl:value-of select="@label"/>
						</Value>
					</Textbox>
					<xsl:call-template name="RenderFieldTextBoxes">
						<xsl:with-param name="Fields" select="$Fields"/>
						<xsl:with-param name="DataFields" select="$DataFields"/>
						<xsl:with-param name="HeaderOnly" select="true()"/>
						<xsl:with-param name="ReportWidth" select="$ReportWidth"/>
					</xsl:call-template>
				</ReportItems>
				<Height>0.75in</Height>
				<PrintOnLastPage>true</PrintOnLastPage>
				<Style>
					<BorderStyle>
						<Bottom>Solid</Bottom>
					</BorderStyle>
					<BorderWidth>
						<Default>2pt</Default>
					</BorderWidth>
				</Style>
			</PageHeader>
			<BottomMargin>0.5in</BottomMargin>
			<rd:ReportID>9168933f-7fe4-4dac-89eb-8714804b3c85</rd:ReportID>
			<PageWidth>
				<xsl:value-of select="concat($ReportWidth + 1, 'in')"/>
			</PageWidth>
			<PageHeight>
				<xsl:value-of select="concat($ReportHeight + 1, 'in')"/>
			</PageHeight>
			<DataSets>
				<DataSet Name="{@name}">
					<Fields>
						<xsl:for-each select="$Fields[not(@onDemand='true')]">
							<Field Name="{@name}">
								<DataField>
									<xsl:value-of select="@name"/>
								</DataField>
								<rd:TypeName>
									<xsl:text>System.</xsl:text>
									<xsl:value-of select="@type"/>
								</rd:TypeName>
							</Field>
						</xsl:for-each>
					</Fields>
					<Query>
						<DataSourceName>
							<xsl:value-of select="$Self/@name"/>
							<xsl:text>DataSource</xsl:text>
						</DataSourceName>
						<CommandText />
					</Query>
				</DataSet>
			</DataSets>
			<Code />
			<Width>
				<xsl:value-of select="concat($ReportWidth,'in')"/>
			</Width>
			<Body>
				<ReportItems>
					<List Name="list1">
						<DataSetName>
							<xsl:value-of select="@name"/>
						</DataSetName>
						<ReportItems>
							<Line Name="line2">
								<Visibility>
									<Hidden>=Not Parameters!ShowLines.Value</Hidden>
								</Visibility>
								<Top>0.25in</Top>
								<Width>
									<xsl:value-of select="concat('-', $ReportWidth,'in')"/>
								</Width>
								<Style>
									<BorderColor>
										<Default>Silver</Default>
									</BorderColor>
									<BorderStyle>
										<Default>Solid</Default>
									</BorderStyle>
								</Style>
								<ZIndex>2</ZIndex>
								<Left>
									<xsl:value-of select="concat($ReportWidth,'in')"/>
								</Left>
							</Line>
							<xsl:call-template name="RenderFieldTextBoxes">
								<xsl:with-param name="Fields" select="$Fields"/>
								<xsl:with-param name="DataFields" select="$DataFields"/>
								<xsl:with-param name="HeaderOnly" select="false()"/>
								<xsl:with-param name="ReportWidth" select="$ReportWidth"/>
							</xsl:call-template>
						</ReportItems>
					</List>
				</ReportItems>
				<Height>0.25in</Height>
			</Body>
			<Language>en-US</Language>
			<PageFooter>
				<PrintOnFirstPage>true</PrintOnFirstPage>
				<ReportItems>
					<Textbox Name="textbox7">
						<rd:DefaultName>textbox7</rd:DefaultName>
						<Width>2.125in</Width>
						<Style>
							<FontFamily>Arial Unicode MS</FontFamily>
							<FontStyle>Italic</FontStyle>
							<FontSize>8pt</FontSize>
							<VerticalAlign>Bottom</VerticalAlign>
							<PaddingLeft>2pt</PaddingLeft>
							<PaddingRight>2pt</PaddingRight>
							<PaddingTop>2pt</PaddingTop>
							<PaddingBottom>2pt</PaddingBottom>
						</Style>
						<ZIndex>1</ZIndex>
						<CanGrow>true</CanGrow>
						<Value>="^PrintedOn^Printed on^PrintedOn^ " &amp; CStr(Now())</Value>
					</Textbox>
					<Textbox Name="textbox6">
						<rd:DefaultName>textbox6</rd:DefaultName>
						<Style>
							<FontFamily>Arial Unicode MS</FontFamily>
							<TextAlign>Right</TextAlign>
							<PaddingLeft>2pt</PaddingLeft>
							<PaddingRight>2pt</PaddingRight>
							<PaddingTop>2pt</PaddingTop>
							<PaddingBottom>2pt</PaddingBottom>
						</Style>
						<CanGrow>true</CanGrow>
						<Left>6in</Left>
						<Value>="^Page^Page^Page^ " &amp; CStr(Globals!PageNumber) &amp; " ^of^of^of^ " &amp; CStr(Globals!TotalPages)</Value>
					</Textbox>
				</ReportItems>
				<Height>0.25in</Height>
				<PrintOnLastPage>true</PrintOnLastPage>
			</PageFooter>
			<TopMargin>0.5in</TopMargin>
		</Report>
	</xsl:template>

	<xsl:template name="RenderFieldTextBoxes">
		<xsl:param name="DataFields"/>
		<xsl:param name="Fields"/>
		<xsl:param name="HeaderOnly" />
		<xsl:param name="ReportWidth"/>
		<xsl:variable name="TotalColumns" select="sum($DataFields[@columns>=$MinimumFieldColumns]/@columns) + count($DataFields[@columns &lt; $MinimumFieldColumns])*$MinimumFieldColumns + count($DataFields[not(@columns)])*$DefaultFieldColumns"/>
		<xsl:for-each select="$DataFields">
			<xsl:variable name="Field" select="$Fields[(current()/@aliasFieldName and @name = current()/@aliasFieldName) or (not(current()/@aliasFieldName) and current()/@fieldName = @name)]"/>
			<xsl:variable name="FieldColumns">
				<xsl:choose>
					<xsl:when test="@columns>$MinimumFieldColumns">
						<xsl:value-of select="@columns"/>
					</xsl:when>
					<xsl:when test="@columns">
						<xsl:value-of select="$MinimumFieldColumns"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="$DefaultFieldColumns"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>
			<xsl:variable name="TextBoxName">
				<xsl:value-of select="$Field/@name"/>
				<xsl:if test="$HeaderOnly=true()">
					<xsl:text>Header</xsl:text>
				</xsl:if>
				<xsl:text>TextBox</xsl:text>
			</xsl:variable>
			<Textbox Name="{$TextBoxName}">
				<rd:DefaultName>
					<xsl:value-of select="$TextBoxName"/>
				</rd:DefaultName>
				<xsl:if test="$HeaderOnly=true()">
					<Top>0.375in</Top>
				</xsl:if>
				<xsl:variable name="PrecedingColumns" select="sum(preceding-sibling::*[@columns>=$MinimumFieldColumns and not(@hidden='true')]/@columns) + count(preceding-sibling::*[@columns &lt; $MinimumFieldColumns and not(@hidden='true')])*$MinimumFieldColumns + count(preceding-sibling::*[not(@columns) and not(@hidden='true')])*$DefaultFieldColumns"/>
				<Left>
					<xsl:value-of select="$ReportWidth * $PrecedingColumns div $TotalColumns"/>
					<xsl:text>in</xsl:text>
				</Left>
				<xsl:choose>
					<xsl:when test="$HeaderOnly=true()">
						<Height>0.20in</Height>
					</xsl:when>
					<xsl:otherwise>
						<Height>0.25in</Height>
					</xsl:otherwise>
				</xsl:choose>
				<Width>
					<xsl:value-of select="$ReportWidth * $FieldColumns div $TotalColumns"/>
					<xsl:text>in</xsl:text>
				</Width>
				<Style>
					<FontFamily>Arial Unicode MS</FontFamily>
					<FontSize>8pt</FontSize>
					<xsl:if test="$HeaderOnly=false()">
						<xsl:if test="not(@dataFormatString!='') and $Field/@type='DateTime'">
							<Format>d</Format>
						</xsl:if>
						<TextAlign>
							<xsl:choose>
								<xsl:when test="$Field/@type='Boolean'">
									<xsl:text>Center</xsl:text>
								</xsl:when>
								<xsl:when test="$Field/@type[not(.='String' or .='DateTime')]">
									<xsl:text>Right</xsl:text>
								</xsl:when>
								<xsl:otherwise>
									<xsl:text>Left</xsl:text>
								</xsl:otherwise>
							</xsl:choose>
						</TextAlign>
					</xsl:if>
					<xsl:if test="$HeaderOnly=true()">
						<FontWeight>700</FontWeight>
						<VerticalAlign>Top</VerticalAlign>
						<TextAlign>
							<xsl:choose>
								<xsl:when test="$Field/@type='Boolean'">
									<xsl:text>Center</xsl:text>
								</xsl:when>
								<xsl:when test="$Field/@type[not(.='String' or .='DateTime')]">
									<xsl:text>Right</xsl:text>
								</xsl:when>
								<xsl:otherwise>
									<xsl:text>Left</xsl:text>
								</xsl:otherwise>
							</xsl:choose>
						</TextAlign>
					</xsl:if>
					<PaddingLeft>2pt</PaddingLeft>
					<PaddingRight>2pt</PaddingRight>
					<PaddingTop>2pt</PaddingTop>
					<PaddingBottom>2pt</PaddingBottom>
				</Style>
				<ZIndex>1</ZIndex>
				<CanGrow>true</CanGrow>
				<Value>
                    <xsl:variable name="DataFormatString">
                        <xsl:choose>
                            <xsl:when test="@dataFormatString!=''">
                                <xsl:value-of select="@dataFormatString"/>
                            </xsl:when>
                            <xsl:otherwise>
                                <xsl:value-of select="$Field/@dataFormatString"/>
                            </xsl:otherwise>
                        </xsl:choose>
                    </xsl:variable>
					<xsl:choose>
						<xsl:when test="$HeaderOnly=true()">
							<xsl:value-of select="$Field/@label"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:choose>
								<xsl:when test="$DataFormatString!=''">
									<xsl:variable name="dfs">
										<xsl:choose>
											<xsl:when test="contains($DataFormatString, '{')">
												<xsl:value-of select="$DataFormatString"/>
											</xsl:when>
											<xsl:otherwise>
												<xsl:text>{0:</xsl:text>
												<xsl:value-of select="$DataFormatString"/>
												<xsl:text>}</xsl:text>
											</xsl:otherwise>
										</xsl:choose>
									</xsl:variable>
									<xsl:text>=String.Format("</xsl:text>
									<xsl:value-of select="$dfs"/>
									<xsl:text>", Fields!</xsl:text>
									<xsl:value-of select="$Field/@name"/>
									<xsl:text>.Value)</xsl:text>
								</xsl:when>
								<xsl:otherwise>
									<xsl:text>=Fields!</xsl:text>
									<xsl:value-of select="$Field/@name"/>
									<xsl:text>.Value</xsl:text>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:otherwise>
					</xsl:choose>
				</Value>
			</Textbox>
		</xsl:for-each>
	</xsl:template>
</xsl:stylesheet>
