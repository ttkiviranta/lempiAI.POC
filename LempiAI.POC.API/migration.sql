IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [AgentReports] (
    [Id] int NOT NULL IDENTITY,
    [AgentName] nvarchar(255) NOT NULL,
    [Title] nvarchar(255) NOT NULL,
    [Content] nvarchar(max) NOT NULL,
    [KeyFindings] nvarchar(max) NOT NULL,
    [ReportType] nvarchar(50) NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NOT NULL,
    CONSTRAINT [PK_AgentReports] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Employees] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(255) NOT NULL,
    [Email] nvarchar(255) NOT NULL,
    [Role] nvarchar(100) NOT NULL,
    [PhoneNumber] nvarchar(20) NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NOT NULL,
    CONSTRAINT [PK_Employees] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [ProcessImprovements] (
    [Id] int NOT NULL IDENTITY,
    [Title] nvarchar(255) NOT NULL,
    [Description] nvarchar(max) NOT NULL,
    [ProcessArea] nvarchar(100) NOT NULL,
    [ImpactLevel] nvarchar(50) NOT NULL,
    [Status] nvarchar(50) NOT NULL,
    [SuggestedActions] nvarchar(max) NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NOT NULL,
    CONSTRAINT [PK_ProcessImprovements] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Feedback] (
    [Id] int NOT NULL IDENTITY,
    [ProvidedByEmployeeId] int NOT NULL,
    [Rating] int NOT NULL,
    [FeedbackText] nvarchar(max) NOT NULL,
    [Category] nvarchar(100) NOT NULL,
    [Status] nvarchar(50) NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NOT NULL,
    CONSTRAINT [PK_Feedback] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Feedback_Employees_ProvidedByEmployeeId] FOREIGN KEY ([ProvidedByEmployeeId]) REFERENCES [Employees] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [Schedules] (
    [Id] int NOT NULL IDENTITY,
    [EmployeeId] int NOT NULL,
    [TaskDescription] nvarchar(500) NOT NULL,
    [ScheduledStart] datetime2 NOT NULL,
    [ScheduledEnd] datetime2 NOT NULL,
    [Status] nvarchar(50) NOT NULL,
    [Priority] nvarchar(50) NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NOT NULL,
    CONSTRAINT [PK_Schedules] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Schedules_Employees_EmployeeId] FOREIGN KEY ([EmployeeId]) REFERENCES [Employees] ([Id]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_AgentReports_AgentName] ON [AgentReports] ([AgentName]);
GO

CREATE UNIQUE INDEX [IX_Employees_Email] ON [Employees] ([Email]);
GO

CREATE INDEX [IX_Feedback_ProvidedByEmployeeId] ON [Feedback] ([ProvidedByEmployeeId]);
GO

CREATE INDEX [IX_Schedules_EmployeeId] ON [Schedules] ([EmployeeId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260606045852_InitialCreate', N'8.0.11');
GO

COMMIT;
GO

