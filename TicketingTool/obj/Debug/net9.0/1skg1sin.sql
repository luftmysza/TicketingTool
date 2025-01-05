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
CREATE TABLE [AspNetRoles] (
    [Id] nvarchar(450) NOT NULL,
    [Name] nvarchar(256) NULL,
    [NormalizedName] nvarchar(256) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetRoles] PRIMARY KEY ([Id])
);

CREATE TABLE [AspNetUsers] (
    [Id] nvarchar(450) NOT NULL,
    [UserName] nvarchar(256) NOT NULL,
    [NormalizedUserName] nvarchar(256) NULL,
    [Email] nvarchar(256) NULL,
    [NormalizedEmail] nvarchar(256) NULL,
    [EmailConfirmed] bit NOT NULL,
    [PasswordHash] nvarchar(max) NULL,
    [SecurityStamp] nvarchar(max) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    [PhoneNumber] nvarchar(max) NULL,
    [PhoneNumberConfirmed] bit NOT NULL,
    [TwoFactorEnabled] bit NOT NULL,
    [LockoutEnd] datetimeoffset NULL,
    [LockoutEnabled] bit NOT NULL,
    [AccessFailedCount] int NOT NULL,
    CONSTRAINT [PK_AspNetUsers] PRIMARY KEY ([Id]),
    CONSTRAINT [AK_AspNetUsers_UserName] UNIQUE ([UserName])
);

CREATE TABLE [Project] (
    [ID] int NOT NULL IDENTITY,
    [ProjectKey] nvarchar(max) NOT NULL,
    [Counter] int NOT NULL,
    [ProjectName] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Project] PRIMARY KEY ([ID])
);

CREATE TABLE [Status] (
    [ID] int NOT NULL IDENTITY,
    [StatusName] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Status] PRIMARY KEY ([ID])
);

