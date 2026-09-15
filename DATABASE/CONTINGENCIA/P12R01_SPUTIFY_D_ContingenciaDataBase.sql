BEGIN TRY
	IF EXISTS (SELECT 1 FROM sys.databases WHERE name = 'sputiffy')
		BEGIN
			USE master;
			ALTER DATABASE  sputiffy SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
			DROP DATABASE sputiffy;

			PRINT 'Base de Datos sputiffy eliminada correctamente';
		END
	ELSE
		BEGIN
			PRINT 'La Base de Datos [sputiffy] no existe'
		END
END TRY
BEGIN CATCH
	--Intentar restaurar acceso multi-usuario si algo falló
	IF EXISTS (SELECT 1 FROM sys.databases WHERE name = 'sputiffy')
		ALTER DATABASE sputiffy SET MULTI_USER;
	THROW;
END CATCH
GO