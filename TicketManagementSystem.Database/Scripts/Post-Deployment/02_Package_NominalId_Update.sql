IF COL_LENGTH('dbo.Package', 'NominalId') IS NOT NULL
	AND OBJECT_ID('tempdb..#PackageId_Nominal') IS NOT NULL
BEGIN
	UPDATE [p]
	SET [NominalId] = [n].[Id]
	FROM [dbo].[Package] AS [p]
	JOIN [#PackageId_Nominal] AS [temp] ON [temp].[PackageId] = [p].[Id]
	JOIN [Nominal] AS [n] ON [n].[Amount] = [temp].[Nominal]

	DROP TABLE [#PackageId_Nominal]
END