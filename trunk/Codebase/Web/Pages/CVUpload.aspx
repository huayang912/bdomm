<%@ Page Language="C#" MasterPageFile="~/MasterPagePopup.master" AutoEventWireup="true" CodeFile="CVUpload.aspx.cs"
    Inherits="Pages_CVUpload" Title="CV Upload" %>

<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="Server">    
    
    
    <script type="text/javascript" language="javascript">

        function DeleteCV(id, filePath, contactID) {
            alert(filePath);
            PageMethods.DeleteCVT(id, filePath, contactID, DeleteCV_Success, OnAjax_Error, OnAjax_TimeOut);        
            
            
        }

        function DeleteCV_Success(result) {
            alert('CV Deleted Successfully');
        }
    </script>
    
    <asp:Label runat="server" ID="lbl" ></asp:Label>
    <div factory:flow="NewRow" xmlns:factory="urn:codeontime:app-factory">
        <table cellpadding="3" cellspacing="0" style="width: 90%;">
            <%--<tr>
            <td>
                
                <asp:Label ID="lblFileType" runat="server" Text="Document Type"></asp:Label>
            
                <asp:DropDownList ID="ddlFileType" runat="server">
                    <asp:ListItem Selected="True" Value="1">Doc</asp:ListItem>
                    <asp:ListItem Value="2">Pdf</asp:ListItem>
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddlFileType"
                    SetFocusOnError="true" ErrorMessage="Please Select File Type." Display="Dynamic"
                    ValidationGroup="SaveInfo">
                </asp:RequiredFieldValidator>
            </td>
            </tr>--%>
            <tr>
                <td>
                    <div runat="server" id="divAttachmentError">
                    </div>
                </td>
            </tr>
            <tr>
                <td>Select File:   
                    <asp:FileUpload ID="fileUploadCV" runat="server" />
                    <asp:Button ID="btnUpload" runat="server" Text="Upload" ValidationGroup="SaveInfo"
                        OnClick="btnUpload_onclick"  />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:GridView ID="grdUploadedDocument" runat="server" GridLines="None"  CssClass="GridView">
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:GridView ID="grdsearch" runat="server" AutoGenerateColumns="False" GridLines="None"
                        CssClass="GridView"
                        OnRowCommand="GridView1_RowCommand" 
                        OnRowDataBound="GridView1_RowDataBound"                         
                        OnRowDeleting="GridView1_RowDeleting"  
                        EmptyDataText = "No record found"                      
                        >
                        <Columns>
                            <asp:BoundField HeaderText="File Name" DataField="FileName" />
                            <asp:BoundField HeaderText="Created On" DataField="ChangedOn" />
                            <asp:TemplateField HeaderText="..">
                                <ItemTemplate>
                                    <a href='/uploadedcv/<%# Eval("ID").ToString()%>_<%# Eval("FileName").ToString()%>'>
                                        Download</a>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="..">
                                <ItemTemplate>
                                    <asp:LinkButton ID="LinkButton1" CommandArgument='<%# Eval("ID") %>' CommandName="Delete"
                                        runat="server">
                                 Delete</asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            
        </table>
    </div>
    
 </asp:Content>   
