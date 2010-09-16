


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
public class CustomQuoteClass:ActionHandlerBase
{
    public CustomQuoteClass()
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
                "select dbo.GenerateNewQuotationNumber (@EnqID,1) as enq_no"))
            {
                findPrice.AddParameter("@EnqID", args["EnquiryID"].Value);
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
	
		protected override void ExecuteAction(ActionArgs args, ActionResult result)
	{
		if (args.CommandName == "Custom" && args.CommandArgument == "SetQuoteStatusClosed")
		 {
        //     args["NewStatusID"].NewValue = "OK";
         //    args["NewStatusID"].Modified = true;

        //    args["MainEquipment"].NewValue = "TRYME";
        //    args["MainEquipment"].Modified = true;

            // Result.ShowAlert("Quote closed." + " O-" + args["NewStatusID"].OldValue + " N-" + args["NewStatusID"].NewValue + " V-" + args["NewStatusID"].Value + " ID-" + args["ID"].Value);

             using (SqlText updatePrice = new SqlText(
                "update [Quotations] set StatusID=@StatusID where ID=@ID"))
             {
                 updatePrice.AddParameter("@StatusID", 5);
                 updatePrice.AddParameter("@ID", args["ID"].Value);
              //   updatePrice.ExecuteNonQuery();

                }



             Result.ShowAlert("Quote closed.");
        }

        if (args.CommandName == "Custom" && args.CommandArgument == "SetQuoteStatusSuccess")
        {         
            Result.ShowAlert("Quote Successful.");
        }

        if (args.CommandName == "Custom" && args.CommandArgument == "SetQuoteStatusRevision")
        {
            Result.ShowAlert("Quote Revised.");
        }
	}

}
