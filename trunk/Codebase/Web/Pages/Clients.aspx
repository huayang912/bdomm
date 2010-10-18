<%@ Page Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="Clients.aspx.cs" Inherits="Pages_Clients"  Title="Clients"%>
<asp:Content ID="Content1" ContentPlaceHolderID="PageHeaderContentPlaceHolder" runat="Server">Clients</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageContentPlaceHolder" runat="Server">
  <div factory:flow="NewRow" xmlns:factory="urn:codeontime:app-factory">
    <div class="DataViewHeader">Clients</div>
    <div id="view1" runat="server"></div>
    <aquarium:DataViewExtender id="view1Extender" runat="server" TargetControlID="view1" Controller="Clients" view="grid1" ShowInSummary="True" PageSize="25"/>
  </div>
  <div factory:flow="NewRow" style="padding-top:8px" xmlns:factory="urn:codeontime:app-factory">
    <div factory:activator="Tab|Client Contacts">
      <div id="view2" runat="server"></div>
      <aquarium:DataViewExtender id="view2Extender" runat="server" TargetControlID="view2" Controller="ClientContacts" view="grid1" FilterSource="view1Extender" FilterFields="CompanyID" PageSize="5" AutoHide="Container" />
    </div>
    <div factory:activator="Tab|Clients Notes">
      <div id="view5" runat="server"></div>
      <aquarium:DataViewExtender id="view5Extender" runat="server" TargetControlID="view5" Controller="ClientsNotes" view="grid1" FilterSource="view1Extender" FilterFields="ClientID" PageSize="5" AutoHide="Container" />
    </div>
    <div factory:activator="Tab|Purchase Orders">
      <div id="view7" runat="server"></div>
      <aquarium:DataViewExtender id="view7Extender" runat="server" TargetControlID="view7" Controller="ClientPurchaseOrders" view="grid1" FilterSource="view1Extender" FilterFields="ClientID" />
    </div>
    <div factory:activator="Tab|Financial Details">
      <div id="view9" runat="server"></div>
      <aquarium:DataViewExtender id="view9Extender" runat="server" TargetControlID="view9" Controller="ClientsFinancial" view="grid1" FilterSource="view1Extender" FilterFields="ClientID" />
    </div>
  </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="SideBarPlaceHolder" runat="Server">
  <div class="TaskBox">
    <div class="Inner">
      <div class="Header">About</div>
      <div class="Value">This page allows clients management.</div>
    </div>
  </div>
</asp:Content>