<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ClientControlEdit.ascx.cs" Inherits="Controls_ClientControl"  %>



<asp:UpdatePanel ID="UpdatePanel1" runat="server">

<ContentTemplate>
 <table width="100%" border="0">
    <tr>
      <td valign="top"  width="50%" ><div factory:flow="NewRow" style="padding-top:8px; width:400px " xmlns:factory="urn:codeontime:app-factory" >
             <act:TabContainer ID="OrderManager" runat="server">
            <act:TabPanel ID="Clients" runat="server" HeaderText="Clients">
              <ContentTemplate>
                <div id="view1" runat="server"></div>             
                <aquarium:DataViewExtender id="view1Extender" runat="server" TargetControlID="view1" Controller="Clients"   StartCommandName="Edit" StartCommandArgument="editForm1" ShowViewSelector="false" />
              </ContentTemplate>
            </act:TabPanel>
               <act:TabPanel ID="PurchaseOrdersTab" runat="server" HeaderText="Purchase Orders">
              <ContentTemplate>
                <div id="view7" runat="server"></div>
                 <aquarium:DataViewExtender id="view7Extender" runat="server" TargetControlID="view7" Controller="ClientPurchaseOrders" view="grid1" FilterSource="view1Extender" FilterFields="ClientID" ShowViewSelector="false"/>
              </ContentTemplate>
            </act:TabPanel>
			
			      <act:TabPanel ID="FinancialDetailsTab" runat="server" HeaderText="Financial Details">
              <ContentTemplate>
                <div id="view9" runat="server"></div>
               <aquarium:DataViewExtender id="view9Extender" runat="server" TargetControlID="view9" Controller="ClientsFinancial" view="grid1" FilterSource="view1Extender" FilterFields="ClientID"   ShowViewSelector="false"/>
              </ContentTemplate>
            </act:TabPanel>
			
			
         
		
		
		
			
			
          </act:TabContainer>
        </div></td>
        
        <td> <div factory:flow="NewRow" style="padding-top:8px" xmlns:factory="urn:codeontime:app-factory">
              <div id="view2" runat="server"></div>
              <aquarium:DataViewExtender id="DataViewExtender1" runat="server" TargetControlID="view2" Controller="ClientContacts" view="grid1" FilterSource="view1Extender" FilterFields="CompanyID" PageSize="5" AutoHide="Container"  ShowQuickFind="false" ShowActionBar="true" ShowViewSelector="false"/>
        
         <div id="view5" runat="server"></div>
      <aquarium:DataViewExtender id="view5Extender" runat="server" TargetControlID="view5" Controller="ClientsNotes" view="grid1" FilterSource="view1Extender" FilterFields="ClientID" PageSize="5" AutoHide="Container"  ShowQuickFind="false" ShowActionBar="true" ShowViewSelector="false"/>
    </div>
    
             
        </div></td>
       </tr> 
  </table>
</ContentTemplate>

</asp:UpdatePanel>