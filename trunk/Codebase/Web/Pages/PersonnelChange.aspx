<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="PersonnelChange.aspx.cs" Inherits="Pages_PersonnelChange" %>

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
            alert(telephoneNumber);
        }
        $(document).ready(function() {
            $('#TabContainer td').each(function(i, obj) {
                $(this).click(function() { ShowTab(obj, i); });
            });
            ShowTab($('#TabContainer td:first'), 0);
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
                <td class="TabItem" NavigateUrl="PersonnelEmploymentHistory.aspx?<%=AppConstants.QueryString.CONTACT_ID %>=<%=_ID.ToString() %>">Employment History</td>
                <td class="TabItem" NavigateUrl="">Certification</td>                                
                <td class="TabItem" NavigateUrl="PersonnelNotes.aspx?<%=AppConstants.QueryString.CONTACT_ID %>=<%=_ID.ToString() %>">Notes</td>
                <td class="TabItem" NavigateUrl="">Travel Details</td>
                <td class="TabItem" NavigateUrl="">Next Of Kin</td>
                <td class="TabItem" NavigateUrl="">Bank Details</td>
                <td class="TabItem" NavigateUrl="CVUpload.aspx?<%=AppConstants.QueryString.ID %>=<%=_ID.ToString() %>">&nbsp;CV&nbsp;</td>
                <%} %>
            </tr>
        </table>
        
        <%--COT Like Tab Start--%>        
        
        <div class="WinGroupBox" style="min-height:300px; margin-top:0px; padding-top:0px;">
            <iframe id="frmContainer" src="" style="height:350px; width:100%; border:none;" frameborder="0" scrolling="no"></iframe>
        </div>
    </asp:Panel>
</asp:Content>

