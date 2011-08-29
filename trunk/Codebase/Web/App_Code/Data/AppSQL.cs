using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for AppSQL
/// </summary>
public class AppSQL
{
    public const String GET_NOTES_BY_CONTACT = @"
        WITH PersonnelNotes AS
        (
            SELECT ID
	            , ContactID
	            , Notes AS Note 	
	            , ROW_NUMBER() OVER(ORDER BY ChangedOn DESC) AS RowNumber	
            FROM ContactsNotes
            WHERE ContactID = @ContactID
        )
        SELECT ID
            , ContactID	
            , Note
            , (SELECT COUNT(*) FROM PersonnelNotes) AS TotalRecord
        FROM PersonnelNotes
        WHERE RowNumber BETWEEN {0} AND {1}";

    public const String GET_EMPLOYMENT_HISTORY_BY_CONTACT = @"
        SELECT E.ID
            , E.ContactID
	        , E.StartDate AS [Start Date]
	        , E.EndDate AS [End Date]
	        , Projects.Name AS [Project]
	        --, (Contacts.FirstNames + Contacts.LastName) AS [Contact]
	        , Clients.Name AS [Client]
	        , Roles.Name AS [Role]			
	    FROM EmploymentHistory E
	        LEFT JOIN Contacts ON Contacts.ID = E.ContactID
	        LEFT JOIN Clients ON Clients.ID = E.ClientID
	        LEFT JOIN Projects ON Projects.ID = E.ProjectID
	        LEFT JOIN Roles ON Roles.ID = E.RoleID
	    WHERE E.ContactID = @ContactID";

    public const String GET_BANK_DETAILS_BY_CONTACT = @"
        SELECT 
        bd.ID,
        bd.ContactID,
        bd.BankName,
        bd.BranchName,
        bd.BranchAddress,
        bd.SortCode,
        bd.AccountNumber,
        bd.AccountName,
        bd.BicCode,
        bd.AbaCode 
        FROM BankDetails bd
        WHERE bd.ContactID = @ContactID";

    public const String GET_CRETIFICATION_DETAILS_BY_CONTACT = @"
        SELECT ct.[Name] AS [Type Name], 
        c.ID,
        c.ContactID,
        c.TypeID,
        c.Details,
        c.ExpiryDate,
        c.PlaceIssued 
        FROM Certificates c
        INNER JOIN CertificateTypes ct 
        ON ct.ID = c.TypeID
        WHERE c.ContactID = @ContactID";

    public const String GET_PASSPORT_DETAILS_BY_CONTACT = @"
        SELECT p.ID,
        p.ContactID,
        p.Number,
        p.WhereIssued,
        p.ExpiryDate,
        p.Nationality
        FROM Passports p 
        WHERE p.ContactID =  @ContactID";

    public const String GET_VISA_DETAILS_BY_CONTACT = @"
        SELECT v.ID,
        v.ContactID,
        v.CountryID,
        c.[Name],
        v.VisaType,
        v.ExpiryDate 
        FROM Visas v 
        INNER JOIN Countries c ON c.ID = v.CountryID
        WHERE v.ContactID =  @ContactID";


    public const String GET_NEXT_OF_KIN_BY_CONTACT = @"
        SELECT nok.ID,
        nok.ContactID,
        nok.[Name],
        nok.Relationship,
        nok.[Address],
        nok.Postcode,
        c.[Name] AS [Country],
        nok.HomeNumber,
        nok.WorkNumber,
        nok.MobileNumber
        FROM NextOfKin nok
        INNER JOIN Countries c ON c.ID = nok.CountryID
        WHERE nok.ContactID =  @ContactID";
}
