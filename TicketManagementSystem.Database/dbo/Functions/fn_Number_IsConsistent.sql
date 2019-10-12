
CREATE FUNCTION [dbo].[fn_Number_IsConsistent]
(
	@Number NVARCHAR (6)
)
RETURNS INT
AS
BEGIN
	IF LEFT(@Number, 3) = RIGHT(@Number, 3)
		RETURN 1

	RETURN 0
END