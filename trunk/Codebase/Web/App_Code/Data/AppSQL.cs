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
}
