CREATE PROCEDURE [dbo].[USP_Statistics_HappyTickets_Monthly]
	@StartDate DATE = NULL,
	@EndDate DATE = NULL
AS
BEGIN
	SELECT	FORMAT([Date], 'MMMM yyyy', 'uk-ua') AS [Name],
			[HappyTickets] AS [Count]
	FROM [Summary]
	WHERE (@StartDate IS NULL AND @EndDate IS NULL) OR [Date] BETWEEN @StartDate AND @EndDate
END