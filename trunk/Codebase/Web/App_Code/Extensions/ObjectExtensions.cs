using System.Collections;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;
using System.Collections.Generic;

namespace App.Core.Extensions
{
    public static class ObjectExtensions
    {
        private static readonly Regex _isNumber = new Regex(@"^\d+$");

        /// <summary>
        /// Determines whether the specified obj is numeric.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns>
        /// 	<c>true</c> if the specified obj is numeric; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNumeric(this object obj)
        {
            return obj != null && _isNumber.Match(obj.ToString()).Success;
        }

        /// <summary>
        /// Toes the JSON.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns></returns>
        public static string ToJSON(this object obj)
        {
            return new JavaScriptSerializer().Serialize(obj);
        }

        /// <summary>
        /// Toes the JSON.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="recursionDepth">The recursion depth.</param>
        /// <returns></returns>
        public static string ToJSON(this object obj, int recursionDepth)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer {RecursionLimit = recursionDepth};
            return serializer.Serialize(obj);
        }

        /// <summary>
        /// Ins the specified obj.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="enumerable">The enumerable.</param>
        /// <returns></returns>
        public static bool In(this object obj, IEnumerable enumerable)
        {
            return enumerable.Cast<object>().Contains(obj);
        }

        /// <summary>
        /// Ins the specified obj.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="enumerable">The enumerable.</param>
        /// <returns></returns>
        public static bool In<T>(this object obj, IEnumerable<T> enumerable)
        {
            return obj is T && enumerable.Contains((T) obj);
        }

        /// <summary>
        /// Determines whether the specified o is null.
        /// </summary>
        /// <param name="o">The o.</param>
        /// <returns>
        /// 	<c>true</c> if the specified o is null; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNull(this object o)
        {
            return o == null;
        }

        /// <summary>
        /// Determines whether [is not null] [the specified o].
        /// </summary>
        /// <param name="o">The o.</param>
        /// <returns>
        /// 	<c>true</c> if [is not null] [the specified o]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNotNull(this object o)
        {
            return o != null;
        }

        /// <summary>
        /// Returns all the public properties as a list of Name, Value pairs
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static IEnumerable<KeyValuePair<string, object>> GetProperties(this object item)
        {
            foreach (PropertyInfo property in item.GetType().GetProperties())
            {
                yield return new KeyValuePair<string, object>(property.Name, property.GetValue(item, null));
            }
        }
    }
}