CREATE TABLE [dbo].[Document]
(
	[Id] INT NOT NULL IDENTITY,
	[TypeId] INT NOT NULL,
	[FileName] NVARCHAR(100) NOT NULL,
	[DisplayName] NVARCHAR(100) NULL

	CONSTRAINT [PK_Document] PRIMARY KEY([Id]),
	CONSTRAINT [FK_Document_DocumentType] FOREIGN KEY ([TypeId]) REFERENCES [DocumentType]([Id])
)
