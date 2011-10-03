<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPagePopup.master" AutoEventWireup="true" CodeFile="PersonnelSkills.aspx.cs" Inherits="Pages_PersonnelBasicInfo" %>
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

        function ValidateNotesList(sender, args) {
            args.IsValid = true;
            $('#tblNotesList tr:gt(0)').each(function() {

                if ($(_Tr).find("input[type='hidden']").val() > 0) {

                    if ($(this).find("input[type='text']").val().length == 0)
                        args.IsValid = false;
                }
            });
        }
        
        var _Roles = new Array();
        var _TelephoneTypes = new Array();
        var _hdnCommTypes = new Array();
        var _Telephones = new Array();
        var _Notes = new Array();        
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
            if ($('#<%= hdnCommTypes.ClientID %>').val().length > 0)
                _hdnCommTypes = eval($('#<%= hdnCommTypes.ClientID %>').val());
            if ($('#<%= hdnNotes.ClientID %>').val().length > 0)
                _Notes = eval($('#<%= hdnNotes.ClientID %>').val());
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

        function BindCommTypeDropdown(selectedID) {

            //alert(selectedID);
            
            var html = '';
            selectedID == '' ? html += '<select>' : html += '<select disabled>'
            
            for (i = 0; i < _hdnCommTypes.length; i++) {
                var selectedText = _hdnCommTypes[i].ID == selectedID ? ' selected="selected"' : '';
                html += '<option value="' + _hdnCommTypes[i].ID + '"' + selectedText + '>' + _hdnCommTypes[i].Name + '</option>';
            }
            html += '</select>';
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

        function DeleteNotes(anchorElement) {
            _Tr = $(anchorElement).parent().parent();
            var id = $(_Tr).find("input[type='hidden']").val();
            if (id > 0) {
                ShowParentLoading();
                PageMethods.DeleteNotes(id, DeleteNotes_Success, OnAjax_Error, OnAjax_TimeOut);
            }
            else {
                $(_Tr).remove();
                SetParentHeight();
                FormatTable($(_Tr).parent());
            }
        }

        function DeleteNotes_Success(result) {
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

        function AddNewNotesRow() {
            //alert("T");
            var tr = '<tr><td><TEXTAREA ROWS=3 style="width:90%"></textarea><input type="hidden" value="0"/></td><td colspan="3">' + BindCommTypeDropdown('') + '&nbsp;&nbsp;<a href="javascript:void(0);" onclick="DeleteNotes(this);">Delete</a></td></tr>';
            $('#tblNotesList').append(tr);
            SetParentHeight();
            FormatTable($('#tblNotesList'));
        }
        //<input type="text" style="width:250px;" value=""/>
        
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
                var tr = '<tr><td><input type="text" value="' + obj.Number + '"/><input type="hidden" value="' + obj.ID + '"/></td><td>' + BindTelephoneTypeDropdown(obj.TypeID) + '</td><td><a href="javascript:void(0);" onclick="SendSMS(this);">SMS</a> &nbsp; <a href="javascript:void(0);" onclick="DeleteTelephone(this);">Delete</a></td></tr>';
                $('#tblPhoneNumbersList').append(tr);
            }
            FormatTable($('#tblPhoneNumbersList'));
        }

        function BindNotes() {
            //alert(_Notes.length);
            $('#tblNotesList').find('tr:gt(0)').remove();
            for (j = 0; j < _Notes.length; j++) {
                var obj = _Notes[j];
                var tr = '<tr><td>' + obj.Notes + '<input type="hidden" value="' + obj.ID + '"/></td><td>' + obj.CommsType + '</td><td>' + obj.ChangedBy + '</td><td>' + obj.ChangedOn + '</td></tr>';
                $('#tblNotesList').append(tr);
            }
            FormatTable($('#tblNotesList'));
            
        }
        //' + BindCommTypeDropdown(obj.CommsTypeID) + '
        //<input type="text" style="width:250px;"  readonly="readonly" value="' + obj.Notes + '"/>
        //<td> <a href="javascript:void(0);" onclick="DeleteNotes(this);">Delete</a></td>
        //readonly="readonly"
        
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
                var tr = '<tr><td><input type="text" value="' + obj.Email + '"/><input type="hidden" value="' + obj.ID + '"/></td><td><a href="mailto:' + obj.Email + '">Email</a> &nbsp; <a href="javascript:void(0);" onclick="DeleteEmail(this);">Delete</a></td></tr>';
                $('#tblEmailAddressList').append(tr);
            }
            FormatTable($('#tblEmailAddressList'));

            //toggleAlert('1');

            //document.getElementById('txtLastName').style.visibility = "hidden";
            //document.getElementById('txtLastName').style.v = "none";

            //var control = document.getElementById('TextBox1');
            //if(control.style.visibility == "visible" || control.style.visibility == "")
                //control.style.visibility = "hidden";
            //else
                //control.style.visibility = "visible";
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
            _Personnel.PPE_Sizes = $('#<%= ddlPPE_Size.ClientID %>').val();
            _Personnel.Coverall = $('#<%= txtcoverall.ClientID %>').val();
            _Personnel.Boots = $('#<%= ddlbootsize.ClientID %>').val();
            _Personnel.companyname = $('#<%= txtcompanyname.ClientID %>').val();
            _Personnel.companyreg = $('#<%= txtcompanyreg.ClientID %>').val();
            _Personnel.companyvat = $('#<%= txtcompanyvat.ClientID %>').val();
            _Personnel.companyaddr = $('#<%= txtcompanyadr.ClientID %>').val();
            _Personnel.insurance = $('#<%= ddlinsurance.ClientID %>').val();
            _Personnel.employmentstatus = $('#<%= ddlemploymentstatus.ClientID %>').val();


            

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

        function PopulateNotes() {
            _Notes.length = 0;
            $('#tblNotesList tr:gt(0)').each(function() {

                if ($(this).find("input[type='hidden']").val() == 0) {
                    if ($(this).find('textarea').val().length > 0) {
                        var notes = new App.CustomModels.ConNote();
                        notes.ID = $(this).find("input[type='hidden']").val();
                        notes.Notes = $(this).find('textarea').val();
                        notes.CommsTypeID = $(this).find("select").val();
                        _Notes.push(notes);
                    }
                    //alert(notes.Notes);
                }
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
                    PopulateNotes();
                    PageMethods.SavePersonnel(_Personnel, _Telephones, _Notes, _Emails, _ContactRoles, SavePersonnel_Success, OnAjax_Error, OnAjax_TimeOut);
                    //            try {}
                    //            catch (E) {
                    //            }

               
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
        function SendSMS(anchorElement) {
            var telephone = $(anchorElement).parent().parent().find("input[type='text']").val();
            window.parent.ShowPopupToSendSMS(telephone);
        }
        //_Tr = $(anchorElement).parent().parent();
        //var id = $(_Tr).find("input[type='hidden']").val();
        $(document).ready(function() {
            PrepareCollection();
            BindTelephoneNumbers();
            BindNotes();
            BindRoles();
            BindEmailAddresses();
            //ShowParentLoading();
            SetParentHeight();
        });


//        function toggleAlert(T) {
//            if (T > 1)
//            {
//                document.getElementById('EditMode').style.visibility = "hidden";
//                //document.getElementById('TextBox1').style.visibility = "visible";
//            }
//            toggleDisabled(document.getElementById("content"));
//        }
//        function toggleDisabled(el) {

//            
//            try {
//                el.disabled = el.disabled ? false : true;
//            }
//            catch (E) {
//            }
//            if (el.childNodes && el.childNodes.length > 0) {
//                for (var x = 0; x < el.childNodes.length; x++) {
//                    toggleDisabled(el.childNodes[x]);
//                }
//            }
//        }
        //

//        function toggleVisibility(controlId) {
//            alert("YY");
//            var control = document.getElementById(controlId);

//            //if (control.style.visibility == "visible" || control.style.visibility == "")

//                control.style.visibility = "hidden";

//            //else

//              //  control.style.visibility = "visible";

//            alert("YY");

//        }
    </script>
    
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="body" Runat="Server">
    <div id="divMessage" runat="server" enableviewstate="false" visible="false"></div>
    <%--<textarea style="width:100%"></textarea>--%>
    <asp:HiddenField ID="hdnTelephoneNumbers" runat="server" />
    <asp:HiddenField ID="hdnNotes" runat="server" />
    <asp:HiddenField ID="hdnEmailAddresses" runat="server" />
    <asp:HiddenField ID="hdnContactRoles" runat="server" />
    <asp:HiddenField ID="hdnRoles" runat="server" />
    <asp:HiddenField ID="hdnTelephoneTypes" runat="server" />
    <asp:HiddenField ID="hdnCommTypes" runat="server" />


    <asp:Button ID="Button1" runat="server" Text="Edit" onclick="Button1_Click" />
  <%--  <input type="button" ID="btnShowHide" value="Show/Hide" onclick="toggleVisibility('TextBox1');" />
<asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
<input type="button" id="EditMode" class="ButtonCommon" value="Edit" onclick="toggleVisibility('txtLastName')" />
<div id="content">  
</div> --%>  

     <%--<div>
       <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
        <input type="button" id="btnShowHide" value="Show/Hide" onclick="toggleVisibility('txtLastName');" />
    </div>--%>
    <asp:Panel ID="pnlDetails" runat="server">
    
        
        <div class="LeftColumn">
             <asp:Panel ID="Panel_otherdetail" runat="server" Visible="true">
                <div class="WinGroupBox">
                <div class="WinGroupBoxHeader">
                    Other Details</div>
                <table cellpadding="3" cellspacing="0" style="width: 100%;">
                    <colgroup>
                        <col style="width: 30%;" />
                        <col />
                    </colgroup>
                    <tr>
                        <td>
                            Date Of Last Meeting
                        </td>
                        <td>
                            <asp:Label ID="lblDateOfLastMeeting" runat="server" Text=""></asp:Label>
                            <asp:TextBox ID="txtDateOfLastMeeting" MaxLength="50" CssClass="CalendarTextBox"
                                runat="server"></asp:TextBox>
                            <asp:CustomValidator ID="cvDateOfLastMeeting" runat="server" ControlToValidate="txtDateOfLastMeeting"
                                SetFocusOnError="true" ClientValidationFunction="ValidateDate" ErrorMessage="Please Select a Valid Date."
                                Display="Dynamic" ValidationGroup="SaveInfo">
                            </asp:CustomValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Preferred Day Rate
                        </td>
                        <td>
                            <table cellpadding="0" cellspacing="0" style="width: 100%;">
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtPreferredDayRate" MaxLength="4" runat="server"></asp:TextBox>
                                        <asp:Label ID="lblPreferredDayRate" runat="server" Text=""></asp:Label>    
                                        <asp:CompareValidator ID="cpvPreferredDayRate" runat="server" ControlToValidate="txtPreferredDayRate"
                                            SetFocusOnError="true" Operator="DataTypeCheck" Type="Double" ErrorMessage="Please Enter Digits Only."
                                            Display="Dynamic" ValidationGroup="SaveInfo">
                                        </asp:CompareValidator>
                                    </td>
                                    <td style="padding-left: 10px; width: 20%;">
                                        <asp:DropDownList ID="ddlDayRateCurrencyID" runat="server">
                                        </asp:DropDownList>
                                        <asp:Label ID="lblDayRateCurrencyID" runat="server" Text=""></asp:Label>
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
                        <td>
                            PPE Sizes
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlPPE_Size" runat="server">
                                <asp:ListItem>XM</asp:ListItem>
                                <asp:ListItem>M</asp:ListItem>
                                <asp:ListItem>L</asp:ListItem>
                                <asp:ListItem>XL</asp:ListItem>
                                <asp:ListItem>XXL</asp:ListItem>
                                <asp:ListItem>XXXL</asp:ListItem>
                            </asp:DropDownList>
                            <asp:Label ID="lblPPE_Size" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Coverall
                        </td>
                        <td>
                            <asp:TextBox ID="txtcoverall" runat="server" MaxLength="20" Width="107px"></asp:TextBox>
                            <asp:Label ID="lblCoverall" runat="server" Text=""></asp:Label>    
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Boot Size
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlbootsize" runat="server">
                                <asp:ListItem>4</asp:ListItem>
                                <asp:ListItem>5</asp:ListItem>
                                <asp:ListItem>6</asp:ListItem>
                                <asp:ListItem>7</asp:ListItem>
                                <asp:ListItem>8</asp:ListItem>
                                <asp:ListItem>9</asp:ListItem>
                                <asp:ListItem>10</asp:ListItem>
                                <asp:ListItem>11</asp:ListItem>
                                <asp:ListItem>12</asp:ListItem>
                                <asp:ListItem>13</asp:ListItem>
                                <asp:ListItem>14</asp:ListItem>
                                <asp:ListItem>15</asp:ListItem>
                            </asp:DropDownList>
                            <asp:Label ID="lblBootsize" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            No SMS or Email
                        </td>
                        <td>
                            <asp:CheckBox ID="chkNoSMSorEmail" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Inactive
                        </td>
                        <td>
                            <asp:CheckBox ID="chkInactive" runat="server" />
                        </td>
                    </tr>
                </table>
            </div>     
         </asp:Panel>
           <asp:Panel ID="main_details" runat="server" Visible="false">
            <div class="WinGroupBox">
                <div class="WinGroupBoxHeader">Personnel Details</div>
               <%-- <input type="text" />--%>
                <table cellpadding="3" cellspacing="0" style="width:100%;">
                    <colgroup>
                        <col style="width:25%;" />
                        <col />                                        
                    </colgroup>				
			        <tr>
				        <td>Surname<span class="requiredMark">*</span></td>
				        <td>
					        <asp:TextBox ID="txtLastName" MaxLength="50" runat="server"></asp:TextBox>
                            <asp:Label ID="lblLastName" runat="server" Text=""></asp:Label>
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
					        <asp:Label ID="lblFirstNames" runat="server" Text=""></asp:Label>
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
					        <asp:Label ID="lblAddress" runat="server" Text=""></asp:Label>
				        </td>
			        </tr>
			        <tr>
				        <td>Postcode</td>
				        <td>
					        <asp:TextBox ID="txtPostcode" MaxLength="20" runat="server"></asp:TextBox>
					        <asp:Label ID="lblPostcode" runat="server" Text=""></asp:Label>
				        </td>
			        </tr>
			        <tr>
				        <td>Country</td>
				        <td>
					        <asp:DropDownList ID="ddlCountryID" runat="server"></asp:DropDownList>
					        <asp:Label ID="lblCountryID" runat="server" Text=""></asp:Label>
				        </td>
			        </tr>				
			        <tr>
				        <td>Marital Status<span class="requiredMark">*</span></td>
				        <td>
				            <asp:Label ID="lblMaritalStatusID" runat="server" Text=""></asp:Label>
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
					        <asp:Label ID="lblPlaceOfBirth" runat="server" Text=""></asp:Label>
				        </td>
			        </tr>
			        <tr>
				        <td>Country Of Birth</td>
				        <td>
					        <asp:DropDownList ID="ddlCountryOfBirthID" runat="server"></asp:DropDownList>
					        <asp:Label ID="lblCountryOfBirthID" runat="server" Text=""></asp:Label>
				        </td>
			        </tr>
			        <tr>
				        <td>Date Of Birth</td>
				        <td>
					        <asp:TextBox ID="txtDateOfBirth" MaxLength="50" CssClass="CalendarTextBox" runat="server"></asp:TextBox>
					        <asp:Label ID="lblDateOfBirth" runat="server" Text=""></asp:Label>
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
            </asp:Panel>
       

        </div>
            
        <div class="RightColumn">
        
           <asp:Panel ID="Panel_Phone" runat="server" Visible="false">
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

           </asp:Panel> 

   <asp:Panel ID="Panel_email" runat="server" Visible="false">

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
    </asp:Panel>    
        
    

             <asp:Panel ID="Panel_empdetail" runat="server" Visible="true">
                 <div class="WinGroupBox">
                <div class="WinGroupBoxHeader">
                    Employment Details</div>
                <table cellpadding="3" cellspacing="0" style="width: 100%;">
                    <colgroup>
                        <col style="width: 30%;" />
                        <col />
                    </colgroup>
                    <tr>
                        <td>
                            Company
                        </td>
                        <td>
                            <asp:TextBox ID="txtcompanyname" runat="server" MaxLength="20" Width="248px"></asp:TextBox>
                            <asp:Label ID="lblCompanyname" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Reg
                        </td>
                        <td>
                            <asp:TextBox ID="txtcompanyreg" runat="server" MaxLength="20" Width="248px"></asp:TextBox>
                            <asp:Label ID="lblCompanyreg" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Vat
                        </td>
                        <td>
                            <asp:TextBox ID="txtcompanyvat" runat="server" MaxLength="20" Width="248px"></asp:TextBox>
                            <asp:Label ID="lblCompanyvat" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Address
                        </td>
                        <td>
                            <asp:TextBox ID="txtcompanyadr" runat="server" MaxLength="200" TextMode="MultiLine"></asp:TextBox>
                            <asp:Label ID="lblCompanyadr" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Employment Status
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlemploymentstatus" runat="server">
                                <asp:ListItem>Self Employed</asp:ListItem>
                                <asp:ListItem>LTD</asp:ListItem>
                                <asp:ListItem>Sole Trader</asp:ListItem>
                            </asp:DropDownList>
                            <asp:Label ID="lblEmploymentstatus" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Insurance
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlinsurance" runat="server">
                                <asp:ListItem>OWN</asp:ListItem>
                                <asp:ListItem>OMM</asp:ListItem>
                            </asp:DropDownList>
                            <asp:Label ID="lblInsurance" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                </table>
            </div>        
              </asp:Panel>
            

            <asp:Panel ID="Panel_role" runat="server" Visible="true">
             <div class="WinGroupBox">
                <div class="WinGroupBoxHeader">
                    Roles</div>
                <div id="div1">
                    <div class="AddNewLink">
                        <a href="javascript:void(0);" onclick="AddNewRoleRow();">Add New Role</a>
                    </div>
                    <table id="tblRolesList" class="GridView" cellpadding="3" cellspacing="0">
                        <colgroup>
                            <col style="width: 35%;" />
                            <col style="width: 35%;" />
                            <col />
                        </colgroup>
                        <tr>
                            <th>
                                Order
                            </th>
                            <th>
                                Role
                            </th>
                            <th>
                                Actions
                            </th>
                        </tr>
                    </table>
                </div>
            </div>
            </asp:Panel>
           
        </div>
        <div class="clearboth"></div>
        <div class="TenPixelTopMargin" id="Div2">
            &nbsp;
        </div>
             <asp:Panel ID="Panel_notes" runat="server" Visible="false">
        <div class="WinGroupBox">
                <div class="WinGroupBoxHeader">Notes</div>
                <div id="divNotesList">
                    <div>
                        <asp:CustomValidator ID="cvNotesList" runat="server"
                            Display="Dynamic" ValidateEmptyText="true"
                            ClientValidationFunction="ValidateNotesList"
                            ValidationGroup="SaveInfo"
                            ErrorMessage="Please Enter Notes.">
                        </asp:CustomValidator>
                    </div>
                    <div class="AddNewLink">
                        <a href="javascript:void(0);" onclick="AddNewNotesRow();">Add New Notes</a>
                    </div>
                    <table id="tblNotesList" class="GridView" cellpadding="3" cellspacing="0">
                        <colgroup>
                            <col style="width:60%;" />
                            <col style="width:10%;" />                  
                            <col style="width:20%;" />
                            <col style="width:10%;" />
                        </colgroup>
                        <tr>
                            <th>Notes</th>
                            <th>Comm Type</th>                                
                            <th>Changed By</th>
                            <th>Changed On</th>
                        </tr>
                    </table>
                </div>
            </div>
         </asp:Panel>
        
        <div class="TenPixelTopMargin" id="dvSaveBtn">
            <input type="button" class="ButtonCommon" value="Save" onclick="SavePersonnel();" />
        </div>        
    </asp:Panel>   
        
</asp:Content>

