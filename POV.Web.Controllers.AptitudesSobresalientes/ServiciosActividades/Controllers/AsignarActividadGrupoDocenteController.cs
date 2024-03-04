using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POV.ConfiguracionActividades.BO;
using POV.ConfiguracionActividades.Services;
using POV.Modelo.Context;
using POV.AmazonAWS.Services;
using POV.Logger.Service;
using POV.Comun.BO;
using POV.Comun.Service;

namespace POV.ServiciosActividades.Controllers
{
    /// <summary>
    /// Controlador de la interfaz de usuario Crear Actividad
    /// </summary>
    public class AsignarActividadGrupoController
    {
        #region Atributos

        private readonly Contexto ctx;
        private object firma = new object();
        #endregion

        #region Constructores
		/// <summary>
		/// Constructor por defecto
		/// </summary>
        public AsignarActividadGrupoController()
		{
			ctx = new Contexto(firma);
			
		}
        #endregion

        /// <summary>
        /// Consulta un actividad
        /// </summary>
        /// <param name="actividadDocente">Actividad filtro</param>
        /// <returns>Actividad encontrada, en caso contrario devuelve una actividad vacia</returns>
        public ActividadDocente RetrieveActividad(ActividadDocente actividadDocente)
        {
            var ctrl = new ActividadDocenteCtrl(ctx);
            List<ActividadDocente> list = ctrl.Retrieve(actividadDocente, true);
            ActividadDocente result = list.FirstOrDefault();
            if (result == null)
                return new ActividadDocente();

            return result;
        }

        public AsignacionActividadGrupo InsertAsignacionActividadGrupo(AsignacionActividadGrupo asignacionActividadGrupo)
        {
            using (var ctrl = new AsignacionActividadGrupoCtrl(ctx))
            {
                ctrl.Insert(asignacionActividadGrupo);
                ctx.Commit(firma);
            }
            return asignacionActividadGrupo;
        }

        /// <summary>
        /// Método que consulta la asignación de la actividad
        /// </summary>
        /// <param name="asignacionActividad">Asignación a consultar</param>
        /// <param name="tracking">Valor para saber si se requiere mantener el rastreo</param>
        /// <returns>Lista de Asignaciones que cumplen con los criterios de búsqueda</returns>
        public List<AsignacionActividad> ConsultarAsignacionActividades(AsignacionActividad asignacionActividad,
            Boolean tracking)
        {
            var ctrl = new AsignacionActividadCtrl(ctx);
            var lstAsignaciones = ctrl.RetrieveWithRelationship(asignacionActividad, tracking);
            ctrl.Dispose();

            return lstAsignaciones;
        }
    }
}
