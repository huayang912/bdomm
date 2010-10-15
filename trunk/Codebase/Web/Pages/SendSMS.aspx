<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" 
AutoEventWireup="true" CodeFile="SendSMS.aspx.cs" 
Inherits="Pages_SendSMS" enableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

    <script language="javascript" type="text/javascript">
        function GetDetails(clientID) {
            ShowProgress();
            PageMethods.GetClientContact(clientID, OnGetClientSuccess, OnAjaxFailure);
        }

        function OnAjaxFailure(error) {
            HideProgress();
            alert(error.get_message());
        }
        function MoveNext(stepIndex) {
            for(i = 1; i < 5; i++)
            {
                document.getElementById('divStep' + i).style.display = 'none';
            }
            document.getElementById('divStep' + stepIndex).style.display = 'block';

            if (stepIndex < 3) {
                document.getElementById('divStep3').style.display = 'block';
            }

            if (stepIndex == 1) {

                //document.getElementById('btnFinish').disabled = true;
                document.getElementById('btnPrev').disabled = true;
                document.getElementById('btnNext').disabled = false;
            }

            if (stepIndex == 2) {
                //document.getElementById('btnFinish').disabled = false;
                document.getElementById('btnPrev').disabled = false;
                document.getElementById('btnNext').disabled = true;
            }

//            if (stepIndex == 1) {
//                alert("1");
//                document.getElementById('btnFinish').disabled = true;
//               
//            }

//            if (stepIndex == 2) {
//                alert("2");
//                document.getElementById('btnFinish').disabled = false;
//            }
        }
        
        
        function CreateStausLinks(enquiryID)
        {
            //alert(enquiryID);
            if(_Enquiry.ID > 0)
                $('#aNewQuation').attr('href', '/Pages/QuotationList.aspx?ENQ=' + enquiryID).html('View the quotations related to this enquiry');
            else
                $('#aNewQuation').attr('href', '/Pages/QuotationChange.aspx?ENQ=' + enquiryID);
            
        }
    </script>

    <script language="JavaScript">
        function setMouseOverColor(element) {
            try {
                element.style.cursor = 'hand';
                oldgridSelectedColor = element.style.backgroundColor;
                element.style.backgroundColor = 'Thistle';
                //element.style.cursor='wait';
                //element.style.textDecoration='underline';
            }
            catch (e) {

            }
        }

        function setMouseOutColor(element) {
            try {
                element.style.backgroundColor = oldgridSelectedColor;
                element.style.textDecoration = 'none';
            }
            catch (e) {

            }
        }

        function textboxMultilineMaxNumber(txt, maxLen) {
            try {
                if (txt.value.length > (maxLen - 1)) return false;
            } catch (e) {
            }
        }


        function SendMessage() {
            var ids = '';
            $("#<%=GridView1.ClientID %> input[type='checkbox']:checked").each(function(i) {
                if(i == 0)
                    ids += $(this).val();
                else
                    ids += ',' + $(this).val();
            });
            
            var message = $('#<%=tbxMessage.ClientID %>').val();
            AjaxService.SendSms(ids, message, OnSendSmsSuccess, OnAjax_Error, OnAjax_TimeOut);
        }
        function OnSendSmsSuccess(result) {
            //alert(result);
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageHeaderContentPlaceHolder" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="SideBarPlaceHolder" Runat="Server">
</asp:Content>


<asp:Content ID="Content4" ContentPlaceHolderID="PageContentPlaceHolder" Runat="Server">
    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
        <Services>
            <asp:ServiceReference Path="~/Services/AjaxService.asmx" />
        </Services>
    </asp:ScriptManagerProxy>
    
    <div class="GroupBox">
        <table cellpadding="5" cellspacing="0" border="0" width="100%">
        <tr>
            <td>
               <asp:Label ID="lblMessage" runat="server" Font-Bold="True" Font-Size="Medium"></asp:Label>
                
                <div id="divStep1" style="display:block;">
                    Please Insert The Message To Be Sent<br />
                    <asp:TextBox ID="tbxMessage" runat="server" TextMode="MultiLine" Height="300px" Width="99%"></asp:TextBox>
                </div>
                <div class="GroupBox" id="divStep2" style="display:none;">
                    <asp:GridView ID="GridView1" runat="server" CssClass="GridView" 
                        AutoGenerateColumns="False" CellPadding="3" CellSpacing="0"                         
                        onrowdatabound="GridView1_RowDataBound" Enabled="False">
                        <%--
                        DataSourceID="LinqDataSource1" --%>
                        <Columns>
                            <asp:TemplateField HeaderText="Select">
                                <ItemTemplate>
                                    <input type="checkbox" name="chkSelect" value="<%# ((int)Eval("ID")).ToString() %>" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Recipient_Name" HeaderText="Recipient_Name" 
                                ReadOnly="True" SortExpression="Recipient_Name" />
                            <asp:BoundField DataField="Destination" HeaderText="Destination" 
                                ReadOnly="True" SortExpression="Destination" />
                            <asp:BoundField DataField="SMS_Credits" HeaderText="SMS_Credits" 
                                ReadOnly="True" SortExpression="SMS_Credits" />
                            <asp:BoundField DataField="ID" HeaderText="ID" Visible="false" ReadOnly="True" 
                                SortExpression="ID" />
                        </Columns>
                        <RowStyle CssClass="OddRowStyle" />
                        <AlternatingRowStyle CssClass="EventRowStyle" />
                    </asp:GridView> <br />
                    
                    
                    
                    <asp:Button ID="btnVerifyPhoneNumbers" runat="server" 
                        Text="Verify Phone Numbers" onclick="btnVerifyPhoneNumbers_Click"/>
                    <asp:Button ID="btnDelete" runat="server" Text="Delete Recipient" 
                        onclick="btnDelete_Click"/>
                </div>
            </td>
        </tr>
        <tr>
            <td align="right">
                 <div id="divStep3" style="display:block;">
                    <input type="button" id="btnPrev" value="< Previous" onclick="MoveNext(1);"  disabled="true"/>&nbsp;
                    <input type="button" id="btnNext" value="Next >" onclick="MoveNext(2);" />
                    
                    <%--<asp:Button ID="btnPrevious" runat="server" Text="< Previous" 
                         Enabled="false" onclick="javascript:MoveNext(1);"/>
                    <asp:Button ID="btnNext" runat="server" Text="Next >" 
                        onclick="MoveNext(2);" />--%>
                    <%--<asp:Button ID="btnFinish" runat="server" Text="Finish" onclick="SendMessage();" />--%>
                    
                    <input type="button" value="Finish" onclick="SendMessage();" />
                    
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" />
                </div>
            </td>
        </tr>
        <tr>
            <td>
            
                <div id="divStep4" style="display:none;">
                    <table cellpadding="5" cellspacing="5" border="0" width="100%">
                        <tr>
                            <td>
                                <asp:Button ID="btnBackToAll" runat="server" Text="< Back To All" 
                                    Enabled="True" onclick="btnBackToAll_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="tbxMobileNumber" runat="server" Width="99%"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Please Insert The Message To Be Sent
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="tbxSingleSms" runat="server" Height="200px" Width="99%"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Button ID="btnSendSingleSMS" runat="server" Text="Send SMS" 
                                    Enabled="True" />
                            </td>
                        </tr>
                    </table>
              </div>
                
            </td>
        </tr>
    </table>    
    </div>
    
    
    
    <asp:LinqDataSource ID="LinqDataSource1" runat="server"
        ContextTypeName="OMMDataContext" 
        TableName="Message_Recipients" 
        Select="new (Recipient_Name, Destination, SMS_Credits, ID)">
    </asp:LinqDataSource>
</asp:Content>

