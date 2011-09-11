<%@ Page Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="QuotationPricingTypes.aspx.cs" Inherits="Pages_QuotationPricingTypes"  Title="Quotation Pricing Types"%>
<%@ MasterType VirtualPath="~/Main.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageHeaderContentPlaceHolder" runat="Server">Quotation Pricing Types</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageContentPlaceHolder" runat="Server">
  <div factory:flow="NewRow" xmlns:factory="urn:codeontime:app-factory">
    <div id="view1" runat="server"></div>
    <aquarium:DataViewExtender id="view1Extender" runat="server" TargetControlID="view1" Controller="QuotationPricingTypes" view="grid1" ShowInSummary="True" />
  </div>
  <div factory:flow="NewRow" style="padding-top:8px" xmlns:factory="urn:codeontime:app-factory">
    <div factory:activator="Tab|Quotation Pricing Lines">
      <div id="view2" runat="server"></div>
      <aquarium:DataViewExtender id="view2Extender" runat="server" TargetControlID="view2" Controller="QuotationPricingLines" view="grid1" FilterSource="view1Extender" FilterFields="PricingTypeID" PageSize="5" AutoHide="Container" />
    </div>
  </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="SideBarPlaceHolder" runat="Server">
  <div class="TaskBox">
    <div class="Inner">
      <div class="Header">About</div>
      <div class="Value">This page allows quotation pricing types management.</div>
    </div>
  </div>
</asp:Content>