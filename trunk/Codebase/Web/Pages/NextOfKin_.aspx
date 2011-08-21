<%@ Page Language="C#" MasterPageFile="~/MasterPagePopup.master" AutoEventWireup="true" CodeFile="NextOfKin_.aspx.cs"
    Inherits="Pages_NextOfKin_" Title="Next Of Kin" %>

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
    <div class="WinGroupBox">
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
            <%--<asp:FileUpload ID="fileUploadCV" runat="server" />--%>
            <asp:Button ID="btnUpload" runat="server" Text="Save" ValidationGroup="SaveInfo"
                OnClick="btnUpload_onclick" />
        </div>
    </div> 
    
    <div class="WinGroupBox">
        <div class="WinGroupBoxHeader">
            Next of Kin Information
        </div>
        
        
        <div>
            <table cellpadding=1 cellspacing=1 border=0>
                <tr>
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
                    <td><asp:TextBox ID="tbxChangedBy" runat="server" Width="200px" Enabled="false"></asp:TextBox></td>
                    <td><asp:TextBox ID="tbxChangeOn" runat="server" Width="200px" Enabled="false"></asp:TextBox></td> 
                </tr>
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
            Next of Kin List
        </div>
        <div>
            <asp:GridView ID="grdsearch" runat="server" AutoGenerateColumns="False" GridLines="None"
                CssClass="GridView" OnRowCommand="GridView1_RowCommand" OnRowDataBound="GridView1_RowDataBound"
                OnRowDeleting="GridView1_RowDeleting" 
                OnRowEditing = "GridView1_RowEditing"
                EmptyDataText="No record found">
                <Columns>
                    <asp:BoundField HeaderText="Name" DataField="Name" />
                    <asp:BoundField HeaderText="Relationship" DataField="Relationship" />
                    <asp:BoundField HeaderText="Address" DataField="Address" />
                    <asp:BoundField HeaderText="Postcode" DataField="Postcode" />
                    <asp:BoundField HeaderText="CountryID" DataField="CountryID" />
                    <asp:BoundField HeaderText="HomeNumber" DataField="HomeNumber" />
                    <asp:BoundField HeaderText="WorkNumber" DataField="WorkNumber" />
                    <asp:BoundField HeaderText="MobileNumber" DataField="MobileNumber" />
                    <asp:BoundField HeaderText="ChangedByUserID" DataField="ChangedByUserID" />
                    <asp:BoundField HeaderText="ChangedOn" DataField="ChangedOn" />
                    
                    
                    <%--<asp:TemplateField HeaderText="Edit" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <a href="javascript:void(0);" onclick="editNextOfKin('1');">
                                <img src="../Images/download.jpg" style="border:none; height:20px; width:20px;" onclick="editNextOfKin(1)" />
                            </a>
                        </ItemTemplate>
                    </asp:TemplateField>--%>
                    
                    <asp:TemplateField HeaderText="Edit" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:LinkButton ID="LinkButton2" CommandArgument='<%# Eval("ID") %>' CommandName="Edit" runat="server">
                                <img src="../Images/download.jpg"  style="border:none; height:20px; width:20px;" />
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
