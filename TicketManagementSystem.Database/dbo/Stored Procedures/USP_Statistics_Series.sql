CREATE PROCEDURE [dbo].[USP_Statistics_Series]
	@Date DATE = NULL
AS
BEGIN
	SELECT	[s].[Name],
			COUNT([t].[SerialId]) AS [Count]

	FROM [Serial] AS [s]
	LEFT JOIN [Ticket] AS [t] ON [t].[SerialId] = [s].[Id]
	WHERE @Date IS NULL OR [t].[AddDate] <= @Date

	GROUP BY [s].[Name]
	ORDER BY [Count] DESC
END
