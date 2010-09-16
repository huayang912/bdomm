<%@ Page Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="PersonnelAdd.aspx.cs" Inherits="Pages_PersonnelAdd"  Title="Add New Personnel"%>
<%@ Register Src="~/Controls/AddNewPersonnelControl.ascx" TagName="AddNewPersonnelControl"  TagPrefix="uc"%>
<asp:Content ID="Content1" ContentPlaceHolderID="PageHeaderContentPlaceHolder" runat="Server">Add New Personnel</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageContentPlaceHolder" runat="Server">
  <div factory:flow="NewRow" xmlns:factory="urn:codeontime:app-factory"><uc:AddNewPersonnelControl ID="c100" runat="server"></uc:AddNewPersonnelControl></div>
</asp:Content>