CREATE PROCEDURE [dbo].[USP_Statistics_Tickets_Monthly]
	@StartDate DATE = NULL,
	@EndDate DATE = NULL
AS
BEGIN
	SELECT	CONCAT(UPPER(LEFT([s].[Date], 1)), SUBSTRING([s].[Date], 2, LEN([s].[Date]))) AS [Name],
			[s].[Count]
	FROM
	(
		SELECT	FORMAT([Date], 'MMMM yyyy', 'uk-ua') AS [Date],
				[Tickets] AS [Count]
		FROM [Summary]
		WHERE (@StartDate IS NULL AND @EndDate IS NULL) OR [Date] BETWEEN @StartDate AND @EndDate
	) AS [s]
END