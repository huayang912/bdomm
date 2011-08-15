using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Collections.Generic;
using App.Core.Extensions;

public partial class UserControls_Pager : BaseUserControl
{
    public delegate void PageChanged(object sender, PagerEventArgs e);
    public event PageChanged PageIndexChanged;

    private PagerEventArgs args;
    private int _pageNo = 1;
    private int _pageSize = 10;
    private int _numberOfPageLinkToDisplay = -1;

    #region Public Properties [TotalRecord] [PageIndex] [PageSize] [NumberOfPageLinksToDisplay]
    public long TotalRecord
    {
        get;
        set;
    }
    /// <summary>
    /// Gets or Sets the currently showing data page number
    /// </summary>
    public int PageNo
    {
        get { return _pageNo; }
        set { _pageNo = value; }
    }
    /// <summary>
    /// Gets or Sets the number of records to show in a data page
    /// </summary>
    public int PageSize
    {
        get { return _pageSize; }
        set { _pageSize = value; }
    }
    /// <summary>
    /// Gets or Sets total count of page links to display
    /// </summary>
    public int NumberOfPageLinksToDisplay
    {
        get 
        {
            ///If the number of pages property is not set then
            ///return a default value
            if (_numberOfPageLinkToDisplay <= 0)
                return 5;

            return _numberOfPageLinkToDisplay;
        }
        set
        {
            _numberOfPageLinkToDisplay = value;
        }
    }
    public String TotalRecordMessage
    {
        get;
        set;
    }
    #endregion Public Properties [TotalRecord] [PageIndex] [PageSize] [NumberOfPageLinksToDisplay]

    protected void Page_Load(object sender, EventArgs e)
    {
        /// Page Load Event
    }

    public override void DataBind()
    {
        lblCurrentPage.Text = this.PageNo.ToString();
        //if (this.TotalRecord > 0 && this.PageSize > 0)
        BindPager();
        base.DataBind();
    }

    /// <summary>
    /// Creates the Pager Buttons
    /// </summary>
    private void BindPager()
    {        
        int totalPage = Convert.ToInt32(Math.Ceiling((double)this.TotalRecord / (double)this.PageSize));
        divPageCount.InnerHtml = String.Format("Showing Page {0} of {1}", this.PageNo, totalPage);
        if (totalPage > 1)
        {                        
            int startPage = 0;
            ///Determine the Starting Page to display, calculating 
            ///current page no and total page numbers to show
            if ((this.PageNo - (this.NumberOfPageLinksToDisplay / 2)) <= 0)
            {
                startPage = 1;
            }
            else
            {
                startPage = this.PageNo - (this.NumberOfPageLinksToDisplay / 2);
                if ((totalPage - startPage) < this.NumberOfPageLinksToDisplay)
                {
                    startPage = (totalPage - this.NumberOfPageLinksToDisplay) + 1;
                }
                if (startPage <= 0)
                    startPage = 1;
            }
            ///Populate the Page Objects Collection
            BindPages(startPage, totalPage);
            
            ///Set Visibility for Previous and Next Buttons
            SetVisibilityForPreviousAndNextButtons(totalPage);
        }
        else        
            divPager.Visible = false;                       
        
        divPageCount.Visible = totalPage > 0 ? true : false;
        if (TotalRecord > 0 && !this.TotalRecordMessage.IsNullOrEmpty() && this.TotalRecordMessage.IndexOf("{0}") > -1)        
            divTotalRecordMessage.InnerHtml = String.Format(this.TotalRecordMessage, TotalRecord);
        else
            divTotalRecordMessage.InnerHtml = String.Empty;

        divTotalRecordMessage.Visible = divTotalRecordMessage.InnerHtml.IsNullOrEmpty() ? false : true;
    }
    /// <summary>
    /// Binds the Repeater to show page links
    /// </summary>
    /// <param name="startPage">Start Page Number</param>
    /// <param name="totalPage">Total Page Count</param>
    private void BindPages(int startPage, int totalPage)
    {
        List<DataPage> pages = new List<DataPage>();
        for (int i = 1; i <= this.NumberOfPageLinksToDisplay; i++)
        {
            if (startPage > totalPage)
                break;
            DataPage page = new DataPage();
            page.PageNo = startPage;
            pages.Add(page);
            startPage++;
        }
        ///Bind the Repeater control for Page Numbers
        if (pages.Count > 0)
        {
            rptPages.DataSource = pages;
            rptPages.DataBind();
            divPager.Visible = true;
        }
    }
    /// <summary>
    /// Sets Visibility for Previous and Next Buttons
    /// </summary>
    /// <param name="totalPage"></param>
    private void SetVisibilityForPreviousAndNextButtons(int totalPage)
    {
        if (this.PageNo == 1)
        {
            lnkPrevious.Visible = false;
            lblPrevious.Visible = true;
        }
        else
        {
            lnkPrevious.Visible = true;
            lblPrevious.Visible = false;
        }
        if (this.PageNo == totalPage)
        {
            lnkNext.Visible = false;
            lblNext.Visible = true;
        }
        else
        {
            lnkNext.Visible = true;
            lblNext.Visible = false;
        }
    }
    /// <summary>
    /// Event Handler for Next Previous Buttons
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lnkNextPrevious_Click(object sender, EventArgs e)
    {
        args = new PagerEventArgs();
        switch (((LinkButton)sender).ID)
        {
            case "lnkPrevious":
                args.PageIndex = Convert.ToInt32(lblCurrentPage.Text) - 1;
                break;
            case "lnkNext":
                args.PageIndex = Convert.ToInt32(lblCurrentPage.Text) + 1;
                break;
        }
        this.PageNo = args.PageIndex;
        PropagatePageChangedEvent(args);

        ///Bind the Pager Again
        BindPager();
        lblCurrentPage.Text = args.PageIndex.ToString();
    }

