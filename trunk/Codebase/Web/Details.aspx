<%@ Page Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="Details.aspx.cs" Inherits="BUDI2_NS.Handlers.Details" Title="Details"  %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageHeaderContentPlaceHolder" runat="Server">
					Details
				</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageContentPlaceHolder" runat="Server">
  <div id="Div1" runat="server" visible="false" />
  <aquarium:DataViewExtender ID="Extender1" runat="server" TargetControlID="Div1" FilterSource="ExtenderFilter" View="editForm1" ShowActionBar="false" />
  <input type="hidden" name="ExtenderFilter" id="ExtenderFilter" runat="server" />
</asp:Content>