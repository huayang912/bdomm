/****** Object:  Table [dbo].[UserInRole]    Script Date: 09/26/2010 00:05:11 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[UserInRole](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NULL,
	[RoleID] [int] NULL,
	[ModifiedBy] [int] NULL,
	[Modified] [datetime] NULL,
 CONSTRAINT [PK_UserInRole] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[UserInRole]  WITH CHECK ADD  CONSTRAINT [FK_UserInRole_Roles] FOREIGN KEY([RoleID])
REFERENCES [dbo].[Roles] ([ID])
GO

ALTER TABLE [dbo].[UserInRole] CHECK CONSTRAINT [FK_UserInRole_Roles]
GO

ALTER TABLE [dbo].[UserInRole]  WITH CHECK ADD  CONSTRAINT [FK_UserInRole_Users] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([ID])
GO

ALTER TABLE [dbo].[UserInRole] CHECK CONSTRAINT [FK_UserInRole_Users]
GO

ALTER TABLE [dbo].[UserInRole]  WITH CHECK ADD  CONSTRAINT [FK_UserInRole_Users1] FOREIGN KEY([ModifiedBy])
REFERENCES [dbo].[Users] ([ID])
GO

ALTER TABLE [dbo].[UserInRole] CHECK CONSTRAINT [FK_UserInRole_Users1]
GO

