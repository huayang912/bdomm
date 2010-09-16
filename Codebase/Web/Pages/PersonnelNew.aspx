<%@ Page Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="PersonnelNew.aspx.cs" Inherits="Pages_PersonnelNew"  Title="Personnel"%>
<%@ Register Src="~/Controls/PersonnelAdvPageControl.ascx" TagName="PersonnelAdvPageControl"  TagPrefix="uc"%>
<asp:Content ID="Content1" ContentPlaceHolderID="PageHeaderContentPlaceHolder" runat="Server">Personnel</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageContentPlaceHolder" runat="Server">
  <div factory:flow="NewRow" xmlns:factory="urn:codeontime:app-factory"><uc:PersonnelAdvPageControl ID="c100" runat="server"></uc:PersonnelAdvPageControl></div>
</asp:Content>