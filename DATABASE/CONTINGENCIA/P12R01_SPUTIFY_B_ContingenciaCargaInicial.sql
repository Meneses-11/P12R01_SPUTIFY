USE sputiffy;
GO

SELECT name FROM sys.tables ORDER BY name;
--------------------------------Albumes
BEGIN TRANSACTION
BEGIN TRY
	--VALIDACIONES
	IF EXISTS (
			SELECT 1
			FROM INFORMATION_SCHEMA.TABLES
			WHERE TABLE_NAME = 'Albumes'
				AND TABLE_SCHEMA = 'dbo'
			)
	BEGIN
		--ELIMINAR: Datos de Albumes
		DELETE
		FROM dbo.Albumes

		PRINT 'Datos de la tabla Albumes eliminados correctamente.'
	END
	ELSE
	BEGIN
		PRINT 'La tabla [dbo].[Albumes] no existe.'
	END

	COMMIT TRANSACTION;
END TRY

BEGIN CATCH
	IF @@TRANCOUNT > 0
		ROLLBACK TRANSACTION;

	THROW;
END CATCH
GO
---------------------------------------------Artistas
BEGIN TRANSACTION
BEGIN TRY
	--VALIDACIONES
	IF EXISTS (
			SELECT 1
			FROM INFORMATION_SCHEMA.TABLES
			WHERE TABLE_NAME = 'Artistas'
				AND TABLE_SCHEMA = 'dbo'
			)
	BEGIN
		--ELIMINAR: Datos de Albumes
		DELETE
		FROM dbo.Artistas

		PRINT 'Datos de la tabla Artistas eliminados correctamente.'
	END
	ELSE
	BEGIN
		PRINT 'La tabla [dbo].[Artistas] no existe.'
	END

	COMMIT TRANSACTION;
END TRY

BEGIN CATCH
	IF @@TRANCOUNT > 0
		ROLLBACK TRANSACTION;

	THROW;
END CATCH
GO
----------------- Canciones
BEGIN TRANSACTION
BEGIN TRY
	--VALIDACIONES
	IF EXISTS (
			SELECT 1
			FROM INFORMATION_SCHEMA.TABLES
			WHERE TABLE_NAME = 'Artistas'
				AND TABLE_SCHEMA = 'dbo'
			)
	BEGIN
		--ELIMINAR: Datos de Albumes
		DELETE
		FROM dbo.Artistas

		PRINT 'Datos de la tabla Artistas eliminados correctamente.'
	END
	ELSE
	BEGIN
		PRINT 'La tabla [dbo].[Artistas] no existe.'
	END

	COMMIT TRANSACTION;
END TRY

BEGIN CATCH
	IF @@TRANCOUNT > 0
		ROLLBACK TRANSACTION;

	THROW;
END CATCH
GO

----------------- CancionesOffline
BEGIN TRANSACTION
BEGIN TRY
	--VALIDACIONES
	IF EXISTS (
			SELECT 1
			FROM INFORMATION_SCHEMA.TABLES
			WHERE TABLE_NAME = 'CancionesOffline'
				AND TABLE_SCHEMA = 'dbo'
			)
	BEGIN
		--ELIMINAR: Datos de Albumes
		DELETE
		FROM dbo.CancionesOffline

		PRINT 'Datos de la tabla CancionesOffline eliminados correctamente.'
	END
	ELSE
	BEGIN
		PRINT 'La tabla [dbo].[CancionesOffline] no existe.'
	END

	COMMIT TRANSACTION;
END TRY

BEGIN CATCH
	IF @@TRANCOUNT > 0
		ROLLBACK TRANSACTION;

	THROW;
END CATCH
GO

----------------- Favoritos
BEGIN TRANSACTION
BEGIN TRY
	--VALIDACIONES
	IF EXISTS (
			SELECT 1
			FROM INFORMATION_SCHEMA.TABLES
			WHERE TABLE_NAME = 'Favoritos'
				AND TABLE_SCHEMA = 'dbo'
			)
	BEGIN
		--ELIMINAR: Datos de Albumes
		DELETE
		FROM dbo.Favoritos

		PRINT 'Datos de la tabla Favoritos eliminados correctamente.'
	END
	ELSE
	BEGIN
		PRINT 'La tabla [dbo].[Favoritos] no existe.'
	END

	COMMIT TRANSACTION;
END TRY

