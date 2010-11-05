<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ProjectNoteDetails.aspx.cs" Inherits="Pages_ProjectNoteDetails" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../Scripts/Main.css" rel="Stylesheet" type="text/css" />    
    <script language="javascript" src="../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
</head>
<body style="padding-top:0px;">
    <form id="form1" runat="server">
        <div style="margin:10px;">            
            <div id="divMessage" runat="server" enableviewstate="false" visible="false"></div>
            <asp:Panel ID="pnlDetailsContainer" runat="server" CssClass="GroupBox">
                <div style="margin-left:5px;"><asp:Literal ID="ltrProjectName" runat="server"></asp:Literal></div>
                <table id="tblProjectList" class="GridView" cellpadding="5" cellspacing="0">
                    <colgroup>
                        <col style="width:100px;" />
                        <col /> 
                        <col style="width:10px;" />       
                    </colgroup>                    
                    <tr>
                        <td><asp:Literal ID="ltrUserName" runat="server"></asp:Literal></td>
                        <td><asp:Literal ID="ltrDetails" runat="server"></asp:Literal></td>
                        <td></td>
                    </tr>  
                </table>             
            </asp:Panel>
        </div>
    </form>
</body>
</html>