CREATE TABLE [AspNetRoleClaims] (
    [Id] int NOT NULL IDENTITY,
    [RoleId] nvarchar(450) NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [AspNetUserClaims] (
    [Id] int NOT NULL IDENTITY,
    [UserId] nvarchar(450) NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [AspNetUserLogins] (
    [LoginProvider] nvarchar(128) NOT NULL,
    [ProviderKey] nvarchar(128) NOT NULL,
    [ProviderDisplayName] nvarchar(max) NULL,
    [UserId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey]),
    CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [AspNetUserRoles] (
    [UserId] nvarchar(450) NOT NULL,
    [RoleId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY ([UserId], [RoleId]),
    CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [AspNetUserTokens] (
    [UserId] nvarchar(450) NOT NULL,
    [LoginProvider] nvarchar(128) NOT NULL,
    [Name] nvarchar(128) NOT NULL,
    [Value] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name]),
    CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [ApplicationUserProject] (
    [ProjectsID] int NOT NULL,
    [UsersId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_ApplicationUserProject] PRIMARY KEY ([ProjectsID], [UsersId]),
    CONSTRAINT [FK_ApplicationUserProject_AspNetUsers_UsersId] FOREIGN KEY ([UsersId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_ApplicationUserProject_Project_ProjectsID] FOREIGN KEY ([ProjectsID]) REFERENCES [Project] ([ID]) ON DELETE CASCADE
);

CREATE TABLE [Component] (
    [ID] int NOT NULL IDENTITY,
    [ComponentName] nvarchar(max) NOT NULL,
    [ProjectID] int NULL,
    CONSTRAINT [PK_Component] PRIMARY KEY ([ID]),
    CONSTRAINT [FK_Component_Project_ProjectID] FOREIGN KEY ([ProjectID]) REFERENCES [Project] ([ID])
);

CREATE TABLE [TicketField] (
    [ID] int NOT NULL IDENTITY,
    [ProjectID] int NOT NULL,
    [FieldName] nvarchar(20) NOT NULL,
    CONSTRAINT [PK_TicketField] PRIMARY KEY ([ID]),
    CONSTRAINT [FK_TicketField_Project_ProjectID] FOREIGN KEY ([ProjectID]) REFERENCES [Project] ([ID]) ON DELETE CASCADE
);

CREATE TABLE [Component2Project] (
    [ComponentID] int NOT NULL,
    [ProjectID] int NOT NULL,
    CONSTRAINT [PK_Component2Project] PRIMARY KEY ([ComponentID], [ProjectID]),
    CONSTRAINT [FK_Component2Project_Component_ComponentID] FOREIGN KEY ([ComponentID]) REFERENCES [Component] ([ID]) ON DELETE CASCADE,
    CONSTRAINT [FK_Component2Project_Project_ProjectID] FOREIGN KEY ([ProjectID]) REFERENCES [Project] ([ID]) ON DELETE CASCADE
);

CREATE TABLE [Ticket] (
    [ID] int NOT NULL IDENTITY,
    [IssueKey] nvarchar(max) NOT NULL,
    [ProjectID] int NOT NULL,
    [ComponentID] int NOT NULL,
    [Title] nvarchar(50) NULL,
    [Description] nvarchar(1000) NULL,
    [StatusID] int NOT NULL,
    [CreatorID] nvarchar(256) NOT NULL,
    [AssigneeID] nvarchar(256) NULL,
    [CreatedDate] datetime2 NOT NULL,
    [LastUpdatedDate] datetime2 NOT NULL,
    [ResolvedDate] datetime2 NOT NULL,
    CONSTRAINT [PK_Ticket] PRIMARY KEY ([ID]),
    CONSTRAINT [FK_Ticket_AspNetUsers_AssigneeID] FOREIGN KEY ([AssigneeID]) REFERENCES [AspNetUsers] ([UserName]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Ticket_AspNetUsers_CreatorID] FOREIGN KEY ([CreatorID]) REFERENCES [AspNetUsers] ([UserName]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Ticket_Component_ComponentID] FOREIGN KEY ([ComponentID]) REFERENCES [Component] ([ID]) ON DELETE CASCADE,
    CONSTRAINT [FK_Ticket_Project_ProjectID] FOREIGN KEY ([ProjectID]) REFERENCES [Project] ([ID]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Ticket_Status_StatusID] FOREIGN KEY ([StatusID]) REFERENCES [Status] ([ID]) ON DELETE CASCADE
);

CREATE TABLE [TicketChange] (
    [ChangeID] int NOT NULL IDENTITY,
    [TicketID] int NOT NULL,
    [ChangedFieldID] int NOT NULL,
    [ChangedFieldRefID] int NOT NULL,
    [OldValue] nvarchar(max) NOT NULL,
    [NewValue] nvarchar(max) NOT NULL,
    [changedByID] nvarchar(max) NOT NULL,
    [ChangedByRefId] nvarchar(450) NOT NULL,
    [ChangedAt] datetime2 NOT NULL,
    CONSTRAINT [PK_TicketChange] PRIMARY KEY ([ChangeID]),
    CONSTRAINT [FK_TicketChange_AspNetUsers_ChangedByRefId] FOREIGN KEY ([ChangedByRefId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_TicketChange_TicketField_ChangedFieldRefID] FOREIGN KEY ([ChangedFieldRefID]) REFERENCES [TicketField] ([ID]) ON DELETE CASCADE,
    CONSTRAINT [FK_TicketChange_Ticket_TicketID] FOREIGN KEY ([TicketID]) REFERENCES [Ticket] ([ID]) ON DELETE NO ACTION
);

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'AccessFailedCount', N'ConcurrencyStamp', N'Email', N'EmailConfirmed', N'LockoutEnabled', N'LockoutEnd', N'NormalizedEmail', N'NormalizedUserName', N'PasswordHash', N'PhoneNumber', N'PhoneNumberConfirmed', N'SecurityStamp', N'TwoFactorEnabled', N'UserName') AND [object_id] = OBJECT_ID(N'[AspNetUsers]'))
    SET IDENTITY_INSERT [AspNetUsers] ON;
INSERT INTO [AspNetUsers] ([Id], [AccessFailedCount], [ConcurrencyStamp], [Email], [EmailConfirmed], [LockoutEnabled], [LockoutEnd], [NormalizedEmail], [NormalizedUserName], [PasswordHash], [PhoneNumber], [PhoneNumberConfirmed], [SecurityStamp], [TwoFactorEnabled], [UserName])
VALUES (N'1', 0, N'd5692055-4be8-48de-becd-42a41504d2ef', NULL, CAST(0 AS bit), CAST(0 AS bit), NULL, NULL, NULL, NULL, NULL, CAST(0 AS bit), N'dced5a13-a153-4767-93a3-958733b6b794', CAST(0 AS bit), N'X01');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'AccessFailedCount', N'ConcurrencyStamp', N'Email', N'EmailConfirmed', N'LockoutEnabled', N'LockoutEnd', N'NormalizedEmail', N'NormalizedUserName', N'PasswordHash', N'PhoneNumber', N'PhoneNumberConfirmed', N'SecurityStamp', N'TwoFactorEnabled', N'UserName') AND [object_id] = OBJECT_ID(N'[AspNetUsers]'))
    SET IDENTITY_INSERT [AspNetUsers] OFF;

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'ID', N'ComponentName', N'ProjectID') AND [object_id] = OBJECT_ID(N'[Component]'))
    SET IDENTITY_INSERT [Component] ON;
