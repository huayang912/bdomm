<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPagePopup.master" AutoEventWireup="true" CodeFile="PersonnelBasicInfo.aspx.cs" Inherits="Pages_PersonnelBasicInfo" %>
<%@ Register Src="~/Controls/jQueryCalendar.ascx" TagName="jQueryCalendar" TagPrefix="UC" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">   
    <UC:jQueryCalendar runat="server" ID="ucjQueryCalendar" />
    <style type="text/css">
        .LeftColumn{width:410px; float:left;}
        .RightColumn{width:410px; float:left; margin-left:10px;}
        .AddNewLink{margin-bottom:5px;}
    </style> 
    <script type="text/javascript" language="javascript">
        function ValidateEmailList(sender, args) {
            args.IsValid = true;
            $('#tblEmailAddressList tr:gt(0)').each(function() {
                if ($(this).find("input[type='text']").val().length == 0)
                    args.IsValid = false;
            });
        }
        function ValidateEmail(sender, args) {
            args.IsValid = true;
            $('#tblEmailAddressList tr:gt(0)').each(function() {
                if (!IsValidEmail($(this).find("input[type='text']").val()))
                    args.IsValid = false;
            });
        }
        function ValidatePhoneList(sender, args) {
            args.IsValid = true;
            $('#tblPhoneNumbersList tr:gt(0)').each(function() {
                if ($(this).find("input[type='text']").val().length == 0)
                    args.IsValid = false;
            });
        }
        var _Roles = new Array();
        var _TelephoneTypes = new Array();
        var _Telephones = new Array();
        var _Emails = new Array();
        var _ContactRoles = new Array();
        var _Personnel = null;
        
        function PrepareCollection() {
            if ($('#<%= hdnRoles.ClientID %>').val().length > 0)
                _Roles = eval($('#<%= hdnRoles.ClientID %>').val());
            if ($('#<%= hdnTelephoneTypes.ClientID %>').val().length > 0)
                _TelephoneTypes = eval($('#<%= hdnTelephoneTypes.ClientID %>').val());
            if ($('#<%= hdnTelephoneNumbers.ClientID %>').val().length > 0)
                _Telephones = eval($('#<%= hdnTelephoneNumbers.ClientID %>').val());
            if ($('#<%= hdnEmailAddresses.ClientID %>').val().length > 0)
                _Emails = eval($('#<%= hdnEmailAddresses.ClientID %>').val());
            if ($('#<%= hdnContactRoles.ClientID %>').val().length > 0)
                _ContactRoles = eval($('#<%= hdnContactRoles.ClientID %>').val());
            //alert(_Roles.length);
        }
        function BindTelephoneTypeDropdown(selectedID) {
            var html = '<select>';
            for (i = 0; i < _TelephoneTypes.length; i++) {
                var selectedText = _TelephoneTypes[i].ID == selectedID ? ' selected="selected"' : '';
                html += '<option value="' + _TelephoneTypes[i].ID + '"' + selectedText +'>' + _TelephoneTypes[i].Name + '</option>';
            }
            html +='</select>';
            return html;
        }
        function SetSelectedRole(ddl) {
            var index = $(ddl).parent().parent().index() - 1;
            //alert(index);
            var obj = _ContactRoles[index];
            obj.RoleID = $(ddl).val();
            //alert(obj.RoleID);
        }
        function BindRolesDropdown(selectedRoleID) {
            var html = '<select onchange="SetSelectedRole(this);">';
            for (i = 0; i < _Roles.length; i++) {
                var seletedText = selectedRoleID == _Roles[i].ID ? ' selected="selected"' : '';
                html += '<option value="' + _Roles[i].ID + '"' + seletedText + '>' + _Roles[i].Name + '</option>';
            }
            html += '</select>';
            return html;
        }
        var _Tr = null;
        function DeleteRole(anchorElement) {
            _Tr = $(anchorElement).parent().parent();            
            var id = $(_Tr).find("input[type='hidden']").val();
            if (id > 0) {
                ShowParentLoading();
                PageMethods.DeleteRole(id, DeleteRole_Success, OnAjax_Error, OnAjax_TimeOut);
            }
            else {
                ReArrangeContactRoles();
                SetParentHeight();
                FormatTable($(_Tr).parent());
            }
        }
        function DeleteRole_Success(result) {
            if (result == true) {
                HideParentLoading();
                ReArrangeContactRoles();
                SetParentHeight();
                FormatTable($(_Tr).parent());
            }
        }
        function ReArrangeContactRoles() {
            _ContactRoles.splice($(_Tr).index() - 1, 1);                       
            for (i = 0; i < _ContactRoles.length; i++) {
                _ContactRoles[i].Order = i + 1;
            }
            BindRoles();
        }
        function DeleteEmail(anchorElement) {
            _Tr = $(anchorElement).parent().parent();
            var id = $(_Tr).find("input[type='hidden']").val();
            if (id > 0) {
                ShowParentLoading();
                PageMethods.DeleteEmail(id, DeleteEmail_Success, OnAjax_Error, OnAjax_TimeOut);
            }
            else {
                $(_Tr).remove();
                SetParentHeight();
                FormatTable($(_Tr).parent());
            }
        }
        function DeleteEmail_Success(result) {
            if (result == true) {
                HideParentLoading();
                $(_Tr).remove();
                SetParentHeight();
                FormatTable($(_Tr).parent());
            }
        }
              
        function DeleteTelephone(anchorElement) {
            _Tr = $(anchorElement).parent().parent();
            var id = $(_Tr).find("input[type='hidden']").val();
            if (id > 0) {
                ShowParentLoading();
                PageMethods.DeleteTelephone(id, DeleteTelephone_Success, OnAjax_Error, OnAjax_TimeOut);
            }
            else {
                $(_Tr).remove();
                SetParentHeight();
                FormatTable($(_Tr).parent());
            }
        }
        function DeleteTelephone_Success(result) {
            if (result == true) {
                HideParentLoading();
                $(_Tr).remove();
                SetParentHeight();
                FormatTable($(_Tr).parent());
            }
        }
        function AddNewTelephoneRow() {
            var tr = '<tr><td><input type="text" value=""/><input type="hidden" value="0"/></td><td>' + BindTelephoneTypeDropdown('') + '</td><td><a href="javascript:void(0);" onclick="DeleteTelephone(this);">Delete</a></td></tr>';
            $('#tblPhoneNumbersList').append(tr);
            SetParentHeight();
            FormatTable($('#tblPhoneNumbersList'));
        }
        function AddNewEmailRow() {            
            var tr = '<tr><td><input type="text" value=""/><input type="hidden" value="0"/></td><td><a href="javascript:void(0);" onclick="DeleteEmail(this);">Delete</a></td></tr>';
            $('#tblEmailAddressList').append(tr);
            SetParentHeight();
            FormatTable($('#tblEmailAddressList'));
        }
        function AddNewRoleRow() {
            var contactRole = new App.CustomModels.PersonnelRole();
            contactRole.ID = 0;
            contactRole.Order = _ContactRoles.length + 1;
            contactRole.RoleID = 0;
            _ContactRoles.push(contactRole);
            BindRoles();
            SetParentHeight(); 
        }
        function BindTelephoneNumbers() {            
            $('#tblPhoneNumbersList').find('tr:gt(0)').remove();
            for (j = 0; j < _Telephones.length; j++) {
                var obj = _Telephones[j];
                var tr = '<tr><td><input type="text" value="' + obj.Number + '"/><input type="hidden" value="' + obj.ID + '"/></td><td>' + BindTelephoneTypeDropdown(obj.TypeID) + '</td><td><a href="javascript:void(0);" onclick="DeleteTelephone(this);">Delete</a></td></tr>';
                $('#tblPhoneNumbersList').append(tr);
            }
            FormatTable($('#tblPhoneNumbersList'));
        }
        function BindRoles() {
            $('#tblRolesList').find('tr:gt(0)').remove();
            for (j = 0; j < _ContactRoles.length; j++) {
                var obj = _ContactRoles[j];
                var tr = '<tr><td><span class="Order">' + obj.Order + '</span><input type="hidden" value="' + obj.ID + '"/></td><td>' + BindRolesDropdown(obj.RoleID) + '</td><td><a href="javascript:void(0);" onclick="DeleteRole(this);">Delete</a></td></tr>';
                $('#tblRolesList').append(tr);
            }
            FormatTable($('#tblRolesList'));
        }
        function BindEmailAddresses() {
            $('#tblEmailAddressList').find('tr:gt(0)').remove();
            for (j = 0; j < _Emails.length; j++) {
                var obj = _Emails[j];
                var tr = '<tr><td><input type="text" value="' + obj.Email + '"/><input type="hidden" value="' + obj.ID + '"/></td><td><a href="javascript:void(0);" onclick="DeleteEmail(this);">Delete</a></td></tr>';
                $('#tblEmailAddressList').append(tr);
            }
            FormatTable($('#tblEmailAddressList'));
        }
        function PopulatePersonnelObject() {
            _Personnel = new App.CustomModels.Personnel();
            _Personnel.ID = '<%=_ID.ToString() %>';
            _Personnel.FirstName = $('#<%= txtFirstNames.ClientID %>').val();
            _Personnel.LastName = $('#<%= txtLastName.ClientID %>').val();
            _Personnel.Address = $('#<%= txtAddress.ClientID %>').val();
            _Personnel.PostCode = $('#<%= txtPostcode.ClientID %>').val();
            _Personnel.CountryID = $('#<%= ddlCountryID.ClientID %>').val();
            _Personnel.MaritalStatus = $('#<%= ddlMaritalStatusID.ClientID %>').val();
            _Personnel.PlaceOfBirth = $('#<%= txtPlaceOfBirth.ClientID %>').val();
            _Personnel.CountryOfBirthID = $('#<%= ddlCountryOfBirthID.ClientID %>').val();
            _Personnel.DateOfBirth = $('#<%= txtDateOfBirth.ClientID %>').val();
            _Personnel.DateOfLastMeeting = $('#<%= txtDateOfLastMeeting.ClientID %>').val();
            _Personnel.PreferredDayRate = $('#<%= txtPreferredDayRate.ClientID %>').val();
            _Personnel.DayRateCurrencyID = $('#<%= ddlDayRateCurrencyID.ClientID %>').val();
            _Personnel.NoSMSOrEmail = $('#<%= chkNoSMSorEmail.ClientID %>').is(':checked');
            _Personnel.InActive = $('#<%= chkInactive.ClientID %>').is(':checked');
        }
        function PopulateTelephones() {
            _Telephones.length = 0;
            $('#tblPhoneNumbersList tr:gt(0)').each(function() {
                var telephone = new App.CustomModels.PersonnelTelephone();
                telephone.ID = $(this).find("input[type='hidden']").val();
                telephone.Number = $(this).find("input[type='text']").val();
                telephone.TypeID = $(this).find("select").val();
                _Telephones.push(telephone);
            });
        }
        function PopulateRoles() {
            _ContactRoles.length = 0;
            $('#tblRolesList tr:gt(0)').each(function() {
                var contactRole = new App.CustomModels.PersonnelRole();
                contactRole.ID = $(this).find("input[type='hidden']").val();
                contactRole.RoleID = $(this).find("select").val();
                contactRole.Order = $(this).find("span").html();
                _ContactRoles.push(contactRole);
            });
        }
        function PopulateEmails() {
            _Emails.length = 0;
            $('#tblEmailAddressList tr:gt(0)').each(function() {
                var email = new App.CustomModels.PersonnelEmail();
                email.ID = $(this).find("input[type='hidden']").val();
                email.Email = $(this).find("input[type='text']").val();     
                _Emails.push(email);
            });
        }
        function SavePersonnel() {
            if (Page_ClientValidate('SaveInfo')) {
                ShowParentLoading();
                PopulatePersonnelObject();                
                //PopulateRoles();
                PopulateEmails();
                PopulateTelephones();                
                PageMethods.SavePersonnel(_Personnel, _Telephones, _Emails, _ContactRoles, SavePersonnel_Success, OnAjax_Error, OnAjax_TimeOut);              
            }
        }
        function SavePersonnel_Success(result) {
            if (result > 0) {
                HideParentLoading();
                alert('Personnel Saved Successfully');
                if(_Personnel.ID == 0)
                    window.parent.RefreshPage(result);
                else
                    window.location = '<%=Request.Url.AbsolutePath %>?ID=' + result + '&Rnd=' + GetRandomNumber(); 
            }
        }
        function SetParentHeight() {
            var iFrame = window.parent.document.getElementById('frmContainer');
            if (null != iFrame)
                window.parent.SetFrameHeight($(document).height());
        }
        function FormatTable(tbl) {
            $(tbl).find('tr:gt(0)').each(function(i, tr) {
                if (i % 2 == 0)
                    $(tr).removeClass('OddRowListing EvenRowListing').addClass('OddRowListing');
                else
                    $(tr).removeClass('OddRowListing EvenRowListing').addClass('EvenRowListing');
            });
        }
        function ShowParentLoading() {
            window.parent.ShowLoading();
        }
        function HideParentLoading() {
            window.parent.HideLoading();
        }
        $(document).ready(function() {
            PrepareCollection();
            BindTelephoneNumbers();
            BindRoles();
            BindEmailAddresses();
            //ShowParentLoading();
            SetParentHeight();            
        });
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="body" Runat="Server">
    <div id="divMessage" runat="server" enableviewstate="false" visible="false"></div>
    
    <asp:HiddenField ID="hdnTelephoneNumbers" runat="server" />
    <asp:HiddenField ID="hdnEmailAddresses" runat="server" />
    <asp:HiddenField ID="hdnContactRoles" runat="server" />
    <asp:HiddenField ID="hdnRoles" runat="server" />
    <asp:HiddenField ID="hdnTelephoneTypes" runat="server" />
    
    <asp:Panel ID="pnlDetails" runat="server">
        <div class="LeftColumn">
            <div class="WinGroupBox">
                <div class="WinGroupBoxHeader">Personnel Details</div>
            
                <table cellpadding="3" cellspacing="0" style="width:100%;">
                    <colgroup>
                        <col style="width:25%;" />
                        <col />                                        
                    </colgroup>				
			        <tr>
				        <td>Surname<span class="requiredMark">*</span></td>
				        <td>
					        <asp:TextBox ID="txtLastName" MaxLength="50" runat="server"></asp:TextBox>
					        <asp:RequiredFieldValidator ID="rfvLastName" runat="server"
						        ControlToValidate="txtLastName" SetFocusOnError="true"
						        ErrorMessage="Please Enter a LastName." Display="Dynamic"
						        ValidationGroup="SaveInfo">
					        </asp:RequiredFieldValidator>
				        </td>
			        </tr>
			        <tr>
				        <td>First Name<span class="requiredMark">*</span></td>
				        <td>
					        <asp:TextBox ID="txtFirstNames" MaxLength="50" runat="server"></asp:TextBox>
					        <asp:RequiredFieldValidator ID="rfvFirstNames" runat="server"
						        ControlToValidate="txtFirstNames" SetFocusOnError="true"
						        ErrorMessage="Please Enter a FirstNames." Display="Dynamic"
						        ValidationGroup="SaveInfo">
					        </asp:RequiredFieldValidator>
				        </td>
			        </tr>
			        <tr>
				        <td>Address</td>
				        <td>
					        <asp:TextBox ID="txtAddress" TextMode="MultiLine" MaxLength="200" runat="server"></asp:TextBox>
				        </td>
			        </tr>
			        <tr>
				        <td>Postcode</td>
				        <td>
					        <asp:TextBox ID="txtPostcode" MaxLength="20" runat="server"></asp:TextBox>
				        </td>
			        </tr>
			        <tr>
				        <td>Country</td>
				        <td>
					        <asp:DropDownList ID="ddlCountryID" runat="server"></asp:DropDownList>
				        </td>
			        </tr>				
			        <tr>
				        <td>Marital Status<span class="requiredMark">*</span></td>
				        <td>
					        <asp:DropDownList ID="ddlMaritalStatusID" runat="server"></asp:DropDownList>
					        <asp:RequiredFieldValidator ID="rfvMaritalStatusID" runat="server"
						        ControlToValidate="ddlMaritalStatusID" SetFocusOnError="true"
						        ErrorMessage="Please Select a MaritalStatus." Display="Dynamic"
						        ValidationGroup="SaveInfo">
					        </asp:RequiredFieldValidator>
				        </td>
			        </tr>
			        <tr>
				        <td>Place Of Birth</td>
				        <td>
					        <asp:TextBox ID="txtPlaceOfBirth" MaxLength="100" runat="server"></asp:TextBox>
				        </td>
			        </tr>
			        <tr>
				        <td>Country Of Birth</td>
				        <td>
					        <asp:DropDownList ID="ddlCountryOfBirthID" runat="server"></asp:DropDownList>
				        </td>
			        </tr>
			        <tr>
				        <td>Date Of Birth</td>
				        <td>
					        <asp:TextBox ID="txtDateOfBirth" MaxLength="50" CssClass="CalendarTextBox" runat="server"></asp:TextBox>
					        <asp:CustomValidator ID="cvDateOfBirth" runat="server"
						        ControlToValidate="txtDateOfBirth" SetFocusOnError="true"
						        ClientValidationFunction="ValidateDate"
						        ErrorMessage="Please Select a Valid Date." Display="Dynamic"
						        ValidationGroup="SaveInfo">
					        </asp:CustomValidator>
				        </td>
			        </tr>              
                </table>
            </div>
            
            <div class="WinGroupBox">
                <div class="WinGroupBoxHeader">Phone Numbers</div>
                <div id="divPhoneNumbersList">
                    <div>
                        <asp:CustomValidator ID="cvPhoneList" runat="server"
                            Display="Dynamic" ValidateEmptyText="true"
                            ClientValidationFunction="ValidatePhoneList"
                            ValidationGroup="SaveInfo"
                            ErrorMessage="Please Enter Telephone.">
                        </asp:CustomValidator>
                    </div>
                    <div class="AddNewLink">
                        <a href="javascript:void(0);" onclick="AddNewTelephoneRow();">Add New Telephone</a>
                    </div>
                    <table id="tblPhoneNumbersList" class="GridView" cellpadding="3" cellspacing="0">
                        <colgroup>
                            <col style="width:35%;" />
                            <col style="width:35%;" />                  
                            <col />
                        </colgroup>
                        <tr>
                            <th>Number</th>
                            <th>Type</th>                                
                            <th>Actions</th>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
            
        <div class="RightColumn">
            <div class="WinGroupBox">
                <div class="WinGroupBoxHeader">Business Details</div>
                <table cellpadding="3" cellspacing="0" style="width:100%;">
                    <colgroup>
                        <col style="width:30%;" />
                        <col />                                        
                    </colgroup>
                    <tr>
			            <td>Date Of Last Meeting</td>
			            <td>
				            <asp:TextBox ID="txtDateOfLastMeeting" MaxLength="50" CssClass="CalendarTextBox" runat="server"></asp:TextBox>
				            <asp:CustomValidator ID="cvDateOfLastMeeting" runat="server"
					            ControlToValidate="txtDateOfLastMeeting" SetFocusOnError="true"
					            ClientValidationFunction="ValidateDate"
					            ErrorMessage="Please Select a Valid Date." Display="Dynamic"
					            ValidationGroup="SaveInfo">
				            </asp:CustomValidator>
			            </td>
		            </tr>
		            <tr>
				        <td>Preferred Day Rate</td>
				        <td>
				            <table cellpadding="0" cellspacing="0" style="width:100%;">
				                <tr>
				                    <td>
				                        <asp:TextBox ID="txtPreferredDayRate" MaxLength="4" runat="server"></asp:TextBox>
					                    <asp:CompareValidator ID="cpvPreferredDayRate" runat="server"
						                    ControlToValidate="txtPreferredDayRate" SetFocusOnError="true"
						                    Operator="DataTypeCheck" Type="Double"
						                    ErrorMessage="Please Enter Digits Only." Display="Dynamic"
						                    ValidationGroup="SaveInfo">
					                    </asp:CompareValidator>
				                    </td>
				                    <td style="padding-left:10px; width:20%;">
				                        <asp:DropDownList ID="ddlDayRateCurrencyID" runat="server"></asp:DropDownList>
					                    <%--<asp:CompareValidator ID="cpvDayRateCurrencyID" runat="server"
						                    ControlToValidate="txtDayRateCurrencyID" SetFocusOnError="true"
						                    Operator="DataTypeCheck" Type="Integer"
						                    ErrorMessage="Please Enter Digits Only." Display="Dynamic"
						                    ValidationGroup="SaveInfo">
					                    </asp:CompareValidator>--%>
				                    </td>
				                </tr>
				            </table>						    
				        </td>
			        </tr>	
				    
			        <tr>
				        <td>No SMS or Email</td>
				        <td>
					        <asp:CheckBox ID="chkNoSMSorEmail" runat="server"/>
				        </td>
			        </tr>	        
			        <tr>
				        <td>Inactive</td>
				        <td>
					        <asp:CheckBox ID="chkInactive" runat="server"/>
				        </td>
			        </tr>
		        </table>
            </div>
            
            
            <div class="WinGroupBox">
                <div class="WinGroupBoxHeader">Roles</div>
                <div id="div1">
                    <div class="AddNewLink">
                        <a href="javascript:void(0);" onclick="AddNewRoleRow();">Add New Role</a>
                    </div>
                    <table id="tblRolesList" class="GridView" cellpadding="3" cellspacing="0">
                        <colgroup>
                            <col style="width:35%;" />
                            <col style="width:35%;" />                  
                            <col />
                        </colgroup>
                        <tr>
                            <th>Order</th>
                            <th>Role</th>                                
                            <th>Actions</th>
                        </tr>
                    </table>
                </div>
            </div>
            
            <div class="WinGroupBox">
                <div class="WinGroupBoxHeader">Email Addresses</div>
                <div>
                    <div>
                        <asp:CustomValidator ID="cvEmailAddressList" runat="server"
                            Display="Dynamic" ValidateEmptyText="true"
                            ClientValidationFunction="ValidateEmailList"
                            ValidationGroup="SaveInfo"
                            ErrorMessage="Please Enter Email.">
                        </asp:CustomValidator>                            
                    </div>
                    <div>
                        <asp:CustomValidator ID="cvEmailAddressListEmailValidation" runat="server"
                            Display="Dynamic" ValidateEmptyText="true"
                            ClientValidationFunction="ValidateEmail"
                            ValidationGroup="SaveInfo"
                            ErrorMessage="Please Enter Valid Email.">
                        </asp:CustomValidator>                            
                    </div>
                    <div class="AddNewLink">
                        <a href="javascript:void(0);" onclick="AddNewEmailRow();">Add New Email</a>
                    </div>                        
                    <table id="tblEmailAddressList" class="GridView" cellpadding="3" cellspacing="0">
                        <colgroup>
                            <col style="width:65%;" />                                                             
                            <col />
                        </colgroup>
                        <tr>
                            <th>Email</th>                                
                            <th>Actions</th>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
        <div class="clearboth"></div>
        
        <div class="TenPixelTopMargin">
            <input type="button" class="ButtonCommon" value="Save" onclick="SavePersonnel();" />
        </div>        
    </asp:Panel>    
</asp:Content>

