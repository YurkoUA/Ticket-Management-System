﻿CREATE TABLE [dbo].[Nominal]
(
	[Id]		INT NOT NULL,
	[Amount]	FLOAT NOT NULL,
	[IsDefault] BIT NOT NULL

	CONSTRAINT [PK_Nominal] PRIMARY KEY CLUSTERED ([Id])
)
