CREATE TABLE [dbo].[Color] (
    [Id]         INT           IDENTITY (1, 1) NOT NULL,
    [RowVersion] ROWVERSION    NOT NULL,
    [Name]       NVARCHAR (32) NULL,
    CONSTRAINT [PK_Color] PRIMARY KEY CLUSTERED ([Id] ASC)
);

