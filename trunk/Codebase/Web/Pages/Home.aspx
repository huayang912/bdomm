<%@ Page Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="Home.aspx.cs" Inherits="Pages_Home"  Title="Start"%>
<%@ Register Src="~/Controls/TableOfContents.ascx" TagName="TableOfContents"  TagPrefix="uc"%>
<asp:Content ID="Content1" ContentPlaceHolderID="PageHeaderContentPlaceHolder" runat="Server">Start</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageContentPlaceHolder" runat="Server">
  <div factory:flow="NewRow" xmlns:factory="urn:codeontime:app-factory"><uc:TableOfContents ID="control1" runat="server"></uc:TableOfContents></div>
  <div factory:flow="NewColumn" xmlns:factory="urn:codeontime:app-factory"></div>
</asp:Content>