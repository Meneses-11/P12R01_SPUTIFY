/*
 P12R01_Sputify
 Autores: Adrián Manuel Meneses Lopez, Adrián Eleuterio Hernández Martínez
 FECHA: 15/09/2026
*/

--=============================
-- CREACIÓN DE TABLAS
--=============================
USE sputiffy;
GO

-- =========================================
-- TABLA: Roles
-- =========================================
BEGIN TRANSACTION
	BEGIN TRY
		IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'Roles' AND TABLE_SCHEMA = 'dbo')
		BEGIN
			CREATE TABLE Roles (
                IdRol INT IDENTITY(1,1) PRIMARY KEY,
                NombreRol VARCHAR(50) NOT NULL UNIQUE,
                Activo BIT NOT NULL DEFAULT 1
            );
			PRINT 'Tabla Roles creada correctamente'
		END
		ELSE
		BEGIN
			PRINT 'Tabla Roles ya existe'
		END
	COMMIT TRANSACTION
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
			ROLLBACK TRANSACTION
		THROW;
	END CATCH
GO

-- =========================================
-- TABLA: Usuarios
-- Login sin contrase�a hasheada (solo texto plano)
-- =========================================
BEGIN TRANSACTION
	BEGIN TRY
		IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'Usuarios' AND TABLE_SCHEMA = 'dbo')
		BEGIN
			
            CREATE TABLE Usuarios (
                IdUsuario INT IDENTITY(1,1) PRIMARY KEY,
                Nombre VARCHAR(100) NOT NULL,
                ApellidoPaterno VARCHAR(100) NULL,
                ApellidoMaterno VARCHAR(100) NULL,
                NombreUsuario VARCHAR(50) NOT NULL UNIQUE,
                Correo VARCHAR(150) NOT NULL UNIQUE,
                Contrasena VARCHAR(100) NOT NULL,
                FechaNacimiento DATE NOT NULL,
                Pais VARCHAR(80) NULL,
                FechaRegistro DATETIME NOT NULL DEFAULT GETDATE(),
                Activo BIT NOT NULL DEFAULT 1,
                IdRol INT NOT NULL,
                CONSTRAINT FK_Usuarios_Roles
                    FOREIGN KEY (IdRol) REFERENCES Roles(IdRol)
            );
			PRINT 'Tabla Usuarios creada correctamente'
		END
		ELSE
		BEGIN
			PRINT 'Tabla Usuarios ya existe'
		END
	COMMIT TRANSACTION
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
			ROLLBACK TRANSACTION
		THROW;
	END CATCH
GO

-- =========================================
-- TABLA: Generos
-- =========================================
BEGIN TRANSACTION
	BEGIN TRY
		IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'Generos' AND TABLE_SCHEMA = 'dbo')
		BEGIN
            CREATE TABLE Generos (
                IdGenero INT IDENTITY(1,1) PRIMARY KEY,
                NombreGenero VARCHAR(100) NOT NULL UNIQUE,
                Descripcion VARCHAR(250) NULL,
                Activo BIT NOT NULL DEFAULT 1
            );
			PRINT 'Tabla Generos creada correctamente'
		END
		ELSE
		BEGIN
			PRINT 'Tabla Generos ya existe'
		END
	COMMIT TRANSACTION
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
			ROLLBACK TRANSACTION
		THROW;
	END CATCH
GO

-- =========================================
-- TABLA: Artistas
-- =========================================
BEGIN TRANSACTION
	BEGIN TRY
		IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'Artistas' AND TABLE_SCHEMA = 'dbo')
		BEGIN
            CREATE TABLE Artistas (
                IdArtista INT IDENTITY(1,1) PRIMARY KEY,
                NombreArtistico VARCHAR(150) NOT NULL,
                PaisOrigen VARCHAR(80) NULL,
                FechaDebut DATE NULL,
                FotoUrl VARCHAR(500) NULL,
                Activo BIT NOT NULL DEFAULT 1
            );
			PRINT 'Tabla Artistas creada correctamente'
		END
		ELSE
		BEGIN
			PRINT 'Tabla Artistas ya existe'
		END
	COMMIT TRANSACTION
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
			ROLLBACK TRANSACTION
		THROW;
	END CATCH
GO

