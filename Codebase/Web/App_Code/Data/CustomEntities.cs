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
    public class CustomQuotation
    {
        public int ID { get; set; }
        public int EnquiryID { get; set; }
        public int StatusID { get; set; }
        public int ValidityDays { get; set; }
        public String Number { get; set; }
        public String Subcontractor { get; set; }
        public String ScopeOfWork { get; set; }
        public String Scheduel { get; set; }
        public String MainEquipment { get; set; }
        public String SubmissionDate { get; set; }
        public String DecisionDate { get; set; }
    }
    public class CustomQuotationPricingLine
    {
        public int ID { get; set; }
        public int QuotationID { get; set; }        
        public String Item { get; set; }
        public String Description { get; set; }
        public int PricingTypeID { get; set; }
        public String PricingType { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }    
}
