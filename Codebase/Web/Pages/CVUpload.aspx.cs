using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Net;

using System.IO;



public partial class Pages_CVUpload : BasePage
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
            return true;
        else if (String.Compare(extension, ".docx", true) == 0)
            return true;
        else if (String.Compare(extension, ".pdf", true) == 0)
            return true;
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
}
