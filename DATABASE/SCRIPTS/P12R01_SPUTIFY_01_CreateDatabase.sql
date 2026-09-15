
/*
 P12R01_Sputify
 Autores: Adrián Manuel Meneses Lopez, Adrián Eleuterio Hernández Martínez
 FECHA: 15/09/2026
*/

--=============================
-- CREACIÓN DE LA BASE DE DATOS
--=============================
IF NOT EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = N'sputiffy')
BEGIN
		CREATE DATABASE sputiffy;
		PRINT 'Base de datos sputiffy creada correctamente.';
END
ELSE
BEGIN
	PRINT 'la base de datos sputiffy ya existe.';
END
GO
