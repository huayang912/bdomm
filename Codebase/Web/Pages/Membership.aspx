<%@ Page Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="Membership.aspx.cs" Inherits="Pages_Membership"  Title="Membership Manager"%>
<%@ Register Src="~/Controls/MembershipManager.ascx" TagName="MembershipManager"  TagPrefix="uc"%>
<asp:Content ID="Content1" ContentPlaceHolderID="PageHeaderContentPlaceHolder" runat="Server">Membership Manager</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageContentPlaceHolder" runat="Server">
  <div factory:flow="NewRow" xmlns:factory="urn:codeontime:app-factory"><uc:MembershipManager ID="control1" runat="server"></uc:MembershipManager></div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="SideBarPlaceHolder" runat="Server">
  <div class="TaskBox">
    <div class="Inner">
      <div class="Header">About</div>
      <div class="Value">This page allows to manage roles and users.</div>
    </div>
  </div>
</asp:Content>