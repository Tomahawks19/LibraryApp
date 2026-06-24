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
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260624013149_InitialCreate'
)
BEGIN
    CREATE TABLE [Authors] (
        [AuthorId] int NOT NULL IDENTITY,
        [FullName] nvarchar(150) NOT NULL,
        [Nationality] nvarchar(100) NOT NULL,
        [DateOfBirth] datetime2 NULL,
        [Biography] nvarchar(1000) NOT NULL,
        CONSTRAINT [PK_Authors] PRIMARY KEY ([AuthorId])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260624013149_InitialCreate'
)
BEGIN
    CREATE TABLE [Genre] (
        [GenreId] int NOT NULL IDENTITY,
        [Name] nvarchar(100) NOT NULL,
        CONSTRAINT [PK_Genre] PRIMARY KEY ([GenreId])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260624013149_InitialCreate'
)
BEGIN
    CREATE TABLE [Members] (
        [MemberId] int NOT NULL IDENTITY,
        [FullName] nvarchar(150) NOT NULL,
        [IsActive] bit NOT NULL,
        [MemberType] nvarchar(1) NOT NULL,
        [LateFee] decimal(18,2) NOT NULL,
        CONSTRAINT [PK_Members] PRIMARY KEY ([MemberId])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260624013149_InitialCreate'
)
BEGIN
    CREATE TABLE [Books] (
        [BookId] int NOT NULL IDENTITY,
        [Title] nvarchar(200) NOT NULL,
        [PublicationYear] int NOT NULL,
        [PageCount] int NOT NULL,
        [IsAvailable] bit NOT NULL,
        [AuthorId] int NOT NULL,
        CONSTRAINT [PK_Books] PRIMARY KEY ([BookId]),
        CONSTRAINT [FK_Books_Authors_AuthorId] FOREIGN KEY ([AuthorId]) REFERENCES [Authors] ([AuthorId]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260624013149_InitialCreate'
)
BEGIN
    CREATE TABLE [BookGenre] (
        [BookId] int NOT NULL,
        [GenreId] int NOT NULL,
        [DateTagged] datetime2 NOT NULL DEFAULT (GETDATE()),
        CONSTRAINT [PK_BookGenre] PRIMARY KEY ([BookId], [GenreId]),
        CONSTRAINT [FK_BookGenre_Books_BookId] FOREIGN KEY ([BookId]) REFERENCES [Books] ([BookId]) ON DELETE CASCADE,
        CONSTRAINT [FK_BookGenre_Genre_GenreId] FOREIGN KEY ([GenreId]) REFERENCES [Genre] ([GenreId]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260624013149_InitialCreate'
)
BEGIN
    CREATE TABLE [Loans] (
        [LoanId] int NOT NULL IDENTITY,
        [LoanDate] datetime2 NOT NULL,
        [ReturnDate] datetime2 NULL,
        [MemberId] int NOT NULL,
        [BookId] int NOT NULL,
        CONSTRAINT [PK_Loans] PRIMARY KEY ([LoanId]),
        CONSTRAINT [FK_Loans_Books_BookId] FOREIGN KEY ([BookId]) REFERENCES [Books] ([BookId]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Loans_Members_MemberId] FOREIGN KEY ([MemberId]) REFERENCES [Members] ([MemberId]) ON DELETE NO ACTION
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260624013149_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_BookGenre_GenreId] ON [BookGenre] ([GenreId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260624013149_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Books_AuthorId] ON [Books] ([AuthorId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260624013149_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Loans_BookId] ON [Loans] ([BookId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260624013149_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Loans_MemberId] ON [Loans] ([MemberId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260624013149_InitialCreate'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260624013149_InitialCreate', N'10.0.9');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260624052141_AddMemberEmail'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260624052141_AddMemberEmail', N'10.0.9');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260624053135_MakeAuthorFieldsOptional'
)
BEGIN
    DECLARE @var nvarchar(max);
    SELECT @var = QUOTENAME([d].[name])
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Authors]') AND [c].[name] = N'Nationality');
    IF @var IS NOT NULL EXEC(N'ALTER TABLE [Authors] DROP CONSTRAINT ' + @var + ';');
    ALTER TABLE [Authors] ALTER COLUMN [Nationality] nvarchar(100) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260624053135_MakeAuthorFieldsOptional'
)
BEGIN
    DECLARE @var1 nvarchar(max);
    SELECT @var1 = QUOTENAME([d].[name])
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Authors]') AND [c].[name] = N'Biography');
    IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [Authors] DROP CONSTRAINT ' + @var1 + ';');
    ALTER TABLE [Authors] ALTER COLUMN [Biography] nvarchar(1000) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260624053135_MakeAuthorFieldsOptional'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260624053135_MakeAuthorFieldsOptional', N'10.0.9');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260624055235_AddMemberProfileAndBookPublishedDate'
)
BEGIN
    ALTER TABLE [Members] ADD [DateOfBirth] datetime2 NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260624055235_AddMemberProfileAndBookPublishedDate'
)
BEGIN
    ALTER TABLE [Books] ADD [PublishedDate] datetime2 NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260624055235_AddMemberProfileAndBookPublishedDate'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260624055235_AddMemberProfileAndBookPublishedDate', N'10.0.9');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260624154903_AddBookISBN'
)
BEGIN
    ALTER TABLE [Books] ADD [ISBN] nvarchar(20) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260624154903_AddBookISBN'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260624154903_AddBookISBN', N'10.0.9');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260624161058_AddLibraryBranch'
)
BEGIN
    CREATE TABLE [LibraryBranches] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(max) NOT NULL,
        [Address] nvarchar(max) NOT NULL,
        [Phone] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_LibraryBranches] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260624161058_AddLibraryBranch'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260624161058_AddLibraryBranch', N'10.0.9');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260624165543_SeedInitialReferenceData'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'AuthorId', N'Biography', N'DateOfBirth', N'FullName', N'Nationality') AND [object_id] = OBJECT_ID(N'[Authors]'))
        SET IDENTITY_INSERT [Authors] ON;
    EXEC(N'INSERT INTO [Authors] ([AuthorId], [Biography], [DateOfBirth], [FullName], [Nationality])
    VALUES (1001, NULL, NULL, N''Gabriel García Márquez'', N''Colombian''),
    (1002, NULL, NULL, N''Isabel Allende'', N''Chilean''),
    (1003, NULL, NULL, N''Mario Vargas Llosa'', N''Peruvian'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'AuthorId', N'Biography', N'DateOfBirth', N'FullName', N'Nationality') AND [object_id] = OBJECT_ID(N'[Authors]'))
        SET IDENTITY_INSERT [Authors] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260624165543_SeedInitialReferenceData'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'GenreId', N'Name') AND [object_id] = OBJECT_ID(N'[Genre]'))
        SET IDENTITY_INSERT [Genre] ON;
    EXEC(N'INSERT INTO [Genre] ([GenreId], [Name])
    VALUES (2001, N''Magical Realism''),
    (2002, N''Historical Fiction''),
    (2003, N''Biography'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'GenreId', N'Name') AND [object_id] = OBJECT_ID(N'[Genre]'))
        SET IDENTITY_INSERT [Genre] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260624165543_SeedInitialReferenceData'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260624165543_SeedInitialReferenceData', N'10.0.9');
END;

COMMIT;
GO

