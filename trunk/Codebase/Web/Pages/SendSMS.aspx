<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" 
    CodeFile="SendSMS.aspx.cs" Inherits="Pages_SendSMS" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

    <script language="javascript" type="text/javascript">
        $(document).ready(function() {
            $("#<%=gvRecipients.ClientID %> th:first").attr('style', 'text-align:center; width:100px;');
            $('#<%=txtMessage.ClientID %>').keyup(function(event) {
                UpdateSMSCharacterCount();
            })
            $('#<%=txtMessage.ClientID %>').change(function(event) {
                UpdateSMSCharacterCount();
            })
            var interval = window.setInterval(function(){UpdateSMSCharacterCount();}, 3000);
        });
        function UpdateSMSCharacterCount() {
            var length = $('#<%=txtMessage.ClientID %>').val().length;            
            $('#divMsgCharacterCount').html('Total ' + length + ' character(s) entered.');
        }               
        function MoveNext(stepIndex) {
            for (i = 1; i < 5; i++) {
                document.getElementById('divStep' + i).style.display = 'none';
            }
            document.getElementById('divStep' + stepIndex).style.display = 'block';
        }
        function SendMessage() {
            if (Page_ClientValidate('SaveInfo2')) {
                ShowProgress();
                var ids = '';
                $("#<%=gvRecipients.ClientID %> input[type='checkbox']:checked").each(function(i) {
                    if (i == 0)
                        ids += $(this).val();
                    else
                        ids += ',' + $(this).val();
                });
                var message = $('#<%=txtMessage.ClientID %>').val();
                PageMethods.SendSms(ids, message, OnSendSms_Success, OnSendSMS_Error, OnSendSMS_TimeOut);
            }
        }
        function OnSendSms_Success(result) {
            HideProgress();
            var process = eval(result);
            var serverReply = '';
            if (process.StatusID > 0)
                serverReply = 'SMS Sent Succesfully to the following Mobile Numbers:<br/><br/>' + process.Message;                
            else
                serverReply = 'Sorry an Error Occurred. Review the following error message.<br/><br/>' + process.Message;            

            $('#divSummary').html(serverReply);
            MoveNext(3);            
        }
        function OnSendSMS_Error(error) {
            HideProgress();
            alert(error.get_message());
        }
        function OnSendSMS_TimeOut() {
            HideProgress();
            alert('Failed! Operation Timeout!');
        }
        /*********************/
        ///My New Methods
        function ValidateAndMoveNext(validationGroup, stepIndex) {
            if (Page_ClientValidate(validationGroup)) {
                MoveNext(stepIndex);
            }
        }
        function ValidateRecipientSelection(sender, args) {
            args.IsValid = false;
            if ($("#<%=gvRecipients.ClientID %> input[type='checkbox']:checked").length > 0)
                args.IsValid = true;
        }
        function SelectDeselectAll(anchor) {
            if ($(anchor).html() == 'Select All') {
                $("#<%=gvRecipients.ClientID %> input[type='checkbox']").each(function(i) {
                    $(this).attr('checked', true);
                });
                $(anchor).html('Deselect All');
            }
            else {
                $("#<%=gvRecipients.ClientID %> input[type='checkbox']").each(function(i) {
                    $(this).attr('checked', false);
                });
                $(anchor).html('Select All');
            }
        }
        function DeleteSelected() {
            $("#<%=gvRecipients.ClientID %> input[type='checkbox']:checked").each(function(i) {
                $(this).parent().parent().remove();                
            });
        }
    </script>
    <style type="text/css">
        .MessageTextBox
        {
            width:200px;
            height:250px;
            font-size:16px;
            font-family:Verdana;
            border:#A9A9A9 3px solid;
            margin-top:5px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageHeaderContentPlaceHolder" Runat="Server">
    <asp:Literal id="ltrHeading" runat="server">Send SMS Wizard</asp:Literal>    
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="SideBarPlaceHolder" Runat="Server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="PageContentPlaceHolder" Runat="Server">   
    <div id="divMessage" runat="server" enableviewstate="false" visible="false"></div>
    
    <asp:Panel ID="pnlMainContainer" runat="server">
        <%--The Message Step1--%>
        <div id="divStep1" class="GroupBox" style="display:none;">
            <div class="FormHeader">
                <b>Create Message</b>
                <div>Create the Message to be Sent</div>
            </div>
            <div>Write Your Message</div>
            <div>
                <div class="floatleft">
                    <asp:TextBox ID="txtMessage" runat="server" TextMode="MultiLine" CssClass="MessageTextBox"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvMessage" runat="server"
                        ControlToValidate="txtMessage" SetFocusOnError="true" Display="Dynamic"                        
                        ValidationGroup="SaveInfoStep1"
                        ErrorMessage="<br />Please write your messsage.">
                    </asp:RequiredFieldValidator>
                </div>
                <div class="floatleft">
                    <div id="divMsgCharacterCount" style="margin:5px 0px 0px 5px; font-weight:bold;"></div>
                </div>
                <div class="clearboth"></div>
            </div>
            <div style="margin-top:5px;">
                <input type="button" value=" Next > " onclick="ValidateAndMoveNext('SaveInfoStep1', 2);" />
            </div>
        </div>
        
        <%--The Telephone Number Listing Step--%>
        <div id="divStep2" class="GroupBox" style="display:block;">
            <div class="FormHeader">
                <b>Review Recipients</b>
                <div>Please review the recipients of the Message.</div>
            </div>
            <div style="margin:0px 0px 5px 3px;">
                <a href="javascript:void(0);" onclick="SelectDeselectAll(this);">Select All</a> &nbsp;
                <a href="javascript:void(0);" onclick="DeleteSelected();">Remove Selected</a>
            </div>
            <asp:GridView ID="gvRecipients" runat="server" CssClass="GridView" 
                AutoGenerateColumns="False" CellPadding="3" CellSpacing="0" GridLines="None">                
                <Columns>
                    <asp:TemplateField HeaderText="Select">
                        <ItemTemplate>
                            <input type="checkbox" name="chkSelect" value="<%# (Eval("ID")).ToString() %>" />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />                        
                    </asp:TemplateField>
                    <asp:BoundField DataField="Number" HeaderText="Destination" ReadOnly="True"/>
                    <asp:BoundField DataField="ID" HeaderText="ID" Visible="false" ReadOnly="True"/>
                    <asp:TemplateField HeaderText="SMS Credit">
                        <ItemTemplate>
                            <asp:Label ID="lblSMSCredit" runat="server"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <RowStyle CssClass="OddRowListing" />
                <AlternatingRowStyle CssClass="EvenRowListing" />
            </asp:GridView>
            <div>
                <asp:CustomValidator ID="cvSelected" runat="server"
                    ClientValidationFunction="ValidateRecipientSelection" Display="Dynamic"
                    ValidationGroup="SaveInfo2"
                    ErrorMessage="Please select recipients to send SMS.">
                </asp:CustomValidator>
            </div>
            <div style="margin-top:15px;">
                <input type="button" value=" < Previous " onclick="MoveNext(1);" />
                <input type="button" value=" Finish " onclick="SendMessage();" />
            </div>
        </div>
        
        <%--Step 3 Start--%>
        <div id="divStep3" class="GroupBox" style="display:none; min-height:200px;">
            <div class="FormHeader">
                <b>Summary</b>                
            </div>
            <div id="divSummary">
                
            </div>
        </div>
        
        <%--Step 4 Start--%>
        <div id="divStep4" class="GroupBox" style="display:none;">
        
        </div>        
    </asp:Panel>
</asp:Content>

