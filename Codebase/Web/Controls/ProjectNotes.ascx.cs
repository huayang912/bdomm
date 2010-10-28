using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using App.Core.Extensions;

public partial class Controls_ProjectNotes : System.Web.UI.UserControl
{
    OMMDataContext _DataContex; 

    #region Public Properties
    public int ProjectID { get; set; }
    
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        hdnUserName.Value = SessionCache.CurrentUser.UserNameWeb;
    }
    public override void DataBind()
    {
        _DataContex = new OMMDataContext();
        IList<ProjectNote> notes = (from P in _DataContex.ProjectNotes where P.ProjectID == this.ProjectID orderby P.CreatedDate select P).ToList();
        rptProjectNotesList.DataSource = notes;
        rptProjectNotesList.DataBind();
    }
    protected void rptProjectNotesList_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
        {
            ProjectNote note = e.Item.DataItem as ProjectNote;
            User user = _DataContex.Users.SingleOrDefault(U => U.ID == note.CreatedBy);
            Literal ltrUserName = e.Item.FindControl("ltrUserName") as Literal;
            ltrUserName.Text = user == null ? "Annonymus" : user.UserNameWeb;
            //String noteDate = note.CreatedDate.ToString(AppConstants.ValueOf.DATE_FROMAT_DISPLAY);
            ltrUserName.Text = String.Format("{0}<div class='NoteDate'>{1}</div>", ltrUserName.Text, note.CreatedDate.GetDifference());

            Literal ltrDetails = e.Item.FindControl("ltrDetails") as Literal;
            ltrDetails.Text = WebUtil.FormatText(note.Details);
        }
    }
}
