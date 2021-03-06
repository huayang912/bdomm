set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
GO
ALTER FUNCTION [dbo].[GenerateNewProjectNumber]
(
) RETURNS NVARCHAR(50)
AS
BEGIN
	DECLARE @ProjectNumberMax AS INT
	DECLARE @ProjectNumberStart AS INT
	DECLARE @ProjectNumber AS INT
	DECLARE @ProjectNumberString AS NVARCHAR(40)
	DECLARE @CurrentYearString AS NVARCHAR(4)
	DECLARE @SuffixString AS NVARCHAR(5)
	
	--Owner: Rabbani
	--Date: 22-Jan-2011
	--Purpose: Start quotation number from 0001 from every new year
	
	--(This is modified code)
	SET @ProjectNumberStart = 1 
	
	-- (This is the previous code)
	-- SET @ProjectNumberStart = 65 
	
	--End Rabbani
	
	
	SET @ProjectNumberMax       = 9999
	
	SET @CurrentYearString      = RIGHT(CAST((YEAR(GETDATE())) AS NVARCHAR), 2)
	
	SET @ProjectNumber = 
		(
			SELECT ISNULL(1 + MAX(CAST(SUBSTRING([Number], 6, 4) AS INTEGER)), @ProjectNumberStart) 
			FROM [Projects]
			--Owner: Rabbani
			--Date: 22-Jan-2011
			--Purpose: Start quotation number from 0001 from every new year
			
			--New filtering option added
			WHERE SUBSTRING([Number], 4, 2) = @CurrentYearString
			
			--End Rabbani
		)

	IF @ProjectNumber > @ProjectNumberMax RETURN NULL		-- CANNOT SUPPORT NUMBERS > MAX	

	
	
	SET @ProjectNumberString    = CAST(@ProjectNumber AS NVARCHAR)
	
	WHILE (LEN(@ProjectNumberString) < 4)
	BEGIN
		SET @ProjectNumberString = '0' + @ProjectNumberString
	END
	
	
	RETURN 'OMP' + @CurrentYearString + @ProjectNumberString
END


