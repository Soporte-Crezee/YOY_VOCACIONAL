using POV.CentroEducativo.BO;
using Framework.Base.Entity.Queries;
using Framework.Base.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace POV.CentroEducativo.Queries
{
    internal class AlumnoQry : IQuery<Alumno>
    {
        /// <summary>
        /// Propiedad a la que se asignan los criterios de búsqueda
        /// </summary>
        private readonly Alumno _criteria;

        /// <summary>
        /// Constructor no nulo que recibe los criterios de búsqueda
        /// </summary>
        /// <param name="criteria">Alumno que provee los criterios de búsqueda</param>
        public AlumnoQry(Alumno criteria)
        {
            _criteria = criteria ?? new Alumno();
        }

        /// <summary>
        /// Clase implementada del Framework
        /// </summary>
        /// <param name="filters">Filtros del criterio de búsqueda</param>
        /// <returns>Expresión que contendra los filtros de búsqueda</returns>
        public Expression<Func<Alumno, bool>> Action(params Expression<Func<Alumno, bool>>[] filters)
        {
            Expression<Func<Alumno, bool>> exp = x => true;

            if (_criteria == null) return filters.Aggregate(exp, (current, e) => current.And(e));

            if (_criteria.AlumnoID != null)
                exp = exp.And(a => a.AlumnoID == _criteria.AlumnoID);
            if (_criteria.Curp != null)
                exp = exp.And(a => a.Curp == _criteria.Curp);
            if (_criteria.IDReferenciaOXXO != null)
                exp = exp.And(a => a.IDReferenciaOXXO == _criteria.IDReferenciaOXXO);
            if (_criteria.ReferenciaOXXO != null)
                exp = exp.And(a => a.ReferenciaOXXO == _criteria.ReferenciaOXXO);
            
            exp = filters.Aggregate(exp, (current, e) => current.And(e));

            return exp;
        }
    }
}
