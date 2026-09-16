USE sputiffy;
GO

/*
P12R01_SPUTIFY
AUTORES: 
Adrian Manuel Meneses López
Adrian Heleuterio Hernandez Martinez
FECHA: 15/09/2026
*/


/*==========================================
	CONTINGENCIA DE TABLAS
  ==========================================*/

SELECT name FROM sys.tables ORDER BY name;



/* -----Tabla NarracionesDeportivas----- */
BEGIN TRANSACTION;
BEGIN TRY

	IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'NarracionesDeportivas' AND TABLE_SCHEMA = 'dbo')
	BEGIN
		DROP TABLE dbo.NarracionesDeportivas;
		PRINT 'Tabla NarracionesDeportivas eliminada correctamente';
	END
	ELSE
	BEGIN
		PRINT 'Tabla NarracionesDeportivas no existe';
	END
COMMIT TRANSACTION;--Confirmar cambios
END TRY
BEGIN CATCH

	IF @@TRANCOUNT > 0
		ROLLBACK TRANSACTION;
	THROW;

END CATCH
GO


/* -----Tabla AudiolibrosGuardados----- */
BEGIN TRANSACTION;
BEGIN TRY

	IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'AudiolibrosGuardados' AND TABLE_SCHEMA = 'dbo')
	BEGIN
		DROP TABLE dbo.AudiolibrosGuardados;
		PRINT 'Tabla AudiolibrosGuardados eliminada correctamente';
	END
	ELSE
	BEGIN
		PRINT 'Tabla AudiolibrosGuardados no existe';
	END
COMMIT TRANSACTION;--Confirmar cambios
END TRY
BEGIN CATCH

	IF @@TRANCOUNT > 0
		ROLLBACK TRANSACTION;
	THROW;

END CATCH
GO


/* -----Tabla CancionesOffline----- */
BEGIN TRANSACTION;
BEGIN TRY

	IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'CancionesOffline' AND TABLE_SCHEMA = 'dbo')
	BEGIN
		DROP TABLE dbo.CancionesOffline;
		PRINT 'Tabla CancionesOffline eliminada correctamente';
	END
	COMMIT TRANSACTION;--Confirmar cambios

END TRY
BEGIN CATCH
	
	IF @@TRANCOUNT > 0
		ROLLBACK TRANSACTION;
	THROW;

END CATCH
GO



/* -----Tabla PlaylistCanciones----- */
BEGIN TRANSACTION;
BEGIN TRY
	
	IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'PlaylistCanciones' AND TABLE_SCHEMA = 'dbo')
	BEGIN
		DROP TABLE dbo.PlaylistCanciones;
		PRINT 'Tabla PlaylistCanciones eliminada correctamente';
	END

COMMIT TRANSACTION;--Confirmar cambios
END TRY
BEGIN CATCH
	
	IF @@TRANCOUNT > 0
		ROLLBACK TRANSACTION;
	THROW;

END CATCH
GO


/* -----Tabla PreferenciasUsuario----- */
BEGIN TRANSACTION;
BEGIN TRY
	
	IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'PreferenciasUsuario' AND TABLE_SCHEMA = 'dbo')
	BEGIN

		DROP TABLE dbo.PreferenciasUsuario;
		PRINT 'Tabla PreferenciasUsuario eliminada correctamente';
	END

COMMIT TRANSACTION;--Confirmar cambios
END TRY
BEGIN CATCH
	
	IF @@TRANCOUNT > 0
		ROLLBACK TRANSACTION;
	THROW;

END CATCH
GO


/* -----Tabla PlaylistDetalle----- */
BEGIN TRANSACTION;
BEGIN TRY
	
	IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'PlaylistDetalle' AND TABLE_SCHEMA = 'dbo')
	BEGIN
		DROP TABLE dbo.PlaylistDetalle;
		PRINT 'Tabla PlaylistDetalle eliminada correctamente';
	END

COMMIT TRANSACTION;--Confirmar cambios
END TRY
BEGIN CATCH

	IF @@TRANCOUNT > 0
		ROLLBACK TRANSACTION;
	THROW;

END CATCH
GO


/* -----Tabla Playlists----- */
BEGIN TRANSACTION;
BEGIN TRY
	
	IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Playlists' AND TABLE_SCHEMA = 'dbo')
	BEGIN
		DROP TABLE dbo.Playlists;
		PRINT 'Tabla Playlists eliminada correctamente';
	END

COMMIT TRANSACTION;--Confirmar cambios
END TRY
BEGIN CATCH

	IF @@TRANCOUNT > 0
		ROLLBACK TRANSACTION;
	THROW;

END CATCH
GO


