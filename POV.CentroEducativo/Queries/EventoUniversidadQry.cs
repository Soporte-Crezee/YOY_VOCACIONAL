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
    internal class EventoUniversidadQry : IQuery<EventoUniversidad>
    {
        /// <summary>
        /// Propiedad a la que se asignan los criterios de búsqueda
        /// </summary>
        private readonly EventoUniversidad _criteria;

        /// <summary>
        /// Constructor no nulo que recibe los criterios de búsqueda
        /// </summary>
        /// <param name="criteria">EventoUniversidad que provee los criterios de búsqueda</param>
        public EventoUniversidadQry(EventoUniversidad criteria)
        {
            _criteria = criteria ?? new EventoUniversidad();
        }

        /// <summary>
        /// Clase implementada del Framework
        /// </summary>
        /// <param name="filters">Filtros del criterio de búsqueda</param>
        /// <returns>Expresión que contendra los filtros de búsqueda</returns>
        public Expression<Func<EventoUniversidad, bool>> Action(params Expression<Func<EventoUniversidad, bool>>[] filters)
        {
            Expression<Func<EventoUniversidad, bool>> exp = x => true;

            if (_criteria == null) return filters.Aggregate(exp, (current, e) => current.And(e));

            if (_criteria.EventoUniversidadId != null)
                exp = exp.And(a => a.EventoUniversidadId == _criteria.EventoUniversidadId);
            
            exp = filters.Aggregate(exp, (current, e) => current.And(e));

            return exp;
        }

    }
}
