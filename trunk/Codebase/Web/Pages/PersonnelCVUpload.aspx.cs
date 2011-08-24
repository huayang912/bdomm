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





public partial class Pages_PersonnelCVUpload : BasePage
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
            Page.Title = WebUtil.GetPageTitle("Upload CV");
            _ContactID = WebUtil.GetQueryStringInInt("ID");
            _CurrentUserID = SessionCache.CurrentUser.ID;
            loadUploadedDoc(_ContactID);
        }
        
    }

    
    public void loadUploadedDoc(int contactID)
    {
        OMMDataContext context = new OMMDataContext();
        IList<ContactCV> contactCV = (from P 
                                          in context.ContactCVs 
                                      where P.ContactID == contactID 
                                      select P).ToList();
        

        //OMMDataContext context = new OMMDataContext();
        ////Enquiry enquiry = context.Enquiries.SingleOrDefault(E => E.ID == _EnquiryID);
        //ContactCV contactCV = context.ContactCVs.SingleOrDefault(from P in context.ContactCVs where P.ContactID == contactID);
        
        
        if (contactCV == null || contactID == 0)
        {
            WebUtil.ShowMessageBox(divAttachmentError, "No document uploaded for this contact yet!", true);
        }
        else
        {


            grdsearch.DataSource = contactCV;
            grdsearch.DataBind();
        }
    }

    protected void btnUpload_onclick(object sender, EventArgs e)
    {
        if (fileUploadCV.HasFile)
        {
            String uploadDirectory = Server.MapPath("/UploadedCV");
            if (IsValidDocument(fileUploadCV.PostedFile))
            {
                //Save information in the database
                OMMDataContext context = new OMMDataContext();
                ContactCV contactCV = new ContactCV();

                contactCV.ContactID = WebUtil.GetQueryStringInInt("ID");
                contactCV.FileName = Path.GetFileName(fileUploadCV.FileName);
                contactCV.CreatedBy = SessionCache.CurrentUser.ID;
                contactCV.ChangedBy = SessionCache.CurrentUser.ID;
                contactCV.ChangedOn = DateTime.Now;

                context.ContactCVs.InsertOnSubmit(contactCV);
                context.SubmitChanges(); 


                //Now upload the CV in the web server
                saveFile(uploadDirectory, contactCV.ID);

                loadUploadedDoc(WebUtil.GetQueryStringInInt("ID"));
            }
            else
                WebUtil.ShowMessageBox(divAttachmentError, String.Format("Only Microsoft Word (*.doc, *.docx) and PDF (*.pdf) documents are allowed."), true);
        }
        
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
        if (!Directory.Exists(uploadDirectory))
            Directory.CreateDirectory(uploadDirectory);
        //String fileName = String.Format("{0}_{1}", SessionCache.CurrentUser.ID, Path.GetFileName(fileEnquiry.FileName));
        String fileName = detailID.ToString()+"_"+Path.GetFileName(fileUploadCV.FileName);
        fileUploadCV.SaveAs(Path.Combine(uploadDirectory, fileName));
        return fileName;
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
            "confirm('Sure to Delete This Document?')");
        }
    }

    protected void GridView1_RowCommand(object sender,
                         GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Delete")
        {
            int detailsID = Convert.ToInt32(e.CommandArgument);

            //Get detail information from the database
            OMMDataContext context = new OMMDataContext();
            var cvDetails = context.ContactCVs.SingleOrDefault(P => P.ID == detailsID);

            if (cvDetails != null)
            {
                //First delete CV information from the database
                context.ContactCVs.DeleteOnSubmit(cvDetails);
                context.SubmitChanges();

                
                //Now delete the file from the filesystem
                string path = Server.MapPath(AppConstants.PERSONNEL_CV_DIRECTORY).ToString() + "\\" + detailsID .ToString()+ "_"+cvDetails.FileName.ToString()+"";
                //File.Delete(path);

                // Delete a file by using File class static method...
                if (System.IO.File.Exists(path))
                {
                    // Use a try block to catch IOExceptions, to
                    // handle the case of the file already being
                    // opened by another process.
                    try
                    {
                        System.IO.File.Delete(path);
                    }
                    catch(Exception ex)
                    {
                        string te = ex.Message;
                    }
                }




                //Now reload the cv list
                loadUploadedDoc(cvDetails.ContactID);
            }
            
            //DeleteRecordByID(categoryID);
           

        }
    }

    protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        //int categoryID = (int)grdsearch.DataKeys[e.RowIndex].Value;
        //DeleteRecordByID(categoryID);
    }
}
