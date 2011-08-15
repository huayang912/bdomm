<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Pager.ascx.cs" Inherits="UserControls_Pager" %>

<div style="font-size:11px;">
    <div id="divPager" runat="server" visible="false" style="float:left;">    
        <div class="PagerButton" style="margin-left:0px;">
            <asp:LinkButton ID="lnkPrevious" runat="server" Text="Previous" OnClick="lnkNextPrevious_Click"></asp:LinkButton>                        
            <asp:Label ID="lblPrevious" Visible="false" runat="server" Text="Previous"></asp:Label>
        </div>

        <asp:Repeater ID="rptPages" runat="server" OnItemCommand="rptPages_ItemCommand" OnItemDataBound="rptPages_ItemDataBound">                
            <ItemTemplate>
                <div id="divPagerContainer" runat="server" class="PagerButton">
                    <asp:LinkButton ID="lnkPage" runat="server" Text='<%# Eval("PageNo") %>' CommandArgument='<%# Eval("PageNo") %>'></asp:LinkButton>                                
                    <asp:Label ID="lblListingCurrentPage" runat="server" Text="" Visible="false"></asp:Label>
                </div>       
            </ItemTemplate>
        </asp:Repeater>

        <div class="PagerButton">
            <asp:LinkButton ID="lnkNext" runat="server" Text="Next" OnClick="lnkNextPrevious_Click"></asp:LinkButton>
            <asp:Label ID="lblNext" Visible="false" runat="server" Text="Next"></asp:Label>
        </div>    
        <asp:Label ID="lblCurrentPage" runat="server" Visible="false"></asp:Label>
        <div class="clearboth"></div>
    </div>
    <div id="divPageCount" runat="server" class="PagerPageCount"></div>
    <div class="clearboth"></div>
    <div id="divTotalRecordMessage" runat="server" class="PagerTotalRecordMessage"></div>
</div>