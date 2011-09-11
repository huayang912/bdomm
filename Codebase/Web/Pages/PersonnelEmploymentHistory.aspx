<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPagePopup.master" AutoEventWireup="true" CodeFile="PersonnelEmploymentHistory.aspx.cs" Inherits="Pages_PersonnelEmploymentHistory" %>
<%@ Register Src="~/Controls/DataTableList.ascx" TagName="DataTableList" TagPrefix="UC" %>
<%@ Register Src="~/Controls/Pager.ascx" TagName="Pager" TagPrefix="UC" %>
<%@ Register Src="~/Controls/jQueryCalendar.ascx" TagName="jQueryCalendar" TagPrefix="UC" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <UC:jQueryCalendar ID="ucjQueryCalendar" runat="server" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="body" Runat="Server">
    <div>
        <div id="divMessage" runat="server" visible="false" enableviewstate="false"></div>        
        <asp:Panel ID="pnlFormContainer" runat="server" DefaultButton="btnSave">
                        
            <div class="WinGroupBox">                                
                <div class="WinGroupBoxHeader">Employment History</div>
                
                <table cellpadding="3" cellspacing="0" style="width:100%;">
                    <colgroup>
                        <col style="width:15%;" />
                        <col style="width:35%;" />
                        <col style="width:15%;" />
                        <col />                                        
                    </colgroup>

				    <%--<tr>
					    <td>Contact<span class="requiredMark">*</span></td>
					    <td>
						    <asp:DropDownList ID="ddlContactID" runat="server"></asp:DropDownList>
						    <asp:RequiredFieldValidator ID="rfvContactID" runat="server"
							    ControlToValidate="ddlContactID" SetFocusOnError="true"
							    ErrorMessage="Please Select a Contact." Display="Dynamic"
							    ValidationGroup="SaveInfo">
						    </asp:RequiredFieldValidator>
					    </td>
				    </tr>--%>
				    <tr>
					    <td>Start Date</td>
					    <td>
						    <asp:TextBox ID="txtStartDate" MaxLength="50" CssClass="CalendarTextBox" runat="server"></asp:TextBox>
						    <asp:CustomValidator ID="cvStartDate" runat="server"
							    ControlToValidate="txtStartDate" SetFocusOnError="true"
							    ClientValidationFunction="ValidateDate"
							    ErrorMessage="Please Select a Valid Date." Display="Dynamic"
							    ValidationGroup="SaveInfo">
						    </asp:CustomValidator>
					    </td>
					    <td>End Date</td>
					    <td>
						    <asp:TextBox ID="txtEndDate" MaxLength="50" CssClass="CalendarTextBox" runat="server"></asp:TextBox>
						    <asp:CustomValidator ID="cvEndDate" runat="server"
							    ControlToValidate="txtEndDate" SetFocusOnError="true"
							    ClientValidationFunction="ValidateDate"
							    ErrorMessage="Please Select a Valid Date." Display="Dynamic"
							    ValidationGroup="SaveInfo">
						    </asp:CustomValidator>
					    </td>
				    </tr>				    
				    <tr>
					    <td>Project</td>
					    <td>
						    <asp:DropDownList ID="ddlProjectID" runat="server"></asp:DropDownList>
					    </td>
					    <td>Client</td>
					    <td>
						    <asp:DropDownList ID="ddlClientID" runat="server"></asp:DropDownList>
					    </td>
				    </tr>				   
				    <tr>
					    <td>Role</td>
					    <td>
						    <asp:DropDownList ID="ddlRoleID" runat="server"></asp:DropDownList>
					    </td>
					    <td>Day Rate</td>
					    <td>
						    <asp:TextBox ID="txtDayRate" MaxLength="10" runat="server"></asp:TextBox>
						    <asp:CompareValidator ID="cpvDayRate" runat="server"
							    ControlToValidate="txtDayRate" SetFocusOnError="true"
							    Operator="DataTypeCheck" Type="Double"
							    ErrorMessage="Please Enter Digits Only." Display="Dynamic"
							    ValidationGroup="SaveInfo">
						    </asp:CompareValidator>
					    </td>
				    </tr>				    
				    <tr>
					    <td>Notes</td>
					    <td colspan="3">
						    <asp:TextBox ID="txtNotes" TextMode="MultiLine" MaxLength="500" runat="server"></asp:TextBox>
					    </td>
				    </tr>
				    
				    
				    <tr>
					    <td>ProjectCodeother</td>
					    <td>
						    <asp:TextBox ID="txtProjectCodeother" MaxLength="30" runat="server"></asp:TextBox>
					    </td>
					    
					    <td>TravelRate</td>
					    <td>
						    <asp:TextBox ID="txtTravelRate" MaxLength="4" runat="server"></asp:TextBox>
						    <asp:CompareValidator ID="cpvTravelRate" runat="server"
							    ControlToValidate="txtTravelRate" SetFocusOnError="true"
							    Operator="DataTypeCheck" Type="Integer"
							    ErrorMessage="Please Enter Digits Only." Display="Dynamic"
							    ValidationGroup="SaveInfo">
						    </asp:CompareValidator>
					    </td>
				    </tr>
				    
				    <tr>
					    <td>TravelCost</td>
					    <td>
						    <asp:TextBox ID="txtTravelCost" MaxLength="4" runat="server"></asp:TextBox>
						    <asp:CompareValidator ID="cpvTravelCost" runat="server"
							    ControlToValidate="txtTravelCost" SetFocusOnError="true"
							    Operator="DataTypeCheck" Type="Integer"
							    ErrorMessage="Please Enter Digits Only." Display="Dynamic"
							    ValidationGroup="SaveInfo">
						    </asp:CompareValidator>
					    </td>
					     <td>OffshoreRate</td>
					    <td>
						    <asp:TextBox ID="txtOffshoreRate" MaxLength="4" runat="server"></asp:TextBox>
						    <asp:CompareValidator ID="cpvOffshoreRate" runat="server"
							    ControlToValidate="txtOffshoreRate" SetFocusOnError="true"
							    Operator="DataTypeCheck" Type="Double"
							    ErrorMessage="Please Enter Digits Only." Display="Dynamic"
							    ValidationGroup="SaveInfo">
						    </asp:CompareValidator>
					    </td>
				    </tr>
				    
				    <tr>
					    <td>Contractdays</td>
					    <td>
						    <asp:TextBox ID="txtContractdays" MaxLength="4" runat="server"></asp:TextBox>
						    <asp:CompareValidator ID="cpvContractdays" runat="server"
							    ControlToValidate="txtContractdays" SetFocusOnError="true"
							    Operator="DataTypeCheck" Type="Integer"
							    ErrorMessage="Please Enter Digits Only." Display="Dynamic"
							    ValidationGroup="SaveInfo">
						    </asp:CompareValidator>
					    </td>
					    
					    <td>Currency Code</td>
					    <td>
						    <asp:DropDownList ID="ddlCurrencyCode" runat="server"></asp:DropDownList>
					    </td>
				    </tr>
				    
				    <tr>
				        <td>Office Onsh Rate type</td>
					    <td>
						    <asp:TextBox ID="txtOfficeOnshRatetype" MaxLength="30" runat="server"></asp:TextBox>
					    </td>
					    <td>Office Onshore Rate</td>
					    <td>
						    <asp:TextBox ID="txtOfficeOnshoreRate" MaxLength="4" runat="server"></asp:TextBox>
						    <asp:CompareValidator ID="CompareValidator1" runat="server"
							    ControlToValidate="txtOfficeOnshoreRate" SetFocusOnError="true"
							    Operator="DataTypeCheck" Type="Double"
							    ErrorMessage="Please Enter Digits Only." Display="Dynamic"
							    ValidationGroup="SaveInfo">
						    </asp:CompareValidator>
					    </td>
				    </tr>
				    
				    
				    <tr>
					    <td>HourStandbyRatetype</td>
					    <td>
						    <asp:TextBox ID="txtHourStandbyRatetype" MaxLength="30" runat="server"></asp:TextBox>
					    </td>
				    
					    <td>HourStandbyRate</td>
					    <td>
						    <asp:TextBox ID="txtHourStandbyRate" MaxLength="4" runat="server"></asp:TextBox>
						    <asp:CompareValidator ID="cpvHourStandbyRate" runat="server"
							    ControlToValidate="txtHourStandbyRate" SetFocusOnError="true"
							    Operator="DataTypeCheck" Type="Double"
							    ErrorMessage="Please Enter Digits Only." Display="Dynamic"
							    ValidationGroup="SaveInfo">
						    </asp:CompareValidator>
					    </td>
				    </tr>
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
				    
				    --%>                    
                </table>
                
            </div>        
            <div class="TenPixelTopMargin">
                <asp:Button ID="btnSave" runat="server" Text="Save" ValidationGroup="SaveInfo" OnClick="btnSave_Click" />            
                <%--<asp:Button ID="btnList" runat="server" Text="View List" OnClick="btnList_Click" />--%>
            </div>
            
            <div class="WinGroupBox">
                <div class="WinGroupBoxHeader" style="width:160px;">Employment History List</div>
                
                <UC:DataTableList ID="ucEmploymentHistoryList" runat="server"
                    ExcludeVisibleFields="ID, ContactID"
                    LinkFields="ContactID, ID"
                    NoRecordMessgae="No Employment History Found for this Personnel."
                    DeleteMessage="Sure to Delete Employment History?">
                </UC:DataTableList>
                
                <%--<div class="TenPixelTopMargin">
                    <UC:Pager ID="ucNoteListPager" runat="server" TotalRecordMessage="Total {0} Note(s) Found." OnPageIndexChanged="ucNoteListPager_PageIndexChanged" />
                </div>--%>
            </div>                
        </asp:Panel>   
    </div> 
</asp:Content>

