DECLARE @01_Nominal TABLE
(
	[Id] INT,
	[Amount] MONEY,
	[IsDefault] BIT
)

INSERT INTO @01_Nominal([Id], [Amount], [IsDefault])
VALUES	(1, 1.5, 0),
		(2, 2, 0),
		(3, 3, 0),
		(4, 4, 1),
		(5, 5, 0),
		(6, 10, 0)

INSERT INTO [dbo].[Nominal]([Id], [Amount], [IsDefault])
SELECT	[temp].[Id], [temp].[Amount], [temp].[IsDefault]
FROM @01_Nominal AS [temp]
LEFT JOIN [dbo].[Nominal] AS [actual] ON [actual].[Id] = [temp].[Id]
WHERE [actual].[Id] IS NULL