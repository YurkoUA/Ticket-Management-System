CREATE PROCEDURE [dbo].[USP_Statistics_Colors]
	@Date DATE = NULL
AS
	SELECT	[c].[Name],
			COUNT([t].[ColorId]) AS [Count]

	FROM [Color] AS [c]
	LEFT JOIN [Ticket] AS [t] ON [t].[ColorId] = [c].[Id]
	WHERE @Date IS NULL OR [t].[AddDate] <= @Date

	GROUP BY [c].[Name]
	ORDER BY [Count] DESC
RETURN 0
