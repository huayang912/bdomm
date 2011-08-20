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
    <div runat="server" id="divAttachmentError">
                    </div>
    <div class="WinGroupBox">
        <div class="WinGroupBoxHeader">
            Select File</div>
        <div>
            <asp:FileUpload ID="fileUploadCV" runat="server" />
            <asp:Button ID="btnUpload" runat="server" Text="Upload" ValidationGroup="SaveInfo"
                OnClick="btnUpload_onclick" />
        </div>
    </div> 
    
    <div class="WinGroupBox">
        <div class="WinGroupBoxHeader">
            List of Documents</div>
        <div>
        
        <div>
            <asp:GridView ID="grdUploadedDocument" runat="server" GridLines="None" CssClass="GridView">
            </asp:GridView>
        </div>
        <div>
            <asp:GridView ID="grdsearch" runat="server" AutoGenerateColumns="False" GridLines="None"
                CssClass="GridView" OnRowCommand="GridView1_RowCommand" OnRowDataBound="GridView1_RowDataBound"
                OnRowDeleting="GridView1_RowDeleting" EmptyDataText="No record found">
                <Columns>
                    <asp:BoundField HeaderText="File Name" DataField="FileName" />
                    <asp:BoundField HeaderText="Created On" DataField="ChangedOn" />
                    <asp:TemplateField HeaderText="Download" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <a href='/uploadedcv/<%# Eval("ID").ToString()%>_<%# Eval("FileName").ToString()%>'>
                                <img src="../Images/download.jpg" style="border:none; height:20px; width:20px;" />
                            </a>
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
