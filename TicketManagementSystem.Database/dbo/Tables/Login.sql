CREATE TABLE [dbo].[Login] (
    [Id]        INT            IDENTITY (1, 1) NOT NULL,
    [UserId]    INT            NOT NULL,
    [Date]      DATETIME       NOT NULL,
    [IpAddress] NVARCHAR (MAX) NULL,
    [Browser]   NVARCHAR (MAX) NULL,
    [UserAgent] NVARCHAR (MAX) NULL,
    [Type]      NVARCHAR (MAX) NULL,
    [Host]      NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_Login] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Login_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_UserId]
    ON [dbo].[Login]([UserId] ASC);

