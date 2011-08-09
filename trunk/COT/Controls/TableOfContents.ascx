<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TableOfContents.ascx.cs" Inherits="Controls_TableOfContents"  %>
            
<div class="ParaInfo">
        ^TocInstruction^Please select a page link in the table of contents below.^TocInstruction^</div>
<div class="ParaHeader">
    ^TocHeader^Site Map^TocHeader^
</div>
<asp:SiteMapDataSource ID="SiteMapDataSource1" runat="server" ShowStartingNode="false" />
<asp:TreeView ID="TreeView1" runat="server" DataSourceID="SiteMapDataSource1" CssClass="TreeView">
</asp:TreeView>
          