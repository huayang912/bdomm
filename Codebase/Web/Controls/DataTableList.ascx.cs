using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using App.Core.Extensions;
using App.Core.DB;

public partial class UserControls_DataTableList : BaseUserControl
{
    private bool _HasEditPermission = false;
    private bool _HasDeletePermission = false;
    private bool _addConfirmationPopup = true;
    private bool _enableScroller = true;
    private bool _checkForDeletePermission = true;

    #region Public Properties
    private String _noRecordMessgae = "No record found.";
    private String _editLinkText = "Edit";
    private String _actionHeader = "Actions";
    private bool _openEditPageInNewWindow = false;
    /// <summary>
    /// Gets or Sets the Data Source to Show In Listing
    /// </summary>
    public DataTable DataSource { get; set; }
    /// <summary>
    /// Formattable String to Prepare Edit Link
    /// </summary>
    public String EditLink { get; set; }
    /// <summary>
    /// Gets or Sets the Edit Link Text
    /// </summary>
    public String EditLinkText
    {
        get { return _editLinkText; }
        set { _editLinkText = value; }
    }
    /// <summary>
    /// Formattable String to Prepare Delete Link
    /// </summary>
    public String DeleteLink { get; set; }
    /// <summary>
    /// Delete Message to Confirm from the User
    /// </summary>
    public String DeleteMessage { get; set; }
    /// <summary>
    /// QueryString Parameter Value Fields for the Edit and Delete Link
    /// </summary>
    public String LinkFields { get; set; }     
    /// <summary>
    /// Gets or Sets the Visible Fields List, Comma (,) Separated Filed Names
    /// </summary>
    public String VisibleFields { get; set; }
    /// <summary>
    /// Exclude the Visible Fields from the Field List
    /// </summary>
    public String ExcludeVisibleFields { get; set; }
    /// <summary>
    /// Which Fields Value should be Given in the Selection Checkbox. No checkbox will be there if this value is not specified
    /// </summary>
    public String SelectionCheckBoxField { get; set; }    
    /// <summary>
    /// The Message to Show When there is no record in the datasoure to display to the user.
    /// </summary>
    public String NoRecordMessgae
    {
        get { return _noRecordMessgae; }
        set { _noRecordMessgae = value; }
    }
    public bool AddConfirmationPopup
    {
        get { return _addConfirmationPopup; }
        set { _addConfirmationPopup = value; }
    }
    public bool EnableScroller
    {
        get { return _enableScroller; }
        set { _enableScroller = value; }
    }
    public bool CheckForDeletePermission
    {
        get { return _checkForDeletePermission; }
        set { _checkForDeletePermission = value; }
    }
    public bool OpenEditPageInNewWindow
    {
        get { return _openEditPageInNewWindow; }
        set { _openEditPageInNewWindow = value; }
    }
    public String ActionHeader
    {
        get { return _actionHeader; }
        set { _actionHeader = value; }
    }
    public String ListTableClientID
    {
        get { return String.Format("{0}_tblResult", this.ID); }
    }    

    public String SelectionCheckboxName { get { return string.Format("chk_{0}", this.ID); } }
    #endregion

    #region Private/Protected Method and Properties
    /// <summary>
    /// Determines Whether to Show Selection Checkbox or Not
    /// </summary>
    protected bool ShowSelectionCheckBox
    {
        get { return !this.SelectionCheckBoxField.IsNullOrEmpty(); }
    }

    private String[] VisibleFieldsCollection
    {
        get
        {
            IList<String> fields = this.VisibleFields.IsNullOrEmpty() ? CreateVisibleFieldList() : this.VisibleFields.Split(',').ToList();
            if (!this.ExcludeVisibleFields.IsNullOrEmpty())
            {
                String[] excludedFields = this.ExcludeVisibleFields.Split(',');
                foreach (String str in excludedFields)
                {
                    if (!fields.SingleOrDefault(S => S == str.Trim()).IsNullOrEmpty())
                        fields.Remove(str.Trim());
                }
            }
            return fields.ToArray();
        }
    }

