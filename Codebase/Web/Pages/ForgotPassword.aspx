<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="ForgotPassword.aspx.cs" Inherits="Pages_ForgotPassword" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script language="javascript" type="text/javascript">
        function MoveNext(stepIndex) {
            for (i = 1; i < 2; i++) {
                document.getElementById('divStep' + i).style.display = 'none';
            }
            document.getElementById('divStep' + stepIndex).style.display = 'block';
        }
        function SendPassword() {
            if (Page_ClientValidate('SaveInfoStep1')) {
                ShowProgress();
                PageMethods.SendPassword($('#<%=txtEmail.ClientID %>').val(), OnSendPasswordSuccess, OnAjax_Error, OnAjax_TimeOut);
            }
        }
        function OnSendPasswordSuccess(result) {
            HideProgress();
            if (result == 1)
                $('#divSummary').html('Your password has been sent to <b>' + $('#<%=txtEmail.ClientID %>').val() + '</b>.');
            else if (result == 0)
                $('#divSummary').html('Sorry! unable to send password to your email <b>' + $('#<%=txtEmail.ClientID %>').val() + '</b>.');
            else if (result == -1)
                $('#divSummary').html('Sorry! your email <b>' + $('#<%=txtEmail.ClientID %>').val() + '</b> does not exist.');

            MoveNext(2);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageHeaderContentPlaceHolder" Runat="Server">
    <asp:Literal id="ltrHeading" runat="server">Retrieve Password</asp:Literal>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="SideBarPlaceHolder" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="PageContentPlaceHolder" Runat="Server">
    <div id="divStep1" class="GroupBox" style="display:block; min-height:200px;">
        <div class="FormHeader">
            <b>Enter Email</b>
            <div>Enter the Email that was used to create your user.</div>    
        </div>
        <div>
            <div class="floatleft">Email</div>
            <div class="floatleft" style="margin-left:10px;">
                <asp:TextBox ID="txtEmail" runat="server" MaxLength="100" style="width:170px;"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvMessage" runat="server"
                    ControlToValidate="txtEmail" SetFocusOnError="true" Display="Dynamic"                        
                    ValidationGroup="SaveInfoStep1"
                    ErrorMessage="<br />Please enter an Email.">
                </asp:RequiredFieldValidator>               
                <asp:RegularExpressionValidator id="revEmail" runat="server"
                    ControlToValidate="txtEmail" Display="Dynamic"
                    ValidationExpression=".*@.*\..*" SetFocusOnError="true" 
                    ErrorMessage="<br/>Please enter a valid Email address."
                    ValidationGroup="SaveInfoStep1">
                </asp:RegularExpressionValidator>
            </div>
            <div class="clearboth"></div>
            <div style="margin-top:100px;">
                <input type="button" value="Send Password" onclick="SendPassword();" />
            </div>
        </div>
    </div>
    <div id="divStep2" class="GroupBox" style="display:none; min-height:200px;">
        <div class="FormHeader">
            <b>Summary</b>                
        </div>
        <div id="divSummary">
            
        </div>
    </div>
</asp:Content>

