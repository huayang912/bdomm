<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="EnquiryChange.aspx.cs" Inherits="Pages_EnquiryChange" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link href="/Scripts/Main.css" rel="Stylesheet" type="text/css" />
    <link href="/Scripts/ModalPopupStyles.css" rel="Stylesheet" type="text/css" />
    <script language="javascript" src="/Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <style type="text/css">
        .FormHeader
        {
        	height:40px;
        	padding:10px 10px 0px 10px;
        	background-color:#FFFFFF;
        	border-bottom:#B9B9B9 1px solid;
        	margin-bottom:10px;
        }
        textarea
        {
        	width:100%;
        	font-family:Verdana;
        	font-size:12px;
        }
    </style>
    <script language="javascript" type="text/javascript">
        function GetDetails(clientID) {
            //alert(clientID);            
            PageMethods.GetClientContact(clientID, OnGetClientSuccess, OnAjaxFailure);
        }
        function OnGetClientSuccess(result) {
            //alert(result);
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
        var _Enquiry = null; //new App.CustomModels.CustomEnquiry();
        function PrepareObject() {
            _Enquiry = new App.CustomModels.CustomEnquiry();
            _Enquiry.ID = <%=_EnquiryID.ToString() %>;
            _Enquiry.StatusID = <%=_StatusID.ToString() %>;
            _Enquiry.ContactID = $('#<%= ddlContact.ClientID%>').val();
            _Enquiry.TypeID = $('#<%= ddlEnquiryType.ClientID%>').val();
            _Enquiry.Details = $('#<%= txtDetails.ClientID%>').val();            
        }
        function SaveEnquiry() {
            ShowProgress();
            PrepareObject();
            PageMethods.SaveEnquiry(_Enquiry, OnSaveEnquirySuccess, OnAjaxFailure)
        }
        
        function OnSaveEnquirySuccess(result) {
            HideProgress();
            MoveNext(4);
            //var enquiryNumber = result;
            CreateStausLinks(result);
        }
        function CreateStausLinks(enquiryID)
        {
            //alert(enquiryID);
            if(_Enquiry.ID > 0)
                $('#aNewQuation').attr('href', '/').html('View the quotations related to this enquiry');
            else
                $('#aNewQuation').attr('href', '/Pages/QuotationChange.aspx?ENQ=' + enquiryID);
            
        }
        //View the quotations related to this enquiry
        //document.getElementById('aNewQuation').href = '/Pages/QuationChange.aspx?ID=' + 777777;
        
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageHeaderContentPlaceHolder" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="SideBarPlaceHolder" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="PageContentPlaceHolder" Runat="Server">    
    
    <h1 id="h1Heading" runat="server">Create New Enquiry Wizard</h1>
    
    <div id="divMessage" runat="server" enableviewstate="false" visible="false"></div>
    
    <asp:Panel ID="pnlDetails" runat="server">
    
        <div id="divStep1" class="GroupBox" style="height:350px; display:block;">
            <div class="FormHeader">
                <b>Assign Contact</b><br />
                <asp:Label ID="lblStep1Title" Text="Select a Contact to Make this Enquiry" runat="server"></asp:Label>
            </div>
            <table cellpadding="3" cellspacing="0" style="width:60%;">
                <colgroup>
                    <col style="width:35%;" />
                    <col style="width:65%;" />
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
                <%--<tr>
                    <td colspan="2" style="height:100px;">&nbsp;</td>
                </tr>--%>
                <tr>
                    <td colspan="2" style="padding-top:10px;">
                        <input type="button" value="Next >" onclick="MoveNext(2);" />
                    </td>
                </tr>
            </table>        
        </div>
        
        <%--Step 2--%>
        <div id="divStep2" class="GroupBox" style="height:350px; display:none;">
            <div class="FormHeader">
                <b>Select Enquiry Type</b><br />
                <asp:Label ID="lblStep2Title" Text="Select the type of enquiry being made. Note that this cannot be modified once set." runat="server"></asp:Label>
            </div>
            <div>
                <asp:DropDownList ID="ddlEnquiryType" runat="server"></asp:DropDownList>
            </div>
            <div style="height:100px;">&nbsp;</div>
            <div>
                <input type="button" value="< Back" onclick="MoveNext(1);" />&nbsp;
                <input type="button" value="Next >" onclick="MoveNext(3);" />
            </div>
        </div>
        
        <%--Step 3--%>
        <div id="divStep3" class="GroupBox" style="height:350px; display:none;">
            <div class="FormHeader">
                <b>Enter Enquiry Details</b><br />
                <asp:Label ID="lblStep3Title" Text="Enter specific details for this enquiry. Note that text can be pasted from another application into the text box below." runat="server"></asp:Label>
            </div>
            <div>
                <asp:TextBox ID="txtDetails" MaxLength="1000" TextMode="MultiLine" runat="server" style="height:200px;"></asp:TextBox>
            </div>
            <%--<div style="height:100px;">&nbsp;</div>--%>
            <div style="margin-top:10px;">
                <input type="button" value="< Back" onclick="MoveNext(2);" />&nbsp;
                <input type="button" value="Finish" onclick="SaveEnquiry();" />
            </div>
        </div>
        
        <%--Step 4--%>
        <div id="divStep4" class="GroupBox" style="height:350px; display:none;">
            <div class="FormHeader">
                <b>Summary</b><br />
                <asp:Label ID="lblStep4" Text="The enquiry has been successfully created." runat="server"></asp:Label>
            </div>
            <div>
                <a id="aNewQuation" href="javascript:void(0);">Create a new quation now</a>.
            </div>        
        </div>
    
    </asp:Panel>
    
</asp:Content>

