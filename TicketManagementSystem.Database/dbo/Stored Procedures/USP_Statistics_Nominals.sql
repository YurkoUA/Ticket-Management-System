CREATE PROCEDURE [dbo].[USP_Statistics_Nominals]
	@Date DATE = NULL
AS
	SELECT	CONCAT([n].[Amount], N' грн.') AS [Name],
			COUNT([t].[NominalId]) AS [Count]

	FROM [Nominal] AS [n]
	LEFT JOIN [Ticket] AS [t] ON [t].[NominalId] = [n].[Id]
	WHERE @Date IS NULL OR [t].[AddDate] <= @Date

	GROUP BY [n].[Amount]
	ORDER BY [Count] DESC
RETURN 0
