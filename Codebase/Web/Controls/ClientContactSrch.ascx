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
                                Name Starts With 
                                <asp:DropDownList ID="ddlNameStartsWith" runat="server">
                                    <asp:ListItem Value="" Text=""></asp:ListItem>
                                    <asp:ListItem Value="A" Text="A"></asp:ListItem>
                                    <asp:ListItem Value="B" Text="B"></asp:ListItem>
                                    <asp:ListItem Value="C" Text="C"></asp:ListItem>
                                    <asp:ListItem Value="D" Text="D"></asp:ListItem>
                                    <asp:ListItem Value="E" Text="E"></asp:ListItem>
                                    <asp:ListItem Value="F" Text="F"></asp:ListItem>
                                    <asp:ListItem Value="G" Text="G"></asp:ListItem>
                                    <asp:ListItem Value="H" Text="H"></asp:ListItem>
                                    <asp:ListItem Value="I" Text="I"></asp:ListItem>
                                    <asp:ListItem Value="J" Text="J"></asp:ListItem>
                                    <asp:ListItem Value="K" Text="K"></asp:ListItem>
                                    <asp:ListItem Value="L" Text="L"></asp:ListItem>
                                    <asp:ListItem Value="M" Text="M"></asp:ListItem>
                                    <asp:ListItem Value="N" Text="N"></asp:ListItem>
                                    <asp:ListItem Value="O" Text="O"></asp:ListItem>
                                    <asp:ListItem Value="P" Text="P"></asp:ListItem>
                                    <asp:ListItem Value="Q" Text="Q"></asp:ListItem>
                                    <asp:ListItem Value="R" Text="R"></asp:ListItem>
                                    <asp:ListItem Value="S" Text="S"></asp:ListItem>
                                    <asp:ListItem Value="T" Text="T"></asp:ListItem>
                                    <asp:ListItem Value="U" Text="U"></asp:ListItem>
                                    <asp:ListItem Value="V" Text="V"></asp:ListItem>
                                    <asp:ListItem Value="W" Text="W"></asp:ListItem>
                                    <asp:ListItem Value="X" Text="X"></asp:ListItem>
                                    <asp:ListItem Value="Y" Text="Y"></asp:ListItem>
                                    <asp:ListItem Value="Z" Text="Z"></asp:ListItem>                                    
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Button ID="SearchButton" runat="server" Text="Search" OnClick="SearchButton_Click" />
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