BEGIN CATCH
	IF @@TRANCOUNT > 0
		ROLLBACK TRANSACTION;

	THROW;
END CATCH
GO

----------------- Generos
BEGIN TRANSACTION
BEGIN TRY
	--VALIDACIONES
	IF EXISTS (
			SELECT 1
			FROM INFORMATION_SCHEMA.TABLES
			WHERE TABLE_NAME = 'Generos'
				AND TABLE_SCHEMA = 'dbo'
			)
	BEGIN
		--ELIMINAR: Datos de Albumes
		DELETE
		FROM dbo.Generos

		PRINT 'Datos de la tabla Generos eliminados correctamente.'
	END
	ELSE
	BEGIN
		PRINT 'La tabla [dbo].[Generos] no existe.'
	END

	COMMIT TRANSACTION;
END TRY

BEGIN CATCH
	IF @@TRANCOUNT > 0
		ROLLBACK TRANSACTION;

	THROW;
END CATCH
GO


----------------- HistorialReproduccion
BEGIN TRANSACTION
BEGIN TRY
	--VALIDACIONES
	IF EXISTS (
			SELECT 1
			FROM INFORMATION_SCHEMA.TABLES
			WHERE TABLE_NAME = 'HistorialReproduccion'
				AND TABLE_SCHEMA = 'dbo'
			)
	BEGIN
		--ELIMINAR: Datos de Albumes
		DELETE
		FROM dbo.HistorialReproduccion

		PRINT 'Datos de la tabla HistorialReproduccion eliminados correctamente.'
	END
	ELSE
	BEGIN
		PRINT 'La tabla [dbo].[HistorialReproduccion] no existe.'
	END

	COMMIT TRANSACTION;
END TRY

BEGIN CATCH
	IF @@TRANCOUNT > 0
		ROLLBACK TRANSACTION;

	THROW;
END CATCH
GO

----------------- HistorialReproduccion
BEGIN TRANSACTION
BEGIN TRY
	--VALIDACIONES
	IF EXISTS (
			SELECT 1
			FROM INFORMATION_SCHEMA.TABLES
			WHERE TABLE_NAME = 'HistorialReproduccion'
				AND TABLE_SCHEMA = 'dbo'
			)
	BEGIN
		--ELIMINAR: Datos de Albumes
		DELETE
		FROM dbo.HistorialReproduccion

		PRINT 'Datos de la tabla HistorialReproduccion eliminados correctamente.'
	END
	ELSE
	BEGIN
		PRINT 'La tabla [dbo].[HistorialReproduccion] no existe.'
	END

	COMMIT TRANSACTION;
END TRY

BEGIN CATCH
	IF @@TRANCOUNT > 0
		ROLLBACK TRANSACTION;

	THROW;
END CATCH
GO
----------------- PlaylistCanciones
BEGIN TRANSACTION
BEGIN TRY
	--VALIDACIONES
	IF EXISTS (
			SELECT 1
			FROM INFORMATION_SCHEMA.TABLES
			WHERE TABLE_NAME = 'PlaylistCanciones'
				AND TABLE_SCHEMA = 'dbo'
			)
	BEGIN
		--ELIMINAR: Datos de Albumes
		DELETE
		FROM dbo.PlaylistCanciones

		PRINT 'Datos de la tabla PlaylistCanciones eliminados correctamente.'
	END
	ELSE
	BEGIN
		PRINT 'La tabla [dbo].[PlaylistCanciones] no existe.'
	END

	COMMIT TRANSACTION;
END TRY

BEGIN CATCH
	IF @@TRANCOUNT > 0
		ROLLBACK TRANSACTION;

	THROW;
END CATCH
GO

----------------- PlaylistDetalle
BEGIN TRANSACTION
BEGIN TRY
	--VALIDACIONES
	IF EXISTS (
			SELECT 1
			FROM INFORMATION_SCHEMA.TABLES
			WHERE TABLE_NAME = 'PlaylistDetalle'
				AND TABLE_SCHEMA = 'dbo'
			)
	BEGIN
		--ELIMINAR: Datos de Albumes
		DELETE
		FROM dbo.PlaylistDetalle

		PRINT 'Datos de la tabla PlaylistDetalle eliminados correctamente.'
	END
	ELSE
	BEGIN
		PRINT 'La tabla [dbo].[PlaylistDetalle] no existe.'
	END

	COMMIT TRANSACTION;
END TRY

BEGIN CATCH
	IF @@TRANCOUNT > 0
		ROLLBACK TRANSACTION;

	THROW;
