using System;

namespace App.Data
{
    /// <summary>
    /// For NullHandler related work
    /// </summary>
    public class NullHandler
    {
        #region CONSTRUCTOR(S)
        #endregion CONSTRUCTOR(S)
        /// <summary>
        /// Get string representation of an object. If null is found then empty string returned else string representation of input parameter is returned
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string GetString(object data)
        {
            return ((data == null) || (data == DBNull.Value)) ? string.Empty : data.ToString();
        }

        /// <summary>
        /// Gets the unique identifier.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public static Guid GetUniqueIdentifier(object data)
        {
            return ((data == null) || (data == DBNull.Value) || (data.ToString() == string.Empty)) ? Guid.Empty : new Guid(Convert.ToString(data));
        }

        /// <summary>
        /// Get datetime representation of an object. If null is found then min date returned else datetime representation of input parameter is returned
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static DateTime GetDateTime(object data)
        {
            return ((data == null) || (data == DBNull.Value)) ? DateTime.MinValue : Convert.ToDateTime(data);
        }

        /// <summary>
        /// Get int representation of an object. If null is found then zero returned else int representation of input parameter is returned
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static int GetInt32(object data)
        {
            return ((data == null) || (data == DBNull.Value) || (data.ToString() == string.Empty)) ? 0 : Convert.ToInt32(data);
        }

        /// <summary>
        /// Get int representation of an object. If null is found then zero returned else int representation of input parameter is returned
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static int GetInt(object data)
        {
            return GetInt32(data);
        }
        /// <summary>
        /// Get int representation of an object. If null is found then zero returned else int representation of input parameter is returned
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static long GetLong(object data)
        {
            return ((data == null) || (data == DBNull.Value) || (data.ToString() == string.Empty)) ? 0 : Convert.ToInt64(data);
        }

        /// <summary>
        /// Get byte representation of an object. If null is found then zero returned else int representation of input parameter is returned
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte GetByte(object data)
        {
            return ((data == null) || (data == DBNull.Value) || (data.ToString() == string.Empty)) ? (byte)0 : Convert.ToByte(data);
        }

        /// <summary>
        /// Get double representation of an object. If null is found then zero returned else double representation of input parameter is returned
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static double GetDouble(object data)
        {
            return ((data == null) || (data == DBNull.Value) || (data.ToString() == string.Empty)) ? 0 : Convert.ToDouble(data);
        }

        /// <summary>
        /// Get float representation of an object. If null is found then zero returned else float representation of input parameter is returned
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static float GetFloat(object data)
        {
            return ((data == null) || (data == DBNull.Value) || (data.ToString() == string.Empty)) ? 0 : Convert.ToSingle(data);
        }

        /// <summary>
        /// Get bool representation of an object. If null is found then false returned else bool representation of input parameter is returned
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool GetBoolean(object data)
        {
            return ((data == null) || (data == DBNull.Value)) ? false : Convert.ToBoolean(data);
        }

        /// <summary>
        /// Get binary/byte representation of an object. If null is found then false returned else byte representation of input parameter is returned
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] GetBytes(object data)
        {
            return ((data == null) || (data == DBNull.Value)) ? new byte[0] : (byte[])data;
        }

        /// <summary>
        /// Check if the parameter passed is valid date. If not then return db null value
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static object GetDBNullValue(DateTime date)
        {
            return (date == DateTime.MinValue) ? DBNull.Value : new object();
        }
        /// <summary>
        /// Check if the parameter passed is a null string. If it is a null or empty string then returns db null value
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        public static object GetDBNullValue(String text)
        {
            return String.IsNullOrEmpty(text) ? (object)DBNull.Value : text;
        }

        public static object GetDBNullValueForLong(long id)
        {
            return id <= 0 ? (object)DBNull.Value : id;
        }

        /// <summary>
        /// Check if the parameter passed is valid date. If not then return db null value
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static object GetDBNullValueCorrected(DateTime date)
        {
            return date == DateTime.MinValue ? (object)DBNull.Value : date;
        }

        /// <summary>
        /// Gets the enum.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The obj.</param>
        /// <returns></returns>
        public static T GetEnum<T>(object obj)
        {
            return (T)Enum.Parse(typeof(T), obj.ToString());
        }

        public static object GetObject(object obj)
        {
            return ((obj == null) || (obj == DBNull.Value) || (obj.ToString() == string.Empty)) ? null : obj;
        }

        public static Decimal GetDecimal(object data)
        {
            return ((data == null) || (data == DBNull.Value) || (data.ToString() == string.Empty)) ? 0 : Convert.ToDecimal(data);
        }
    }
}