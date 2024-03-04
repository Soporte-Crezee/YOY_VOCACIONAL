using POV.CentroEducativo.BO;
using POV.CentroEducativo.Queries;
using POV.Modelo.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POV.CentroEducativo.Services
{
    public class NotaCompraCtrl : IDisposable
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
        public NotaCompraCtrl(Contexto contexto) 
        {
            _sing = new object();
            _model = contexto ?? new Contexto(_sing);
        }

        /// <summary>
        /// Consulta la NotaCompra que existen en el sistema
        /// </summary>
        /// <param name="criteria"> NotaCompra que provee el criterio de búsqueda </param>
        /// <param name="tracking"> Permite saber si se rastrearán los objetos que se obtengan de la consulta </param>
        /// <returns> NotaCompra que satisfacen el criterio de búsqueda </returns>
        public List<NotaCompra> Retrieve(NotaCompra criteria, bool tracking) 
        {
            DbQuery<NotaCompra> notaCompraQry = (tracking) ? _model.NotaCompra : _model.NotaCompra.AsNoTracking();
            return notaCompraQry.Where(new NotaCompraQry(criteria).Action()).ToList();
        }

        /// <summary>
        /// Agrega una NotaCompra al sistema
        /// </summary>
        /// <param name="notaCompra"> NotaCompra que se registrará </param>
        /// <returns> Resultado del registro </returns>
        public Boolean Insert(NotaCompra notaCompra) 
        {
            var resultado = false;

            #region validaciones
            if (notaCompra == null) throw new ArgumentNullException("NotaCompra", "NotaCompra no puede ser nulo");
            if (notaCompra.FechaCompra == null) throw new ArgumentNullException("NotaCompra.FechaCompra", "Fecha de compra no puede ser nulo");
            if (notaCompra.CostoProductoID == null) throw new ArgumentNullException("NotaCompra.CostoProductoID", "CostoProductoID no puede ser nulo");
            if (notaCompra.FechaCompra == null) throw new ArgumentNullException("NotaCompra.FechaCompra", "FechaCompra no puede ser nulo");
            #endregion

            try
            {
                _model.NotaCompra.Add(notaCompra);
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
        /// Método para cerrar la conexión del controlador con la base de datos
        /// </summary>
        public void Dispose()
        {
            _model.Disposing(_sing);
        }
    }
}
