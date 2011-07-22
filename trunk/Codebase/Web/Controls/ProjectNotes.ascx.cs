using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using App.Core.Extensions;

public partial class Controls_ProjectNotes : System.Web.UI.UserControl
{
    private OMMDataContext _DataContext;
    protected int _NoteWordCount = ConfigReader.ProjectNoteWordCount;

    #region Public Properties
    public int ProjectID { get; set; }
    
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        hdnUserName.Value = SessionCache.CurrentUser.UserName; //SessionCache.CurrentUser.UserNameWeb;
    }
    public override void DataBind()
    {
        _DataContext = new OMMDataContext();
        IList<ProjectNote> notes = (from P in _DataContext.ProjectNotes where P.ProjectID == this.ProjectID orderby P.CreatedDate select P).ToList();
        rptProjectNotesList.DataSource = notes;
        rptProjectNotesList.DataBind();
    }
    protected void rptProjectNotesList_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
        {
            ProjectNote note = e.Item.DataItem as ProjectNote;
            User user = _DataContext.Users.SingleOrDefault(U => U.ID == note.CreatedBy);
            Literal ltrUserName = e.Item.FindControl("ltrUserName") as Literal;
            ltrUserName.Text = user == null ? "Annonymus" : user.UserName; //user.UserNameWeb;
            //String noteDate = note.CreatedDate.ToString(AppConstants.ValueOf.DATE_FROMAT_DISPLAY);
            ltrUserName.Text = String.Format("{0}<div class='NoteDate'>{1}</div>", ltrUserName.Text, note.CreatedDate.ToString(AppConstants.ValueOf.DATE_FROMAT_DISPLAY_WITH_TIME));

            Literal ltrDetails = e.Item.FindControl("ltrDetails") as Literal;
            ltrDetails.Text = FormatNoteText(note.Details, note.ID);
        }
    }
    protected String FormatNoteText(String text, long noteID)
    {
        bool hasPrunnedText = false;
        if (text.WordCount() > _NoteWordCount)
        {
            text = text.GetWords(_NoteWordCount);
            hasPrunnedText = true;
        }
        text = WebUtil.FormatText(text);
        if (hasPrunnedText)
            return String.Format("{0}<div style='margin-top:5px;'><a href='javascript:void(0);' onclick=\"ShowCenteredPopUp('{1}?{2}={3}', 'NoteDetails', 650, 580, true)\">More..</a></div>", text, 
                AppConstants.Pages.PROJECT_NOTE_DETAILS, 
                AppConstants.QueryString.ID, noteID);
        return text;
    }
}
