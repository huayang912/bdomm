<%@ Page Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="ClientNew.aspx.cs" Inherits="Pages_ClientNew"  Title="View Clients"%>
<%@ Register Src="~/Controls/ClientControl.ascx" TagName="ClientControl"  TagPrefix="uc"%>
<%@ MasterType VirtualPath="~/Main.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageHeaderContentPlaceHolder" runat="Server">View Clients
 <style type="text/css">
        .NewIcon
        {
            display: none;
        }
        
       table.DataView tr.ActionRow td.ActionBar
       {
           border: none;
           background-image : url(ActionBarBg_white.gif);   
           padding: none;
           background-color: white;
       }
        
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageContentPlaceHolder" runat="Server">
  <div factory:flow="NewRow" xmlns:factory="urn:codeontime:app-factory"><uc:ClientControl ID="c100" runat="server"></uc:ClientControl></div>
</asp:Content>