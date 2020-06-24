﻿using NHibernate.Mapping.ByCode.Conformist;
using NHibernate.Mapping.ByCode;

namespace DAL.Mapeos
{
    public class MapPersonas : ClassMapping<BE.Pedidos.Personas>
    {
        public MapPersonas()
        {
            Id(x => x.PERSONASID, id =>
            {
                id.Generator(
                    Generators.Identity
                    );
            });
            Property(p => p.DNI);
            Property(p => p.NOMBRE);
            Property(p => p.DIRECCION1);
            Property(p => p.DIRECCION2);
            Property(p => p.TELEFONO1);
            Property(p => p.TELEFONO2);
            Property(p => p.MAIL);
            Property(p => p.CUIT);
            Property(p => p.FECHANACIMIENTO);                        
            Property(p => p.LOCALIDADESID);
            Property(p => p.SITUACIONIVAID);
            Property(p => p.TIPOSDOCUMENTOID);
            Property(p => p.OBSERVACIONES);
            Property(p => p.TIPOPERSONA);
            Property(p => p.NRODOCUMENTO);
            Property(p => p.TIPOSPERSONAID);
            Property(p => p.SEXO);
            //Property(p => p.ROWGUID);
            Property(p => p.FechaNacimientoS, m => {
                m.Formula("dbo.ConvFechaSQL(FechaNacimiento)");
            });
            Property(p => p.ESTADOCTACTE);
            //Property(p => p.BLOQUEOSID);
            Property(p => p.TIENECTACTE);
        }
    }
}
