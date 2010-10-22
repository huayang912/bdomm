<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="monthlyEnqueryReport.aspx.cs" Inherits="Reports_monthlyEnqueryReport" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="PageHeaderContentPlaceHolder" Runat="Server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="SideBarPlaceHolder" Runat="Server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="PageContentPlaceHolder" Runat="Server">
    <%--Filter Box--%>
    <div class="GroupBox">
        <div class="floatleft">Select Year:</div>
        <div class="floatleft" style="margin-left:10px; width:120px;">
            <asp:DropDownList ID="ddlYear" runat="server" CssClass="DropDownListCommon"></asp:DropDownList>
        </div>
        <div class="floatleft" style="margin-left:10px;">
            <asp:Button ID="btnShowReport" runat="server" Text="Show Report" onclick="btnShowReport_Click" />
        </div>
        <div class="clearboth"></div>
    </div> 
         
    <%--Crystal Report Viewer Box--%>
    <div id="divReportContainer" runat="server" visible="false" class="GroupBox" style="margin-bottom:0px;">
        <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" 
            AutoDataBind="true" DisplayGroupTree="False" 
            HasToggleGroupTreeButton="False" />
    </div>
</asp:Content>

