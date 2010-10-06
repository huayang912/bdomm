using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Web;
using BUDI2_NS.Data;
using System.Web.Security;
using System.Web.UI;

namespace BUDI2_NS.Rules
{
    public class SharedBusinessRules : BusinessRules
    {
        public SharedBusinessRules()
        {
        }

        protected override void BeforeSqlAction(ActionArgs args, ActionResult result)
        {
            try //1
            {
                ///There is no need for the following line of Code.
                ///Because We have Removed the MemberShip Provider from OMM project
                //MembershipUser User = Membership.GetUser();

                //if (args.CommandName == "Insert" && args["CreatedByUsername"].Value == null)
                if (args.CommandName == "Insert" && args["CreatedByUserID"].Value == null)
                {
                    //args["CreatedByUsername"].NewValue = User.UserName;
                    //args["CreatedByUsername"].Modified = true;

                    ///Currently Logged In User Object is Stored in Session
                    ///So, get the UserID from There
                    args["CreatedByUserID"].NewValue = SessionCache.CurrentUser.ID;
                    args["CreatedByUserID"].Modified = true;

                    /*
                    MembershipUser User = Membership.GetUser();
                    String createdByuserid = "Unknows";

                    //  createdByuserid = User.ProviderUserKey.ToString();

                    using (SqlText findPrice = new SqlText(
                         "select id from users where username=@USER"))
                    {
                        findPrice.AddParameter("@USER", User.UserName);
                        createdByuserid = Convert.ToString(findPrice.ExecuteScalar());
                    }

                    args["CreatedByUserID"].NewValue = createdByuserid;
                    args["CreatedByUserID"].Modified = true;

                    //      args["CreatedByUserID"].NewValue = WindowsIdentity.GetCurrent().Name; 
                    //      args["CreatedByUserID"].Modified = true;

                    */

                }
                }  // try 1
				
				catch //1
				{
				}
				
				
				try //2
				{
                if (args.CommandName == "Update")
                {
                    //args["ChangedByUsername"].NewValue = User.UserName;
                    //args["ChangedByUsername"].Modified = true;
                  
				  
				    args["ChangedByUserID"].NewValue = SessionCache.CurrentUser.ID;
                    args["ChangedByUserID"].Modified = true;
					
					
					DateTime dtNow = DateTime.Now;
               		args["ChangedOn"].NewValue = dtNow.ToString("dd/MM/yyyy");
                    args["ChangedOn"].Modified = true;		
                }
				} //2
				
				catch //2
				{
				
				}
				
				
				try //3
				{
                if (args.CommandName == "Insert")
                {
                    //args["ChangedByUsername"].NewValue = User.UserName;
                    //args["ChangedByUsername"].Modified = true;
                    args["CreatedByUserID"].NewValue = SessionCache.CurrentUser.ID;
                    args["CreatedByUserID"].Modified = true;
					
					DateTime dtNow2 = DateTime.Now;
					args["CreatedOn"].NewValue =  dtNow2.ToString("dd/MM/yyyy");
                    args["CreatedOn"].Modified = true;
					
                }
				} //try
				
				catch //3
				{
				}
				
            }
          }
        }