DECLARE @tStatisticsChart_06 TABLE
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
)

DECLARE @06_PieChart_DefaultStyle NVARCHAR(MAX) = 'col-lg-6 col-md-6 col-sm-12 col-xs-12 pie-chart'

INSERT INTO @tStatisticsChart_06([Id], [Title], [KeyTitle], [ValueTitle], [TypeId], [ComputingStrategyId], [PageId], [SortOrder], [SPName], [Color], [Is3D], [IsLegend], [StyleClass])
VALUES	(1, N'Квитки за серіями',		N'Серія', N'Квитків',	1, 2, NULL, 1, 'USP_Statistics_Series', NULL, 1, 1, @06_PieChart_DefaultStyle) -- Pie/Moment.
		,(2, N'Квитки за кольорами',	N'Колір', N'Квитків',	1, 2, NULL, 2, 'USP_Statistics_Colors', NULL, 1, 1, @06_PieChart_DefaultStyle) -- Pie/Moment.
		,(3, N'Квитки за першою цифрою',N'Цифра', N'Квитків',	1, 2, NULL, 3, 'USP_Statistics_FirstDigit', NULL, 1, 1, @06_PieChart_DefaultStyle) -- Pie/Moment.
		,(4, N'Квитки за номіналом',	N'Номінал', N'Квитків', 1, 2, NULL, 4, 'USP_Statistics_Nominals', NULL, 1, 1, @06_PieChart_DefaultStyle) -- Pie/Moment.
		,(5, N'Щасливі/Звичайні',		N'Тип', N'Квитків',		1, 2, NULL, 5, 'USP_Statistics_Happy', NULL, 1, 1, @06_PieChart_DefaultStyle) -- Pie/Moment.

INSERT INTO [tStatisticsChart]([Id], [Title], [KeyTitle], [ValueTitle], [TypeId], [ComputingStrategyId], [PageId], [SortOrder], [SPName], [Color], [Is3D], [IsLegend], [StyleClass])
SELECT	[temp].[Id], [temp].[Title], [temp].[KeyTitle], [temp].[ValueTitle], [temp].[TypeId], [temp].[ComputingStrategyId], [temp].[PageId], 
		[temp].[SortOrder], [temp].[SPName], [temp].[Color], [temp].[Is3D], [temp].[IsLegend], [temp].[StyleClass]
FROM @tStatisticsChart_06 AS [temp]
LEFT JOIN [tStatisticsChart] AS [actual] ON [actual].[Id] = [temp].[Id]
WHERE [actual].[Id] IS NULL