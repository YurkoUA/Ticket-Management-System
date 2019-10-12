CREATE TABLE [dbo].[Serial] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    [Name]       NVARCHAR (4)   NULL,
    [Note]       NVARCHAR (128) NULL,
    CONSTRAINT [PK_Serial] PRIMARY KEY CLUSTERED ([Id] ASC)
);

