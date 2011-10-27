<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="PersonnelDetails2.aspx.cs" Inherits="Pages_PersonnelDetails2" %>
<%@ MasterType VirtualPath="~/Main.master" %>

<%@ Register Src="~/Controls/DataTableList.ascx" TagName="DataTableList" TagPrefix="UC" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageHeaderContentPlaceHolder" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="SideBarPlaceHolder" Runat="Server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="PageContentPlaceHolder" Runat="Server">
    <div id="divMessage" runat="server" enableviewstate="false" visible="false"></div>
    
    <asp:Panel ID="pnlDetails" runat="server">
        <div class="LeftColumn">
            <div class="WinGroupBox">
                <div class="WinGroupBoxHeader">Personnel Details</div>       
                <table cellpadding="3" cellspacing="0" style="width:100%;">
                    <colgroup>
                        <col style="width:25%;" />
                        <col />                                        
                    </colgroup>				
	                <tr>
		                <td>Surname</td>
		                <td>					        
                            <asp:Label ID="lblLastName" runat="server" Text=""></asp:Label>					        
		                </td>
	                </tr>
	                <tr>
		                <td>First Name</td>
		                <td>					        
			                <asp:Label ID="lblFirstNames" runat="server" Text=""></asp:Label>					        
		                </td>
	                </tr>
	                <tr>
		                <td>Address</td>
		                <td>					        
			                <asp:Label ID="lblAddress" runat="server" Text=""></asp:Label>
		                </td>
	                </tr>
	                <tr>
		                <td>Postcode</td>
		                <td>					        
			                <asp:Label ID="lblPostcode" runat="server" Text=""></asp:Label>
		                </td>
	                </tr>
	                <tr>
		                <td>Country</td>
		                <td>					        
			                <asp:Label ID="lblCountryID" runat="server" Text=""></asp:Label>
		                </td>
	                </tr>				
	                <tr>
		                <td>Marital Status</td>
		                <td>
		                    <asp:Label ID="lblMaritalStatusID" runat="server" Text=""></asp:Label>					        
		                </td>
	                </tr>
	                <tr>
		                <td>Place Of Birth</td>
		                <td>					        
			                <asp:Label ID="lblPlaceOfBirth" runat="server" Text=""></asp:Label>
		                </td>
	                </tr>
	                <tr>
		                <td>Country Of Birth</td>
		                <td>					        
			                <asp:Label ID="lblCountryOfBirthID" runat="server" Text=""></asp:Label>
		                </td>
	                </tr>
	                <tr>
		                <td>Date Of Birth</td>
		                <td>					        
			                <asp:Label ID="lblDateOfBirth" runat="server" Text=""></asp:Label>					        
		                </td>
	                </tr>              
                </table>
            </div>            
        </div>
        
        <%--Right Column Starts From Here--%>
        
        <div class="RightColumn">
            <div class="WinGroupBox">
                <div class="WinGroupBoxHeader">Phone Numbers</div> 
                <UC:DataTableList ID="ucTelephoneNumberList" runat="server" />
            </div>
            <div class="WinGroupBox">
                <div class="WinGroupBoxHeader">Email Addresses</div> 
                <UC:DataTableList ID="ucEmailList" runat="server"
                    VisibleFields="Email" />
            </div>
        </div>
        <div class="clearboth"></div>
        
        <%--Employment History Start--%>
        <div class="WinGroupBox">
            <div class="WinGroupBoxHeader">Employment History</div>
            <UC:DataTableList ID="ucEmploymentHistory" runat="server"
                ExcludeVisibleFields="ID, ContactID" />
        </div>
    </asp:Panel>
</asp:Content>

