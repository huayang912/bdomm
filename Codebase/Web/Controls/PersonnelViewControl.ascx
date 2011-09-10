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
        <div class='box'>
 <div class='boxtop'><div></div></div>
 
			<div class="container">
 <b class="rtop"><b class="r1"></b> <b class="r2"></b> <b class="r3"></b> <b class="r4"></b></b>
 
 
  <div class='boxcontent'>
        <table width="100%" border="0">    
            <tr> 
                <td valign="top">
                    <div factory:flow="NewRow" style="padding-top:8px" xmlns:factory="urn:codeontime:app-factory">
                        <act:TabContainer ID="OrderManager" runat="server">
                           
						    <div class="container">

						   
						    <act:TabPanel ID="CustomersTab" runat="server" HeaderText="Personnel" PageSize="25">                                
                                <ContentTemplate>
                                    <div id="view1" runat="server"></div>
			                        <aquarium:DataViewExtender id="view1Extender" runat="server" 
			                            TargetControlID="view1" Controller="ContactsView" view="grid1" PageSize="25"  
			                            ShowViewSelector="false" ShowQuickFind="true"/>
			                    </ContentTemplate>
                            </act:TabPanel>
          
							
							
							
							
			
			              
			
                        </act:TabPanel>
						
                    </act:TabContainer>
                </div>
				
				<div factory:flow="NewRow" style="padding-top:8px" xmlns:factory="urn:codeontime:app-factory">
                       
						             <div id="view611" runat="server"></div>
                                <aquarium:DataViewExtender id="view611Extender" runat="server" TargetControlID="view611" Controller="ContactsCommsNotes" view="grid1" FilterSource="view1Extender" FilterFields="ContactID" PageSize="5" AutoHide="Container" ShowViewSelector="False" ShowQuickFind="false" ShowActionBar="true" />

			   </div>
            </td>     
            <td>  
			

			
			
			
                <table>
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
                                <div id="view12" runat="server"></div>
                                <aquarium:DataViewExtender id="view12Extender" runat="server" TargetControlID="view12" Controller="TelephoneNumbers" view="grid1" FilterSource="view1Extender" FilterFields="ContactID" PageSize="5" AutoHide="Container" ShowViewSelector="false" ShowQuickFind="false" ShowActionBar="true" />
                                <div id="view8" runat="server"></div>
                                <aquarium:DataViewExtender id="view8Extender" runat="server" TargetControlID="view8" Controller="EmailAddresses" view="grid1" FilterSource="view1Extender" FilterFields="ContactID" PageSize="5" AutoHide="Container" ShowViewSelector="False" ShowQuickFind="false" ShowActionBar="true"/>

                                <div id="view4" runat="server"></div>
                                <aquarium:DataViewExtender id="view4Extender" runat="server" TargetControlID="view4" Controller="ContactRoles" view="grid1" FilterSource="view1Extender" FilterFields="ContactID" PageSize="5" AutoHide="Container" ShowViewSelector="False" ShowQuickFind="false" ShowActionBar="true"/>
                                <div id="view6" runat="server"></div>
                            </div>  
	                    </td>
	                </tr>
	            </table>	


 </div>



		    </td>
        </tr>
  </table>
  				</div>
				
				 <b class="rbottom"><b class="r4"></b> <b class="r3"></b> <b class="r2"></b> <b class="r1"></b></b>
				
				
 <div class='boxbottom'><div></div></div>
</div>
<%--    </ContentTemplate>
</asp:UpdatePanel>--%>
