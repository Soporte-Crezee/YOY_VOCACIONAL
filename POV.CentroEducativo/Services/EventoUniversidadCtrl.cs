using POV.CentroEducativo.BO;
using POV.CentroEducativo.Queries;
using POV.Modelo.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POV.CentroEducativo.Services
{
    public class EventoUniversidadCtrl : IDisposable
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
        public EventoUniversidadCtrl(Contexto contexto)
        {
            _sing = new object();
            _model = contexto ?? new Contexto(_sing);
        }

        /// <summary>
        /// Consulta la EventoUniversidades que existen en el sistema
        /// </summary>
        /// <param name="criteria">EventoUniversidad que provee el criterio de búsqueda</param>
        /// <param name="tracking">Permite saber si se rastrearán los objetos que se obtengan de la consulta</param>
        /// <returns>EventoUniversidades que satisfacen el criterio de búsqueda</returns>
        public List<EventoUniversidad> Retrieve(EventoUniversidad criteria, bool tracking)
        {
            DbQuery<EventoUniversidad> qryEventoUniversidades = (tracking) ? _model.EventoUniversidad : _model.EventoUniversidad.AsNoTracking();

            return qryEventoUniversidades.Where(new EventoUniversidadQry(criteria).Action()).ToList();
        }
        
        /// <summary>
        /// Agrega una EventoUniversidad al sistema
        /// </summary>
        /// <param name="EventoUniversidad"> EventoUniversidad que se registrará</param>
        /// <returns>Resultado del registro</returns>
        public Boolean Insert(EventoUniversidad EventoUniversidad)
        {
            var resultado = false;

            #region ** validaciones **
            if (EventoUniversidad == null) throw new ArgumentNullException("EventoUniversidad", "EventoUniversidad no puede ser nulo");
            if (string.IsNullOrEmpty(EventoUniversidad.Nombre)) throw new ArgumentException("EventoUniversidad.NombreEventoUniversidad", "Nombre de EventoUniversidad no puede ser nulo o vacio");
            #endregion

            try
            {
                _model.EventoUniversidad.Add(EventoUniversidad);
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
        /// Actualiza una EventoUniversidad
        /// </summary>
        /// <param name="EventoUniversidad">EventoUniversidad a actualizar</param>
        /// <param name="tareasEliminadas">Lista de tareas a eliminar</param>
        /// <returns></returns>
        public Boolean Update(EventoUniversidad EventoUniversidad)
        {
            var resultado = false;

            #region ** validaciones **
            if (EventoUniversidad == null) throw new ArgumentNullException("EventoUniversidad", "EventoUniversidad no puede ser nulo");
            if (EventoUniversidad.EventoUniversidadId == null) throw new ArgumentNullException("EventoUniversidad.EventoUniversidadID", "Identificador de EventoUniversidad no puede ser nulo");
            if (string.IsNullOrEmpty(EventoUniversidad.Nombre)) throw new ArgumentException("EventoUniversidad.Nombre", "Nombre de EventoUniversidad no puede ser nulo o vacio");

            #endregion

            var afectados = _model.Commit(_sing);
            if (afectados != 0) resultado = true;

            return resultado;
        }

        /// <summary>
        /// Elimina un EventoUniversidad de la base de datos
        /// </summary>
        /// <param name="tarea"></param>
        /// <returns></returns>
        public Boolean Delete(EventoUniversidad evento)
        {
            var resultado = false;

            try
            {
                _model.EventoUniversidad.Remove(evento);
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