-- =========================================
-- TABLA: Albumes
-- =========================================
BEGIN TRANSACTION
	BEGIN TRY
		IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'Albumes' AND TABLE_SCHEMA = 'dbo')
		BEGIN
            CREATE TABLE Albumes (
                IdAlbum INT IDENTITY(1,1) PRIMARY KEY,
                Titulo VARCHAR(200) NOT NULL,
                FechaLanzamiento DATE NULL,
                AnioLanzamiento INT NULL,
                PortadaUrl VARCHAR(500) NULL,
                IdArtista INT NOT NULL,
                Activo BIT NOT NULL DEFAULT 1,
                CONSTRAINT FK_Albumes_Artistas
                    FOREIGN KEY (IdArtista) REFERENCES Artistas(IdArtista)
            );
			PRINT 'Tabla Albumes creada correctamente'
		END
		ELSE
		BEGIN
			PRINT 'Tabla Albumes ya existe'
		END
	COMMIT TRANSACTION
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
			ROLLBACK TRANSACTION
		THROW;
	END CATCH
GO

-- =========================================
-- TABLA: Canciones
-- =========================================
BEGIN TRANSACTION
	BEGIN TRY
		IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'Canciones' AND TABLE_SCHEMA = 'dbo')
		BEGIN
            CREATE TABLE Canciones (
                IdCancion INT IDENTITY(1,1) PRIMARY KEY,
                Titulo VARCHAR(200) NOT NULL,
                DuracionSegundos INT NULL,
                AnioLanzamiento INT NOT NULL,
                PreviewUrl VARCHAR(500) NULL,
                AudioUrl VARCHAR(500) NULL,
                ImagenUrl VARCHAR(500) NULL,
                FuenteApi VARCHAR(50) NULL,
                IdExternoApi VARCHAR(100) NULL,
                IdArtista INT NOT NULL,
                IdAlbum INT NULL,
                IdGenero INT NULL,
                Reproducciones BIGINT NOT NULL DEFAULT 0,
                EsExplIcita BIT NOT NULL DEFAULT 0,
                Activo BIT NOT NULL DEFAULT 1,
                CONSTRAINT FK_Canciones_Artistas
                    FOREIGN KEY (IdArtista) REFERENCES Artistas(IdArtista),
                CONSTRAINT FK_Canciones_Albumes
                    FOREIGN KEY (IdAlbum) REFERENCES Albumes(IdAlbum),
                CONSTRAINT FK_Canciones_Generos
                    FOREIGN KEY (IdGenero) REFERENCES Generos(IdGenero)
            );
			PRINT 'Tabla Canciones creada correctamente'
		END
		ELSE
		BEGIN
			PRINT 'Tabla Canciones ya existe'
		END
	COMMIT TRANSACTION
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
			ROLLBACK TRANSACTION
		THROW;
	END CATCH
GO

-- =========================================
-- TABLA: Playlists
-- =========================================
BEGIN TRANSACTION
	BEGIN TRY
		IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'Playlists' AND TABLE_SCHEMA = 'dbo')
		BEGIN
            CREATE TABLE Playlists (
                IdPlaylist INT IDENTITY(1,1) PRIMARY KEY,
                NombrePlaylist VARCHAR(150) NOT NULL,
                Descripcion VARCHAR(300) NULL,
                FechaCreacion DATETIME NOT NULL DEFAULT GETDATE(),
                Publica BIT NOT NULL DEFAULT 1,
                IdUsuario INT NOT NULL,
                Activo BIT NOT NULL DEFAULT 1,
                CONSTRAINT FK_Playlists_Usuarios
                    FOREIGN KEY (IdUsuario) REFERENCES Usuarios(IdUsuario)
            );
			PRINT 'Tabla Playlists creada correctamente'
		END
		ELSE
		BEGIN
			PRINT 'Tabla Playlists ya existe'
		END
	COMMIT TRANSACTION
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
			ROLLBACK TRANSACTION
		THROW;
	END CATCH
GO

-- =========================================
-- TABLA: PlaylistDetalle
-- =========================================
BEGIN TRANSACTION
	BEGIN TRY
		IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'PlaylistDetalle' AND TABLE_SCHEMA = 'dbo')
		BEGIN
            CREATE TABLE PlaylistDetalle (
                IdPlaylistDetalle INT IDENTITY(1,1) PRIMARY KEY,
                IdPlaylist INT NOT NULL,
                IdCancion INT NOT NULL,
                FechaAgregado DATETIME NOT NULL DEFAULT GETDATE(),
                OrdenCancion INT NULL,
                CONSTRAINT FK_PlaylistDetalle_Playlists
                    FOREIGN KEY (IdPlaylist) REFERENCES Playlists(IdPlaylist),
                CONSTRAINT FK_PlaylistDetalle_Canciones
                    FOREIGN KEY (IdCancion) REFERENCES Canciones(IdCancion)
            );
			PRINT 'Tabla PlaylistDetalle creada correctamente'
		END
		ELSE
		BEGIN
			PRINT 'Tabla PlaylistDetalle ya existe'
		END
	COMMIT TRANSACTION
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
			ROLLBACK TRANSACTION
		THROW;
	END CATCH
