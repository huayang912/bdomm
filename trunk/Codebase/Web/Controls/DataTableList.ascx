<%@ Control Language="C#" AutoEventWireup="true" CodeFile="DataTableList.ascx.cs" Inherits="UserControls_DataTableList" %>

<div <% if(this.EnableScroller){ %> class="XScrollableListContainer"<%} %>>    
    <asp:Repeater ID="rptList" runat="server" OnItemDataBound="rptList_ItemDataBound">    
        <HeaderTemplate>
            <%--<div style="margin-bottom:5px;">
                <UC:DataExporter ID="ucDataExporter" runat="server" />
            </div>--%>
            <%if (this.ShowSelectionCheckBox){ %>
            <div style="margin:0px 0px 2px 3px;"><a href="javascript:void(0);" onclick="SelectDeselect(this, '<%=this.ListTableClientID %>');">Select All</a></div>        
            <%} %>
            <div <% if(this.EnableScroller){ %>style="overflow-x:auto;"<%} %>>
                <table id="<%=this.ListTableClientID %>" cellpadding="3" cellspacing="0" class="GridView">
                    <tr>
                        <asp:Literal ID="ltrHeader" runat="server"></asp:Literal>
                    </tr>
        </HeaderTemplate>
        <ItemTemplate>
            <tr class="OddRowListing">
                <asp:Literal ID="ltrItems" runat="server"></asp:Literal>
            </tr>
        </ItemTemplate>
        <AlternatingItemTemplate>
            <tr class="EvenRowListing">
                <asp:Literal ID="ltrItems" runat="server"></asp:Literal>
            </tr>
        </AlternatingItemTemplate>
        <FooterTemplate>
                </table>
            </div>
        </FooterTemplate>
    </asp:Repeater>
    <div id="divMessage" runat="server" enableviewstate="false" visible="false"></div>
</div>

