using System;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace App.Core.Extensions
{
    public static class StringExtensions
    {
        #region String Manipulation
        #region Constant Properties
        public static char CR
        {
            get { return '\r'; }
        }

        public static char NEWLINE
        {
            get { return '\n'; }
        }

        public static string ENTER
        {
            get { return CR + NEWLINE.ToString(); }
        }

        #endregion

        /// <summary>
        /// Toes the format.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <param name="args">The args.</param>
        /// <returns></returns>
        public static string ToFormat(this string expression, params object[] args)
        {
            return String.Format(expression, args);;
        }

        /// <summary>
        /// Formats a string to an invariant culture
        /// </summary>
        /// <param name="formatString">The format string.</param>
        /// <param name="objects">The objects.</param>
        /// <returns></returns>
        public static string ToInvariantFormat(this string formatString, params object[] objects)
        {
            return string.Format(CultureInfo.InvariantCulture, formatString, objects);
        }

        /// <summary>
        /// Formats a string to the current culture.
        /// </summary>
        /// <param name="formatString">The format string.</param>
        /// <param name="objects">The objects.</param>
        /// <returns></returns>
        public static string ToCurrentCultureFormat(this string formatString, params object[] objects)
        {
            return string.Format(CultureInfo.CurrentCulture, formatString, objects);
        }
        
        /// <summary>
        /// Promoted from PObjectFieldMetadata
        /// </summary>
        /// <param name="strArray"></param>
        /// <returns></returns>
        public static string[] TrimStringArray(this string[] strArray)
        {
            for (int i = 0; i < strArray.Length; i++)
            {
                strArray[i] = strArray[i].Trim();
            }
            return strArray;
        }

        /// <summary>
        /// Checks to see if a string is null or empty, including
        /// whether its full of only spaces
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string s)
        {
            return String.IsNullOrEmpty(s);
        }

        //private static Regex _isNumber = new Regex(@"^\d+$");
        ///// <summary>
        ///// Checks to see if a string is a number
        ///// </summary>
        ///// <param name="s"></param>
        ///// <returns></returns>
        //public static bool IsNumeric(this string s)
        //{
        //    return !string.IsNullOrEmpty(s) && _isNumber.Match(s).Success;
        //}

        /// <summary>
        /// Tries the parse.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <param name="b">if set to <c>true</c> [b].</param>
        /// <returns></returns>
        public static bool TryParse(this string s, out bool b)
        {
            int num;
            s = int.TryParse(s, out num) ? ((num == 0) ? "false" : "true") : s.ToLower();
            return bool.TryParse(s, out b);
        }

        /// <summary>
        /// Remove a list of unwanted characters from a string
        /// </summary>
        /// <param name="s"></param>
        /// <param name="chars"></param>
        /// <returns></returns>
        public static string RemoveCharacters(this string s, params string[] chars)
        {
            foreach (string c in chars.Where(c => s.Contains(c)))
            {
                s = s.Replace(c, "");
            }
            return s;
        }

        /// <summary>
        /// Returns the given potion of the string before the [search] string. 
        /// the full string [s] is returned if the [search] string is not found
        /// </summary>
        /// <param name="s">string to search</param>
        /// <param name="search"></param>
        /// <returns></returns>
        public static string SubStrBefore(this string s, string search)
        {
            return s.Contains(search) ? s.Substring(0, s.IndexOf(search)) : s;
        }

        /// <summary>
        /// Returns the given potion of the string after the [search] string. 
        /// an empty string [s] is returned if the [search] string is not found
        /// </summary>
        /// <param name="s">string to search</param>
        /// <param name="search"></param>
        /// <returns></returns>
        public static string SubStrAfter(this string s, string search)
        {
            return s.Contains(search) ? s.Substring(s.IndexOf(search) + search.Length) : string.Empty;
        }

        /// <summary>
        /// Returns the given potion of the string before the last occurrence of the [search] string. 
        /// the full string [s] is returned if the [search] string is not found
        /// </summary>
        /// <param name="s">string to search</param>
        /// <param name="search"></param>
        /// <returns></returns>
        public static string SubStrBeforeLast(this string s, string search)
        {
            return s.Contains(search) ? s.Substring(0, s.LastIndexOf(search)) : s;
        }

        /// <summary>
        /// Returns the given potion of the string after the last occurrence of the [search] string. 
        /// an empty string [s] is returned if the [search] string is not found
        /// </summary>
        /// <param name="s">string to search</param>
        /// <param name="search"></param>
        /// <returns></returns>
        public static string SubStrAfterLast(this string s, string search)
        {
            return s.Contains(search) ? s.Substring(s.LastIndexOf(search) + search.Length) : string.Empty;
        }

        /// <summary>
        /// HTMLs the encode.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns></returns>
        public static string HtmlEncode(this string s)
        {
            return HttpUtility.HtmlEncode(s);
        }

        /// <summary>
        /// HTMLs the decode.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns></returns>
        public static string HtmlDecode(this string s)
        {
            return HttpUtility.HtmlDecode(s);
        }

        /// <summary>
        /// Removes the non numeric.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns></returns>
        public static string RemoveNonNumeric(this string s)
        {
            if (!string.IsNullOrEmpty(s))
            {
                char[] result = new char[s.Length];
                int resultIndex = 0;
                foreach (char c in s.Where(c => char.IsNumber(c)))
                {
                    result[resultIndex++] = c;
                }
                if (0 == resultIndex)
                    s = string.Empty;
                else if (result.Length != resultIndex)
                    s = new string(result, 0, resultIndex);
            }
            return s;
        }

        private static readonly Regex _email = new Regex(@"([\w\.!#\$%\-+.]+@[A-Za-z0-9\-]+(\.[A-Za-z0-9\-]+)+)", RegexOptions.Compiled);
        private static readonly Regex _url = new Regex(@"(<\w+.*?>|[^=!:'""/]|^)((?:https?://)|(?:www\.))([-\w]+(?:\.[-\w]+)*(?::\d+)?(?:/(?:(?:[~\w\+@%-]|(?:[,.;:][^\s$]))+)?)*(?:\?[\w\+@%&=.;-]+)?(?:\#[\w\-]*)?)([[:punct:]]|\s|<|$)", RegexOptions.Compiled);

        /// <summary>
        /// Autoes the link.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="opts">The opts.</param>
        /// <param name="hrefattrs">The hrefattrs.</param>
        /// <returns></returns>
        public static string AutoLink(string text, AutoLinkOptions opts, string hrefattrs)
        {
            if (string.IsNullOrEmpty(text)) return text;

            switch (opts)
            {
                case AutoLinkOptions.All:
                    return AutoLinkEmailAddresses(AutoLinkUrls(text, hrefattrs));
                case AutoLinkOptions.Email:
                    return AutoLinkEmailAddresses(text);
                case AutoLinkOptions.Url:
                    return AutoLinkUrls(text, hrefattrs);
                default:
                    return text;
            }
        }

        /// <summary>
        /// Autoes the link email addresses.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        private static string AutoLinkEmailAddresses(string text)
        {
            return _email.Replace(text, x => Regex.IsMatch(text, @"<a\b[^>]*>(.*)(" + Regex.Escape(x.Groups[0].Value) + @")(.*)<\/a>") ? x.Groups[0].Value : String.Format("<a href=\"mailto:{0}\">{0}</a>", x.Groups[0].Value));
        }

        /// <summary>
        /// Autoes the link urls.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="hrefattrs">The hrefattrs.</param>
        /// <returns></returns>
        private static string AutoLinkUrls(string text, string hrefattrs)
        {
            return _url.Replace(text, x =>
            {
                if (Regex.IsMatch(x.Groups[1].Value, @"<a\s", RegexOptions.IgnoreCase))
                {
                    return x.Groups[0].Value;
                }

                string t = x.Groups[2].Value + x.Groups[3].Value;
                return String.Format("{0}<a href=\"{1}{2}\"{3}>{4}</a>{5}", x.Groups[1].Value,
                                        x.Groups[2].Value == "www." ? "http://www." : x.Groups[2].Value,
                                        x.Groups[3].Value,
                                        string.IsNullOrEmpty(hrefattrs) ? "" : " " + hrefattrs,
                                        t,
                                        x.Groups[4].Value);
            });
        }

        public enum AutoLinkOptions
        {
            Email,
            Url,
            All
        }
        #endregion

        #region Email 
        /// <summary>
        /// Determines whether [is valid email address] [the specified s].
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns>
        /// 	<c>true</c> if [is valid email address] [the specified s]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsValidEmailAddress(this string s)
        {
            return new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$").IsMatch(s);
        }
        #endregion

        /// <summary>
        /// Salts the and hash.
        /// </summary>
        /// <param name="rawString">The raw string.</param>
        /// <param name="salt">The salt.</param>
        /// <returns></returns>
        public static string SaltAndHash(string rawString, string salt)
        {
            byte[] salted = Encoding.UTF8.GetBytes(string.Concat(rawString, salt));
            byte[] hashed = new SHA256Managed().ComputeHash(salted);

            return Convert.ToBase64String(hashed);
        }
        /// <summary>
        /// Replaces All Html tags from a Html String
        /// </summary>
        /// <param name="htmlString"></param>
        /// <returns></returns>
        public static string StripHTML(this String htmlString)
        {
            String pattern = @"<(.|\n)*?>";
            return Regex.Replace(htmlString, pattern, string.Empty);
        }
        /// <summary>
        /// Gets word count of a given text. This is done using split methodology
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static int WordCountSplit(this String text)
        {
            return text.Split(new char[] { ' ', '.', '?' }, StringSplitOptions.RemoveEmptyEntries).Length;
        }
        /// <summary>
        /// Gets word count of a given text. This is done using regular expression
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static int WordCount(this String text)
        {
            MatchCollection collection = Regex.Matches(text, @"[\S]+");
            return collection.Count;
        }        
        /// <summary>
        /// Gets First Given Number of Words from a String
        /// </summary>
        /// <param name="text"></param>
        /// <param name="wordsToReturn"></param>
        /// <returns></returns>
        public static string GetWords(this String text, int wordCount)
        {
            wordCount = wordCount > 1 ? wordCount - 1 : wordCount;
            String[] words = text.Split(' ');
            StringBuilder sb = new StringBuilder(10);
            for (int i = 0; i < words.Length; i++)
            {
                if (i == 0)
                    sb.Append(words[i]);
                else
                    sb.AppendFormat(" {0}", words[i]);
                if (i >= wordCount)
                    break;
            }
            return sb.ToString();
            //return Regex.Match(text, @"(\w+\s+){40}").ToString().Trim(); 
        }
        /// <summary>
        /// Converts a String to Integer
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int ToInt(this String str)
        {
            int result = 0;
            int.TryParse(str, out result);
            return result;
        }
        public static long ToLong(this String str)
        {
            long result = 0;
            long.TryParse(str, out result);
            return result;
        }
    }
}
