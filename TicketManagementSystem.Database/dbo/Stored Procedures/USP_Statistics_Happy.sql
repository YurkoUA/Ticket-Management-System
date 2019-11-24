CREATE PROCEDURE [dbo].[USP_Statistics_Happy]
	@Date DATE = NULL
AS
	DECLARE @TotalCount INT
	DECLARE @HappyCount INT

	SELECT @TotalCount = COUNT(1)
	FROM [Ticket]
	WHERE @Date IS NULL OR [AddDate] <= @Date

	SELECT @HappyCount = COUNT(1)
	FROM [Ticket]
	WHERE [dbo].[fn_Number_IsHappy]([Number]) = 1
		AND @Date IS NULL OR [AddDate] <= @Date

	SELECT N'Звичайні' AS [Name], (@TotalCount - @HappyCount) AS [Count]
	UNION
	SELECT N'Щасливі' AS [Name], @HappyCount AS [Count]

RETURN 0
