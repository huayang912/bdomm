using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Pages_EnquiryDetails : BasePage
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
        _EnquiryID = WebUtil.GetQueryStringInInt(AppConstants.QueryString.ID);
    }
    protected void BindEnquiryDetails()
    {
        OMMDataContext context = new OMMDataContext();
        Enquiry enquiry = context.Enquiries.SingleOrDefault(E => E.ID == _EnquiryID);
        if (enquiry != null)
        {
            ltrHeading.Text = String.Format("Enquiry Details: Enquiry {0}", enquiry.Number);
        }
    }
}
