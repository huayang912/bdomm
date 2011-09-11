<%@ Page Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="QuoteSrch.aspx.cs"
    Inherits="Pages_QuoteSrch" Title="Quotation" %>
<%@ Register Src="~/Controls/QuoteSrchControl.ascx" TagName="QuoteSrchControl" TagPrefix="uc" %>
<%@ MasterType VirtualPath="~/Main.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageHeaderContentPlaceHolder" runat="Server">
    Quotation</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageContentPlaceHolder" runat="Server">
    <div factory:flow="NewRow" xmlns:factory="urn:codeontime:app-factory">
        <uc:QuoteSrchControl ID="c100" runat="server"></uc:QuoteSrchControl>
    </div>
</asp:Content>
