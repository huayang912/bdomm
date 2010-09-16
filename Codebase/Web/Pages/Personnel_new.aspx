<%@ Page Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="Personnel_new.aspx.cs" Inherits="Pages_Personnel_new"  Title="Personnel New"%>
<asp:Content ID="Content1" ContentPlaceHolderID="PageHeaderContentPlaceHolder" runat="Server">Personnel New</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageContentPlaceHolder" runat="Server">
  <div factory:flow="NewColumn" xmlns:factory="urn:codeontime:app-factory">
    <div id="dv100" runat="server"></div>
    <aquarium:DataViewExtender id="dv100Extender" runat="server" TargetControlID="dv100" Controller="ContactsTravel" view="" />
    <div id="dv101" runat="server"></div>
    <aquarium:DataViewExtender id="dv101Extender" runat="server" TargetControlID="dv101" Controller="Passports" view="grid1" FilterSource="dv100Extender" FilterFields="ContactID" ShowActionBar="False" ShowDescription="False" ShowViewSelector="False" />
    <div id="dv102" runat="server"></div>
    <aquarium:DataViewExtender id="dv102Extender" runat="server" TargetControlID="dv102" Controller="Visas" view="grid1" FilterSource="dv100Extender" FilterFields="ContactID" ShowActionBar="False" ShowDescription="False" ShowViewSelector="False" />
  </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="SideBarPlaceHolder" runat="Server">
  <div class="TaskBox">
    <div class="Inner">
      <div class="Header">About</div>
      <div class="Value">PErsonnel</div>
    </div>
  </div>
</asp:Content>