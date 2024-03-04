using POV.CentroEducativo.BO;
using POV.CentroEducativo.Queries;
using POV.Modelo.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POV.CentroEducativo.Services
{
    public class EncuestaSatisfaccionCtrl : IDisposable
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
        public EncuestaSatisfaccionCtrl(Contexto contexto)
        {
            _sing = new object();
            _model = contexto ?? new Contexto(_sing);
        }

        /// <summary>
        /// Consulta la EncuestaSatisfaccion que existen en el sistema
        /// </summary>
        /// <param name="criteria">EncuestaSatisfaccion que provee el criterio de búsqueda</param>
        /// <param name="tracking">Permite saber si se rastrearán los objetos que se obtengan de la consulta</param>
        /// <returns>EncuestaSatisfaccion que satisfacen el criterio de búsqueda</returns>
        public List<EncuestaSatisfaccion> Retrieve(EncuestaSatisfaccion criteria, bool tracking)
        {
            DbQuery<EncuestaSatisfaccion> qryEncuestaSatisfaccion = (tracking) ? _model.EncuestaSatisfaccion : _model.EncuestaSatisfaccion.AsNoTracking();

            return qryEncuestaSatisfaccion.Where(new EncuestaSatisfaccionQry(criteria).Action()).ToList();
        }

        /// <summary>
        /// Agrega una EncuestaSatisfaccion al sistema
        /// </summary>
        /// <param name="Carrera"> EncuestaSatisfaccion que se registrará</param>
        /// <returns>Resultado del registro</returns>
        public Boolean Insert(EncuestaSatisfaccion encuestaSatisfaccion)
        {
            var resultado = false;

            #region ** validaciones **
            //if (Carrera == null) throw new ArgumentNullException("Carrera", "Carrera no puede ser nulo");
            ////if (Carrera.CarreraID == null) throw new ArgumentNullException("Carrera.CarreraID", "Identificador de carrera no puede ser nulo");
            //if (string.IsNullOrEmpty(Carrera.NombreCarrera)) throw new ArgumentException("Carrera.NombreCarrera", "Nombre de Carrera no puede ser nulo o vacio");
            //if (string.IsNullOrEmpty(Carrera.Descripcion)) throw new ArgumentException("Carrera.Descripcion", "Descripción de Carrera no puede ser nulo o vacio");
            //if (Carrera.Activo == null) throw new ArgumentNullException("Carrera.Activo", "Activo no puede ser nulo");
            //if (Carrera.ClasificadorID == null) throw new ArgumentNullException("Carrera.ClasificadorID", "ClasificadorID no puede ser nulo");
            #endregion

            try
            {
                _model.EncuestaSatisfaccion.Add(encuestaSatisfaccion);
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
