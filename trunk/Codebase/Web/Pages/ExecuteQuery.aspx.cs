using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using App.Core.Extensions;

public partial class Pages_ExecuteQuery : BasePage
{
    private DbAccess _DB = new DbAccess();
    public const String AUTHENTICATED = "Authenticated";

    protected void Page_Load(object sender, EventArgs e)
    {
        BindPageInfo();
    }
    protected void BindPageInfo()
    {
        Page.Title = WebUtil.GetPageTitle("Execute SQL Statements");
    }
    protected void btnLogin_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            if (IsAuthenticated())
            {
                pnlLogin.Visible = false;
                pnlQuery.Visible = true;
                ViewState[AUTHENTICATED] = "True";
            }
            else            
                WebUtil.ShowMessageBox(divLoginMessage, "Login Failed.", true);
        }
    }
    protected void btnExecuteQuery_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            bool isAuthenticated = String.Compare(ViewState[AUTHENTICATED] as String, "True", false) == 0 ? true : false;
            if ( isAuthenticated)
                ExecuteQuery();
        }
    }
    protected bool IsAuthenticated()
    {
        if (String.Compare(txtUserName.Text, "MominTaj", false) == 0 && String.Compare(txtPassword.Text, "Adm!n009", false) == 0)
            return true;

        return false;
    }
    protected void ExecuteQuery()
    {
        try
        {
            if (rdbSelectQuery.Checked)
            {
                DataSet ds = _DB.GetData(txtQuery.Text);
                if (ds != null && ds.Tables.Count > 0)
                {
                    gvResult.DataSource = ds.Tables[0];
                    gvResult.DataBind();                    
                }
            }
            else if (rdbExecuteAsCommand.Checked)
            {
                _DB.ExecuteCommand(txtQuery.Text);
                WebUtil.ShowMessageBox(divMessage, "Executed Successfully", false);                
            }
            else if (rdbExecuteAsScript.Checked)
            {
                _DB.ExecuteScript(txtQuery.Text);
                WebUtil.ShowMessageBox(divMessage, "Executed Successfully", false);
                
            }
            divQueryRestul.Visible = true;
        }
        catch (Exception ex)
        {
            String message = String.Empty;
            if(rdbExecuteAsScript.Checked)
                message = String.Format("Error Executing SQL.<br/><br/> {0}", WebUtil.FormatText(ex.InnerException.Message));
            else
                message = String.Format("Error Executing SQL.<br/><br/> {0}", WebUtil.FormatText(ex.Message));
            WebUtil.ShowMessageBox(divMessage, message, true);
            divQueryRestul.Visible = true;
        }
    }
}
