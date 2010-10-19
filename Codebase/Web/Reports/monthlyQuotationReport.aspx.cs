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


public partial class Reports_monthlyQuotationReport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnShowReport_Click(object sender, EventArgs e)
    {
        OMMDataContext dataContext = new OMMDataContext();
        var query1 = from i in dataContext.reportMonthlyQuotations()
                     where i.Year == Convert.ToInt32(ddlYear.Text)
                     //orderby i.Price
                     select i;
        DataTable dtEnqueryDetails = LINQToDataTable(query1);

        

        ReportDocument repDoc = new ReportDocument();
        repDoc.Load(HttpContext.Current.Request.PhysicalApplicationPath.Trim()
                    + @"\Reports\monthlyQuotationReport.rpt");

        //repDoc.SetDataSource = DBNull.Value;
        repDoc.SetDataSource(dtEnqueryDetails);

        CrystalReportViewer1.ReportSource = repDoc;
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
                    ==typeof(Nullable<>)))
                     {
                         colType = colType.GetGenericArguments()[0];
                     }

                    dtReturn.Columns.Add(new DataColumn(pi.Name, colType));
               }
          }

          DataRow dr = dtReturn.NewRow();

          foreach (PropertyInfo pi in oProps)
          {
               dr[pi.Name] = pi.GetValue(rec, null) == null ?DBNull.Value :pi.GetValue
               (rec,null);
          }

          dtReturn.Rows.Add(dr);
     }
     return dtReturn;
}



}
