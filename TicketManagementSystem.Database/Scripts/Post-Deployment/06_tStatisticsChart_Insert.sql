DECLARE @tStatisticsChart_06 TABLE
(
	[Id] INT NOT NULL,
	[Title] NVARCHAR(50) NOT NULL,
	[TypeId] INT NOT NULL,
	[ComputingStrategyId] INT NOT NULL,
	[PageId] INT NULL,
	[SortOrder] INT NOT NULL,
	[SPName] NVARCHAR(30) NOT NULL,
	[Color] NVARCHAR(15) NULL,
	[Is3D] BIT NULL,
	[IsLegend] BIT NULL
)

INSERT INTO @tStatisticsChart_06([Id], [Title], [TypeId], [ComputingStrategyId], [PageId], [SortOrder], [SPName], [Color], [Is3D], [IsLegend])
VALUES	(1, N'Квитки за серіями', 1, 2, NULL, 1, 'USP_Statistics_Series', NULL, 1, 1) -- Pie/Moment.

INSERT INTO [tStatisticsChart]([Id], [Title], [TypeId], [ComputingStrategyId], [PageId], [SortOrder], [SPName], [Color], [Is3D], [IsLegend])
SELECT	[temp].[Id], [temp].[Title], [temp].[TypeId], [temp].[ComputingStrategyId], [temp].[PageId], 
		[temp].[SortOrder], [temp].[SPName], [temp].[Color], [temp].[Is3D], [temp].[IsLegend]
FROM @tStatisticsChart_06 AS [temp]
LEFT JOIN [tStatisticsPage] AS [actual] ON [actual].[Id] = [temp].[Id]
WHERE [actual].[Id] IS NULL