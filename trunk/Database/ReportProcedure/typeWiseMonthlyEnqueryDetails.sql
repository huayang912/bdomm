set ANSI_NULLS OFF
set QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[reportTypeWiseMOnthlyEnqueryDetails]
--	@intQryType INT = NULL,
--	@Year INT = NULL
		

AS
BEGIN	
	SELECT A.CreatedMonth,
	A.CreatedYear,
	A.EnqueryType,
	SUM(A.Outstanding) + SUM(A.Quoted) + SUM(A.Closed) AS [TotalSubmitted], 
	SUM(A.Outstanding) AS [Outstanding],
	SUM(A.Quoted) AS [Quoted],
	SUM(A.Closed) AS [Closed] 
	FROM (
	SELECT 
	MONTH(e.CreatedOn) AS [CreatedMonth],
	YEAR(e.CreatedOn) AS [CreatedYear],
	et.[Name] AS [EnqueryType],
	CASE WHEN e.StatusID=1 THEN 1 ELSE 0 END [Outstanding],
	CASE WHEN e.StatusID=2 THEN 1 ELSE 0 END [Quoted],
	CASE WHEN e.StatusID=3 THEN 1 ELSE 0 END [Closed]  
	FROM Enquiries e
	INNER JOIN EnquiryTypes et ON e.TypeID=et.ID
	) A
	GROUP BY A.CreatedMonth,A.CreatedYear,A.EnqueryType
	ORDER BY A.EnqueryType
END



