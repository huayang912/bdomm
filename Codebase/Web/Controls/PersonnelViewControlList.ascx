<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PersonnelViewControlList.ascx.cs" Inherits="Controls_PersonnelViewControl"  %>

<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <table width="100%" border="0">
    
     <tr> <td  width="50%" valign="top"><div factory:flow="NewRow" style="padding-top:8px" xmlns:factory="urn:codeontime:app-factory">
             <act:TabContainer ID="OrderManager" runat="server">
            <act:TabPanel ID="CustomersTab" runat="server" HeaderText="Personnel" PageSize="25">
              <ContentTemplate>
                <div id="view1" runat="server"></div>
                <aquarium:DataViewExtender id="view1Extender" runat="server" TargetControlID="view1" Controller="ContactsView" PageSize="25"  ShowViewSelector="false" />
              
			  
			   
			    </ContentTemplate>   
			   
			   
			   
			   
            </act:TabPanel>
          
			
			
           
		

			
			
		
		
			
			
          </act:TabContainer>
        </div></td>
     
<td width="50%">  
<Table>
<tr><td>&nbsp;</td></tr>
<tr><td>&nbsp;</td></tr>
<tr><td>
<tr><td>
			    <div factory:flow="NewRow" style="padding-top:8px" xmlns:factory="urn:codeontime:app-factory">
              <div id="view12" runat="server" style="background-color:#A9D0F5"></div>
                <aquarium:DataViewExtender id="view12Extender" runat="server" TargetControlID="view12" Controller="TelephoneNumbers" view="grid1" FilterSource="view1Extender" FilterFields="ContactID" PageSize="5" AutoHide="Container" ShowViewSelector="False" ShowQuickFind="false" ShowActionBar="false" />
              <div id="view8" runat="server" style="background-color:#A9D0F5"></div>
                <aquarium:DataViewExtender id="view8Extender" runat="server" TargetControlID="view8" Controller="EmailAddresses" view="grid1" FilterSource="view1Extender" FilterFields="ContactID" PageSize="5" AutoHide="Container" ShowViewSelector="False" ShowQuickFind="false" ShowActionBar="false"/>

            <div id="view4" runat="server"></div>
                <aquarium:DataViewExtender id="view4Extender" runat="server" TargetControlID="view4" Controller="ContactRoles" view="grid1" FilterSource="view1Extender" FilterFields="ContactID" PageSize="5" AutoHide="Container" ShowViewSelector="False" ShowQuickFind="false" ShowActionBar="false"/>
                 <div id="view6" runat="server"></div>
                <aquarium:DataViewExtender id="view6Extender" runat="server" TargetControlID="view6" Controller="ContactsNotes" view="grid1" FilterSource="view1Extender" FilterFields="ContactID" PageSize="5" AutoHide="Container" ShowViewSelector="False" ShowQuickFind="false" ShowActionBar="false" />
        
             
        </div>  
	</td></tr></Table>	
		</td>

    </tr>
  </table>
    </ContentTemplate>
</asp:UpdatePanel>
