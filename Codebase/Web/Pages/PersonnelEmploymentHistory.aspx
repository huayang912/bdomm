<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPagePopup.master" AutoEventWireup="true" 
CodeFile="PersonnelEmploymentHistory.aspx.cs" Inherits="Pages_PersonnelEmploymentHistory" %>
<%@ Register Src="~/Controls/DataTableList.ascx" TagName="DataTableList" TagPrefix="UC" %>
<%@ Register Src="~/Controls/Pager.ascx" TagName="Pager" TagPrefix="UC" %>
<%@ Register Src="~/Controls/jQueryCalendar.ascx" TagName="jQueryCalendar" TagPrefix="UC" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <UC:jQueryCalendar ID="ucjQueryCalendar" runat="server" />
    
    <style type="text/css">
        .LeftColumn{width:30%; float:left;}
        .CenterColumn{width:30%; float:left; margin-left:10px;}
        .RightColumn{width:30%; float:left; margin-left:10px;}
        .AddNewLink{margin-bottom:5px;}
    </style> 
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="Server">
    <div>
        <div id="divMessage" runat="server" visible="false" enableviewstate="false">
        </div>
        <asp:Panel ID="pnlFormContainer" runat="server" DefaultButton="btnSave">
            <asp:Panel runat="server" ID="pnlSkills" Visible="true" BackColor="AliceBlue">
                
                <asp:Panel runat="server" ID="pnlEditButton" Visible="true">
                    <asp:Button ID="btnEnableEdit" runat="server" Text="Edit" onclick="btnEnableEdit_Click" />
                </asp:Panel>
                <asp:Panel runat="server" ID="pnlSaveButton" Visible="false">
                    <asp:Button ID="btnSaveSkills" runat="server" Text="Save" 
                        ValidationGroup="SaveInfo" OnClick="btnSaveSkills_Click" />
                </asp:Panel>
                
                
                <div class="LeftColumn">
                    <asp:Panel ID="Panel_otherdetail" runat="server" Visible="true">
                        <div class="WinGroupBox">
                            <div class="WinGroupBoxHeader">
                                Other Details</div>
                            <table cellpadding="3" cellspacing="0" style="width: 100%;">
                                <colgroup>
                                    <col style="width: 40%;" />
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
                            <div class="WinGroupBoxHeader">
                                Personnel Details</div>
                            <%-- <input type="text" />--%>
                            <table cellpadding="3" cellspacing="0" style="width: 100%;">
                                <colgroup>
                                    <col style="width: 25%;" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>
                                        Surname<span class="requiredMark">*</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtLastName" MaxLength="50" runat="server"></asp:TextBox>
                                        <asp:Label ID="lblLastName" runat="server" Text=""></asp:Label>
                                        <asp:RequiredFieldValidator ID="rfvLastName" runat="server" ControlToValidate="txtLastName"
                                            SetFocusOnError="true" ErrorMessage="Please Enter a LastName." Display="Dynamic"
                                            ValidationGroup="SaveInfo">
                                        </asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        First Name<span class="requiredMark">*</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFirstNames" MaxLength="50" runat="server"></asp:TextBox>
                                        <asp:Label ID="lblFirstNames" runat="server" Text=""></asp:Label>
                                        <asp:RequiredFieldValidator ID="rfvFirstNames" runat="server" ControlToValidate="txtFirstNames"
                                            SetFocusOnError="true" ErrorMessage="Please Enter a FirstNames." Display="Dynamic"
                                            ValidationGroup="SaveInfo">
                                        </asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Address
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAddress" TextMode="MultiLine" MaxLength="200" runat="server"></asp:TextBox>
                                        <asp:Label ID="lblAddress" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Postcode
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPostcode" MaxLength="20" runat="server"></asp:TextBox>
                                        <asp:Label ID="lblPostcode" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Country
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlCountryID" runat="server">
                                        </asp:DropDownList>
                                        <asp:Label ID="lblCountryID" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Marital Status<span class="requiredMark">*</span>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblMaritalStatusID" runat="server" Text=""></asp:Label>
                                        <asp:DropDownList ID="ddlMaritalStatusID" runat="server">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rfvMaritalStatusID" runat="server" ControlToValidate="ddlMaritalStatusID"
                                            SetFocusOnError="true" ErrorMessage="Please Select a MaritalStatus." Display="Dynamic"
                                            ValidationGroup="SaveInfo">
                                        </asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Place Of Birth
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPlaceOfBirth" MaxLength="100" runat="server"></asp:TextBox>
                                        <asp:Label ID="lblPlaceOfBirth" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Country Of Birth
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlCountryOfBirthID" runat="server">
                                        </asp:DropDownList>
                                        <asp:Label ID="lblCountryOfBirthID" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Date Of Birth
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDateOfBirth" MaxLength="50" CssClass="CalendarTextBox" runat="server"></asp:TextBox>
                                        <asp:Label ID="lblDateOfBirth" runat="server" Text=""></asp:Label>
                                        <asp:CustomValidator ID="cvDateOfBirth" runat="server" ControlToValidate="txtDateOfBirth"
                                            SetFocusOnError="true" ClientValidationFunction="ValidateDate" ErrorMessage="Please Select a Valid Date."
                                            Display="Dynamic" ValidationGroup="SaveInfo">
                                        </asp:CustomValidator>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </asp:Panel>
                </div>
                <div class="CenterColumn">
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
                </div>
                <div class="RightColumn">
                    <div class="WinGroupBox">
                        <div class="WinGroupBoxHeader">
                            Roles</div>
                        <div id="div1">
                            
                            <asp:DropDownList ID="ddlRoles" runat="server"></asp:DropDownList>
                            <asp:Button ID="btnAddRoles" runat="server" Text="Add" OnClick="btnAddRoles_Click" />
                            
                            <UC:DataTableList ID="ucContactRoles" runat="server" 
                                ExcludeVisibleFields="ContactID,ID"
                                LinkFields="ContactID, ID" 
                                NoRecordMessgae="No Role Found for this Personnel."
                                DeleteMessage="Sure to Delete Role?">
                            </UC:DataTableList>
                            
                            
                            
                            <%--<div class="AddNewLink">
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
                            </table>--%>
                        </div>
                    </div>
                    
                    
                </div>
                <div class="clearboth">
                </div>
                
                <div class="TenPixelTopMargin"></div>
            </asp:Panel>
            
            <asp:Panel runat="server" ID="pnlEmpHistory" Visible="false">
                <div class="WinGroupBox">
                    <div class="WinGroupBoxHeader">
                        Employment History</div>
                    <table cellpadding="3" cellspacing="0" style="width: 100%;">
                        <colgroup>
                            <col style="width: 15%;" />
                            <col style="width: 35%;" />
                            <col style="width: 15%;" />
                            <col />
                        </colgroup>
                        <tr>
                            <td>
                                Start Date
                            </td>
                            <td>
                                <asp:TextBox ID="txtStartDate" MaxLength="50" CssClass="CalendarTextBox" runat="server"></asp:TextBox>
                                <asp:CustomValidator ID="cvStartDate" runat="server" ControlToValidate="txtStartDate"
                                    SetFocusOnError="true" ClientValidationFunction="ValidateDate" ErrorMessage="Please Select a Valid Date."
                                    Display="Dynamic" ValidationGroup="SaveInfo">
                                </asp:CustomValidator>
                            </td>
                            <td>
                                End Date
                            </td>
                            <td>
                                <asp:TextBox ID="txtEndDate" MaxLength="50" CssClass="CalendarTextBox" runat="server"></asp:TextBox>
                                <asp:CustomValidator ID="cvEndDate" runat="server" ControlToValidate="txtEndDate"
                                    SetFocusOnError="true" ClientValidationFunction="ValidateDate" ErrorMessage="Please Select a Valid Date."
                                    Display="Dynamic" ValidationGroup="SaveInfo">
                                </asp:CustomValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Project
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlProjectID" runat="server">
                                </asp:DropDownList>
                            </td>
                            <td>
                                Project Code Other
                            </td>
                            <td>
                                <asp:TextBox ID="txtProjectCodeother" MaxLength="30" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Role
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlRoleID" runat="server">
                                </asp:DropDownList>
                            </td>
                            <td>
                                Currency Code
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlCurrencyCode" runat="server">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Travel Rate
                            </td>
                            <td>
                                <asp:TextBox ID="txtTravelRate" MaxLength="4" runat="server"></asp:TextBox>
                                <asp:CompareValidator ID="cpvTravelRate" runat="server" ControlToValidate="txtTravelRate"
                                    SetFocusOnError="true" Operator="DataTypeCheck" Type="Integer" ErrorMessage="Please Enter Digits Only."
                                    Display="Dynamic" ValidationGroup="SaveInfo">
                                </asp:CompareValidator>
                            </td>
                            <td>
                                Travel Cost
                            </td>
                            <td>
                                <asp:TextBox ID="txtTravelCost" MaxLength="4" runat="server"></asp:TextBox>
                                <asp:CompareValidator ID="cpvTravelCost" runat="server" ControlToValidate="txtTravelCost"
                                    SetFocusOnError="true" Operator="DataTypeCheck" Type="Integer" ErrorMessage="Please Enter Digits Only."
                                    Display="Dynamic" ValidationGroup="SaveInfo">
                                </asp:CompareValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Day Rate
                            </td>
                            <td>
                                <asp:TextBox ID="txtDayRate" MaxLength="10" runat="server"></asp:TextBox>
                                <asp:CompareValidator ID="cpvDayRate" runat="server" ControlToValidate="txtDayRate"
                                    SetFocusOnError="true" Operator="DataTypeCheck" Type="Double" ErrorMessage="Please Enter Digits Only."
                                    Display="Dynamic" ValidationGroup="SaveInfo">
                                </asp:CompareValidator>
                            </td>
                            <td>
                                Offshore Rate#
                            </td>
                            <td>
                                <asp:TextBox ID="txtOffshoreRate" MaxLength="4" runat="server"></asp:TextBox>
                                <asp:CompareValidator ID="cpvOffshoreRate" runat="server" ControlToValidate="txtOffshoreRate"
                                    SetFocusOnError="true" Operator="DataTypeCheck" Type="Double" ErrorMessage="Please Enter Digits Only."
                                    Display="Dynamic" ValidationGroup="SaveInfo">
                                </asp:CompareValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Rate Type
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlRateType" runat="server">
                                    <asp:ListItem Selected="True" Text="N/A" Value="N/A"></asp:ListItem>
                                    <asp:ListItem Text="Office Rate" Value="Office Rate"></asp:ListItem>
                                    <asp:ListItem Text="Ofshore Rate" Value="Ofshore Rate"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                Rate
                            </td>
                            <td>
                                <asp:TextBox ID="txtOfficeOnshoreRate" MaxLength="4" runat="server"></asp:TextBox>
                                <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToValidate="txtOfficeOnshoreRate"
                                    SetFocusOnError="true" Operator="DataTypeCheck" Type="Double" ErrorMessage="Please Enter Digits Only."
                                    Display="Dynamic" ValidationGroup="SaveInfo">
                                </asp:CompareValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Hour/Standby
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlHourStandby" runat="server">
                                    <asp:ListItem Selected="True" Text="N/A" Value="N/A"></asp:ListItem>
                                    <asp:ListItem Text="Hourly Rate" Value="Hourly Rate"></asp:ListItem>
                                    <asp:ListItem Text="Standby Rate" Value="Standby Rate"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                Hourly/Standby Rate
                            </td>
                            <td>
                                <asp:TextBox ID="txtHourStandbyRate" MaxLength="4" runat="server"></asp:TextBox>
                                <asp:CompareValidator ID="cpvHourStandbyRate" runat="server" ControlToValidate="txtHourStandbyRate"
                                    SetFocusOnError="true" Operator="DataTypeCheck" Type="Double" ErrorMessage="Please Enter Digits Only."
                                    Display="Dynamic" ValidationGroup="SaveInfo">
                                </asp:CompareValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Contract Days#
                            </td>
                            <td>
                                <asp:RadioButton ID="radNull" Checked="true" GroupName="conDays" Text="N/A" runat="server" />
                                <asp:RadioButton ID="radFive" GroupName="conDays" Text="5 Days" runat="server" />
                                <asp:RadioButton ID="radSeven" GroupName="conDays" Text="7 Days" runat="server" />
                                <%--<asp:TextBox ID="txtContractdays" MaxLength="4" runat="server"></asp:TextBox>
                            <asp:CompareValidator ID="cpvContractdays" runat="server" ControlToValidate="txtContractdays"
                                SetFocusOnError="true" Operator="DataTypeCheck" Type="Integer" ErrorMessage="Please Enter Digits Only."
                                Display="Dynamic" ValidationGroup="SaveInfo">
                            </asp:CompareValidator> --%>
                            </td>
                            <td>
                                Notes
                            </td>
                            <td>
                                <asp:TextBox ID="txtNotes" TextMode="MultiLine" MaxLength="500" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <%--<tr>
                        <td>
                            Contract Days
                        </td>
                        <td>
                            
                        </td>
                    </tr>
				    
                    <tr>
                        <td>
                            Office Onsh Rate type
                        </td>
                        <td>
                            <asp:TextBox ID="txtOfficeOnshRatetype" MaxLength="30" runat="server"></asp:TextBox>
                        </td>
                    </tr>
				    <tr>
                        <td>
                            Office Onshore Rate
                        </td>
                        <td>
                            
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Hour Standby Rate type
                        </td>
                        <td>
                            <asp:TextBox ID="txtHourStandbyRatetype" MaxLength="30" runat="server"></asp:TextBox>
                        </td>
                    </tr>
				    <tr>
                        <td>
                            Hour Standby Rate
                        </td>
                        <td>
                            
                        </td>
                    </tr>
                    
                    <tr>
                        <td>
                            Client
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlClientID" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>--%>
                    </table>
                    <%--<tr>
					    <td>ChangedByUser</td>
					    <td>
						    <asp:DropDownList ID="ddlChangedByUserID" runat="server"></asp:DropDownList>
					    </td>
				    </tr>
				   <tr>
					    <td>ChangedOn<span>*</span></td>
					    <td>
						    <asp:TextBox ID="txtChangedOn" MaxLength="50" CssClass="CalendarTextBox" runat="server"></asp:TextBox>
						    <asp:RequiredFieldValidator ID="rfvChangedOn" runat="server"
							    ControlToValidate="ddlChangedOn" SetFocusOnError="true"
							    ErrorMessage="Please Select a ChangedOn." Display="Dynamic"
							    ValidationGroup="SaveInfo">
						    </asp:RequiredFieldValidator>
						    <asp:CustomValidator ID="cvChangedOn" runat="server"
							    ControlToValidate="txtChangedOn" SetFocusOnError="true"
							    ClientValidationFunction="ValidateDate"
							    ErrorMessage="Please Select a Valid Date." Display="Dynamic"
							    ValidationGroup="SaveInfo">
						    </asp:CustomValidator>
					    </td>
				    </tr>
				    <tr>
					    <td>Version<span class="requiredMark">*</span></td>
					    <td>
						    <asp:TextBox ID="txtVersion" MaxLength="8" runat="server"></asp:TextBox>
						    <asp:RequiredFieldValidator ID="rfvVersion" runat="server"
							    ControlToValidate="txtVersion" SetFocusOnError="true"
							    ErrorMessage="Please Enter a Version." Display="Dynamic"
							    ValidationGroup="SaveInfo">
						    </asp:RequiredFieldValidator>
					    </td>
				    </tr>
				    
				    <tr>
					    
				    </tr>
				    
				    <tr>
					    <td>Currency</td>
					    <td>
						    <asp:TextBox ID="txtCurrencyID" MaxLength="4" runat="server"></asp:TextBox>
						    <asp:CompareValidator ID="cpvCurrencyID" runat="server"
							    ControlToValidate="txtCurrencyID" SetFocusOnError="true"
							    Operator="DataTypeCheck" Type="Integer"
							    ErrorMessage="Please Enter Digits Only." Display="Dynamic"
							    ValidationGroup="SaveInfo">
						    </asp:CompareValidator>
					    </td>
				    </tr>
				    <tr>
					   
				    </tr>
				    <tr>
					    <td>OfficeOnshRatetype</td>
					    <td>
						    <asp:TextBox ID="txtOfficeOnshRatetype" MaxLength="30" runat="server"></asp:TextBox>
					    </td>
				    </tr>
				    <tr>
					    <td>OfficeOnshoreRate</td>
					    <td>
						    <asp:TextBox ID="txtOfficeOnshoreRate" MaxLength="4" runat="server"></asp:TextBox>
						    <asp:CompareValidator ID="cpvOfficeOnshoreRate" runat="server"
							    ControlToValidate="txtOfficeOnshoreRate" SetFocusOnError="true"
							    Operator="DataTypeCheck" Type="Double"
							    ErrorMessage="Please Enter Digits Only." Display="Dynamic"
							    ValidationGroup="SaveInfo">
						    </asp:CompareValidator>
					    </td>
				    </tr>
				    
				    
                </table>--%>
                </div>
                <div class="TenPixelTopMargin">
                    <asp:Button ID="btnSave" runat="server" Text="Save" ValidationGroup="SaveInfo" OnClick="btnSave_Click" />
                </div>
            
            <div class="clearboth">
            </div>
          </asp:Panel>
            
            
            <div class="WinGroupBox">
                <div class="WinGroupBoxHeader" style="width: 160px;">
                    Employment History List</div>
                
                <asp:Button ID="btnShowEmpDetails" runat="server" Text="Add New Details" 
                OnClick="btnShowEmpDetails_Click" />
                    
                <UC:DataTableList ID="ucEmploymentHistoryList" runat="server" ExcludeVisibleFields="ID, ContactID"
                    LinkFields="ContactID, ID" NoRecordMessgae="No Employment History Found for this Personnel."
                    DeleteMessage="Sure to Delete Employment History?"></UC:DataTableList>
                <%--<div class="TenPixelTopMargin">
                    <UC:Pager ID="ucNoteListPager" runat="server" TotalRecordMessage="Total {0} Note(s) Found." OnPageIndexChanged="ucNoteListPager_PageIndexChanged" />
                </div>--%>
            </div>
            
            
        </asp:Panel>
        
        
    </div>
</asp:Content>

