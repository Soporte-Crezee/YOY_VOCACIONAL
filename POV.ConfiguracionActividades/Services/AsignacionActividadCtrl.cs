using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using POV.ConfiguracionActividades.BO;
using POV.ConfiguracionActividades.Queries;
using POV.Modelo.Context;
using POV.CentroEducativo.BO;

namespace POV.ConfiguracionActividades.Services
{
	public class AsignacionActividadCtrl : IDisposable
	{
        /// <summary>
        /// Contexto interno de conexión a la base de datos
        /// </summary>
		private readonly Contexto _model;

        /// <summary>
        /// Firma de la conexión a la base de datos
        /// </summary>
		private readonly object _sign;

        /// <summary>
        /// Constructor encargado de crear la conexion interna a la base de datos
        /// </summary>
        /// <param name="contexto">Contexto de conexion a la base de datos</param>
		public AsignacionActividadCtrl(Contexto contexto)
		{
			_sign = new object();
			_model = contexto ?? new Contexto(_sign);
		}

        /// <summary>
        ///  Consulta las asignaciones existentes en el sistema
        /// </summary>
        /// <param name="criteria">Asignación de Actividad que provee el criterio de búsqueda</param>
        /// <param name="tracking">Permite saber si se rastrearán los objetos que se obtengan de la consulta</param>
        /// <returns>Asignaciones de Actividades que satisfacen el criterio de búsqueda</returns>
		public List<AsignacionActividad> Retrieve(AsignacionActividad criteria, bool tracking)
		{
			DbQuery<AsignacionActividad> qryAsignacionActividad = (tracking) ? _model.AsignacionesActividades : _model.AsignacionesActividades.AsNoTracking();
            return qryAsignacionActividad.Where(new AsignacionActividadQry(criteria).Action())
                .Include(x => x.TareasRealizadas)
                .ToList();

		}

        /// <summary>
        /// Consulta las asignaciones del alumno
        /// </summary>
        /// <param name="alumno">Alumno</param>
        /// <param name="escuela">Escuela</param>
        /// <param name="cicloEscolar">ciclo escolar</param>
        /// <param name="tracking"></param>
        /// <returns>Listado de asignaciones del alumno para la escuela y el ciclo escolar</returns>
        public List<AsignacionActividad> RetrieveAsignacionesAlumno(Alumno alumno, Escuela escuela, CicloEscolar cicloEscolar, bool tracking)
        {
            List<AsignacionActividad> result = new List<AsignacionActividad>();
            DbQuery<AsignacionActividad> qryAsignacionActividad = (tracking) ? _model.AsignacionesActividades : _model.AsignacionesActividades.AsNoTracking();

            result = qryAsignacionActividad.Where(item => item.AlumnoId == alumno.AlumnoID &&
                item.Actividad.BloqueActividad.EscuelaId == escuela.EscuelaID &&
                item.Actividad.BloqueActividad.CicloEscolarId == cicloEscolar.CicloEscolarID)
                .Include(x => x.Actividad)
                .Include(x=> x.Alumno)
				.Include(x=> x.TareasRealizadas)
				.OrderBy(item => item.Actividad.BloqueActividad.FechaInicio).ToList();

            return result;
        }


        /// <summary>
        /// Consulta la información completa de las asignaciones de actividades del sistema
        /// </summary>
        /// <param name="criteria">Asignación de Actividad que provee el criterio de búsqueda</param>
        /// <param name="tracking">Permite saber si se rastrearán los objetos que se obtengan de la consulta</param>
        /// <returns>Asignaciones de Actividades que satisfacen el criterio de búsqueda</returns>
		public List<AsignacionActividad> RetrieveWithRelationship(AsignacionActividad criteria, bool tracking)
		{
			DbQuery<AsignacionActividad> qryAsignacionActividad = (tracking) ? _model.AsignacionesActividades : _model.AsignacionesActividades.AsNoTracking();
			return qryAsignacionActividad.Where(new AsignacionActividadQry(criteria).Action())
				.Include(x=>x.Actividad)
				.Include(x=>x.Alumno)
				.Include(x=>x.TareasRealizadas)
				.ToList();
		}

        /// <summary>
        /// Agrega la asignacion de la actividad al alumno
        /// </summary>
        /// <param name="asignacionActividad">Asignación de Actividad que se registrará</param>
        /// <returns>Resultado del registro de la asiganción</returns>
	    public Boolean Insert(AsignacionActividad asignacionActividad)
	    {
	        var resultado = false;

	        try
	        {
	            _model.AsignacionesActividades.Add(asignacionActividad);
	            var afectados = _model.Commit(_sign);

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
	        _model.Disposing(_sign);
	    }
        
        /// <summary>
        /// Método para Actualizar la asignacion actividad
        /// </summary>
		public void Update(AsignacionActividad asigActividad)
		{
			_model.Commit(_sign);
		}

        /// <summary>
        /// Método para eliminar las asignaciones de las actividades
        /// </summary>
        /// <param name="asignacionActividad">Asignación a eliminar</param>
        /// <returns>Resultado de la eliminación</returns>
	    public Boolean Delete(AsignacionActividad asignacionActividad)
	    {
            var resultado = false;

	        try
	        {
                List<TareaRealizada> tareas = new List<TareaRealizada>();
                _model.TareasRealizadas.RemoveRange(asignacionActividad.TareasRealizadas);
                _model.AsignacionesActividades.Remove(asignacionActividad);


                var afectados = _model.Commit(_sign);
                if (afectados != 0) resultado = true;
	        }
            catch (Exception exception)
            {
                throw exception;
            }

            return resultado;
	    }

        public List<AsignacionActividad> Retrieve(Alumno alumno, ActividadDocente actividad,AsignacionActividad criteria, bool tracking)
        {
            DbQuery<AsignacionActividad> qryAsignacionActividad = (tracking) ? _model.AsignacionesActividades : _model.AsignacionesActividades.AsNoTracking();
            List<AsignacionActividad> result = qryAsignacionActividad.Where(new AsignacionActividadQry(criteria).Action(alumno, actividad))
                .Include(a => a.Alumno)
                .Include(a => a.Actividad)
                .Include(a => a.Actividad.Docente)
                .Include(a => a.TareasRealizadas)
                .Include(a => a.Alumno.Tutores).ToList();
                //.Include(a => a.Alumno.Docente)
            return result;

        }
	}
}
