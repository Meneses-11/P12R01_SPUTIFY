USE sputiffy;
GO

-- =========================================
-- DATOS INICIALES
-- =========================================
INSERT INTO Roles (NombreRol) VALUES
('Administrador'),
('Usuario');
GO

INSERT INTO Generos (NombreGenero, Descripcion) VALUES
('Pop', 'Musica pop'),
('Rock', 'Musica rock'),
('Balada', 'Baladas romanticas'),
('Electronica', 'Musica electronica'),
('Regional Mexicano', 'Banda, norte�o, mariachi, etc.'),
('Rap', 'Hip hop y rap'),
('Clasica', 'Musica clasica'),
('Jazz', 'Jazz y derivados');
GO

-- Usuario admin inicial
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
GO


-- INSERTAR CANCIONES OFFLINE CFL
INSERT INTO CancionesOffline (Titulo, Artista, Album, RutaArchivo, Duracion, FechaRegistro)
VALUES
('Believer', 'Imagine Dragons', 'Evolve', 'C:\Musica\Believer.mp3', 204, GETDATE()),
('Numb', 'Linkin Park', 'Meteora', 'C:\Musica\Numb.mp3', 187, GETDATE()),
('In The End', 'Linkin Park', 'Hybrid Theory', 'C:\Musica\InTheEnd.mp3', 216, GETDATE());
GO
