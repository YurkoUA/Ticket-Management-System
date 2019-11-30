CREATE TABLE [dbo].[Report_Legacy] (
    [Id]          INT      IDENTITY (1, 1) NOT NULL,
    [Date]        DATETIME NOT NULL,
    [IsAutomatic] BIT      NOT NULL,
    CONSTRAINT [PK_Report_Legacy] PRIMARY KEY CLUSTERED ([Id] ASC)
);

