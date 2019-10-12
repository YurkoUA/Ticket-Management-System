CREATE VIEW VW_Packages
AS 

SELECT	[p].[Id]
		,IIF([p].[IsSpecial] = 1, [p].[Name], CONCAT([s].[Name], '-', [c].[Name], IIF([FirstTicket].[Number] IS NOT NULL, CONCAT(' (', [FirstTicket].[Number], ')'), ''))) 
							AS [Name]
		,[p].[IsOpened]
		,[p].[IsSpecial]
		,[p].[FirstNumber]
		,[p].[Nominal]
		,[p].[Note]
		,[p].[Date]
		,COUNT([t].[Id])	AS [TicketsCount]

		-- Serial
		,[s].[Id]			AS [SerialId]
		,[s].[Name]			AS [SerialName]

		-- Color
		,[c].[Id]			AS [ColorId]
		,[c].[Name]			AS [ColorName]

FROM [Package] AS [p]

LEFT JOIN [Serial] AS [s] ON [s].[Id] = [p].[SerialId]
LEFT JOIN [Color]	AS [c] ON [c].[Id] = [p].[ColorId]
LEFT JOIN [Ticket] AS [t] ON [t].[PackageId] = [p].[Id]

OUTER APPLY
	(
		SELECT TOP 1 [Number]
		FROM [Ticket]
		WHERE [Ticket].[PackageId] = [p].[Id]
		ORDER BY [Ticket].[Id]
	) AS [FirstTicket]

GROUP BY [p].[Id]
		,[p].[Name]
		,[p].[IsOpened]
		,[p].[IsSpecial]
		,[p].[FirstNumber]
		,[p].[Nominal]
		,[p].[Note]
		,[p].[Date]
		,[c].[Id]
		,[c].[Name]

		,[s].[Id]
		,[s].[Name]

		,[FirstTicket].[Number]