DECLARE @tChartComputingStrategy_05 TABLE
(
	[Id] INT,
	[Name] NVARCHAR(10)
)

INSERT INTO @tChartComputingStrategy_05([Id], [Name])
VALUES	(1, 'Period'),
		(2, 'Moment')

INSERT INTO [tChartComputingStrategy]([Id], [Name])
SELECT	[temp].[Id], [temp].[Name]
FROM @tChartComputingStrategy_05 AS [temp]
LEFT JOIN [tChartComputingStrategy] AS [actual] ON [actual].[Id] = [temp].[Id]
WHERE [actual].[Id] IS NULL