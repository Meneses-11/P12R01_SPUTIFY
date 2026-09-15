/*
 P12R01_Sputify
 Autores: Adrián Manuel Meneses Lopez, Adrián Eleuterio Hernández Martínez
 FECHA: 15/09/2026
*/

--=============================
-- CREACIÓN DE ÍNDICES
--=============================

USE sputiffy;
GO
-- =========================================
-- ÍNDICES
-- =========================================

--CREATE INDEX IX_Canciones_AnioLanzamiento ON Canciones(AnioLanzamiento);
--CREATE INDEX IX_Canciones_IdGenero ON Canciones(IdGenero);
--CREATE INDEX IX_Canciones_IdArtista ON Canciones(IdArtista);
--CREATE INDEX IX_HistorialReproduccion_IdUsuario ON HistorialReproduccion(IdUsuario);
--CREATE INDEX IX_Favoritos_IdUsuario ON Favoritos(IdUsuario);
--CREATE INDEX IX_RecomendacionesUsuario_IdUsuario ON RecomendacionesUsuario(IdUsuario);

BEGIN TRANSACTION
    BEGIN TRY
        IF NOT EXISTS (
            SELECT 1 
            FROM sys.indexes 
            WHERE name = N'IX_Canciones_AnioLanzamiento' 
              AND object_id = OBJECT_ID(N'dbo.Canciones')
        )
        BEGIN
            CREATE INDEX IX_Canciones_AnioLanzamiento 
                ON dbo.Canciones(AnioLanzamiento);
            PRINT 'Índice IX_Canciones_AnioLanzamiento creado correctamente';
        END
        ELSE
        BEGIN
            PRINT 'Índice IX_Canciones_AnioLanzamiento ya existe';
        END
    COMMIT TRANSACTION
    END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION
    THROW;
END CATCH
GO

BEGIN TRANSACTION
    BEGIN TRY
        IF NOT EXISTS (
            SELECT 1 
            FROM sys.indexes 
            WHERE name = N'IX_Canciones_IdGenero' 
              AND object_id = OBJECT_ID(N'dbo.Canciones')
        )
        BEGIN
            CREATE INDEX IX_Canciones_IdGenero 
                ON dbo.Canciones(IdGenero);
            PRINT 'Índice IX_Canciones_IdGenero creado correctamente';
        END
        ELSE
        BEGIN
            PRINT 'Índice IX_Canciones_IdGenero ya existe';
        END
    COMMIT TRANSACTION
    END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION
    THROW;
END CATCH
GO

BEGIN TRANSACTION
    BEGIN TRY
        IF NOT EXISTS (
            SELECT 1 
            FROM sys.indexes 
            WHERE name = N'IX_Canciones_IdArtista' 
              AND object_id = OBJECT_ID(N'dbo.Canciones')
        )
        BEGIN
            CREATE INDEX IX_Canciones_IdArtista 
                ON dbo.Canciones(IdArtista);
            PRINT 'Índice IX_Canciones_IdArtista creado correctamente';
        END
        ELSE
        BEGIN
            PRINT 'Índice IX_Canciones_IdArtista ya existe';
        END
    COMMIT TRANSACTION
    END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION
    THROW;
END CATCH
GO

BEGIN TRANSACTION
    BEGIN TRY
        IF NOT EXISTS (
            SELECT 1 
            FROM sys.indexes 
            WHERE name = N'IX_HistorialReproduccion_IdUsuario' 
              AND object_id = OBJECT_ID(N'dbo.HistorialReproduccion')
        )
        BEGIN
            CREATE INDEX IX_HistorialReproduccion_IdUsuario 
                ON dbo.HistorialReproduccion(IdUsuario);
            PRINT 'Índice IX_HistorialReproduccion_IdUsuario creado correctamente';
        END
        ELSE
        BEGIN
            PRINT 'Índice IX_HistorialReproduccion_IdUsuario ya existe';
        END
    COMMIT TRANSACTION
    END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION
    THROW;
END CATCH
GO

BEGIN TRANSACTION
    BEGIN TRY
        IF NOT EXISTS (
            SELECT 1 
            FROM sys.indexes 
            WHERE name = N'IX_Favoritos_IdUsuario' 
              AND object_id = OBJECT_ID(N'dbo.Favoritos')
        )
        BEGIN
            CREATE INDEX IX_Favoritos_IdUsuario 
                ON dbo.Favoritos(IdUsuario);
            PRINT 'Índice IX_Favoritos_IdUsuario creado correctamente';
        END
        ELSE
        BEGIN
            PRINT 'Índice IX_Favoritos_IdUsuario ya existe';
        END
    COMMIT TRANSACTION
    END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION
    THROW;
END CATCH
GO

BEGIN TRANSACTION
    BEGIN TRY
        IF NOT EXISTS (
            SELECT 1 
            FROM sys.indexes 
            WHERE name = N'IX_RecomendacionesUsuario_IdUsuario' 
              AND object_id = OBJECT_ID(N'dbo.RecomendacionesUsuario')
        )
        BEGIN
            CREATE INDEX IX_RecomendacionesUsuario_IdUsuario 
                ON dbo.RecomendacionesUsuario(IdUsuario);
            PRINT 'Índice IX_RecomendacionesUsuario_IdUsuario creado correctamente';
        END
        ELSE
        BEGIN
            PRINT 'Índice IX_RecomendacionesUsuario_IdUsuario ya existe';
        END
    COMMIT TRANSACTION
    END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION
    THROW;
END CATCH
GO