<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ClientControl.ascx.cs" Inherits="Controls_ClientControl"  %>


<asp:UpdatePanel ID="UpdatePanel1" runat="server">

<ContentTemplate>
 <table width="100%" border="0">
    <tr>
      <td valign="top"  width="50%"><div factory:flow="NewRow" style="padding-top:8px" xmlns:factory="urn:codeontime:app-factory">
             <act:TabContainer ID="OrderManager" runat="server">
            <act:TabPanel ID="Clients" runat="server" HeaderText="Clients">
              <ContentTemplate>
                <div id="view1" runat="server"></div>
                <aquarium:DataViewExtender id="view1Extender" runat="server" TargetControlID="view1" Controller="Clients"   PageSize="25"/>
              </ContentTemplate>
            </act:TabPanel>
                 <a href="~/Controls/AddNewPersonnelControl.ascx">~/Controls/AddNewPersonnelControl.ascx</a>   <act:TabPanel ID="PurchaseOrdersTab" runat="server" HeaderText="Purchase Orders">
              <ContentTemplate>
                <div id="view7" runat="server"></div>
                 <aquarium:DataViewExtender id="view7Extender" runat="server" TargetControlID="view7" Controller="ClientPurchaseOrders" view="grid1" FilterSource="view1Extender" FilterFields="ClientID" />
              </ContentTemplate>
            </act:TabPanel>
			
			      <act:TabPanel ID="FinancialDetailsTab" runat="server" HeaderText="Financial Details">
              <ContentTemplate>
                <div id="view9" runat="server"></div>
               <aquarium:DataViewExtender id="view9Extender" runat="server" TargetControlID="view9" Controller="ClientsFinancial" view="grid1" FilterSource="view1Extender" FilterFields="ClientID" />
              </ContentTemplate>
            </act:TabPanel>
			
			
         
		
		
		
			
			
          </act:TabContainer>
        </div></td>
       </tr> <tr>
      <td width="50%" valign="top">
	  <div factory:flow="NewRow" style="padding-top:8px" xmlns:factory="urn:codeontime:app-factory">
              <div id="view2" runat="server"></div>
              <aquarium:DataViewExtender id="view2Extender" runat="server" TargetControlID="view2" Controller="ClientContacts" view="grid1" FilterSource="view1Extender" FilterFields="CompanyID" PageSize="5" AutoHide="Container" />
        
             
        </div></td>
    </tr>
  </table>
</ContentTemplate>

</asp:UpdatePanel>