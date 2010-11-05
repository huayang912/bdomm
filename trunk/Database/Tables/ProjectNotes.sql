/****** Object:  Table [dbo].[ProjectNotes]    Script Date: 10/29/2010 00:18:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ProjectNotes](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[ProjectID] [int] NOT NULL,
	[Details] [varchar](4000) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_ProjectNotes] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER TABLE [dbo].[ProjectNotes]  WITH CHECK ADD  CONSTRAINT [FK_ProjectNotes_Project] FOREIGN KEY([ProjectID])
REFERENCES [dbo].[Projects] ([ID])
GO
ALTER TABLE [dbo].[ProjectNotes] CHECK CONSTRAINT [FK_ProjectNotes_Project]

GO
ALTER TABLE ProjectNotes ALTER COLUMN Details VARCHAR(MAX) NOT NULL