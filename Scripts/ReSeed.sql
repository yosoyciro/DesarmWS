--Empiezo por las tablas que se cargan en el data solamente
DECLARE @PedidosWebId INT
SELECT @PedidosWebId = MAX(PEDIDOSWEBID) + 1 FROM PedidosWeb
EXECUTE [dbo].[ReseedIdentity] 'PedidosWeb','PEDIDOSWEBID',@PedidosWebId

DECLARE @PedidosWebDetalleId INT
SELECT @PedidosWebDetalleId = MAX(PEDIDOSWEBDETALLEID) + 1 FROM PedidosWebDetalle
EXECUTE  [dbo].[ReseedIdentity] 'PedidosWebDetalle','PEDIDOSWEBDETALLEID',@PedidosWebDetalleId

DECLARE @PedidosWebFormasPagoId INT
SELECT @PedidosWebFormasPagoId = MAX(PEDIDOSWEBFORMASPAGOID) + 1 FROM PedidosWebFormasPago
EXECUTE [dbo].[ReseedIdentity] 'PedidosWebFormasPago','PEDIDOSWEBFORMASPAGOID',@PedidosWebFormasPagoId

DECLARE @PedidosWebTarjetasCuponesId INT
SELECT @PedidosWebTarjetasCuponesId = MAX(PEDIDOSWEBTARJETASCUPONESID) + 1 FROM PedidosWebTarjetasCupones
EXECUTE [dbo].[ReseedIdentity] 'PedidosWebTarjetasCupones','PEDIDOSWEBTARJETASCUPONESID',@PedidosWebTarjetasCuponesId


--Aca van las tablas que se cargan en ambos lados
EXECUTE[dbo].[ReseedIdentity] 'Personas','PERSONASID',0
EXECUTE [dbo].[ReseedIdentity] 'PedidosWebArchivos','PEDIDOSWEBARCHIVOSID',0
EXECUTE [dbo].[ReseedIdentity] 'Vehiculos','VEHICULOSID',0;
EXECUTE [dbo].[ReseedIdentity] 'Localidades','LOCALIDADESID',5001;