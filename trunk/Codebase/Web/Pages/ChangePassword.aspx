<%@ Page Title="Change Password" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" 
    CodeFile="ChangePassword.aspx.cs" Inherits="Pages_ChangePassword" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

    <script language="javascript" type="text/javascript">
        function MoveNext(stepIndex) {
            for (i = 1; i < 3; i++) {
                document.getElementById('divStep' + i).style.display = 'none';
            }
            document.getElementById('divStep' + stepIndex).style.display = 'block';
        }
        function ChangePassword() {
            if (Page_ClientValidate('SaveInfoStep1')) {
                ShowProgress();
                var oldPassword = $('#<%= txtOldPassword.ClientID %>').val();
                var newPassword = $('#<%= txtNewPassword.ClientID %>').val();
                PageMethods.ChangePassword(newPassword, oldPassword, OnPasswordChangeSuccessfull, OnAjax_Error, OnAjax_TimeOut);
                //alert("Password Changed Successfully");
            }
        }
        function OnPasswordChangeSuccessfull(result) {
            HideProgress();
            var status = eval(result);
            $('#divSummaryMessage').html(status.Message);
            if (status.StatusID == 1)
                $('#divStep2PrevButton').hide();
            else
                $('#divStep2PrevButton').show();
            MoveNext(2);           
        }      
    </script>
    <style type="text/css">
        /*.MessageTextBox
        {
            width:200px;
            height:250px;
            font-size:16px;
            font-family:Verdana;
            border:#A9A9A9 3px solid;
            margin-top:5px;
        }*/
        .Controls
        {
            width:100px;
            float:left;
            margin-bottom:5px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageHeaderContentPlaceHolder" Runat="Server">
    <asp:Literal id="ltrHeading" runat="server">Change Password</asp:Literal>    
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="SideBarPlaceHolder" Runat="Server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="PageContentPlaceHolder" runat="Server">
    <%--<div id="divMessage" runat="server" enableviewstate="false" visible="false">
    </div>--%>
    <asp:Panel ID="pnlMainContainer" runat="server">
        <%--The Message Step1--%>
        <div id="divStep1" class="GroupBox" style="display: block;">
            <div class="FormHeader">
                <b>Change Password</b>
                <div>
                    Give Necessary Information to Change Your Password
                </div>
            </div>
            
            <%--First Row for Old Password--%>
            <div class="Controls">Old Password</div>
            <div class="Controls" style="width:200px;">
                <asp:TextBox ID="txtOldPassword" runat="server" TextMode="Password"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvOldPassword" runat="server"
                    ControlToValidate="txtOldPassword" SetFocusOnError="true" Display="Dynamic"
                    ValidationGroup="SaveInfoStep1"
                    ErrorMessage="<br/>Please give your old Password.">
                </asp:RequiredFieldValidator>
            </div>
            <div class="clearboth"></div>
            
            <%--Second Row for New Password--%>
            <div class="Controls">New Password</div>
            <div class="Controls" style="width:200px;">
                <asp:TextBox ID="txtNewPassword" runat="server" TextMode="Password"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvNewPassword" runat="server"
                    ControlToValidate="txtNewPassword" SetFocusOnError="true" Display="Dynamic"
                    ValidationGroup="SaveInfoStep1"
                    ErrorMessage="<br/>Please give your New Password.">
                </asp:RequiredFieldValidator>
            </div>
            <div class="clearboth"></div>
            
            <%--Third Row for Confirm password--%>
            <div class="Controls">Confirm Password</div>
            <div class="Controls" style="width:200px;">
                <asp:TextBox ID="txtConfirmPassword" runat="server" TextMode="Password"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvConfirmPassword" runat="server"
                    ControlToValidate="txtConfirmPassword" SetFocusOnError="true" Display="Dynamic"
                    ValidationGroup="SaveInfoStep1"
                    ErrorMessage="<br/>Please Confirm Your Password.">
                </asp:RequiredFieldValidator>
                <asp:CompareValidator ID="cvConfirmPassword" runat="server"
                    ControlToValidate="txtConfirmPassword" ControlToCompare="txtNewPassword" Operator="Equal"
                    SetFocusOnError="true" Display="Dynamic"
                    ValidationGroup="SaveInfoStep1"
                    ErrorMessage="<br/>Sorry! Your two passwords do not match.">
                </asp:CompareValidator>
            </div>
            <div class="clearboth"></div>
            
            <%--The Last Row for Buttons--%>
            <div style="margin-top: 15px;">
                <input type="button" value="Change Password" onclick="ChangePassword();" />
            </div>
        </div>
        
        <div id="divStep2" class="GroupBox" style="display:none;">
            <div class="FormHeader">
                <b>Summary</b>                
            </div>
            <div id="divSummaryMessage"></div>
            <div id="divStep2PrevButton" style="margin-top:20px;">
                <input type="button" value="Previous" onclick="MoveNext(1);" />
            </div>
        </div>
    </asp:Panel>
</asp:Content>

