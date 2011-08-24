<%@ Page Language="C#" MasterPageFile="~/MasterPagePopup.master" AutoEventWireup="true" CodeFile="PersonnelCertification_.aspx.cs"
    Inherits="Pages_PersonnelCertification_" Title="Next Of Kin" %>
<%@ Register Src="~/Controls/jQueryCalendar.ascx" TagName="jQueryCalendar" TagPrefix="UC" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <UC:jQueryCalendar ID="ucjQueryCalendar" runat="server" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="Server">    
    
    
    <script type="text/javascript" language="javascript">

        function editNextOfKin(id) {
            alert(id);             
            
            
        }

        function DeleteCV_Success(result) {
            alert('CV Deleted Successfully');
        }
    </script>
    
    <asp:Label runat="server" ID="lbl" ></asp:Label>
    <div runat="server" id="divAttachmentError">
                    </div>
    <%--<div class="WinGroupBox">
        <div class="WinGroupBoxHeader">
            Immediate Family Details</div>
            
        <div>
            <table cellpadding=1 cellspacing=1 border=0>
                <tr>
                    <td>Mother's Name</td>
                    <td><asp:TextBox ID="tbxMotherName" runat="server" Width="200px"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>Father's Name</td>
                    <td><asp:TextBox ID="tbxFatherName" runat="server" Width="200px"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>Children's Name</td>
                    <td><asp:TextBox ID="tbxChildName" runat="server" Height="51px" 
                            TextMode="MultiLine" Width="200px"></asp:TextBox></td>
                </tr>
            </table>
        
           
        </div>    
        
        <div>
            
            <asp:Button ID="btnUpload" runat="server" Text="Save" ValidationGroup="SaveInfo"
                OnClick="btnUpload_onclick" />
        </div>
    </div>--%> 
    
    <div class="WinGroupBox">
        <div class="WinGroupBoxHeader">
            Personnel Certification
        </div>
        
        
        <div>
            <table cellpadding=1 cellspacing=1 border=0>
                <tr>
                    <td>Type</td>
                    <td>Details</td>
                    <td>Expiry Date</td>
                    <td>Place Issued</td>
                    <td>Changed By</td>
                    <td>Change On</td>
                </tr>
                
                <tr>
                    <td>
                        <asp:DropDownList ID="ddlCertificateType" runat="server">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvCertificateType" runat="server"
                            ControlToValidate="ddlCertificateType"
                            Display="Dynamic" SetFocusOnError="true"
                            ErrorMessage="Please Select a Type."
                            ValidationGroup="SaveInfo">
                        </asp:RequiredFieldValidator>
                    </td>
                    <td>
                        <asp:TextBox ID="tbxDetails" runat="server" Width="200px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                            ControlToValidate="ddlCertificateType"
                            Display="Dynamic" SetFocusOnError="true"
                            ErrorMessage="Please Select a Type."
                            ValidationGroup="SaveInfo">
                        </asp:RequiredFieldValidator>
                    </td>
                    <td>
                        <asp:TextBox ID="tbxExpiryDate" MaxLength="50" CssClass="CalendarTextBox" runat="server"></asp:TextBox>
						    <asp:CustomValidator ID="cvStartDate" runat="server"
							    ControlToValidate="tbxExpiryDate" SetFocusOnError="true"
							    ClientValidationFunction="ValidateDate"
							    ErrorMessage="Please Select a Valid Date." Display="Dynamic"
							    ValidationGroup="SaveInfo">
						    </asp:CustomValidator>
					    </td>
					<td><asp:TextBox ID="tbxPlaceIssued" runat="server" Width="200px"></asp:TextBox></td>
					<td><asp:TextBox ID="tbxChangedBy" runat="server" Width="200px" Enabled="false"></asp:TextBox></td>
                    <td><asp:TextBox ID="tbxChangeOn" runat="server" Width="200px" Enabled="false"></asp:TextBox></td> 
                </tr>
                
                <%--<tr>
                    <td>Name</td>
                    <td>Relationship</td>
                    <td>Address</td>
                    <td>Post Code</td>
                    <td>Country</td>
                    
                </tr>
                <tr>
                    
                    <td><asp:TextBox ID="tbxName" runat="server" Width="200px"></asp:TextBox></td>
                    <td><asp:TextBox ID="tbxRelationShip" runat="server" Width="200px"></asp:TextBox></td>
                    <td><asp:TextBox ID="tbxAddress" runat="server" Width="200px"></asp:TextBox></td>
                    <td><asp:TextBox ID="tbxPostCode" runat="server" Width="200px"></asp:TextBox></td>
                    <td>
                        <asp:DropDownList ID="ddlCountry" runat="server" Width="100px">
                        </asp:DropDownList>
                    </td>
                                       
                </tr>
                <tr>
                    <td>Home Number</td>
                    <td>Work Number</td>
                    <td>Mobile Number</td>
                    <td>Changed By</td>
                    <td>Change On</td>
                </tr>
                <tr>
                    <td><asp:TextBox ID="tbxHomeNumber" runat="server" Width="200px"></asp:TextBox></td>
                    <td><asp:TextBox ID="tbxWorkNumber" runat="server" Width="200px"></asp:TextBox></td>
                    <td><asp:TextBox ID="tbxMobile" runat="server" Width="200px"></asp:TextBox></td>
                    <td><asp:TextBox ID="tbxChangedBy_" runat="server" Width="200px" Enabled="false"></asp:TextBox></td>
                    <td><asp:TextBox ID="tbxChangeOn_" runat="server" Width="200px" Enabled="false"></asp:TextBox></td> 
                </tr>--%>
                <tr>
                    <td>
                        <asp:Button ID="btnClean" runat="server" Text="Clear" ValidationGroup="SaveInfo"  OnClick="btnClean_Click"/>
                        <asp:Button ID="btnSaveNextKin" runat="server" Text="Save" ValidationGroup="SaveInfo" OnClick="btnSaveNextKin_Click"/>
                        <asp:TextBox ID="tbxNextOfKinID" runat="server" Width="30px" Enabled="false"></asp:TextBox>
                    </td>
                </tr>
                
            </table>
        </div>

        
    </div>  
    
    
    <div class="WinGroupBox">
        <div class="WinGroupBoxHeader">
            Certification List
        </div>
        <div>
            <asp:GridView ID="grdsearch" runat="server" AutoGenerateColumns="False" GridLines="None"
                CssClass="GridView" OnRowCommand="GridView1_RowCommand" OnRowDataBound="GridView1_RowDataBound"
                OnRowDeleting="GridView1_RowDeleting" 
                OnRowEditing = "GridView1_RowEditing"
                EmptyDataText="No record found">
                <Columns>
                    <asp:BoundField HeaderText="Type" DataField="TypeID" />
                    <asp:BoundField HeaderText="Details" DataField="Details" />
                    <asp:BoundField HeaderText="Expiry Date" DataField="ExpiryDate" />
                    <asp:BoundField HeaderText="Place Issued" DataField="PlaceIssued" />
                    <asp:BoundField HeaderText="Changed By" DataField="ChangedByUserID" />
                    <asp:BoundField HeaderText="Changed On" DataField="ChangedOn" />
                    
                    <asp:TemplateField HeaderText="Edit" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:LinkButton ID="LinkButton2" CommandArgument='<%# Eval("ID") %>' CommandName="Edit" runat="server">
                                <img src="../Images/edit.jpg"  style="border:none; height:20px; width:20px;" />
                            </asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    
                    <asp:TemplateField HeaderText="Delete" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:LinkButton ID="LinkButton1" CommandArgument='<%# Eval("ID") %>' CommandName="Delete" runat="server">
                                <img src="../Images/delete.jpg"  style="border:none; height:20px; width:20px;" />
                            </asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div> 
 </asp:Content>   
