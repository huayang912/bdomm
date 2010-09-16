<%@ Page Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="CVNew.aspx.cs" Inherits="Pages_CVNew"  Title="Personnel CV"%>
<%@ Register Src="~/Controls/cv_usr_control.ascx" TagName="cv_usr_control"  TagPrefix="uc"%>
<asp:Content ID="Content1" ContentPlaceHolderID="PageHeaderContentPlaceHolder" runat="Server">Personnel CV</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageContentPlaceHolder" runat="Server">
  <div factory:flow="NewRow" xmlns:factory="urn:codeontime:app-factory"><uc:cv_usr_control ID="c100" runat="server"></uc:cv_usr_control></div>
</asp:Content>