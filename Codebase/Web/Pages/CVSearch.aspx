<%@ Page Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="CVSearch.aspx.cs" 
Inherits="Pages_CVSearch"  Title="Personnel CV"%>
<%@ Register Src="~/Controls/cv_usr_control.ascx" TagName="cv_usr_control"  TagPrefix="uc"%>
<asp:Content ID="Content1" ContentPlaceHolderID="PageHeaderContentPlaceHolder" runat="Server">CV Search</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageContentPlaceHolder" runat="Server">
    <div factory:flow="NewRow" xmlns:factory="urn:codeontime:app-factory">
    <%-- <uc:cv_usr_control ID="c100" runat="server"></uc:cv_usr_control>--%>
      <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
      <asp:Button ID="Button1" runat="server" Text="Search" onclick="Button1_Click" />
      <br />
      <br />
      <asp:GridView ID="grdsearch" runat="server" AutoGenerateColumns="False" 
            Width="100%">
          <RowStyle BackColor="#FFFFCC" />
          <Columns>
              <asp:BoundField HeaderText="File Name" DataField="FILENAME" />
              <asp:BoundField HeaderText="File Size" DataField="SIZE" />
              
              
              <%--<asp:BoundField HeaderText="Education" DataField="Education" />
              <asp:BoundField HeaderText="Skills" DataField="Skillsetdetails" />
              <asp:BoundField HeaderText="Total Experience" DataField="Totalexperience" />
              <asp:BoundField HeaderText="Relvent Experience" DataField="Relventexperience" />
              <asp:BoundField HeaderText="Address" DataField="Address" />
              <asp:BoundField HeaderText="Previous Company Detail" DataField="Previousemployerdetail" />
              <asp:BoundField HeaderText="Mobile Number" DataField="Mobileno" />
              <asp:BoundField HeaderText="Availability" DataField="Availability" />
              <asp:BoundField HeaderText="Currentctc" DataField="Currentctc" />
              <asp:BoundField HeaderText="Expectedctc" DataField="Expectedctc" />
              <asp:BoundField HeaderText="Preferred Location" DataField="StateName" />
              <asp:BoundField HeaderText="Resume Name" DataField="ResumeName" />--%>
              
              <asp:TemplateField HeaderText="File Location">
                  <ItemTemplate>
                        <%-- <a href='<%#Eval("PATH")%>'>Resume</a>--%>
                        '<%# Eval("PATH").ToString().Replace("d:\\project\\ommm\\smssendingdummy\\codebase\\web\\uploadedcv\\", "http://omm.local.com//uploadedcv//") %>'
                        
                      
                  </ItemTemplate>
              </asp:TemplateField>
              
              <asp:TemplateField HeaderText="..">
                  <ItemTemplate>
                        <%-- <a href='<%#Eval("PATH")%>'>Resume</a>--%>
                        <a href='<%# Eval("PATH").ToString().Replace("d:\\project\\ommm\\smssendingdummy\\codebase\\web\\uploadedcv\\", "http://omm.local.com//uploadedcv//") %>'>Download</a>
                        
                      
                  </ItemTemplate>
              </asp:TemplateField>
          </Columns>
          <AlternatingRowStyle BackColor="White" />
      </asp:GridView>
  </div>
</asp:Content>