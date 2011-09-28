<%@ Page Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="ClientContactSrch.aspx.cs" Inherits="Pages_QuoteSrch"  Title="ClientContact"%>
<%@ Register Src="~/Controls/ClientContactSrch.ascx" TagName="QuoteSrchControl"  TagPrefix="uc"%>
<%@ MasterType VirtualPath="~/Main.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageHeaderContentPlaceHolder" runat="Server">Client Contact</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageContentPlaceHolder" runat="Server">
    <div factory:flow="NewRow" xmlns:factory="urn:codeontime:app-factory">
        <uc:QuoteSrchControl ID="c100" runat="server"></uc:QuoteSrchControl>
    </div>
</asp:Content>
