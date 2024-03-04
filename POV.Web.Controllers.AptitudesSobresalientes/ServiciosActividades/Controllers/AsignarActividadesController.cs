//Asignar Actividades a Alumnos
using System;
using System.Collections.Generic;
using POV.ConfiguracionActividades.BO;
using POV.ConfiguracionActividades.Services;
using POV.Modelo.Context;

namespace POV.ServiciosActividades.Controllers
{
    public class AsignarActividadesController
    {
        private readonly Contexto _model;
        private readonly object _sing;

        /// <summary>
        /// Constructor del Controlador
        /// </summary>
        /// <param name="contexto">Contexto a inicializar</param>
        public AsignarActividadesController(Contexto contexto)
        {
            _sing = new object();
            _model = contexto ?? new Contexto(_sing);
        }
        
        /// <summary>
        /// Método que consulta las actividades que existen en el sistema y estan activas
        /// </summary>
        /// <param name="criteriaActividad"></param>
        /// <returns></returns>
        public List<Actividad> ConsultarActividades(Actividad criteriaActividad)
        {
            criteriaActividad.Activo = true;
            var ctrl = new ActividadCtrl(_model);
            var lstActividades = ctrl.Retrieve(criteriaActividad, false);
            ctrl.Dispose();
            return lstActividades;
        }

        /// <summary>
        /// Método que consulta las tareas que tiene una actividad
        /// </summary>
        /// <param name="criteriaActividad">Actividad que se requiere conocer sus actividades</param>
        /// <returns>Actividad con las tareas que contiene</returns>
        public List<Actividad> ConsultarTareasActividad(Actividad criteriaActividad)
        {
            var ctrl = new ActividadCtrl(_model);
            var lstActividades = ctrl.RetrieveWithRelationship(criteriaActividad, false);
            ctrl.Dispose();
            return lstActividades;
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
            var ctrl = new AsignacionActividadCtrl(_model);
            var lstAsignaciones = ctrl.RetrieveWithRelationship(asignacionActividad, tracking);
            ctrl.Dispose();

            return lstAsignaciones;
        } 

        /// <summary>
        /// Método que registra la asignación de la actividad a un alumno
        /// </summary>
        /// <param name="asignacion">Asignación a registrar</param>
        /// <returns>Resultado del registro</returns>
        public AsignacionActividad InsertAsignacionActividad(AsignacionActividad asignacion)
        {
            var ctrl = new AsignacionActividadCtrl(_model);
            ctrl.Insert(asignacion);

            _model.Commit(_sing);

            return asignacion;
        }

        /// <summary>
        /// Metodo para cargar las actividades del contexto
        /// </summary>
        /// <param name="asignacion">asigacion que tiene el identificador de la actividad</param>
        public void LoadActividadAsignacion(AsignacionActividad asignacion)
        {
            _model.Entry(asignacion).Reference(x => x.Actividad).Load();
            _model.Entry(asignacion).Reference(y => y.Alumno).Load();
        }

        /// <summary>
        /// Método que elimina la asignación de acticvidad a un alumno
        /// </summary>
        /// <param name="asignacion">Asignación a eliminar</param>
        public void DeleteAsignacionActividad(AsignacionActividad asignacion)
        {
            var ctrl = new AsignacionActividadCtrl(_model);
            ctrl.Delete(asignacion);
            _model.Commit(_sing);
            ctrl.Dispose();

        }
    }
}
