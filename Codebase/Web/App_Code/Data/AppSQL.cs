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
}
