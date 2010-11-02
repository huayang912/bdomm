<%@ Page Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="Home.aspx.cs" Inherits="Pages_Home"  Title="Start"%>
<%@ Register Src="~/Controls/TableOfContents.ascx" TagName="TableOfContents"  TagPrefix="uc"%>
<asp:Content ID="Content1" ContentPlaceHolderID="PageHeaderContentPlaceHolder" runat="Server">Start</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageContentPlaceHolder" runat="Server">
  
  <table width="800" border="0" background="">
  <tr>
    <td>&nbsp;<img src="../App_Themes/BUDI2_NS/omm-log-vertical.gif" alt="omm" width="84" height="316" border="0" /></td>
	<td>&nbsp;</td>
    <td><table width="100%" border="0">
 
  <tr>
    <td>&nbsp;<table width="100%" border="0">
  <tr>
    <td align="center">&nbsp;<a href="client-omm.aspx"><img src="../App_Themes/BUDI2_NS/omm-log-bg.gif" alt="Client" width="51" height="52" border="0"  /></a></td>
	    <td align="center">&nbsp;<a href="Personnel-omm.aspx"><img src="../App_Themes/BUDI2_NS/omm-log-bg.gif" alt="Personnel" width="51" height="52" border="0"  /></a></td>

    <td align="center">&nbsp;<a href="enq-omm.aspx"><img src="../App_Themes/BUDI2_NS/omm-log-bg.gif" alt="Enquiries" width="51" height="52" border="0"  /></a></td>
    <td align="center">&nbsp;<a href="QuoteSrch.aspx"><img src="../App_Themes/BUDI2_NS/omm-log-bg.gif" alt="Quotations" width="51" height="52" border="0"  /></a></td>
	    <td align="center">&nbsp;<a href="ProjectSrch.aspx"><img src="../App_Themes/BUDI2_NS/omm-log-bg.gif" alt="Projects" width="51" height="52" border="0"  /></a></td>
  </tr>
  <tr>
    <td align="center">&nbsp;<a href="client-omm.aspx">Clients</a></td>
	    <td align="center">&nbsp;<a href="Personnel-omm.aspx">Personnel</a></td>
    <td align="center">&nbsp;<a href="enq-omm.aspx">Enquiries</a></td>
    <td align="center">&nbsp;<a href="QuoteSrch.aspx">Quotations</a></td>
	    <td align="center">&nbsp;<a href="ProjectSrch.aspx">Projects</a></td>
  </tr>
  
</table></td>
  </tr> <tr>
    <td><img src="../App_Themes/BUDI2_NS/omm-background.jpg" alt="Client" width="688" height="170" border="0"  />

</td>
  </tr>
</table>
&nbsp;</td>
  </tr>
</table>

  

</asp:Content>