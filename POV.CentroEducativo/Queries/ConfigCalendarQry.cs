using POV.CentroEducativo.BO;
using Framework.Base.Entity.Queries;
using Framework.Base.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using POV.Seguridad.BO;

namespace POV.CentroEducativo.Queries
{
    internal class ConfigCalendarQry : IQuery<ConfigCalendar>
    {
        /// <summary>
        /// Propiedad a la que se asignan los criterios de búsqueda
        /// </summary>
        private readonly ConfigCalendar _criteria;

        /// <summary>
        /// Constructor no nulo que recibe los criterios de búsqueda
        /// </summary>
        /// <param name="criteria">ConfigCalendar que provee los criterios de búsqueda</param>
        public ConfigCalendarQry(ConfigCalendar criteria)
        {
            _criteria = criteria ?? new ConfigCalendar();
        }

        /// <summary>
        /// Clase implementada del Framework
        /// </summary>
        /// <param name="filters">Filtros del criterio de búsqueda</param>
        /// <returns>Expresión que contendra los filtros de búsqueda</returns>
        public Expression<Func<ConfigCalendar, bool>> Action(params Expression<Func<ConfigCalendar, bool>>[] filters)
        {
            Expression<Func<ConfigCalendar, bool>> exp = x => true;

            if (_criteria == null) return filters.Aggregate(exp, (current, e) => current.And(e));

            if (_criteria.ConfigCalendarID != null)
                exp = exp.And(a => a.ConfigCalendarID == _criteria.ConfigCalendarID);

            if (_criteria.UsuarioID != null)
                exp = exp.And(a => a.UsuarioID == _criteria.UsuarioID);

            exp = filters.Aggregate(exp, (current, e) => current.And(e));

            return exp;
        }
    }
}
