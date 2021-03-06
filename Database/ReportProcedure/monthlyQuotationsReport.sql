set ANSI_NULLS OFF
set QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[reportMonthlyQuotations]
--	@intQryType INT = NULL,
--	@Year INT = NULL
		

AS
BEGIN	
	SELECT 
	a.[MonthName] AS [MonthName],
	A.[Year] AS [Year],
	SUM(A.NotSubmitted)+SUM(A.Submitted)+SUM(A.Unsuccessful) 
	+SUM(A.Successful)+SUM(A.[Re-quote Requested])+SUM(A.Revised)
	+ SUM(A.Closed) AS [TotalInitiated], 
	SUM(A.NotSubmitted) AS NotSubmitted,
	SUM(A.Submitted) AS [Submitted],
	SUM(A.Unsuccessful) AS [Unsuccessful],
	SUM(A.Successful) AS [Successful] ,
	SUM(A.[Re-quote Requested]) AS [RequoteRequested],
	SUM(A.Revised) AS [Revised],
	SUM(A.Closed) AS [Closed]
	FROM
	(
	SELECT 
	MONTH(q.CreatedOn) AS [ToOrder],
	YEAR(q.CreatedOn) AS [Year],
	CASE 
		WHEN MONTH(q.CreatedOn)=1 THEN 'January'
		WHEN MONTH(q.CreatedOn)=2 THEN 'February'
		WHEN MONTH(q.CreatedOn)=3 THEN 'March'
		WHEN MONTH(q.CreatedOn)=4 THEN 'April'
		WHEN MONTH(q.CreatedOn)=5 THEN 'May'
		WHEN MONTH(q.CreatedOn)=6 THEN 'June'
		WHEN MONTH(q.CreatedOn)=7 THEN 'July'
		WHEN MONTH(q.CreatedOn)=8 THEN 'August'
		WHEN MONTH(q.CreatedOn)=9 THEN 'September'
		WHEN MONTH(q.CreatedOn)=10 THEN 'October'
		WHEN MONTH(q.CreatedOn)=11 THEN 'November'
		WHEN MONTH(q.CreatedOn)=12 THEN 'December'
	END [MonthName]
	,
	CASE 
		WHEN q.StatusID=1 THEN 1 ELSE 0	
	END [NotSubmitted]
	,
	CASE 
		WHEN q.StatusID=2 THEN 1 ELSE 0	
	END [Submitted]
	,
	CASE 
		WHEN q.StatusID=3 THEN 1 ELSE 0	
	END [Unsuccessful] 
	,
	CASE 
		WHEN q.StatusID=4 THEN 1 ELSE 0	
	END [Successful] 
	,
	CASE 
		WHEN q.StatusID=5 THEN 1 ELSE 0	
	END [Re-quote Requested] 
	,
	CASE 
		WHEN q.StatusID=6 THEN 1 ELSE 0	
	END [Revised] 
	,
	CASE 
		WHEN q.StatusID=7 THEN 1 ELSE 0	
	END [Closed] 
	
	FROM Quotations q
	--WHERE YEAR(e.CreatedOn)=2009
	) A
	GROUP BY A.[MonthName],A.ToOrder,A.[Year]
	ORDER BY A.ToOrder
END


