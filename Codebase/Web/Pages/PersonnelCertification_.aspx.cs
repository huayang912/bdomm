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





public partial class Pages_PersonnelCertification_ : BasePage
{
    protected int _ContactID = 0;
   protected int _CurrentUserID = 0;
    
    protected void Page_Load(object sender, EventArgs e)
    {
        
        if (!IsPostBack)
        {
            Page.Title = WebUtil.GetPageTitle("Personnel Certification");
            _ContactID = WebUtil.GetQueryStringInInt("ID");
            _CurrentUserID = SessionCache.CurrentUser.ID;


            //Countries(ddlCertificateType);
            BindDropdownList.CertificateTypes(ddlCertificateType);

            loadCertificationList(_ContactID);            
        }
        
    }


    

    


    public void loadImmediateFamilyDetails(int contactID)
    {
        //OMMDataContext context = new OMMDataContext();
        ////IList<ContactCV> contactCV = (from P 
        ////                                  in context.ContactCVs 
        ////                              where P.ContactID == contactID 
        ////                              select P).ToList();

        ////IList<ContactsNextOfKin> ContactsNextOfKin = (from P
        ////                                  in context.ContactsNextOfKins
        ////                              where P.ContactID == contactID
        ////                              select P).ToList();

        //var ContactsNextOfKins = context.ContactsNextOfKins.FirstOrDefault(P => P.ContactID == contactID);

        //if (ContactsNextOfKins == null || contactID == 0)
        //{
        //    btnUpload.Text = "Save";
        //}
        //else
        //{
        //    btnUpload.Text = "Update";

        //    tbxMotherName.Text = (ContactsNextOfKins.MothersName.IsNullOrEmpty()) ? "" : ContactsNextOfKins.MothersName.ToString();
        //    tbxFatherName.Text = (ContactsNextOfKins.FathersName.IsNullOrEmpty()) ? "" : ContactsNextOfKins.FathersName.ToString(); 
        //    tbxChildName.Text = (ContactsNextOfKins.ChildrensNames.IsNullOrEmpty()) ? "" : ContactsNextOfKins.ChildrensNames.ToString();           
            

        //    //grdsearch.DataSource = ContactsNextOfKin;
        //    //grdsearch.DataBind();
        //}
    }


