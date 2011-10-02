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

        createGraph1();
        createGraph2();
        //createGraph3();
        //createGraph4();
        //createGraph5();
        
    }

    public void createGraph1()
    {
        UtilityDAO dao = new UtilityDAO();
        DbParameter[] parameters = new[] { new DbParameter("@ContactID", 1) };
        DataSet ds1 = dao.GetDataSet(AppSQL.GET_GRAPH_DATA, parameters, false);
        if (ds1.Tables[0].Rows.Count > 2)
            divGraph1.InnerHtml = CreateChart(ds1.Tables[0], "FCF_Pie3D", "CH1");
    }

    public void createGraph2()
    {
        UtilityDAO dao = new UtilityDAO();
        DbParameter[] parameters = new[] { new DbParameter("@ContactID", 1) };
        DataSet ds2 = dao.GetDataSet(AppSQL.GET_GRAPH_DATA, parameters, false);
        if (ds2.Tables[0].Rows.Count>2)
            divGraph2.InnerHtml = CreateChart(ds2.Tables[0], "FCF_Column3D", "CH2");
    }

    public void createGraph3()
    {
        UtilityDAO dao = new UtilityDAO();
        DbParameter[] parameters = new[] { new DbParameter("@ContactID", 1) };
        DataSet ds3 = dao.GetDataSet(AppSQL.GET_GRAPH_DATA, parameters, false);
        if (ds3.Tables[0].Rows.Count > 2)
            divGraph3.InnerHtml = CreateChart(ds3.Tables[0], "FCF_Line", "CH3");
    }

    //public void createGraph4()
    //{
    //    UtilityDAO dao = new UtilityDAO();
    //    DbParameter[] parameters = new[] { new DbParameter("@ContactID", 1) };
    //    DataSet ds3 = dao.GetDataSet(AppSQL.GET_GRAPH_3_DATA, parameters, false);
    //    divGraph4.InnerHtml = CreateChart(ds3.Tables[0], "FCF_Pie3D", "CH4");
    //}

    //public void createGraph5()
    //{
    //    UtilityDAO dao = new UtilityDAO();
    //    DbParameter[] parameters = new[] { new DbParameter("@ContactID", 1) };
    //    DataSet ds3 = dao.GetDataSet(AppSQL.GET_GRAPH_3_DATA, parameters, false);
    //    divGraph5.InnerHtml = CreateChart(ds3.Tables[0], "FCF_Pie3D", "CH5");
    //}

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
        return FusionCharts.RenderChart("../FusionCharts/" + chartType + ".swf", "myChartId2", strXML, chID, "300", "300", false, false);

    }


    //public string CreateChart2(DataTable graphTab, string chartType)
    //{
    //    //FusionCharts.
        
    //    // Creating util Object
    //    //Util util = new Util();
    //    FCUtility util = new FCUtility();

    //    //In this example, we plot a single series chart from data contained
    //    //in an array. The array will have two columns - first one for data label
    //    //and the next one for data values.

    //    //the name of products. 
    //    string[,] arrData = new string[graphTab.Rows.Count, 2];

    //    int j = 0;
    //    while (j < graphTab.Rows.Count)
    //    {
    //        arrData[j, 0] = Convert.ToDateTime(graphTab.Rows[j][0].ToString()).ToString("MMM yy");
    //        arrData[j, 1] = graphTab.Rows[j][1].ToString();

    //        j++;
    //    }

    //    //Now, we need to convert this data into XML. We convert using string concatenation.
    //    string strXML; int i;

    //    //Initialize <graph> element
    //    strXML = "<graph caption='' numberPrefix='' formatNumberScale='0' decimalPrecision='0'>";

    //    //Convert data to XML and append
    //    for (i = 0; i < graphTab.Rows.Count; i++)
    //    {
    //        //add values using <set name='...' value='...' color='...'/>
    //        strXML += "<set name='" + arrData[i, 0] + "' value='" + arrData[i, 1] + "' color='" + util.getFCColor() + "' />";
    //    }
    //    //Close <graph> element
    //    strXML += "</graph>";

    //    //int graphWidth = graphTab.Rows.Count * 100;
    //    //if (graphWidth >= 4000)
    //    //{
    //    //    graphWidth = 4000;
    //    //}

    //    //Create the chart - Column 3D Chart with data contained in strXML

    //            return FusionCharts.RenderChart("../FusionCharts/" + chartType + ".swf", "myChartId1", strXML, "c2", Convert.ToString(400), "300", false, false);

    //}
}
