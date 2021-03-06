<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPagePopup.master" AutoEventWireup="true" 
CodeFile="PersonnelBankDetails.aspx.cs" Inherits="Pages_PersonnelBankDetails" %>

<%@ Register Src="~/Controls/DataTableList.ascx" TagName="DataTableList" TagPrefix="UC" %>
<%@ Register Src="~/Controls/Pager.ascx" TagName="Pager" TagPrefix="UC" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="body" Runat="Server">    
    <div>
        <div id="divMessage" runat="server" visible="false" enableviewstate="false"></div>
        <asp:Panel ID="pnlFormContainer" runat="server" DefaultButton="btnSave">
            <%--ContactsNotes Information Group Box Start--%>               
            <div class="WinGroupBox">                
                <div class="WinGroupBoxHeader">Personnel Bank Details</div>                
                
                <table cellpadding="3" cellspacing="0" style="width:100%;">
                    <colgroup>
                        <col style="width:10%;" />
                        <col style="width:20%;" />
                        <col style="width:10%;" />
                        <col />
                    </colgroup>
				    <tr>
				        <td>Bank Name</td>
				        <td>
				            <asp:TextBox ID="tbxBankName" runat="server"></asp:TextBox>
						    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
						        SetFocusOnError="true" Display="Dynamic"
						        ControlToValidate="tbxBankName"
						        ErrorMessage="<br/>Please Enter a Bank Name."
						        ValidationGroup="SaveInfo">
			                </asp:RequiredFieldValidator>
				        </td>
				        <td>Branch Name</td>
				        <td>
				            <asp:TextBox ID="tbxBranchName" runat="server"></asp:TextBox>
						</td>
			        </tr>
			        <tr>
				        <td>Branch Address</td>
				        <td>
				            <asp:TextBox ID="tbxBranchAddress" runat="server"></asp:TextBox>
				        </td>
				        <td>Sort Code</td>
				        <td>
				            <asp:TextBox ID="tbxSortCode" runat="server"></asp:TextBox>
				        </td>
				    </tr>				    
				    <tr>
				        <td>Accout Number</td>
				        <td>
				            <asp:TextBox ID="tbxAccNumber" runat="server"></asp:TextBox>
						</td>
				        <td>Account Name</td>
				        <td>
				            <asp:TextBox ID="tbxAccName" runat="server"></asp:TextBox>
						</td>
				    </tr>
			        <tr>
				        <td>Bic Code</td>
				        <td>
				            <asp:TextBox ID="tbxBicCode" runat="server"></asp:TextBox>
				        </td>
				        <td>Aba Code</td>
				        <td>
				            <asp:TextBox ID="tbxAbaCode" runat="server"></asp:TextBox>
				        </td>
				    </tr>
				    <tr>
				        <td>Changed By</td>
				        <td>
				            <asp:Label ID="lblChangedBy" runat="server"></asp:Label>
				        </td>
				        <td>Changed On</td>
				        <td>
				            <asp:Label ID="lblChangedOn" runat="server"></asp:Label>
				        </td>
				    </tr>			                    
                </table>
            </div>        
            <div class="TenPixelTopMargin">
                <asp:Button ID="btnSave" runat="server" Text="Save" ValidationGroup="SaveInfo" OnClick="btnSave_Click" />            
                <%--<asp:Button ID="btnList" runat="server" Text="View List" OnClick="btnList_Click" />--%>
            </div>
            
            <div class="WinGroupBox">
                <div class="WinGroupBoxHeader">Bank Details List</div>
                
                <UC:DataTableList ID="ucBankList" runat="server"
                    ExcludeVisibleFields = "ContactID, ID"
                    LinkFields="ContactID, ID"
                    NoRecordMessgae="No Bank Details Found for this Personnel."
                    DeleteMessage="Sure to Delete Bank Details?">
                </UC:DataTableList>
                
                <%--<div class="TenPixelTopMargin">
                    <UC:Pager ID="ucNoteListPager" runat="server" TotalRecordMessage="Total {0} Note(s) Found." OnPageIndexChanged="ucNoteListPager_PageIndexChanged" />
                </div>--%>
            </div>                
        <%--ContactsNotes Information Group Box End--%>
        </asp:Panel>   
    </div> 
</asp:Content>


                
                
                
                
                
                
                
                
                
                