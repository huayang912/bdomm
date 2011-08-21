using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Net;

using System.IO;

using System.Web.Services;

using App.Core.Extensions;





public partial class Pages_NextOfKin_ : BasePage
{
    protected int _ContactID = 0;
    protected int _CurrentUserID = 0;



    public string CssClass
    {
        get
        {
            return "";
        }
    }
    
    protected void Page_Load(object sender, EventArgs e)
    {
        
        if (!IsPostBack)
        {
            Page.Title = WebUtil.GetPageTitle("Next of Kin");
            _ContactID = WebUtil.GetQueryStringInInt("ID");
            _CurrentUserID = SessionCache.CurrentUser.ID;
            loadImmediateFamilyDetails(_ContactID);
            loadNextOfKinDetails(_ContactID);


            Countries(ddlCountry);
        }
        
    }


    private static void InsertBlankOption(DropDownList ddl)
    {
        ddl.Items.Insert(0, new ListItem("-Select-", "0"));
    }

    public static void Countries(DropDownList ddl)
    {
        OMMDataContext context = new OMMDataContext();
        var country = from P in context.Countries orderby P.Name select new { P.ID, P.Name };
        ddl.DataSource = country;
        ddl.DataTextField = "Name";
        ddl.DataValueField = "ID";
        ddl.DataBind();
        InsertBlankOption(ddl);
    }


    public void loadImmediateFamilyDetails(int contactID)
    {
        OMMDataContext context = new OMMDataContext();
        //IList<ContactCV> contactCV = (from P 
        //                                  in context.ContactCVs 
        //                              where P.ContactID == contactID 
        //                              select P).ToList();

        //IList<ContactsNextOfKin> ContactsNextOfKin = (from P
        //                                  in context.ContactsNextOfKins
        //                              where P.ContactID == contactID
        //                              select P).ToList();

        var ContactsNextOfKins = context.ContactsNextOfKins.FirstOrDefault(P => P.ContactID == contactID);

        if (ContactsNextOfKins == null || contactID == 0)
        {
            btnUpload.Text = "Save";
        }
        else
        {
            btnUpload.Text = "Update";

            tbxMotherName.Text = (ContactsNextOfKins.MothersName.IsNullOrEmpty()) ? "" : ContactsNextOfKins.MothersName.ToString();
            tbxFatherName.Text = (ContactsNextOfKins.FathersName.IsNullOrEmpty()) ? "" : ContactsNextOfKins.FathersName.ToString(); 
            tbxChildName.Text = (ContactsNextOfKins.ChildrensNames.IsNullOrEmpty()) ? "" : ContactsNextOfKins.ChildrensNames.ToString();           
            

            //grdsearch.DataSource = ContactsNextOfKin;
            //grdsearch.DataBind();
        }
    }


    public void loadNextOfKinDetails(int contactID)
    {
        grdsearch.DataSource = null;
        grdsearch.DataBind();


        OMMDataContext context = new OMMDataContext();
        IList<NextOfKin> NextOfKin_ = (from P
                                          in context.NextOfKins
                                      where P.ContactID == contactID
                                      select P).ToList();



        if (NextOfKin_ == null || contactID == 0)
        {
            grdsearch.DataSource = NextOfKin_;
            grdsearch.DataBind();

            btnSaveNextKin.Text = "Save";
        }
        else
        {
            btnUpload.Text = "Update";

            //tbxMotherName.Text = (ContactsNextOfKins.MothersName.IsNullOrEmpty()) ? "" : ContactsNextOfKins.MothersName.ToString();
            //tbxFatherName.Text = (ContactsNextOfKins.FathersName.IsNullOrEmpty()) ? "" : ContactsNextOfKins.FathersName.ToString();
            //tbxChildName.Text = (ContactsNextOfKins.ChildrensNames.IsNullOrEmpty()) ? "" : ContactsNextOfKins.ChildrensNames.ToString();


            grdsearch.DataSource = NextOfKin_;
            grdsearch.DataBind();
        }
    }

