<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="PersonnelChange.aspx.cs" Inherits="Pages_PersonnelChange" %>
<%@ MasterType VirtualPath="~/Main.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    
    <style type="text/css">
        #TabContainer{margin-left:12px;}         
        .TabItem
        {
            padding: 1px 8px 4px 8px;
            border-right: solid 1px #c2dcff;
            border-top: solid 1px #c2dcff;
            background-image: url(../App_Themes/_Shared/TabsInactiveTab.gif);
            border-left: white 1px solid;
            white-space: nowrap;
            cursor: pointer;
            color:#3764A0;                        
        }
        .TabItemSelected
        {
            padding: 1px 8px 4px 8px;
            background-image: url(../App_Themes/_Shared/TabsSelectedTab.gif);
            border-right: solid 1px #79a7e3;
            border-top: solid 1px #79a7e3;
            white-space: nowrap;
            cursor: pointer;            
            text-align:center;
            font-weight:bold;
            color:#0033CC;
        }
        .TabItem:hover, .TabItemSelected:hover
        {
            background-image: url(../App_Themes/_Shared/TabsActiveTab.gif);
            border-right: solid 1px #c2a770;
            border-top: solid 1px #c2a770; 
            color:#000000;
            /*background-image: url(/App_Themes/_Shared/TabsActiveTab.gif);*/
        }               
    </style>
    <script language="javascript" type="text/javascript">
        function ShowLoading() {
            ShowProgress();
        }
        function HideLoading() {
            HideProgress();
        }
        /************************ Scripts For Tab View Start *************************/
        var _CurrentTabIndex = -1;
        function ShowTab(tdElement, tabIndex) {
            if (tabIndex != _CurrentTabIndex) {
                ShowProgress();
                var url = $(tdElement).attr('NavigateUrl');
                SetFrameHeight(0);
                document.getElementById('frmContainer').src = GetFormattedURL(url);
                _CurrentTabIndex = tabIndex;
                ///Remove all Selected Image
                $('#TabContainer td').each(function(i) {
                    $(this).removeClass('TabItemSelected').addClass('TabItem');
                    //$(this).attr('class', className)
                });
                ///Set Selected Tab
                $(tdElement).removeClass('TabItem').addClass('TabItemSelected');
            }
        }
        function GetFormattedURL(url) {
            var rnd = Math.floor(Math.random() * 1001);
            if (url.indexOf('?') > -1)
                url += '&Rnd=' + rnd;
            else
                url += '?Rnd=' + rnd;
            return url;
        }
        /************************ Scripts For Tab View End ***************************/
        
        function SetFrameHeight(height) {            
            if(height > 0)
                HideProgress();                
            document.getElementById('frmContainer').style.height = height + 'px';
        }
        function RefreshPage(newContactID) {
            window.location = '<%=Request.Url.AbsolutePath %>?<%=AppConstants.QueryString.ID %>=' + newContactID + '&Rnd=' + GetRandomNumber();
        }
        function ShowPopupToSendSMS(telephoneNumber) {
            //alert(telephoneNumber);
            $('#divTelephone').html(telephoneNumber);
            ShowModalPopup('divSMSSending', '', '');
        }
        function UpdateSMSCharacterCount() {
            var length = $('#<%=txtMessage.ClientID %>').val().length;
            $('#divMsgCharacterCount').html('Total ' + length + ' character(s) entered.');
        }
        function SendSms() {
            if (Page_ClientValidate('SendMessage')) {
                ShowLoading();
                PageMethods.SendSms($('#divTelephone').html(), $('#<%=txtMessage.ClientID %>').val(), OnSendSms_Success, OnSendSMS_Error, OnSendSMS_TimeOut);
            }
        }
        function OnSendSms_Success(result) {
            HideLoading();
            var process = eval(result);
            var serverReply = '';
            var div = $('#divSendStatusMessage').removeClass('MessageBox ErrorMessageBox');
            if (process.StatusID > 0) {
                $(div).addClass('MessageBox');
                serverReply = 'SMS Sent Succesfully to the following Number:<br/>' + process.Message;
            }
            else {
                $(div).addClass('ErrorMessageBox');
                serverReply = 'Sorry an Error Occurred. Review the following error message.<br/>' + process.Message;
            }

            $(div).show().html(serverReply);
        }
        function OnSendSMS_Error(error) {
            HideLoading();
            alert(error.get_message());
        }
        function OnSendSMS_TimeOut() {
            HideLoading();
            alert('Failed! Operation Timeout!');
        }
        $(document).ready(function() {
            $('#TabContainer td').each(function(i, obj) {
                $(this).click(function() { ShowTab(obj, i); });
            });
            ShowTab($('#TabContainer td:first'), 0);

            $('#<%=txtMessage.ClientID %>').keyup(function(event) {
                UpdateSMSCharacterCount();
            })
            $('#<%=txtMessage.ClientID %>').change(function(event) {
                UpdateSMSCharacterCount();
            })
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageHeaderContentPlaceHolder" Runat="Server">
    <asp:Literal ID="ltrHeading" runat="server">Add Personnel</asp:Literal>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="SideBarPlaceHolder" Runat="Server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="PageContentPlaceHolder" Runat="Server">
    <div id="divMessage" runat="server" visible="false" enableviewstate="false"></div>
    <asp:Panel ID="pnlFormContainer" runat="server">
        <table class="Menu" id="TabContainer" cellpadding="0" cellspacing="0">
            <tr>                
                <td class="TabItemSelected" NavigateUrl="PersonnelBasicInfo.aspx?<%=AppConstants.QueryString.ID %>=<%=_ID.ToString() %>">Basic Info</td>
                <%if (_ID > 0) { %>
                        <td class="TabItemSelected" NavigateUrl="PersonnelSkills.aspx?<%=AppConstants.QueryString.ID %>=<%=_ID.ToString() %>">Skills</td>
       
                <td class="TabItem" NavigateUrl="PersonnelEmploymentHistory.aspx?<%=AppConstants.QueryString.CONTACT_ID %>=<%=_ID.ToString() %>">Employment History</td>
                 <td class="TabItem" NavigateUrl="ContactPlacementHistory.aspx?ContactID=<%=_ID.ToString() %>">Placement History</td>
                
                <td class="TabItem" NavigateUrl="ClientContactSearch.aspx?<%=AppConstants.QueryString.CONTACT_ID %>=<%=_ID.ToString() %>">ClientContactSearch</td>                                
                
                <td class="TabItem" NavigateUrl="PersonnelCertification.aspx?<%=AppConstants.QueryString.CONTACT_ID %>=<%=_ID.ToString() %>">Certification</td>                                
                <td class="TabItem" NavigateUrl="PersonnelNotes.aspx?<%=AppConstants.QueryString.CONTACT_ID %>=<%=_ID.ToString() %>">Notes</td>
                <td class="TabItem" NavigateUrl="PersonnelTravelDetails.aspx?<%=AppConstants.QueryString.CONTACT_ID %>=<%=_ID.ToString() %>">Travel Details</td>
                <td class="TabItem" NavigateUrl="PersonnelNextOfkin.aspx?<%=AppConstants.QueryString.CONTACT_ID %>=<%=_ID.ToString() %>">Next Of Kin</td>
                <td class="TabItem" NavigateUrl="PersonnelBankDetails.aspx?<%=AppConstants.QueryString.CONTACT_ID %>=<%=_ID.ToString() %>">Bank Details</td>
                <td class="TabItem" NavigateUrl="PersonnelCVUpload.aspx?<%=AppConstants.QueryString.ID %>=<%=_ID.ToString() %>">&nbsp;CV&nbsp;</td>
                <% } %>
                
                
            </tr>
        </table>
        
        <%--COT Like Tab Start--%>        
        
        <div class="WinGroupBox" style="min-height:300px; margin-top:0px; padding-top:0px;">
            <iframe id="frmContainer" src="" style="height:350px; width:100%; border:none;" frameborder="0" scrolling="no"></iframe>
        </div>
    </asp:Panel>
    
    <%--Send SMS Modal Popup Start--%>
    <div id="divSMSSending" class="PopupContainer" style="display: none; width:500px; height:auto;">        
        <div class="PopupHeaderMiddle">Send SMS to Personnel</div> 
        <div class="PopupBody" style="padding:10px;">                        
            <%--<img id="imgSubscribeToNewsletterLoading" src="/Images/Loading.gif" alt="" title="" />--%>
            <div>Please Enter the Message to be Sent</div>
            <div>
                <div class="floatleft">
                    <asp:TextBox ID="txtMessage" runat="server" TextMode="MultiLine" CssClass="MessageTextBox" style="height:200px;"></asp:TextBox>                    
                </div>
                <div class="floatright" style="width:55%;">
                    <table class="GridView" cellpadding="3" cellspacing="0" style="width:100%;">
                        <colgroup>
                            <col style="width:50%;"/>
                            <col />
                        </colgroup>
                        <tr>
                            <th>Number</th>
                            <th>SMS Credit</th>
                        </tr>
                        <tr class="OddRowListing">
                            <td><div id="divTelephone"></div></td>
                            <td><div id="divSmsCredit"></div></td>
                        </tr>
                    </table>
                    <div id="divMsgCharacterCount" style="margin:5px 0px 0px 5px; font-weight:bold;"></div>
                    <div id="divSendStatusMessage" class="MessageBox" style="display:none;"></div>
                    <div>
                        <asp:RequiredFieldValidator ID="rfvMessage" runat="server"
                            ControlToValidate="txtMessage" SetFocusOnError="true" Display="Dynamic"                        
                            ValidationGroup="SendMessage"
                            ErrorMessage="<br />Please write a Messsage.">
                        </asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="clearboth"></div>
            </div>            
        </div>
        <div class="PopupButtonContainer">  
            <div class="floatleft">
                <%--<input type="button" value="Verify" class="ButtonInActive" onclick="VerifyPhoneNumber();" />&nbsp;--%>
                <input type="button" value="Send SMS" class="ButtonCommon" onclick="SendSms();" />
            </div>           
            <div class="floatright">
                <input type="button" value="Close" class="ButtonInActive" onclick="HideModalPopup();" />
            </div>
            <div class="clearboth"></div>
        </div>
    </div>
    <%--Send SMS Modal Popup End--%>
</asp:Content>

