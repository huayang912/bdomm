using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Pages_QuotationDetails : BasePage
{
    private int _QuotationID = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        BindPageInfo();
        if (!IsPostBack)
        {
            BindQuotationInfo();
        }
    }
    protected void BindPageInfo()
    {
        _QuotationID = WebUtil.GetQueryStringInInt(AppConstants.QueryString.ID);
        this.Master.SelectedTab = SelectedTab.Project;
    }
    protected void BindQuotationInfo()
    {
        OMMDataContext context = new OMMDataContext();
        Quotation quotation = context.Quotations.SingleOrDefault(Q => Q.ID == _QuotationID);
        if (quotation != null)
        {
            ltrHeading.Text = String.Format("Quotation Details: Quotation {0} ", quotation.Number);
            Page.Title = WebUtil.GetPageTitle(ltrHeading.Text);
        }
    }
}
