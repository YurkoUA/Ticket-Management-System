CREATE TABLE [dbo].[tStatisticsChart]
(
	[Id] INT NOT NULL,
	[Title] NVARCHAR(50) NOT NULL,
	[KeyTitle] NVARCHAR(10) NOT NULL,
	[ValueTitle] NVARCHAR(10) NOT NULL,
	[TypeId] INT NOT NULL,
	[ComputingStrategyId] INT NOT NULL,
	[PageId] INT NULL,
	[SortOrder] INT NOT NULL,
	[SPName] NVARCHAR(30) NOT NULL,
	[Color] NVARCHAR(15) NULL,
	[Is3D] BIT NULL,
	[IsLegend] BIT NULL,
	[StyleClass] NVARCHAR(MAX) NULL

	CONSTRAINT [PK_tStatisticsChart] PRIMARY KEY ([Id]),
	CONSTRAINT [FK_tStatisticsChart_tChartType] FOREIGN KEY ([TypeId]) REFERENCES [tChartType]([Id]),
	CONSTRAINT [FK_tStatisticsChart_tChartComputingStrategy] FOREIGN KEY ([ComputingStrategyId]) REFERENCES [tChartComputingStrategy]([Id]),
	CONSTRAINT [FK_tStatisticsChart_tStatisticsPage] FOREIGN KEY ([PageId]) REFERENCES [tStatisticsPage]([Id])
)
