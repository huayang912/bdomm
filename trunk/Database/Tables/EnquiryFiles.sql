/****** Object:  Table [dbo].[EnquiryFiles]    Script Date: 10/30/2010 03:07:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[EnquiryFiles](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[EnquiryID] [int] NOT NULL,
	[FileName] [varchar](350) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[UploadedBy] [int] NOT NULL,
	[UploadedOn] [datetime] NOT NULL,
 CONSTRAINT [PK_EnquiryFiles] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER TABLE [dbo].[EnquiryFiles]  WITH CHECK ADD  CONSTRAINT [FK_EnquiryFiles_Enquiries] FOREIGN KEY([EnquiryID])
REFERENCES [dbo].[Enquiries] ([ID])
GO
ALTER TABLE [dbo].[EnquiryFiles] CHECK CONSTRAINT [FK_EnquiryFiles_Enquiries]