CREATE VIEW VW_Tickets
AS 

SELECT	[t].[Id]
		,[t].[Number]
		,[t].[SerialNumber]
		,[t].[AddDate]
		,[t].[Date]
		,[t].[Note]

		,[p].[Id]	AS [PackageId]
		,[p].[Name]	AS [PackageName]

		,[s].[Id]	AS [SerialId]
		,[s].[Name]	AS [SerialName]

		,[c].[Id]	AS [ColorId]
		,[c].[Name]	AS [ColorName]

FROM [Ticket] AS [t]

JOIN [Serial] AS [s] ON [s].[Id] = [t].[SerialId]
JOIN [Color] AS [c] ON [c].[Id] = [t].[ColorId]
LEFT JOIN [VW_Packages] AS [p] ON [p].[Id] = [t].[PackageId]

ORDER BY [t].[Number]
OFFSET 0 ROWS

