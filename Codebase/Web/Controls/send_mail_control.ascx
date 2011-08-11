<%@ Control Language="C#" AutoEventWireup="true" CodeFile="send_mail_control.ascx.cs" Inherits="Controls_send_mail_control"  %>


<asp:UpdatePanel ID="UpdatePanel1" runat="server">
<ContentTemplate><div style="margin:2px;border: solid 1px silver;padding:8px;">
    <div> 
      <div align="center">
        <p><strong>BUDI Feedback Form
</strong></p>
        <p>&nbsp;</p>
      </div>
      <table align=center>
				<tr>
					<td><strong>Your Name:</strong></td>
					<td><asp:textbox id="txtName" Width="241" Runat="server"></asp:textbox></td>
				</tr>
				<tr>
					<td><strong>Your Email Address</strong>:</td>
					<td><asp:textbox id="txtEmail" Width="241" Runat="server"></asp:textbox></td>
				</tr>
				<tr>
					<td colSpan="2" ><strong>Your Comments:</strong></td>
				</tr>
				<tr>
					<td align="center" colSpan="2" width=100%><asp:textbox id="txtMessage" Width="100%" Runat="server" Height="99" TextMode="MultiLine" MaxLength="400"></asp:textbox></td>
				</tr>
				<tr>
					<td colSpan="2">&nbsp;</td>
				</tr>
				<tr>
					<td align=center><asp:button id="btnSendmail" Runat="server" Text="Send Mail" OnClick="btnSendmail_Click"></asp:button></td>
					<td align=center><asp:button id="btnReset" Runat="server" Text="Reset" OnClick="btnReset_Click"></asp:button></td>
				</tr>
				<tr>
					<td colSpan="2"><asp:label id="lblStatus" Runat="server" EnableViewState="False"></asp:label></td>
				</tr>
	  </table>
  </div></div></ContentTemplate>
</asp:UpdatePanel>