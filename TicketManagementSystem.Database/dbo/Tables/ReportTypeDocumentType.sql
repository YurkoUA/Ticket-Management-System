CREATE TABLE [dbo].[ReportTypeDocumentType]
(
	[Id] INT NOT NULL,
	[ReportTypeId] INT NOT NULL,
	[DocumentTypeId] INT NOT NULL

	CONSTRAINT [PK_ReportTypeDocumentType] PRIMARY KEY([Id]),
	CONSTRAINT [FK_ReportTypeDocumentType_ReportType] FOREIGN KEY([ReportTypeId]) REFERENCES [ReportType]([Id]),	
	CONSTRAINT [FK_ReportTypeDocumentType_DocumentType] FOREIGN KEY([DocumentTypeId]) REFERENCES [DocumentType]([Id])
)
