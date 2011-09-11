<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="EnquiryDetails.aspx.cs" Inherits="Pages_EnquiryDetails" %>
<%@ MasterType VirtualPath="~/Main.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script language="javascript" type="text/javascript">
        function ShowCloseDialogBox()
        {
            ShowModalPopup('confirm', 'Close Enquiry', 'Are you sure you want to close this enquiry?');
        }
        function CloseEnquery() {
            //if(confirm('Are you sure you want to close this enquiry?'))
            {
                ShowProgress();
                var enqueryID = <%= _EnquiryID %>;
                PageMethods.CloseEnquery(enqueryID, CloseEnquery_Success, OnAjax_Error, OnAjax_TimeOut);
            }
        }
        function CloseEnquery_Success(hasClosed)
        {
            HideProgress();
            if(hasClosed)
            {
                //HideModalPopup();
                $('#popupMessage').html('Enquiry Closed Successfully.');
                //alert('Enquiry Closed Successfully.');
                $('#btnYes').hide();$('#btnNo').hide();$('#btnClosePopup').show(); $('#btnCloseEnquery').attr('disabled', true);
            }
            else
                $('#popupMessage').html('Unable to close the Enquiry. Please try again later.');
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageHeaderContentPlaceHolder" Runat="Server">
    <asp:Literal ID="ltrHeading" runat="server" Text="Enquiry Details"></asp:Literal>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="SideBarPlaceHolder" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="PageContentPlaceHolder" Runat="Server">
    <div id="divMessage" runat="server" enableviewstate="false" visible="false"></div>
    <asp:Panel ID="pnlDetails" runat="server">
        <div style="line-height:18px;">
            <asp:Literal ID="ltrEnquiryDetails" runat="server"></asp:Literal> 
        </div>
        
        <div style="margin-top:30px;">
            <input id="btnCloseEnquery" type="button" value="Close this Enquiry" onclick="ShowCloseDialogBox();" <%= _HasClosed ? "disabled='disabled'" : String.Empty %>/>
        </div>
        
        
        
        <div class="PopupContainer" id="confirm" style="display: none; width:450px;">
            <div>            
                <%--<div class="PopupHeaderMiddle">--%>
                    <div id="popupHeader"><%--The Popup Title Will be shown here--%></div>
                    <%--<div style="float: right;">
                        <img src="/Images/btn/btn_popup_close.gif" onclick="HideConfirmationPopup('confirm');"
                            alt="Close" border="0" align="right" title="Close" style="margin-top: 2px; cursor: pointer;" />
                    </div>--%>                
                    <%--<div class="clearboth"></div>--%>
                <%--</div> --%>           
            </div>
            <div class="PopupBody">
                <span id="popupMessage"><%--The Message will be shown here--%></span>                        
            </div>
            <div id="divPopupButtonContainer" class="PopupButtonContainer">               
                <input id="btnClosePopup" type="button" value="Close" style="padding:2px 5px 2px 5px; margin:5px 15px 5px 0px; display:none;" onclick="HideModalPopup();" />
                
                <input id="btnYes" type="button" value="Yes" style="padding:2px 5px 2px 5px; margin:5px;" onclick="CloseEnquery();" />
                <input id="btnNo" type="button" value="No" style="padding:2px 5px 2px 5px; margin:5px 15px 5px 0px;" onclick="HideModalPopup();" />            
            </div>
        </div>
    </asp:Panel>
</asp:Content>

