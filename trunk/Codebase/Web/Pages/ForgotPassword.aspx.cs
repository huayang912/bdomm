using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Text;
using App.Core.Extensions;

public partial class Pages_ForgotPassword : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    /// <summary>
    /// Returns 1 If Sent Password Successfully
    /// Returns 0 if the Email not sent
    /// Returns -1 if the Email does not exist.
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    [WebMethod]
    public static int SendPassword(String email)
    {
        OMMDataContext context = new OMMDataContext();
        var users = from U in context.Users where U.Email == email select U;
        if (users == null || users.Count() == 0)
            return -1;
        else
        {
            if(SendEmailNotification(users.First()))
                return 1;
        }
        return 0;
    }
    private static bool SendEmailNotification(User user)
    {
        if (user != null)
        {
            String HtmlTemplate = WebUtil.ReadEmailTemplate(AppConstants.EmailTemplate.GENERAL_EMAIL_TEMPLATE);
            StringBuilder sb = new StringBuilder(10);            
            sb.AppendFormat("Dear {0},<br/>", user.DisplayName.HtmlEncode());
            sb.AppendFormat("Below is your {0} User Name and Password.<br/><br/>", WebUtil.GetDomainAddress());
            sb.AppendFormat("<b>User Name:</b> {0}<br/>", user.UserNameWeb.HtmlEncode());
            sb.AppendFormat("<b>Password:</b> {0}<br/><br/>", user.Password.HtmlEncode());

            sb.AppendFormat("Click the link below to login.<br/>");
            sb.AppendFormat("<a href='{0}{1}'>{0}{1}</a>", WebUtil.GetDomainAddress(), "Login.aspx");
            HtmlTemplate = HtmlTemplate.Replace(AppConstants.ETConstants.MESSAGE, sb.ToString());
            String subject = "Your User Name and Password.";
            bool isEmailSent = WebUtil.SendMail(user.Email, String.Empty, String.Empty, ConfigReader.AdminEmail, subject, HtmlTemplate);
            return isEmailSent;
        }
        return false;
    }   
}
