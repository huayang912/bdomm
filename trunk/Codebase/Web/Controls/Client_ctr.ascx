<%@ Control Language="VB" AutoEventWireup="true" CodeFile="Client_Ctr.ascx.vb"
    Inherits="Controls_Client" %>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
       <table><tr><td style="padding-left:15px; padding-top:3px;">
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
       
       </td></tr></table>
       
    </ContentTemplate>
</asp:UpdatePanel>
<div id="ClienttList" runat="server">
</div>
<aquarium:DataViewExtender ID="ClientListExtender" runat="server" TargetControlID="ClienttList"
    Controller="Clients" ShowDescription="false" ShowQuickFind="true" ShowViewSelector="false"
    PageSize="25" />
