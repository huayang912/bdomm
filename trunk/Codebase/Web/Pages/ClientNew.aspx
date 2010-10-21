<%@ Page Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="ClientNew.aspx.cs" Inherits="Pages_ClientNew"  Title="View Clients"%>
<%@ Register Src="~/Controls/ClientControl.ascx" TagName="ClientControl"  TagPrefix="uc"%>
<asp:Content ID="Content1" ContentPlaceHolderID="PageHeaderContentPlaceHolder" runat="Server">View Clients</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageContentPlaceHolder" runat="Server">
  <div factory:flow="NewRow" xmlns:factory="urn:codeontime:app-factory"><uc:ClientControl ID="c100" runat="server"></uc:ClientControl></div>
</asp:Content>