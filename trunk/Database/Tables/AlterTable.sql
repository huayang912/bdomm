ALTER TABLE dbo.Users ADD Email NVARCHAR(100) NULL
GO
ALTER TABLE dbo.Users ADD Password NVARCHAR(50) NULL
GO
ALTER TABLE dbo.Users ADD ModifiedBy int NULL
GO
ALTER TABLE dbo.Users ADD Modified datetime NULL
GO
ALTER TABLE Users Add [UserNameWeb] VARCHAR(100) NULL
