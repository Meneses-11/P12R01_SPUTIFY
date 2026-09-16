/*
 P12R01_Sputify
 Autores: Adrián Manuel Meneses Lopez, Adrián Eleuterio Hernández Martínez
 FECHA: 15/09/2026
*/

--=============================
-- CONSULTAS GENERALES
--=============================

USE sputiffy;
GO
SELECT * FROM Favoritos;
SELECT * FROM Usuarios;
SELECT * FROM CancionesOffline;
SELECT * FROM Canciones;
SELECT * FROM Artistas;
SELECT * FROM AudiolibrosGuardados

EXEC sp_help '[dbo].[Albumes]'
EXEC sp_help '[dbo].[Artistas]'
EXEC sp_help '[dbo].[AudiolibrosGuardados]'
EXEC sp_help '[dbo].[Canciones]'
EXEC sp_help '[dbo].[CancionesOffline]'
EXEC sp_help '[dbo].[Favoritos]'
EXEC sp_help '[dbo].[Generos]'
EXEC sp_help '[dbo].[HistorialReproduccion]'
EXEC sp_help '[dbo].[PlaylistCanciones]'
EXEC sp_help '[dbo].[PlaylistDetalle]'
EXEC sp_help '[dbo].[Playlists]'
EXEC sp_help '[dbo].[PreferenciasUsuario]'
EXEC sp_help '[dbo].[RecomendacionesUsuario]'
EXEC sp_help '[dbo].[Roles]'
EXEC sp_help '[dbo].[Usuarios]'

SELECT * FROM AudiolibrosGuardados

SELECT * FROM NarracionesDeportivas

USE sputiffy;
GO

SELECT * FROM INFORMATION_SCHEMA.TABLES 
WHERE TABLE_NAME = 'NarracionesDeportivas';