    private IList<String> CreateVisibleFieldList()
    {
        List<String> fields = new List<String>();
        if (this.DataSource != null && this.DataSource.Columns.Count > 0)
        {
            for (int i = 0; i < this.DataSource.Columns.Count; i++)
            {
                fields.Add(this.DataSource.Columns[i].ColumnName);
            }
        }
        return fields;
    }
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {

    }
    public override void DataBind()
    {
        //_HasDeletePermission = base.HasDeletePermission;
        //_HasEditPermission = base.HasEditPermission;
        if (!_HasDeletePermission)
            ucModalConfirmationPopup.ModalDialogType = UserControls_ModalConfirmationPopup.DialogType.OK;

        if (!this.AddConfirmationPopup)
            ucModalConfirmationPopup.Visible = false;

        //base.DataBind();
        if (this.DataSource == null || this.DataSource.Rows.Count == 0)
        {
            rptList.DataSource = null;
            WebUtil.ShowMessageBox(divMessage, this.NoRecordMessgae, true);
        }
        else        
            rptList.DataSource = this.DataSource;            
        
        rptList.DataBind();
    }    
    protected void rptList_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Header)
        {
            Literal ltrHeader = e.Item.FindControl("ltrHeader") as Literal;
            BindHeader(ltrHeader);
        }
        else if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            System.Data.DataRowView row = e.Item.DataItem as DataRowView;
            Literal ltrItems = e.Item.FindControl("ltrItems") as Literal;
            BindItems(row, ltrItems);
        }
    }
    /// <summary>
    /// Binds Header
    /// </summary>
    /// <param name="ltrHeader"></param>
    private void BindHeader(Literal ltrHeader)
    {
        if (this.VisibleFieldsCollection != null && this.VisibleFieldsCollection.Length > 0)
        {
            StringBuilder sb = new StringBuilder(10);
            if(this.ShowSelectionCheckBox)
                sb.AppendFormat("<th style='text-align:center; width:43px;'>Select</th>");

            for (int i = 0; i < this.VisibleFieldsCollection.Length; i++)
            {
                sb.AppendFormat("<th>{0}</th>", this.VisibleFieldsCollection[i].Trim());
            }
            
            if (!this.EditLink.IsNullOrEmpty() || !this.DeleteLink.IsNullOrEmpty())
                sb.AppendFormat("<th style='text-align:center;'>{0}</th>", this.ActionHeader);

            ltrHeader.Text = sb.ToString();
        }
    }
    private void BindItems(DataRowView row, Literal ltrItems)
    {
        if (this.VisibleFieldsCollection != null && this.VisibleFieldsCollection.Length > 0)
        {
            StringBuilder sb = new StringBuilder(10);
            
            if(this.ShowSelectionCheckBox)
                sb.Append(AddSelectionCheckBox(row));
            
            for (int i = 0; i < this.VisibleFieldsCollection.Length; i++)
            {
                sb.AppendFormat("<td>{0}</td>", getFormattedData(row[this.VisibleFieldsCollection[i].Trim()]));
            }
            if (!this.EditLink.IsNullOrEmpty() || !this.DeleteLink.IsNullOrEmpty())
            {
                sb.AppendFormat("<td style='text-align:center;'>{0}&nbsp;{1}</td>", BuildEditLink(row), BuildDeleteLink(row));
            }
            ltrItems.Text = sb.ToString();
        }
    }
    private String getFormattedData(object data)
    {
        if(data.GetType() == typeof(String))
        {
            String text = NullHandler.GetString(data);
            if(text.WordCount() > 30)
                text = text.GetWords(30);
            return WebUtil.FormatText(text);
        }
        else
            return WebUtil.FormatData(data);
    }
    protected String AddSelectionCheckBox(DataRowView row)
    {
        return String.Format("<td style='text-align:center;'><input type='checkbox' name='chk_{0}' value='{1}'/></td>", this.ID, GetCheckBoxValue(row));
    }

    private String GetCheckBoxValue(DataRowView row)
    {
        if (this.SelectionCheckBoxField.IndexOf(',') > -1)
        {
            String[] fields = this.SelectionCheckBoxField.Split(',');
            StringBuilder sb = new StringBuilder(10);
            foreach (String field in fields)
            {
                if(!field.IsNullOrEmpty())
                    sb.Append(NullHandler.GetString(row[field.Trim()]));
            }
            return sb.ToString();
        }
        return NullHandler.GetString(row[this.SelectionCheckBoxField]);
    }    
    protected String BuildEditLink(DataRowView row)
    {
        if (!this.EditLink.IsNullOrEmpty())
        {
            String[] fields = BuildQueryStringValues(row);
            String editLink = String.Format(this.EditLink, fields);            
            return String.Format("<a href=\"{0}\"{2}>{1}</a>", editLink, this.EditLinkText, this.OpenEditPageInNewWindow ? " target=\"_blank\"" : String.Empty);
        }
        return String.Empty;
    }
    protected String BuildDeleteLink(DataRowView row)
    {
        if (!this.DeleteLink.IsNullOrEmpty())
        {
            String[] fields = BuildQueryStringValues(row);
            //String editLink = String.Format(this.EditLink, fields);
            String deleteLink = String.Format(this.DeleteLink, fields);
            if (this.CheckForDeletePermission)
            {
                if (_HasDeletePermission)
                    deleteLink = String.Format("javascript: ConfirmDelete('{0}', '{1}'); void(0);", deleteLink, this.DeleteMessage.IsNullOrEmpty() ? "Sure to Delete Data?" : this.DeleteMessage);
                else
                    deleteLink = WebUtil.GetDeletePermissionDeniedMessage();
            }
            else
                deleteLink = String.Format("javascript: ConfirmDelete('{0}', '{1}'); void(0);", deleteLink, this.DeleteMessage.IsNullOrEmpty() ? "Sure to Delete Data?" : this.DeleteMessage);

            return String.Format("<a href=\"{0}\">Delete</a>", deleteLink);
        }
        return String.Empty;
    }
    //protected String BuildActionLinks(DataRowView row)
    //{
    //    String[] fields = BuildQueryStringValues(row);
    //    String editLink = String.Format(this.EditLink, fields);
    //    String deleteLink = String.Format(this.DeleteLink, fields);
    //    return String.Format("<a href='{0}'>Edit</a> &nbsp; <a href='{1}'>Delete</a>", editLink, deleteLink);
    //}

    protected String[] BuildQueryStringValues(DataRowView row)
    {
        if (this.LinkFields.IsNullOrEmpty())
            throw new ArgumentException("The property 'LinkFields' must be defined to create Edit or Delete link");
        String[] fields = this.LinkFields.Split(',');
        IList<String> values = new List<String>();
        foreach (String filed in fields)
        {
            values.Add(NullHandler.GetString(row[filed.Trim()]));
        }
        return values.ToArray();
    }  
}
