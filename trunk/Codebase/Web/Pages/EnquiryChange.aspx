<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="EnquiryChange.aspx.cs" Inherits="Pages_EnquiryChange" %>
<%@ MasterType VirtualPath="~/Main.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">    
    
    <script language="javascript" type="text/javascript">
        var _ImgElement = null;
        var _FileID = 0;
        function DeleteAttachment(fileId, imgElement)
        {        
            _ImgElement = imgElement;    
            _FileID = fileId;
            if(fileId > 0)
            {
                ShowProgress();                                
                PageMethods.DeleteEnquiryAttachment(fileId, '', OnAttachmentDelete, OnAjax_Error, OnAjax_TimeOut)
            }
            else
            {
                $(imgElement).parent().remove();
                var filePath = $(imgElement).parent().find('a').attr('href');
                //alert(filePath);
                PageMethods.DeleteEnquiryAttachment(fileId, filePath, OnAttachmentDelete, OnAjax_Error, OnAjax_TimeOut);
            }
        }
        function OnAttachmentDelete(hasDeleted)
        {
            HideProgress();
            if(_FileID  > 0 && hasDeleted == true)
                $(_ImgElement).parent().remove();            
        }
        function GetDetails(clientID) {
            if(clientID > 0)
            {
                ShowProgress();
                PageMethods.GetClientContact(clientID, OnGetClientSuccess, OnAjaxFailure);
            }
        }
        function OnGetClientSuccess(result) {
            HideProgress();
            var contact = eval(result);
            document.getElementById('<%= txtClientName.ClientID%>').value = contact.ClientName;
            document.getElementById('<%= txtContactName.ClientID%>').value = contact.ContactName;
            document.getElementById('<%= txtJobTitle.ClientID%>').value = contact.JobTitle;
            document.getElementById('<%= txtCountry.ClientID%>').value = contact.Country;
        }
        function OnAjaxFailure(error) {
            HideProgress();
            alert(error.get_message());
        }
        function MoveNext(stepIndex) {
            for(i = 1; i < 5; i++)
            {
                document.getElementById('divStep' + i).style.display = 'none';
            }
            document.getElementById('divStep' + stepIndex).style.display = 'block';
        }
        function ValidateAndMoveNext(validationGroup, stepID)
        {
            if(Page_ClientValidate(validationGroup))
            {
                MoveNext(stepID);
            }
        }
        var _Enquiry = null; //new App.CustomModels.CustomEnquiry();
        function PrepareObject() {
            _Enquiry = new App.CustomModels.CustomEnquiry();
            _Enquiry.ID = <%=_EnquiryID.ToString() %>;
            _Enquiry.StatusID = <%=_StatusID.ToString() %>;
            _Enquiry.ContactID = $('#<%= ddlContact.ClientID%>').val();
            _Enquiry.TypeID = $('#<%= ddlEnquiryType.ClientID%>').val();
            _Enquiry.SourceTypeID = $('#<%= ddlEnquirySourceTypes.ClientID%>').val();
            _Enquiry.EnguirySubject = $('#<%= txtEnguirySubject.ClientID%>').val();
            _Enquiry.Details = $('#<%= txtDetails.ClientID%>').val();            
        }
        function SaveEnquiry() {
            if(Page_ClientValidate('SaveInfo3'))
            {
                ShowProgress();
                PrepareObject();
                PageMethods.SaveEnquiry(_Enquiry, OnSaveEnquirySuccess, OnAjaxFailure)
            }
        }
        
        function OnSaveEnquirySuccess(result) {
            HideProgress();
            MoveNext(4);
            //alert(result);
            //var enquiry = eval(result);
            var enquiryID = result.split(':')[0];
            var enquiryNo = result.split(':')[1];
            CreateStausLinks(enquiryID, enquiryNo);
        }
        function CreateStausLinks(enquiryID, enquiryNo)
        {
            //alert(_Enquiry.ID + 'ID: ' + enquiryID + 'No: ' + enquiryNo);
            if(_Enquiry.ID > 0)
            {                
                $('#aNewQuation').attr('href', 'QuotationList.aspx?ENQ=' + enquiryID).html('View the quotations related to this enquiry');
            }
            else
            {
                $('#<%=lblStep4Message.ClientID %>').html('Enquiry <b>' + enquiryNo + '</b> has been successfully created.');
                $('#aNewQuation').attr('href', 'QuotationChange.aspx?<%=AppConstants.QueryString.ENQUIRY_ID %>=' + enquiryID);
            }            
        }
        function AddAttachmentLink(fileName)
        {
            var element = '<li><a href="..<%=AppConstants.TEMP_DIRECTORY %>/' + fileName + '" target="_blank">' + fileName + '</a> <img onclick="DeleteAttachment(0, this)" src="../Images/delete.png" style="cursor:pointer;" alt="Delete" title="Delete"/></li>';
            $('#ulAttachedFiles li:last').after(element);
        }
        //View the quotations related to this enquiry
        //document.getElementById('aNewQuation').href = '/Pages/QuationChange.aspx?ID=' + 777777;
        
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="PageHeaderContentPlaceHolder" Runat="Server">
    <asp:Literal id="ltrHeading" runat="server">Create New Enquiry Wizard</asp:Literal>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="SideBarPlaceHolder" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="PageContentPlaceHolder" Runat="Server">
    
    <div id="divMessage" runat="server" enableviewstate="false" visible="false"></div>
    
    <asp:Panel ID="pnlDetails" runat="server">
    
        <div id="divStep1" class="GroupBox" style="display:block;">
            <div class="FormHeader">
                <b>Assign Contact</b><br />
                <asp:Label ID="lblStep1Title" Text="Select a Contact to Make this Enquiry" runat="server"></asp:Label>
            </div>
            <table cellpadding="3" cellspacing="0" class="FormTable">
                <colgroup>
                    <col style="width:15%;" />
                    <col style="width:85%;" />
                </colgroup>
                <tr>
                    <td>Select Contact</td>
                    <td><asp:DropDownList ID="ddlContact" runat="server" onchange="GetDetails(this.value);"></asp:DropDownList></td>
                </tr>
                <tr>
                    <td>Contact Name</td>
                    <td><asp:TextBox ID="txtContactName" ReadOnly="true" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>Job Title</td>
                    <td><asp:TextBox ID="txtJobTitle" ReadOnly="true" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>Client Name</td>
                    <td><asp:TextBox ID="txtClientName" ReadOnly="true" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>Country</td>
                    <td><asp:TextBox ID="txtCountry" ReadOnly="true" runat="server"></asp:TextBox></td>
                </tr>                
                <tr>
                    <td colspan="2" style="padding-top:10px;">
                        <input type="button" value="Next >" onclick="MoveNext(2);" />
                    </td>
                </tr>
            </table>        
        </div>
        
        <%--Step 2--%>
        <div id="divStep2" class="GroupBox" style="display:none;">
            <div class="FormHeader">
                <b>Select Enquiry Type</b><br />
                <asp:Label ID="lblStep2Title" Text="Select the type of enquiry being made. Note that this cannot be modified once set." runat="server"></asp:Label>
            </div>
            <div class="floatleft">
                <div>
                    <asp:DropDownList ID="ddlEnquiryType" runat="server"></asp:DropDownList>
                </div>
                <div style="margin:10px 0px 5px 0px;">
                    <b>Select Enquiry Source </b><br />
                    <asp:Label ID="Label11" Text="Select the  enquiry source. Note that this cannot be modified once set." runat="server"></asp:Label>
                </div>
                <div>
                     <asp:DropDownList ID="ddlEnquirySourceTypes" runat="server"></asp:DropDownList>
                </div> 
                <div style="margin:10px 0px 5px 0px;">
                    Subject 
                </div>           
                <div>
                    <asp:TextBox ID="txtEnguirySubject" runat="server" TextMode="SingleLine" MaxLength="100" style="width:200px; "></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvEnguirySubject" runat="server"
                        ControlToValidate="txtEnguirySubject" SetFocusOnError="true" Display="Dynamic"
                        ValidationGroup="SaveInfo2"
                        ErrorMessage="<br/>Please Enter Enquiry Subject.">
                    </asp:RequiredFieldValidator>
                </div>
            </div>
			
			<!--
            <div class="floatleft" style="margin-left:20px;">
                <div style="margin:10px 0px 5px 0px;"><b>Attach File(s)</b></div>
                <div>
                    <ul id="ulAttachedFiles">
                        <li style="display:none;">&nbsp;</li>
						                        <asp:Literal ID="ltrAttachmentList" runat="server" Text=""></asp:Literal>
                  
				    </ul>
                </div>
                <div>
				
                    <a href="javascript:void(0);" onclick="ShowCenteredPopUp('EnquiryFiles.aspx?ID=<%=_EnquiryID %>', 'EnquiryAttachment', 500, 330, false);">Attach Document</a>
                </div>
				
            </div>
			-->
            <div class="clearboth"></div>
            <div style="margin-top:30px;">
                <input type="button" value="< Back" onclick="MoveNext(1);" />&nbsp;
                <input type="button" value="Next >" onclick="ValidateAndMoveNext('SaveInfo2', 3);" />
            </div>
        </div>
        
        <%--Step 3--%>
        <div id="divStep3" class="GroupBox" style="display:none;">
            <div class="FormHeader">
                <b>Enter Enquiry Details</b><br />
                <asp:Label ID="lblStep3Title" Text="Enter specific details for this enquiry. Note that text can be pasted from another application into the text box below." runat="server"></asp:Label>
            </div>
            <div>
                <asp:TextBox ID="txtDetails" MaxLength="1000" TextMode="MultiLine" runat="server" style="height:200px;"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvDetails" runat="server"
                    SetFocusOnError="true" Display="Dynamic"
                    ControlToValidate="txtDetails" ValidationGroup="SaveInfo3"
                    ErrorMessage="Please Enter Enquiry Details.">                    
                </asp:RequiredFieldValidator>
            </div>
            <%--<div style="height:100px;">&nbsp;</div>--%>
            <div style="margin-top:10px;">
                <input type="button" value="< Back" onclick="MoveNext(2);" />&nbsp;
                <input type="button" value="Finish" onclick="SaveEnquiry();" />
            </div>
        </div>
        
        <%--Step 4--%>
        <div id="divStep4" class="GroupBox" style="display:none;">
            <div class="FormHeader">
                <b>Summary</b><br />
                <asp:Label ID="lblStep4Message" Text="Enquiry information Saved." runat="server"></asp:Label>
            </div>
            <div style="min-height:100px;">
                <a id="aNewQuation" href="javascript:void(0);">Create a new quotation now</a>.
            </div>        
        </div>
    
    </asp:Panel>
    
</asp:Content>

