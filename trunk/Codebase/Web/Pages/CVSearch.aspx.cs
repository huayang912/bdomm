using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Net;
using System.IO;
using App.Core.Extensions;

using App.Data;

public partial class Pages_CVSearch : BasePage
{            
    protected void Page_Load(object sender, EventArgs e)
    {
        BindPageInfo();
    }
    protected void BindPageInfo()
    {
        Page.Title = WebUtil.GetPageTitle("CV Search");
    }
    protected String GetRelativeDownloadUrl(String path)
    {
        if(rdbPersonnelCV.Checked)
            return String.Format("{0}/{1}", AppConstants.PERSONNEL_CV_DIRECTORY, Path.GetFileName(path));
        else
            return String.Format("{0}/{1}", ConfigReader.CVBankDirectory  , Path.GetFileName(path));
    }
    protected void SearchCV()
    {
        grdsearch.DataSource = null;
        grdsearch.DataBind();


        //Dim strCatalog As String
        string strCatalog;

        //' Catalog Name
        //strCatalog = "TestCatalog"
        if (rdbPersonnelCV.Checked)
            strCatalog = ConfigReader.PersonnelCatelogName.ToString();
        else
            strCatalog = ConfigReader.CVBankCatelogName.ToString();

        //Dim strQuery As String
        string strQuery;
        //strQuery = "Select DocTitle,Filename,Size,PATH,URL from SCOPE() where FREETEXT('" & TextBox1.Text & "')".... 
        strQuery = "Select Contents,DocTitle, Filename, Size, PATH, URL,characterization "+
            " from SCOPE() WHERE FREETEXT(Contents,'" + txtKeyword.Text.ToSqlSafeData() + "')";
        //strQuery = "select doctitle, filename, vpath, rank, characterization from scope() where FREETEXT(Contents, '" + txtKeyword.Text.ToSqlSafeData() + "') order by rank desc ";
        
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

        DataTable detTable = new DataTable();
        detTable = getDetailsInformation(testDataSet.Tables[0]);

        grdsearch.DataSource = detTable;
        grdsearch.DataBind();

    }

    public DataTable getDetailsInformation(DataTable dt)
    {
        //Create a DataTable instance

        DataTable table = new DataTable();

        //Create 7 columns for this DataTable

        DataColumn col1 = new DataColumn("Filename");
        DataColumn col2 = new DataColumn("characterization");
        DataColumn col3 = new DataColumn("Size");
        DataColumn col4 = new DataColumn("FirstNames");
        DataColumn col5 = new DataColumn("LastName");
        DataColumn col6 = new DataColumn("Address");
        DataColumn col7 = new DataColumn("PATH");

        //Define DataType of the Columns

        col1.DataType = System.Type.GetType("System.String");
        col2.DataType = System.Type.GetType("System.String");
        col3.DataType = System.Type.GetType("System.String");
        col4.DataType = System.Type.GetType("System.String");
        col5.DataType = System.Type.GetType("System.String");
        col6.DataType = System.Type.GetType("System.String");
        col7.DataType = System.Type.GetType("System.String");

        //Add All These Columns into DataTable table

        table.Columns.Add(col1);
        table.Columns.Add(col2);
        table.Columns.Add(col3);
        table.Columns.Add(col4);
        table.Columns.Add(col5);
        table.Columns.Add(col6);
        table.Columns.Add(col7);

        //Create a Row in the DataTable table

        int loopCount = 0;
        while (loopCount < dt.Rows.Count)
        {
            string[] PersonenlID = dt.Rows[loopCount]["Filename"].ToString().Split(new Char[] { '_' });
            
            UtilityDAO dao = new UtilityDAO();
            DbParameter[] parameters = new[] { new DbParameter("@ContactID", PersonenlID[0].ToString().Trim()) };
            
            
            //DataSet ds = dao.GetPagedData(AppSQL.GET_BANK_DETAILS_BY_CONTACT, parameters, pageNumber, PAGE_SIZE, out totalRecord);
            DataSet ds = dao.GetDataSet(AppSQL.GET_PERSONNEL_DETAILS_BY_CONTACT, parameters, false);

            //Bind the List Control
            //ucBankList.DataSource = ds.Tables[0];


            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = table.NewRow();

                //Fill All Columns with Data

                row[col1] = dt.Rows[loopCount]["Filename"].ToString();
                row[col2] = dt.Rows[loopCount]["characterization"].ToString();
                row[col3] = dt.Rows[loopCount]["Size"].ToString();
                row[col4] = ds.Tables[0].Rows[0]["FirstNames"].ToString();
                row[col5] = ds.Tables[0].Rows[0]["LastName"].ToString();
                row[col6] = ds.Tables[0].Rows[0]["Address"].ToString();
                row[col7] = dt.Rows[loopCount]["PATH"].ToString();

                //Add the Row into DataTable

                table.Rows.Add(row);

            }

            loopCount++;
        }

        

        return table;
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            SearchCV();
        }        
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
