<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="ProjectDetails.aspx.cs" Inherits="Pages_ProjectDetails" %>
<%@ Register Src="~/Controls/ProjectNotes.ascx" TagName="ProjectNotes" TagPrefix="UC" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        table th
        {
        	text-align:left;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="PageHeaderContentPlaceHolder" Runat="Server">
    <asp:Literal ID="ltrHeading" runat="server" Text="Project Details"></asp:Literal>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="SideBarPlaceHolder" Runat="Server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="PageContentPlaceHolder" Runat="Server">
    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
        <Services>
            <asp:ServiceReference Path="~/Services/AjaxService.asmx" />
        </Services>
    </asp:ScriptManagerProxy>
    <div class="GroupBox" style="margin-bottom:0px;">
        <asp:Panel ID="pnlDetails" runat="server" Width="70%">
            <div id="divDetails" runat="server" style="line-height:17px;"></div>
            <%--Project Notes--%>            
            <div>
                <div style="margin-bottom:5px;"><b>Project Notes:</b></div>
                <UC:ProjectNotes ID="ucProjectNotes" runat="server" />
            </div>
        </asp:Panel>
    </div> 
</asp:Content>

