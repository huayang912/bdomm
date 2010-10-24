<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PersonnelDetails.ascx.cs" Inherits="Controls_PersonnelViewControl"  %>

<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <table width="100%" border="0">
    <tr>
    <td><form><input type="button" value=" GO TO EDIT PAGE" onClick="window.location.href='PersonnelNew.aspx'"></form></td></tr>
     <tr> <td valign="top"><div factory:flow="NewRow" style="padding-top:8px" xmlns:factory="urn:codeontime:app-factory">
             <act:TabContainer ID="OrderManager" runat="server">
            <act:TabPanel ID="CustomersTab" runat="server" HeaderText="Personnel" PageSize="25">
              <ContentTemplate>
                <div id="view1" runat="server"></div>
                <aquarium:DataViewExtender id="view1Extender" runat="server" TargetControlID="view1" ShowActionBar="false" Controller="ContactsView" StartCommandName="Select" StartCommandArgument="editForm1" />
              </ContentTemplate>
            </act:TabPanel>
            <act:TabPanel ID="EmploymentHistoryTab" runat="server" HeaderText="Employment History">
              <ContentTemplate>
                <div id="view9" runat="server"></div>
                <aquarium:DataViewExtender id="view9Extender" runat="server" TargetControlID="view9" Controller="EmploymentHistory" view="grid1" FilterSource="view1Extender" FilterFields="ContactID" PageSize="5"  />
              </ContentTemplate>
            </act:TabPanel>
			
			      <act:TabPanel ID="CVTab" runat="server" HeaderText="Personnel CV">
              <ContentTemplate>
                <div id="view91" runat="server"></div>
                <aquarium:DataViewExtender id="view91Extender" runat="server" TargetControlID="view91" Controller="CVs_m" view="grid1" FilterSource="view1Extender" FilterFields="ContactID" PageSize="5"  />
              </ContentTemplate>
            </act:TabPanel>
			
			
            <act:TabPanel ID="NextOfKinTab" runat="server" HeaderText="Next Of Kin">
              <ContentTemplate>
                <act:TabContainer ID="KinManager" runat="server">
                  <act:TabPanel ID="NextOgKinTab" runat="server" HeaderText="Main">
                    <ContentTemplate>
                      <div id="view5" runat="server"></div>
                      <aquarium:DataViewExtender id="view5Extender" runat="server" TargetControlID="view5" Controller="ContactsNextOfKin" view="grid1" FilterSource="view1Extender" FilterFields="ContactID" PageSize="5"  />
                    </ContentTemplate>
                  </act:TabPanel>
                  <act:TabPanel ID="NextOgKinDetailsTab" runat="server" HeaderText="Next Of Kin Details">
                    <ContentTemplate>
                      <div id="view10" runat="server"></div>
                      <aquarium:DataViewExtender id="view10Extender" runat="server" TargetControlID="view10" Controller="NextOfKin" view="grid1" FilterSource="view1Extender" FilterFields="ContactID" PageSize="5" />
                    </ContentTemplate>
                  </act:TabPanel>
                </act:TabContainer>
              </ContentTemplate>
            </act:TabPanel>
			
			 <act:TabPanel ID="CertificateTab" runat="server" HeaderText="Certificates">
              <ContentTemplate>
                <div id="view3" runat="server"></div>
                <aquarium:DataViewExtender id="view3Extender" runat="server" TargetControlID="view3" Controller="Certificates" FilterSource="view1Extender" FilterFields="ContactID" PageSize="5" />
              </ContentTemplate>
            </act:TabPanel>
			
			  <act:TabPanel ID="BankTab" runat="server" HeaderText="Bank Details">
              <ContentTemplate>
      <div id="view2" runat="server"></div>
      <aquarium:DataViewExtender id="view2Extender" runat="server" TargetControlID="view2" Controller="BankDetails" view="grid1" FilterSource="view1Extender" FilterFields="ContactID" PageSize="5"  />
              </ContentTemplate>
            </act:TabPanel>
			
			
			 <act:TabPanel ID="TravelTab" runat="server" HeaderText="Travel Details">
              <ContentTemplate>
                <act:TabContainer ID="TravelManager" runat="server">
                  <act:TabPanel ID="TravrlDetTab" runat="server" HeaderText="Travel">
                    <ContentTemplate>
                         <div id="view7" runat="server"></div>
      <aquarium:DataViewExtender id="view7Extender" runat="server" TargetControlID="view7" Controller="ContactsTravel" view="grid1" FilterSource="view1Extender" FilterFields="ContactID" PageSize="5"  />
                  </ContentTemplate>
                  </act:TabPanel>
                  <act:TabPanel ID="VisaTab" runat="server" HeaderText="Visa">
                    <ContentTemplate>
    <div id="view13" runat="server"></div>
      <aquarium:DataViewExtender id="view13Extender" runat="server" TargetControlID="view13" Controller="Visas" view="grid1" FilterSource="view1Extender" FilterFields="ContactID" PageSize="5"/>

                    </ContentTemplate>
                  </act:TabPanel>
				     <act:TabPanel ID="PassportTab" runat="server" HeaderText="Passport">
                    <ContentTemplate>
    <div id="view11" runat="server"></div>
      <aquarium:DataViewExtender id="view11Extender" runat="server" TargetControlID="view11" Controller="Passports" view="grid1" FilterSource="view1Extender" FilterFields="ContactID" PageSize="5" />


                    </ContentTemplate>
                  </act:TabPanel>
				  
                </act:TabContainer>
              </ContentTemplate>
            </act:TabPanel>
		
			
			
          </act:TabContainer>
        </div></td>
      <td>
      <td width="50%" valign="top">
	  <div factory:flow="NewRow" style="padding-top:8px" xmlns:factory="urn:codeontime:app-factory">
              <div id="view12" runat="server" style="background-color:#A9D0F5"></div>
                <aquarium:DataViewExtender id="view12Extender" runat="server" TargetControlID="view12" Controller="TelephoneNumbers" view="grid1" FilterSource="view1Extender" FilterFields="ContactID" PageSize="5" AutoHide="Container" ShowViewSelector="False" ShowQuickFind="false" ShowActionBar="false" />
            <div id="view4" runat="server"></div>
                <aquarium:DataViewExtender id="view4Extender" runat="server" TargetControlID="view4" Controller="ContactRoles" view="grid1" FilterSource="view1Extender" FilterFields="ContactID" PageSize="5" AutoHide="Container" ShowViewSelector="False" ShowQuickFind="false" ShowActionBar="false"/>
           <div id="view8" runat="server" style="background-color:#A9D0F5"></div>
                <aquarium:DataViewExtender id="view8Extender" runat="server" TargetControlID="view8" Controller="EmailAddresses" view="grid1" FilterSource="view1Extender" FilterFields="ContactID" PageSize="5" AutoHide="Container" ShowViewSelector="False" ShowQuickFind="false" ShowActionBar="false"/>
          <div id="view6" runat="server"></div>
                <aquarium:DataViewExtender id="view6Extender" runat="server" TargetControlID="view6" Controller="ContactsNotes" view="grid1" FilterSource="view1Extender" FilterFields="ContactID" PageSize="5" AutoHide="Container" ShowViewSelector="False" ShowQuickFind="false" ShowActionBar="false" />
        
             
        </div></td>
    </tr>
  </table>
    </ContentTemplate>
</asp:UpdatePanel>
