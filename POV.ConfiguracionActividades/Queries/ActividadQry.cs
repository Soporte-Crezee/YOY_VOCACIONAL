using System;
using System.Linq;
using System.Linq.Expressions;
using Framework.Base.Entity;
using Framework.Base.Entity.Queries;
using POV.ConfiguracionActividades.BO;

namespace POV.ConfiguracionActividades.Queries
{
    internal class ActividadQry : IQuery<Actividad>
    {
        /// <summary>
        /// Propiedad a la que se asignan los criterios de búsqueda
        /// </summary>
        private readonly Actividad _criteria;

        /// <summary>
        /// Constructor no nulo que recibe los criterios de búsqueda
        /// </summary>
        /// <param name="criteria">Actividad que provee los criterios de búsqueda</param>
        public ActividadQry(Actividad criteria)
        {
            _criteria = criteria ?? new Actividad();
        }

        /// <summary>
        /// Clase implementada del Framework
        /// </summary>
        /// <param name="filters">Filtros del criterio de búsqueda</param>
        /// <returns>Expresión que contendra los filtros de búsqueda</returns>
        public Expression<Func<Actividad, bool>> Action(params Expression<Func<Actividad, bool>>[] filters)
        {
            Expression<Func<Actividad, bool>> exp = x => true;

            if (_criteria == null) return filters.Aggregate(exp, (current, e) => current.And(e));

            if (_criteria.ActividadID != null)
                exp = exp.And(a => a.ActividadID == _criteria.ActividadID);

            if (_criteria.Activo != null)
                exp = exp.And(a => a.Activo == _criteria.Activo);

            if (!string.IsNullOrEmpty(_criteria.Nombre))
                exp = exp.And(a => a.Nombre.Contains(_criteria.Nombre.Trim()));
            if (!string.IsNullOrEmpty(_criteria.Descripcion))
                exp = exp.And(a => a.Descripcion.Contains(_criteria.Descripcion.Trim()));

            if (_criteria.BloqueActividad != null && _criteria.BloqueActividad.BloqueActividadId != null)
                exp = exp.And(a => a.BloqueActividad.BloqueActividadId == _criteria.BloqueActividad.BloqueActividadId);
            else if (_criteria.BloqueActividadId != null)
                exp = exp.And(a => a.BloqueActividadId == _criteria.BloqueActividadId);


            if (_criteria.Escuela != null && _criteria.Escuela.EscuelaID != null)
                exp = exp.And(a => a.Escuela.EscuelaID == _criteria.Escuela.EscuelaID);
            else if (_criteria.EscuelaId != null)
                exp = exp.And(a => a.EscuelaId == _criteria.EscuelaId);

            if (_criteria.Clasificador != null && _criteria.Clasificador.ClasificadorID != null)
                exp = exp.And(a => a.Clasificador.ClasificadorID == _criteria.Clasificador.ClasificadorID);
            else if (_criteria.ClasificadorID != null)
                exp = exp.And(a => a.ClasificadorID == _criteria.ClasificadorID);

            else if (_criteria.ActividadID != null)
                exp = exp.And(a => a.ActividadID == _criteria.ActividadID);

            
            exp = filters.Aggregate(exp, (current, e) => current.And(e));

            return exp;
        }
    }
}
