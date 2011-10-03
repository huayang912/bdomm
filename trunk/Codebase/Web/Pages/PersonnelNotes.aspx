<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPagePopup.master" AutoEventWireup="true" CodeFile="PersonnelNotes.aspx.cs" Inherits="Pages_PersonnelNotes" %>
<%@ Register Src="~/Controls/DataTableList.ascx" TagName="DataTableList" TagPrefix="UC" %>
<%@ Register Src="~/Controls/Pager.ascx" TagName="Pager" TagPrefix="UC" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="body" Runat="Server">    
    <div>
        <div id="divMessage" runat="server" visible="false" enableviewstate="false"></div>
        <asp:Panel ID="pnlFormContainer" runat="server" DefaultButton="btnSave">
         
            <div class="WinGroupBox">
                <div class="WinGroupBoxHeader">Notes List</div>
                
                <UC:DataTableList ID="ucNoteList" runat="server"
                    VisibleFields="ID, Note,CommunicationType,ChangedBy,ChangedOn"
                    LinkFields="ContactID, ID"
                    NoRecordMessgae="No Note Found for this Personnel."
                    DeleteMessage="Delete Note?">
                </UC:DataTableList>
                
                <div class="TenPixelTopMargin">
                    <UC:Pager ID="ucNoteListPager" runat="server" TotalRecordMessage="Total {0} Note(s) Found." OnPageIndexChanged="ucNoteListPager_PageIndexChanged" />
                </div>
            </div>    
            
               <%--ContactsNotes edit box  Group Box Start--%>               
            <div class="WinGroupBox">                
                <div class="WinGroupBoxHeader">Personnel Note</div>                
                
                <table cellpadding="3" cellspacing="0" style="width:100%;">
                    <colgroup>
                        <col style="width:15%;" />
                        <col />                                        
                    </colgroup
				    <tr>
				    </tr __designer:mapid="109a">
				    <tr>
				        <td>Note</td>
				        <td>
                            <asp:TextBox ID="txtNotes" runat="server" Height="100" TextMode="MultiLine" 
                                width="400"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvNote" runat="server" 
                                ControlToValidate="txtNotes" Display="Dynamic" 
                                ErrorMessage="Please Enter a Note." SetFocusOnError="true" 
                                ValidationGroup="SaveInfo">
			                </asp:RequiredFieldValidator>
                        </td>
                        				    <tr>
                                                <td>
                                                    Communication Type</td>
                                                <td>
                                                    <asp:DropDownList ID="ddlCommType" runat="server" Width="20%">
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                                                        ControlToValidate="ddlCommType" Display="Dynamic" 
                                                        ErrorMessage="Please Select a Type." SetFocusOnError="true" 
                                                        ValidationGroup="SaveInfo">
                            </asp:RequiredFieldValidator>
                                                </td>
                        </tr>
                        				    </tr>				                    
                </table>
            </div>        
            <div class="TenPixelTopMargin">
                <asp:Button ID="btnSave" runat="server" Text="Save" ValidationGroup="SaveInfo" OnClick="btnSave_Click" />            
                <%--<asp:Button ID="btnList" runat="server" Text="View List" OnClick="btnList_Click" />--%>
            </div>
               <%--ContactsNotes edit box  end--%>              
    
        </asp:Panel>   
    </div> 
</asp:Content>


                
                
                
                
                
                
                
                
                
                