END CATCH
GO
----------------- Playlists
BEGIN TRANSACTION
BEGIN TRY
	--VALIDACIONES
	IF EXISTS (
			SELECT 1
			FROM INFORMATION_SCHEMA.TABLES
			WHERE TABLE_NAME = 'Playlists'
				AND TABLE_SCHEMA = 'dbo'
			)
	BEGIN
		--ELIMINAR: Datos de Albumes
		DELETE
		FROM dbo.Playlists

		PRINT 'Datos de la tabla Playlists eliminados correctamente.'
	END
	ELSE
	BEGIN
		PRINT 'La tabla [dbo].[Playlists] no existe.'
	END

	COMMIT TRANSACTION;
END TRY

BEGIN CATCH
	IF @@TRANCOUNT > 0
		ROLLBACK TRANSACTION;

	THROW;
END CATCH
GO
----------------- PreferenciasUsuario
BEGIN TRANSACTION
BEGIN TRY
	--VALIDACIONES
	IF EXISTS (
			SELECT 1
			FROM INFORMATION_SCHEMA.TABLES
			WHERE TABLE_NAME = 'PreferenciasUsuario'
				AND TABLE_SCHEMA = 'dbo'
			)
	BEGIN
		--ELIMINAR: Datos de Albumes
		DELETE
		FROM dbo.PreferenciasUsuario

		PRINT 'Datos de la tabla PreferenciasUsuario eliminados correctamente.'
	END
	ELSE
	BEGIN
		PRINT 'La tabla [dbo].[PreferenciasUsuario] no existe.'
	END

	COMMIT TRANSACTION;
END TRY

BEGIN CATCH
	IF @@TRANCOUNT > 0
		ROLLBACK TRANSACTION;

	THROW;
END CATCH
GO
----------------- RecomendacionesUsuario
BEGIN TRANSACTION
BEGIN TRY
	--VALIDACIONES
	IF EXISTS (
			SELECT 1
			FROM INFORMATION_SCHEMA.TABLES
			WHERE TABLE_NAME = 'RecomendacionesUsuario'
				AND TABLE_SCHEMA = 'dbo'
			)
	BEGIN
		--ELIMINAR: Datos de Albumes
		DELETE
		FROM dbo.RecomendacionesUsuario

		PRINT 'Datos de la tabla RecomendacionesUsuario eliminados correctamente.'
	END
	ELSE
	BEGIN
		PRINT 'La tabla [dbo].[RecomendacionesUsuario] no existe.'
	END

	COMMIT TRANSACTION;
END TRY

BEGIN CATCH
	IF @@TRANCOUNT > 0
		ROLLBACK TRANSACTION;

	THROW;
END CATCH
GO
----------------- Roles
BEGIN TRANSACTION
BEGIN TRY
	--VALIDACIONES
	IF EXISTS (
			SELECT 1
			FROM INFORMATION_SCHEMA.TABLES
			WHERE TABLE_NAME = 'Roles'
				AND TABLE_SCHEMA = 'dbo'
			)
	BEGIN
		--ELIMINAR: Datos de Roles
		DELETE
		FROM dbo.Roles

		PRINT 'Datos de la tabla Roles eliminados correctamente.'
	END
	ELSE
	BEGIN
		PRINT 'La tabla [dbo].[Roles] no existe.'
	END

	COMMIT TRANSACTION;
END TRY

BEGIN CATCH
	IF @@TRANCOUNT > 0
		ROLLBACK TRANSACTION;

	THROW;
END CATCH
GO

----------------- Usuarios
BEGIN TRANSACTION
BEGIN TRY
	--VALIDACIONES
	IF EXISTS (
			SELECT 1
			FROM INFORMATION_SCHEMA.TABLES
			WHERE TABLE_NAME = 'Usuarios'
				AND TABLE_SCHEMA = 'dbo'
			)
	BEGIN
		--ELIMINAR: Datos de Usuarios
		DELETE
		FROM dbo.Usuarios

		PRINT 'Datos de la tabla Usuarios eliminados correctamente.'
	END
	ELSE
	BEGIN
		PRINT 'La tabla [dbo].[Usuarios] no existe.'
	END

	COMMIT TRANSACTION;
END TRY

BEGIN CATCH
	IF @@TRANCOUNT > 0
		ROLLBACK TRANSACTION;

	THROW;
END CATCH
GO