set ANSI_NULLS OFF
set QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[reportMonthlyEnquery]
--	@intQryType INT = NULL,
--	@Year INT = NULL
		

AS
BEGIN	
	SELECT 
	a.[MonthName] AS [MonthName],
	A.[Year] AS [Year],
	SUM(A.Outstanding)+SUM(A.[Quoted])+SUM(A.Closed) AS [TotalInitiated], 
	SUM(A.Outstanding) AS Outstanding,
	SUM(A.[Quoted]) AS [Quoted],
	SUM(A.Closed) AS [Closed] 
	FROM
	(
	SELECT 
	MONTH(e.CreatedOn) AS [ToOrder],
	YEAR(e.CreatedOn) AS [Year],
	CASE 
		WHEN MONTH(e.CreatedOn)=1 THEN 'January'
		WHEN MONTH(e.CreatedOn)=2 THEN 'February'
		WHEN MONTH(e.CreatedOn)=3 THEN 'March'
		WHEN MONTH(e.CreatedOn)=4 THEN 'April'
		WHEN MONTH(e.CreatedOn)=5 THEN 'May'
		WHEN MONTH(e.CreatedOn)=6 THEN 'June'
		WHEN MONTH(e.CreatedOn)=7 THEN 'July'
		WHEN MONTH(e.CreatedOn)=8 THEN 'August'
		WHEN MONTH(e.CreatedOn)=9 THEN 'September'
		WHEN MONTH(e.CreatedOn)=10 THEN 'October'
		WHEN MONTH(e.CreatedOn)=11 THEN 'November'
		WHEN MONTH(e.CreatedOn)=12 THEN 'December'
	END [MonthName]
	,
	CASE 
		WHEN e.StatusID=1 THEN 1 ELSE 0	
	END [Outstanding]
	,
	CASE 
		WHEN e.StatusID=2 THEN 1 ELSE 0	
	END [Quoted]
	,
	CASE 
		WHEN e.StatusID=3 THEN 1 ELSE 0	
	END [Closed] 
	FROM Enquiries e
	--WHERE YEAR(e.CreatedOn)=2009
	) A
	GROUP BY A.[MonthName],A.ToOrder,A.[Year]
	ORDER BY A.ToOrder
END

