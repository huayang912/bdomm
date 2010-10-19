<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="monthlyQuotationReport.aspx.cs" Inherits="Reports_monthlyQuotationReport" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageHeaderContentPlaceHolder" Runat="Server">
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
                                <asp:ListItem Text="2005" Value="2005" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="2006" Value="2006"></asp:ListItem>
                                <asp:ListItem Text="2007" Value="2007"></asp:ListItem>
                                <asp:ListItem Text="2008" Value="2008"></asp:ListItem>
                                <asp:ListItem Text="2009" Value="2009"></asp:ListItem>
                                <asp:ListItem Text="2010" Value="2010"></asp:ListItem>
                                <asp:ListItem Text="2011" Value="2011"></asp:ListItem>
                                <asp:ListItem Text="2012" Value="2012"></asp:ListItem>
                                <asp:ListItem Text="2013" Value="2013"></asp:ListItem>
                                <asp:ListItem Text="2014" Value="2014"></asp:ListItem>
                                <asp:ListItem Text="2015" Value="2015"></asp:ListItem>
                                <asp:ListItem Text="2016" Value="2016"></asp:ListItem>
                                
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
                <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true" />
            </td>
        </tr>
    </table>
    </div>
</asp:Content>

