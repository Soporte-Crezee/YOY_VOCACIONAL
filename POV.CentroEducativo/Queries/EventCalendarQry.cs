using Framework.Base.Entity;
using Framework.Base.Entity.Queries;
using POV.Seguridad.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace POV.CentroEducativo.Queries
{
    internal class EventCalendarQry : IQuery<EventCalendar>
    {
        /// <summary>
        /// Propiedad a la que se asignan los criterios de búsqueda
        /// </summary>
        private readonly EventCalendar _criteria;

        /// <summary>
        /// Constructor no nulo que recibe los criterios de búsqueda
        /// </summary>
        /// <param name="criteria"> EventCalendar que provee kis criterios de búsqueda </param>
        public EventCalendarQry(EventCalendar criteria) 
        {
            _criteria = criteria ?? new EventCalendar();
        }

        public Expression<Func<EventCalendar,bool>>Action(params Expression<Func<EventCalendar,bool>>[] filters)
        {
            Expression<Func<EventCalendar, bool>> exp = x => true;

            if (_criteria == null) return filters.Aggregate(exp, (current, e) => current.And(e));

            if (_criteria.UsuarioID != null)
                exp = exp.And(a => a.UsuarioID == _criteria.UsuarioID);

            if (_criteria.EventCalendarID != null)
                exp = exp.And(a => a.EventCalendarID == _criteria.EventCalendarID);

            if (_criteria.ConfigCalendarID != null)
                exp = exp.And(a => a.ConfigCalendarID == _criteria.ConfigCalendarID);

            if (_criteria.AlumnoID != null)
                exp = exp.And(a => a.AlumnoID == _criteria.AlumnoID);

            if (!String.IsNullOrEmpty(_criteria.Fecha))
                exp = exp.And(a => a.Fecha == _criteria.Fecha);

            if (_criteria.CantidadHoras != null)
                exp = exp.And(a => a.CantidadHoras == _criteria.CantidadHoras);

            if (!String.IsNullOrEmpty(_criteria.HrsInicio))
                exp = exp.And(a => a.HrsInicio == _criteria.HrsInicio);

            if (!String.IsNullOrEmpty(_criteria.HrsFin))
                exp = exp.And(a => a.HrsFin == _criteria.HrsFin);

            exp = filters.Aggregate(exp, (current, e) => current.And(e));

            return exp;
        }
    }
}
