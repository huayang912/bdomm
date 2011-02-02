<%@ Control Language="VB" AutoEventWireup="true" CodeFile="ClientContactSrch.ascx.vb"
    Inherits="Controls_ClientContactSrch" %>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <table width="100%" border="0">
            <tr>
                <td>
                    <table>
                        <tr>
                            <td>
                                Type:
                                <asp:DropDownList ID="CRM_Type" runat="server">
                                    <asp:ListItem>Show All</asp:ListItem>
                                    <asp:ListItem>All</asp:ListItem>
                                    <asp:ListItem>Procurement</asp:ListItem>
                                    <asp:ListItem>Personnel</asp:ListItem>
                                    <asp:ListItem>O&amp;M</asp:ListItem>
                                    <asp:ListItem>Project</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Button ID="SearchButton" runat="server" Text="Search" OnClick="SearchButton_Click" />
                            </td>
                            <td style="padding-left:15px; padding-top:3px;">
                                <div class="floatleft">|</div>
                                <asp:Repeater ID="rptStartsWith" runat="server" 
                                    onitemdatabound="rptStartsWith_ItemDataBound"
                                    OnItemCommand="rptStartsWith_Command">
                                    <ItemTemplate>
                                        <div class="floatleft">
                                            <%--<asp:Literal ID="ltrStartsWith" runat="server"></asp:Literal>--%>
                                            &nbsp;<asp:LinkButton ID="lkbCommand" CommandName="Filter" runat="server"></asp:LinkButton>&nbsp;| 
                                        </div>
                                    </ItemTemplate>
                                    <%--<AlternatingItemTemplate>
                                        <div class="floatleft">
                                            <asp:Literal ID="ltrStartsWith" runat="server"></asp:Literal>
                                        </div>
                                    </AlternatingItemTemplate>--%>
                                </asp:Repeater>
                                <div style="clear:both;"></div>
                            </td>                            
                        </tr>
                    </table>
                </td>
                <td align="right">
                    <table>
                        <tr>
                            <td>
                                <asp:Button ID="ShowAllButton" runat="server" Text="Show All" OnClick="ShowAllButton_Click" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
<div id="ClientContactList" runat="server">
</div>
<aquarium:DataViewExtender ID="ClientContactListExtender" runat="server" TargetControlID="ClientContactList"
    Controller="ClientContacts" ShowDescription="false" ShowQuickFind="true" ShowViewSelector="false"
    PageSize="25" />
