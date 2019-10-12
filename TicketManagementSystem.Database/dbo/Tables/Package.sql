CREATE TABLE [dbo].[Package] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [Name]        NVARCHAR (64)  NULL,
    [ColorId]     INT            NULL,
    [SerialId]    INT            NULL,
    [FirstNumber] INT            NULL,
    [Nominal]     FLOAT (53)     NOT NULL,
    [IsSpecial]   BIT            NOT NULL,
    [IsOpened]    BIT            NOT NULL,
    [Date]        DATETIME       NOT NULL,
    [Note]        NVARCHAR (128) NULL,
    CONSTRAINT [PK_Package] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Package_Color] FOREIGN KEY ([ColorId]) REFERENCES [dbo].[Color] ([Id]),
    CONSTRAINT [FK_Package_Serial] FOREIGN KEY ([SerialId]) REFERENCES [dbo].[Serial] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_ColorId]
    ON [dbo].[Package]([ColorId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_SerialId]
    ON [dbo].[Package]([SerialId] ASC);

