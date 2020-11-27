/****** Object:  StoredProcedure [dbo].[ConsultaStock]    Script Date: 14/3/2020 10:31:52 ******/
DROP PROCEDURE [dbo].[ConsultaStockRemitos]
GO

-- =============================================
-- Author:		Ciro
-- Create date: 2020/11/25
-- Description:	SP para consulta de stock para remitos
-- =============================================
ALTER  PROCEDURE [dbo].[ConsultaStockRemitos]
	-- Add the parameters for the stored procedure here
	@pPatente VARCHAR(15),@pLegajo INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT S.ARTICULOSSTOCKID, S.CODIGOBARRA, A.DESCRIPCION ARTICULO, M.DESCRIPCION MARCA, MO.DESCRIPCION MODELO, V.DESCRIPCION, V.MOTOR, VT.DESCRIPCION TIPOVEHICULO, V.ANIO, V.PATENTE, 
	S.ARTICULOSID, S.VEHICULOSID
	FROM ArticulosStock S INNER JOIN Vehiculos V ON S.VEHICULOSID = V.VEHICULOSID
	INNER JOIN Marcas M ON V.MARCASID = M.MARCASID
	INNER JOIN Modelos MO ON V.MODELOSID = MO.MODELOSID
	INNER JOIN Articulos A ON S.ARTICULOSID = A.ARTICULOSID
	INNER JOIN VehiculosTipo VT ON V.VEHICULOSTIPOID = VT.VEHICULOSTIPOID	
	INNER JOIN Formulario04D F ON V.VEHICULOSID = F.VEHICULOSID
	WHERE V.PATENTE = @pPatente OR F.NROLEGAJO = @pLegajo
	AND S.ESTADOSID = 2 AND S.SECTORESID = 0
END