INSERT INTO [Component] ([ID], [ComponentName], [ProjectID])
VALUES (1, N'Unidentified', NULL),
(2, N'User Interface Module', NULL),
(3, N'Database Management', NULL),
(4, N'API Gateway', NULL),
(5, N'Logging Service', NULL),
(6, N'Notification System', NULL),
(7, N'Payment Processor', NULL),
(8, N'Analytics Engine', NULL),
(9, N'Reporting Tool', NULL),
(10, N'Cache Management', NULL),
(11, N'Authentication Service', NULL);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'ID', N'ComponentName', N'ProjectID') AND [object_id] = OBJECT_ID(N'[Component]'))
    SET IDENTITY_INSERT [Component] OFF;

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'ID', N'Counter', N'ProjectKey', N'ProjectName') AND [object_id] = OBJECT_ID(N'[Project]'))
    SET IDENTITY_INSERT [Project] ON;
INSERT INTO [Project] ([ID], [Counter], [ProjectKey], [ProjectName])
VALUES (1, 5, N'BSC', N'Basic Project');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'ID', N'Counter', N'ProjectKey', N'ProjectName') AND [object_id] = OBJECT_ID(N'[Project]'))
    SET IDENTITY_INSERT [Project] OFF;

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'ID', N'StatusName') AND [object_id] = OBJECT_ID(N'[Status]'))
    SET IDENTITY_INSERT [Status] ON;
INSERT INTO [Status] ([ID], [StatusName])
VALUES (1, N'Open'),
(2, N'In Progress'),
(3, N'Resolved'),
(4, N'Closed');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'ID', N'StatusName') AND [object_id] = OBJECT_ID(N'[Status]'))
    SET IDENTITY_INSERT [Status] OFF;

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'ComponentID', N'ProjectID') AND [object_id] = OBJECT_ID(N'[Component2Project]'))
    SET IDENTITY_INSERT [Component2Project] ON;
INSERT INTO [Component2Project] ([ComponentID], [ProjectID])
VALUES (1, 1),
(2, 1),
(3, 1),
(4, 1),
(5, 1),
(6, 1),
(7, 1),
(8, 1),
(9, 1),
(10, 1),
(11, 1);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'ComponentID', N'ProjectID') AND [object_id] = OBJECT_ID(N'[Component2Project]'))
    SET IDENTITY_INSERT [Component2Project] OFF;

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'ID', N'AssigneeID', N'ComponentID', N'CreatedDate', N'CreatorID', N'Description', N'IssueKey', N'LastUpdatedDate', N'ProjectID', N'ResolvedDate', N'StatusID', N'Title') AND [object_id] = OBJECT_ID(N'[Ticket]'))
    SET IDENTITY_INSERT [Ticket] ON;
