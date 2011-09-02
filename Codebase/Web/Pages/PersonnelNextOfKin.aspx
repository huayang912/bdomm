<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPagePopup.master" AutoEventWireup="true" 
CodeFile="PersonnelNextOfKin.aspx.cs" Inherits="Pages_PersonnelNextOfKin" %>


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
                <div class="WinGroupBoxHeader">Immediate Family Details</div>
                <div>
                    <table cellpadding="3" cellspacing="0" style="width:350px;">
                        <colgroup>
                            <col style="width:33%;" />
                            <col />
                        </colgroup>
                        <tr>
                            <td>
                                Mother's Name
                            </td>
                            <td>
                                <asp:TextBox ID="tbxMotherName" runat="server"></asp:TextBox>
                                <asp:TextBox ID="tbxSaveUpdate" runat="server" Visible="false"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Father's Name
                            </td>
                            <td>
                                <asp:TextBox ID="tbxFatherName" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Children's Name
                            </td>
                            <td>
                                <asp:TextBox ID="tbxChildName" runat="server" TextMode="MultiLine"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            
            <div class="WinGroupBox">
                <div class="WinGroupBoxHeader">
                    Next of Kin Information
                </div>
                <div>
                    <table cellpadding="3" cellspacing="0" style="width:700px;">
                        <colgroup>
                            <col style="width:17%;" />
                            <col style="width:33%;" />
                            <col style="width:17%;" />
                            <col />
                        </colgroup>
                        <tr>
                            <td>
                                Name
                            </td>
                            <td>
                                <asp:TextBox ID="tbxName" runat="server"></asp:TextBox>
                            </td>
                            
                            <td>
                                Relationship
                            </td>
                            <td>
                                <asp:TextBox ID="tbxRelationShip" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        
                        <tr>
                            <td>
                                Address
                            </td>
                            <td>
                                <asp:TextBox ID="tbxAddress" runat="server"></asp:TextBox>
                            </td>
                            <td>
                                Post Code
                            </td>
                            <td>
                                <asp:TextBox ID="tbxPostCode" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        
                        <tr>                         
                            <td>
                                Country
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlCountry" runat="server"></asp:DropDownList>
                            </td>
                            <td>
                                Home Number
                            </td>
                            <td>
                                <asp:TextBox ID="tbxHomeNumber" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Work Number
                            </td>
                            <td>
                                <asp:TextBox ID="tbxWorkNumber" runat="server"></asp:TextBox>
                            </td>
                            <td>
                                Mobile Number
                            </td>
                            <td>
                                <asp:TextBox ID="tbxMobile" runat="server"></asp:TextBox>
                            </td>
                            
                        </tr>
   <%--                     <tr>
                            <td>
                                Changed By
                            </td>
                            <td>
                                <asp:TextBox ID="tbxChangedBy" runat="server" Width="200px" Enabled="false"></asp:TextBox>
                            </td>
                            <td>
                                Change On
                            </td>
                            <td>
                                <asp:TextBox ID="tbxChangeOn" runat="server" Width="200px" Enabled="false"></asp:TextBox>
                            </td>
                        </tr>--%>
                                               
                    </table>
                </div>
            </div>
            
            
            <%--<div class="WinGroupBox">
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
            </div>--%>
            
            <div class="TenPixelTopMargin">
                <asp:Button ID="btnSave" runat="server" Text="Save" 
                ValidationGroup="SaveInfo" OnClick="btnSaveBasicInfo_Click"/>                
            </div>
            
            <div class="WinGroupBox">
                <div class="WinGroupBoxHeader">
                    Next Of Kin</div>
                <UC:DataTableList ID="ucPassportList" 
                    runat="server" 
                    ExcludeVisibleFields="ContactID, ID"
                    LinkFields="ContactID, ID" 
                    NoRecordMessgae="No Passport Details Found for this Personnel."
                    DeleteMessage="Sure to Delete Passport Details?">
                </UC:DataTableList>
            </div>
                    
            <%--<div class="TenPixelTopMargin">
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
            </div>--%>
                           
        </asp:Panel>   
    </div> 
</asp:Content>


                
                
                
                
                
                
                
                
                
                