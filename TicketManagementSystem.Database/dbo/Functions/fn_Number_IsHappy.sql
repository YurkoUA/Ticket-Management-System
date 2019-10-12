CREATE FUNCTION [dbo].[fn_Number_IsHappy]
(
	@Number NVARCHAR (6)
)
RETURNS BIT
AS
BEGIN
	DECLARE @leftSum INT
	DECLARE @rightSum INT

	SET @leftSum = CONVERT(INT, SUBSTRING(@Number, 1, 1)) + CONVERT(INT, SUBSTRING(@Number, 2, 1)) + CONVERT(INT, SUBSTRING(@Number, 3, 1))
	SET @rightSum = CONVERT(INT, SUBSTRING(@Number, 4, 1)) + CONVERT(INT, SUBSTRING(@Number, 5, 1)) + CONVERT(INT, SUBSTRING(@Number, 6, 1))

	IF @leftSum = @rightSum
		RETURN 1

	RETURN 0
END
