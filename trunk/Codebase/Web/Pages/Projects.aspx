<%@ Page Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="Projects.aspx.cs" Inherits="Pages_Projects"  Title="Projects"%>
<asp:Content ID="Content1" ContentPlaceHolderID="PageHeaderContentPlaceHolder" runat="Server">Projects</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageContentPlaceHolder" runat="Server">
  <div factory:flow="NewRow" xmlns:factory="urn:codeontime:app-factory">
    <div id="view1" runat="server"></div>
    <aquarium:DataViewExtender id="view1Extender" runat="server" TargetControlID="view1" Controller="Projects" view="grid1" ShowInSummary="True" />
  </div>
  <div factory:flow="NewRow" style="padding-top:8px" xmlns:factory="urn:codeontime:app-factory">
    <div factory:activator="Tab|Client Project Contacts">
      <div id="view2" runat="server"></div>
      <aquarium:DataViewExtender id="view2Extender" runat="server" TargetControlID="view2" Controller="ClientProjectContacts" view="grid1" FilterSource="view1Extender" FilterFields="ProjectID" PageSize="5" AutoHide="Container" />
    </div>
    <div factory:activator="Tab|Client Purchase Orders">
      <div id="view3" runat="server"></div>
      <aquarium:DataViewExtender id="view3Extender" runat="server" TargetControlID="view3" Controller="ClientPurchaseOrders" view="grid1" FilterSource="view1Extender" FilterFields="ProjectID" PageSize="5" AutoHide="Container" />
    </div>
    <div factory:activator="Tab|Employment History">
      <div id="view4" runat="server"></div>
      <aquarium:DataViewExtender id="view4Extender" runat="server" TargetControlID="view4" Controller="EmploymentHistory" view="grid1" FilterSource="view1Extender" FilterFields="ProjectID" PageSize="5" AutoHide="Container" />
    </div>
  </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="SideBarPlaceHolder" runat="Server">
  <div class="TaskBox">
    <div class="Inner">
      <div class="Header">About</div>
      <div class="Value">This page allows projects management.</div>
    </div>
  </div>
</asp:Content>