USE sputiffy;
GO

-- =========================================
-- TABLA: Roles
-- =========================================
CREATE TABLE Roles (
    IdRol INT IDENTITY(1,1) PRIMARY KEY,
    NombreRol VARCHAR(50) NOT NULL UNIQUE,
    Activo BIT NOT NULL DEFAULT 1
);
GO

-- =========================================
-- TABLA: Usuarios
-- Login sin contrase�a hasheada (solo texto plano)
-- =========================================
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
GO

-- =========================================
-- TABLA: Generos
-- =========================================
CREATE TABLE Generos (
    IdGenero INT IDENTITY(1,1) PRIMARY KEY,
    NombreGenero VARCHAR(100) NOT NULL UNIQUE,
    Descripcion VARCHAR(250) NULL,
    Activo BIT NOT NULL DEFAULT 1
);
GO

-- =========================================
-- TABLA: Artistas
-- =========================================
CREATE TABLE Artistas (
    IdArtista INT IDENTITY(1,1) PRIMARY KEY,
    NombreArtistico VARCHAR(150) NOT NULL,
    PaisOrigen VARCHAR(80) NULL,
    FechaDebut DATE NULL,
    FotoUrl VARCHAR(500) NULL,
    Activo BIT NOT NULL DEFAULT 1
);
GO

-- =========================================
-- TABLA: Albumes
-- =========================================
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
GO

-- =========================================
-- TABLA: Canciones
-- =========================================
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
GO

-- =========================================
-- TABLA: Playlists
-- =========================================
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
GO

-- =========================================
-- TABLA: PlaylistDetalle
-- =========================================
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
GO

-- =========================================
-- TABLA: Favoritos
-- =========================================
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
GO

-- =========================================
-- TABLA: HistorialReproduccion
-- =========================================
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
GO

-- =========================================
-- TABLA: RecomendacionesUsuario
-- Guarda sugerencias precalculadas por a�o/d�cada
-- =========================================
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
GO

-- =========================================
-- TABLA: PreferenciasUsuario
-- Para guardar d�cada favorita, g�nero favorito, etc.
-- =========================================
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
GO

-- TABLA PARA LISTA DE CANCIONES EAEM *AGREGACI�N*
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

-- CANCIONES OFFLINE CFL
CREATE TABLE CancionesOffline (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Titulo NVARCHAR(200) NOT NULL,
    Artista NVARCHAR(200) NULL,
    Album NVARCHAR(200) NULL,
    RutaArchivo NVARCHAR(500) NOT NULL,
    Duracion INT NULL,
    FechaRegistro DATETIME NOT NULL DEFAULT GETDATE()
);
GO