using POV.Administracion.BO;
using POV.Administracion.Queries;
using POV.Modelo.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POV.Administracion.Services
{
    public class CompraPremiumCtrl : IDisposable
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
        public CompraPremiumCtrl(Contexto contexto)
        {
            _sing = new object();
            _model = contexto ?? new Contexto(_sing);
        }

        /// <summary>
        /// Consulta la compra premium que existe en el sistema
        /// </summary>
        /// <param name="criteria">CompraPremium que provee el criterio de búsqueda</param>
        /// <param name="tracking">Permite saber si se rastrearán los objetos que se obtengan de la consulta</param>
        /// <returns>CompraPremium que satisfacen el criterio de búsqueda</returns>
        public List<CompraPremium> Retrieve(CompraPremium criteria, bool tracking)
        {
            DbQuery<CompraPremium> qryTutores = (tracking) ? _model.CompraPremium : _model.CompraPremium.AsNoTracking();

            return qryTutores.Where(new CompraPremiumQry(criteria).Action()).ToList();
        }

        /// <summary>
        /// Agrega un Tutor al sistema
        /// </summary>
        /// <param name="tutor"> Tutor que se registrará</param>
        /// <returns>Resultado del registro</returns>
        public Boolean Insert(CompraPremium compraPremium)
        {
            var resultado = false;

            #region ** validaciones **
            if (compraPremium == null) throw new ArgumentNullException("CompraPremium", "CompraPremium no puede ser nulo");
            if (compraPremium.CostoCompra == null) throw new ArgumentException("compraPremium.CostoCompra", "CostoCompra de CompraPremium no puede ser nulo o vacio");
            if (compraPremium.FechaCompra == null) throw new ArgumentException("compraPremium.FechaCompra", "FechaCompra de CompraPremium no puede ser nulo o vacio");
            if (string.IsNullOrEmpty(compraPremium.CodigoCompra)) throw new ArgumentException("compraPremium.CodigoCompra", "CodigoCompra de CompraPremium no puede ser nulo o vacio");
            if (compraPremium.CodigoPaquete == null) throw new ArgumentException("compraPremium.CodigoPaquete", "CodigoPaquete de CompraPremium no puede ser nulo o vacio");
            #endregion

            try
            {
                _model.CompraPremium.Add(compraPremium);
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
        /// Actualiza un Tutor
        /// </summary>
        /// <param name="tutor">Tutor a actualizar</param>
        /// <returns></returns>
        public Boolean Update(CompraPremium compraPremium)
        {
            var resultado = false;

            #region ** validaciones **
            if (compraPremium == null) throw new ArgumentNullException("CompraPremium", "Tutor no puede ser nulo");
            if (compraPremium.CompraPremiumID == null) throw new ArgumentNullException("compraPremium.CompraPremiumID", "Identificador de tutor no puede ser nulo");
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
