USE sputiffy;
GO

/*
 P12R01_Sputify
 Autores: Adrián Manuel Meneses Lopez, Adrián Eleuterio Hernández Martínez
 FECHA: 15/09/2026
*/

--=============================
-- CREACIÓN DE SP's
--=============================


-- =============================================
-- SP: Listar narraciones de un usuario
-- =============================================
CREATE OR ALTER PROCEDURE sp_Narraciones_ListarPorUsuario
    @IdUsuario INT,
    @SearchString VARCHAR(200) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        n.IdNarracion, n.VideoId, n.Titulo, n.Descripcion,
        n.CanalYoutube, n.ThumbnailUrl, n.UrlYoutube,
        n.DuracionSegundos, n.FechaPublicacion,
        n.Deporte, n.EquipoLocal, n.EquipoVisitante,
        n.Evento, n.FechaEvento, n.Marcador,
        n.IdUsuario, n.EsFavorito, n.Notas,
        n.FechaRegistro, n.Activo
    FROM NarracionesDeportivas n
    WHERE n.IdUsuario = @IdUsuario
      AND n.Activo = 1
      AND (
            @SearchString IS NULL OR @SearchString = ''
            OR n.Titulo LIKE '%' + @SearchString + '%'
            OR n.Deporte LIKE '%' + @SearchString + '%'
            OR n.EquipoLocal LIKE '%' + @SearchString + '%'
            OR n.EquipoVisitante LIKE '%' + @SearchString + '%'
            OR n.Evento LIKE '%' + @SearchString + '%'
          )
    ORDER BY n.FechaRegistro DESC;
END
GO

-- =============================================
-- SP: Guardar narración (evita duplicados por usuario+video)
-- =============================================
CREATE OR ALTER PROCEDURE sp_Narraciones_Guardar
    @IdUsuario        INT,
    @VideoId          VARCHAR(50),
    @Titulo           VARCHAR(300),
    @Descripcion      VARCHAR(1000) = NULL,
    @CanalYoutube     VARCHAR(200)  = NULL,
    @ThumbnailUrl     VARCHAR(500)  = NULL,
    @UrlYoutube       VARCHAR(500)  = NULL,
    @DuracionSegundos INT           = NULL,
    @FechaPublicacion DATE          = NULL,
    @Deporte          VARCHAR(80)   = NULL,
    @EquipoLocal      VARCHAR(150)  = NULL,
    @EquipoVisitante  VARCHAR(150)  = NULL,
    @Evento           VARCHAR(200)  = NULL,
    @FechaEvento      DATE          = NULL,
    @Marcador         VARCHAR(50)   = NULL
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @IdNarracionExistente INT;
    DECLARE @EstabaActivo BIT;

    -- Buscamos si ya existe el registro (activo o inactivo)
    SELECT TOP 1
        @IdNarracionExistente = IdNarracion,
        @EstabaActivo = Activo
    FROM NarracionesDeportivas
    WHERE IdUsuario = @IdUsuario
      AND VideoId = @VideoId
    ORDER BY IdNarracion DESC;

    -- CASO 1: Ya existe y está ACTIVO → no hacemos nada
    IF @IdNarracionExistente IS NOT NULL AND @EstabaActivo = 1
    BEGIN
        SELECT 0 AS ok,
               'Ya tienes esta narración guardada.' AS mensaje,
               @IdNarracionExistente AS IdNarracion;
        RETURN;
    END

    -- CASO 2: Ya existe pero estaba ELIMINADO → reactivamos y actualizamos
    IF @IdNarracionExistente IS NOT NULL AND @EstabaActivo = 0
    BEGIN
        UPDATE NarracionesDeportivas
        SET Titulo           = @Titulo,
            Descripcion      = @Descripcion,
            CanalYoutube     = @CanalYoutube,
            ThumbnailUrl     = @ThumbnailUrl,
            UrlYoutube       = @UrlYoutube,
            DuracionSegundos = @DuracionSegundos,
            FechaPublicacion = @FechaPublicacion,
            Deporte          = @Deporte,
            EquipoLocal      = @EquipoLocal,
            EquipoVisitante  = @EquipoVisitante,
            Evento           = @Evento,
            FechaEvento      = @FechaEvento,
            Marcador         = @Marcador,
            EsFavorito       = 0,
            FechaRegistro    = GETDATE(),
            Activo           = 1
        WHERE IdNarracion = @IdNarracionExistente;

        SELECT 1 AS ok,
               'Narración restaurada correctamente.' AS mensaje,
               @IdNarracionExistente AS IdNarracion;
        RETURN;
    END

    -- CASO 3: No existe → insertamos nuevo
    INSERT INTO NarracionesDeportivas
        (VideoId, Titulo, Descripcion, CanalYoutube, ThumbnailUrl, UrlYoutube,
         DuracionSegundos, FechaPublicacion, Deporte, EquipoLocal, EquipoVisitante,
         Evento, FechaEvento, Marcador, IdUsuario, EsFavorito, FechaRegistro, Activo)
    VALUES
        (@VideoId, @Titulo, @Descripcion, @CanalYoutube, @ThumbnailUrl, @UrlYoutube,
         @DuracionSegundos, @FechaPublicacion, @Deporte, @EquipoLocal, @EquipoVisitante,
         @Evento, @FechaEvento, @Marcador, @IdUsuario, 0, GETDATE(), 1);

    SELECT 1 AS ok,
           'Narración guardada correctamente.' AS mensaje,
           SCOPE_IDENTITY() AS IdNarracion;
END
GO

-- =============================================
-- SP: Eliminar narración (soft delete)
-- =============================================
CREATE OR ALTER PROCEDURE sp_Narraciones_Eliminar
    @IdNarracion INT,
    @IdUsuario INT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE NarracionesDeportivas
       SET Activo = 0
     WHERE IdNarracion = @IdNarracion
       AND IdUsuario = @IdUsuario;

    IF @@ROWCOUNT > 0
        SELECT 1 AS ok, 'Narración eliminada.' AS mensaje;
    ELSE
        SELECT 0 AS ok, 'No se encontró la narración.' AS mensaje;
END
GO

-- =============================================
-- SP: Toggle favorito
-- =============================================
CREATE OR ALTER PROCEDURE sp_Narraciones_ToggleFavorito
    @IdNarracion INT,
    @IdUsuario INT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE NarracionesDeportivas
       SET EsFavorito = CASE WHEN EsFavorito = 1 THEN 0 ELSE 1 END
     WHERE IdNarracion = @IdNarracion
       AND IdUsuario = @IdUsuario;

    SELECT 1 AS ok, 'Favorito actualizado.' AS mensaje;
END
GO



