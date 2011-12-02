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
                    <table cellpadding="3" cellspacing="0" style="width:100%;">
                        <colgroup>
                            <col style="width:10%;" />                            
                            <col />
                        </colgroup>
                        <tr>
                            <td>
                                Mother's Name*
                            </td>
                            <td>
                                <asp:TextBox ID="tbxMotherName" runat="server" MaxLength="50"></asp:TextBox> 
                                <asp:RequiredFieldValidator ID="rfvMothersName" runat="server"
                                    SetFocusOnError="true" Display="Dynamic"
                                    ControlToValidate="tbxMotherName"
                                    ValidationGroup="SaveInfo" 
                                    ErrorMessage="<br/>Please Enter Mother's Name.">
                                </asp:RequiredFieldValidator>                              
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Father's Name*
                            </td>
                            <td>
                                <asp:TextBox ID="tbxFatherName" runat="server" MaxLength="50"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                                    SetFocusOnError="true" Display="Dynamic"
                                    ControlToValidate="tbxFatherName"
                                    ValidationGroup="SaveInfo"
                                    ErrorMessage="<br/>Please Enter Father's Name.">
                                </asp:RequiredFieldValidator>                              
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Children's Name
                            </td>
                            <td>
                                <asp:TextBox ID="tbxChildName" runat="server" TextMode="MultiLine" MaxLength="150"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Changed By</td>
                            <td><asp:Label ID="lblChangedBy" runat="server"></asp:Label></td>
                        </tr>
                        <tr>
                            <td>Changed On</td>
                            <td><asp:Label ID="lblChagedOn" runat="server"></asp:Label></td>                            
                        </tr>
                    </table>
                    <div class="TenPixelTopMargin">
                        <asp:Button ID="btnSave" runat="server" Text="Save" 
                            ValidationGroup="SaveInfo" OnClick="btnSave_Click"/>  
                    </div>
                </div>
            </div>
        </asp:Panel>
        
        <asp:Panel ID="pnlNextOfKin" runat="server" DefaultButton="btnSaveNextOfKin">
            <div class="WinGroupBox">
                <div class="WinGroupBoxHeader">
                    Next of Kin Information
                </div>
                <div>
                    <table cellpadding="3" cellspacing="0" style="width:100%;">
                        <colgroup>
                            <col style="width:10%;" />
                            <col style="width:20%;" />
                            <col style="width:10%;" />
                            <col />
                        </colgroup>
                        <tr>
                            <td>
                                Name*
                            </td>
                            <td>
                                <asp:TextBox ID="tbxName" runat="server" MaxLength="150"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvTxtName" runat="server"
                                    SetFocusOnError="true" Display="Dynamic"
                                    ControlToValidate="tbxName"
                                    ValidationGroup="SaveNextOfKin"
                                    ErrorMessage="<br/>Please Enter Name.">
                                </asp:RequiredFieldValidator>
                            </td>
                            
                            <td>
                                Relationship
                            </td>
                            <td>
                                <asp:TextBox ID="tbxRelationShip" runat="server" MaxLength="30"></asp:TextBox>
                            </td>
                        </tr>
                        
                        <tr>
                            <td>
                                Address
                            </td>
                            <td>
                                <asp:TextBox ID="tbxAddress" runat="server" MaxLength="200"></asp:TextBox>
                            </td>
                            <td>
                                Post Code
                            </td>
                            <td>
                                <asp:TextBox ID="tbxPostCode" runat="server" MaxLength="20"></asp:TextBox>
                            </td>
                        </tr>
                        
                        <tr>                         
                            <td>
                                Country*</td>
                            <td>
                                <asp:DropDownList ID="ddlCountry" runat="server"></asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rftddlCountry" runat="server"
                                    SetFocusOnError="true" Display="Dynamic"
                                    ControlToValidate="ddlCountry"
                                    ValidationGroup="SaveNextOfKin"
                                    ErrorMessage="<br/>Please Select Country.">
                                </asp:RequiredFieldValidator>
                            </td>
                            <td>
                                Home Number
                            </td>
                            <td>
                                <asp:TextBox ID="tbxHomeNumber" runat="server" MaxLength="30"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Work Number
                            </td>
                            <td>
                                <asp:TextBox ID="tbxWorkNumber" runat="server" MaxLength="30"></asp:TextBox>
                            </td>
                            <td>
                                Mobile Number
                            </td>
                            <td>
                                <asp:TextBox ID="tbxMobile" runat="server" MaxLength="30"></asp:TextBox>
                            </td>                            
                        </tr>                  
                    </table>
                    
                    <div class="TenPixelTopMargin">
                        <asp:Button ID="btnSaveNextOfKin" runat="server" ValidationGroup="SaveNextOfKin" Text="Save" OnClick="btnSaveNextOfKin_Click" />
                    </div>
                    
                    <div class="WinGroupBox">
                        <div class="WinGroupBoxHeader">Next Of Kin List</div>
                        <UC:DataTableList ID="ucPassportList" 
                            runat="server" 
                            ExcludeVisibleFields="ContactID, ID"
                            LinkFields="ContactID, ID" 
                            NoRecordMessgae="No Passport Details Found for this Personnel."
                            DeleteMessage="Sure to Delete Next of Kin?">
                        </UC:DataTableList>
                    </div>
                </div>
            </div>
        </asp:Panel>              
    </div> 
</asp:Content>


                
                
                
                
                
                
                
                
                
                