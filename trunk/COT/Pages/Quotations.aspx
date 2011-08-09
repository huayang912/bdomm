<%@ Page Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="Quotations.aspx.cs" Inherits="Pages_Quotations"  Title="Quotations"%>
<asp:Content ID="Content1" ContentPlaceHolderID="PageHeaderContentPlaceHolder" runat="Server">Quotations</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageContentPlaceHolder" runat="Server">
  <div factory:flow="NewRow" xmlns:factory="urn:codeontime:app-factory">
    <div id="view1" runat="server"></div>
    <aquarium:DataViewExtender id="view1Extender" runat="server" TargetControlID="view1" Controller="Quotations" view="grid1" ShowInSummary="True" />
  </div>
  <div factory:flow="NewRow" style="padding-top:8px" xmlns:factory="urn:codeontime:app-factory">
    <div factory:activator="Tab|Projects">
      <div id="view2" runat="server"></div>
      <aquarium:DataViewExtender id="view2Extender" runat="server" TargetControlID="view2" Controller="Projects" view="grid1" FilterSource="view1Extender" FilterFields="QuotationID" PageSize="5" AutoHide="Container" ShowModalForms="True" />
    </div>
    <div factory:activator="Tab|Quotation Pricing Lines">
      <div id="view3" runat="server"></div>
      <aquarium:DataViewExtender id="view3Extender" runat="server" TargetControlID="view3" Controller="QuotationPricingLines" view="grid1" FilterSource="view1Extender" FilterFields="QuotationID" PageSize="5" AutoHide="Container" ShowModalForms="True" />
    </div>
  </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="SideBarPlaceHolder" runat="Server">
  <div class="TaskBox About">
    <div class="Inner">
      <div class="Header">About</div>
      <div class="Value">This page allows quotations management.</div>
    </div>
  </div>
</asp:Content>