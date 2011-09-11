using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Reflection;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;


public partial class Reports_monthlyEnqueryReport : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        BindPageInfo();        
        if (!IsPostBack)
        {
            BindYearDropDownlist();
        }

        else
        {
            //btnShowReport_Click(sender, e);
            loadReport();
        }
    }
    protected void BindPageInfo()
    {
        this.Master.SelectedTab = SelectedTab.Report;
        Page.Title = WebUtil.GetPageTitle(ltrHeading.Text);
    }
    /// <summary>
    /// Load list of year in the dropdown control
    /// </summary>
    protected void BindYearDropDownlist()
    {
        for (int i = DateTime.Now.Year - 5; i <= DateTime.Now.Year + 5; i++)
        {
            ddlYear.Items.Add(new ListItem(i.ToString(), i.ToString()));
        }
    }
    
    protected void btnShowReport_Click(object sender, EventArgs e)
    {
        //Necessary Method called from the PostBack 
        //Occured after click the button
    }

    public void loadReport()
    {
        OMMDataContext dataContext = new OMMDataContext();
        var query1 = from i in dataContext.reportMonthlyEnquery()
                     where i.Year == Convert.ToInt32(ddlYear.Text)
                     //orderby i.Price
                     select i;
        DataTable dtEnqueryDetails = LINQToDataTable(query1);



        ReportDocument repDoc = new ReportDocument();
        repDoc.Load(HttpContext.Current.Request.PhysicalApplicationPath.Trim()
                    + @"\Reports\monthlyEnqueryReport.rpt");

        CrystalReportViewer1.Visible = true;
        CrystalReportViewer1.ReportSource = repDoc;
        repDoc.SetDataSource(dtEnqueryDetails);

        divReportContainer.Visible = true;
    }


    public DataTable LINQToDataTable<T>(IEnumerable<T> varlist)
    {
        DataTable dtReturn = new DataTable();

        // column names 
        PropertyInfo[] oProps = null;

        if (varlist == null) return dtReturn;

        foreach (T rec in varlist)
        {
            // Use reflection to get property names, to create table, Only first time, others 
            //will follow 
            if (oProps == null)
            {
                oProps = ((Type)rec.GetType()).GetProperties();
                foreach (PropertyInfo pi in oProps)
                {
                    Type colType = pi.PropertyType;

                    if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition()
                    == typeof(Nullable<>)))
                    {
                        colType = colType.GetGenericArguments()[0];
                    }

                    dtReturn.Columns.Add(new DataColumn(pi.Name, colType));
                }
            }

            DataRow dr = dtReturn.NewRow();

            foreach (PropertyInfo pi in oProps)
            {
                dr[pi.Name] = pi.GetValue(rec, null) == null ? DBNull.Value : pi.GetValue
                (rec, null);
            }

            dtReturn.Rows.Add(dr);
        }
        return dtReturn;
    }
}
