using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Web.Security;
using System.Configuration;
using System.Web.UI.HtmlControls;
using System.Net.Mail;

using System.Web.Services;
using System.Data;


using System.Web.UI.WebControls.WebParts;

using BUDI2_NS.Data;

using System.Collections.Generic;


public partial class Controls_send_mail_control : System.Web.UI.UserControl
{
        private OMMDataContext _DataContext = new OMMDataContext();
		
		
    protected void Page_Load(object sender, EventArgs e)
    {
    }
	
		    #region  "Send email"
    protected void btnSendmail_Click(object sender, EventArgs e)
    {
        // System.Web.Mail.SmtpMail.SmtpServer is obsolete in 2.0
        // System.Net.Mail.SmtpClient is the alternate class for this in 2.0
		
       SmtpClient smtpClient = new SmtpClient();
		
        MailMessage message = new MailMessage();

        try
        {
		
	
          MailAddress fromAddress = new MailAddress(txtEmail.Text, txtName.Text);

            // You can specify the host name or ipaddress of your server
            // Default in IIS will be localhost 
			
			/*
            smtpClient.Host = "localhost";

            //Default port will be 25
            smtpClient.Port = 25;
			*/
			
			
			   smtpClient.Host = "smtp.gmail.com";   // We use gmail as our smtp client
        smtpClient.Port = 587;
        smtpClient.EnableSsl = true;
        smtpClient.UseDefaultCredentials = true;
        smtpClient.Credentials = new System.Net.NetworkCredential("momintaj@gmail.com", "casinova");

			
			
			

            //From address will be given as a MailAddress Object
            message.From = fromAddress;

            // To address collection of MailAddress
            message.To.Add("momin@emrex.com");
            message.Subject = "BUDI - Feedback";

            // CC and BCC optional
            // MailAddressCollection class is used to send the email to various users
            // You can specify Address as new MailAddress("admin1@yoursite.com")
       //     message.CC.Add("admin1@yoursite.com");
       //    message.CC.Add("admin2@yoursite.com");

            // You can specify Address directly as string
     //     message.Bcc.Add(new MailAddress("admin3@yoursite.com"));
     //    message.Bcc.Add(new MailAddress("admin4@yoursite.com"));

            //Body can be Html or text format
            //Specify true if it  is html message
            message.IsBodyHtml = false;

            // Message body content
			
            message.Body = "From:" + "  - " + txtMessage.Text;
         
            // Send SMTP mail
            smtpClient.Send(message);

            lblStatus.Text = "Email successfully sent.";
			
			
			
			
        }
        catch (Exception ex)
        {
            lblStatus.Text = "Send Email Failed.<br>" + ex.Message;
        }
    }
    #endregion

    #region "Reset"
    protected void btnReset_Click(object sender, EventArgs e)
    {
        txtName.Text = "";
        txtMessage.Text = "";
        txtEmail.Text = "";
    }
    #endregion
	
	
	
}
