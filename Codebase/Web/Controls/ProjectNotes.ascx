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
</style>
<script language="javascript" type="text/javascript">
    var _Note = null;
    var _NoteWordCount = <%= _NoteWordCount.ToString() %>;
    
    function GetNoteHtml() {
        var noteDetails = GetPrunnedWords(_Note.Details, _Note.ID);                  
        var rowStyle = $('#tblProjectList tr').length % 2 == 0 ? 'EvenRowListing' : 'OddRowListing';
        var html = '<tr class="' + rowStyle + '">';
        html += '   <td>' + $('#<%= hdnUserName.ClientID%>').val() + '<div class="NoteDate">Now</div></td>';
        html += '   <td>' + noteDetails + '</td>';
        html += '   <td>&nbsp;</td>';
        html += '</tr>'; 
        return html;       
    }
    function GetPrunnedWords(text, id)
    {
        var detailsLink = '<div style="margin-top:5px;"><a href="javascript:void(0);" onclick="ShowCenteredPopUp(\'<%=AppConstants.Pages.PROJECT_NOTE_DETAILS %>?<%=AppConstants.QueryString.ID %>=' + id + '\', \'NoteDetails\', 650, 580, true)">More..</a></div>';
        var noteDetails = FormatText(GetWords(text, _NoteWordCount));
        //alert('wjklj count: ' + GetWordCount(text));
        if(GetWordCount(text) > _NoteWordCount)
            noteDetails += detailsLink;  
        return noteDetails;
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
            _Note.ID = result;
            $('#tblProjectList tr:last').after(GetNoteHtml());
            $('#<%= txtDetails.ClientID %>').val('');
        }
    }
    function ValidateDetailsTextLength(sender, args)
    {
        if(args.Value.length <= 8000)
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
    <asp:TextBox ID="txtDetails" runat="server" TextMode="MultiLine" MaxLength="8000" style="width:700px; height:150px;"></asp:TextBox>
    <asp:RequiredFieldValidator ID="rfvDetails" runat="server"
        ControlToValidate="txtDetails" SetFocusOnError="true" Display="Dynamic"
        ValidationGroup="SaveNote"
        ErrorMessage="<br/>Please Enter a Note for this Project.">
    </asp:RequiredFieldValidator>
    <asp:CustomValidator ID="cvDetails" runat="server"
        ControlToValidate="txtDetails" SetFocusOnError="true" Display="Dynamic"
        ValidationGroup="SaveNote" ClientValidationFunction="ValidateDetailsTextLength"
        ErrorMessage="<br />Sorry! Note Details Cannot be more than 8000 Characters long.">
    </asp:CustomValidator>
</div>
<div style="margin-top:5px;">
    <input type="button" value="Save Note" onclick="SaveProjectNote();" />
</div>