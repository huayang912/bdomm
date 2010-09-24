using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App.CustomModels
{
    public class CustomContact
    {
        public int ID { get; set; }
        public String ContactName { get; set; }
        public String ClientName { get; set; }
        public String JobTitle { get; set; }
        public String Country { get; set; }

    }
    public class CustomEnquiry
    {
        public int ID { get; set; }
        public int ContactID { get; set; }
        public int TypeID { get; set; }
        public String Details { get; set; }
        public int StatusID { get; set; }
    }
    public static class EnquiryStatus
    {
        public const int New = 1;
    }
}
