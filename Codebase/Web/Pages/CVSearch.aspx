<%@ Page Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="CVSearch.aspx.cs" 
Inherits="Pages_CVSearch"  Title="Personnel CV"%>
<%@ Register Src="~/Controls/cv_usr_control.ascx" TagName="cv_usr_control"  TagPrefix="uc"%>
<asp:Content ID="Content1" ContentPlaceHolderID="PageHeaderContentPlaceHolder" runat="Server">CV Search</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageContentPlaceHolder" runat="Server">
    <div class="WinGroupBox">
        <div class="WinGroupBoxHeader">
            Search Options</div>
        Keyword
        <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
        <asp:RadioButton ID="rdbPersonnelCV" runat="server" Text="Personnel CV" GroupName="SearchType"
            Checked="true" />
        <asp:RadioButton ID="rdbCVBank" runat="server" Text="CV Bank" GroupName="SearchType" />
        <asp:Button ID="Button1" runat="server" Text="Search" OnClick="Button1_Click" />
    </div>
  
    <div class="WinGroupBox">
        <asp:GridView ID="grdsearch" runat="server" GridLines="None" AutoGenerateColumns="False"
            CssClass="GridView">
            <Columns>
                <asp:BoundField HeaderText="File Name" DataField="FILENAME" />
                <asp:BoundField HeaderText="File Size (KB)" DataField="SIZE" />
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