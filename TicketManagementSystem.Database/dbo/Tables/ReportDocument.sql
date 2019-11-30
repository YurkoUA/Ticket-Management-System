CREATE TABLE [dbo].[ReportDocument]
(
	[Id] INT NOT NULL IDENTITY,
	[ReportId] INT NOT NULL,
	[DocumentId] INT NOT NULL

	CONSTRAINT [PK_ReportDocument] PRIMARY KEY([Id]),
	CONSTRAINT [FK_ReportDocument_Report] FOREIGN KEY ([ReportId]) REFERENCES [Report]([Id]),
	CONSTRAINT [FK_ReportDocument_Document] FOREIGN KEY ([DocumentId]) REFERENCES [Document]([Id])
)
