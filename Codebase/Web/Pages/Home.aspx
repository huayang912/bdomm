<%@ Page Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="Home.aspx.cs" Inherits="Pages_Home"  Title="Start"%>
<%@ Register Src="~/Controls/TableOfContents.ascx" TagName="TableOfContents"  TagPrefix="uc"%>
<asp:Content ID="Content1" ContentPlaceHolderID="PageHeaderContentPlaceHolder" runat="Server">Start</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageContentPlaceHolder" runat="Server">
  
    <table width="100%" border="0" background="">
        <tr>
            <td style="width:33%">
                <div class="WinGroupBox">
                    <div class="WinGroupBoxHeader">
                        Total Enquiry Posted</div>
                     <div id="divGraph1" runat="server" visible="True"></div>   
                    
                </div>
            </td>
            <td style="width:33%">
                <div class="WinGroupBox">
                    <div class="WinGroupBoxHeader">
                        Total Quotation Posted</div>
                    <div id="divGraph2" runat="server" visible="True"></div>
                </div>
            </td>
            <td>
                <div class="WinGroupBox">
                    <div class="WinGroupBoxHeader">
                        Total Quotation Successful</div>
                    <div id="divGraph3" runat="server" visible="True"></div>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content3" runat="server" contentplaceholderid="head">
</asp:Content>
