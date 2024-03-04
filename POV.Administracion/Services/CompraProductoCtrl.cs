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
    public class CompraProductoCtrl : IDisposable
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
        public CompraProductoCtrl(Contexto contexto)
        {
            _sing = new object();
            _model = contexto ?? new Contexto(_sing);
        }

        /// <summary>
        /// Consulta la CompraProducto que existe en el sistema
        /// </summary>
        /// <param name="criteria">CompraProducto que provee el criterio de búsqueda</param>
        /// <param name="tracking">Permite saber si se rastrearán los objetos que se obtengan de la consulta</param>
        /// <returns>CompraProducto que satisfacen el criterio de búsqueda</returns>
        public List<CompraProducto> Retrieve(CompraProducto criteria, bool tracking) 
        {
            DbQuery<CompraProducto> queryCompraProducto = (tracking) ? _model.CompraProducto : _model.CompraProducto.AsNoTracking();
            return queryCompraProducto.Where(new CompraProductoQry(criteria).Action()).ToList();
        }

        /// <summary>
        /// Agrega un CompraProducto al sistema
        /// </summary>
        /// <param name="compraProducto"> CompraProducto que se registrará</param>
        /// <returns> Resultado del registro</returns>
        public Boolean Insert(CompraProducto compraProducto) 
        {
            var resultado = false;

            #region validaciones
            if (compraProducto == null) throw new ArgumentNullException("CompraProducto", "CompraProducto no puede ser nulo");
            if (compraProducto.CostoCompra == null) throw new ArgumentException("compraProducto.CostoCompra", "CostoCompra no puede ser nulo");
            if (compraProducto.FechaCompra == null) throw new ArgumentException("compraProducto.FechaCompra", "FechaCompra no puede ser nulo");
            if (string.IsNullOrEmpty(compraProducto.CodigoCompra)) throw new ArgumentException("compraProducto.CodigoCompra", "CodigoCompra no puede ser nulo");
            #endregion

            try
            {
                _model.CompraProducto.Add(compraProducto);
                var afectados = _model.Commit(_sing);

                if (afectados != 0) resultado = true;
            }
            catch (Exception exception)
            {
                throw exception;
            }

            return resultado;
        }

        /// <summary>
        /// Actualiza un CompraProducto
        /// </summary>
        /// <param name="compraProducto">CompraProducto a actualizar</param>
        /// <returns></returns>
        public Boolean Update(CompraProducto compraProducto) 
        {
            var resultao = false;

            #region validaciones
            if (compraProducto == null) throw new ArgumentNullException("CompraPremium", "CompraPremium no puede ser nulo");
            if (compraProducto.CompraProductoID == null) throw new ArgumentNullException("compraProducto.CompraProductoID", "Identificador de compra producto no puede ser nulo");
            #endregion

            var afectados = _model.Commit(_sing);
            if (afectados != 0) resultao = true;

            return resultao;
        }

        /// <summary>
        /// Método para cerrar la conexion del controlador con la base de datos
        /// </summary>
        public void Dispose() 
        {
            _model.Disposing(_sing);
        }
    }
}
