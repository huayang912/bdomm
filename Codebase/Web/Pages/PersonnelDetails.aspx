<%@ Page Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="PersonnelDetails.aspx.cs" Inherits="Pages_PersonnelView"  Title="Personnel"%>
<%@ Register Src="~/Controls/PersonnelDetails.ascx" TagName="PersonnelViewControl"  TagPrefix="uc"%>
<asp:Content ID="Content1" ContentPlaceHolderID="PageHeaderContentPlaceHolder" runat="Server">Personnel</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageContentPlaceHolder" runat="Server">
  <div factory:flow="NewRow" xmlns:factory="urn:codeontime:app-factory"><uc:PersonnelViewControl ID="c100" runat="server"></uc:PersonnelViewControl></div>
</asp:Content>