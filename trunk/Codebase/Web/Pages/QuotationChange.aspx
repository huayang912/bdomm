<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="QuotationChange.aspx.cs" Inherits="Pages_QuotationChange" %>
<%@ Register Src="~/Controls/jQueryCalendar.ascx" TagName="jQueryCalendar" TagPrefix="UC"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <UC:jQueryCalendar id="ucjQueryCalendar" runat="server"></UC:jQueryCalendar>
    <style type="text/css">
        table th
        {
        	text-align:left;
        }
    </style>
    
    <script language="javascript" type="text/javascript">
        var _PricingList = new Array();
        var _CustomQuotation = null;
        var _QuotationID = <%=_ID.ToString() %>;
        var _EnquiryID = <%=_EnquiryID.ToString() %>;
        var _EditIndex = -1;
        
        function MoveNext(stepIndex) {
            for (i = 1; i < 4; i++) {
                document.getElementById('divStep' + i).style.display = 'none';
            }
            document.getElementById('divStep' + stepIndex).style.display = 'block';
        }
        ///Quotations 
        function ValidateAndMoveNext(validationGroup, stepID)
        {            
            if (Page_ClientValidate(validationGroup))
            {                
                //SaveQuotationInformation();
                MoveNext(stepID);                
            }
        }
        function PrepareQuotationObject() {
            _CustomQuotation = new App.CustomModels.CustomQuotation();
            _CustomQuotation.ID = _QuotationID;
            _CustomQuotation.EnquiryID = _EnquiryID;
            _CustomQuotation.Subcontractor = $('#<%=txtSubcontractor.ClientID %>').val();
            _CustomQuotation.ScopeOfWork = $('#<%=txtScopeOfWork.ClientID %>').val();
            _CustomQuotation.MainEquipment = $('#<%=txtMainEquipment.ClientID %>').val();
            _CustomQuotation.Scheduel = $('#<%=txtSchedule.ClientID %>').val();
            _CustomQuotation.ValidityDays = $('#<%=txtValidityDays.ClientID %>').val();
            _CustomQuotation.SubmissionDate = $('#<%=txtSubmissionDate.ClientID %>').val();
            _CustomQuotation.DecisionDate = $('#<%=txtDecisionDate.ClientID %>').val();        
        }
        function SaveQuotationInformation() {
            ShowProgress();
            PrepareQuotationObject();
            var currencyID = $('#<%= ddlCurrency.ClientID %>').val().split(':')[0];
            //alert(currencyID);
            _CustomQuotation.CurrencyID = currencyID;
            
            PageMethods.SaveQuotation(_CustomQuotation, _PricingList, OnSaveEnquirySuccess, OnAjax_Error, OnAjax_TimeOut);
        }
        function OnSaveEnquirySuccess(result)
        {
            HideProgress();
            var id = result.split(':')[0];
            var number = result.split(':')[1];                     
            $('#<%=lblQuotationMessage.ClientID %>').html('Quotation <b>' + number + '</b> has been successfully created.');
            MoveNext(3);            
        }
        
        ///Pricing Section
        function ValidateAndBindPricingList() {
            if (Page_ClientValidate('SaveInfo2')) {
                PrepareQuotationPricingObject();
                BindPricingList();                
            }
        }        
        function PrepareQuotationPricingObject()
        {
            var pricingLine = new App.CustomModels.CustomQuotationPricingLine(); 
    
            pricingLine.ID = 0;
            pricingLine.QuotationID = _QuotationID;
            pricingLine.Item = $('#<%= txtItem.ClientID %>').val();
            pricingLine.Description = $('#<%= txtDescription.ClientID %>').val();
            pricingLine.PricingTypeID =  $('#<%= ddlPricingTypeID.ClientID %>').val();
            pricingLine.PricingType =  $("#<%= ddlPricingTypeID.ClientID %> option[value='" + pricingLine.PricingTypeID + "']").text();
            pricingLine.UnitPrice = $('#<%= txtUnitPrice.ClientID %>').val();
            pricingLine.Quantity = $('#<%= txtQuantity.ClientID %>').val();
            pricingLine.Price = pricingLine.UnitPrice * pricingLine.Quantity;
            if(_EditIndex > -1)
                _PricingList[_EditIndex] = pricingLine;
            else        
                _PricingList.push(pricingLine);
        }
        function ShowPricingForm(showPricing) {
            if(showPricing)
            {
                $('#divPricingForm').show();
                $('#divPricingList').hide();
                $('#divMasterControl').hide()
                $('#divStep2ButtonContainer').hide();
            }
            else
            {
                $('#divPricingForm').hide();                
                $('#divPricingList').show();
                $('#divMasterControl').show();
                $('#divStep2ButtonContainer').show();
            }
            _EditIndex = -1;
        }
        function ChangeCurrency(currencyValue)
        {
            //alert('CurrencyChange NOt Implemented ' + currencyValue);   
            BindPricingList();
        }
        function BindPricingList()
        {
            if(_PricingList.length > 0)
            {            
                var html = '<table cellpadding="3" cellspacing="0" style="width:100%;">';
                html += '<colgroup>';
                html += '   <col style="width:12%;" />';
                html += '   <col style="width:35%;" />';
                html += '   <col style="width:15%;" />';
                html += '   <col style="width:10%;" />';
                html += '   <col style="width:10%;" />';
                html += '   <col style="width:10%;" />';
                html += '   <col style="width:8%;" />';
                html += '   <col />';
                html += '</colgroup>';
                
                html += '<tr>';
                html += '   <th>Item</th><th>Description</th><th>Pricing Type</th><th>Unit Price</th><th>Quantity</th><th style="text-align:right;">Price</th><th style="text-align:center;">Edit</th>';
                html += '</tr>';                
                var currencySymbol = $('#<%= ddlCurrency.ClientID %>').val().split(':')[1];               
                
                var totalPrice = 0;
                for(i = 0; i < _PricingList.length; i++)
                {
                    var pricingLine = _PricingList[i];                    
                    html += '<tr>';
                    html += '   <td>' + pricingLine.Item + '</td>';
                    html += '   <td>' + FormatText(pricingLine.Description) + '</td>';
                    html += '   <td>' + pricingLine.PricingType + '</td>';
                    html += '   <td>' + pricingLine.UnitPrice + '</td>';
                    html += '   <td>' + pricingLine.Quantity + '</td>';
                    html += '   <td style="text-align:right;">' + jQuery.trim(currencySymbol) + pricingLine.Price + '</td>';
                    html += '   <td style="text-align:center;"><a href="javascript:void(0);" onclick="LoadPricingForEdit(' + i + ')">Edit</a></td>';
                    html += '</tr>';                    
                    totalPrice += pricingLine.Price;
                }
                html += '<tr>';
                html += '   <td colspan="6" style="text-align:right;"><b>Total Price:</b> &nbsp;<input type="text" readonly="readonly" value="' + jQuery.trim(currencySymbol) + totalPrice + '" style="text-align:right;" /></td>'; 
                html += '   <td></td>';
                html += '</tr>';
                html += '</table>';
                //html += '<div style="margin: 10px 0px 0px 3px; width:100%; text-align:right;"></div>'
                $('#divPricingList').html(html);
                ShowPricingForm(false);
            }
        }
        function LoadPricingForEdit(index)
        {
            ShowPricingForm(true);
            _EditIndex = index;
            var pricingLine =  _PricingList[index];
            $('#<%= txtItem.ClientID %>').val(pricingLine.Item);
            $('#<%= txtDescription.ClientID %>').val(pricingLine.Description);
            $('#<%= ddlPricingTypeID.ClientID %>').val(pricingLine.PricingTypeID);               
            $('#<%= txtUnitPrice.ClientID %>').val(pricingLine.UnitPrice);
            $('#<%= txtQuantity.ClientID %>').val(pricingLine.Quantity);
        }
        function ClearFormData()
        {
            $('#<%= txtItem.ClientID %>').val('');
            $('#<%= txtDescription.ClientID %>').val('');
            //$('#<%= ddlPricingTypeID.ClientID %>').val('');                           
            $('#<%= txtUnitPrice.ClientID %>').val('0');
            $('#<%= txtQuantity.ClientID %>').val('0');
        }
        function PreparePricingList()
        {
            if($('#<%= hdnQuotationPricings.ClientID %>').val().length > 0)
                _PricingList = eval($('#<%= hdnQuotationPricings.ClientID %>').val());            
            
            var showEditForm = false;
            if(_PricingList == null || 'undefined' == _PricingList || _PricingList.length == 0)
                showEditForm = true;
            else
                BindPricingList();
            
            ShowPricingForm(showEditForm);
        }
        function SubmitQuotation()
        {
            ///There Is No Operation Specified in the Windows Application for this
        }
        ///Page Load
        $(document).ready(function(){
            PreparePricingList();            
            ClearFormData();
        });
    </script>
    
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="PageHeaderContentPlaceHolder" Runat="Server">
    <asp:Literal id="ltrHeading" runat="server">Create New Quotation Wizard</asp:Literal>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="SideBarPlaceHolder" Runat="Server">
    
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="PageContentPlaceHolder" Runat="Server">    
    
    <div id="divMessage" runat="server" enableviewstate="false" visible="false"></div>
    
    <asp:HiddenField ID="hdnQuotationPricings" runat="server" />
    
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
                <div id="divMasterControl" style="padding-left:3px; margin-bottom:10px;">
                    <div class="floatleft">
                        <input type="button" value="Add Pricing" onclick="ShowPricingForm(true); ClearFormData();" /> 
                    </div>
                    <div class="floatleft" style="margin-left:20px;">
                        Currency &nbsp;<asp:DropDownList ID="ddlCurrency" runat="server" onchange="ChangeCurrency(this.value);"></asp:DropDownList>
                    </div>                    						        
                    <div class="clearboth"></div>
                </div>
                 <div id="divPricingForm" style="display:block;">
                    <table cellpadding="3" cellspacing="0" class="FormTable">
                        <colgroup>
                            <col style="width:15%;" />
                            <col style="width:85%;" />
                        </colgroup>				        
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
						        <asp:TextBox ID="txtDescription" TextMode="MultiLine" MaxLength="2000" runat="server" style="height:90px;"></asp:TextBox>						        
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
                                    Operator="DataTypeCheck" Type="Double"
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
                <div id="divPricingList" style="display:none; min-height:200px;">
                    
                </div>                
            </div>
            <%--<div style="height:100px;">&nbsp;</div>--%>
            <div id="divStep2ButtonContainer" style="margin-top:10px; display:block;">
                <input type="button" value="< Back" onclick="MoveNext(1);" />&nbsp;
                <input type="button" value="Finish" onclick="SaveQuotationInformation();" />
            </div>
        </div>
        
        <%--Step 3--%>
        <div id="divStep3" class="GroupBox" style="display:none; min-height:120px;">
            <div class="FormHeader">
                <b>Summary</b><br />
                <asp:Label ID="lblStep3Title" Text="Tick the box below to submit this quotation now." runat="server"></asp:Label>
            </div>
            <div>
                <asp:Label ID="lblQuotationMessage" runat="server" Text=""></asp:Label>               
                <div style="margin-top:15px;">
                    <asp:CheckBox ID="chkSubmitQuotation" Enabled="false" runat="server" Text="Submit this quotation now." />
                </div>                
            </div>
            <%-- Next Previous Buttons --%>
            <div style="margin-top:20px;">
                <%--<input type="button" value="< Back" onclick="MoveNext(2);" />&nbsp;--%>
                <input type="button" value="Close" disabled="disabled" onclick="SubmitQuotation();" />
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

