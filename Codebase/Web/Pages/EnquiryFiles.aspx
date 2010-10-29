<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EnquiryFiles.aspx.cs" Inherits="Pages_EnquiryFiles" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../Scripts/Main.css" rel="Stylesheet" type="text/css" />    
    <script language="javascript" src="../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>      
</head>
<body style="padding-top:0px;">
    <form id="form1" runat="server">
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
    </form>
    
    <script language="javascript" type="text/javascript">
        $(document).ready(function() {
            var fileName = $('#<%=hdnFileName.ClientID %>').val();
            if (fileName.length > 0) {
                window.opener.AddAttachmentLink(fileName);
                window.close();
            }
        });
    </script> 
    
</body>
</html>
