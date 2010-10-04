<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="QuotationDecision.aspx.cs" Inherits="Pages_QuotationDecision" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script language="javascript" type="text/javascript">
        var _QuotationID = <%= _QuotationID.ToString()%>;
        var _EnquiryID = <%= _EnquiryID.ToString() %>;
        var _Decision = null;
        
        function MoveNext(stepIndex) {
            for (i = 1; i < 3; i++) {
                document.getElementById('divStep' + i).style.display = 'none';
            }
            document.getElementById('divStep' + stepIndex).style.display = 'block';
        }
        function ValidateDecisionSelection(sender, args) {
            if($("#<%= rdbDecisions.ClientID%> input[type='radio']:checked").length > 0)
                args.IsValid = true;
            else
                args.IsValid = false;

            //alert(args.IsValid);
        }
        function SaveDecision() {
            if (Page_ClientValidate('SaveInfo')) {
                ShowProgress();
                _Decision = $("#<%= rdbDecisions.ClientID%> input[type='radio']:checked").val();
                PageMethods.SaveDecision(_QuotationID, _Decision, OnSaveDecisionSuccess, OnAjax_Error, OnAjax_TimeOut);                
            } 
        }
        function OnSaveDecisionSuccess(result)
        {
            HideProgress();
            if(result.length > 0)
            {
                MoveNext(2);
                if(_Decision == 4 || _Decision == 3) ///Successfull OR Not Successfull
                {
                    $('#divDecisionMessage').html('The quotation has been updated and the enquiry has been closed.');
                    $('#divCheckContainer').hide();
                }
                else if(_Decision == 5) ///Requote
                {
                    $('#divDecisionMessage').html('The quotation has been updated, and a new quotation based on this one has been created.');
                    $('#divCheckContainer').show();                    
                }                                
            }
        }
        function MovePage()
        {
            if(_Decision == 5) ///Requote
            {
                //$('#divDecisionMessage').html('The quotation has been updated, and a new quotation based on this one has been created.');
                //$('#divCheckContainer').show();
                if($('#<%=chkRevisedQuotation.ClientID %>').is(':checked')) 
                    window.location = '<%=AppConstants.Pages.QUOTATION_CHANGE + "?" + AppConstants.QueryString.ID %>=' + _QuotationID + '&<%=String.Format("{0}={1}", AppConstants.QueryString.ENQUIRY_ID, _EnquiryID) %>&Rnd=' + GetRandomNumber();
            }                        
        }
    </script>    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageHeaderContentPlaceHolder" Runat="Server">
    <asp:Literal id="ltrHeading" runat="server">Quotation Decision Wizard</asp:Literal>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="SideBarPlaceHolder" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="PageContentPlaceHolder" Runat="Server">
    <div id="divMessage" runat="server" enableviewstate="false" visible="false"></div>
    
    <asp:HiddenField ID="hdnQuotationPricings" runat="server" />
    
    <asp:Panel ID="pnlDetails" runat="server">
    
        <div id="divStep1" class="GroupBox" style="display:block;">
            <div class="FormHeader">
                <b>Quotation Decision</b><br />
                <asp:Label ID="lblStep1Title" Text="Specify the outcome of this quotation." runat="server"></asp:Label>
            </div>
            <table cellpadding="3" cellspacing="0" class="FormTable">
                <colgroup>
                    <%--<col style="width:15%;" />--%>
                    <col/>
                </colgroup>		
				<tr>					
					<td style="height:120px; vertical-align:top;">
						<asp:RadioButtonList ID="rdbDecisions" CellPadding="3" CellSpacing="0" RepeatDirection="Vertical" runat="server">
						    <asp:ListItem Text="The quotation was successful" Value="4"></asp:ListItem>
						    <asp:ListItem Text="The customer has requested a revised quotation" Value="5"></asp:ListItem>
						    <asp:ListItem Text="The quotation was not successful" Value="3"></asp:ListItem>
						</asp:RadioButtonList>
						<%--<asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" 
                            ControlToValidate="txtDecisionDate" SetFocusOnError="true" 
                            ErrorMessage="<br/>Please Select Decision Date." Display="Dynamic"
                            ValidationGroup="SaveInfo1">
                        </asp:RequiredFieldValidator> --%>                       
                        <asp:CustomValidator ID="CustomValidator3" runat="server" 
                            Display="Dynamic" 
                            ClientValidationFunction="ValidateDecisionSelection"
                            ErrorMessage="Select a outcome of this quotation."
                            ValidationGroup="SaveInfo">
                        </asp:CustomValidator>
					</td>
				</tr>				                
            </table> 
            <div>
                <%--<input type="button" value="< Back" onclick="MoveNext(1);" />&nbsp;--%>
                <input type="button" value="Finish" onclick="SaveDecision();" />
            </div>      
        </div>
        
        <%--Step 3--%>
        <div id="divStep2" class="GroupBox" style="display:none; min-height:120px;">
            <div class="FormHeader">
                <b>Summary</b>
                <%--<div ID="divStep2Title"></div>--%>
            </div>
            <div>
                <div id="divDecisionMessage"></div>
                
                <div id="divCheckContainer" style="margin-top:15px;">
                    <asp:CheckBox ID="chkRevisedQuotation" runat="server" Text="View Revised Quotation Now." />
                </div>                
            </div>
            <%-- Next Previous Buttons --%>
            <div style="margin-top:20px;">
                <%--<input type="button" value="< Back" onclick="MoveNext(2);" />&nbsp;--%>
                <input type="button" value="Close" onclick="MovePage();" />
            </div>
        </div>
    
    </asp:Panel>    
    
</asp:Content>

