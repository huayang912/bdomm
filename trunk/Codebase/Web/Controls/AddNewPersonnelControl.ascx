<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AddNewPersonnelControl.ascx.cs"
    Inherits="Controls_AddNewPersonnelControl" %>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <table width="100%" border="0">
    <tr>
      <td valign="top"><div factory:flow="NewRow" style="padding-top:8px" xmlns:factory="urn:codeontime:app-factory">
             <act:TabContainer ID="OrderManager" runat="server">
            <act:TabPanel ID="CustomersTab" runat="server" HeaderText="Personnel">
              <ContentTemplate>
                <div id="view1" runat="server"></div>
                <aquarium:DataViewExtender id="view1Extender" runat="server" TargetControlID="view1" Controller="Contacts"   view="createForm1" StartCommandName="New"/>
              </ContentTemplate>
            </act:TabPanel>
      
			
			
			
			
          </act:TabContainer>
        </div></td>
      <td>
      
    </tr>
  </table>
    </ContentTemplate>
</asp:UpdatePanel>
