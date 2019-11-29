CREATE PROCEDURE [dbo].[USP_Statistics_Packages_Monthly]
	@StartDate DATE = NULL,
	@EndDate DATE = NULL
AS
BEGIN
	SELECT	FORMAT([Date], 'MMMM yyyy', 'uk-ua') AS [Name],
			[Packages] AS [Count]
	FROM [Summary]
	WHERE (@StartDate IS NULL AND @EndDate IS NULL) OR [Date] BETWEEN @StartDate AND @EndDate
END