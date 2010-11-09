ALTER TABLE dbo.Contacts ADD Coverall NVARCHAR(100) NULL
GO
ALTER TABLE dbo.Contacts ADD Boots int NULL

ALTER TABLE dbo.Contacts ADD employment_status NVARCHAR(100) NULL
ALTER TABLE dbo.Contacts ADD Insurance NVARCHAR(100) NULL

ALTER TABLE dbo.NextOfKin ADD email NVARCHAR(200) NULL

ALTER TABLE dbo.EmploymentHistory ADD OffshoreRate [smallmoney] NULL
ALTER TABLE dbo.EmploymentHistory ADD Office_Onsh_Rate_type  NVARCHAR(30) NULL
ALTER TABLE dbo.EmploymentHistory ADD OfficeOnshoreRate   [smallmoney] NULL

ALTER TABLE dbo.EmploymentHistory ADD Hour_Standby_Rate_type  NVARCHAR(30) NULL
ALTER TABLE dbo.EmploymentHistory ADD HourStandbyRate   [smallmoney] NULL

