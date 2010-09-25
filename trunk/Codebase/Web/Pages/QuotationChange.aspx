<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="QuotationChange.aspx.cs" Inherits="Pages_QuotationChange" %>
<%@ Register Src="~/Controls/jQueryCalendar.ascx" TagName="jQueryCalendar" TagPrefix="UC"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <UC:jQueryCalendar id="ucjQueryCalendar" runat="server"></UC:jQueryCalendar>
    
    <script language="javascript" type="text/javascript">
        var _PricingList = new Array();
        
        function MoveNext(stepIndex) {
            for (i = 1; i < 5; i++) {
                document.getElementById('divStep' + i).style.display = 'none';
            }
            document.getElementById('divStep' + stepIndex).style.display = 'block';
        }
        function ValidateAndMoveNext(validationGroup, stepID)
        {
            if (Page_ClientValidate(validationGroup))
                MoveNext(stepID);
        }
        function ValidateAndBindPricingList() {
            if (Page_ClientValidate('SaveInfo2')) {
                //var         
                //MoveNext(3);
            }
        }
        function ShowPricingForm() {
        }
    </script>
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageHeaderContentPlaceHolder" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="SideBarPlaceHolder" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="PageContentPlaceHolder" Runat="Server">
    <h1 id="h1Heading" runat="server">Create New Quotation Wizard</h1>
    
    
    <div id="divMessage" runat="server" enableviewstate="false" visible="false"></div>
    
    <asp:Panel ID="pnlDetails" runat="server">
    
        <div id="divStep1" class="GroupBox" style="display:block;">
            <div class="FormHeader">
                <b>Enter Quotation Details - Step 1 of 2</b><br />
                <asp:Label ID="lblStep1Title" Text="Enter the Main Quotation Details Below" runat="server"></asp:Label>
            </div>
            <table cellpadding="3" cellspacing="0" class="FormTable">
                <colgroup>
                    <col style="width:15%;" />
                    <col style="width:85%;" />
                </colgroup>
				<tr>
					<td>Subcontractor(s)</td>
					<td>
						<asp:TextBox ID="txtSubcontractor" MaxLength="50" runat="server" style="width:100%;"></asp:TextBox>
						<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
							ControlToValidate="txtSubcontractor" SetFocusOnError="true"
							ErrorMessage="Please Enter Subcontractor(s)." Display="Dynamic"
							ValidationGroup="SaveInfo1">
						</asp:RequiredFieldValidator>
					</td>
				</tr>
				<tr>
					<td>Scope of Work</td>
					<td>
						<asp:TextBox ID="txtScopeOfWork" TextMode="MultiLine" MaxLength="2000" runat="server" style="height:150px;"></asp:TextBox>						
						<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
							ControlToValidate="txtScopeOfWork" SetFocusOnError="true"
							ErrorMessage="Please Enter Scope of Work." Display="Dynamic"
							ValidationGroup="SaveInfo1">
						</asp:RequiredFieldValidator>
					</td>
				</tr>
				<tr>
					<td>Main Equipment</td>
					<td>
						<asp:TextBox ID="txtMainEquipment" TextMode="MultiLine" MaxLength="2000" runat="server"></asp:TextBox>
					</td>
				</tr>				
				<tr>
					<td>Schedule</td>
					<td>
						<asp:TextBox ID="txtSchedule" TextMode="MultiLine" MaxLength="2000" runat="server" style="height:100px;"></asp:TextBox>
					</td>
				</tr>
				<tr>
					<td>Validity</td>
					<td>
						<asp:TextBox ID="txtValidityDays" MaxLength="4" runat="server" Text="60"></asp:TextBox>
						<asp:RequiredFieldValidator ID="rfvValidityDays" runat="server"
							ControlToValidate="txtValidityDays" SetFocusOnError="true"
							ErrorMessage="<br/>Please Enter Validity." Display="Dynamic"
							ValidationGroup="SaveInfo1">
						</asp:RequiredFieldValidator>
                        <asp:CompareValidator ID="cvValidityDays" runat="server" 
                            ControlToValidate="txtValidityDays" Display="Dynamic"
                            Operator="DataTypeCheck" Type="Integer"
                            SetFocusOnError="true" ValidationGroup="SaveInfo1"
                            ErrorMessage="<br/>Please Enter a Valid Validity in digits.">
                        </asp:CompareValidator>
					</td>
				</tr>
				<tr>
					<td>Submission Date</td>
					<td>
						<asp:TextBox ID="txtSubmissionDate" MaxLength="50" CssClass="CalendarTextBox" runat="server"></asp:TextBox>
						<asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                            ControlToValidate="txtSubmissionDate" SetFocusOnError="true" 
                            ErrorMessage="<br/>Please Select Submission Date." Display="Dynamic"
                            ValidationGroup="SaveInfo1">
                        </asp:RequiredFieldValidator>                        
                        <asp:CustomValidator ID="CustomValidator1" runat="server" 
                            ControlToValidate="txtSubmissionDate" SetFocusOnError="true" 
                            Display="Dynamic" ClientValidationFunction="ValidateDate"
                            ErrorMessage="<br/>Enter a Valid Date."
                            ValidationGroup="SaveInfo1">
                        </asp:CustomValidator>
					</td>
				</tr>				
				<tr>
					<td>Decision Date</td>
					<td>
						<asp:TextBox ID="txtDecisionDate" MaxLength="50" CssClass="CalendarTextBox" runat="server"></asp:TextBox>
						<asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" 
                            ControlToValidate="txtDecisionDate" SetFocusOnError="true" 
                            ErrorMessage="<br/>Please Select Decision Date." Display="Dynamic"
                            ValidationGroup="SaveInfo1">
                        </asp:RequiredFieldValidator>                        
                        <asp:CustomValidator ID="CustomValidator3" runat="server" 
                            ControlToValidate="txtDecisionDate" SetFocusOnError="true" 
                            Display="Dynamic" ClientValidationFunction="ValidateDate"
                            ErrorMessage="<br/>Enter a Valid Date."
                            ValidationGroup="SaveInfo1">
                        </asp:CustomValidator>
					</td>
				</tr>				                
            </table> 
            <div>
                <%--<input type="button" value="< Back" onclick="MoveNext(1);" />&nbsp;--%>
                <input type="button" value="Next >" onclick="ValidateAndMoveNext('SaveInfo1', 2);" />
            </div>      
        </div>
        
        <%--Step 2--%>
        <div id="divStep2" class="GroupBox" style="display:none;">
            <div class="FormHeader">
                <b>Enter Contract Pricing - Step 2 of 2</b><br />
                <asp:Label ID="lblStep2Title" Text="Enter the Estimated Breakdown." runat="server"></asp:Label>
            </div>
            <div>
                 <input type="button" value="Add Pricing" onclick="ShowPricingForm();" />
                 <div id="divPricingForm" style="display:block;">
                    <table cellpadding="3" cellspacing="0" class="FormTable">
                        <colgroup>
                            <col style="width:15%;" />
                            <col style="width:85%;" />
                        </colgroup>
				        <tr>
					        <td>Quotation</td>
					        <td>
						        <asp:DropDownList ID="ddlQuotationID" runat="server"></asp:DropDownList>
						        <asp:RequiredFieldValidator ID="rfvQuotationID" runat="server"
							        ControlToValidate="ddlQuotationID" SetFocusOnError="true"
							        ErrorMessage="<br/>Please Select Quotation." Display="Dynamic"
							        ValidationGroup="SaveInfo2">
						        </asp:RequiredFieldValidator>
					        </td>
				        </tr>
				        <tr>
					        <td>Item</td>
					        <td>
						        <asp:TextBox ID="txtItem" MaxLength="20" runat="server"></asp:TextBox>
						        <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server"
							        ControlToValidate="txtItem" SetFocusOnError="true"
							        ErrorMessage="<br/>Please Enter Item." Display="Dynamic"
							        ValidationGroup="SaveInfo2">
						        </asp:RequiredFieldValidator>
					        </td>
				        </tr>
				        <tr>
					        <td>Description</td>
					        <td>
						        <asp:TextBox ID="txtDescription" TextMode="MultiLine" MaxLength="2000" runat="server"></asp:TextBox>						        
					            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server"
							        ControlToValidate="txtDescription" SetFocusOnError="true"
							        ErrorMessage="Please Enter a Description." Display="Dynamic"
							        ValidationGroup="SaveInfo2">
						        </asp:RequiredFieldValidator>					        
					        </td>
				        </tr>
				        <tr>
					        <td>Pricing Type</td>
					        <td>
						        <asp:DropDownList ID="ddlPricingTypeID" runat="server"></asp:DropDownList>
						        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server"
							        ControlToValidate="ddlPricingTypeID" SetFocusOnError="true"
							        ErrorMessage="<br/>Please Select Pricing Type." Display="Dynamic"
							        ValidationGroup="SaveInfo2">
						        </asp:RequiredFieldValidator>
					        </td>
				        </tr>
				        <tr>
					        <td>Unit Price</td>
					        <td>
						        <asp:TextBox ID="txtUnitPrice" MaxLength="8" runat="server"></asp:TextBox>
						        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server"
							        ControlToValidate="txtUnitPrice" SetFocusOnError="true"
							        ErrorMessage="<br/>Please Enter Unit Price." Display="Dynamic"
							        ValidationGroup="SaveInfo2">
						        </asp:RequiredFieldValidator>
                                <asp:CompareValidator ID="CompareValidator2" runat="server" 
                                    ControlToValidate="txtUnitPrice" Display="Dynamic"
                                    Operator="DataTypeCheck" Type="Integer"
                                    SetFocusOnError="true" ValidationGroup="SaveInfo2"
                                    ErrorMessage="<br/>Please Enter a Valid Unit Price in digits.">
                                </asp:CompareValidator>
					        </td>
				        </tr>
				        <tr>
					        <td>Quantity</td>
					        <td>
						        <asp:TextBox ID="txtQuantity" MaxLength="4" runat="server"></asp:TextBox>
						        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server"
							        ControlToValidate="txtQuantity" SetFocusOnError="true"
							        ErrorMessage="<br/>Please Enter Quantity." Display="Dynamic"
							        ValidationGroup="SaveInfo2">
						        </asp:RequiredFieldValidator>
                                <asp:CompareValidator ID="CompareValidator1" runat="server" 
                                    ControlToValidate="txtQuantity" Display="Dynamic"
                                    Operator="DataTypeCheck" Type="Integer"
                                    SetFocusOnError="true" ValidationGroup="SaveInfo2"
                                    ErrorMessage="<br/>Please Enter a Valid Quantity in digits.">
                                </asp:CompareValidator>
					        </td>
				        </tr>				        
			        </table>
			        <div>
			            <input type="button" value="Save Pricing" onclick="ValidateAndBindPricingList();" />&nbsp;
			        </div>
                </div>
                <div id="divPricingList" style="display:none;">
                    
                </div>
            </div>
            <%--<div style="height:100px;">&nbsp;</div>--%>
            <div id="divStep2ButtonContainer" style="margin-top:10px; display:none;">
                <input type="button" value="< Back" onclick="MoveNext(1);" />&nbsp;
                <input type="button" value="Next >" onclick="MoveNext(3);" />
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
            </div>
            <%-- Next Previous Buttons --%>
            <div style="margin-top:10px;">
                <input type="button" value="< Back" onclick="MoveNext(2);" />&nbsp;
                <input type="button" value="Finish" onclick="SaveEnquiry();" />
            </div>
        </div>
        
        <%--Step 4--%>
        <div id="divStep4" class="GroupBox" style="display:none;">
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

