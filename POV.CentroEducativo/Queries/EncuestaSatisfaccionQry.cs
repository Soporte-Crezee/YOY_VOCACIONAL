using Framework.Base.Entity.Queries;
using Framework.Base.Entity;
using POV.CentroEducativo.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace POV.CentroEducativo.Queries
{
    internal class EncuestaSatisfaccionQry : IQuery<EncuestaSatisfaccion>
    {
         /// <summary>
        /// Propiedad a la que se asignan los criterios de búsqueda
        /// </summary>
        private readonly EncuestaSatisfaccion _criteria;

        /// <summary>
        /// Constructor no nulo que recibe los criterios de búsqueda
        /// </summary>
        /// <param name="criteria">Alumno que provee los criterios de búsqueda</param>
        public EncuestaSatisfaccionQry(EncuestaSatisfaccion criteria)
        {
            _criteria = criteria ?? new EncuestaSatisfaccion();
        }

        /// <summary>
        /// Clase implementada del Framework
        /// </summary>
        /// <param name="filters">Filtros del criterio de búsqueda</param>
        /// <returns>Expresión que contendra los filtros de búsqueda</returns>
        public Expression<Func<EncuestaSatisfaccion, bool>> Action(params Expression<Func<EncuestaSatisfaccion, bool>>[] filters)
        {
            Expression<Func<EncuestaSatisfaccion, bool>> exp = x => true;

            if (_criteria == null) return filters.Aggregate(exp, (current, e) => current.And(e));

            if (_criteria.EncuestaID != null)
                exp = exp.And(a => a.EncuestaID == _criteria.EncuestaID);
            
            exp = filters.Aggregate(exp, (current, e) => current.And(e));

            return exp;
        }
    }
}
