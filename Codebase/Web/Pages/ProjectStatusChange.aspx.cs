using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using App.Core.Extensions;
using System.Web.Services;

public partial class Pages_ProjectStatusChange : BasePage
{
    protected int _ProjectID = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        BindPageInfo();
        if (!IsPostBack)
        {
            BindProjectInfo();
        }
    }
    protected void BindPageInfo()
    {        
        _ProjectID = WebUtil.GetQueryStringInInt(AppConstants.QueryString.ID);        
        Page.Title = ltrHeading.Text;
        this.Master.SelectedTab = SelectedTab.Project;
    }
    protected void BindProjectInfo()
    {
        OMMDataContext context = new OMMDataContext();
        
        Project project = context.Projects.SingleOrDefault(P => P.ID == _ProjectID);
        if (project == null)
        {
            WebUtil.ShowMessageBox(divMessage, "Sorry! Requested Project was not found.", true);
            pnlDetails.Visible = false;
        }
        else
        {
            ltrHeading.Text = String.Format("{0} : Project {1}", ltrHeading.Text, project.Number);
            if (project.StatusID == App.CustomModels.ProjectStatus.Closed)
            {
                WebUtil.ShowMessageBox(divMessage, "Sorry! Closed project's status cannot be changed.", true);
                pnlDetails.Visible = false;
            }
            else
            {
                ltrProjectName.Text = project.Name.HtmlEncode();
                rdblStatuses.DataSource = context.ProjectStatus;
                rdblStatuses.DataTextField = "Name";
                rdblStatuses.DataValueField = "ID";
                rdblStatuses.DataBind();
                rdblStatuses.SetSelectedItem(project.StatusID.GetValueOrDefault().ToString());
            }
        }
            
        /////Bind Default Value for Start Date and End Date
        //if (quotation != null && !_IsEditMode)
        //{
        //    txtStartDate.Text = DateTime.Now.ToString(ConfigReader.CSharpCalendarDateFormat);
        //    txtEndDate.Text = DateTime.Now.AddDays(quotation.ValidityDays).ToString(ConfigReader.CSharpCalendarDateFormat);
        //}
        Page.Title = WebUtil.GetPageTitle(ltrHeading.Text);
    }
    
    [WebMethod]
    public static bool ChangeProjectStatus(int projectID, int newStatusID)
    {
        OMMDataContext context = new OMMDataContext();
        Project project = context.Projects.SingleOrDefault(P => P.ID == projectID);
        if(project != null)
        {
            project.StatusID = newStatusID;
            project.ChangedByUserID = SessionCache.CurrentUser.ID;
            project.ChangedByUsername = SessionCache.CurrentUser.UserNameWeb;
            project.ChangedOn = DateTime.Now;
            context.SubmitChanges();
            return true;
        }
        return false;
    }
}