    public void loadCertificationList(int contactID)
    {
        grdsearch.DataSource = null;
        grdsearch.DataBind();


        OMMDataContext context = new OMMDataContext();
        IList<Certificate> Certificate_ = (
            from P
            in context.Certificates
            //join PP in context.CertificateTypes on P.TypeID equals PP.ID
            where P.ContactID == contactID
            
            select P

              //select new

              //  {
              //      ID = P.ID,
              //      TypeID = P.TypeID,
              //      Details = P.Details,
              //      ExpiryDate = P.ExpiryDate,
              //      PlaceIssued = P.PlaceIssued,
              //      ChangedByUserID = P.ChangedByUserID,
              //      ChangedOn = P.ChangedOn
              //  }

            ).ToList();

        if (Certificate_ == null || contactID == 0)
        {
            grdsearch.DataSource = Certificate_;
            grdsearch.DataBind();

            btnSaveNextKin.Text = "Save";
        }
        else
        {
            grdsearch.DataSource = Certificate_;
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
        tbxDetails.Text = "";
        tbxExpiryDate.Text = "";
        tbxPlaceIssued.Text = "";
        tbxChangedBy.Text = "";
        tbxChangeOn.Text = "";
        tbxNextOfKinID.Text = "";

        ddlCertificateType.SelectedValue = "0";

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
                Certificate ct = new Certificate();
                ct.ContactID = contactID;
                if (Convert.ToInt32(ddlCertificateType.SelectedValue) > 0)
                {
                    ct.TypeID = Convert.ToInt32(ddlCertificateType.SelectedValue);
                }
                ct.Details = (tbxDetails.Text.ToString().Trim().IsNullOrEmpty()) ? "" : tbxDetails.Text.ToString().Trim();
                if (!tbxExpiryDate.Text.ToString().Trim().IsNullOrEmpty())
                {
                    ct.ExpiryDate = Convert.ToDateTime(tbxExpiryDate.Text.ToString().Trim());
                }
                ct.PlaceIssued = (tbxPlaceIssued.Text.ToString().Trim().IsNullOrEmpty()) ? "" : tbxPlaceIssued.Text.ToString().Trim();

                ct.ChangedByUserID = SessionCache.CurrentUser.ID;
                ct.ChangedOn = System.DateTime.Today;

                context.Certificates.InsertOnSubmit(ct);
                context.SubmitChanges();

                clearControl();
            }
            else
            {
                var ct = context.Certificates.FirstOrDefault(P => P.ID == Convert.ToInt32(tbxNextOfKinID.Text));


                if (ct == null || contactID == 0)
                {

                }
                else
                {
                    ct.ContactID = contactID;
                    if (Convert.ToInt32(ddlCertificateType.SelectedValue) > 0)
                    {
                        ct.TypeID = Convert.ToInt32(ddlCertificateType.SelectedValue);
                    }
                    ct.Details = (tbxDetails.Text.ToString().Trim().IsNullOrEmpty()) ? "" : tbxDetails.Text.ToString().Trim();

                    if (!tbxExpiryDate.Text.ToString().Trim().IsNullOrEmpty())
                    {
                        ct.ExpiryDate = Convert.ToDateTime(tbxExpiryDate.Text.ToString().Trim());
                    }
                    
                    ct.PlaceIssued = (tbxPlaceIssued.Text.ToString().Trim().IsNullOrEmpty()) ? "" : tbxPlaceIssued.Text.ToString().Trim();

                    ct.ChangedByUserID = SessionCache.CurrentUser.ID;
                    ct.ChangedOn = System.DateTime.Today;

                    context.SubmitChanges();
                }
            }


            loadCertificationList(contactID);
        }
    }


    protected void btnUpload_onclick(object sender, EventArgs e)
    {
        ////Save information in the database
        //OMMDataContext context = new OMMDataContext();
        //int contactID = WebUtil.GetQueryStringInInt("ID");

        //if (btnUpload.Text == "Save")
        //{
        //    ContactsNextOfKin conNextKin = new ContactsNextOfKin();

        //    conNextKin.MothersName = (tbxMotherName.Text.ToString().Trim().IsNullOrEmpty()) ? "" : tbxMotherName.Text.ToString().Trim();
        //    conNextKin.FathersName = (tbxFatherName.Text.ToString().Trim().IsNullOrEmpty()) ? "" : tbxFatherName.Text.ToString().Trim();
        //    conNextKin.ChildrensNames = (tbxChildName.Text.ToString().Trim().IsNullOrEmpty()) ? "" : tbxChildName.Text.ToString().Trim();
        //    conNextKin.ChangedByUserID = SessionCache.CurrentUser.ID;
        //    conNextKin.ChangedOn = System.DateTime.Today;

        //    context.ContactsNextOfKins.InsertOnSubmit(conNextKin);
        //    context.SubmitChanges();
        //}
        //else
        //{
        //    var ContactsNextOfKins = context.ContactsNextOfKins.FirstOrDefault(P => P.ContactID == contactID);


        //    if (ContactsNextOfKins == null || contactID == 0)
        //    {
                
        //    }
        //    else
        //    {
        //        ContactsNextOfKins.MothersName = (tbxMotherName.Text.ToString().Trim().IsNullOrEmpty()) ? "" : tbxMotherName.Text.ToString().Trim();
        //        ContactsNextOfKins.FathersName = (tbxFatherName.Text.ToString().Trim().IsNullOrEmpty()) ? "" : tbxFatherName.Text.ToString().Trim();
        //        ContactsNextOfKins.ChildrensNames = (tbxChildName.Text.ToString().Trim().IsNullOrEmpty()) ? "" : tbxChildName.Text.ToString().Trim();
        //        ContactsNextOfKins.ChangedByUserID = SessionCache.CurrentUser.ID;
        //        ContactsNextOfKins.ChangedOn = System.DateTime.Today;

        //        context.SubmitChanges();
        //    }
        //}


        //loadImmediateFamilyDetails(contactID);
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
            var ct = context.Certificates.SingleOrDefault(P => P.ID == ID);

            if (ct != null)
            {
                //Delete Next of Kin information from the database
                context.Certificates.DeleteOnSubmit(ct);
                context.SubmitChanges();
            }

            loadCertificationList(WebUtil.GetQueryStringInInt("ID"));
        }

        if (e.CommandName == "Edit")
        {
            int ID = Convert.ToInt32(e.CommandArgument);

            //Get detail information from the database
            OMMDataContext context = new OMMDataContext();
            var ct = context.Certificates.SingleOrDefault(P => P.ID == ID);

            if (ct != null)
            {
                ddlCertificateType.SelectedValue = (ct.TypeID.ToString().Trim().IsNullOrEmpty()) ? "0" : ct.TypeID.ToString().Trim();
                tbxDetails.Text = (ct.Details.ToString().Trim().IsNullOrEmpty()) ? "" : ct.Details.ToString().Trim();
                tbxExpiryDate.Text = (ct.ExpiryDate.ToString().Trim().IsNullOrEmpty()) ? "" : ct.ExpiryDate.ToString().Trim();
                tbxPlaceIssued.Text = (ct.PlaceIssued.IsNullOrEmpty()) ? "" : ct.PlaceIssued.ToString().Trim();
                
                tbxChangedBy.Text = (ct.ChangedByUserID.ToString().Trim().IsNullOrEmpty()) ? "" : ct.ChangedByUserID.ToString().Trim();
                tbxChangeOn.Text = (ct.ChangedOn.ToString().Trim().IsNullOrEmpty()) ? "" : ct.ChangedOn.ToString().Trim();

                tbxNextOfKinID.Text = ct.ID.ToString().Trim();

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
