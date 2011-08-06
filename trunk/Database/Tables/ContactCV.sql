
CREATE TABLE [dbo].[ContactCV](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ContactID] [int] NOT NULL,
	[FileName] [nvarchar](350) NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[ChangedBy] [int] NOT NULL,
	[ChangedOn] [datetime] NOT NULL,
 CONSTRAINT [PK_ContactCV] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[ContactCV]  WITH CHECK ADD  CONSTRAINT [FK_ContactCV_Contacts] FOREIGN KEY([ContactID])
REFERENCES [dbo].[Contacts] ([ID])
GO

ALTER TABLE [dbo].[ContactCV] CHECK CONSTRAINT [FK_ContactCV_Contacts]
GO