/* -----Tabla RecomendacionesUsuario----- */
BEGIN TRANSACTION;
BEGIN TRY

	IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'RecomendacionesUsuario' AND TABLE_SCHEMA = 'dbo')
	BEGIN
		DROP TABLE dbo.RecomendacionesUsuario;
		PRINT 'Tabla RecomendacionesUsuario eliminada correctamente';
	END

COMMIT TRANSACTION;--Confirmar cambios
END TRY
BEGIN CATCH

	IF @@TRANCOUNT > 0
		ROLLBACK TRANSACTION;
	THROW;

END CATCH
GO

/* -----Tabla HistorialResproduccion----- */
BEGIN TRANSACTION;
BEGIN TRY

	IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'HistorialReproduccion' AND TABLE_SCHEMA = 'dbo')
	BEGIN
		DROP TABLE dbo.HistorialReproduccion;
		PRINT 'Tabla HistorialReproduccion eliminada correctamente';
	END

COMMIT TRANSACTION;--Confirmar cambios
END TRY
BEGIN CATCH

	IF @@TRANCOUNT > 0
		ROLLBACK TRANSACTION;
	THROW;

END CATCH
GO


/* -----Tabla Favoritos----- */
BEGIN TRANSACTION;
BEGIN TRY
	
	IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Favoritos' AND TABLE_SCHEMA = 'dbo')
	BEGIN
		DROP TABLE dbo.Favoritos;
		PRINT 'Tabla Favoritos eliminada correctamente';
	END

COMMIT TRANSACTION;--Confirmar cambios
END TRY
BEGIN CATCH

	IF @@TRANCOUNT > 0
		ROLLBACK TRANSACTION;
	THROW;

END CATCH
GO


/* -----Tabla Canciones----- */
BEGIN TRANSACTION;
BEGIN TRY

	IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Canciones' AND TABLE_SCHEMA = 'dbo')
	BEGIN
		DROP TABLE dbo.Canciones;
		PRINT 'Tabla Canciones eliminada correctamente';
	END
	COMMIT TRANSACTION;--Confirmar cambios

END TRY
BEGIN CATCH

	IF @@TRANCOUNT > 0
		ROLLBACK TRANSACTION;
	THROW;

END CATCH
GO



		
/* -----Tabla Generos----- */
BEGIN TRANSACTION;
BEGIN TRY
	
	IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Generos' AND TABLE_SCHEMA = 'dbo')
	BEGIN
		DROP TABLE dbo.Generos;
		PRINT 'Tabla Generos eliminada correctamente';
	END

COMMIT TRANSACTION;--Confirmar cambios
END TRY
BEGIN CATCH
	
	IF @@TRANCOUNT > 0
		ROLLBACK TRANSACTION;
	THROW;
END CATCH
GO


/* -----Tabla Albumes----- */
BEGIN TRANSACTION;
BEGIN TRY
	
	IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Albumes' AND TABLE_SCHEMA = 'dbo')
	BEGIN
		DROP TABLE dbo.Albumes;
		PRINT 'Tabla Albumes eliminada correctamente';
	END

COMMIT TRANSACTION;--Confirmar cambios
END TRY
BEGIN CATCH
	
	IF @@TRANCOUNT > 0
		ROLLBACK TRANSACTION;
	THROW;

END CATCH
GO


/* -----Tabla Artistas----- */
BEGIN TRANSACTION;
BEGIN TRY

	IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Artistas' AND TABLE_SCHEMA = 'dbo')
	BEGIN
		DROP TABLE dbo.Artistas;
		PRINT 'Tabla Artistas eliminada correctamente';
	END

COMMIT TRANSACTION;--Confirmar cambios
END TRY
BEGIN CATCH

	IF @@TRANCOUNT > 0
		ROLLBACK TRANSACTION;
	THROW;

END CATCH
GO



/* -----Tabla Usuarios----- */
BEGIN TRANSACTION;
BEGIN TRY
	
	IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Usuarios' AND TABLE_SCHEMA = 'dbo')
	BEGIN
		DROP TABLE dbo.Usuarios;
		PRINT 'Tabla Usuarios eliminada correctamente';
	END

COMMIT TRANSACTION;--Confirmar cambios
END TRY
BEGIN CATCH
	
	IF @@TRANCOUNT > 0
		ROLLBACK TRANSACTION;
	THROW;

END CATCH
GO

/* -----Tabla Roles----- */
BEGIN TRANSACTION;
BEGIN TRY

	IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Roles' AND TABLE_SCHEMA = 'dbo')
	BEGIN
		DROP TABLE dbo.Roles;
		PRINT 'Tabla Roles eliminada correctamente';
	END

COMMIT TRANSACTION;--Confirmar cambios
END TRY
BEGIN CATCH

	IF @@TRANCOUNT > 0
		ROLLBACK TRANSACTION;
	THROW;

END CATCH
GO