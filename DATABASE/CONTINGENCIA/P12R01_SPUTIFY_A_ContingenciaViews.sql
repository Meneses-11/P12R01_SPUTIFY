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
	CONTINGENCIA DE VISTAS
  ==========================================*/

SELECT name FROM sys.views ORDER BY name;

BEGIN TRANSACTION;
	BEGIN TRY
	IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.VIEWS WHERE TABLE_NAME = 'vw_Usuarios_AnioBaseRecomendacion')
		BEGIN
			DROP VIEW dbo.vw_Usuarios_AnioBaseRecomendacion;
			PRINT 'Vista vw_Usuarios_AnioBaseRecomendacion eliminada correctamente';
		END
	ELSE
		BEGIN
			PRINT 'La vista [dbo].[vw_Usuarios_AnioBaseRecomendacion] no existe'
		END
	COMMIT TRANSACTION;
END TRY
BEGIN CATCH
	IF @@TRANCOUNT >0
		ROLLBACK TRANSACTION;
	THROW;
	END CATCH
GO