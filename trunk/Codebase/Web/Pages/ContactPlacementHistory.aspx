<%@ Page Language="C#" MasterPageFile="~/MasterPagePopup.master" AutoEventWireup="true" CodeFile="ContactPlacementHistory.aspx.cs" Inherits="Pages_ContactPlacementHistory"  Title="PlacementHistory"%>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="body" Runat="Server">    
    <div>      
                  
            <div class="COTBox">                
                <div class="WinGroupBoxHeader">Placement History</div>  


    <div id="view1" runat="server"></div>
    <aquarium:DataViewExtender id="view1Extender" runat="server" TargetControlID="view1" Controller="ContactPlacementHistory" view="grid1" ShowInSummary="True"  ShowQuickFind="false" ShowViewSelector="false" />

  <div factory:flow="NewRow" style="padding-top:8px" xmlns:factory="urn:codeontime:app-factory"></div>
            </div>                

        
    </div> 
</asp:Content>



