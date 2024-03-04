using POV.Administracion.BO;
using POV.Administracion.Queries;
using POV.Modelo.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POV.Administracion.Services
{
    public class CompraCreditoCtrl : IDisposable
    {
        /// <summary>
        /// Contexto interno de conexión a la base de datos
        /// </summary>
        private readonly Contexto _model;

        /// <summary>
        /// Firma de la conexión a la base de datos
        /// </summary>
        private readonly object _sing;

        /// <summary>
        /// Constructor encargado de crear la conexion interna a la base de datos
        /// </summary>
        /// <param name="contexto">Contexto de conexion a la base de datos</param>
        public CompraCreditoCtrl(Contexto contexto) 
        {
            _sing = new object();
            _model = contexto ?? new Contexto(_sing);
        }

        /// <summary>
        /// Consulta la CompraCredito que existe en el sistema
        /// </summary>
        /// <param name="criteria">CompraCredito que provee el criterio de búsqueda</param>
        /// <param name="tracking">Permite saber si se rastrearán los objetos que se obtengan de la consulta</param>
        /// <returns>CompraCredito que satisfacen el criterio de búsqueda</returns>
        public List<CompraCredito> Retrieve(CompraCredito criteria, bool tracking) 
        {
            DbQuery<CompraCredito> qryCompraCredito = (tracking) ? _model.CompraCredito : _model.CompraCredito.AsNoTracking();
            return qryCompraCredito.Where(new CompraCreditoQry(criteria).Action()).ToList();
        }

        /// <summary>
        /// Agrega un CompraCredito al sistema
        /// </summary>
        /// <param name="compraCredito"> CompraCredito que se registrará</param>
        /// <returns>Resultado del registro</returns>
        public Boolean Insert(CompraCredito compraCredito) 
        {
            var resultado = false;

            #region validaciones
            if (compraCredito == null) throw new ArgumentNullException("CompraCredito", "Compra Credito no puede ser nulo");
            if (compraCredito.CostoCompra == null) throw new ArgumentException("compraCredito.CostoCompra", "compraCredito.CostoCompra no puede ser nulo");
            if (compraCredito.FechaCompra == null) throw new ArgumentException("compraCredito.FechaCompra", "compraCredito.FechaCompra no puede ser nulo");
            if (string.IsNullOrEmpty(compraCredito.CodigoCompra)) throw new ArgumentException("compraCredito.CodigoCompra", "compraCredito.CodigoCompra no puede ser nulo");
            #endregion

            try
            {
                _model.CompraCredito.Add(compraCredito);
                var afectados = _model.Commit(_sing);

                if (afectados != 0) resultado = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return resultado;
        }

        /// <summary>
        /// Actualiza un CompraCredito
        /// </summary>
        /// <param name="compraCredito">CompraCredito a actualizar</param>
        /// <returns></returns>
        public Boolean Update(CompraCredito compraCredito) 
        {
            var resultado = false;

            #region Validaciones
            if (compraCredito == null) throw new ArgumentNullException("CompraCredito", "CompraCredito no puede ser nulo");
            if (compraCredito.CompraCreditoID == null) throw new ArgumentException("compraCredito.CompraCreditoID", "CompraCreditoID no puede ser nulo");
            #endregion

            var afectados = _model.Commit(_sing);
            if (afectados != 0) resultado = true;

            return resultado;
        }

        /// <summary>
        /// Método para cerrar la conexión del controlador con la base de datos
        /// </summary>
        public void Dispose() 
        {
            _model.Disposing(_sing);
        }
    }
}
