#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

#endregion

#region Class

/// <summary>
/// Summary description for BaseUserControl
/// </summary>
public class BaseUserControl : System.Web.UI.UserControl
{
    #region Constructor

    public BaseUserControl()
    {
        //
        // TODO: Add constructor logic here
        //
        WebUtil.LoginUser();

    }

    #endregion

    #region Properties

    //private Measure _measure;
    //public Measure measure
    //{
    //    get
    //    {
    //        return _measure;
    //    }
    //    set
    //    {
    //        _measure = value;
    //    }
    //}

    #endregion

    #region Methods

    /// <summary>
    /// Signouts the user.
    /// </summary>
    protected void SignoutUser()
    {
        WebUtil.SignoutUser();
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
