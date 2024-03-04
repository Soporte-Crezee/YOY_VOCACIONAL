using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using POV.ConfiguracionActividades.BO;
using POV.ConfiguracionActividades.Queries;
using POV.Modelo.Context;

namespace POV.ConfiguracionActividades.Services
{
	public class AsignacionActividadGrupoCtrl : IDisposable
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
		public AsignacionActividadGrupoCtrl(Contexto contexto)
		{
			_sign = new object();
			_model = contexto ?? new Contexto(_sign);
		}

        /// <summary>
        ///  Consulta las asignaciones existentes en el sistema
        /// </summary>
        /// <param name="criteria">Asignación de Actividad de Grupo que provee el criterio de búsqueda</param>
        /// <param name="tracking">Permite saber si se rastrearán los objetos que se obtengan de la consulta</param>
        /// <returns>Asignaciones de Actividades de Grupos que satisfacen el criterio de búsqueda</returns>
		public List<AsignacionActividadGrupo> Retrieve(AsignacionActividadGrupo criteria, bool tracking)
		{
			DbQuery<AsignacionActividadGrupo> qryAsignacionActividadGrupo = (tracking) ? _model.AsignacionesActividadesGrupos : _model.AsignacionesActividadesGrupos.AsNoTracking();
            return qryAsignacionActividadGrupo.Where(new AsignacionActividadGrupoQry(criteria).Action()).ToList();

		}

        /// <summary>
        /// Consulta la información completa de las asignaciones de actividades del sistema
        /// </summary>
        /// <param name="criteria">Asignación de Actividad que provee el criterio de búsqueda</param>
        /// <param name="tracking">Permite saber si se rastrearán los objetos que se obtengan de la consulta</param>
        /// <returns>Asignaciones de Actividades de Grupos que satisfacen el criterio de búsqueda</returns>
		public List<AsignacionActividadGrupo> RetrieveWithRelationship(AsignacionActividadGrupo criteria, bool tracking)
		{
            DbQuery<AsignacionActividadGrupo> qryAsignacionActividadGrupo = (tracking) ? _model.AsignacionesActividadesGrupos : _model.AsignacionesActividadesGrupos.AsNoTracking();
            return qryAsignacionActividadGrupo.Where(new AsignacionActividadGrupoQry(criteria).Action())
				.Include(x=>x.ActividadDocente)
				.Include(x=>x.GrupoCicloEscolar)
				.Include(x=>x.AsignacionesActividades)
				.ToList();
		}

        /// <summary>
        /// Agrega la asignacion de la actividad de grupo
        /// </summary>
        /// <param name="asignacionActividadGrupo">Asignación de Actividad de Grupo que se registrará</param>
        /// <returns>Resultado del registro de la asiganción</returns>
        public Boolean Insert(AsignacionActividadGrupo asignacionActividadGrupo)
	    {
	        var resultado = false;

	        try
	        {
                _model.AsignacionesActividadesGrupos.Add(asignacionActividadGrupo);
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
        /// Método para Actualizar la asignacion actividad de grupo
        /// </summary>
		public void Update(AsignacionActividadGrupo asigActividadGrupo)
		{
			_model.Commit(_sign);
		}

        /// <summary>
        /// Método para eliminar la asignacion actividad de grupo
        /// </summary>
        /// <param name="asignacionActividadGrupo">Asignación a eliminar</param>
        /// <returns>Resultado de la eliminación</returns>
	    public Boolean Delete(AsignacionActividadGrupo asignacionActividadGrupo)
	    {
            var resultado = false;

	        try
	        {
                _model.AsignacionesActividadesGrupos.Remove(asignacionActividadGrupo);
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
        /// Método para eliminar la asignacion actividad de grupo con sus relaciones Asignaciones de Actividades
        /// </summary>
        /// <param name="asignacionActividadGrupo">Asignación a eliminar</param>
        /// <returns>Resultado de la eliminación</returns>
        public Boolean DeleteComplete(AsignacionActividadGrupo asignacionActividadGrupo)
        {
            var resultado = false;
            var controladorAsignaciones = new AsignacionActividadCtrl(_model);
            
            try
            {
                List<AsignacionActividad> asignaciones = new List<AsignacionActividad>();
                asignacionActividadGrupo = Retrieve(asignacionActividadGrupo, true).FirstOrDefault();
                _model.Entry(asignacionActividadGrupo).Collection(a => a.AsignacionesActividades).Load();

                if (asignacionActividadGrupo != null)
                {
                    foreach (var asignacion in asignacionActividadGrupo.AsignacionesActividades)
                    {
                        _model.Entry(asignacion).Collection(a => a.TareasRealizadas).Load();
                        asignaciones.Add(asignacion);
                    }
                    resultado = this.Delete(asignacionActividadGrupo);
                }
                foreach (var asignacionActividad in asignaciones)
                {
                    controladorAsignaciones.Delete(asignacionActividad);
                }                 
            }
            catch (Exception exception)
            {
                throw exception;
            }

            return resultado;
        }
	}
}
