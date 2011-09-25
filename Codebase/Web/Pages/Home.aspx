<%@ Page Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="Home.aspx.cs" Inherits="Pages_Home"  Title="Start"%>
<%@ Register Src="~/Controls/TableOfContents.ascx" TagName="TableOfContents"  TagPrefix="uc"%>
<asp:Content ID="Content1" ContentPlaceHolderID="PageHeaderContentPlaceHolder" runat="Server">Start</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageContentPlaceHolder" runat="Server">
  
    <table width="100%" border="0" background="">
        <tr >
            <td style="width:20%">
                <div class="WinGroupBox">
                    <div class="WinGroupBoxHeader">
                        Total Enquiry Posted</div>
                     <div id="divGraph1" runat="server" visible="True"></div>   
                    
                </div>
            </td>
            <td style="width:20%">
                <div class="WinGroupBox">
                    <div class="WinGroupBoxHeader">
                        Total Quotation Posted</div>
                    <div id="divGraph2" runat="server" visible="True"></div>
                </div>
            </td>
            <td style="width:20%">
                <div class="WinGroupBox">
                    <div class="WinGroupBoxHeader">
                        Total Quotation Successful</div>
                    <div id="divGraph3" runat="server" visible="True"></div>
                </div>
            </td>
            <td style="width:20%">
                <div class="WinGroupBox">
                    <div class="WinGroupBoxHeader">
                        Dummy</div>
                    <div id="divGraph4" runat="server" visible="True"></div>
                </div>
            </td>
            <td style="width:20%">
                <div class="WinGroupBox">
                    <div class="WinGroupBoxHeader">
                        Dummy</div>
                    <div id="divGraph5" runat="server" visible="True"></div>
                </div>
            </td>
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
