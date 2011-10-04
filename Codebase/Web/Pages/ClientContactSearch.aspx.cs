
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using App.Core.Extensions;
using App.Data;


public partial class Pages_ClientContactSearch : BasePage
{
    private int _ContactID = 0;
    private int _ID = 0;
    private bool _IsEditMode = false;
    private bool _IsDeleteMode = false;
    private const int PAGE_SIZE = 15;
    private int _PageIndex = 1;
    public string _SearchApplyBy = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        BindPageInfo();
        if (!IsPostBack)
        {
             

            CheckAndDeleteData();
            BindDropDownLists();
            BindClientContactInfo();
            ClientContactList(_PageIndex,"");
            ShowSuccessMessage();

            
        }
        string filtBy = WebUtil.GetQueryStringInString(AppConstants.QueryString.SearchApplyBy);
        if(filtBy.IsNullOrEmpty())
            lblFilterOn.Text = "Filter By: Show All";
            
        else
            lblFilterOn.Text = "Filter By: " + filtBy.Replace("_", "&") + "";

        loadReapeter();
    }


    public void loadReapeter()
    {
        //Dim chars As New List(Of App.CustomModels.StartsWith)

        //    chars = App.CustomModels.StartsWith.GetStartsWith()
        //    rptStartsWith.DataSource = chars
        //    rptStartsWith.DataBind()
        List<App.CustomModels.StartsWith> chars = new List<App.CustomModels.StartsWith>();
        chars = App.CustomModels.StartsWith.GetStartsWith();
        rptStartsWith.DataSource = chars;
        rptStartsWith.DataBind();
    }
    protected void rptStartsWith_Command(object sender, RepeaterCommandEventArgs e)
    {

        String url = String.Format("{0}?{1}={2}&{3}={4}"
            , Request.Url.AbsolutePath
            , AppConstants.QueryString.ID
            , 0
            , AppConstants.QueryString.SearchApplyBy
            , e.CommandArgument.ToString());
        Response.Redirect(url);
        
        //AppConstants.QueryString.SearchApplyBy.Insert(0,e.CommandArgument.ToString());
        //ClientContactList(1 ,e.CommandArgument.ToString());
    }

    protected void rptStartsWith_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        App.CustomModels.StartsWith start = new App.CustomModels.StartsWith();
        start = (App.CustomModels.StartsWith)e.Item.DataItem;

        LinkButton lkbCommand = new LinkButton();
        lkbCommand = (LinkButton)e.Item.FindControl("lkbCommand");
        lkbCommand.CommandArgument = start.Start;
        lkbCommand.Text = start.Start;
    }

    protected void btnList_Click(object sender, EventArgs e)
    {
        //Response.Redirect(AppConstants.Pages.CONTACTSNOTES_LIST);
        //return;
    }

    protected void SearchButton_Click(object sender, EventArgs e)
    {

        String url = String.Format("{0}?{1}={2}&{3}={4}"
            , Request.Url.AbsolutePath
            , AppConstants.QueryString.ID
            , 1
            , AppConstants.QueryString.SearchApplyBy
            , CRM_Type.Text.Replace("&","_"));
        Response.Redirect(url);

        //ClientContactList(1, CRM_Type.Text);
    }

    protected void ShowAllButton_Click(object sender, EventArgs e)
    {
        //Response.Redirect(AppConstants.Pages.CONTACTSNOTES_LIST);
        //return;
    }
    
    /// <summary>
    /// Bindis the Page Initialization Variables
    /// </summary>
    protected void BindPageInfo()
    {
        _SearchApplyBy = WebUtil.GetQueryStringInString(AppConstants.QueryString.SearchApplyBy);
        _ID = WebUtil.GetQueryStringInInt(AppConstants.QueryString.ID);
        //_ContactID = WebUtil.GetQueryStringInInt(AppConstants.QueryString.CONTACT_ID);
        _IsDeleteMode = String.Compare(WebUtil.GetQueryStringInString(
            AppConstants.QueryString.DELETE), "True", true) == 0 ? true : false;
        if (_ID > 0 && !_IsDeleteMode)
        {
            _IsEditMode = true;
            pnlContainer.Visible = true;
        }
        else
        {
            pnlContainer.Visible = false;
        }

        Page.Title = WebUtil.GetPageTitle("Manage Client Contact Details");
    }

    /// <summary>
    /// Binds Dropdownlists for the initial request.
    /// </summary>
    protected void BindDropDownLists()
    {
        //BindDropdownList.Contactses(ddlContactID);
        //BindDropdownList.Userses(ddlChangedByUserID);
    }
    protected void ShowSuccessMessage()
    {
        if (String.Compare(WebUtil.GetQueryStringInString
            (AppConstants.QueryString.SUCCESS_MSG), "True", false) == 0)
            WebUtil.ShowMessageBox(divMessage, "Client Contact Saved Successfully.", false);
    }
    protected void CheckAndDeleteData()
    {
        if (_IsDeleteMode)
        {
            OMMDataContext context = new OMMDataContext();
            var clientContact = context.ClientContacts.FirstOrDefault(P => P.ID == _ID);
            if (clientContact == null)
                WebUtil.ShowMessageBox(divMessage,
                    "Sorry! requested Client Contact Details not found for delete. Delete Failed.", true);
            else
            {
                context.ClientContacts.DeleteOnSubmit(clientContact);
                try
                {
                    context.SubmitChanges();
                    WebUtil.ShowMessageBox(divMessage, "Client Contact Details deleted successfully.", false);
                }
                catch
                {
                    WebUtil.ShowMessageBox(divMessage,
                        "Sorry! this Client Contact details contains related information. Delete failed.", true);
                }
            }
        }
    }
    protected void ClientContactList(int pageNumber, string startsWith)
    {
        _PageIndex = pageNumber;

        string sb = WebUtil.GetQueryStringInString(AppConstants.QueryString.SearchApplyBy);
        int pn = WebUtil.GetQueryStringInInt(AppConstants.QueryString.PN);
        if (pn == 0) pn = 1;

        UtilityDAO dao = new UtilityDAO();
        DbParameter[] parameters = new[] { new DbParameter("@ContactID", 1) };
        int totalRecord = 0;
        //DataSet ds = dao.GetPagedData(AppSQL.GET_BANK_DETAILS_BY_CONTACT, parameters, pageNumber, PAGE_SIZE, out totalRecord);

        DataSet ds = new DataSet();


        if (sb.IsNullOrEmpty())
        {
            ds = dao.GetPagedData(AppSQL.GET_CLIENT_CONTACT_DETAILS_ALL, 
                parameters, pn, PAGE_SIZE, out totalRecord);
        }
        else if (sb.Trim().Length == 1)
        {
            string sql = " WITH ClientCont AS "+
                    " ( "+
                      "   SELECT *, "+
                        " ROW_NUMBER() OVER(ORDER BY CC.ChangedOn DESC) AS RowNumber  "+
                        //" ,c.ID AS [CountryID], c.[Name] AS [CountryName], "+
                        //" c.IsUK AS [IsUK],c.IsEurope AS [IsEurope], c.Code AS [Code] "+
                        " FROM ClientContacts CC "+
                        //" LEFT JOIN Countries c ON cc.CountryID = c.ID  " +
                        " WHERE cc.[Name] LIKE '" + sb.Trim() + "%' " +
                    " ) "+
                    " SELECT * "+
                      "   , (SELECT COUNT(*) FROM ClientCont) AS TotalRecord "+
                    " FROM ClientCont " +
                    " WHERE RowNumber BETWEEN {0} AND {1}";


            ds = dao.GetPagedData(sql,
                parameters, pn, PAGE_SIZE, out totalRecord);
        }


        else if (sb.Trim().Length > 1)
        {
            string sql = "";
            string field = "";
            if (sb.Trim() == "Show All")
            {
                field = "";
            }
            if (sb.Trim() == "All")
            {
                field = "NL_All";
            }
            if (sb.Trim() == "Procurement")
            {
                field = "NL_Procurement";
            }
            if (sb.Trim() == "Personnel")
            {
                field = "NL_Personnel";
            }
            if (sb.Trim() == "O_M")
            {
                field = "NL_O_M";
            }
            if (sb.Trim() == "Project")
            {
                field = "NL_Project";
            }

            if (field == "")
            {
                sql = " WITH ClientCont AS " +
                        " ( " +
                          "   SELECT *, " +
                            " ROW_NUMBER() OVER(ORDER BY CC.ChangedOn DESC) AS RowNumber  " +
                            //" ,c.ID AS [CountryID], c.[Name] AS [CountryName], " +
                            //" c.IsUK AS [IsUK],c.IsEurope AS [IsEurope], c.Code AS [Code] " +
                            " FROM ClientContacts CC " +
                            //" LEFT JOIN Countries c ON cc.CountryID = c.ID  " +
                        " ) " +
                        " SELECT * " +
                          "   , (SELECT COUNT(*) FROM ClientCont) AS TotalRecord " +
                        " FROM ClientCont " +
                        " WHERE RowNumber BETWEEN {0} AND {1}";
            }
            else
            {

                sql = " WITH ClientCont AS " +
                        " ( " +
                          "   SELECT *, " +
                            " ROW_NUMBER() OVER(ORDER BY CC.ChangedOn DESC) AS RowNumber  " +
                            //" ,c.ID AS [CountryID], c.[Name] AS [CountryName], " +
                            //" c.IsUK AS [IsUK],c.IsEurope AS [IsEurope], c.Code AS [Code] " +
                            " FROM ClientContacts CC " +
                            //" LEFT JOIN Countries c ON cc.CountryID = c.ID  " +
                            " WHERE cc." + field + " = 1 " +
                        " ) " +
                        " SELECT * " +
                          "   , (SELECT COUNT(*) FROM ClientCont) AS TotalRecord " +
                        " FROM ClientCont " +
                        " WHERE RowNumber BETWEEN {0} AND {1}";
            }

            ds = dao.GetPagedData(sql,
                parameters, pn, PAGE_SIZE, out totalRecord);
        }

        //Bind the List Control
        ucBankList.DataSource = ds.Tables[0];
        ucBankList.EditLink = Request.Url.AbsolutePath + "?" + 
            //AppConstants.QueryString.CONTACT_ID + "={0}&" + 
            AppConstants.QueryString.ID + "={0}&" +
            AppConstants.QueryString.SearchApplyBy+"="+
                WebUtil.GetQueryStringInString(AppConstants.QueryString.SearchApplyBy)+"&" +
            AppConstants.QueryString.PN +"="+
                WebUtil.GetQueryStringInString(AppConstants.QueryString.PN)
                ;
        ucBankList.DeleteLink = Request.Url.AbsolutePath + "?" + 
            //AppConstants.QueryString.CONTACT_ID + "={0}&" + 
            AppConstants.QueryString.ID + "={0}&" +
            AppConstants.QueryString.DELETE + "=True&" +
            AppConstants.QueryString.SearchApplyBy + "=" +
                WebUtil.GetQueryStringInString(AppConstants.QueryString.SearchApplyBy) + "&" +
            AppConstants.QueryString.PN + "=" +
                WebUtil.GetQueryStringInString(AppConstants.QueryString.PN);
        ucBankList.DataBind();

        //Bind the Pager Control
        ucClientContactListPager.TotalRecord = totalRecord;
        ucClientContactListPager.PageNo = pn;
        ucClientContactListPager.PageSize = PAGE_SIZE;
        ucClientContactListPager.DataBind();

        //divMessage.Visible = false;
    }
    /// <summary>
    /// Binds ContactsNotes Info Requested through Query Strings
    /// </summary>
    protected void BindClientContactInfo()
    {
        OMMDataContext context = new OMMDataContext();
        if (context.ClientContacts.FirstOrDefault(P => P.ID == _ID) == null)
            ShowNotFoundMessage();
        else
        {
            if (_IsEditMode)
            {                
                ClientContact entity = 
                    context.ClientContacts.FirstOrDefault(P => P.ID == _ID );
                if (entity == null)
                {
                    //divMessage.Visible = false;
                    ShowNotFoundMessage();
                }
                else
                {
                    //divMessage.Visible = true;

                    tbxName.Text = entity.Name;
                    tbxJobTitle.Text = entity.JobTitle;
                    tbxEmail.Text = entity.Email;
                    tbxCompanyName.Text = entity.CompanyID.ToString();
                    tbxCompanyAddress.Text = entity.Address;
                    tbxPostCode.Text = entity.Postcode;
                    tbxCountry.Text = entity.CountryID.ToString();
                    tbxTelephone.Text = entity.Telephone;
                    tbxMobile.Text = entity.Mobile;
                    tbxFax.Text = entity.Fax;
                    tbxCompanyEmail.Text = "";
                    tbxCompanyWeb.Text = "";


                    //tbxBranchName.Text = entity.BranchName;
                    //tbxBranchAddress.Text = entity.BranchAddress;
                    //tbxSortCode.Text = entity.SortCode;
                    //tbxAccNumber.Text = entity.AccountNumber;
                    //tbxAccName.Text = entity.AccountName;
                    //tbxBicCode.Text = entity.BicCode;
                    //tbxAbaCode.Text = entity.AbaCode;
                }
            }
        }
    }
    /// <summary>
    /// Shows a Message in the UI and Hides the Data Editing Controls
    /// </summary>
    protected void ShowNotFoundMessage()
    {
        //pnlFormContainer.Visible = false;
        //WebUtil.ShowMessageBox(divMessage, "Requested Client Contact Details was not found.", true);
    }
    protected void SaveClientContact()
    {
        OMMDataContext context = new OMMDataContext();
        ClientContact entity = null;

        if (_IsEditMode)
            entity = context.ClientContacts.FirstOrDefault(P => P.ID == _ID);
        else
        {
            entity = new ClientContact();
            entity.ID = _ID;
            context.ClientContacts.InsertOnSubmit(entity);
        }

        //ddlContactID.SelectedValue.ToInt();
        //entity.BankName = tbxBankName.Text;
        //entity.BranchName = tbxBranchName.Text;
        //entity.BranchAddress = tbxBranchAddress.Text;
        //entity.SortCode = tbxSortCode.Text;
        //entity.AccountNumber = tbxAccNumber.Text;
        //entity.AccountName = tbxAccName.Text;
        //entity.BicCode = tbxBicCode.Text;
        //entity.AbaCode = tbxAbaCode.Text;

        //entity.ChangedByUserId = SessionCache.CurrentUser.ID;
        entity.ChangedOn = DateTime.Now;        
        //entity = entity.ChangedByUsername = SessionCache.CurrentUser.UserName;

        context.SubmitChanges();
        String url = String.Format("{0}?{1}={2}&{3}=True"
            , Request.Url.AbsolutePath
            , AppConstants.QueryString.CONTACT_ID
            , 1
            , AppConstants.QueryString.SUCCESS_MSG);
        Response.Redirect(url);
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            SaveClientContact();
            //Response.Redirect(AppConstants.Pages.CONTACTSNOTES_LIST);
            return;
        }
    }
    //protected void btnList_Click(object sender, EventArgs e)
    //{
    //    //Response.Redirect(AppConstants.Pages.CONTACTSNOTES_LIST);
    //    //return;
    //}
    protected void ucClientContactListPager_PageIndexChanged(object sender, PagerEventArgs e)
    {
        String url = String.Format("{0}?{1}={2}&{3}={4}&{5}={6}"
            , Request.Url.AbsolutePath
            , AppConstants.QueryString.ID
            , 0
            , AppConstants.QueryString.SearchApplyBy
            , WebUtil.GetQueryStringInString(AppConstants.QueryString.SearchApplyBy)
            , AppConstants.QueryString.PN
            , e.PageIndex
            );
        Response.Redirect(url);

       // ClientContactList(e.PageIndex,);
    }
}














