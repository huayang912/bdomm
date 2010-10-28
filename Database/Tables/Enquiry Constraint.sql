ALTER TABLE [dbo].[Enquiries]  WITH CHECK ADD  CONSTRAINT [FK_Enquiries_EnquirySourceTypes] FOREIGN KEY([SourceTypeID])
REFERENCES [dbo].[EnquirySourceTypes] ([ID])
GO
ALTER TABLE [dbo].[Enquiries] CHECK CONSTRAINT [FK_Enquiries_EnquirySourceTypes]