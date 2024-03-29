﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;

namespace DAL.CRUD.PedidosWeb
{
    public class CRUDPedidosWeb
    {
        #region Singleton
        private static ISession session;
        public static CRUDPedidosWeb instancia = new CRUDPedidosWeb();

        private CRUDPedidosWeb()
        {
            session = DAL.Sesion.GenerarSesion.Instancia.Session;
        }
        #endregion        

        #region Guardar
        public async Task<BE.Pedidos.PedidosWeb> Guardar(BE.Pedidos.PedidosWeb pPedidosWeb)
        {
            ITransaction transaction = session.BeginTransaction();
            try
            {
                //Persona
                //var persona = session.Get<BE.Pedidos.Personas>(pPedidosWeb.Persona.PERSONASID);
                var persona = session.Query<BE.Pedidos.Personas>().Where(a => a.TIPOSDOCUMENTOID == pPedidosWeb.Persona.TIPOSDOCUMENTOID && a.NRODOCUMENTO == pPedidosWeb.Persona.NRODOCUMENTO).SingleOrDefault();
                if (persona == null)
                {
                    persona = pPedidosWeb.Persona;
                    await session.SaveAsync(persona);                    
                    pPedidosWeb.PERSONASID = persona.PERSONASID;
                    pPedidosWeb.Persona.PERSONASID = persona.PERSONASID;
                }                   
                else
                {
                    persona = pPedidosWeb.Persona;
                    pPedidosWeb.PERSONASID = persona.PERSONASID;
                    await session.MergeAsync(persona);
                }
                    
                //Guardo el pedido
                await session.SaveAsync(pPedidosWeb);

                //Detalle
                int pedidosWebId = pPedidosWeb.PEDIDOSWEBID;                
                foreach (var item in pPedidosWeb.PedidosWebDetalle)
                {
                    var pedidosWebDetalle = await session.GetAsync<BE.Pedidos.PedidosWebDetalle>(item.PEDIDOSWEBDETALLEID);
                    if (pedidosWebDetalle == null)
                    {
                        item.PEDIDOSWEBID = pedidosWebId;
                        await session.SaveAsync(item);
                    }
                    
                }

                //Formas Pago
                foreach (var item in pPedidosWeb.PedidosWebFormaPago)
                {
                    var pedidosWebFormasPago = await session.GetAsync<BE.Pedidos.PedidosWebFormasPago>(item.PEDIDOSWEBFORMASPAGOID);
                    if (pedidosWebFormasPago == null)
                    {
                        var tarjetaCupon = item.PedidosWebTarjetasCupones;

                        item.PEDIDOSWEBID = pedidosWebId;
                        await session.SaveAsync(item);

                        if (tarjetaCupon != null)
                        {
                            tarjetaCupon.PEDIDOSWEBFORMASPAGOID = item.PEDIDOSWEBFORMASPAGOID;
                            await session.SaveAsync(tarjetaCupon);
                        }
                    }

                }

                switch (pPedidosWeb.PedidosWebArchivos.ARCHIVO)
                {
                    case null:
                        break;

                    default:
                        pPedidosWeb.PedidosWebArchivos.PEDIDOSWEBID = pedidosWebId;
                        await session.SaveAsync(pPedidosWeb.PedidosWebArchivos);
                        break;
                }

                session.Flush();
                await transaction.CommitAsync();                
                return pPedidosWeb;
            }
            catch (Exception e)
            {
                transaction.Rollback();
                throw e;
            }
        }
        #endregion

        #region Consultar
        public IList<BE.Pedidos.PedidosWeb> ConsultarPorNombre(string pNombre)
        {
            session.Clear();
            //IList<BE.Pedidos.PedidosWeb> pedidosWeb = session.Query<BE.Pedidos.PedidosWeb>()
            //    .Where(a => a.Persona.NOMBRE.Contains(pNombre))
            //    .ToList();

            //return pedidosWeb;
            List<int> personasId = new List<int>();

            IList<BE.Pedidos.Personas> personas = session.Query<BE.Pedidos.Personas>()
                .Where(a => a.NOMBRE.Contains(pNombre))
                .ToList();

            switch (personas.Count)
            {
                case 0:
                    return null;
                default:
                    foreach (var item in personas)
                    {
                        personasId.Add(item.PERSONASID);
                    }
                    break;
            }

            IList<BE.Pedidos.PedidosWeb> pedidosWeb = session.Query<BE.Pedidos.PedidosWeb>()
                .Where(a => personasId.Contains(a.PERSONASID))
                //.Where(a => a.NOMBRE.Contains(pNombre))
                .ToList();

            switch (pedidosWeb.Count)
            {
                case 0:
                    break;

                default:
                    foreach (var item in pedidosWeb)
                    {
                        item.PedidosWebDetalle = session.Query<BE.Pedidos.PedidosWebDetalle>()
                            .Where(a => a.PEDIDOSWEBID == item.PEDIDOSWEBID)
                            .ToList();

                        item.PedidosWebArchivos = session.Query<BE.Pedidos.PedidosWebArchivos>()
                            .Where(a => a.PEDIDOSWEBID == item.PEDIDOSWEBID && a.TIPO == "Factura")
                            .SingleOrDefault();
                    }
                    break;
            }
            
            return pedidosWeb;
        }

        public IList<BE.Pedidos.PedidosWeb> ConsultarPorFecha(int pFechaDesde, int pFechaHasta)
        {
            session.Clear();

            IList<BE.Pedidos.PedidosWeb> pedidosWeb = session.Query<BE.Pedidos.PedidosWeb>()
                .Where(a => a.FECHAPEDIDO >= pFechaDesde && a.FECHAPEDIDO <= pFechaHasta)
                //.Where(a => a.NOMBRE.Contains(pNombre))
                .ToList();

            switch (pedidosWeb.Count)
            {
                case 0:
                    break;

                default:
                    foreach (var item in pedidosWeb)
                    {
                        item.PedidosWebDetalle = session.Query<BE.Pedidos.PedidosWebDetalle>()
                            .Where(a => a.PEDIDOSWEBID == item.PEDIDOSWEBID)
                            .ToList();

                        //Los archivos se generan sobre el pedido que se consulta solamente
                        //item.PedidosWebArchivos = session.Query<BE.Pedidos.PedidosWebArchivos>()
                        //    .Where(a => a.PEDIDOSWEBID == item.PEDIDOSWEBID && a.TIPO == "Factura")
                        //    .SingleOrDefault();
                    }
                    break;
            }

            return pedidosWeb;
        }

        
        #endregion
    }
}
