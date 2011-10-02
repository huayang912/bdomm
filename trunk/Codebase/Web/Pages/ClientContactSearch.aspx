<%@ Page Title="" Language="C#" 
MasterPageFile="~/Main.master" AutoEventWireup="true" 
CodeFile="ClientContactSearch.aspx.cs" Inherits="Pages_ClientContactSearch" %>

<%@ Register Src="~/Controls/DataTableList.ascx" TagName="DataTableList" TagPrefix="UC" %>
<%@ Register Src="~/Controls/Pager.ascx" TagName="Pager" TagPrefix="UC" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .LeftColumn{width:60%; float:left;}
        .RightColumn{width:30%; float:left; margin-left:10px;}
        .AddNewLink{margin-bottom:5px;}
    </style>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="PageContentPlaceHolder" runat="Server">
  
    <div>
        <div id="divMessage" runat="server" visible="false" enableviewstate="false"></div>
        <asp:Panel ID="pnlFormContainer" runat="server" DefaultButton="btnSave">
        
        
            <%--ContactsNotes Information Group Box Start--%>
            <div class="LeftColumn">
                <div class="WinGroupBox">
                    <div class="WinGroupBoxHeader">
                        Contact Details</div>
                    <table cellpadding="3" cellspacing="0" style="width: 100%;">
                        <tr>
                            <td style="width: 16%">
                                Name
                            </td>
                            <td style="width: 16%">
                                <asp:TextBox ID="tbxName" runat="server"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" SetFocusOnError="true"
                                    Display="Dynamic" ControlToValidate="tbxName" ErrorMessage="Please Enter Name."
                                    ValidationGroup="SaveInfo">
                                </asp:RequiredFieldValidator>
                            </td>
                            <td style="width: 16%">
                                Job Title
                            </td>
                            <td style="width: 16%">
                                <asp:TextBox ID="tbxJobTitle" runat="server"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" SetFocusOnError="true"
                                    Display="Dynamic" ControlToValidate="tbxJobTitle" ErrorMessage="Please Enter Job Title."
                                    ValidationGroup="SaveInfo">
                                </asp:RequiredFieldValidator>
                            </td>
                            <td style="width: 16%">
                                Email
                            </td>
                            <td style="width: 14%">
                                <asp:TextBox ID="tbxEmail" runat="server"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" SetFocusOnError="true"
                                    Display="Dynamic" ControlToValidate="tbxEmail" ErrorMessage="Please Enter Email."
                                    ValidationGroup="SaveInfo">
                                </asp:RequiredFieldValidator>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="WinGroupBox">
                    <div class="WinGroupBoxHeader">
                        Company Details</div>
                    <table cellpadding="3" cellspacing="0" style="width: 100%;">
                        <tr>
                            <td style="width: 16%">
                                Company Name
                            </td>
                            <td style="width: 16%">
                                <asp:TextBox ID="tbxCompanyName" runat="server"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" SetFocusOnError="true"
                                    Display="Dynamic" ControlToValidate="tbxCompanyName" ErrorMessage="Please Enter Company Name."
                                    ValidationGroup="SaveInfo">
                                </asp:RequiredFieldValidator>
                            </td>
                            <td style="width: 16%">
                                Company Address
                            </td>
                            <td style="width: 16%">
                                <asp:TextBox ID="tbxCompanyAddress" runat="server"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" SetFocusOnError="true"
                                    Display="Dynamic" ControlToValidate="tbxCompanyAddress" ErrorMessage="Please Enter Company Address."
                                    ValidationGroup="SaveInfo">
                                </asp:RequiredFieldValidator>
                            </td>
                            <td style="width: 16%">
                                Company Post Code
                            </td>
                            <td style="width: 14%">
                                <asp:TextBox ID="tbxPostCode" runat="server"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" SetFocusOnError="true"
                                    Display="Dynamic" ControlToValidate="tbxPostCode" ErrorMessage="Please Enter Company Post Code."
                                    ValidationGroup="SaveInfo">
                                </asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Telephone
                            </td>
                            <td>
                                <asp:TextBox ID="tbxTelephone" runat="server"></asp:TextBox>
                            </td>
                            <td>
                                Mobile
                            </td>
                            <td>
                                <asp:TextBox ID="tbxMobile" runat="server"></asp:TextBox>
                            </td>
                            <td>
                                Fax
                            </td>
                            <td>
                                <asp:TextBox ID="tbxFax" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Company Email
                            </td>
                            <td>
                                <asp:TextBox ID="tbxCompanyEmail" runat="server"></asp:TextBox>
                            </td>
                            <td>
                                Company Web
                            </td>
                            <td>
                                <asp:TextBox ID="tbxCompanyWeb" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <div class="RightColumn">
                <div class="WinGroupBox">
                    <div class="WinGroupBoxHeader">
                        Company Country Details</div>
                    <table cellpadding="3" cellspacing="0" style="width: 100%;">
                        <tr>
                            <td style="width: 16%">
                                Country
                            </td>
                            <td style="width: 16%">
                                <asp:TextBox ID="tbxCountry" runat="server"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" SetFocusOnError="true"
                                    Display="Dynamic" ControlToValidate="tbxCountry" ErrorMessage="Please Select Country."
                                    ValidationGroup="SaveInfo">
                                </asp:RequiredFieldValidator>
                            </td>
                         </tr>
                         <tr>
                            <td>
                                Is UK
                            </td>
                            <td>
                                <asp:RadioButton ID="radUKYes" Checked="true" GroupName="isUK" runat="server" />
                                <asp:RadioButton ID="radUKNo" GroupName="isUK" runat="server" />
                            </td>
                        </tr>
                         <tr>
                            <td>
                                Is Europe
                            </td>
                            <td>
                                <asp:RadioButton ID="radEuropeYes" Checked="true" GroupName="isEurope" runat="server" />
                                <asp:RadioButton ID="radEuropeNo" GroupName="isEurope" runat="server" />
                            </td>
                            
                       </tr>
                         <tr>
                            <td>
                                Code
                            </td>
                            <td>
                                <asp:TextBox ID="tbxCode" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            
            <div class="clearboth"></div>
            
            <div class="TenPixelTopMargin">
                <asp:Button ID="btnSave" runat="server" Text="Save" ValidationGroup="SaveInfo" OnClick="btnSave_Click" />
            </div>
            <div class="WinGroupBox">
                <%--<div class="WinGroupBoxHeader">
                    Contact Details</div>--%>
                <table width="100%" border="0">
                    <tr>
                        <td>
                            <table>
                                <tr>
                                    <td>
                                        Type:
                                        <asp:DropDownList ID="CRM_Type" runat="server">
                                            <asp:ListItem>Show All</asp:ListItem>
                                            <asp:ListItem>All</asp:ListItem>
                                            <asp:ListItem>Procurement</asp:ListItem>
                                            <asp:ListItem>Personnel</asp:ListItem>
                                            <asp:ListItem>O&amp;M</asp:ListItem>
                                            <asp:ListItem>Project</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Button ID="SearchButton" runat="server" Text="Search" OnClick="SearchButton_Click" />
                                    </td>
                                    <td style="padding-left: 15px; padding-top: 3px;">
                                        <div class="floatleft">
                                            |</div>
                                        <asp:Repeater ID="rptStartsWith" runat="server" OnItemDataBound="rptStartsWith_ItemDataBound"
                                            OnItemCommand="rptStartsWith_Command">
                                            <ItemTemplate>
                                                <div class="floatleft">
                                                    &nbsp;<asp:LinkButton ID="lkbCommand" CommandName="Filter" runat="server"></asp:LinkButton>&nbsp;|
                                                </div>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                        <div style="clear: both;">
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td align="right">
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblFilterOn" runat="server" Font-Bold="True" Font-Size="10pt" 
                                            ForeColor="#006600"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="WinGroupBox">
                <div class="WinGroupBoxHeader">
                    Client Contact List</div>
                <UC:DataTableList ID="ucBankList" runat="server" 
                    VisibleFields="Name, JobTitle,Email, CompanyID"
                    LinkFields="ID" NoRecordMessgae="No Client Contact Found." 
                    DeleteMessage="Sure to Delete Client Contact?">
                </UC:DataTableList>
                <div class="TenPixelTopMargin">
                    <UC:Pager ID="ucClientContactListPager" runat="server" TotalRecordMessage="Total {0} Note(s) Found."
                        OnPageIndexChanged="ucClientContactListPager_PageIndexChanged" />
                </div>
            </div>
            <%--ContactsNotes Information Group Box End--%>
        </asp:Panel>   
    </div> 
</asp:Content>


                
                
                
                
                
                
                
                
                
                