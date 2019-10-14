DECLARE @03_DefaultNominalId INT = (SELECT TOP 1 [Id] FROM [Nominal] WHERE [IsDefault] = 1)

UPDATE [t]
SET [t].[NominalId] = ISNULL([p].[NominalId], @03_DefaultNominalId)
FROM [Ticket] AS [t]
LEFT JOIN [Package] AS [p] ON [p].[Id] = [t].[PackageId]
WHERE [t].[NominalId] IS NULL