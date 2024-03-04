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
    public class PaquetePremiumCtrl : IDisposable
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
        public PaquetePremiumCtrl(Contexto contexto)
        {
            _sing = new object();
            _model = contexto ?? new Contexto(_sing);
        }

        /// <summary>
        /// Consulta la compra premium que existe en el sistema
        /// </summary>
        /// <param name="criteria">PaquetePremuim que provee el criterio de búsqueda</param>
        /// <param name="tracking">Permite saber si se rastrearán los objetos que se obtengan de la consulta</param>
        /// <returns>PaquetePremuim que satisfacen el criterio de búsqueda</returns>
        public List<PaquetePremium> Retrieve(PaquetePremium criteria, bool tracking)
        {
            DbQuery<PaquetePremium> qryTutores = (tracking) ? _model.PaquetePremium : _model.PaquetePremium.AsNoTracking();

            return qryTutores.Where(new PaquetePremiumQry(criteria).Action()).ToList();
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