    protected void btnClean_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            clearControl();
        }
    }

    public void clearControl()
    {
        //SearchCV();
        tbxName.Text = "";
        tbxRelationShip.Text = "";
        tbxAddress.Text = "";
        tbxPostCode.Text = "";
        tbxHomeNumber.Text = "";
        tbxWorkNumber.Text = "";
        tbxMobile.Text = "";
        tbxChangedBy.Text = "";
        tbxChangeOn.Text = "";
        tbxNextOfKinID.Text = "";

        ddlCountry.SelectedValue = "0";

        btnSaveNextKin.Text = "Save";
    }
         

    protected void btnSaveNextKin_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            //Save information in the database
            OMMDataContext context = new OMMDataContext();
            int contactID = WebUtil.GetQueryStringInInt("ID");

            if (btnSaveNextKin.Text == "Save")
            {
                NextOfKin NextKin = new NextOfKin();

                NextKin.ContactID = contactID;
                NextKin.Name = (tbxName.Text.ToString().Trim().IsNullOrEmpty()) ? "" : tbxName.Text.ToString().Trim();
                NextKin.Relationship = (tbxRelationShip.Text.ToString().Trim().IsNullOrEmpty()) ? "" : tbxRelationShip.Text.ToString().Trim();
                NextKin.Address = (tbxAddress.Text.ToString().Trim().IsNullOrEmpty()) ? "" : tbxAddress.Text.ToString().Trim();
                NextKin.Postcode = (tbxPostCode.Text.ToString().Trim().IsNullOrEmpty()) ? "" : tbxPostCode.Text.ToString().Trim();
                if (Convert.ToInt32(ddlCountry.SelectedValue) > 0)
                {
                    NextKin.CountryID = Convert.ToInt32(ddlCountry.SelectedValue);
                }
                NextKin.HomeNumber = (tbxHomeNumber.Text.ToString().Trim().IsNullOrEmpty()) ? "" : tbxHomeNumber.Text.ToString().Trim();
                NextKin.WorkNumber = (tbxWorkNumber.Text.ToString().Trim().IsNullOrEmpty()) ? "" : tbxWorkNumber.Text.ToString().Trim();
                NextKin.MobileNumber = (tbxMobile.Text.ToString().Trim().IsNullOrEmpty()) ? "" : tbxMobile.Text.ToString().Trim();

                NextKin.ChangedByUserID = SessionCache.CurrentUser.ID;
                NextKin.ChangedOn = System.DateTime.Today;

                context.NextOfKins.InsertOnSubmit(NextKin);
                context.SubmitChanges();

                clearControl();
            }
            else
            {
                var NextKin = context.NextOfKins.FirstOrDefault(P => P.ID == Convert.ToInt32(tbxNextOfKinID.Text));


                if (NextKin == null || contactID == 0)
                {

                }
                else
                {
                    NextKin.ContactID = contactID;
                    NextKin.Name = (tbxName.Text.ToString().Trim().IsNullOrEmpty()) ? "" : tbxName.Text.ToString().Trim();
                    NextKin.Relationship = (tbxRelationShip.Text.ToString().Trim().IsNullOrEmpty()) ? "" : tbxRelationShip.Text.ToString().Trim();
                    NextKin.Address = (tbxAddress.Text.ToString().Trim().IsNullOrEmpty()) ? "" : tbxAddress.Text.ToString().Trim();
                    NextKin.Postcode = (tbxPostCode.Text.ToString().Trim().IsNullOrEmpty()) ? "" : tbxPostCode.Text.ToString().Trim();
                    if (Convert.ToInt32(ddlCountry.SelectedValue) > 0)
                    {
                        NextKin.CountryID = Convert.ToInt32(ddlCountry.SelectedValue);
                    }
                    NextKin.HomeNumber = (tbxHomeNumber.Text.ToString().Trim().IsNullOrEmpty()) ? "" : tbxHomeNumber.Text.ToString().Trim();
                    NextKin.WorkNumber = (tbxWorkNumber.Text.ToString().Trim().IsNullOrEmpty()) ? "" : tbxWorkNumber.Text.ToString().Trim();
                    NextKin.MobileNumber = (tbxMobile.Text.ToString().Trim().IsNullOrEmpty()) ? "" : tbxMobile.Text.ToString().Trim();

                    NextKin.ChangedByUserID = SessionCache.CurrentUser.ID;
                    NextKin.ChangedOn = System.DateTime.Today;

                    context.SubmitChanges();
                }
            }


            loadNextOfKinDetails(contactID);
        }
    }


    protected void btnUpload_onclick(object sender, EventArgs e)
    {
        //Save information in the database
        OMMDataContext context = new OMMDataContext();
        int contactID = WebUtil.GetQueryStringInInt("ID");

        if (btnUpload.Text == "Save")
        {
            ContactsNextOfKin conNextKin = new ContactsNextOfKin();

            conNextKin.MothersName = (tbxMotherName.Text.ToString().Trim().IsNullOrEmpty()) ? "" : tbxMotherName.Text.ToString().Trim();
            conNextKin.FathersName = (tbxFatherName.Text.ToString().Trim().IsNullOrEmpty()) ? "" : tbxFatherName.Text.ToString().Trim();
            conNextKin.ChildrensNames = (tbxChildName.Text.ToString().Trim().IsNullOrEmpty()) ? "" : tbxChildName.Text.ToString().Trim();
            conNextKin.ChangedByUserID = SessionCache.CurrentUser.ID;
            conNextKin.ChangedOn = System.DateTime.Today;

            context.ContactsNextOfKins.InsertOnSubmit(conNextKin);
            context.SubmitChanges();
        }
        else
        {
            var ContactsNextOfKins = context.ContactsNextOfKins.FirstOrDefault(P => P.ContactID == contactID);


            if (ContactsNextOfKins == null || contactID == 0)
            {
                
            }
            else
            {
                ContactsNextOfKins.MothersName = (tbxMotherName.Text.ToString().Trim().IsNullOrEmpty()) ? "" : tbxMotherName.Text.ToString().Trim();
                ContactsNextOfKins.FathersName = (tbxFatherName.Text.ToString().Trim().IsNullOrEmpty()) ? "" : tbxFatherName.Text.ToString().Trim();
                ContactsNextOfKins.ChildrensNames = (tbxChildName.Text.ToString().Trim().IsNullOrEmpty()) ? "" : tbxChildName.Text.ToString().Trim();
                ContactsNextOfKins.ChangedByUserID = SessionCache.CurrentUser.ID;
                ContactsNextOfKins.ChangedOn = System.DateTime.Today;

                context.SubmitChanges();
            }
        }


        loadImmediateFamilyDetails(contactID);
    }


    private bool IsValidDocument(HttpPostedFile httpPostedFile)
    {
        String extension = Path.GetExtension(httpPostedFile.FileName);
        if (String.Compare(extension, ".doc", true) == 0)
        {
            return true;
        }
        else if (String.Compare(extension, ".docx", true) == 0)
        {
            return true;
        }
        else if (String.Compare(extension, ".pdf", true) == 0)
        {
            return true;
        }
            
        return false;
    }


    private String saveFile(String uploadDirectory, int detailID)
    {
        //if (!Directory.Exists(uploadDirectory))
        //    Directory.CreateDirectory(uploadDirectory);
        ////String fileName = String.Format("{0}_{1}", SessionCache.CurrentUser.ID, Path.GetFileName(fileEnquiry.FileName));
        //String fileName = detailID.ToString()+"_"+Path.GetFileName(fileUploadCV.FileName);
        //fileUploadCV.SaveAs(Path.Combine(uploadDirectory, fileName));
        //return fileName;

        return "";
    }


    protected String GetRelativeDownloadUrl()
    {
        return Server.MapPath(AppConstants.PERSONNEL_CV_DIRECTORY);
        //if (rdbPersonnelCV.Checked)
            //return String.Format("{0}/{1}", AppConstants.PERSONNEL_CV_DIRECTORY, Path.GetFileName(path));
        //else
        //    return String.Format("{0}/{1}", ConfigReader.CVBankDirectory, Path.GetFileName(path));
    }

    [WebMethod]
    public static bool DeleteCVT(int id, string filePath, int contactID)
    {

        OMMDataContext context = new OMMDataContext();
        var cvDetails = context.ContactCVs.SingleOrDefault(P => P.ID == id);
        
        if (cvDetails != null)
        {
            //First delete CV information from the database
            
            //context.ContactCVs.DeleteOnSubmit(cvDetails);
            //context.SubmitChanges();

            //Now delete the file from the filesystem

            
            //File.Delete(filePath);

        }

        

        return true;
    }

    protected void GridView1_RowDataBound(object sender,
                         GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            LinkButton l = (LinkButton)e.Row.FindControl("LinkButton1");
            l.Attributes.Add("onclick", "javascript:return " +
            "confirm('Sure to Delete This Information?')");
        }
    }

    protected void GridView1_RowCommand(object sender,
                         GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Delete")
        {
            int ID = Convert.ToInt32(e.CommandArgument);

            //Get detail information from the database
            OMMDataContext context = new OMMDataContext();
            var nextOfKinDetails = context.NextOfKins.SingleOrDefault(P => P.ID == ID);

            if (nextOfKinDetails != null)
            {
                //Delete Next of Kin information from the database
                context.NextOfKins.DeleteOnSubmit(nextOfKinDetails);
                context.SubmitChanges();

                
                //Now reload the list
                loadNextOfKinDetails(WebUtil.GetQueryStringInInt("ID"));
            }
        }

        if (e.CommandName == "Edit")
        {
            int ID = Convert.ToInt32(e.CommandArgument);

            //Get detail information from the database
            OMMDataContext context = new OMMDataContext();
            var nextOfKinDetails = context.NextOfKins.SingleOrDefault(P => P.ID == ID);

            if (nextOfKinDetails != null)
            {
                tbxName.Text = (nextOfKinDetails.Name.ToString().Trim().IsNullOrEmpty()) ? "" : nextOfKinDetails.Name.ToString().Trim();
                tbxRelationShip.Text = (nextOfKinDetails.Relationship.ToString().Trim().IsNullOrEmpty()) ? "" : nextOfKinDetails.Relationship.ToString().Trim();
                tbxAddress.Text = (nextOfKinDetails.Address.ToString().Trim().IsNullOrEmpty()) ? "" : nextOfKinDetails.Address.ToString().Trim();
                tbxPostCode.Text = "";
                ddlCountry.SelectedValue = (nextOfKinDetails.CountryID.ToString().Trim().IsNullOrEmpty()) ? "0" : nextOfKinDetails.CountryID.ToString().Trim();
                tbxHomeNumber.Text = (nextOfKinDetails.HomeNumber.ToString().Trim().IsNullOrEmpty()) ? "" : nextOfKinDetails.HomeNumber.ToString().Trim();
                tbxWorkNumber.Text = (nextOfKinDetails.WorkNumber.ToString().Trim().IsNullOrEmpty()) ? "" : nextOfKinDetails.WorkNumber.ToString().Trim();
                tbxMobile.Text = (nextOfKinDetails.MobileNumber.ToString().Trim().IsNullOrEmpty()) ? "" : nextOfKinDetails.MobileNumber.ToString().Trim();
                tbxChangedBy.Text = (nextOfKinDetails.ChangedByUserID.ToString().Trim().IsNullOrEmpty()) ? "" : nextOfKinDetails.ChangedByUserID.ToString().Trim();
                tbxChangeOn.Text = (nextOfKinDetails.ChangedOn.ToString().Trim().IsNullOrEmpty()) ? "" : nextOfKinDetails.ChangedOn.ToString().Trim();

                tbxNextOfKinID.Text = nextOfKinDetails.ID.ToString().Trim();

                btnSaveNextKin.Text = "Edit";
                
            }

            else
                clearControl();

        }
    }

    protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        //int categoryID = (int)grdsearch.DataKeys[e.RowIndex].Value;
        //DeleteRecordByID(categoryID);
    }

    protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
    {
        //int categoryID = (int)grdsearch.DataKeys[e.RowIndex].Value;
        //DeleteRecordByID(categoryID);
    }
}
