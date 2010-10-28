<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ProjectNotes.ascx.cs" Inherits="Controls_ProjectNotes" %>
<style type="text/css">    
    .NotesTable
    {
        border-collapse:collapse;
        width:100%;
    }
    .NotesTable td
    {
        height:50px;       
    }
    .NoteDate
    {
        color:#8695aa;
    }    
</style>
<script language="javascript" type="text/javascript">
    var _Note = null;
    
    function GetNoteHtml() {
        var rowStyle = $('#tblProjectList tr').length % 2 == 0 ? 'EvenRowListing' : 'OddRowListing';
        var html = '<tr class="' + rowStyle + '">';
        html += '   <td>' + $('#<%= hdnUserName.ClientID%>').val() + '<div class="NoteDate">Now</div></td>';
        html += '   <td>' + FormatText(_Note.Details) + '</td>';
        html += '   <td>&nbsp;</td>';
        html += '</tr>'; 
        return html;       
    }
    function PrepareNoteObject() {
        _Note = new App.CustomModels.CustomProjectNote();
        _Note.ID = 0;
        _Note.ProjectID = <%= this.ProjectID %>;
        _Note.Details = $('#<%= txtDetails.ClientID %>').val();
    }
    function SaveProjectNote()
    {
        if(Page_ClientValidate('SaveNote'))
        {
            ShowProgress();
            PrepareNoteObject();
            AjaxService.SaveProjectNote(_Note, OnSaveProjectSuccess, OnAjax_Error, OnAjax_TimeOut);
        }
    }
    function OnSaveProjectSuccess(result)
    {
        HideProgress();
        if(result > 0)
        {
            $('#tblProjectList tr:last').after(GetNoteHtml());
            $('#<%= txtDetails.ClientID %>').val('');
        }
    }
    function ValidateDetailsTextLength(sender, args)
    {
        if(args.Value.length <= 4000)
            args.IsValid = true;
        else
            args.IsValid = false;
    }
</script>

<asp:HiddenField ID="hdnUserName" runat="server" />

<table id="tblProjectList" class="NotesTable" cellpadding="5" cellspacing="0">
    <colgroup>
        <col style="width:200px;" />
        <col /> 
        <col style="width:100px;" />       
    </colgroup>
    <%--Requires following tr to insert tr by JS in case of blank table --%>
    <tr style="display:none;">
        <td colspan="3"></td>
    </tr>
    <asp:Repeater ID="rptProjectNotesList" runat="server" OnItemDataBound="rptProjectNotesList_ItemDataBound">        
        <ItemTemplate>
            <tr class="OddRowListing">
                <td><asp:Literal ID="ltrUserName" runat="server"></asp:Literal></td>
                <td><asp:Literal ID="ltrDetails" runat="server"></asp:Literal></td>
                <td>&nbsp;</td>
            </tr>
        </ItemTemplate>
        <AlternatingItemTemplate>
            <tr class="EvenRowListing">
                <td><asp:Literal ID="ltrUserName" runat="server"></asp:Literal></td>
                <td><asp:Literal ID="ltrDetails" runat="server"></asp:Literal></td>
                <td>&nbsp;</td>
            </tr>
        </AlternatingItemTemplate>        
    </asp:Repeater>
</table>
<div>
    <div style="margin:10px 0px 2px 0px;">Add a Note for this Project</div>
    <asp:TextBox ID="txtDetails" runat="server" TextMode="MultiLine" MaxLength="1000" style="width:500px; height:100px;"></asp:TextBox>
    <asp:RequiredFieldValidator ID="rfvDetails" runat="server"
        ControlToValidate="txtDetails" SetFocusOnError="true" Display="Dynamic"
        ValidationGroup="SaveNote"
        ErrorMessage="<br/>Please Enter a Note for this Project.">
    </asp:RequiredFieldValidator>
    <asp:CustomValidator ID="cvDetails" runat="server"
        ControlToValidate="txtDetails" SetFocusOnError="true" Display="Dynamic"
        ValidationGroup="SaveNote" ClientValidationFunction="ValidateDetailsTextLength"
        ErrorMessage="<br />Sorry! Note Details Cannot be more than 4000 Characters long.">
    </asp:CustomValidator>
</div>
<div>
    <input type="button" value="Save Note" onclick="SaveProjectNote();" />
</div>