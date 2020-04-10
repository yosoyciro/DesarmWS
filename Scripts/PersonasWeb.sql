INSERT INTO [dbo].[PersonasWeb]([CODIGODOCUMENTO],[NRODOCUMENTO],[NOMBRE],[DIRECCION1],[DIRECCION2],[TELEFONO1],[TELEFONO2],[MAIL],[FECHANACIMIENTO],[LOCALIDADESID],[CODIGOSITUACIONIVA],[TIPOSPERSONAID])
SELECT CASE TIPOSDOCUMENTOID WHEN 15 THEN 80 WHEN 25 THEN 96 WHEN 26 THEN 86 END, NRODOCUMENTO, NOMBRE, DIRECCION1, '', TELEFONO1, '', MAIL, 
dbo.ConvFechaClarion(FECHANACIMIENTO), LOCALIDADESID, CASE SITUACIONIVAID WHEN 1 THEN 1 WHEN 3 THEN 4 when 4 then 5 when 5 then 6 end, TIPOSPERSONAID FROM Personas
WHERE NRODOCUMENTO NOT IN (SELECT NRODOCUMENTO FROM PersonasWeb)