    private void PropagatePageChangedEvent(PagerEventArgs args)
    {
        ///Propagate the Page Change Event
        if (PageIndexChanged == null)
            throw new Exception(String.Format("CustomPager {0} fired a PageChanged event which was not handled.", this.ID));
        else
            PageIndexChanged(this, args);
    }
    /// <summary>
    /// Pager Control Data Bound Event for Contacts
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rptPages_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        args = new PagerEventArgs();
        this.PageNo = Convert.ToInt32(e.CommandArgument);
        args.PageIndex = this.PageNo;
        lblCurrentPage.Text = this.PageNo.ToString();
        PropagatePageChangedEvent(args);
    }
    /// <summary>
    /// Event Handler for ItemDataBount of Pager Control
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rptPages_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        int pageNumber = 0;
        int.TryParse(lblCurrentPage.Text, out pageNumber);

        DataPage page = (DataPage)e.Item.DataItem;
        if (page.PageNo == pageNumber)
        {
            LinkButton lnkPage = e.Item.FindControl("lnkPage") as LinkButton;
            Label lblListingCurrentPage = e.Item.FindControl("lblListingCurrentPage") as Label;
            lnkPage.Visible = false;
            lblListingCurrentPage.Text = page.PageNo.ToString();
            lblListingCurrentPage.Visible = true;
            HtmlGenericControl divPagerContainer = e.Item.FindControl("divPagerContainer") as HtmlGenericControl;
            divPagerContainer.Attributes["class"] = "CurrentPage"; //String.Format("CurrentPage {0}", divPagerContainer.Attributes["class"]);
        }
    }
}


/// <summary>
/// Summary description for PagerEventArgs
/// </summary>
public class PagerEventArgs : EventArgs
{
    public int PageIndex
    {
        get;
        set;
    }    
}

/// <summary>
/// This Class is used to bind pager control
/// </summary>
public class DataPage
{
    private int _pageNo;
    public int PageNo
    {
        get { return _pageNo; }
        set { _pageNo = value; }
    }
}