using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Web.SessionState;
//using Utilities;
using InfoSoftGlobal;
using System.Collections.Generic;
using System.IO;

using App.Core.Extensions;
using App.Data;


public partial class Pages_Home : BasePage
{

    //private string _lastRefresh = null;
    
    public string CssClass
    {
        get
        {
            return "HomePage Wide";
        }
    }
    
    protected void Page_Load(object sender, EventArgs e)
    {
        Page.Title = WebUtil.GetPageTitle("Dashboard");
        lblGraphTitle.Text = "Month: " + System.DateTime.Today.ToString("MMM-yyyy");
        

        string test = (String)Session["LastRefreshTime"];

        if ((String)Session["LastRefreshTime"] == null)
        {
            Session["LastRefreshTime"] = System.DateTime.Now.ToString();

            lblLastRefreshedOn.Text = System.DateTime.Now.ToString();
            lblNextRefreshTime.Text = 
                System.DateTime.Now.AddMinutes
                (Convert.ToDouble(AppConstants.QueryString.GRAPH_REFRESH_TIME)).ToString();

            //createGraph1();
            createGraph2("RefreshData");
            //createGraph3();
            //createGraph4();
            //createGraph5();
        }
        else
        {
            double refreshTime = 
                Convert.ToDouble(AppConstants.QueryString.GRAPH_REFRESH_TIME);

            if (Convert.ToDateTime((String)Session["LastRefreshTime"])
                < System.DateTime.Now.AddMinutes(-refreshTime))
            {
                Session["LastRefreshTime"] = System.DateTime.Now.ToString();

                lblLastRefreshedOn.Text = System.DateTime.Now.ToString();
                lblNextRefreshTime.Text = 
                    System.DateTime.Now.AddMinutes
                    (Convert.ToDouble(AppConstants.QueryString.GRAPH_REFRESH_TIME)).ToString();

                createGraph2("RefreshData");
            }
            else
            {
                lblLastRefreshedOn.Text = (String)Session["LastRefreshTime"];
                lblNextRefreshTime.Text = 
                    Convert.ToDateTime((String)Session["LastRefreshTime"]).AddMinutes
                    (Convert.ToDouble(AppConstants.QueryString.GRAPH_REFRESH_TIME)).ToString();

                createGraph2("");
            }
        }
        
    }

    public void createGraph1()
    {
        UtilityDAO dao = new UtilityDAO();
        DbParameter[] parameters = new[] { new DbParameter("@ContactID", 1) };
        DataSet ds1 = dao.GetDataSet(AppSQL.GET_GRAPH_DATA, parameters, false);
        if (ds1.Tables[0].Rows.Count > 2)
            divGraph1.InnerHtml = CreateChart(ds1.Tables[0], "FCF_Pie3D", "CH1");
    }

    public void createGraph2(string refData)
    {
        if (refData.Trim() == "RefreshData")
        {
            UtilityDAO dao = new UtilityDAO();
            DbParameter[] parameters = new[] { new DbParameter("@ContactID", 1) };
            DataSet ds2 = dao.GetDataSet(AppSQL.GET_GRAPH_DATA, parameters, false);

            Session["dtbl"] = ds2.Tables[0];

            if (ds2.Tables[0].Rows.Count > 2)
                divGraph2.InnerHtml = CreateChart(ds2.Tables[0], "FCF_Column3D", "CH2");
        }
        else
        {
            if ((DataTable)Session["dtbl"] != null)
            {
                DataTable dtbl = (DataTable)Session["dtbl"];
                if (dtbl.Rows.Count > 2)
                    divGraph2.InnerHtml = CreateChart(dtbl, "FCF_Column3D", "CH2");
            }
        }
    }

    public void createGraph3()
    {
        UtilityDAO dao = new UtilityDAO();
        DbParameter[] parameters = new[] { new DbParameter("@ContactID", 1) };
        DataSet ds3 = dao.GetDataSet(AppSQL.GET_GRAPH_DATA, parameters, false);
        if (ds3.Tables[0].Rows.Count > 2)
            divGraph3.InnerHtml = CreateChart(ds3.Tables[0], "FCF_Line", "CH3");
    }

    public string CreateChart(DataTable graphTab, string chartType, string chID)
    {
        // Creating util Object
        FCUtility util = new FCUtility();

        //the name of products. 
        string[,] arrData = new string[graphTab.Rows.Count, 2];

        int j = 0;
        while (j < graphTab.Rows.Count)
        {
            //arrData[j, 0] =Convert.ToDateTime(graphTab.Rows[j][0].ToString()).ToString("MMM yy");
            //arrData[j, 1] = graphTab.Rows[j][1].ToString();

            arrData[j, 0] = graphTab.Rows[j][0].ToString();
            arrData[j, 1] = graphTab.Rows[j][1].ToString();

            j++;
        }

        //Now, we need to convert this data into XML. We convert using string concatenation.
        string strXML; int i;

        //Initialize <graph> element
        strXML = "<graph caption='' numberPrefix='' formatNumberScale='0' decimalPrecision='0'>";

        //Convert data to XML and append
        for (i = 0; i < graphTab.Rows.Count; i++)
        {
            //add values using <set name='...' value='...' color='...'/>
            strXML += "<set name='" + arrData[i, 0] + "' value='" + arrData[i, 1] + "' color='" + util.getFCColor() + "' />";
        }
        //Close <graph> element
        strXML += "</graph>";

        //Create the chart - Column 3D Chart with data contained in strXML
        return FusionCharts.RenderChart("../FusionCharts/" + chartType + ".swf", "myChartId2", strXML, chID, "600", "300", false, false);

    }

}
