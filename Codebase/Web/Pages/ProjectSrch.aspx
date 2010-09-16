<%@ Page Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="ProjectSrch.aspx.cs" Inherits="Pages_ProjectSrch"  Title="Projects"%>
<%@ Register Src="~/Controls/ProjectControl.ascx" TagName="ProjectControl"  TagPrefix="uc"%>
<asp:Content ID="Content1" ContentPlaceHolderID="PageHeaderContentPlaceHolder" runat="Server">Projects</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageContentPlaceHolder" runat="Server">
  <div factory:flow="NewRow" xmlns:factory="urn:codeontime:app-factory"><uc:ProjectControl ID="c100" runat="server"></uc:ProjectControl></div>
</asp:Content>