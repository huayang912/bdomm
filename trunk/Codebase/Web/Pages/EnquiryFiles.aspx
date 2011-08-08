<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPagePopup.master" AutoEventWireup="true" CodeFile="EnquiryFiles.aspx.cs" Inherits="Pages_EnquiryFiles" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script language="javascript" type="text/javascript">
        $(document).ready(function() {
            var fileName = $('#<%=hdnFileName.ClientID %>').val();
            if (fileName.length > 0) {
                window.opener.AddAttachmentLink(fileName);
                window.close();
            }
        });
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="body" Runat="Server">
    <asp:HiddenField id="hdnFileName" runat="server" />
    <div style="width:480px; margin:10px; margin-right:27px;">
        <div class="GroupBox">
            <div id="divMessage" runat="server" enableviewstate="false" visible="false"></div>
            <asp:Panel ID="pnlUploadContainer" runat="server">
                <div class="FormHeader" style="padding-bottom:10px;">
                    <b><asp:Literal ID="ltrHeading" runat="server"></asp:Literal></b><br />
                    Attach files with this Enquiry. 
                    Remeber, only Microsoft Word (*.doc, *.docx) and PDF (*.pdf) documents are allowed for attachment.
                </div>
                <div class="floatleft">Select File</div>
                <div class="floatleft" style="margin-left:5px;"><asp:FileUpload ID="fileEnquiry" runat="server" /></div>
                <div class="clearboth"></div>
                <div style="margin-top:20px;">
                    <asp:Button ID="btnUpload" runat="server" Text="Attach" OnClick="btnUpload_Click" />     
                </div>
            </asp:Panel>
        </div>
    </div>   
</asp:Content>

