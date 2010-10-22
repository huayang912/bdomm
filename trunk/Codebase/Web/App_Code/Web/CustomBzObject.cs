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

            ///There is no need for the following line of Code.
            ///Because We have Removed the MemberShip Provider from OMM project
            //MembershipUser User = Membership.GetUser();

            //if (args.CommandName == "Insert" && args["CreatedByUsername"].Value == null)

            try
            {

                if (args.CommandName == "Update")
                {
                    //args["ChangedByUsername"].NewValue = User.UserName;
                    //args["ChangedByUsername"].Modified = true;

                    try //2
                    {
                        args["ChangedByUserID"].NewValue = SessionCache.CurrentUser.ID;
                        args["ChangedByUserID"].Modified = true;

                    }

                    catch //2
                    {
                        //throw;
                    }

                    // ID speel different ;)

                    try //2
                    {
                        args["ChangedByUserId"].NewValue = SessionCache.CurrentUser.ID;
                        args["ChangedByUserId"].Modified = true;
                    }

                    catch //2
                    {
                        //  throw;
                    }



                    try //2
                    {
                        DateTime dtNow = DateTime.Now;
                        args["ChangedOn"].NewValue = dtNow.ToString("dd/MM/yyyy");
                        args["ChangedOn"].Modified = true;
                    }
                    catch //2
                    {

                    }
                } //2

            


            if (args.CommandName == "Insert" )
            {

                try //3
                {
                    //args["ChangedByUsername"].NewValue = User.UserName;
                    //args["ChangedByUsername"].Modified = true;
                    args["CreatedByUserID"].NewValue = SessionCache.CurrentUser.ID;
                    args["CreatedByUserID"].Modified = true;
                }
                catch //3
                {
                }
                //

                try //2
                {
                    args["ChangedByUserID"].NewValue = SessionCache.CurrentUser.ID;
                    args["ChangedByUserID"].Modified = true;

                }

                catch //2
                {
                    //throw;
                }
                //

                try //2
                {
                    args["ChangedByUserId"].NewValue = SessionCache.CurrentUser.ID;
                    args["ChangedByUserId"].Modified = true;
                }

                catch //2
                {
                    //  throw;
                }



              
                try //3
                {
                    DateTime dtNow2 = DateTime.Now;
                    args["CreatedOn"].NewValue = dtNow2.ToString("dd/MM/yyyy");
                    args["CreatedOn"].Modified = true;

                }
                catch //3
                {
                }

            } //try

            } //try
            catch //3
            {
            }

        }	
				
        }
         
        }