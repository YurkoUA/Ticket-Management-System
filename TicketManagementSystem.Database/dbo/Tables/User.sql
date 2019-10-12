CREATE TABLE [dbo].[User] (
    [Id]           INT             IDENTITY (1, 1) NOT NULL,
    [RowVersion]   ROWVERSION      NOT NULL,
    [Email]        NVARCHAR (64)   NULL,
    [UserName]     NVARCHAR (64)   NULL,
    [PasswordHash] VARBINARY (MAX) NULL,
    [Salt]         VARBINARY (MAX) NULL,
    [RoleId]       INT             NOT NULL,
    CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_User_Role] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[Role] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_RoleId]
    ON [dbo].[User]([RoleId] ASC);

