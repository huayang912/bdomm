<%@ Page Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="NextOfKin.aspx.cs" Inherits="Pages_NextOfKin"  Title="Next Of Kin"%>
<asp:Content ID="Content1" ContentPlaceHolderID="PageHeaderContentPlaceHolder" runat="Server">Next Of Kin</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageContentPlaceHolder" runat="Server">
  <div factory:flow="NewRow" xmlns:factory="urn:codeontime:app-factory">
    <div id="view1" runat="server"></div>
    <aquarium:DataViewExtender id="view1Extender" runat="server" TargetControlID="view1" Controller="NextOfKin" view="grid1" ShowInSummary="True" />
  </div>
  <div factory:flow="NewRow" style="padding-top:8px" xmlns:factory="urn:codeontime:app-factory"></div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="SideBarPlaceHolder" runat="Server">
  <div class="TaskBox">
    <div class="Inner">
      <div class="Header">About</div>
      <div class="Value">This page allows next of kin management.</div>
    </div>
  </div>
</asp:Content>