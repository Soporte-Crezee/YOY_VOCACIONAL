using POV.CentroEducativo.BO;
using POV.CentroEducativo.Queries;
using POV.Modelo.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POV.CentroEducativo.Services
{
    public class EFDocenteCtrl : IDisposable
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
        public EFDocenteCtrl(Contexto contexto)
        {
            _sing = new object();
            _model = contexto ?? new Contexto(_sing);
        }

        /// <summary>
        /// Consulta la información completa de los Docentes del sistema
        /// </summary>
        /// <param name="criteria">Docente que provee el criterio de búsqueda</param>
        /// <param name="tracking">Permite saber si se rastrearán los objetos que se obtengan de la consulta</param>
        /// <returns>Docentes que satisfacen el criterio de búsqueda</returns>
        public List<Docente> RetrieveWithRelationship(Docente criteria, bool tracking)
        {
            DbQuery<Docente> qryDocente = (tracking) ? _model.Docente : _model.Docente.AsNoTracking();

            return qryDocente.Where(new DocenteQry(criteria).Action())
                                //.Include(x => x.Carreras)
                                //.Include(x => x.Docentes)
                                .Include(x => x.Universidades)
                //.Include(x => x.EventosUniversidad)
                                .ToList();
        }

        /// <summary>
        /// Consulta la Docentees que existen en el sistema
        /// </summary>
        /// <param name="criteria">Docente que provee el criterio de búsqueda</param>
        /// <param name="tracking">Permite saber si se rastrearán los objetos que se obtengan de la consulta</param>
        /// <returns>Docentees que satisfacen el criterio de búsqueda</returns>
        public List<Docente> Retrieve(Docente criteria, bool tracking)
        {
            DbQuery<Docente> qryDocentees = (tracking) ? _model.Docente : _model.Docente.AsNoTracking();

            return qryDocentees.Where(new DocenteQry(criteria).Action()).ToList();
        }

        //public Docente GetDocenteMenosCarga__()
        //{
        //    var docentes = Retrieve(new Docente() { Estatus = true , PermiteAsignaciones = true}, false).ToList();
        //    var docente = (from row in docentes
        //                     orderby row.Alumnos.Count(), row.NombreCompletoDocente
        //                     select row).ToList().FirstOrDefault();

        //    return docente;
        //}

        public Boolean Update(Docente docente)
        {
            var resultado = false;
            #region Validaciones
            if (docente == null) throw new ArgumentException("Docente", "Docente no puede ser nulo");
            if (docente.DocenteID == null) throw new ArgumentException("docente.DocenteID", "Identicador de docente no puede ser nullo");
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
