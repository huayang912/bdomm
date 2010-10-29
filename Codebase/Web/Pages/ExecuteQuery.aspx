<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="ExecuteQuery.aspx.cs" Inherits="Pages_ExecuteQuery" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="PageHeaderContentPlaceHolder" Runat="Server">
    <asp:Literal ID="ltrHeading" runat="server" Text="Execute Query"></asp:Literal>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="SideBarPlaceHolder" Runat="Server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="PageContentPlaceHolder" Runat="Server">
    <asp:Panel ID="pnlLogin" runat="server" CssClass="GroupBox" DefaultButton="btnLogin">
        <div id="divLoginMessage" runat="server" enableviewstate="false" visible="false"></div>
        <div class="floatleft">User Name</div>
        <div class="floatleft" style="margin-left:5px;">
            <asp:TextBox ID="txtUserName" runat="server"></asp:TextBox>            
        </div>
        <div class="floatleft" style="margin-left:20px;">Password</div>
        <div class="floatleft" style="margin-left:5px;">
            <asp:TextBox ID="txtPassword" TextMode="Password" runat="server"></asp:TextBox>
        </div>
        <div class="clearboth"></div>
        <div style="margin-top:10px;">
            <asp:Button ID="btnLogin" runat="server" Text="Log In" OnClick="btnLogin_Click" />
        </div>
    </asp:Panel>
    
    <asp:Panel ID="pnlQuery" runat="server" Visible="false">
        <div class="GroupBox">                        
            <asp:RadioButton ID="rdbSelectQuery" GroupName="MyQuery" runat="server" Text="Select Query" Checked="true" />
            &nbsp;
            <asp:RadioButton ID="rdbExecuteAsCommand" GroupName="MyQuery" runat="server" Text="Execute as DML Command" />
            &nbsp;
            <%--<asp:RadioButton ID="rdbExecuteAsScript" GroupName="MyQuery" runat="server" Text="Execute as SQL Script" />--%>
        </div>
        <div class="GroupBox">
            <asp:TextBox id="txtQuery" TextMode="MultiLine" runat="server" style="height:200px; width:100%;"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvQuery" runat="server"
                ControlToValidate="txtQuery" SetFocusOnError="true" Display="Dynamic"
                ValidationGroup="Execute"
                ErrorMessage="Enter a Query to Execute.">
            </asp:RequiredFieldValidator>
            
            <div style="margin-top:10px;">
                <asp:Button ID="btnExecuteQuery" runat="server" Text="Execute" OnClick="btnExecuteQuery_Click" ValidationGroup="Execute" />            
            </div>
        </div>
        
        <div id="divQueryRestul" runat="server" visible="false" class="GroupBox" style="overflow-x:auto;">
            <div id="divMessage" runat="server" visible="false" enableviewstate="false"></div>
            <asp:GridView ID="gvResult" runat="server" AutoGenerateColumns="true" GridLines="None" 
                CssClass="GridView" Width="100%" CellPadding="3" CellSpacing="0" EnableViewState="false">
                <RowStyle CssClass="OddRowListing" />
                <AlternatingRowStyle CssClass="EvenRowListing" />
            </asp:GridView>
        </div>
    </asp:Panel>
</asp:Content>