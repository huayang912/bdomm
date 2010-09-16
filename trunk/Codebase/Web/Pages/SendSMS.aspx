<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="SendSMS.aspx.cs" Inherits="Pages_SendSMS" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageHeaderContentPlaceHolder" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="SideBarPlaceHolder" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="PageContentPlaceHolder" Runat="Server">
    <table cellpadding="5" cellspacing="0" border="0" width="100%">
        <tr>
            <td>
                Please Insert The Message To Be Sent<br />
                <asp:TextBox ID="tbxMessage" runat="server" Height="43px" Width="520px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="btnNext" runat="server" Text="Next >>" 
                    onclick="btnNext_Click" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:GridView ID="GridView1" runat="server" 
                    DataSourceID="LinqDataSource1" Width="100%" 
                    AllowPaging="True" AutoGenerateColumns="False">
                    <Columns>
                        <asp:BoundField DataField="Recipient_Name" HeaderText="Recipient_Name" 
                            ReadOnly="True" SortExpression="Recipient_Name" />
                        <asp:BoundField DataField="Destination" HeaderText="Destination" 
                            ReadOnly="True" SortExpression="Destination" />
                        <asp:BoundField DataField="SMS_Credits" HeaderText="SMS_Credits" 
                            ReadOnly="True" SortExpression="SMS_Credits" />
                        <asp:BoundField DataField="ID" HeaderText="ID" ReadOnly="True" 
                            SortExpression="ID" />
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
        
        <tr>
            <td>
                <asp:Button ID="btnVerifyPhoneNumbers" runat="server" 
                    Text="Verify Phone Numbers" onclick="btnVerifyPhoneNumbers_Click"/>
                <asp:Button ID="btnDelete" runat="server" Text="Delete Recipient" 
                    onclick="btnDelete_Click"/>
            </td>
        </tr>
        
        <tr>
            <td>
                <asp:Button ID="btnFinish" runat="server" Text="Finish" onclick="btnFinish_Click" 
                    />
            </td>
        </tr>
    </table>
    
    
    
    <asp:LinqDataSource ID="LinqDataSource1" runat="server"
        ContextTypeName="OMMDataContext" 
        TableName="Message_Recipients" 
        Select="new (Recipient_Name, Destination, SMS_Credits, ID)">
    </asp:LinqDataSource>
</asp:Content>

