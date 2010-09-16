<%@ Control Language="VB" AutoEventWireup="true" CodeFile="cv_usr_control.ascx.vb" Inherits="Controls_cv_usr_control"  %>

<asp:UpdatePanel ID="UpdatePanel1" runat="server">


    <ContentTemplate>
       <div class="SettingsPanel">
     <table width="100%" border="0">
  <tr>
    <td><table>
                <tr>
                
                    
                    <td>
                        CV:<br />
                       
                        
                        <asp:TextBox ID="CVTextBox1" runat="server"></asp:TextBox>
                    </td>
                        <td>
                        Notes:<br />
                       
                        
                        <asp:TextBox ID="NotesTextBox1" runat="server"></asp:TextBox>
                    </td>
                  <td>
                        <br />
                        <asp:Button ID="SearchButton" runat="server" Text="Search" 
                            onclick="SearchButton_Click" />                  </td>
                </tr>
            </table></td>
    <td align="right"><table>
                <tr>
                
                 <td>
                    <br />
                    <asp:Button ID="ShowAllButton" runat="server" Text="Show All" 
                            onclick="ShowAllButton_Click" />                    </td>
                </tr>
            </table></td>
  </tr>
</table>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>

			    <div id="dv100" runat="server"></div>
    <aquarium:DataViewExtender id="dv100Extender" runat="server" TargetControlID="dv100" Controller="CVs_m" view="grid1" PageSize="25"/>
  

