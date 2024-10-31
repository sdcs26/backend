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

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240507192649_SegundaMigracion'
)
BEGIN
    CREATE TABLE [Auditoria] (
        [AuditoriaID] int NOT NULL IDENTITY,
        [Documento_ID] int NULL,
        [NombreCompleto] varchar(100) NULL,
        [CorreoElectronico] varchar(70) NULL,
        [Contrasenia] varchar(150) NULL,
        [ID_Ubicacion] int NULL,
        [ID_Rol] int NULL,
        [FechaRegistro] datetime NULL,
        [Accion] varchar(10) NULL,
        [FechaAuditoria] datetime NULL DEFAULT ((getdate())),
        CONSTRAINT [PK__Auditori__095694E363614E51] PRIMARY KEY ([AuditoriaID])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240507192649_SegundaMigracion'
)
BEGIN
    CREATE TABLE [Inventario] (
        [ID_Inventario] int NOT NULL IDENTITY,
        [NombreEspecie] varchar(100) NULL,
        [LugarOrigen] varchar(50) NULL,
        [CantidadStock] int NULL,
        [Imagen] nvarchar(max) NULL,
        CONSTRAINT [PK__Inventar__4FF10151F25CC3D7] PRIMARY KEY ([ID_Inventario])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240507192649_SegundaMigracion'
)
BEGIN
    CREATE TABLE [Rol] (
        [ID_Rol] int NOT NULL IDENTITY,
        [TipoRol] varchar(50) NULL,
        [Descripcion] varchar(255) NULL,
        CONSTRAINT [PK__Rol__202AD22069BB4517] PRIMARY KEY ([ID_Rol])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240507192649_SegundaMigracion'
)
BEGIN
    CREATE TABLE [TipoHistorial] (
        [ID_TipoHistorial] int NOT NULL IDENTITY,
        [TipoHistorial] varchar(50) NULL,
        CONSTRAINT [PK__TipoHist__D8EE1CFFCE6BA44C] PRIMARY KEY ([ID_TipoHistorial])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240507192649_SegundaMigracion'
)
BEGIN
    CREATE TABLE [Ubicacion] (
        [ID_Ubicacion] int NOT NULL IDENTITY,
        [Ciudad] varchar(80) NULL,
        [Barrio] varchar(80) NULL,
        CONSTRAINT [PK__Ubicacio__E56CAFFBB039C259] PRIMARY KEY ([ID_Ubicacion])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240507192649_SegundaMigracion'
)
BEGIN
    CREATE TABLE [Usuario] (
        [Documento_ID] int NOT NULL,
        [NombreCompleto] varchar(100) NULL,
        [CorreoElectronico] varchar(100) NULL,
        [Contrasenia] varchar(150) NULL,
        [ID_Ubicacion] int NULL,
        [ID_Rol] int NULL,
        [FechaRegistro] datetime NULL,
        CONSTRAINT [PK__Usuario__FBEBB46011505A89] PRIMARY KEY ([Documento_ID]),
        CONSTRAINT [FK__Usuario__ID_Rol__73BA3083] FOREIGN KEY ([ID_Rol]) REFERENCES [Rol] ([ID_Rol]),
        CONSTRAINT [FK__Usuario__ID_Ubic__75A278F5] FOREIGN KEY ([ID_Ubicacion]) REFERENCES [Ubicacion] ([ID_Ubicacion])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240507192649_SegundaMigracion'
)
BEGIN
    CREATE TABLE [DistroSemilla] (
        [ID_DistroSemilla] int NOT NULL IDENTITY,
        [Documento_ID] int NULL,
        [FechaEnvio] datetime NULL,
        [Cantidad] int NULL,
        [ID_Inventario] int NULL,
        [ID_Ubicacion] int NULL,
        CONSTRAINT [PK__DistroSe__2F7388757C98015E] PRIMARY KEY ([ID_DistroSemilla]),
        CONSTRAINT [FK__DistroSem__Docum__412EB0B6] FOREIGN KEY ([Documento_ID]) REFERENCES [Usuario] ([Documento_ID]),
        CONSTRAINT [FK__DistroSem__ID_In__4222D4EF] FOREIGN KEY ([ID_Inventario]) REFERENCES [Inventario] ([ID_Inventario]),
        CONSTRAINT [FK__DistroSem__ID_Ub__4316F928] FOREIGN KEY ([ID_Ubicacion]) REFERENCES [Ubicacion] ([ID_Ubicacion])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240507192649_SegundaMigracion'
)
BEGIN
    CREATE TABLE [Historial] (
        [ID_Historial] int NOT NULL IDENTITY,
        [Fecha] datetime NULL,
        [Documento_ID] int NULL,
        [ID_TipoHistorial] int NULL,
        [ID_DistroSemilla] int NULL,
        CONSTRAINT [PK__Historia__ECA89454F0ED3D45] PRIMARY KEY ([ID_Historial]),
        CONSTRAINT [FK__Historial__Docum__47DBAE45] FOREIGN KEY ([Documento_ID]) REFERENCES [Usuario] ([Documento_ID]),
        CONSTRAINT [FK__Historial__ID_Di__49C3F6B7] FOREIGN KEY ([ID_DistroSemilla]) REFERENCES [DistroSemilla] ([ID_DistroSemilla]),
        CONSTRAINT [FK__Historial__ID_Ti__48CFD27E] FOREIGN KEY ([ID_TipoHistorial]) REFERENCES [TipoHistorial] ([ID_TipoHistorial])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240507192649_SegundaMigracion'
)
BEGIN
    CREATE INDEX [IX_DistroSemilla_Documento_ID] ON [DistroSemilla] ([Documento_ID]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240507192649_SegundaMigracion'
)
BEGIN
    CREATE INDEX [IX_DistroSemilla_ID_Inventario] ON [DistroSemilla] ([ID_Inventario]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240507192649_SegundaMigracion'
)
BEGIN
    CREATE INDEX [IX_DistroSemilla_ID_Ubicacion] ON [DistroSemilla] ([ID_Ubicacion]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240507192649_SegundaMigracion'
)
BEGIN
    CREATE INDEX [IX_Historial_Documento_ID] ON [Historial] ([Documento_ID]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240507192649_SegundaMigracion'
)
BEGIN
    CREATE INDEX [IX_Historial_ID_DistroSemilla] ON [Historial] ([ID_DistroSemilla]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240507192649_SegundaMigracion'
)
BEGIN
    CREATE INDEX [IX_Historial_ID_TipoHistorial] ON [Historial] ([ID_TipoHistorial]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240507192649_SegundaMigracion'
)
BEGIN
    CREATE INDEX [IX_Usuario_ID_Rol] ON [Usuario] ([ID_Rol]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240507192649_SegundaMigracion'
)
BEGIN
    CREATE INDEX [IX_Usuario_ID_Ubicacion] ON [Usuario] ([ID_Ubicacion]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240507192649_SegundaMigracion'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240507192649_SegundaMigracion', N'8.0.4');
END;
GO

COMMIT;
GO

