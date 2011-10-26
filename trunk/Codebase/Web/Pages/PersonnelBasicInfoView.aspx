<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPagePopup.master" AutoEventWireup="true" 
CodeFile="PersonnelBasicInfoView.aspx.cs" Inherits="Pages_PersonnelBasicInfoView" %>
<%@ Register Src="~/Controls/DataTableList.ascx" TagName="DataTableList" TagPrefix="UC" %>
<%@ Register Src="~/Controls/Pager.ascx" TagName="Pager" TagPrefix="UC" %>
<%@ Register Src="~/Controls/jQueryCalendar.ascx" TagName="jQueryCalendar" TagPrefix="UC" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <UC:jQueryCalendar ID="ucjQueryCalendar" runat="server" />
    
    <style type="text/css">
        .LeftColumn{width:30%; float:left;}
        .CenterColumn{width:30%; float:left; margin-left:10px;}
        .RightColumn{width:30%; float:left; margin-left:10px;}
        .AddNewLink{margin-bottom:5px;}
    </style> 
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="Server">
    <div>
        <div id="divMessage" runat="server" visible="false" enableviewstate="false">
        </div>
        <asp:Panel ID="pnlFormContainer" runat="server">
            
            <div class="WinGroupBox">
                <div class="WinGroupBoxHeader" style="width: 160px;">
                    Other Details</div>
                
                <table cellpadding="3" cellspacing="0" style="width: 100%; 
                    border:1px; border-style:solid; border-color:Gray; ">
                    
                    <tr>
                        <td style="width:10%; background-color:#D6E8FF">
                            Date Of Last Meeting
                        </td>
                        <td style="width:15%">
                            <asp:Label ID="lblDateOfLastMeeting" runat="server" Text=""></asp:Label>
                         </td>
                    
                        <td style="width:10% ;background-color:#D6E8FF">
                            Preferred Day Rate
                        </td>
                        <td style="width:15%">
                            <asp:Label ID="lblDayRateCurrencyID" runat="server" Text=""></asp:Label>
                            <asp:Label ID="lblPreferredDayRate" runat="server" Text=""></asp:Label>
                        </td>
                    
                        <td style="width:10%; background-color:#D6E8FF">
                            PPE Sizes
                        </td>
                        <td style="width:15%">
                            <asp:Label ID="lblPPE_Size" runat="server" Text=""></asp:Label>
                        </td>
                   
                        <td style="width:10%; background-color:#D6E8FF">
                            Coverall
                        </td>
                        <td style="width:15%">
                            <asp:Label ID="lblCoverall" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width:10%; background-color:#D6E8FF">
                            Boot Size
                        </td>
                        <td>
                            <asp:Label ID="lblBootsize" runat="server" Text=""></asp:Label>
                        </td>
                    
                        <td style="width:10%; background-color:#D6E8FF">
                            No SMS or Email
                        </td>
                        <td>
                            <asp:CheckBox ID="chkNoSMSorEmail" runat="server" />
                        </td>
                    
                        <td style="width:10%; background-color:#D6E8FF">
                            Inactive
                        </td>
                        <td>
                            <asp:CheckBox ID="chkInactive" runat="server" />
                        </td>
                    </tr>
                </table>
            </div>
                        
            <div class="WinGroupBox">
                <div class="WinGroupBoxHeader" style="width: 160px;">
                    Employment Details</div>
                <table cellpadding="3" cellspacing="0" style="width: 100%;
                    border:1px; border-style:solid; border-color:Gray; ">
                    <tr>
                        <td style="width:10%; background-color:#D6E8FF">
                            Company
                        </td>
                        <td style="width:15%">
                            <asp:Label ID="lblCompanyname" runat="server" Text=""></asp:Label>
                        </td>
                    
                        <td style="width:10%; background-color:#D6E8FF">
                            Reg
                        </td>
                        <td style="width:15%">
                            <asp:Label ID="lblCompanyreg" runat="server" Text=""></asp:Label>
                        </td>
                   
                        <td style="width:10%; background-color:#D6E8FF">
                            Vat
                        </td>
                        <td style="width:15%">
                            <asp:Label ID="lblCompanyvat" runat="server" Text=""></asp:Label>
                        </td>
                    
                        <td style="width:10%; background-color:#D6E8FF">
                            Address
                        </td>
                        <td style="width:15%">
                            <asp:Label ID="lblCompanyadr" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width:10%; background-color:#D6E8FF">
                            Employment Status
                        </td>
                        <td>
                            <asp:Label ID="lblEmploymentstatus" runat="server" Text=""></asp:Label>
                        </td>
                    
                        <td style="width:10%; background-color:#D6E8FF">
                            Insurance
                        </td>
                        <td>
                            <asp:Label ID="lblInsurance" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                </table>   
                    
            </div>
            
            <div class="WinGroupBox">
                <div class="WinGroupBoxHeader" style="width: 160px;">
                    Employment History List</div>
                
                <%--<asp:Button ID="btnShowEmpDetails" runat="server" Text="Add New Details" 
                OnClick="btnShowEmpDetails_Click" />--%>
                    
                <UC:DataTableList ID="ucEmploymentHistoryList" runat="server" ExcludeVisibleFields="ID, ContactID"
                    LinkFields="ContactID, ID" NoRecordMessgae="No Employment History Found for this Personnel."
                    DeleteMessage="Sure to Delete Employment History?"></UC:DataTableList>
            </div>
            
            <div class="WinGroupBox">
                <div class="WinGroupBoxHeader" style="width: 160px;">
                    Roles</div>
                    
                <UC:DataTableList ID="ucContactRoles" runat="server" 
                                ExcludeVisibleFields="ContactID,ID"
                                LinkFields="ContactID, ID" 
                                NoRecordMessgae="No Role Found for this Personnel."
                                DeleteMessage="Sure to Delete Role?">
                            </UC:DataTableList>  
            </div>
            
        </asp:Panel>
        
        
    </div>
</asp:Content>

