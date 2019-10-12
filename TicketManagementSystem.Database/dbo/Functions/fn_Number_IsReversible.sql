CREATE FUNCTION [dbo].[fn_Number_IsReversible]
(
	@Number NVARCHAR (6)
)
RETURNS BIT
AS
BEGIN
	IF LEFT(@Number, 3) = REVERSE(RIGHT(@Number, 3))
		RETURN 1

	RETURN 0
END

