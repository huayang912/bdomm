#region References

using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Data;
using System.IO;


#endregion

#region Class

/// <summary>
/// Summary description for BasePage
/// </summary>
public class BasePage : System.Web.UI.Page
{
    //private bool terminateEvents = false;

    public BasePage()
    {
        //
        // TODO: Add constructor logic here
        //
        WebUtil.LoginUser();
    }

    #region Overridden Page load event
    /// <summary>
    /// Handles page load event
    /// </summary>
    /// <param name="e"></param>
    protected override void OnLoad(EventArgs e)
    {
        WebUtil.LoginUser();
        base.OnLoad(e);
    }
    #endregion

    #region ViewState Compression and Decompresson
    /// <summary>
    /// Load and Decompress ViewState
    /// </summary>
    /// <returns></returns>
    protected override object LoadPageStateFromPersistenceMedium()
    {
        //return base.LoadPageStateFromPersistenceMedium();
        string viewState = Request.Form["__VSTATE"];
        byte[] bytes = Convert.FromBase64String(viewState);
        bytes = Compressor.Decompress(bytes);
        LosFormatter formatter = new LosFormatter();
        return formatter.Deserialize(Convert.ToBase64String(bytes));
    }
    /// <summary>
    /// Compress and Save ViewState
    /// </summary>
    /// <param name="viewState"></param>
    protected override void SavePageStateToPersistenceMedium(object viewState)
    {
        //base.SavePageStateToPersistenceMedium(state);
        LosFormatter formatter = new LosFormatter();
        StringWriter writer = new StringWriter();
        formatter.Serialize(writer, viewState);
        string viewStateString = writer.ToString();
        byte[] bytes = Convert.FromBase64String(viewStateString);
        bytes = Compressor.Compress(bytes);
        ClientScript.RegisterHiddenField("__VSTATE", Convert.ToBase64String(bytes));
    }
    #endregion

    #region Signout user
    /// <summary>
    /// Signouts the user.
    /// </summary>
    protected void SignoutUser()
    {
        WebUtil.SignoutUser();
    }
    #endregion
    #region Properties
    public bool IsAdministrator
    {
        get
        {
            //if (SessionCache.CurrentUser != null)
            //    return SessionCache.CurrentUser.IsAdmin;
            
            return false;
        }
    }
    #endregion
    #region User Permission
    /// <summary>
    /// Checks Whether the Currentyl Logged In User Has Delete Permission
    /// </summary>
    protected bool HasDeletePermission
    {
        get
        {
            return true;
        }
    }
    /// <summary>
    /// Checks Whether the Currently Logged In User has Edit Permission
    /// </summary>
    protected bool HasEditPermission
    {
        get
        {
            return true;
        }
    }
    #endregion
}

#endregion