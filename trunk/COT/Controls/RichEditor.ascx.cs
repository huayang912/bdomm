using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;



[BUDI.Web.AquariumFieldEditor()]
public partial class Controls_RichEditor : System.Web.UI.UserControl
{
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!(IsPostBack))
        	Page.ClientScript.RegisterClientScriptBlock(GetType(), "ClientScripts", String.Format("\n                                    function FieldEditor_GetValue(){{return $fin" +
                        "d(\'{0}\').get_content();}}\nfunction FieldEditor_SetValue(value) {{$find(\'{0}\').se" +
                        "t_content(value);}}\n                                  ", Controls[0].ClientID), true);
    }
}
