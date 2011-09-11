<%@ Page Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="CVSearch.aspx.cs" 
Inherits="Pages_CVSearch"  Title="Personnel CV"%>
<%@ Register Src="~/Controls/cv_usr_control.ascx" TagName="cv_usr_control"  TagPrefix="uc"%>
<%@ MasterType VirtualPath="~/Main.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageHeaderContentPlaceHolder" runat="Server">CV Search</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageContentPlaceHolder" runat="Server">
    <script language="javascript" type="text/javascript">
        function ValidateKeyword(sender, args) {
            if (args.Value.length > 1)
                args.IsValid = true;
            else
                args.IsValid = false;
        }
    </script>
    <div class="WinGroupBox">
        <div class="WinGroupBoxHeader">Search Options</div>
        <div>
            Keyword &nbsp;
            <asp:TextBox ID="txtKeyword" MaxLength="50" runat="server"></asp:TextBox>
            <asp:RadioButton ID="rdbPersonnelCV" runat="server" Text="Personnel CV" GroupName="SearchType" Checked="true" />
            <!--<asp:RadioButton ID="rdbCVBank" runat="server" Text="CV Bank" GroupName="SearchType" /> -->
            <asp:Button ID="btnSearch" runat="server" Text="Search" ValidationGroup="Search" OnClick="btnSearch_Click" />
        </div>
        <div>
            <asp:RequiredFieldValidator ID="rfvSearch" runat="server"
                SetFocusOnError="true" Display="Dynamic"
                ControlToValidate="txtKeyword"
                ErrorMessage="Please enter a Keyword to Search."
                ValidationGroup="Search">
            </asp:RequiredFieldValidator>
            <asp:CustomValidator ID="RequiredFieldValidator1" runat="server"
                SetFocusOnError="true" Display="Dynamic"
                ControlToValidate="txtKeyword" ClientValidationFunction="ValidateKeyword"
                ErrorMessage="Please enter More than One Letter as Keyword."
                ValidationGroup="Search">
            </asp:CustomValidator>
        </div>
    </div>
  
    <div class="WinGroupBox">
        <div class="WinGroupBoxHeader">Search Result</div>
        <asp:GridView ID="grdsearch" runat="server" GridLines="None" AutoGenerateColumns="False"
            CssClass="GridView">
            <Columns>
                <asp:BoundField HeaderText="File Name" DataField="FILENAME" /> 
                <asp:BoundField HeaderText="First Name" DataField="FirstNames" /> 
                <asp:BoundField HeaderText="Last Name" DataField="LastName" /> 
                <asp:BoundField HeaderText="Address" DataField="Address" /> 
                <asp:BoundField HeaderText="File Abstract" DataField="characterization" /> 
                <asp:BoundField HeaderText="File Size (KB)" DataField="Size" />             
                <asp:TemplateField HeaderText="..">
                    <ItemTemplate>
                        <%-- <a href='<%#Eval("PATH")%>'>Resume</a>--%>
                        <%--<a href='<%# Eval("PATH").ToString().Replace("d:\\project\\ommm\\smssendingdummy\\codebase\\web\\uploadedcv\\", "http://omm.local.com//uploadedcv//") %>'>
                            Download</a>--%>
                        <a href='<%# GetRelativeDownloadUrl(Eval("PATH").ToString())%>'>Download</a>    
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>