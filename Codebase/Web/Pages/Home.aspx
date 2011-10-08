<%@ Page Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="Home.aspx.cs" Inherits="Pages_Home"  Title="Start"%>
<%@ Register Src="~/Controls/TableOfContents.ascx" TagName="TableOfContents"  TagPrefix="uc"%>
<asp:Content ID="Content1" ContentPlaceHolderID="PageHeaderContentPlaceHolder" runat="Server">Start</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageContentPlaceHolder" runat="Server">
  
    <table width="100%" border="0" background="">
        <tr>
            <td align="right">
                <asp:Label ID="Label1" runat="server" Text="Last Refresh ON  :" Font-Bold="True" 
                        ForeColor="#000099"></asp:Label>
                    <asp:Label ID="lblLastRefreshedOn" runat="server" Text="R" Font-Bold="True" 
                        ForeColor="#339933"></asp:Label>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Label ID="Label2" runat="server" Text="Next Refresh ON :" Font-Bold="True" 
                        ForeColor="#003399"></asp:Label>
                    <asp:Label ID="lblNextRefreshTime" runat="server" Text="F" Font-Bold="True" 
                        ForeColor="#339933"></asp:Label>
            </td>
        </tr>
        <tr>
            <asp:Panel ID="Panel_pie_chart" runat="server" Visible="false">
                <td style="width: 20%">
                    <div class="WinGroupBox">
                        <div class="WinGroupBoxHeader">
                            Pie Chart</div>
                        <div id="divGraph1" runat="server" visible="True">
                        </div>
                    </div>
                </td>
            </asp:Panel>
            
                    
                        
            <td style="width: 100%">
                <div class="WinGroupBox">
                    <div class="WinGroupBoxHeader">
                        <asp:Label ID="lblGraphTitle" runat="server" Text="Label"></asp:Label>
                    </div>
                    <div id="divGraph2" runat="server" visible="True">
                    </div>
                </div>
            </td>
            <asp:Panel ID="Panel_line_chart" runat="server" Visible="false">
                <td style="width: 20%">
                    <div class="WinGroupBox">
                        <div class="WinGroupBoxHeader">
                            Line Chart</div>
                        <div id="divGraph3" runat="server" visible="True">
                        </div>
                    </div>
                </td>
            </asp:Panel>
        </tr>
        <%--<tr>
            <td style="width:25%">
                <div class="WinGroupBox">
                    <div class="WinGroupBoxHeader">
                        Dummy</div>
                     <div id="div1" runat="server" visible="True"></div>   
                    
                </div>
            </td>
            <td style="width:25%">
                <div class="WinGroupBox">
                    <div class="WinGroupBoxHeader">
                        Dummy</div>
                    <div id="div2" runat="server" visible="True"></div>
                </div>
            </td>
            <td style="width:25%">
                <div class="WinGroupBox">
                    <div class="WinGroupBoxHeader">
                        Dummy</div>
                    <div id="div3" runat="server" visible="True"></div>
                </div>
            </td>
            <td style="width:25%">
                <div class="WinGroupBox">
                    <div class="WinGroupBoxHeader">
                        Dummy</div>
                    <div id="div4" runat="server" visible="True"></div>
                </div>
            </td>
        </tr>--%>
    </table>
</asp:Content>
<asp:Content ID="Content3" runat="server" contentplaceholderid="head">
</asp:Content>
