
CREATE TABLE [dbo].[ContactCommsTypes](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[NumberSuffix] [nvarchar](5) NOT NULL
)
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ContactsCommsNotes](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ContactID] [int] NOT NULL,
    [ContactCommsTypeID] [int] NULL,
	[Notes] [nvarchar](2000) COLLATE Latin1_General_CI_AS NULL,
	[ChangedByUserID] [int] NULL,
	[ChangedOn] [datetime] NOT NULL CONSTRAINT [DF_ContactsCommsNotes_ChangedOn]  DEFAULT (getdate()),
	[Version] [timestamp] NOT NULL,
	[CreatedByUsername] [nvarchar](50) COLLATE Latin1_General_CI_AS NULL,
	[ChangedByUsername] [nvarchar](50) COLLATE Latin1_General_CI_AS NULL,
 CONSTRAINT [PK_ContactsCommsNotes] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[ContactsCommsNotes]  WITH CHECK ADD  CONSTRAINT [FK_ContactsCommsNotes_Contacts] FOREIGN KEY([ContactID])
REFERENCES [dbo].[Contacts] ([ID])
GO
ALTER TABLE [dbo].[ContactsCommsNotes]  WITH CHECK ADD  CONSTRAINT [FK_ContactsCommsNotes_ContactCommsTypes] FOREIGN KEY([ContactCommsTypeID])
REFERENCES [dbo].[ContactCommsTypes] ([ID])
GO
ALTER TABLE [dbo].[ContactsCommsNotes] CHECK CONSTRAINT [FK_ContactsCommsNotes_Contacts]
GO
ALTER TABLE [dbo].[ContactsCommsNotes]  WITH CHECK ADD  CONSTRAINT [FK_ContactsCommsNotes_Users] FOREIGN KEY([ChangedByUserID])
REFERENCES [dbo].[Users] ([ID])
GO
ALTER TABLE [dbo].[ContactsCommsNotes] CHECK CONSTRAINT [FK_ContactsCommsNotes_Users]