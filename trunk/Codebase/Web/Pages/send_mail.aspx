<%@ Page Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="send_mail.aspx.cs" Inherits="send_mail"  Title="Feedback"%>
<%@ Register Src="~/Controls/send_mail_control.ascx" TagName="send_mail_control"  TagPrefix="uc"%>
<asp:Content ID="Content1" ContentPlaceHolderID="PageHeaderContentPlaceHolder" runat="Server">Feedback</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageContentPlaceHolder" runat="Server">
  <div factory:flow="NewRow" xmlns:factory="urn:codeontime:app-factory"><uc:send_mail_control ID="c100" runat="server"></uc:send_mail_control></div>
</asp:Content>