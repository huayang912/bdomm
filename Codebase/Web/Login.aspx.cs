using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;



public partial class Login : BasePage
{
    
    //public string CssClass
    //{
    //    get
    //    {
    //        return "HomePage Wide Tall";
    //    }
    //}
    private OMMDataContext _DataContext = new OMMDataContext();

    protected void Page_Load(object sender, EventArgs e)
    {
        Page.Title = WebUtil.GetPageTitle("User Log In");
        if (!IsPostBack)
        {
            if (String.Compare(Request[AppConstants.QueryString.LOG_OUT], "True", true) == 0)
            {
                base.SignoutUser();
            }
            //else if (SessionCache.CurrentUser != null)
            //{
            //    Response.Redirect(AppConstants.Pages.PERFORMENCE_TABLE, false);
            //    return;
            //}
            txtUserName.Focus();
        }
    }
    protected void btnLogin_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            if (Page.IsValid)
            {
                if (SessionCache.FailedLoginAttemptCount >= 4)
                {
                    SessionCache.NotificationMessage = "Repeated multiple attempts have been made trying to access this site.";
                    Response.Redirect(AppConstants.Pages.SHOW_MESSAGE, false);
                    return;
                }
                String userName = txtUserName.Text;
                String password = txtUserPassword.Text;
                //userName = ReplaceBadWords(userName);
                var user = _DataContext.Users.SingleOrDefault(P => P.UserNameWeb == userName && P.Password == password);                
                LoginUser(user, userName, chkRememberMe.Checked);
            }
        }
    }

    /// <summary>
    /// Logs In a User to the web site and redirects to the desired page.
    /// </summary>
    /// <param name="user"></param>
    /// <param name="userName"></param>
    /// <param name="rememberMe"></param>
    protected void LoginUser(User user, String userName, bool rememberMe)
    {
        if (user == null) //Login Failed
        {
            //SessionCache.AttemptedUserName = userName;
            SessionCache.FailedLoginAttemptCount = SessionCache.FailedLoginAttemptCount + 1;
            WebUtil.ShowMessageBox(divMessage, "Login Failed. Your login was unsuccessful. Check your User Name, Password and try again.", true);
        }
        else
        {
            //if (String.Compare(user.IsActive, "A", false) != 0)
            //{
            //    String message = @"Your login information is valid, however your account has been disabled.";                
            //    WebUtil.ShowMessageBox(divMessage, message, true);
            //    SessionCache.FailedLoginAttemptCount = SessionCache.FailedLoginAttemptCount + 1;                
            //    return;
            //}
            SessionCache.CurrentUser = user;

            ///After Successfull Login Redirect to the Requested Page
            //System.Web.Security.FormsAuthentication.RedirectFromLoginPage(user.UserLogInName, rememberMe);
            FormsAuthenticationUtil.RedirectFromLoginPage(user.UserNameWeb, "Administrator", rememberMe);
        }
    }
    protected void LogOutUser()
    {
        //FormsAuthentication.SignOut();
        //SessionCache.ClearSession();

        //Response.Cookies[AppConstants.Cookie.BASE].Expires = DateTime.Now.AddYears(-100);
    } 
}
