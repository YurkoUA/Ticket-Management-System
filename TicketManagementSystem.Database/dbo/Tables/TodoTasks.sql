CREATE TABLE [dbo].[TodoTasks] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [Date]        DATETIME       NOT NULL,
    [Title]       NVARCHAR (64)  NOT NULL,
    [Description] NVARCHAR (256) NULL,
    [Priority]    TINYINT        NOT NULL,
    [Status]      TINYINT        NOT NULL,
    CONSTRAINT [PK_dbo.TodoTasks] PRIMARY KEY CLUSTERED ([Id] ASC)
);

