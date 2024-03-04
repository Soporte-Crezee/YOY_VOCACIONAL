//using System;
//using System.Collections.Generic;
//using System.Data.Entity.Infrastructure;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using POV.CentroEducativo.BO;
//using POV.CentroEducativo.Queries;
//using POV.Modelo.Context;
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
    public class TutorAlumnoCtrl : IDisposable
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
        public TutorAlumnoCtrl(Contexto contexto)
        {
            _sing = new object();
            _model = contexto ?? new Contexto(_sing);
        }

        /// <summary>
        /// Consulta la TutorAlumnoes que existen en el sistema
        /// </summary>
        /// <param name="criteria">TutorAlumno que provee el criterio de búsqueda</param>
        /// <param name="tracking">Permite saber si se rastrearán los objetos que se obtengan de la consulta</param>
        /// <returns>TutorAlumnoes que satisfacen el criterio de búsqueda</returns>
        public List<TutorAlumno> Retrieve(TutorAlumno criteria, bool tracking)
        {
            DbQuery<TutorAlumno> qryTutorAlumnos = (tracking) ? _model.TutorAlumno : _model.TutorAlumno.AsNoTracking();

            return qryTutorAlumnos.Where(new TutorAlumnoQry(criteria).Action())
                //.Include(x=>x.Tutor)
                .ToList();
        }

        /// <summary>
        /// Actualiza una TutorAlumno
        /// </summary>
        /// <param name="TutorAlumno">TutorAlumno a actualizar</param>
        /// <param name="tareasEliminadas">Lista de tareas a eliminar</param>
        /// <returns></returns>
        public Boolean Update(TutorAlumno TutorAlumno, List<long> tareasEliminadas = null, List<int> enfoquesEliminados = null)
        {
            var resultado = false;

            #region ** validaciones **
            if (TutorAlumno == null) throw new ArgumentNullException("TutorAlumno", "TutorAlumno no puede ser nulo");
            if (TutorAlumno.Tutor.TutorID == null) throw new ArgumentNullException("TutorAlumno.Tutor.TutorID", "Identificador de Tutor no puede ser nulo");
            if (TutorAlumno.Alumno.AlumnoID == null) throw new ArgumentException("TutorAlumno.Alumno.AlumnoID", "Identificador de Alumno no puede ser nulo o vacio");
            if (TutorAlumno.Parentesco == null) throw new ArgumentException("TutorAlumno.Parentesco", "Parentesco de TutorAlumno no puede ser nulo o vacio");

            #endregion

            var afectados = _model.Commit(_sing);
            if (afectados != 0) resultado = true;

            return resultado;
        }

        /// <summary>
        /// Elimina una TutorAlumno de la base de datos
        /// </summary>
        /// <param name="tarea"></param>
        /// <returns></returns>
        public Boolean Delete(TutorAlumno TutorAlumno)
        {
            var resultado = false;

            try
            {
                _model.TutorAlumno.Remove(TutorAlumno);
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
