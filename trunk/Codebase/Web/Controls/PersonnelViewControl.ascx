<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PersonnelViewControl.ascx.cs" Inherits="Controls_PersonnelViewControl"  %>

<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>        
         <div>
            <asp:Repeater ID="rptStartsWith" runat="server" 
                onitemdatabound="rptStartsWith_ItemDataBound"
                OnItemCommand="rptStartsWith_Command">
                <ItemTemplate>
                    <div class="floatleft">                        
                        &nbsp;<asp:LinkButton ID="lkbCommand" CommandName="Filter" runat="server"></asp:LinkButton>&nbsp;| 
                    </div>
                </ItemTemplate>                
            </asp:Repeater>
            <div style="clear:both;"></div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
        
        <table width="100%" border="0">    
            <tr> 
                <td valign="top">
                    <div factory:flow="NewRow" style="padding-top:8px" xmlns:factory="urn:codeontime:app-factory">
                        <act:TabContainer ID="OrderManager" runat="server">
                            <act:TabPanel ID="CustomersTab" runat="server" HeaderText="Personnel" PageSize="25">                                
                                <ContentTemplate>
                                    <div id="view1" runat="server"></div>
			                        <aquarium:DataViewExtender id="view1Extender" runat="server" 
			                            TargetControlID="view1" Controller="ContactsView" view="grid1" PageSize="25"  
			                            ShowViewSelector="false" ShowQuickFind="true"/>
			                    </ContentTemplate>
                            </act:TabPanel>
                            
                            <act:TabPanel ID="EmploymentHistoryTab" runat="server" HeaderText="Employment History">
                                <ContentTemplate>
                                    <div id="view9" runat="server"></div>
                                    <aquarium:DataViewExtender id="view9Extender" runat="server" 
                                        TargetControlID="view9" Controller="EmploymentHistory" 
                                        view="grid1" FilterSource="view1Extender" 
                                        FilterFields="ContactID"  ShowViewSelector="false" PageSize="5"  />
                              
                                    <div id="EmploymentHistory_editForm1" style="display: none">
                                        <table style="width: 100%">
                                            <tr>
                                                <td width="50%" valign="top">
				                                <div>
                                                        {ContactID}
                                                    </div>
                                                    <div>
                                                        {StartDate}
                                                    </div>
                                                    <div>
                                                        {EndtDate}
                                                    </div>
                                                    <div>
                                                        {ProjectID}
                                                    </div>
                                                    <div>
                                                        {ProjectCode_other}
                                                    </div>
                                                    <div>
                                                        {RoleID}
                                                    </div>
					                                 <div>
                                                        {Contract_days}
                                                    </div>
					                                 <div>
                                                        {Notes}
                                                    </div>
                                					
                                					
                                                </td>
                                                <td width="50%" valign="top">
                                                    <div>
                                                        {CurrencyID}
                                                    </div>
                                                    <div>
                                                        {TravelRate}
                                                    </div>
                                                    <div>
                                                        {TravelCost}
                                                    </div>
                                                    <div>
                                                        {OffshoreRate}
                                                    </div>
                                                    <div>
                                                        {Office_Onsh_Rate_type}
                                                    </div>
					                                 <div>
                                                        {OfficeOnshoreRate}
                                                    </div>
					                                 <div>
                                                        {Hour_Standby_Rate_type} - {HourStandbyRate}
                                                    </div>
                                					 
                                					
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </ContentTemplate>
                            </act:TabPanel>
			
			                <act:TabPanel ID="CVTab" runat="server" HeaderText="Personnel CV">
                                <ContentTemplate>
                                    <div id="view91" runat="server"></div>
                                    <aquarium:DataViewExtender id="view91Extender" runat="server" 
                                        TargetControlID="view91" Controller="CVs_m" 
                                        view="grid1" FilterSource="view1Extender" FilterFields="ContactID"
                                        ShowViewSelector="false" PageSize="5"  />
                                </ContentTemplate>
                            </act:TabPanel>
                            
                            <act:TabPanel ID="NextOfKinTab" runat="server" HeaderText="Next Of Kin">
                                <ContentTemplate>
                                  <div id="view10" runat="server"></div>
                                  <aquarium:DataViewExtender id="view10Extender" runat="server" 
                                        TargetControlID="view10" Controller="NextOfKin" 
                                        view="grid1" FilterSource="view1Extender" FilterFields="ContactID"  
                                        ShowViewSelector="false" PageSize="5" />
                                </ContentTemplate>
                            </act:TabPanel>
			
			                <act:TabPanel ID="CertificateTab" runat="server" HeaderText="Certificates">
                                <ContentTemplate>
                                    <div id="view3" runat="server"></div>
                                    <aquarium:DataViewExtender id="view3Extender" runat="server" 
                                        TargetControlID="view3" Controller="Certificates" 
                                        FilterSource="view1Extender" FilterFields="ContactID"  
                                        ShowViewSelector="false" PageSize="5" />
                                </ContentTemplate> 
                            </act:TabPanel>
			
			                <act:TabPanel ID="BankTab" runat="server" HeaderText="Bank Details">
                                <ContentTemplate>
                                    <div id="view2" runat="server"></div>
                                    <aquarium:DataViewExtender id="view2Extender" runat="server" 
                                        TargetControlID="view2" Controller="BankDetails" 
                                        view="grid1" FilterSource="view1Extender" FilterFields="ContactID" 
                                        ShowViewSelector="false"  PageSize="5"  />
                                </ContentTemplate>
                            </act:TabPanel>
			
			
			                <act:TabPanel ID="TravelTab" runat="server" HeaderText="Travel Details">
                                <ContentTemplate>
                                    <act:TabContainer ID="TravelManager" runat="server">
                                        <act:TabPanel ID="TravrlDetTab" runat="server" HeaderText="Travel">
                                            <ContentTemplate>
                                                <div id="view7" runat="server"></div>
                                                    <aquarium:DataViewExtender id="view7Extender" runat="server" 
                                                        TargetControlID="view7" Controller="ContactsTravel" view="grid1" 
                                                        FilterSource="view1Extender" FilterFields="ContactID" ShowViewSelector="false" 
                                                        PageSize="5" />
                                            </ContentTemplate>
                                        </act:TabPanel>
                                        
                                        <act:TabPanel ID="VisaTab" runat="server" HeaderText="Visa">
                                            <ContentTemplate>
                                                <div id="view13" runat="server"></div>
                                                <aquarium:DataViewExtender id="view13Extender" runat="server" 
                                                    TargetControlID="view13" Controller="Visas" view="grid1" 
                                                    FilterSource="view1Extender" FilterFields="ContactID" 
                                                    ShowViewSelector="false" PageSize="5"/>

                                            </ContentTemplate>
                                        </act:TabPanel>
				                    
				                        <act:TabPanel ID="PassportTab" runat="server" HeaderText="Passport">
                                            <ContentTemplate>
                                                <div id="view11" runat="server"></div>
                                                <aquarium:DataViewExtender id="view11Extender" runat="server" 
                                                    TargetControlID="view11" Controller="Passports" view="grid1" 
                                                    FilterSource="view1Extender" FilterFields="ContactID" 
                                                    ShowViewSelector="false" PageSize="5" />
                                            </ContentTemplate>
                                        </act:TabPanel>				  
                                </act:TabContainer>
                            </ContentTemplate>
                        </act:TabPanel>
                    </act:TabContainer>
                </div>
            </td>     
            <td>  
                <table bgcolor="#6699FF">
                    <tr>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td></td>           
                    </tr>
                    <tr>
                        <td>
			                <div factory:flow="NewRow" style="padding-top:8px" xmlns:factory="urn:codeontime:app-factory">
                                <div id="view12" runat="server" style="background-color:#A9D0F5"></div>
                                <aquarium:DataViewExtender id="view12Extender" runat="server" TargetControlID="view12" Controller="TelephoneNumbers" view="grid1" FilterSource="view1Extender" FilterFields="ContactID" PageSize="5" AutoHide="Container" ShowViewSelector="false" ShowQuickFind="false" ShowActionBar="true" />
                                <div id="view8" runat="server" style="background-color:#A9D0F5"></div>
                                <aquarium:DataViewExtender id="view8Extender" runat="server" TargetControlID="view8" Controller="EmailAddresses" view="grid1" FilterSource="view1Extender" FilterFields="ContactID" PageSize="5" AutoHide="Container" ShowViewSelector="False" ShowQuickFind="false" ShowActionBar="true"/>

                                <div id="view4" runat="server"></div>
                                <aquarium:DataViewExtender id="view4Extender" runat="server" TargetControlID="view4" Controller="ContactRoles" view="grid1" FilterSource="view1Extender" FilterFields="ContactID" PageSize="5" AutoHide="Container" ShowViewSelector="False" ShowQuickFind="false" ShowActionBar="true"/>
                                <div id="view6" runat="server"></div>
                                <aquarium:DataViewExtender id="view6Extender" runat="server" TargetControlID="view6" Controller="ContactsNotes" view="grid1" FilterSource="view1Extender" FilterFields="ContactID" PageSize="5" AutoHide="Container" ShowViewSelector="False" ShowQuickFind="false" ShowActionBar="true" />
                            </div>  
	                    </td>
	                </tr>
	            </table>	
		    </td>
        </tr>
  </table>
<%--    </ContentTemplate>
</asp:UpdatePanel>--%>