GO

-- =========================================
-- TABLA: Favoritos
-- =========================================
BEGIN TRANSACTION
	BEGIN TRY
		IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'Favoritos' AND TABLE_SCHEMA = 'dbo')
		BEGIN
            CREATE TABLE Favoritos (
				IdFavorito INT IDENTITY(1,1) PRIMARY KEY,
				IdUsuario INT NOT NULL,
				IdCancion INT NOT NULL,
				FechaAgregado DATETIME NOT NULL DEFAULT GETDATE(),
				CONSTRAINT FK_Favoritos_Usuarios
					FOREIGN KEY (IdUsuario) REFERENCES Usuarios(IdUsuario),
				CONSTRAINT FK_Favoritos_Canciones
					FOREIGN KEY (IdCancion) REFERENCES Canciones(IdCancion),
				CONSTRAINT UQ_Favoritos_Usuario_Cancion UNIQUE (IdUsuario, IdCancion)
			);
			PRINT 'Tabla Favoritos creada correctamente'
		END
		ELSE
		BEGIN
			PRINT 'Tabla Favoritos ya existe'
		END
	COMMIT TRANSACTION
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
			ROLLBACK TRANSACTION
		THROW;
	END CATCH
GO

-- =========================================
-- TABLA: HistorialReproduccion
-- =========================================
BEGIN TRANSACTION
	BEGIN TRY
		IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'HistorialReproduccion' AND TABLE_SCHEMA = 'dbo')
		BEGIN
            CREATE TABLE HistorialReproduccion (
				IdHistorial INT IDENTITY(1,1) PRIMARY KEY,
				IdUsuario INT NOT NULL,
				IdCancion INT NOT NULL,
				FechaReproduccion DATETIME NOT NULL DEFAULT GETDATE(),
				SegundosEscuchados INT NULL,
				Completa BIT NOT NULL DEFAULT 0,
				CONSTRAINT FK_Historial_Usuarios
					FOREIGN KEY (IdUsuario) REFERENCES Usuarios(IdUsuario),
				CONSTRAINT FK_Historial_Canciones
					FOREIGN KEY (IdCancion) REFERENCES Canciones(IdCancion)
			);
			PRINT 'Tabla HistorialReproduccion creada correctamente'
		END
		ELSE
		BEGIN
			PRINT 'Tabla HistorialReproduccion ya existe'
		END
	COMMIT TRANSACTION
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
			ROLLBACK TRANSACTION
		THROW;
	END CATCH
GO

-- =========================================
-- TABLA: RecomendacionesUsuario
-- Guarda sugerencias precalculadas por año/década
-- =========================================
BEGIN TRANSACTION
	BEGIN TRY
		IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'RecomendacionesUsuario' AND TABLE_SCHEMA = 'dbo')
		BEGIN
            CREATE TABLE RecomendacionesUsuario (
				IdRecomendacion INT IDENTITY(1,1) PRIMARY KEY,
				IdUsuario INT NOT NULL,
				IdCancion INT NOT NULL,
				Motivo VARCHAR(200) NOT NULL,
				Puntaje DECIMAL(10,2) NOT NULL DEFAULT 0,
				FechaGenerada DATETIME NOT NULL DEFAULT GETDATE(),
				Mostrada BIT NOT NULL DEFAULT 0,
				CONSTRAINT FK_Recomendaciones_Usuarios
					FOREIGN KEY (IdUsuario) REFERENCES Usuarios(IdUsuario),
				CONSTRAINT FK_Recomendaciones_Canciones
					FOREIGN KEY (IdCancion) REFERENCES Canciones(IdCancion)
			);
			PRINT 'Tabla RecomendacionesUsuario creada correctamente'
		END
		ELSE
		BEGIN
			PRINT 'Tabla RecomendacionesUsuario ya existe'
		END
	COMMIT TRANSACTION
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
			ROLLBACK TRANSACTION
		THROW;
	END CATCH
GO

