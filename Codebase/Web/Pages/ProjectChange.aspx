<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="ProjectChange.aspx.cs" Inherits="Pages_ProjectChange" %>
<%@ Register Src="~/Controls/jQueryCalendar.ascx" TagName="jQueryCalendar" TagPrefix="UC"%>
<%@ MasterType VirtualPath="~/Main.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <UC:jQueryCalendar id="ucjQueryCalendar" runat="server"></UC:jQueryCalendar>
    
    <script language="javascript" type="text/javascript">
        var _QuotationID = <%= _QuotationID.ToString()%>; 
        var _ProjectID = <%= _ProjectID.ToString()%>;      
        var _NewProjectID = 0;
        
        function MoveNext(stepIndex) {
            for (i = 1; i < 4; i++) {
                document.getElementById('divStep' + i).style.display = 'none';
            }
            document.getElementById('divStep' + stepIndex).style.display = 'block';
        }
        ///Quotations 
        function ValidateAndMoveNext(validationGroup, stepID)
        {            
            if (Page_ClientValidate(validationGroup))
                MoveNext(stepID);
        }
        
        var _Project = null;
        
        function PrepareProjectObject()
        {
            _Project = new App.CustomModels.CustomProject();
            _Project.ID = _ProjectID;
            _Project.QuotationID = _QuotationID;
            _Project.Name = $('#<%=txtName.ClientID %>').val();
            _Project.Description = $('#<%=txtDescription.ClientID %>').val();
            _Project.StartDate = $('#<%=txtStartDate.ClientID %>').val();
            _Project.EndDate = $('#<%=txtEndDate.ClientID %>').val();
        }
        function SaveProject()
        {
            if(Page_ClientValidate('SaveInfo2'))
            {
                ShowProgress();
                PrepareProjectObject();
                AjaxService.SaveProject(_Project, OnSaveSuccess, OnAjax_Error, OnAjax_TimeOut);
            }
        }
        function OnSaveSuccess(result)
        {
            HideProgress();
            _NewProjectID = result.split(':')[0];
            var number = result.split(':')[1];                     
            $('#divFinalMessage').html('Project <b>' + number + '</b> has been successfully created.');
            MoveNext(3);
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="PageHeaderContentPlaceHolder" Runat="Server">
    <asp:Literal ID="ltrHeading" runat="server" Text="Create Project Wizard"></asp:Literal>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="SideBarPlaceHolder" Runat="Server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="PageContentPlaceHolder" Runat="Server">
    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
        <Services>
            <asp:ServiceReference Path="~/Services/AjaxService.asmx" />
        </Services>
    </asp:ScriptManagerProxy>
    <div id="divMessage" runat="server" enableviewstate="false" visible="false"></div>
    
    <%--<asp:HiddenField ID="hdnQuotationPricings" runat="server" />--%>
    
    <asp:Panel ID="pnlDetails" runat="server">
    
        <div id="divStep1" class="GroupBox" style="display:block;">
            <div class="FormHeader">
                <b>Create Project</b><br />
                <asp:Label ID="lblStep1Title" Text="Specify the Name of the Project." runat="server"></asp:Label>
            </div>
            <table cellpadding="3" cellspacing="0" class="FormTable">
                <colgroup>
                    <col style="width:15%;" />
                    <col/>
                </colgroup>		
				<tr>
				    <td>Project Name</td>					
					<td>
					    <asp:TextBox id="txtName" runat="server" MaxLength="200" style="width:100%;"></asp:TextBox>
					    <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" 
                            ControlToValidate="txtName" SetFocusOnError="true" 
                            ErrorMessage="<br/>Please Enter a Project Name." Display="Dynamic"
                            ValidationGroup="SaveInfo1">
                        </asp:RequiredFieldValidator>                         
					</td>
				</tr>								                
            </table> 
            <div style="margin-top:70px;">                
                <input type="button" value="Next >" onclick="ValidateAndMoveNext('SaveInfo1', 2);" />
            </div>      
        </div>
        
        <%--Step 2--%>
        <div id="divStep2" class="GroupBox" style="display:none; min-height:120px;">
            <div class="FormHeader">
                <b>Project Details</b><br />
                <asp:Label ID="Label1" Text="Specify the Details Information for this Project." runat="server"></asp:Label>
            </div>
            <table cellpadding="3" cellspacing="0" class="FormTable">
                <colgroup>
                    <col style="width:15%;" />
                    <col/>
                </colgroup>		
				<tr>
					<td>Start Date</td>
					<td>
						<asp:TextBox ID="txtStartDate" MaxLength="50" CssClass="CalendarTextBox" runat="server"></asp:TextBox>
						<asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                            ControlToValidate="txtStartDate" SetFocusOnError="true" 
                            ErrorMessage="<br/>Please Select Project Start Date." Display="Dynamic"
                            ValidationGroup="SaveInfo2">
                        </asp:RequiredFieldValidator>                        
                        <asp:CustomValidator ID="CustomValidator1" runat="server" 
                            ControlToValidate="txtStartDate" SetFocusOnError="true" 
                            Display="Dynamic" ClientValidationFunction="ValidateDate"
                            ErrorMessage="<br/>Enter a Valid Date."
                            ValidationGroup="SaveInfo2">
                        </asp:CustomValidator>
					</td>
				</tr>				
				<tr>
					<td>End Date</td>
					<td>
						<asp:TextBox ID="txtEndDate" MaxLength="50" CssClass="CalendarTextBox" runat="server"></asp:TextBox>
						<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                            ControlToValidate="txtEndDate" SetFocusOnError="true" 
                            ErrorMessage="<br/>Please Select Project End Date." Display="Dynamic"
                            ValidationGroup="SaveInfo2">
                        </asp:RequiredFieldValidator>                        
                        <asp:CustomValidator ID="CustomValidator3" runat="server" 
                            ControlToValidate="txtEndDate" SetFocusOnError="true" 
                            Display="Dynamic" ClientValidationFunction="ValidateDate"
                            ErrorMessage="<br/>Enter a Valid Date."
                            ValidationGroup="SaveInfo2">
                        </asp:CustomValidator>
					</td>
				</tr>
				<tr>
				    <td>Description</td>					
					<td>
					    <asp:TextBox id="txtDescription" TextMode="MultiLine" runat="server" MaxLength="500" style="height:120px;"></asp:TextBox>
					    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                            ControlToValidate="txtDescription" SetFocusOnError="true" 
                            ErrorMessage="Please Enter a Project Description." Display="Dynamic"
                            ValidationGroup="SaveInfo2">
                        </asp:RequiredFieldValidator>                         
					</td>
				</tr>				                
            </table> 
            <div style="margin-top:15px;">
                <input type="button" value="< Back" onclick="MoveNext(1);" />&nbsp;
                <input type="button" value="Finish" onclick="SaveProject();" />
            </div>
        </div>
        
        <%--Step 3--%>
        <div id="divStep3" class="GroupBox" style="display:none; min-height:120px;">
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

