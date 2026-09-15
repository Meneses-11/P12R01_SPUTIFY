USE sputiffy;
GO


/*
 P12R01_Sputify
 Autores: Adrián Manuel Meneses Lopez, Adrián Eleuterio Hernández Martínez
 FECHA: 15/09/2026
*/


-- =========================================
-- DATOS INICIALES
-- =========================================

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Roles' AND TABLE_SCHEMA = 'dbo')
BEGIN

    INSERT INTO Roles (NombreRol) VALUES
    ('Administrador'),
    ('Usuario');

END
ELSE PRINT('No existe la tabla Roles')
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Generos' AND TABLE_SCHEMA = 'dbo')
BEGIN
    INSERT INTO Generos (NombreGenero, Descripcion) VALUES
    ('Pop', 'Musica pop'),
    ('Rock', 'Musica rock'),
    ('Balada', 'Baladas romanticas'),
    ('Electronica', 'Musica electronica'),
    ('Regional Mexicano', 'Banda, norte�o, mariachi, etc.'),
    ('Rap', 'Hip hop y rap'),
    ('Clasica', 'Musica clasica'),
    ('Jazz', 'Jazz y derivados');
END
ELSE PRINT('No existe la tabla Generos')
GO

-- Usuario admin inicial

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Usuarios' AND TABLE_SCHEMA = 'dbo')
BEGIN

    INSERT INTO Usuarios
    (
        Nombre,
        ApellidoPaterno,
        ApellidoMaterno,
        NombreUsuario,
        Correo,
        Contrasena,
        FechaNacimiento,
        Pais,
        IdRol
    )
    VALUES
    (
        'Admin',
        'Sistema',
        '',
        'admin',
        'admin@sputiffy.com',
        '123456',
        '2000-01-01',
        'Mexico',
        1
    );

END
ELSE PRINT ('No existe la tabla Usuario')
GO


IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'CancionesOffline' AND TABLE_SCHEMA = 'dbo')
BEGIN

    -- INSERTAR CANCIONES OFFLINE CFL
    INSERT INTO CancionesOffline (Titulo, Artista, Album, RutaArchivo, Duracion, FechaRegistro)
    VALUES
    ('Believer', 'Imagine Dragons', 'Evolve', 'C:\Musica\Believer.mp3', 204, GETDATE()),
    ('Numb', 'Linkin Park', 'Meteora', 'C:\Musica\Numb.mp3', 187, GETDATE()),
    ('In The End', 'Linkin Park', 'Hybrid Theory', 'C:\Musica\InTheEnd.mp3', 216, GETDATE());

END
ELSE PRINT('No existe la tabla CancionesOffline')
GO