-- =========================================
-- TABLA: PreferenciasUsuario
-- Para guardar década favorita, género favorito, etc.
-- =========================================
BEGIN TRANSACTION
	BEGIN TRY
		IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'PreferenciasUsuario' AND TABLE_SCHEMA = 'dbo')
		BEGIN
            CREATE TABLE PreferenciasUsuario (
				IdPreferencia INT IDENTITY(1,1) PRIMARY KEY,
				IdUsuario INT NOT NULL,
				DecadaPreferida INT NULL,
				IdGeneroFavorito INT NULL,
				ModoRecomendacion VARCHAR(50) NOT NULL DEFAULT 'ANIO_NACIMIENTO',
				CONSTRAINT FK_Preferencias_Usuarios
					FOREIGN KEY (IdUsuario) REFERENCES Usuarios(IdUsuario),
				CONSTRAINT FK_Preferencias_Generos
					FOREIGN KEY (IdGeneroFavorito) REFERENCES Generos(IdGenero)
			);
			PRINT 'Tabla PreferenciasUsuario creada correctamente'
		END
		ELSE
		BEGIN
			PRINT 'Tabla PreferenciasUsuario ya existe'
		END
	COMMIT TRANSACTION
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
			ROLLBACK TRANSACTION
		THROW;
	END CATCH
GO

--================================================
-- TABLA PARA LISTA DE CANCIONES EAEM *AGREGACIÓN*
--================================================
BEGIN TRANSACTION
	BEGIN TRY
		IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'PlaylistCanciones' AND TABLE_SCHEMA = 'dbo')
		BEGIN
            CREATE TABLE PlaylistCanciones (
				IdPlaylistCancion INT IDENTITY(1,1) PRIMARY KEY,
				IdPlaylist INT NOT NULL,
				VideoId VARCHAR(50) NOT NULL,
				Titulo VARCHAR(200) NOT NULL,
				Artista VARCHAR(200) NULL,
				Portada VARCHAR(500) NULL,
				FechaAgregado DATETIME NOT NULL DEFAULT GETDATE(),
				CONSTRAINT FK_PlaylistCanciones_Playlists
					FOREIGN KEY (IdPlaylist) REFERENCES Playlists(IdPlaylist)
			);
			PRINT 'Tabla PlaylistCanciones creada correctamente'
		END
		ELSE
		BEGIN
			PRINT 'Tabla PlaylistCanciones ya existe'
		END
	COMMIT TRANSACTION
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
			ROLLBACK TRANSACTION
		THROW;
	END CATCH
GO

--==========================
-- CANCIONES OFFLINE CFL
--==========================
BEGIN TRANSACTION
	BEGIN TRY
		IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'CancionesOffline' AND TABLE_SCHEMA = 'dbo')
		BEGIN
            CREATE TABLE CancionesOffline (
				Id INT IDENTITY(1,1) PRIMARY KEY,
				Titulo NVARCHAR(200) NOT NULL,
				Artista NVARCHAR(200) NULL,
				Album NVARCHAR(200) NULL,
				RutaArchivo NVARCHAR(500) NOT NULL,
				Duracion INT NULL,
				FechaRegistro DATETIME NOT NULL DEFAULT GETDATE()
			);
			PRINT 'Tabla CancionesOffline creada correctamente'
		END
		ELSE
		BEGIN
			PRINT 'Tabla CancionesOffline ya existe'
		END
	COMMIT TRANSACTION
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
			ROLLBACK TRANSACTION
		THROW;
	END CATCH
GO

--===================================
-- Tabla: AudiolibrosGuardados (AEHM)
--===================================

BEGIN TRANSACTION
	BEGIN TRY
		IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'AudiolibrosGuardados' AND TABLE_SCHEMA = 'dbo')
		BEGIN
            CREATE TABLE AudiolibrosGuardados (
                IdAudiolibro INT IDENTITY(1,1) PRIMARY KEY,
                IdUsuario INT NOT NULL,
                VideoId VARCHAR(50) NOT NULL,
                Titulo VARCHAR(200) NOT NULL,
                CanalTitulo VARCHAR(200) NULL,
                Descripcion VARCHAR(1000) NULL,
                FechaPublicacion DATETIME NULL,
                DuracionSegundos INT NULL,
                ThumbnailUrl VARCHAR(500) NULL,
                ThumbnailMediumUrl VARCHAR(500) NULL,
                ThumbnailHighUrl VARCHAR(500) NULL,
                TipoContenido VARCHAR(50) NOT NULL DEFAULT 'AUDIOLIBRO',
                FechaGuardado DATETIME NOT NULL DEFAULT GETDATE(),
                Activo BIT NOT NULL DEFAULT 1,
                CONSTRAINT FK_Audiolibros_Usuarios
                    FOREIGN KEY (IdUsuario) REFERENCES Usuarios(IdUsuario),
                CONSTRAINT UQ_Audiolibros_Usuario_Video
                    UNIQUE (IdUsuario, VideoId)
            );
			PRINT 'Tabla AudiolibrosGuardados creada correctamente'
		END
		ELSE
		BEGIN
			PRINT 'Tabla AudiolibrosGuardados ya existe'
		END
	COMMIT TRANSACTION
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
			ROLLBACK TRANSACTION
		THROW;
	END CATCH
GO