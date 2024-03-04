using POV.CentroEducativo.BO;
using POV.CentroEducativo.Queries;
using POV.Modelo.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using POV.Seguridad.BO;

namespace POV.CentroEducativo.Services
{
    public class ConfigCalendarCtrl : IDisposable
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
        public ConfigCalendarCtrl(Contexto contexto)
        {
            _sing = new object();
            _model = contexto ?? new Contexto(_sing);
        }

        /// <summary>
        /// Consulta la información completa de las CongifCalendar del sistema
        /// </summary>
        /// <param name="criteria"> CongifCalendar que provee el criterio de búsqueda </param>
        /// <param name="tracking"> Permite saber si se rastrearán los objetos que se obtengan de la consulta </param>
        /// <returns> CongifCalendar que satisfacen el criterio de búsqueda </returns>
        public List<ConfigCalendar> RetrieveWithRelationship(ConfigCalendar criteria, bool tracking) 
        {
            DbQuery<ConfigCalendar> qryConfigCalendar = (tracking) ? _model.ConfigCalendar : _model.ConfigCalendar.AsNoTracking();

            return qryConfigCalendar.Where(new ConfigCalendarQry(criteria).Action())
                .Include(x => x.EventCalendar)
                .ToList();
        }

        /// <summary>
        /// Agrega un ConfigCalendar al sistema
        /// </summary>
        /// <param name="ConfigCalendar"> ConfigCalendar que se registrará</param>
        /// <returns>Resultado del registro</returns>
        public Boolean Insert(ConfigCalendar configCalendar)
        {
            var resultado = false;

            #region ** validaciones **
            if (configCalendar == null) throw new ArgumentNullException("ConfigCalendar", "ConfigCalendar no puede ser nulo");
            if (configCalendar.UsuarioID == null) throw new ArgumentNullException("configCalendar.UsuarioID", "Identificador de usuario no puede ser nulo");
            if (string.IsNullOrEmpty(configCalendar.DiasLaborales)) throw new ArgumentException("configCalendar.DiasLaborales", "DiasLaborales de configuración de calendario no puede ser nulo o vacio");
            if (string.IsNullOrEmpty(configCalendar.InicioTrabajo)) throw new ArgumentException("configCalendar.InicioTrabajo", "InicioTrabajo de configuración de calendario no puede ser nulo o vacio");
            if (string.IsNullOrEmpty(configCalendar.FinTrabajo)) throw new ArgumentException("configCalendar.FinTrabajo", "FinTrabajo de configuración de calendario no puede ser nulo o vacio");
            #endregion

            try
            {
                _model.ConfigCalendar.Add(configCalendar);
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
        /// Consulta la Alumnoes que existen en el sistema
        /// </summary>
        /// <param name="criteria">ConfigCalendar que provee el criterio de búsqueda</param>
        /// <param name="tracking">Permite saber si se rastrearán los objetos que se obtengan de la consulta</param>
        /// <returns>ConfigCalendar que satisfacen el criterio de búsqueda</returns>
        public List<ConfigCalendar> Retrieve(ConfigCalendar criteria, bool tracking)
        {
            DbQuery<ConfigCalendar> qryConfigCalendar = (tracking) ? _model.ConfigCalendar : _model.ConfigCalendar.AsNoTracking();

            return qryConfigCalendar.Where(new ConfigCalendarQry(criteria).Action()).ToList();
        }

        /// <summary>
        /// Actualiza un ConfigCalendar
        /// </summary>
        /// <param name="configCalendar">ConfigCalendar a actualizar</param>
        /// <returns></returns>
        public Boolean Update(ConfigCalendar configCalendar)
        {
            var resultado = false;

            #region ** validaciones **
            if (configCalendar == null) throw new ArgumentNullException("ConfigCalendar", "configCalendar no puede ser nulo");
            if (configCalendar.ConfigCalendarID == null) throw new ArgumentNullException("configCalendar.ConfigCalendarID", "Identificador de configuración de calendario no puede ser nulo");
            if (configCalendar.UsuarioID == null) throw new ArgumentNullException("configCalendar.UsuarioID", "Identificador de usuario no puede ser nulo");
            if (string.IsNullOrEmpty(configCalendar.DiasLaborales)) throw new ArgumentException("configCalendar.DiasLaborales", "DiasLaborales de configuración de calendario no puede ser nulo o vacio");
            if (string.IsNullOrEmpty(configCalendar.InicioTrabajo)) throw new ArgumentException("configCalendar.InicioTrabajo", "InicioTrabajo de configuración de calendario no puede ser nulo o vacio");
            if (string.IsNullOrEmpty(configCalendar.FinTrabajo)) throw new ArgumentException("configCalendar.FinTrabajo", "FinTrabajo de configuración de calendario no puede ser nulo o vacio");
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
