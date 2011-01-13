ALTER TABLE Quotations 
DROP COLUMN ProjectYear
GO


ALTER TABLE Quotations 
ADD  ProjectYear  int NOT NULL DEFAULT(0)
GO
