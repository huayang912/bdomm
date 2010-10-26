ALTER TABLE dbo.Enquiries ADD CONSTRAINT
	FK_Enquiries_EnquirySourceType FOREIGN KEY
	(
	SourceTypeID
	) REFERENCES dbo.EnquirySourceTypes
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 