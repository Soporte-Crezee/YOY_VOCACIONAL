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
    public class ActividadCtrl : IDisposable
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
        public ActividadCtrl(Contexto contexto)
        {
            _sing = new object();
            _model = contexto ?? new Contexto(_sing);
        }

        /// <summary>
        /// Consulta la información completa de los actividades del sistema
        /// </summary>
        /// <param name="criteria">Actividad que provee el criterio de búsqueda</param>
        /// <param name="tracking">Permite saber si se rastrearán los objetos que se obtengan de la consulta</param>
        /// <returns>Actividades que satisfacen el criterio de búsqueda</returns>
        public List<Actividad> RetrieveWithRelationship(Actividad criteria, bool tracking)
        {
            DbQuery<Actividad> qryActividades = (tracking) ? _model.Actividades : _model.Actividades.AsNoTracking();

            return qryActividades.Where(new ActividadQry(criteria).Action())
                                .Include(x => x.Tareas)
                                .Include(x => x.ClasificadoresResultados)
                                .Include(x => x.Escuela)
                                .ToList();
        }

        /// <summary>
        /// Consulta la actividades que existen en el sistema
        /// </summary>
        /// <param name="criteria">Actividad que provee el criterio de búsqueda</param>
        /// <param name="tracking">Permite saber si se rastrearán los objetos que se obtengan de la consulta</param>
        /// <returns>Actividades que satisfacen el criterio de búsqueda</returns>
        public List<Actividad> Retrieve(Actividad criteria, bool tracking)
        {
            DbQuery<Actividad> qryActividades = (tracking) ? _model.Actividades : _model.Actividades.AsNoTracking();

            return qryActividades.Where(new ActividadQry(criteria).Action())
                                .Include(x => x.Tareas)
                                .Include(x => x.ClasificadoresResultados)
                                .ToList();
        }

        /// <summary>
        /// Agrega una actividad al sistema
        /// </summary>
        /// <param name="Actividad"> Actividad que se registrará</param>
        /// <returns>Resultado del registro</returns>
        public Boolean Insert(Actividad actividad)
        {
            var resultado = false;

            #region ** validaciones **
            if (actividad == null) throw new ArgumentNullException("actividad", "Actividad no puede ser nulo");
            if (actividad.BloqueActividadId == null) throw new ArgumentNullException("actividad.BloqueActividadId", "Identificador del bloque no puede ser nulo");
            if (string.IsNullOrEmpty(actividad.Nombre)) throw new ArgumentException("actividad.Nombre", "Nombre de actividad no puede ser nulo o vacio");
            if (string.IsNullOrEmpty(actividad.Descripcion)) throw new ArgumentException("actividad.Descripcion", "Descripción de actividad no puede ser nulo o vacio");
            if (actividad.EscuelaId == null) throw new ArgumentNullException("actividad.EscuelaId", "Identificador de escuela no puede ser nulo");
            if (actividad.FechaCreacion == null) throw new ArgumentNullException("actividad.FechaCreacion", "Fecha de creación no puede ser nulo");
            if (actividad.Activo == null) throw new ArgumentNullException("actividad.Activo", "Activo no puede ser nulo");
            if (actividad.Tareas == null) throw new ArgumentNullException("actividad.Tareas", "Lista de Tareas no puede ser nulo");
            if (!actividad.Tareas.Any()) throw new ArgumentException("actividad.Tareas", "Lista de Tareas no puede estar vacia");
            if (!actividad.Tareas.Any(t => t is TareaPrueba)) throw new ArgumentException("actividad.Tareas", "Lista de Tareas debe contener al menos una tarea tipo Prueba");
            if (actividad.Tareas.Where(t => t is TareaPrueba).Any(t => (t as TareaPrueba).PruebaId == null)) throw new ArgumentException("actividad.Tareas", "Lista de Tareas contiene una TareaPrueba con PruebaID nulo");
            if (actividad.Tareas.Where(t => t is TareaEjeTematico).Any(t => (t as TareaEjeTematico).EjeTematicoId == null)) throw new ArgumentException("actividad.Tareas", "Lista de Tareas contiene una TareaEjeTematico con EjeTematicoId nulo");
            if (actividad.Tareas.Where(t => t is TareaReactivo).Any(t => (t as TareaReactivo).ReactivoId == null)) throw new ArgumentException("actividad.Tareas", "Lista de Tareas contiene una TareaReactivo con ReactivoId nulo");
            if (actividad.Tareas.Where(t => string.IsNullOrEmpty(t.Nombre) || string.IsNullOrEmpty(t.Instruccion)).Any()) throw new ArgumentException("actividad.Tareas", "Lista de Tareas contiene una Tarea con su Nombre o Instrucción nula o vacio");
            #endregion

            try
            {
                _model.Actividades.Add(actividad);
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
        /// Actualiza una actividad
        /// </summary>
        /// <param name="actividad">Actividad a actualizar</param>
        /// <param name="tareasEliminadas">Lista de tareas a eliminar</param>
        /// <returns></returns>
        public Boolean Update(Actividad actividad, List<long> tareasEliminadas = null, List<int> enfoquesEliminados = null)
        {
            var resultado = false;

            #region ** validaciones **
            if (actividad == null) throw new ArgumentNullException("actividad", "Actividad no puede ser nulo");
            if (actividad.BloqueActividadId == null) throw new ArgumentNullException("actividad.BloqueActividadId", "Identificador del bloque no puede ser nulo");
            if (string.IsNullOrEmpty(actividad.Nombre)) throw new ArgumentException("actividad.Nombre", "Nombre de actividad no puede ser nulo o vacio");
            if (string.IsNullOrEmpty(actividad.Descripcion)) throw new ArgumentException("actividad.Descripcion", "Descripción de actividad no puede ser nulo o vacio");
            if (actividad.EscuelaId == null) throw new ArgumentNullException("actividad.EscuelaId", "Identificador de escuela no puede ser nulo");
            if (actividad.FechaCreacion == null) throw new ArgumentNullException("actividad.FechaCreacion", "Fecha de creación no puede ser nulo");
            if (actividad.Activo == null) throw new ArgumentNullException("actividad.Activo", "Activo no puede ser nulo");
            if (actividad.Tareas == null) throw new ArgumentNullException("actividad.Tareas", "Lista de Tareas no puede ser nulo");
            if (!actividad.Tareas.Any()) throw new ArgumentException("actividad.Tareas", "Lista de Tareas no puede estar vacia");
            if (!actividad.Tareas.Any(t => t is TareaPrueba)) throw new ArgumentException("actividad.Tareas", "Lista de Tareas debe contener al menos una tarea tipo Prueba");
            if (actividad.Tareas.Where(t => t is TareaPrueba).Any(t => (t as TareaPrueba).PruebaId == null)) throw new ArgumentException("actividad.Tareas", "Lista de Tareas contiene una TareaPrueba con PruebaID nulo");
            if (actividad.Tareas.Where(t => t is TareaEjeTematico).Any(t => (t as TareaEjeTematico).EjeTematicoId == null)) throw new ArgumentException("actividad.Tareas", "Lista de Tareas contiene una TareaEjeTematico con EjeTematicoId nulo");
            if (actividad.Tareas.Where(t => t is TareaReactivo).Any(t => (t as TareaReactivo).ReactivoId == null)) throw new ArgumentException("actividad.Tareas", "Lista de Tareas contiene una TareaReactivo con ReactivoId nulo");
            if (actividad.Tareas.Where(t => string.IsNullOrEmpty(t.Nombre) || string.IsNullOrEmpty(t.Instruccion)).Any()) throw new ArgumentException("actividad.Tareas", "Lista de Tareas contiene una Tarea con su Nombre o Instrucción nula o vacio");

            #endregion

            
            //si hay tareas por eliminar, se eliminan del sistema
            if (tareasEliminadas != null && tareasEliminadas.Count > 0)
                tareasEliminadas.ForEach(t => DeleteTarea(actividad.Tareas.FirstOrDefault(at => at.TareaId == t)));
            //si hay enfoques por eliminar, se eliminan del sistema
            if (enfoquesEliminados != null && enfoquesEliminados.Count > 0)
                enfoquesEliminados.ForEach(t => DeleteClasificadorResultado(actividad.ClasificadoresResultados.FirstOrDefault(at => at.ClasificadorResultadoId == t)));

            var afectados = _model.Commit(_sing);
            if (afectados != 0) resultado = true;

            return resultado;
        }

        /// <summary>
        /// Elimina una tarea de la base de datos
        /// </summary>
        /// <param name="tarea"></param>
        /// <returns></returns>
        private Boolean DeleteTarea(Tarea tarea)
        {
            var res = true;
            _model.Entry(tarea).State = EntityState.Deleted;
            return res;

        }
        /// <summary>
        /// Elimina un clasificador resultado de los enfoques de una actividad
        /// </summary>
        /// <param name="clasificador">Clasificador perteneciente de la actividad</param>
        /// <returns>true en caso de eliminacion exitosa, false en caso contrario</returns>
        private Boolean DeleteClasificadorResultado(ClasificadorResultado clasificador)
        {
            var res = true;
            _model.Entry(clasificador).State = EntityState.Deleted;
            return res;

        }

        public Boolean Delete(Actividad actividad)
        {
            var resultado = false;

            try
            {
                _model.Actividades.Remove(actividad);
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
