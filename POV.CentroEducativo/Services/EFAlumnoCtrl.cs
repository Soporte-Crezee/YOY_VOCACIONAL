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

namespace POV.CentroEducativo.Services
{
    public class EFAlumnoCtrl : IDisposable
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
        public EFAlumnoCtrl(Contexto contexto)
        {
            _sing = new object();
            _model = contexto ?? new Contexto(_sing);
        }

        /// <summary>
        /// Consulta la Alumnoes que existen en el sistema
        /// </summary>
        /// <param name="criteria">Alumno que provee el criterio de búsqueda</param>
        /// <param name="tracking">Permite saber si se rastrearán los objetos que se obtengan de la consulta</param>
        /// <returns>Alumnoes que satisfacen el criterio de búsqueda</returns>
        public List<Alumno> Retrieve(Alumno criteria, bool tracking)
        {
            DbQuery<Alumno> qryAlumnoes = (tracking) ? _model.Alumno : _model.Alumno.AsNoTracking();

            return qryAlumnoes.Where(new AlumnoQry(criteria).Action()).ToList();
        }

        /// <summary>
        /// Actualiza un Alumno
        /// </summary>
        /// <param name="alumno">Alumno a actualizar</param>
        /// <returns></returns>
        public Boolean Update(Alumno alumno)
        {
            var resultado = false;

            #region ** validaciones **
            if (alumno == null) throw new ArgumentNullException("Alumno", "Alumno no puede ser nulo");
            if (alumno.AlumnoID == null) throw new ArgumentNullException("alumno.AlumnoID", "Identificador de alumno no puede ser nulo");
            if (string.IsNullOrEmpty(alumno.Nombre)) throw new ArgumentException("alumno.Nombre", "Nombre de Alumno no puede ser nulo o vacio");
            if (string.IsNullOrEmpty(alumno.PrimerApellido)) throw new ArgumentException("alumno.PrimerApellido", "Primer Apellido de alumno no puede ser nulo o vacio");
            
            #endregion

            var afectados = _model.Commit(_sing);
            if (afectados != 0) resultado = true;

            return resultado;
        }

        public List<Alumno> GetAlumnosMenosCarga( List<Alumno> alumnos, int expComprados)
        {           
            alumnos = (from a in alumnos
                           orderby a.Universidades.Count(), a.Nombre
                       select a).Take(expComprados).ToList();

            return alumnos;
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
