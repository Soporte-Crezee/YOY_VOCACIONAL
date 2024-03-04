using POV.CentroEducativo.BO;
using POV.CentroEducativo.Queries;
using POV.Modelo.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace POV.CentroEducativo.Services
{
    public class UniversidadCtrl : IDisposable
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
        public UniversidadCtrl(Contexto contexto)
        {
            _sing = new object();
            _model = contexto ?? new Contexto(_sing);
        }

        /// <summary>
        /// Consulta la información completa de los Universidades del sistema
        /// </summary>
        /// <param name="criteria">Universidad que provee el criterio de búsqueda</param>
        /// <param name="tracking">Permite saber si se rastrearán los objetos que se obtengan de la consulta</param>
        /// <returns>Universidades que satisfacen el criterio de búsqueda</returns>
        public List<Universidad> RetrieveWithRelationship(Universidad criteria, bool tracking)
        {
            DbQuery<Universidad> qryUniversidad = (tracking) ? _model.Universidad : _model.Universidad.AsNoTracking();

            return qryUniversidad.Where(new UniversidadQry(criteria).Action())
                                .Include(x => x.Carreras)
                                .Include(x => x.Docentes)
                                .Include(x=>x.Alumnos)
                                //.Include(x => x.EventosUniversidad)
                                .ToList();
        }

        public List<Universidad> RetrieveUniversidadByDocente(Docente docente, bool tracking)
        {
            var qry = from uni in _model.Universidad
                      where uni.Docentes.Any(x => x.DocenteID == docente.DocenteID)
                      select uni;
            return qry.ToList();
        }

        public List<Universidad> RetrieveUniversidadByAlumno(Alumno alumno, bool tracking)
        {
            var qry = from uni in _model.Universidad
                      where uni.Alumnos.Any(x => x.AlumnoID == alumno.AlumnoID)
                      select uni;
            return qry.ToList();
        }
        
        /// <summary>
        /// Consulta la Universidades que existen en el sistema
        /// </summary>
        /// <param name="criteria">Universidad que provee el criterio de búsqueda</param>
        /// <param name="tracking">Permite saber si se rastrearán los objetos que se obtengan de la consulta</param>
        /// <returns>Universidades que satisfacen el criterio de búsqueda</returns>
        public List<Universidad> Retrieve(Universidad criteria, bool tracking)
        {
            DbQuery<Universidad> qryUniversidades = (tracking) ? _model.Universidad : _model.Universidad.AsNoTracking();

            return qryUniversidades.Where(new UniversidadQry(criteria).Action()).ToList();
        }

        /// <summary>
        /// Agrega una Universidad al sistema
        /// </summary>
        /// <param name="Universidad"> Universidad que se registrará</param>
        /// <returns>Resultado del registro</returns>
        public Boolean Insert(Universidad universidad)
        {
            var resultado = false;

            #region ** validaciones **
            if (universidad == null) throw new ArgumentNullException("Universidad", "Universidad no puede ser nulo");
            if (string.IsNullOrEmpty(universidad.NombreUniversidad)) throw new ArgumentException("Universidad.NombreUniversidad", "Nombre de Universidad no puede ser nulo o vacio");
            if (universidad.Activo == null) throw new ArgumentNullException("Universidad.Activo", "Activo no puede ser nulo");            
            #endregion

            try
            {
                _model.Universidad.Add(universidad);
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
        /// Actualiza una Universidad
        /// </summary>
        /// <param name="Universidad">Universidad a actualizar</param>
        /// <param name="tareasEliminadas">Lista de tareas a eliminar</param>
        /// <returns></returns>
        public Boolean Update(Universidad universidad, List<Alumno> alumnos = null)
        {
            var resultado = false;

            #region ** validaciones **
            if (universidad == null) throw new ArgumentNullException("Universidad", "Universidad no puede ser nulo");
            if (universidad.UniversidadID == null) throw new ArgumentNullException("Universidad.UniversidadID", "Identificador de Universidad no puede ser nulo");
            if (string.IsNullOrEmpty(universidad.NombreUniversidad)) throw new ArgumentException("Universidad.Nombre", "Nombre de Universidad no puede ser nulo o vacio");
            if (universidad.Activo == null) throw new ArgumentNullException("Universidad.Activo", "Activo no puede ser nulo");
            #endregion

            #region update with Relation
            if (alumnos != null && alumnos.Count > 0)
            {
                _model.Entry(universidad).State = EntityState.Unchanged;
                if (universidad.Alumnos == null)
                    universidad.Alumnos = new List<Alumno>();

                alumnos.ForEach(x =>
                {
                    _model.Entry(x).State = EntityState.Unchanged;
                    universidad.Alumnos.Add(x);
                });
            }
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
