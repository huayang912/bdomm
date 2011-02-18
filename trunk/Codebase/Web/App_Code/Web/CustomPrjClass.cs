


using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using BUDI2_NS.Data;
using System.Collections.Generic;
//using System.Security.Principal;


/// <summary>
/// Summary description for CustomClass
/// </summary>
public class CustomPrjClass:ActionHandlerBase
{
    public CustomPrjClass()
	{
		//
		// TODO: Add constructor logic here
		//


	}

    protected override void BeforeSqlAction(ActionArgs args, ActionResult result)
    {
        if (args.CommandName == "Insert" && args["Number"].Value == null)
        {
            String Quote_no = null;
            using (SqlText findPrice = new SqlText(
                "select dbo.GenerateNewProjectNumber() as enq_no"))
            {
                findPrice.AddParameter("@EnqID", 1); // Ignore this line
                Quote_no = Convert.ToString(findPrice.ExecuteScalar());
                
            }
            args["Number"].NewValue = Quote_no;
            args["Number"].Modified = true;

      //      args["CreatedByUserID"].NewValue = WindowsIdentity.GetCurrent().Name; 
      //      args["CreatedByUserID"].Modified = true;
            
        }

     //   if (args["CreatedByUsername"].Value == null)

        if (args.CommandName == "Insert" && args["CreatedByUsername"].Value == null)

        {
            MembershipUser User = Membership.GetUser();
            args["CreatedByUsername"].NewValue = User.UserName;
            args["CreatedByUsername"].Modified = true;

            //     args["CreatedByUserID"].NewValue = WindowsIdentity.GetCurrent().Name; 
            //      args["CreatedByUserID"].Modified = true;
 
        }

           
    }
	
	//
		protected override void ExecuteAction(ActionArgs args, ActionResult result)
	{
		// Added for project page oes not go well with page name
		   if (args.CommandName == "Custom" && args.CommandArgument == "SetProjectStatusClosed")
        {
		
		 using (SqlText updatePrice = new SqlText(
                "update [Projects] set StatusID=@StatusID where ID=@ID"))
             {
                 updatePrice.AddParameter("@StatusID", 2);
                 updatePrice.AddParameter("@ID", args["ID"].Value);
                 updatePrice.ExecuteNonQuery();

                }
		
		
            result.ClientScript = "this.set_lastCommandName(null);" + "this.goToView('grid1');";
            Result.ShowAlert("Project Closed.");

        }
		
		
	}

	
	
	//
	
	
	
	
}
