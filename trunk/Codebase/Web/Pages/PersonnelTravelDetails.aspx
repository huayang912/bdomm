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
        <%--<asp:Panel ID="pnlFormContainer" runat="server" DefaultButton="btnSave">--%>
            <div class="WinGroupBox">
                <div class="WinGroupBoxHeader">Basic Travel Details</div>
                <table cellpadding="3" cellspacing="0" style="width: 100%;">
                    <colgroup>
                        <col style="width:10%;" />
                        <col style="width:20%;" />
                        <col style="width:10%;" />
                        <col />
                    </colgroup>
                    <tr>
                        <td>
                            Frequent Flyer Number:
                        </td>
                        <td>
                            <asp:TextBox ID="tbxFrequentFlNumber" runat="server" MaxLength="50"></asp:TextBox>
                        </td>
                        <td>
                            Preferred Airport:
                        </td>                    
                        <td>
                            <asp:TextBox ID="tbxPreferredAirport" runat="server" MaxLength="50"></asp:TextBox>
                        </td>
                     </tr>
                    <tr>   
                        <td>
                            Closest Airport:
                        </td>
                        <td>
                            <asp:TextBox ID="tbxClosestAirport" runat="server" MaxLength="50"></asp:TextBox>
                        </td>
                        <td>Changed By</td>
                        <td>
                            <asp:Label ID="lblChangedBy" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>Changed On</td>
                        <td><asp:Label ID="lblChangedOn" runat="server"></asp:Label></td>
                        <td colspan="2"></td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <asp:Button ID="btnSaveBasicInfo" runat="server" Text="Save" 
                                ValidationGroup="SaveTravelDetails" OnClick="btnSaveBasicInfo_Click"/>
                        </td>
                    </tr>
                </table>
            </div>
           
            
            <div class="WinGroupBox">
                <div class="WinGroupBoxHeader">Passports</div>
                <table cellpadding="3" cellspacing="0" style="width: 100%;">
                    <colgroup>
                        <col style="width:10%;" />
                        <col style="width:20%;" />
                        <col style="width:10%;" />
                        <col />
                    </colgroup>
                    <tr>
                        <td>Number</td>
                        <td>
                            <asp:TextBox ID="tbxNumber" runat="server" MaxLength="50"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                                SetFocusOnError="true"
                                Display="Dynamic" 
                                ControlToValidate="tbxNumber" 
                                ErrorMessage="<br/>Please Enter a Number."
                                ValidationGroup="SavePassport">
                            </asp:RequiredFieldValidator>
                            <asp:CustomValidator ID="cvPassport" runat="server"
                                SetFocusOnError="true" Display="Dynamic"
                                ControlToValidate="tbxNumber" ValidationGroup="SavePassport"
                                ErrorMessage="<br/>This Passport Number already exists."
                                OnServerValidate="cvPassport_OnServerValidate">
                            </asp:CustomValidator>
                        </td>
                        <td>
                            Where Issued
                        </td>
                        <td>
                            <asp:TextBox ID="tbxWhereIssued" runat="server" MaxLength="100"></asp:TextBox>
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
                                ErrorMessage="<br/>Please Select a Valid Date."
                                Display="Dynamic" ValidationGroup="SavePassport">
                            </asp:CustomValidator>
                        </td>
                        <td>
                            Nationality
                        </td>
                        <td>
                            <asp:TextBox ID="tbxNationality" runat="server" MaxLength="100"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <asp:Button ID="btnSavePassport" runat="server" Text="Save" ValidationGroup="SavePassport" OnClick="btnSavePassport_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
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
            
            
            <div class="WinGroupBox">
                <div class="WinGroupBoxHeader">Visas</div>                
                <table cellpadding="3" cellspacing="0" style="width: 100%;">
                    <colgroup>
                        <col style="width:10%;" />
                        <col style="width:20%;" />
                        <col style="width:10%;" />
                        <col />
                    </colgroup>
                    <tr>
                        <td>Country</td>
                        <td>
                            <asp:DropDownList ID="ddlCountry" runat="server"></asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                                SetFocusOnError="true"
                                Display="Dynamic" 
                                ControlToValidate="ddlCountry" 
                                ErrorMessage="<br/>Please Select a Country."
                                ValidationGroup="SaveVInfo">
                            </asp:RequiredFieldValidator>
                        </td>
                        <td>
                            Visa Type
                        </td>
                        <td>
                            <asp:TextBox ID="tbxVisaType" runat="server" MaxLength="50"></asp:TextBox>
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
                                ErrorMessage="<br/>Please Select a Valid Date."
                                Display="Dynamic" ValidationGroup="SaveVInfo">
                            </asp:CustomValidator>
                        </td>
                        <td colspan="2"></td>                        
                    </tr>
                    <tr>
                        <td colspan="4">
                            <asp:Button ID="btnSaveVisa" runat="server" Text="Save" 
                                ValidationGroup="SaveVInfo" OnClick="btnSaveVisa_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" >
                            <UC:DataTableList ID="ucVisaList" runat="server"
                                ExcludeVisibleFields = "ContactID, ID, CountryID"
                                LinkFields="ContactID, ID"
                                NoRecordMessgae="No VISA Details Found for this Personnel."
                                DeleteMessage="Sure to Delete VISA Details?">
                            </UC:DataTableList>
                        </td>
                    </tr>
                </table>
                
            </div>             
            <div style="height:50px;">&nbsp;</div>             
        <%--</asp:Panel>--%>   
    </div> 
</asp:Content>


                
                
                
                
                
                
                
                
                
                