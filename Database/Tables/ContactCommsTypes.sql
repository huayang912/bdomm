CREATE TABLE [dbo].[ContactCommsTypes](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) COLLATE Latin1_General_CI_AS NOT NULL,
	[NumberSuffix] [nvarchar](5) COLLATE Latin1_General_CI_AS NOT NULL
) ON [PRIMARY]