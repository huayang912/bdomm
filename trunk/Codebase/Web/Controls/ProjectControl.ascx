<%@ Control Language="VB" AutoEventWireup="true" CodeFile="ProjectControl.ascx.VB"
    Inherits="Controls_ProjectControl" %>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
    
     <div class="SettingsPanel">
     <table width="100%" border="0">
  <tr>
    <td><table>
                <tr>
                
                    
                    <td>
                        Status:<br />
                       
                        
                        <asp:DropDownList ID="StatusList" runat="server">
                        <asp:ListItem>All</asp:ListItem>                        
                        <asp:ListItem>inProgress</asp:ListItem>
                         <asp:ListItem>Complete</asp:ListItem>
                        </asp:DropDownList>                  </td>
                  <td>
                        <br />
                        <asp:Button ID="SearchButton" runat="server" Text="Search" 
                            onclick="SearchButton_Click" />                  </td>
                </tr>
            </table></td>
    <td align="right"><table>
                <tr>
                
                    <td>
                      Year:<br />
                       
                        
                      <asp:DropDownList ID="YearList" runat="server">
                        <asp:ListItem>All</asp:ListItem>
                        <asp:ListItem Value="1/1/2008" Text="2008">2008</asp:ListItem>
                        <asp:ListItem Text="2009" Value="1/1/2009">2009</asp:ListItem>
                        <asp:ListItem Text="2010" Value="1/1/2010">2010</asp:ListItem>
                      </asp:DropDownList>                      </td>
                        
                                         <td>
                        Month:<br />
                       
                        
                        <asp:DropDownList ID="MonthList" runat="server">
                         <asp:ListItem>All</asp:ListItem>
                        <asp:ListItem Value="1" Text="January">January</asp:ListItem>
                        <asp:ListItem Value="2" Text="February">February</asp:ListItem>
                        <asp:ListItem Value="3" Text="March">March</asp:ListItem>
                              <asp:ListItem Value="4" Text="April">April</asp:ListItem>
                        <asp:ListItem Value="5" Text="May">May</asp:ListItem>
                        <asp:ListItem Value="6" Text="June">June</asp:ListItem>
                         <asp:ListItem Value="7" Text="July">July</asp:ListItem>
                        <asp:ListItem Value="8" Text="August">August</asp:ListItem>
                        <asp:ListItem Value="9" Text="September">September</asp:ListItem>
                   
     <asp:ListItem Value="10" Text="Onctober">Onctober</asp:ListItem>
                        <asp:ListItem Value="11" Text="November">November</asp:ListItem>
                        <asp:ListItem Value="12" Text="December">December</asp:ListItem>
                        </asp:DropDownList>                        </td>
                        
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
     <div factory:flow="NewRow" xmlns:factory="urn:codeontime:app-factory">
    <div id="view1" runat="server"></div>
    <aquarium:DataViewExtender id="view1Extender" runat="server" TargetControlID="view1" Controller="Projects" view="grid1" ShowInSummary="True"  ShowActionBar ="true"  PageSize="25"/>
  </div>

  </div>
 
 
  