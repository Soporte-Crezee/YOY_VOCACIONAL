using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Framework.Base.Entity.Queries;
using POV.CentroEducativo.BO;
using Framework.Base.Entity;

namespace POV.CentroEducativo.Queries
{
    internal class TutorAlumnoQry : IQuery<TutorAlumno>
    {
        /// <summary>
        /// Propiedad a la que se asignan los criterios de búsqueda
        /// </summary>
        private readonly TutorAlumno _criteria;

        /// <summary>
        /// Constructor no nulo que recibe los criterios de búsqueda
        /// </summary>
        /// <param name="criteria">Carrera que provee los criterios de búsqueda</param>
        public TutorAlumnoQry(TutorAlumno criteria)
        {
            _criteria = criteria ?? new TutorAlumno();
        }

        /// <summary>
        /// Clase implementada del Framework
        /// </summary>
        /// <param name="filters">Filtros del criterio de búsqueda</param>
        /// <returns>Expresión que contendra los filtros de búsqueda</returns>
        public Expression<Func<TutorAlumno, bool>> Action(params Expression<Func<TutorAlumno, bool>>[] filters)
        {
            Expression<Func<TutorAlumno, bool>> exp = x => true;

            if (_criteria == null) return filters.Aggregate(exp, (current, e) => current.And(e));

            if (_criteria.TutorID != null) exp = exp.And(a => a.TutorID == _criteria.TutorID);

            if (_criteria.AlumnoID != null) exp = exp.And(a => a.AlumnoID == _criteria.AlumnoID);
            
            exp = filters.Aggregate(exp, (current, e) => current.And(e));

            return exp;
        }
    }
}
