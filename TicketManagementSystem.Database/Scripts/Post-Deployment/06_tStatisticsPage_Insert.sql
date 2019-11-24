DECLARE @07_tStatisticsPage_Insert TABLE
(
	[Id] INT NOT NULL,
	[Name] NVARCHAR(50) NOT NULL,
	[SortOrder] INT NOT NULL
)

INSERT INTO @07_tStatisticsPage_Insert([Id], [Name], [SortOrder])
VALUES	(1, N'Статистика за місяцями', 1)

INSERT INTO [tStatisticsPage]([Id], [Name], [SortOrder])
SELECT	[temp].[Id], [temp].[Name], [temp].[SortOrder]
FROM @07_tStatisticsPage_Insert AS [temp]
LEFT JOIN [tStatisticsPage] AS [actual] ON [actual].[Id] = [temp].[Id]
WHERE [actual].[Id] IS NULL