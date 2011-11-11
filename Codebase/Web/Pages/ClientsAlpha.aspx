<%@ Page Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="ClientsAlpha.aspx.cs" Inherits="Pages_Clients"  Title="Clients"%>
<%@ Register Src="~/Controls/Client_ctr.ascx" TagName="QuoteSrchControl"  TagPrefix="uc"%>
<%@ MasterType VirtualPath="~/Main.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageHeaderContentPlaceHolder" runat="Server">Clients</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageContentPlaceHolder" runat="Server">
    <div factory:flow="NewRow" xmlns:factory="urn:codeontime:app-factory">
        <uc:QuoteSrchControl ID="c10001" runat="server"></uc:QuoteSrchControl>
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