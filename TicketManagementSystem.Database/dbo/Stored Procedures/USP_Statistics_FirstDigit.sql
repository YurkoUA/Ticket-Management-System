CREATE PROCEDURE [dbo].[USP_Statistics_FirstDigit]
	@Date DATE = NULL
AS
	SELECT	LEFT(t.[Number], 1)	AS [Name],
			COUNT(*)			AS [Count]

	FROM [Ticket] AS [t]
	WHERE @Date IS NULL OR [t].[AddDate] <= @Date

	GROUP BY LEFT(t.[Number], 1)
	ORDER BY [Name]
RETURN 0
