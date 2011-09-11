<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="monthlyQuotationReport.aspx.cs" Inherits="Reports_monthlyQuotationReport" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<%@ MasterType VirtualPath="~/Main.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageHeaderContentPlaceHolder" Runat="Server">
    Monthly Quotation
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="SideBarPlaceHolder" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="PageContentPlaceHolder" Runat="Server">

    <div class="GroupBox">
    <table cellpadding="0" cellspacing="0" border="0" width="100%">
        
        <tr>
            <td>
            <div class="GroupBox">
                <table cellpadding="0" cellspacing="0" border="0" width="100%">
                    <tr>
                        <td style="width:10%">Select Year</td>
                        <td style="width:10%">
                            <asp:DropDownList ID="ddlYear" runat="server">
                                
                            </asp:DropDownList>
                        </td>
                        <td >
                            <asp:Button ID="btnShowReport" runat="server" Text="Show Report" 
                                onclick="btnShowReport_Click" />
                        </td>
                    </tr>
                </table>
                </div>
            </td>
        </tr>
        
        <tr>
            <td>
                <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" 
                    AutoDataBind="true" DisplayGroupTree="False" 
                    HasToggleGroupTreeButton="False" />
            </td>
        </tr>
    </table>
    </div>
</asp:Content>

