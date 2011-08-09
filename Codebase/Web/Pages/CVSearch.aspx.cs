using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Net;



public partial class Pages_CVSearch : BasePage
{
    
    public string CssClass
    {
        get
        {
            return "";
        }
    }
    
    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        //Dim strCatalog As String
        string strCatalog;

        //' Catalog Name
        //strCatalog = "TestCatalog"
        strCatalog = ConfigReader.CatelogName.ToString();

        //Dim strQuery As String
        string strQuery;
        //strQuery = "Select DocTitle,Filename,Size,PATH,URL from SCOPE() where FREETEXT('" & TextBox1.Text & "')"
        strQuery = "Select DocTitle,Filename,Size,PATH,URL from SCOPE() where FREETEXT('" + TextBox1.Text + "')";
        //' TextBox1.Text is word that you type in the text box to query by using Index Service.
        //'
        //Dim connString As String = "Provider=MSIDXS.1;Integrated Security .='';Data Source='" & strCatalog & "'"
        string connString;
        connString = "Provider=MSIDXS.1;Integrated Security .='';Data Source='" + strCatalog + "'";

        //Dim cn As New System.Data.OleDb.OleDbConnection(connString)
        System.Data.OleDb.OleDbConnection cn = new System.Data.OleDb.OleDbConnection(connString);
        //Dim cmd As New System.Data.OleDb.OleDbDataAdapter(strQuery, cn)
        System.Data.OleDb.OleDbDataAdapter cmd = new System.Data.OleDb.OleDbDataAdapter(strQuery, cn);
        //Dim testDataSet As New System.Data.DataSet()
        System.Data.DataSet testDataSet = new DataSet();

        //cmd.Fill(testDataSet)
        cmd.Fill(testDataSet);

        //' Bind DataGrid to the DataSet. DataGrid is the ID for the 
        //' DataGrid control in the HTML section.
        //Dim source As New System.Data.DataView(testDataSet.Tables(0))
        //DataGrid1.DataSource = source
        //DataGrid1.DataBind()

        //GridView1.DataSource = testDataSet.Tables[0];
        //GridView1.DataBind();


        grdsearch.DataSource = testDataSet.Tables[0];
        grdsearch.DataBind();
    }


    protected void btnDownload_Click(object sender, System.Web.UI.WebControls.CommandEventArgs e)
    {
        //Response.Write(e.CommandArgument.ToString());
        //Response.Write(e.CommandName.ToString()  );

        string queryString = e.CommandArgument.ToString();
        using (WebClient Client = new WebClient())
        {
            Client.DownloadFile("http://omm.local.com/Pages/cvsearch.aspx", "f:\\T\\cvsearch.aspx");
        }



        //Response.Redirect(queryString);

    } 
}
