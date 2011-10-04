using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

/// <summary>
/// Summary description for BindDropdownList
/// </summary>
public static class BindDropdownList
{
    private static void InsertBlankOption(DropDownList ddl)
    {
        ddl.Items.Insert(0, new ListItem(String.Empty, String.Empty));
    }
    public static void Countries(DropDownList ddl)
    {
        OMMDataContext context = new OMMDataContext();
        var country = from P in context.Countries orderby P.Name select new { P.ID, P.Name };
        ddl.DataSource = country;
        ddl.DataTextField = "Name";
        ddl.DataValueField = "ID";
        ddl.DataBind();
        InsertBlankOption(ddl);
    }
    public static void Currencies(DropDownList ddl)
    {
        OMMDataContext context = new OMMDataContext();
        var currencies = from P in context.Currencies select new { P.ID, P.ShortCode };
        ddl.DataSource = currencies;
        ddl.DataTextField = "ShortCode";
        ddl.DataValueField = "ID";
        ddl.DataBind();
        //InsertBlankOption(ddl);
    }

    public static void Currencies_EmpHistory(DropDownList ddl)
    {
        OMMDataContext context = new OMMDataContext();
        var currencies = from P in context.Currencies select new { P.ID, P.ShortCode };
        ddl.DataSource = currencies;
        ddl.DataTextField = "ShortCode";
        ddl.DataValueField = "ID";
        ddl.DataBind();
        InsertBlankOption(ddl);
    }

    public static void MaritalStatuses(DropDownList ddl)
    {
        OMMDataContext context = new OMMDataContext();
        var statuses = from P in context.MaritalStatuses select P;
        ddl.DataSource = statuses;
        ddl.DataTextField = "Name";
        ddl.DataValueField = "ID";
        ddl.DataBind();
        //InsertBlankOption(ddl);
    }
    public static void Clients(DropDownList ddl)
    {
        OMMDataContext context = new OMMDataContext();
        var statuses = from P in context.Clients orderby P.Name select new { ID = P.ID, Name = P.Name };
        ddl.DataSource = statuses;
        ddl.DataTextField = "Name";
        ddl.DataValueField = "ID";
        ddl.DataBind();
        InsertBlankOption(ddl);
    }
    public static void Projects(DropDownList ddl)
    {
        OMMDataContext context = new OMMDataContext();
        var statuses = from P in context.Projects orderby P.Name select new { ID = P.ID, Name = P.Name };
        ddl.DataSource = statuses;
        ddl.DataTextField = "Name";
        ddl.DataValueField = "ID";
        ddl.DataBind();
        InsertBlankOption(ddl);
    }
    public static void Roles(DropDownList ddl)
    {
        OMMDataContext context = new OMMDataContext();
        var statuses = from P in context.Roles orderby P.Name select new { ID = P.ID, Name = P.Name };
        ddl.DataSource = statuses;
        ddl.DataTextField = "Name";
        ddl.DataValueField = "ID";
        ddl.DataBind();
        InsertBlankOption(ddl);
    }

    public static void RolesNoBlank(DropDownList ddl)
    {
        OMMDataContext context = new OMMDataContext();
        var statuses = from P in context.Roles orderby P.Name select new { ID = P.ID, Name = P.Name };
        ddl.DataSource = statuses;
        ddl.DataTextField = "Name";
        ddl.DataValueField = "ID";
        ddl.DataBind();
        //InsertBlankOption(ddl);
    }

    public static void CertificateTypes(DropDownList ddl)
    {
        OMMDataContext context = new OMMDataContext();
        var certificateType = from P in context.CertificateTypes orderby P.Name select new { P.ID, P.Name };
        ddl.DataSource = certificateType;
        ddl.DataTextField = "Name";
        ddl.DataValueField = "ID";
        ddl.DataBind();
        InsertBlankOption(ddl);
    }

    public static void CommunicationTypes(DropDownList ddl)
    {
        OMMDataContext context = new OMMDataContext();
        var communicationType = from P in context.ContactCommsTypes orderby P.Name select new { P.ID, P.Name };
        ddl.DataSource = communicationType;
        ddl.DataTextField = "Name";
        ddl.DataValueField = "ID";
        ddl.DataBind();
        InsertBlankOption(ddl);
    }
}
