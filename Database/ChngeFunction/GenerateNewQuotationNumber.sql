set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
GO
ALTER FUNCTION [dbo].[GenerateNewQuotationNumber]
(
	@EnquiryID AS INT,
	@WithVersion AS BIT
) RETURNS NVARCHAR(50)
AS
BEGIN
	DECLARE @QuotationNumberMax AS INT
	DECLARE @VersionNumberMax AS INT
	DECLARE @QuotationNumber AS INT
	DECLARE @VersionNumber AS INT
	DECLARE @QuotationNumberString AS NVARCHAR(40)
	DECLARE @VersionNumberString AS NVARCHAR(40)
	DECLARE @PreviousQuotationNumber AS INT
	DECLARE @PreviousQuotationNumberCurrentYear AS INT
	DECLARE @QuotationNumberCurrentEnquiry AS NVARCHAR(50)
	DECLARE @PreviousVersionNumberCurrentEnquiry AS INT

	DECLARE @CurrentYear AS INT
	DECLARE @CurrentYearString AS NVARCHAR(4)
	DECLARE @SuffixString AS NVARCHAR(5)

	SET @QuotationNumberMax		                  = 9999	-- Maximum supported quotation number
	SET @VersionNumberMax		                  = 99		-- Maximum supported quotation version number

	SET @CurrentYear                              = YEAR(GETDATE())
	SET @CurrentYearString                        = RIGHT(CAST(@CurrentYear AS NVARCHAR), 2)

	SET @PreviousQuotationNumber = 
		(SELECT MAX(CAST(SUBSTRING([Number], 6, 4) AS INTEGER)) 
		 FROM [Quotations]
		 --Owner: Rabbani
		 --Date: 22-Jan-2011
		 --Purpose: Start quotation number from 0001 from every new year
		 
		 --New filtering option added
		 WHERE SUBSTRING([Number], 4, 2) = @CurrentYearString
		 
		 --End Rabbani
		 
		
		)
	
	SET @PreviousQuotationNumberCurrentYear = 
		(SELECT MAX(CAST(SUBSTRING([Number], 6, 4) AS INTEGER)) 
		 FROM [Quotations] 
		 WHERE SUBSTRING([Number], 4, 2) = @CurrentYearString)
	
	
	
	SET @QuotationNumberCurrentEnquiry = 
		(SELECT TOP 1 
			CASE 
				WHEN CHARINDEX('.', [Number]) = 0 THEN [Number] 
				ELSE LEFT([Number], CHARINDEX('.', [Number]) - 1) 
			END 
		 FROM [Quotations] 
		 WHERE EnquiryID = @EnquiryID)
	
	SET @PreviousVersionNumberCurrentEnquiry = 
		(SELECT MAX(CAST(RIGHT([Number], 2) AS INTEGER)) 
		 FROM [Quotations] 
		 WHERE CHARINDEX('.', [Number]) > 0 
		 AND EnquiryID = @EnquiryID)
	
	-- RULES FOR NUMBER GENERATION
	
	-- 1. IF NO QUOTATIONS EXIST FOR THE SUPPLIED ENQUIRY THEN GENERATE A NEW NUMBER
	-- 2. IF QUOTATIONS EXIST FOR THE SUPPLIED ENQUIRY THEN REUSE THE NUMBER
	-- 3. IF THE VERSION NUMBER FLAG IS ALSO SET, THEN ISSUE THE NEXT VERSION NUMBER

	IF @QuotationNumberCurrentEnquiry IS NOT NULL
	BEGIN		
		IF @WithVersion = 1
		BEGIN
			SET @VersionNumber = ISNULL(@PreviousVersionNumberCurrentEnquiry, -1) + 1 -- VERSION NUMBERS TO START FROM 0
		
			IF @VersionNumber > @VersionNumberMax RETURN NULL  -- CANNOT SUPPORT NUMBERS > MAX
		
			SET @VersionNumberString = CAST(@VersionNumber AS NVARCHAR)
		
			WHILE (LEN(@VersionNumberString) < 2)
			BEGIN
				SET @VersionNumberString = '0' + @VersionNumberString
			END

			RETURN RTRIM(@QuotationNumberCurrentEnquiry) + '.' + @VersionNumberString
		END
		ELSE
		BEGIN 
			RETURN RTRIM(@QuotationNumberCurrentEnquiry)
		END
		
	END


	-- RULES FOR NEW NUMBER GENERATION
	-- 1. NEW NUMBER = 1
	-- 2. IF QUOTATIONS EXIST THEN NEW NUMBER = 1 + LAST QUOTATION NUMBER
	-- 3. IF NO QUOTATIONS EXIST IN THE CURRENT YEAR AND SPECIFIC START SUPPLIED THEN NEW NUMBER = SPECIFIC NUMBER

	SET @QuotationNumber = 1

	IF @PreviousQuotationNumber IS NOT NULL SET @QuotationNumber = @PreviousQuotationNumber + 1

	IF @PreviousQuotationNumberCurrentYear IS NULL AND @CurrentYear = 2008 SET @QuotationNumber = 72
	IF @PreviousQuotationNumberCurrentYear IS NULL AND @CurrentYear = 2009 SET @QuotationNumber = 99
	
	IF @QuotationNumber > @QuotationNumberMax RETURN NULL		-- CANNOT SUPPORT NUMBERS > MAX	

	SET @SuffixString           = ISNULL((SELECT TOP 1 [NumberSuffix] FROM [EnquiryTypes] ET INNER JOIN [Enquiries] E ON ET.[ID] = E.[TypeID] WHERE E.[ID] = @EnquiryID), '')
	SET @CurrentYearString      = RIGHT(CAST((YEAR(GETDATE())) AS NVARCHAR), 2)
	
	SET @QuotationNumberString  = CAST(@QuotationNumber AS NVARCHAR)
	
	WHILE (LEN(@QuotationNumberString) < 4)
	BEGIN
		SET @QuotationNumberString = '0' + @QuotationNumberString
	END

	RETURN 'OMQ' + @CurrentYearString + @QuotationNumberString + @SuffixString
END


