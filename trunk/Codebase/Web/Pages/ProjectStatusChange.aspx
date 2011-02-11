<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="ProjectStatusChange.aspx.cs" Inherits="Pages_ProjectStatusChange" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script language="javascript" type="text/javascript">        
        var _ProjectID = <%= _ProjectID.ToString()%>;              
        
        function MoveNext(stepIndex) {
            for (i = 1; i < 3; i++) {
                document.getElementById('divStep' + i).style.display = 'none';
            }
            document.getElementById('divStep' + stepIndex).style.display = 'block';
        }
        ///Quotations 
        function ValidateAndSave(){            
            if (Page_ClientValidate('SaveInfo1')){
                ShowProgress(); 
                var newStatusID = $("#<%=rdblStatuses.ClientID%> input[type='radio']:checked").val();          
                PageMethods.ChangeProjectStatus(_ProjectID, newStatusID, OnChangeProjectStatusSuccess, OnAjax_Error, OnAjax_TimeOut);              
            }
        }   
        
        function OnChangeProjectStatusSuccess(result)
        {
            HideProgress();
            if(result == true){            
                $('#divFinalMessage').html('Project Status has been changed successfully.');
                MoveNext(2);
            }
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="PageHeaderContentPlaceHolder" Runat="Server">
    <asp:Literal ID="ltrHeading" runat="server" Text="Change Project Status Wizard"></asp:Literal>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="SideBarPlaceHolder" Runat="Server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="PageContentPlaceHolder" Runat="Server">
    <div id="divMessage" runat="server" enableviewstate="false" visible="false"></div>
        
    <asp:Panel ID="pnlDetails" runat="server">
    
        <div id="divStep1" class="GroupBox" style="display:block;">
            <div class="FormHeader">
                <b>Change Project Status</b><br />
                <asp:Label ID="lblStep1Title" Text="Change the Current Status of a Project." runat="server"></asp:Label>
            </div>
            <table cellpadding="3" cellspacing="0" class="FormTable">
                <colgroup>
                    <col style="width:15%;" />
                    <col/>
                </colgroup>		
				<tr>
				    <td>Project Name</td>					
					<td>
					    <asp:Literal ID="ltrProjectName" runat="server"></asp:Literal>                         
					</td>
				</tr>
				<tr>
				    <td>Status</td>
				    <td>
				        <asp:RadioButtonList ID="rdblStatuses" runat="server"></asp:RadioButtonList>
				        <asp:RequiredFieldValidator ID="rfvStatuses" runat="server"
				            ValidationGroup="SaveInfo1"
				            Display="Dynamic"
				            ControlToValidate="rdblStatuses"
				            ErrorMessage="Select the new Status of this Project.">
				        </asp:RequiredFieldValidator>
				    </td>
				</tr>								                
            </table> 
            <div style="margin-top:70px;">                
                <input type="button" value="Finish" onclick="ValidateAndSave();" />
            </div>
        </div>
        
        <%--Step 2--%>
        <div id="divStep2" class="GroupBox" style="display:none; min-height:120px;">
            <div class="FormHeader">
                <b>Summary</b>                
            </div>
            <div>
                <div id="divFinalMessage"></div>
            </div>
            <%-- Close Button --%>
            <div style="margin-top:20px;">
                <input id="btnClose" type="button" value="Close" disabled="disabled" />
            </div>
        </div>
        
    </asp:Panel>
</asp:Content>

