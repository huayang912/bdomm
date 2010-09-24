using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace App.Core.Extensions
{
    public static class WebControlsExtensions
    {
        /// <summary>
        /// Sets a Selected Item of a DropDownlist Server Control according to value
        /// </summary>
        /// <param name="ddl"></param>
        /// <param name="selectedValue"></param>
        public static void SetSelectedItem(this System.Web.UI.WebControls.DropDownList ddl, String selectedValue)
        {
            if (ddl != null && ddl.Items.Count > 0)
            {
                ddl.ClearSelection();                
                foreach (System.Web.UI.WebControls.ListItem item in ddl.Items)
                {
                    if (String.Compare(item.Value, selectedValue, true) == 0)
                    {                        
                        item.Selected = true;
                        break;
                    }                    
                }                
            }
        }
        /// <summary>
        /// Sets a Selected Item of a RadioButtonList Server Control according to value
        /// </summary>
        /// <param name="ddl"></param>
        /// <param name="selectedValue"></param>
        public static void SetSelectedItem(this System.Web.UI.WebControls.RadioButtonList rdbl, String selectedValue)
        {
            if (rdbl != null && rdbl.Items.Count > 0)
            {
                rdbl.ClearSelection();
                foreach (System.Web.UI.WebControls.ListItem item in rdbl.Items)
                {
                    if (String.Compare(item.Value, selectedValue, true) == 0)
                    {
                        item.Selected = true;
                        break;
                    }
                }
            }
        }
        /// <summary>
        /// Checks Whether a DataSet contains any record or not.
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public static bool IsEmpty(this DataSet ds)
        {
            //return (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0);
            return (ds == null || ds.Tables.Count == 0);
        }
    }
}
