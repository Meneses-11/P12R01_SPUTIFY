USE sputiffy;
GO
-- =========================================
-- �NDICES
-- =========================================
CREATE INDEX IX_Canciones_AnioLanzamiento ON Canciones(AnioLanzamiento);
CREATE INDEX IX_Canciones_IdGenero ON Canciones(IdGenero);
CREATE INDEX IX_Canciones_IdArtista ON Canciones(IdArtista);
CREATE INDEX IX_HistorialReproduccion_IdUsuario ON HistorialReproduccion(IdUsuario);
CREATE INDEX IX_Favoritos_IdUsuario ON Favoritos(IdUsuario);
CREATE INDEX IX_RecomendacionesUsuario_IdUsuario ON RecomendacionesUsuario(IdUsuario);
GO
