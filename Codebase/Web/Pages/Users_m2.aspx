<%@ Page Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="Users_m2.aspx.cs" Inherits="Pages_Users_m2"  Title="Users"%>
<asp:Content ID="Content1" ContentPlaceHolderID="PageHeaderContentPlaceHolder" runat="Server">Users</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageContentPlaceHolder" runat="Server">
  <div factory:flow="NewRow" xmlns:factory="urn:codeontime:app-factory">
    <div id="view1" runat="server"></div>
    <aquarium:DataViewExtender id="view1Extender" runat="server" TargetControlID="view1" Controller="Users" view="grid1" ShowInSummary="True" />
  </div>
 
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="SideBarPlaceHolder" runat="Server">
  <div class="TaskBox">
    <div class="Inner">
      <div class="Header">About</div>
      <div class="Value">This page allows users management.</div>
    </div>
  </div>
</asp:Content>