<%@ Page Language="C#" MasterPageFile="~/MasterPagePopup.master" AutoEventWireup="true" CodeFile="CVUpload.aspx.cs"
    Inherits="Pages_CVUpload" Title="CV Upload" %>

<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="Server">    


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
                        OnClick="btnUpload_onclick" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:GridView ID="grdUploadedDocument" runat="server" GridLines="None"  CssClass="GridView">
                    </asp:GridView>
                </td>
            </tr>
        </table>
    </div>
    
 </asp:Content>   
    
<%--
<%@ Register Src="~/Controls/cv_usr_control.ascx" TagName="cv_usr_control" TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageHeaderContentPlaceHolder" runat="Server">
    CV Upload</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageContentPlaceHolder" runat="Server">    
</asp:Content>--%>