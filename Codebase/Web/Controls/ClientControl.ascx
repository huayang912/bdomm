<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ClientControl.ascx.cs" Inherits="Controls_ClientControl"  %>


<asp:UpdatePanel ID="UpdatePanel1" runat="server">

<ContentTemplate>

<div class="container">
 <b class="rtop"><b class="r1"></b> <b class="r2"></b> <b class="r3"></b> <b class="r4"></b></b>


  <act:TabContainer ID="OrderManager" runat="server">
            <act:TabPanel ID="Clients" runat="server" HeaderText="Clients">
              <ContentTemplate>
              
 <table width="100%" border="0">
    <tr>
      <td valign="top"  >
        
      <div factory:flow="NewRow" style="padding-top:8px;  xmlns:factory="urn:codeontime:app-factory" >
      
      <div class="GroupBox">                
                <div class="WinGroupBoxHeader">Details</div>  
           
                <div id="view1" runat="server"></div>    
                     <aquarium:DataViewExtender id="view1Extender" runat="server" TargetControlID="view1" Controller="Clients"  view="grid1"  ShowViewSelector="false"  PageSize="25"/>
         </div>
         
         
             </td>
        
        <td> 
        
        <div factory:flow="NewRow" style="padding-top:8px" xmlns:factory="urn:codeontime:app-factory">
          <div class="GroupBox">                
                <div class="WinGroupBoxHeader">Contacts</div> 
        
              <div id="view2" runat="server"></div>
              <aquarium:DataViewExtender id="DataViewExtender1" runat="server" TargetControlID="view2" Controller="ClientContacts" view="grid1" FilterSource="view1Extender" FilterFields="CompanyID" PageSize="5" AutoHide="Container"  ShowQuickFind="false" ShowActionBar="true" ShowViewSelector="false" ShowDescription="false"/>
        
        </div>
        
       
    
    <div class="GroupBox">                
                <div class="WinGroupBoxHeader">Contact Notes</div> 
    
	 <div id="view21" runat="server"></div>
      <aquarium:DataViewExtender id="view21Extender" runat="server" TargetControlID="view21" Controller="ViewAllClientContactNote" view="grid1" FilterSource="view1Extender" FilterFields="ClientID" PageSize="5" AutoHide="Container"  ShowQuickFind="false" ShowActionBar="true" ShowViewSelector="false"  ShowDescription="false"/>
  
  </div>
	
	
	
             
        </div></td>
       </tr> 
  </table>
  
              </ContentTemplate>
            </act:TabPanel>
			
			
			
			
			
         
		
		
		
			
			
          </act:TabContainer>
        </div>
        
           <div class="GroupBox">                
                <div class="WinGroupBoxHeader">Client Notes</div> 
         <div id="view5" runat="server"></div>
      <aquarium:DataViewExtender id="view5Extender" runat="server" TargetControlID="view5" Controller="ClientsNotes" view="grid1" FilterSource="view1Extender" FilterFields="ClientID" PageSize="5" AutoHide="Container"  ShowQuickFind="false" ShowActionBar="true" ShowViewSelector="false" ShowDescription="false"/>
    </div>
        
        
     <div class="GroupBox">                
                <div class="WinGroupBoxHeader">Client Events History</div>    
   <div id="view25" runat="server"></div>
      <aquarium:DataViewExtender id="view25Extender" runat="server" TargetControlID="view25" Controller="ClientsEvents" view="grid1" FilterSource="view1Extender" FilterFields="ClientID" PageSize="5" AutoHide="Container"  ShowQuickFind="false" ShowActionBar="true" ShowViewSelector="false" ShowDescription="false"/>
  </div>
  
    </div>
       		 <b class="rbottom"><b class="r4"></b> <b class="r3"></b> <b class="r2"></b> <b class="r1"></b></b>

  
</ContentTemplate>

</asp:UpdatePanel>