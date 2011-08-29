<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPagePopup.master" AutoEventWireup="true" 
CodeFile="PersonnelTravelDetails.aspx.cs" Inherits="Pages_PersonnelTravelDetails" %>


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
                <div class="WinGroupBoxHeader">Basis Travel Details</div>
                <table cellpadding="3" cellspacing="0" style="width: 100%;">
                    <tr>
                        <td>
                            Frequent Flyer Number:
                        </td>
                        <td>
                            <asp:TextBox ID="tbxFrequentFlNumber" runat="server"></asp:TextBox>
                        </td>
                        <td>
                            Preferred Airport:
                        </td>
                    
                        <td>
                            <asp:TextBox ID="tbxPreferredAirport" runat="server"></asp:TextBox>
                        </td>
                     </tr>
                    <tr>   
                        <td>
                            Closetst Airport:
                        </td>
                        <td>
                            <asp:TextBox ID="tbxClosestAirport" runat="server"></asp:TextBox>
                        </td>
                        <td></td>
                        <td>
                            <asp:TextBox ID="tbxUpdateSave" runat="server" Visible = "false"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </div>
            
            <div class="TenPixelTopMargin">
                <asp:Button ID="btnSaveBasicInfo" runat="server" Text="Save" 
                ValidationGroup="SaveInfo" OnClick="btnSaveBasicInfo_Click"/>                
            </div>
            
            <div class="WinGroupBox">
                <div class="WinGroupBoxHeader">
                    Passport Details</div>
                <table cellpadding="3" cellspacing="0" style="width: 100%;">
                    <tr>
                        <td>
                            Number
                        </td>
                        <td>
                            <asp:TextBox ID="tbxNumber" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                                SetFocusOnError="true"
                                Display="Dynamic" 
                                ControlToValidate="tbxNumber" 
                                ErrorMessage="Please Enter a Number."
                                ValidationGroup="SavePInfo">
                            </asp:RequiredFieldValidator>
                        </td>
                        <td>
                            Where Issued
                        </td>
                        <td>
                            <asp:TextBox ID="tbxWhereIssued" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Expiry Date
                        </td>
                        <td>
                            
                            <asp:TextBox ID="tbxExpiryDate" MaxLength="50" 
                                CssClass="CalendarTextBox" runat="server"></asp:TextBox>
                            <asp:CustomValidator ID="cvStartDate" runat="server" 
                                ControlToValidate="tbxExpiryDate"
                                SetFocusOnError="true" ClientValidationFunction="ValidateDate" 
                                ErrorMessage="Please Select a Valid Date."
                                Display="Dynamic" ValidationGroup="SaveInfo">
                            </asp:CustomValidator>
                        </td>
                        <td>
                            Nationality
                        </td>
                        <td>
                            <asp:TextBox ID="tbxNationality" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <%--<tr>
                        <td>
                            Accout Number
                        </td>
                        <td>
                            <asp:TextBox ID="tbxAccNumber" runat="server"></asp:TextBox>
                        </td>
                        <td>
                            Account Name
                        </td>
                        <td>
                            <asp:TextBox ID="tbxAccName" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Bic Code
                        </td>
                        <td>
                            <asp:TextBox ID="tbxBicCode" runat="server"></asp:TextBox>
                        </td>
                        <td>
                            Aba Code
                        </td>
                        <td>
                            <asp:TextBox ID="tbxAbaCode" runat="server"></asp:TextBox>
                        </td>
                    </tr>--%>
                    <tr>
                        <td colspan = 4 >
                            <UC:DataTableList ID="ucPassportList" runat="server"
                                ExcludeVisibleFields = "ContactID, ID"
                                LinkFields="ContactID, ID"
                                NoRecordMessgae="No Passport Details Found for this Personnel."
                                DeleteMessage="Sure to Delete Passport Details?">
                            </UC:DataTableList>
                        </td>
                    </tr>
                </table>
            </div>        
            <div class="TenPixelTopMargin">
                <asp:Button ID="btnSave" runat="server" Text="Save" 
                ValidationGroup="SavePInfo" OnClick="btnSave_Click" />            
            </div>
            
            <div class="WinGroupBox">
                <div class="WinGroupBoxHeader">Visas List</div>
                
                <table cellpadding="3" cellspacing="0" style="width: 100%;">
                    <tr>
                        <td>
                            Country
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlCountry" runat="server"></asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                                SetFocusOnError="true"
                                Display="Dynamic" 
                                ControlToValidate="ddlCountry" 
                                ErrorMessage="Please Select a Country."
                                ValidationGroup="SaveVInfo">
                            </asp:RequiredFieldValidator>
                        </td>
                        <td>
                            Visa Type
                        </td>
                        <td>
                            <asp:TextBox ID="tbxVisaType" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Expiry Date
                        </td>
                        <td>
                            <asp:TextBox ID="tbxVisaExpDate" MaxLength="50" 
                                CssClass="CalendarTextBox" runat="server"></asp:TextBox>
                            <asp:CustomValidator ID="CustomValidator1" runat="server" 
                                ControlToValidate="tbxVisaExpDate"
                                SetFocusOnError="true" ClientValidationFunction="ValidateDate" 
                                ErrorMessage="Please Select a Valid Date."
                                Display="Dynamic" ValidationGroup="SaveInfo">
                            </asp:CustomValidator>
                        </td>
                        <td>
                            
                        </td>
                        <td>
                            
                        </td>
                    </tr>
                    
                    <tr>
                        <td colspan = 4 >
                            <UC:DataTableList ID="ucVisaList" runat="server"
                                ExcludeVisibleFields = "ContactID, ID"
                                LinkFields="ContactID, ID"
                                NoRecordMessgae="No VISA Details Found for this Personnel."
                                DeleteMessage="Sure to Delete VISA Details?">
                            </UC:DataTableList>
                        </td>
                    </tr>
                </table>
                
            </div> 
            <div class="TenPixelTopMargin">
                <asp:Button ID="Button1" runat="server" Text="Save" 
                ValidationGroup="SaveVInfo" OnClick="btnSaveVisa_Click" />            
            </div>
                           
        </asp:Panel>   
    </div> 
</asp:Content>


                
                
                
                
                
                
                
                
                
                