INSERT INTO [Ticket] ([ID], [AssigneeID], [ComponentID], [CreatedDate], [CreatorID], [Description], [IssueKey], [LastUpdatedDate], [ProjectID], [ResolvedDate], [StatusID], [Title])
VALUES (1, NULL, 1, '2025-01-04T23:39:45.8715357+01:00', N'X1', N'Lorem ipsum odor amet, consectetuer adipiscing elit. Curabitur duis non dis ligula potenti praesent aenean. Mus etiam ridiculus viverra sed sapien nascetur, turpis tempor sollicitudin. Aptent enim luctus dui; urna per id. Sodales auctor vel accumsan dictumst placerat feugiat lectus curabitur? Quam risus lorem vitae commodo porttitor orci ultrices.', N'BSC-1', '2025-01-04T23:39:45.8731212+01:00', 1, '0001-01-01T00:00:00.0000000', 1, N'Seed Ticket 1'),
(2, NULL, 1, '2025-01-04T23:39:45.8731398+01:00', N'X1', N'Lorem ipsum odor amet, consectetuer adipiscing elit. Curabitur duis non dis ligula potenti praesent aenean. Mus etiam ridiculus viverra sed sapien nascetur, turpis tempor sollicitudin. Aptent enim luctus dui; urna per id. Sodales auctor vel accumsan dictumst placerat feugiat lectus curabitur? Quam risus lorem vitae commodo porttitor orci ultrices.', N'BSC-1', '2025-01-04T23:39:45.8731402+01:00', 1, '0001-01-01T00:00:00.0000000', 1, N'Seed Ticket 2'),
(3, NULL, 1, '2025-01-04T23:39:45.8731405+01:00', N'X1', N'Lorem ipsum odor amet, consectetuer adipiscing elit. Curabitur duis non dis ligula potenti praesent aenean. Mus etiam ridiculus viverra sed sapien nascetur, turpis tempor sollicitudin. Aptent enim luctus dui; urna per id. Sodales auctor vel accumsan dictumst placerat feugiat lectus curabitur? Quam risus lorem vitae commodo porttitor orci ultrices.', N'BSC-1', '2025-01-04T23:39:45.8731406+01:00', 1, '0001-01-01T00:00:00.0000000', 1, N'Seed Ticket 3'),
(4, NULL, 1, '2025-01-04T23:39:45.8731408+01:00', N'X1', N'Lorem ipsum odor amet, consectetuer adipiscing elit. Curabitur duis non dis ligula potenti praesent aenean. Mus etiam ridiculus viverra sed sapien nascetur, turpis tempor sollicitudin. Aptent enim luctus dui; urna per id. Sodales auctor vel accumsan dictumst placerat feugiat lectus curabitur? Quam risus lorem vitae commodo porttitor orci ultrices.', N'BSC-1', '2025-01-04T23:39:45.8731409+01:00', 1, '0001-01-01T00:00:00.0000000', 1, N'Seed Ticket 4'),
(5, NULL, 1, '2025-01-04T23:39:45.8731412+01:00', N'X1', N'Lorem ipsum odor amet, consectetuer adipiscing elit. Curabitur duis non dis ligula potenti praesent aenean. Mus etiam ridiculus viverra sed sapien nascetur, turpis tempor sollicitudin. Aptent enim luctus dui; urna per id. Sodales auctor vel accumsan dictumst placerat feugiat lectus curabitur? Quam risus lorem vitae commodo porttitor orci ultrices.', N'BSC-1', '2025-01-04T23:39:45.8731413+01:00', 1, '0001-01-01T00:00:00.0000000', 1, N'Seed Ticket 5');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'ID', N'AssigneeID', N'ComponentID', N'CreatedDate', N'CreatorID', N'Description', N'IssueKey', N'LastUpdatedDate', N'ProjectID', N'ResolvedDate', N'StatusID', N'Title') AND [object_id] = OBJECT_ID(N'[Ticket]'))
    SET IDENTITY_INSERT [Ticket] OFF;

CREATE INDEX [IX_ApplicationUserProject_UsersId] ON [ApplicationUserProject] ([UsersId]);

CREATE INDEX [IX_AspNetRoleClaims_RoleId] ON [AspNetRoleClaims] ([RoleId]);

CREATE UNIQUE INDEX [RoleNameIndex] ON [AspNetRoles] ([NormalizedName]) WHERE [NormalizedName] IS NOT NULL;

CREATE INDEX [IX_AspNetUserClaims_UserId] ON [AspNetUserClaims] ([UserId]);

CREATE INDEX [IX_AspNetUserLogins_UserId] ON [AspNetUserLogins] ([UserId]);

CREATE INDEX [IX_AspNetUserRoles_RoleId] ON [AspNetUserRoles] ([RoleId]);

CREATE INDEX [EmailIndex] ON [AspNetUsers] ([NormalizedEmail]);

CREATE UNIQUE INDEX [UserNameIndex] ON [AspNetUsers] ([NormalizedUserName]) WHERE [NormalizedUserName] IS NOT NULL;

CREATE INDEX [IX_Component_ProjectID] ON [Component] ([ProjectID]);

CREATE INDEX [IX_Component2Project_ProjectID] ON [Component2Project] ([ProjectID]);

CREATE INDEX [IX_Ticket_AssigneeID] ON [Ticket] ([AssigneeID]);

CREATE INDEX [IX_Ticket_ComponentID] ON [Ticket] ([ComponentID]);

CREATE INDEX [IX_Ticket_CreatorID] ON [Ticket] ([CreatorID]);

CREATE INDEX [IX_Ticket_ProjectID] ON [Ticket] ([ProjectID]);

CREATE INDEX [IX_Ticket_StatusID] ON [Ticket] ([StatusID]);

CREATE INDEX [IX_TicketChange_ChangedByRefId] ON [TicketChange] ([ChangedByRefId]);

CREATE INDEX [IX_TicketChange_ChangedFieldRefID] ON [TicketChange] ([ChangedFieldRefID]);

CREATE INDEX [IX_TicketChange_TicketID] ON [TicketChange] ([TicketID]);

CREATE INDEX [IX_TicketField_ProjectID] ON [TicketField] ([ProjectID]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250104223946_Initial', N'9.0.0');

COMMIT;
GO

