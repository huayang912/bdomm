<%@ Page Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" Title="Login"  %>
<%--<%@ Register Src="Controls/Login.ascx" TagName="Login" TagPrefix="uc1" %>--%>
<%@ Register Src="Controls/Welcome.ascx" TagName="Welcome" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .TextBoxCommon
        {
        	width:100%;
        }
        table td
        {
        	vertical-align:top;
        }
    </style>
    <script type="text/javascript"><!--
					
        function pageLoad() {
            var inputs = document.getElementsByTagName('input');
            for (var i = 0; i < inputs.length; i++)
                if (inputs[i].id.match(/_UserName/)) {
                inputs[i].focus();
                break;
            }
        }
    
				--></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageHeaderContentPlaceHolder" runat="Server">
    Log In
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="SideBarPlaceHolder" runat="Server" />
<asp:Content ID="Content4" ContentPlaceHolderID="PageContentPlaceHolder" runat="Server">
  <%--<uc1:Login ID="Login1" runat="server" />--%>
  
    <asp:Panel ID="pnlBody" runat="server" DefaultButton="btnLogin" class="SettingsPanel">
        <%--<asp:Login ID="Login1" runat="server" TitleText="" Style="border-collapse: separate;"
            CreateUserText="Sign Up Now" 
            CreateUserUrl="javascript:Web.Membership._instance.signUp();"
            PasswordRecoveryText="Forgot Your Password?" PasswordRecoveryUrl="javascript:Web.Membership._instance.passwordRecovery();">
        </asp:Login>--%>
        <div id="divMessage" runat="server" enableviewstate="false" visible="false"></div>
        <table cellpadding="3" cellspacing="0" style="width:250px;">
            <colgroup>
                <col style="width:60px;" />
                <col />
            </colgroup>
            <tr>
                <td>User Name</td>
                <td>
                    <asp:TextBox ID="txtUserName" CssClass="TextBoxCommon" runat="server" MaxLength="50"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvUserName" runat="server" 
                        ControlToValidate="txtUserName" Display="Dynamic"
                        SetFocusOnError="true"
                        ValidationGroup="SaveInfo"
                        ErrorMessage="Please enter User Name.">
                    </asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td>Password</td>
                <td>
                    <asp:TextBox ID="txtUserPassword" CssClass="TextBoxCommon" TextMode="Password" MaxLength="40" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvUserPassword" runat="server" 
                        ControlToValidate="txtUserPassword" Display="Dynamic"
                        SetFocusOnError="true"
                        ValidationGroup="SaveInfo"
                        ErrorMessage="Please enter Password.">
                    </asp:RequiredFieldValidator>
                </td>
            </tr>            
            <tr>                
                <td colspan="2"><asp:CheckBox ID="chkRememberMe" Text="Remember Me on this Computer"  runat="server" /></td>
            </tr>
            <tr>
                <td colspan="2" style="text-align:right;">
                    <asp:Button ID="btnLogin" Text="Log In" ValidationGroup="SaveInfo" OnClick="btnLogin_Click" runat="server" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <a href="/Pages/ForgotPassword.aspx">Forgot Your Password?</a>
                </td>
            </tr>
        </table>
        
        <div style="width: 300px; margin: 20px -8px; height:700px;">
            <%--<uc1:Welcome ID="Welcome1" runat="server" />--%>
        </div>
    </asp:Panel>
</asp:Content>