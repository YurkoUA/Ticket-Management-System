IF COL_LENGTH('dbo.Package', 'Nominal') IS NOT NULL
BEGIN
	DROP TABLE IF EXISTS [#PackageId_Nominal]

	CREATE TABLE [#PackageId_Nominal]
	(
		[PackageId]	INT NOT NULL,
		[Nominal]	FLOAT
	)

	EXEC('	INSERT INTO [#PackageId_Nominal]([PackageId], [Nominal])
			SELECT	[Id], [Nominal]
			FROM [dbo].[Package]')
END