<%@ Page Language="C#" MasterPageFile="~/MasterPagePopup.master" AutoEventWireup="true" CodeFile="ContactPlacementHistory.aspx.cs" Inherits="Pages_ContactPlacementHistory"  Title="PlacementHistory"%>
<asp:Content ID="Content1" ContentPlaceHolderID="PageHeaderContentPlaceHolder" runat="Server">Placement History</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageContentPlaceHolder" runat="Server">
  <div factory:flow="NewRow" xmlns:factory="urn:codeontime:app-factory">
    <div id="view1" runat="server"></div>
    <aquarium:DataViewExtender id="view1Extender" runat="server" TargetControlID="view1" Controller="ContactPlacementHistory" view="grid1" ShowInSummary="True" />
  </div>
  <div factory:flow="NewRow" style="padding-top:8px" xmlns:factory="urn:codeontime:app-factory"></div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="SideBarPlaceHolder" runat="Server">
  <div class="TaskBox">
    <div class="Inner">
      <div class="Header">About</div>
      <div class="Value">Placement History.</div>
    </div>
  </div>
</asp:Content>