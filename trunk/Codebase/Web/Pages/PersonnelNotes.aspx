<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPagePopup.master" AutoEventWireup="true" CodeFile="PersonnelNotes.aspx.cs" Inherits="Pages_PersonnelNotes" %>
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
                <div class="WinGroupBoxHeader">Personnel Note</div>                
                
                <table cellpadding="3" cellspacing="0" style="width:100%;">
                    <colgroup>
                        <col style="width:15%;" />
                        <col />                                        
                    </colgroup>
				    <tr>
					    <td>Note</td>
					    <td>
						    <asp:TextBox ID="txtNotes" TextMode="MultiLine" MaxLength="2000" runat="server"></asp:TextBox>
						    <asp:RequiredFieldValidator ID="rfvNote" runat="server"
						        SetFocusOnError="true" Display="Dynamic"
						        ControlToValidate="txtNotes"
						        ErrorMessage="Please Enter a Note."
						        ValidationGroup="SaveInfo">
			                </asp:RequiredFieldValidator>
					    </td>
				    </tr>				                    
                </table>
            </div>        
            <div class="TenPixelTopMargin">
                <asp:Button ID="btnSave" runat="server" Text="Save" ValidationGroup="SaveInfo" OnClick="btnSave_Click" />            
                <%--<asp:Button ID="btnList" runat="server" Text="View List" OnClick="btnList_Click" />--%>
            </div>
            
            <div class="WinGroupBox">
                <div class="WinGroupBoxHeader">Notes List</div>
                
                <UC:DataTableList ID="ucNoteList" runat="server"
                    VisibleFields="ID, Note"
                    LinkFields="ContactID, ID"
                    NoRecordMessgae="No Note Found for this Personnel."
                    DeleteMessage="Sure to Delete Note?">
                </UC:DataTableList>
                
                <div class="TenPixelTopMargin">
                    <UC:Pager ID="ucNoteListPager" runat="server" TotalRecordMessage="Total {0} Note(s) Found." OnPageIndexChanged="ucNoteListPager_PageIndexChanged" />
                </div>
            </div>                
        <%--ContactsNotes Information Group Box End--%>
        </asp:Panel>   
    </div> 
</asp:Content>


                
                
                
                
                
                
                
                
                
                