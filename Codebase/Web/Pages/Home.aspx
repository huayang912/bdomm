<%@ Page Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="Home.aspx.cs" Inherits="Pages_Home"  Title="Start"%>
<%@ Register Src="~/Controls/TableOfContents.ascx" TagName="TableOfContents"  TagPrefix="uc"%>
<asp:Content ID="Content1" ContentPlaceHolderID="PageHeaderContentPlaceHolder" runat="Server">Start</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageContentPlaceHolder" runat="Server">
  
  <table width="800" border="0" background="">
  <tr>
    <td>&nbsp;</td>
	<td align="center">
        <img alt="graph" src="../Images/sample-graph.jpg" 
            style="width: 819px; height: 460px" /></td>
    <td>&nbsp;</td>
  </tr>
</table>

  

</asp:Content>
<asp:Content ID="Content3" runat="server" contentplaceholderid="head">
</asp:Content>
