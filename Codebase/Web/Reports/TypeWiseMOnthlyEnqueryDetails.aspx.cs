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


public partial class Reports_TypeWiseMOnthlyEnqueryDetails : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindMonthDropDownlist();
            BindYearDropDownlist();
        }
        else
        {
            //btnShowReport_Click(sender, e);
            loadReport();
        }



    }
    protected void BindYearDropDownlist()
    {
        for (int i = DateTime.Now.Year - 5; i <= DateTime.Now.Year + 5; i++)
        {
            ddlYear.Items.Add(new ListItem(i.ToString(), i.ToString()));
        }
    }

    protected void BindMonthDropDownlist()
    {
        //for (int i = DateTime.Now.Year - 5; i <= DateTime.Now.Year + 5; i++)
        //{
        //    ddlMonth.Items.Add(new ListItem(i.ToString(), i.ToString()));
        //}

        ddlMonth.Items.Add(new ListItem("January", "January"));
        ddlMonth.Items.Add(new ListItem("February", "February"));
        ddlMonth.Items.Add(new ListItem("March", "March"));
        ddlMonth.Items.Add(new ListItem("April", "April"));
        ddlMonth.Items.Add(new ListItem("May", "May"));
        ddlMonth.Items.Add(new ListItem("June", "June"));
        ddlMonth.Items.Add(new ListItem("July", "July"));
        ddlMonth.Items.Add(new ListItem("August", "August"));
        ddlMonth.Items.Add(new ListItem("September", "September"));
        ddlMonth.Items.Add(new ListItem("October", "October"));
        ddlMonth.Items.Add(new ListItem("November", "November"));
        ddlMonth.Items.Add(new ListItem("December", "December"));
    }

    protected void btnShowReport_Click(object sender, EventArgs e)
    {
        
    }


    public void loadReport()
    {
        OMMDataContext dataContext = new OMMDataContext();
        var query1 = from i in dataContext.reportTypeWiseMOnthlyEnqueryDetails()
                     where i.CreatedYear == Convert.ToInt32(ddlYear.Text)
                     && i.CreatedMonth == ddlMonth.Text.Trim()
                     //orderby i.Price
                     select i;
        DataTable dtEnqueryDetails = LINQToDataTable(query1);



        ReportDocument repDoc = new ReportDocument();
        repDoc.Load(HttpContext.Current.Request.PhysicalApplicationPath.Trim()
                    + @"\Reports\TypeWiseMOnthlyEnqueryDetails.rpt");




        //repDoc.SetDataSource = DBNull.Value;
        repDoc.SetDataSource(dtEnqueryDetails);

        CrystalReportViewer1.ReportSource = repDoc;
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
