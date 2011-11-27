<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPagePopup.master" AutoEventWireup="true" 
CodeFile="PersonnelCertification.aspx.cs" Inherits="Pages_PersonnelCertification" %>

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
            <%--ContactsNotes Information Group Box Start--%>               
            <div class="WinGroupBox">                
                <div class="WinGroupBoxHeader">Certification Details</div>                
                
                <table cellpadding="3" cellspacing="0" style="width:100%;">
                    <colgroup>
                        <col style="width:10%;" />
                        <col style="width:20%;" />
                        <col style="width:10%;" />
                        <col />
                    </colgroup>
                    <tr>
                        <td>
                            Type
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlCertificateType" runat="server"></asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfvCertificateType" runat="server"
                                Display="Dynamic" SetFocusOnError="true" 
                                ControlToValidate="ddlCertificateType"
                                ErrorMessage="<br/>Please Select a Type."
                                ValidationGroup="SaveInfo">
                            </asp:RequiredFieldValidator>
                        </td>
                        <td>
                            Details
                        </td>
                        <td>
                            <asp:TextBox ID="tbxDetails" runat="server" MaxLength="100"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                                ControlToValidate="ddlCertificateType"
                                Display="Dynamic" SetFocusOnError="true" 
                                ErrorMessage="<br/>Please add a details."
                                ValidationGroup="SaveInfo">
                            </asp:RequiredFieldValidator>
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
                                Display="Dynamic" ValidationGroup="SaveInfo">
                            </asp:CustomValidator>
                        </td>
                        <td>
                            Place Issued
                        </td>
                        <td>
                            <asp:TextBox ID="tbxPlaceIssued" runat="server" MaxLength="100"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>Changed By</td>
                        <td><asp:Label ID="lblChangedBy" runat="server"></asp:Label></td>
                        <td>Changed On</td>
                        <td><asp:Label ID="lblChangedOn" runat="server"></asp:Label></td>
                    </tr>		                    
                </table>
            </div>        
            <div class="TenPixelTopMargin">
                <asp:Button ID="btnSave" runat="server" Text="Save" 
                    ValidationGroup="SaveInfo" OnClick="btnSave_Click" />            
                
            </div>
            
            <div class="WinGroupBox">
                <div class="WinGroupBoxHeader">Certification List</div>
                
                <UC:DataTableList ID="ucCertificationList" runat="server"
                    ExcludeVisibleFields = "ContactID, ID, TypeID"
                    LinkFields="ContactID, ID"
                    NoRecordMessgae="No Certification Details Found for this Personnel."
                    DeleteMessage="Sure to Delete Certification Details?">
                </UC:DataTableList>
                
                <%--<div class="TenPixelTopMargin">
                    <UC:Pager ID="ucNoteListPager" runat="server" 
                    TotalRecordMessage="Total {0} Certification(s) Found." 
                    OnPageIndexChanged="ucNoteListPager_PageIndexChanged" />
                </div>--%>
            </div>                
        <%--ContactsNotes Information Group Box End--%>
        </asp:Panel>   
    </div> 
</asp:Content>


                
                
                
                
                
                
                
                
                
                