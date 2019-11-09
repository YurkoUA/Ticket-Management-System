DECLARE @tChartType_04 TABLE
(
	[Id] INT,
	[Name] NVARCHAR(20)
)

INSERT INTO @tChartType_04([Id], [Name])
VALUES	(1, 'PieChart'),
		(2, 'ColumnChart'),
		(3, 'LineChart')

INSERT INTO [tChartType]([Id], [Name])
SELECT	[temp].[Id], [temp].[Name]
FROM @tChartType_04 AS [temp]
LEFT JOIN [tChartType] AS [actual] ON [actual].[Id] = [temp].[Id]
WHERE [actual].[Id] IS NULL