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
        public int SourceTypeID { get; set; }
        public String EnguirySubject { get; set; }
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
        public int CurrencyID { get; set; }
        public int ProjectYear { get; set; }
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
    public class CustomProject
    {
        public int ID { get; set; }
        public int QuotationID { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }
        public String StartDate { get; set; }
        public String EndDate { get; set; }
    }
    public class CustomProjectNote
    {
        public long ID { get; set; }
        public int ProjectID { get; set; }
        public String Details { get; set; }
    }
    /// <summary>
    /// This is Used to Return the Send SMS Process Reply to the Client [Client Browser] through a AJAX Call    
    /// </summary>
    public class SendSmsStatus
    {
        /// <summary>
        /// StatusID == 1   > Successfully Sent SMS
        /// StatusID == -1  > Unable to Send SMS. 
        /// </summary>
        public int StatusID { get; set; }
        public String Message { get; set; }
    }
    public class AjaxStatus
    {
        /// <summary>
        /// StatusID == 1   > Successfully Sent SMS
        /// StatusID == -1  > Unable to Send SMS. 
        /// </summary>
        public int StatusID { get; set; }
        public String Message { get; set; }
    }
    #region Enum Like Classes
    public static class EnquiryStatus
    {
        public const int New = 1;  ///Outstanding in the Database
        public const int Quoted = 2;
        public const int Closed = 3;
    }
    public static class QuotationStatus
    {
        public const int NotSubmitted = 1;
        public const int Submitted = 2;
        public const int Unsuccessful = 3;
        public const int Successful = 4;
        public const int ReQquoteRequested = 5;
        public const int Revised = 6;
        public const int Closed = 7;
    }
    public static class ProjectStatus
    {
        public const int InProgress = 1;
        public const int Completed = 2;
        public const int Closed = 3;
    }
    #endregion

    public class StartsWith
    {
        public String Start
        {
            get;
            set;
        }

        public static List<StartsWith> GetStartsWith()
        {
            List<StartsWith> chars = new List<StartsWith>
            {
                new StartsWith(){Start = "A"},
                new StartsWith(){Start = "B"},
                new StartsWith(){Start = "C"},
                new StartsWith(){Start = "D"},
                new StartsWith(){Start = "E"},
                new StartsWith(){Start = "F"},
                new StartsWith(){Start = "G"},
                new StartsWith(){Start = "H"},
                new StartsWith(){Start = "I"},
                new StartsWith(){Start = "J"},
                new StartsWith(){Start = "K"},
                new StartsWith(){Start = "L"},
                new StartsWith(){Start = "M"},
                new StartsWith(){Start = "N"},
                new StartsWith(){Start = "O"},
                new StartsWith(){Start = "P"},
                new StartsWith(){Start = "Q"},
                new StartsWith(){Start = "R"},
                new StartsWith(){Start = "S"},
                new StartsWith(){Start = "T"},
                new StartsWith(){Start = "U"},
                new StartsWith(){Start = "V"},
                new StartsWith(){Start = "W"},
                new StartsWith(){Start = "X"},
                new StartsWith(){Start = "Y"},
                new StartsWith(){Start = "Z"}
            };
            //rptStartsWith.DataSource = chars;
            //rptStartsWith.DataBind();
            return chars;
        }
    }

    public class Personnel
    {
        public int ID { get; set; }
        public String FirstName {get;set; }
        public String LastName { get; set; }
        public String Address { get; set; }
        public String PostCode { get; set; }
        public int CountryID { get; set; }
        public int MaritalStatus { get; set; }
        public String PlaceOfBirth { get; set; }
        public String CountryOfBirthID { get; set; }
        public String DateOfBirth { get; set; }
        public String DateOfLastMeeting { get; set; }
        public decimal? PreferredDayRate { get; set; }
        public int DayRateCurrencyID { get; set; }
        public bool NoSMSOrEmail { get; set; }
        public bool InActive { get; set; }
        public String PPE_Sizes { get; set; }
        public String Coverall { get; set; }
        public int Boots { get; set; }


        public String companyname { get; set; }
        public String companyreg { get; set; } 
        public String companyvat { get; set; } 
        public String companyaddr { get; set; } 
        public String employmentstatus { get; set; } 
        public String insurance { get; set; } 



    }

    public class PersonnelTelephone
    {
        public int ID { get; set; }
        public String Number { get; set; }
        public int TypeID { get; set; }
    }

    public class ConNote
    {
        public int ID { get; set; }
        public String Notes { get; set; }
        public String CommsTypeID { get; set; }
    }

    public class PersonnelEmail
    {
        public int ID { get; set; }
        public String Email { get; set; }
    }
    public class PersonnelRole
    {
        public int ID { get; set; }
        public int Order { get; set; }
        public int RoleID { get; set; }
    }
}
