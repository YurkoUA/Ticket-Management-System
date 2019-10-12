CREATE TABLE [dbo].[Summary] (
    [Id]           INT      IDENTITY (1, 1) NOT NULL,
    [Date]         DATETIME NOT NULL,
    [Tickets]      INT      NOT NULL,
    [HappyTickets] INT      NOT NULL,
    [Packages]     INT      NOT NULL,
    CONSTRAINT [PK_Summary] PRIMARY KEY CLUSTERED ([Id] ASC)
);

