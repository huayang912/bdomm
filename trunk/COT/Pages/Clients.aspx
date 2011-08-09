<%@ Page Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="Clients.aspx.cs" Inherits="Pages_Clients"  Title="Clients"%>
<asp:Content ID="Content1" ContentPlaceHolderID="PageHeaderContentPlaceHolder" runat="Server">Clients</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageContentPlaceHolder" runat="Server">
  <div factory:flow="NewRow" xmlns:factory="urn:codeontime:app-factory">
    <div id="view1" runat="server"></div>
    <aquarium:DataViewExtender id="view1Extender" runat="server" TargetControlID="view1" Controller="Clients" view="grid1" ShowInSummary="True" />
  </div>
  <div factory:flow="NewRow" style="padding-top:8px" xmlns:factory="urn:codeontime:app-factory">
    <div factory:activator="Tab|Client Contacts">
      <div id="view2" runat="server"></div>
      <aquarium:DataViewExtender id="view2Extender" runat="server" TargetControlID="view2" Controller="ClientContacts" view="grid1" FilterSource="view1Extender" FilterFields="CompanyID" PageSize="5" AutoHide="Container" ShowModalForms="True" />
    </div>
    <div factory:activator="Tab|Client Purchase Orders">
      <div id="view3" runat="server"></div>
      <aquarium:DataViewExtender id="view3Extender" runat="server" TargetControlID="view3" Controller="ClientPurchaseOrders" view="grid1" FilterSource="view1Extender" FilterFields="ClientID" PageSize="5" AutoHide="Container" ShowModalForms="True" />
    </div>
    <div factory:activator="Tab|Clients Events">
      <div id="view4" runat="server"></div>
      <aquarium:DataViewExtender id="view4Extender" runat="server" TargetControlID="view4" Controller="ClientsEvents" view="grid1" FilterSource="view1Extender" FilterFields="ClientID" PageSize="5" AutoHide="Container" ShowModalForms="True" />
    </div>
    <div factory:activator="Tab|Clients Financial">
      <div id="view5" runat="server"></div>
      <aquarium:DataViewExtender id="view5Extender" runat="server" TargetControlID="view5" Controller="ClientsFinancial" view="grid1" FilterSource="view1Extender" FilterFields="ClientID" PageSize="5" AutoHide="Container" ShowModalForms="True" />
    </div>
    <div factory:activator="Tab|Clients Notes">
      <div id="view6" runat="server"></div>
      <aquarium:DataViewExtender id="view6Extender" runat="server" TargetControlID="view6" Controller="ClientsNotes" view="grid1" FilterSource="view1Extender" FilterFields="ClientID" PageSize="5" AutoHide="Container" ShowModalForms="True" />
    </div>
    <div factory:activator="Tab|Employment History">
      <div id="view7" runat="server"></div>
      <aquarium:DataViewExtender id="view7Extender" runat="server" TargetControlID="view7" Controller="EmploymentHistory" view="grid1" FilterSource="view1Extender" FilterFields="ClientID" PageSize="5" AutoHide="Container" ShowModalForms="True" />
    </div>
  </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="SideBarPlaceHolder" runat="Server">
  <div class="TaskBox About">
    <div class="Inner">
      <div class="Header">About</div>
      <div class="Value">This page allows clients management.</div>
    </div>
  </div>
</asp:Content>