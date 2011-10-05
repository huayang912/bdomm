<%@ Page Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="EnquirySch.aspx.cs" Inherits="Pages_EnquirySch"  Title="Enquiries"%>
<%@ Register Src="~/Controls/EnquirySrch_archive.ascx" TagName="EnquirySrch"  TagPrefix="uc"%>
<%@ MasterType VirtualPath="~/Main.master" %>
<asp:Content ID="Content4" ContentPlaceHolderID="head" Runat="Server">
   
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="PageHeaderContentPlaceHolder" runat="Server">Enquiries Archived</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="PageContentPlaceHolder" runat="Server">
    <div factory:flow="NewRow" xmlns:factory="urn:codeontime:app-factory">
        <uc:EnquirySrch ID="c100" runat="server"></uc:EnquirySrch>  
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="SideBarPlaceHolder" runat="Server">
  <div class="TaskBox">
    <div class="Inner">
      <div class="Header">About</div>
      <div class="Value">Enquiry</div>
    </div>
  </div>
</asp:Content>