using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

public partial class Pages_EnquiryFiles : BasePage
{
    private int _EnquiryID = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        BindPageInfo();
        if (!IsPostBack)
        {
            BindEnquiryDetails();
        }
    }
    protected void BindPageInfo()
    {
        Page.Title = WebUtil.GetPageTitle("Upload File for Enquiry");
        _EnquiryID = WebUtil.GetQueryStringInInt(AppConstants.QueryString.ID);
    }
    protected void BindEnquiryDetails()
    {
        if (_EnquiryID > 0)
        {
            OMMDataContext context = new OMMDataContext();
            Enquiry enquiry = context.Enquiries.SingleOrDefault(E => E.ID == _EnquiryID);
            if (enquiry == null)
            {
                WebUtil.ShowMessageBox(divMessage, "Sorry! requested enquiry was not found.", true);
                pnlUploadContainer.Visible = false;
            }
            else
                ltrHeading.Text = String.Format("Attach Files: Enquiry {0}", enquiry.Number);
        }
        else
            ltrHeading.Text = "Attach Files.";
    }
    protected void btnUpload_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            String fileName = UploadAndSaveFile();
            SaveInSession(fileName);
        }
    }
    protected void SaveInSession(String fileName)
    {
        if (String.IsNullOrEmpty(fileName))
            hdnFileName.Value = String.Empty;
        else
        {
            EnquiryFile attachment = new EnquiryFile();
            attachment.EnquiryID = _EnquiryID;
            attachment.FileName = fileName;
            attachment.UploadedBy = SessionCache.CurrentUser.ID;
            attachment.UploadedOn = DateTime.Now;
            IList<EnquiryFile> files = SessionCache.CurrentEnquiryFiles;
            if (files.SingleOrDefault(F => F.FileName == fileName) == null)
                files.Add(attachment);
            SessionCache.CurrentEnquiryFiles = files;
            hdnFileName.Value = fileName;
        }
    }
    protected String UploadAndSaveFile()
    {
        if (fileEnquiry.HasFile)
        {
            if (IsValidDocument(fileEnquiry.PostedFile))
            {
                String uploadDirectory = Server.MapPath(AppConstants.TEMP_DIRECTORY);
                if (!Directory.Exists(uploadDirectory))
                    Directory.CreateDirectory(uploadDirectory);
                //String fileName = String.Format("{0}_{1}", SessionCache.CurrentUser.ID, Path.GetFileName(fileEnquiry.FileName));
                String fileName = Path.GetFileName(fileEnquiry.FileName);
                fileEnquiry.SaveAs(Path.Combine(uploadDirectory, fileName));
                return fileName;
            }
            else
                WebUtil.ShowMessageBox(divMessage, "only Microsoft Word (*.doc, *.docx) and PDF (*.pdf) documents are allowed for attachment.", true);

        }
        return String.Empty;
    }
    /// <summary>
    /// Checks Whethear the Uploaded file Is a Valid Document
    /// </summary>
    /// <param name="httpPostedFile"></param>
    /// <returns></returns>
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
}
