using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POV.ConfiguracionesPlataforma.BO;
using POV.ConfiguracionesPlataforma.Queries;
using POV.Modelo.Context;

namespace POV.ConfiguracionesPlataforma.Services
{
    public class ConfiguracionGeneralCtrl
    {
        /// <summary>
        /// Contexto interno de conexión a la base de datos
        /// </summary>
        private readonly Contexto _model;

        /// <summary>
        /// Firma de la conexion a la base de datos
        /// </summary>
        private readonly object _sign;

        /// <summary>
        /// Constructor encargado de crear la conexion interna a la base de datos
        /// </summary>
        /// <param name="contexto">Contexto de conexion a la base de datos</param>
        public ConfiguracionGeneralCtrl(Contexto contexto)
        {
            _sign = new object();
            _model = contexto ?? new Contexto(_sign);
        }

        /// <summary>
        /// Método para cerrar la conexión del controlador con la base de datos
        /// </summary>
        public void Dispose()
        {
            _model.Disposing(_sign);
        }

        /// <summary>
        /// Consulta Configuracion general existentes en el sistema
        /// </summary>
        /// <param name="criteria">Configuracion General que provee el criterio de búsqueda</param>
        /// <param name="isTracking">Permite saber si se rastrearan los objetos que se obtengan de la búsqueda</param>
        /// <returns>Configuracion General que satisfacen el criterio de búsqueda</returns>
        public List<ConfiguracionGeneral> Retrieve(ConfiguracionGeneral criteria, bool isTracking)
        {
            DbQuery<ConfiguracionGeneral> qryConfiguracionGeneral = (isTracking) ? _model.ConfiguracionesGenerales : _model.ConfiguracionesGenerales.AsNoTracking();
            return qryConfiguracionGeneral.Where(new ConfiguracionGeneralQry(criteria).Action()).ToList();
        }

        public Boolean Insert(ConfiguracionGeneral configuracion)
        {
            var resultado = false;
            try
            {
                if (string.IsNullOrEmpty(configuracion.RutaServidorContenido)) throw new ArgumentException("configuracion.RutaServidorContenido", "Ruta de servidor de contenido");
                if (string.IsNullOrEmpty(configuracion.RutaPlantillas)) throw new ArgumentException("configuracion.RutaPlantillas", "Ruta de plantillas");
                if (configuracion.MaximoTopAlumnos == null) throw new ArgumentNullException("configuracion.MaximoTopAlumnos", "Maximo de Top de Alumnos");

                _model.ConfiguracionesGenerales.Add(configuracion);
                var afectados = _model.Commit(_sign);
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return resultado;
        }

        public Boolean Update(ConfiguracionGeneral configuracion)
        {
            var resultado = false;
            try
            {
                if (configuracion.ConfiguracionGeneralId == null) throw new ArgumentNullException("configuracion.ConfiguracionGeneralId", "Identificador de configuracion general");
                if (string.IsNullOrEmpty(configuracion.RutaServidorContenido)) throw new ArgumentException("configuracion.RutaServidorContenido", "Ruta de servidor de contenido");
                if (string.IsNullOrEmpty(configuracion.RutaPlantillas)) throw new ArgumentException("configuracion.RutaPlantillas", "Ruta de plantillas");
                if (configuracion.MaximoTopAlumnos == null) throw new ArgumentNullException("configuracion.MaximoTopAlumnos", "Maximo de Top de Alumnos");
                //si hay tareas por eliminar, se eliminan del sistema
                var afectados = _model.Commit(_sign);
                if (afectados != 0) resultado = true;
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return resultado;
        }
    }
}
