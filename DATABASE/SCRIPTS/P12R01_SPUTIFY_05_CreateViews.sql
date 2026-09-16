/*
 P12R01_Sputify
 Autores: Adrián Manuel Meneses Lopez, Adrián Eleuterio Hernández Martínez
 FECHA: 15/09/2026
*/

--=============================
-- CREACIÓN DE VISTAS
--=============================

USE sputiffy;
GO

-- CREACIÓN DE VISTAS
CREATE OR ALTER VIEW vw_Usuarios_AnioBaseRecomendacion
AS
SELECT
    U.IdUsuario,
    U.Nombre,
    U.NombreUsuario,
    U.Correo,
    U.FechaNacimiento,
    YEAR(U.FechaNacimiento) AS AnioNacimiento,
    CASE
        WHEN YEAR(U.FechaNacimiento) < 1980 THEN 1980
        ELSE YEAR(U.FechaNacimiento)
    END AS AnioBaseRecomendacion
FROM Usuarios U;
GO

SELECT TOP 20
    C.IdCancion,
    C.Titulo,
    C.AnioLanzamiento,
    A.NombreArtistico,
    G.NombreGenero
FROM vw_Usuarios_AnioBaseRecomendacion V
INNER JOIN Canciones C
    ON C.AnioLanzamiento = V.AnioBaseRecomendacion
INNER JOIN Artistas A
    ON C.IdArtista = A.IdArtista
LEFT JOIN Generos G
    ON C.IdGenero = G.IdGenero
WHERE V.IdUsuario = 1
ORDER BY C.Reproducciones DESC, C.Titulo ASC;
GO
