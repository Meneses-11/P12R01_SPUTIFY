USE sputiffy;
GO

/*
P12R01_SPUTIFY
AUTORES: 
Adrian Manuel Meneses López
Adrian Heleuterio Hernandez Martinez
FECHA: 16/09/2026
*/

/*==========================================
	CONTINGENCIA DE SPs
  ==========================================*/

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_NAME = 'sp_Narraciones_Eliminar' AND ROUTINE_SCHEMA = 'dbo') 
BEGIN

	DROP PROCEDURE [dbo].[sp_Narraciones_Eliminar];

	PRINT 'Procedimiento [sp_Narraciones_Eliminar] eliminado correctamente.'

END
ELSE
BEGIN

	PRINT 'El procedimiento [dbo].[sp_Narraciones_Eliminar] no existe.'

END

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_NAME = 'sp_Narraciones_Guardar' AND ROUTINE_SCHEMA = 'dbo') 
BEGIN

	DROP PROCEDURE [dbo].[sp_Narraciones_Guardar];

	PRINT 'Procedimiento [sp_Narraciones_Guardar] eliminado correctamente.'

END
ELSE
BEGIN

	PRINT 'El procedimiento [dbo].[sp_Narraciones_Guardar] no existe.'

END

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_NAME = 'sp_Narraciones_ListarPorUsuario' AND ROUTINE_SCHEMA = 'dbo') 
BEGIN

	DROP PROCEDURE [dbo].[sp_Narraciones_ListarPorUsuario];

	PRINT 'Procedimiento [sp_Narraciones_ListarPorUsuario] eliminado correctamente.'

END
ELSE
BEGIN

	PRINT 'El procedimiento [dbo].[sp_Narraciones_ListarPorUsuario] no existe.'

END

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_NAME = 'sp_Narraciones_ToggleFavorito' AND ROUTINE_SCHEMA = 'dbo') 
BEGIN

	DROP PROCEDURE [dbo].[sp_Narraciones_ToggleFavorito];

	PRINT 'Procedimiento [sp_Narraciones_ToggleFavorito] eliminado correctamente.'

END
ELSE
BEGIN

	PRINT 'El procedimiento [dbo].[sp_Narraciones_ToggleFavorito] no existe.'

END