CREATE TABLE [dbo].[Report]
(
	[Id] INT NOT NULL IDENTITY,
	[TypeId] INT NOT NULL,
	[DateCreated] DATETIME NOT NULL CONSTRAINT [DF_Report_DateCreated] DEFAULT(GETUTCDATE()),
	[IsAutomatical] BIT NOT NULL CONSTRAINT [DF_Report_IsAutomatical] DEFAULT(0),
	[ReportDataId] NVARCHAR(100) NULL

	CONSTRAINT [PK_Report] PRIMARY KEY ([Id]),
	CONSTRAINT [FK_Report_ReportType] FOREIGN KEY ([TypeId]) REFERENCES [ReportType]([Id])
)
