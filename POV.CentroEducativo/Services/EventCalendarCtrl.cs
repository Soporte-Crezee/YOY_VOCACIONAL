using POV.CentroEducativo.Queries;
using POV.Modelo.Context;
using POV.Seguridad.BO;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POV.CentroEducativo.Services
{
    public class EventCalendarCtrl : IDisposable
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
        /// <param name="contexto"> Contexto de conexión a la base de datos </param>
        public EventCalendarCtrl(Contexto contexto) 
        {
            _sing = new object();
            _model = contexto ?? new Contexto(_sing);
        }

        /// <summary>
        /// Agrega un EventCalendar al sistema
        /// </summary>
        /// <param name="eventCalendar"> EventCalendar que se registrará </param>
        /// <returns> Resultado del registro </returns>
        public Boolean Insert(EventCalendar eventCalendar) 
        {
            var resultado = false;

            #region validaciones
            if (eventCalendar == null) throw new ArgumentNullException("EventCalendar", "EventCalendar no puede ser nulo");
            if (eventCalendar.UsuarioID == null) throw new ArgumentNullException("EventCalendar.UsarioID", "UsuarioID no puede ser nulo");
            if (string.IsNullOrEmpty(eventCalendar.Asunto)) throw new ArgumentException("EventCalendar.Asunto", "Asunto no puede ser nulo");
            if (string.IsNullOrEmpty(eventCalendar.Fecha)) throw new ArgumentException("EventCalendar.Fecha", "Fecha no puede ser nulo");
            if (string.IsNullOrEmpty(eventCalendar.HrsInicio)) throw new ArgumentException("EventCalendar.HrsInicio", "HrsInicio no puede ser nulo");
            if (string.IsNullOrEmpty(eventCalendar.HrsFin)) throw new ArgumentException("EventCalendar.HrsFin", "HrsFin no puede ser nulo");
            #endregion

            try
            {
                _model.EventCalendar.Add(eventCalendar);
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
        /// Consulta los EventCalendar que existen en el sistema
        /// </summary>
        /// <param name="criteria"> EventCalendar que provee el criterio de búsqueda </param>
        /// <param name="tracking"> Permite saber si se rastrearán los objetos que se obtengan de la consulta </param>
        /// <returns> EventCalendar que satisfacen el criterio de búsqueda </returns>
        public List<EventCalendar> Retrieve(EventCalendar criteria, bool tracking) 
        {
            DbQuery<EventCalendar> qryEventCalendar = (tracking) ? _model.EventCalendar : _model.EventCalendar.AsNoTracking();
            return qryEventCalendar.Where(new EventCalendarQry(criteria).Action()).ToList();
        }

        /// <summary>
        /// Actualiza un EventCalendar
        /// </summary>
        /// <param name="eventCalendar"> EventCalendar a actualizar </param>
        /// <returns></returns>
        public Boolean Update(EventCalendar eventCalendar) 
        {
            var resultado = false;

            #region validaciones
            if (eventCalendar == null) throw new ArgumentNullException("EventCalendar", "EventCalendar no puede ser nulo");
            if (eventCalendar.UsuarioID == null) throw new ArgumentNullException("EventCalendar.UsarioID", "UsuarioID no puede ser nulo");
            if (string.IsNullOrEmpty(eventCalendar.Asunto)) throw new ArgumentException("EventCalendar.Asunto", "Asunto no puede ser nulo");
            if (string.IsNullOrEmpty(eventCalendar.Fecha)) throw new ArgumentException("EventCalendar.Fecha", "Fecha no puede ser nulo");
            if (string.IsNullOrEmpty(eventCalendar.HrsInicio)) throw new ArgumentException("EventCalendar.HrsInicio", "HrsInicio no puede ser nulo");
            if (string.IsNullOrEmpty(eventCalendar.HrsFin)) throw new ArgumentException("EventCalendar.HrsFin", "HrsFin no puede ser nulo");
            #endregion

            var afectados = _model.Commit(_sing);
            if (afectados != 0) resultado = true;

            return resultado;
        }

        /// <summary>
        /// Elimina un EventCalendar de la base de datos
        /// </summary>
        /// <param name="productoCosteo"></param>
        /// <returns></returns>
        public Boolean Delete(EventCalendar eventCalendar)
        {
            var resultado = false;

            try
            {
                _model.EventCalendar.Remove(eventCalendar);
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
