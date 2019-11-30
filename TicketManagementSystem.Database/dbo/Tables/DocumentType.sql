CREATE TABLE [dbo].[DocumentType]
(
	[Id] INT NOT NULL,
	[Name] NVARCHAR(30) NOT NULL,
	[BucketId] INT NOT NULL,
	[Path] NVARCHAR(50) NOT NULL

	CONSTRAINT [PK_DocumentType] PRIMARY KEY ([Id]),
	CONSTRAINT [FK_DocumentType_Bucket] FOREIGN KEY ([BucketId]) REFERENCES [Bucket]([Id])
)
