using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using App.Core.Extensions;

public partial class Pages_ProjectNoteDetails : BasePage
{
    private long _NoteID = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        BindPageInfo();
        if (!IsPostBack)
        {
            BindNoteDetails();
        }
    }
    protected void BindPageInfo()
    {
        _NoteID = WebUtil.GetQueryStringInLong(AppConstants.QueryString.ID);
        Page.Title = WebUtil.GetPageTitle("Note Details");
    }
    protected void BindNoteDetails()
    {
        OMMDataContext context = new OMMDataContext();
        ProjectNote note = context.ProjectNotes.SingleOrDefault(P => P.ID == _NoteID);
        if (note == null)
            ShowErrorMessage();
        else
        {
            ltrProjectName.Text = String.Format("<b>Project Number: </b>{0}<br /><b>Project Name: </b>{1}", note.Project.Number, note.Project.Name.HtmlEncode());
            User user = context.Users.SingleOrDefault(U => U.ID == note.CreatedBy);            
            ltrUserName.Text = user == null ? "Annonymus" : user.UserNameWeb;            
            ltrUserName.Text = String.Format("{0}<div class='NoteDate'>{1}</div>", ltrUserName.Text, note.CreatedDate.ToString(AppConstants.ValueOf.DATE_FROMAT_DISPLAY_WITH_TIME));

            ltrDetails.Text = WebUtil.FormatText(note.Details);
        }
    }
    protected void ShowErrorMessage()
    {
        WebUtil.ShowMessageBox(divMessage, "Requested Project Note was not found.", true);
        pnlDetailsContainer.Visible = false;
    }
}
