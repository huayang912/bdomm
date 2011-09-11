<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="QuotationSubmit.aspx.cs" Inherits="Pages_QuotationDecision" %>
<%@ MasterType VirtualPath="~/Main.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script language="javascript" type="text/javascript">
        var _QuotationID = <%= _QuotationID.ToString()%>;
        var _ClientContactID = <%= _ClientContactID.ToString()%>;
        
        function MoveNext(stepIndex) {
            for (i = 1; i < 3; i++) {
                document.getElementById('divStep' + i).style.display = 'none';
            }
            document.getElementById('divStep' + stepIndex).style.display = 'block';
        }        
        function GetDetails(clientID) {
            if(clientID > 0)
            {
                ShowProgress();            
                _ClientContactID  = clientID
                AjaxService.GetClientContact(_ClientContactID, OnGetClientSuccess, OnAjax_Error, OnAjax_TimeOut);
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
        function SubmitQuotation()
        {
            ShowProgress();            
            AjaxService.SubmitQuotation(_QuotationID, _ClientContactID, OnSubmitQuotationSuccess, OnAjax_Error, OnAjax_TimeOut);
        }
        function OnSubmitQuotationSuccess(result)
        {
            HideProgress();
            var quotationNumber = result;
            var message = 'The quotation has been successfully submitted. It has been assigned number <b>' + quotationNumber + '</b>';
            $('#divSummaryMessage').html(message);       
            MoveNext(2);            
        }        
    </script>    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageHeaderContentPlaceHolder" Runat="Server">
    <asp:Literal id="ltrHeading" runat="server">Submit Quotation Wizard</asp:Literal>
    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
        <Services>
            <asp:ServiceReference Path="~/Services/AjaxService.asmx" />
        </Services>
    </asp:ScriptManagerProxy>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="SideBarPlaceHolder" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="PageContentPlaceHolder" Runat="Server">
    <div id="divMessage" runat="server" enableviewstate="false" visible="false"></div>
    
    <asp:HiddenField ID="hdnQuotationPricings" runat="server" />
    
    <asp:Panel ID="pnlDetails" runat="server">
    
        <%--Step 1--%>
        <div id="divStep1" class="GroupBox" style="display:block;">
            <div class="FormHeader">
                <b>Submit Quotation</b><br />
                <asp:Label ID="lblStep1Title" Text="Select a Contact to whome the quotation will be sent." runat="server"></asp:Label>
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
                        <input type="button" value="Next >" onclick="SubmitQuotation();" />
                    </td>
                </tr>
            </table>        
        </div>
        
        <%--Step 2--%>
        <div id="divStep2" class="GroupBox" style="display:none;">
            <div class="FormHeader">
                <b>Summary</b>
                <%--<div ID="divStep2Title"></div>--%>
            </div>
            <div style="height:100px;">
                <div id="divSummaryMessage"></div>
            </div>
            <%-- Next Previous Buttons --%>
            <div style="margin-top:20px;">
                <%--<input type="button" value="< Back" onclick="MoveNext(2);" />&nbsp;--%>
                <input type="button" value="Close" disabled="disabled" onclick="MovePage();" />
            </div>
        </div>
    
    </asp:Panel>    
    
</asp:Content>

