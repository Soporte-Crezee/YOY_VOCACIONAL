using System;
using System.Linq;
using System.Linq.Expressions;
using Framework.Base.Entity;
using Framework.Base.Entity.Queries;
using POV.ConfiguracionActividades.BO;
using POV.CentroEducativo.BO;

namespace POV.ConfiguracionActividades.Queries
{
    class BloqueActividadQry: IQuery<BloqueActividad>
    {
        /// <summary>
        /// Propiedad a la que se asignan los criterios de búsqueda
        /// </summary>
        private readonly BloqueActividad _criteria;

        /// <summary>
        /// Constructor no nulo que recibe los criterios de búsqueda
        /// </summary>
        /// <param name="criteria">BloqueActividad que provee los criterios de búsqueda</param>
        public BloqueActividadQry(BloqueActividad criteria)
        {
            _criteria = criteria ?? new BloqueActividad();
        }

        /// <summary>
        /// Clase implementada del Framework
        /// </summary>
        /// <param name="filters">Filtros del criterio de búsqueda</param>
        /// <returns>Expresión que contendra los filtros de búsqueda</returns>
        public Expression<Func<BloqueActividad, bool>> Action(params Expression<Func<BloqueActividad, bool>>[] filters)
        {
            Expression<Func<BloqueActividad, bool>> exp = x => true;

            if (_criteria == null) return filters.Aggregate(exp, (current, e) => current.And(e));

            if (_criteria.BloqueActividadId != null)
                exp = exp.And(a => a.BloqueActividadId == _criteria.BloqueActividadId);

            if (_criteria.Activo != null)
                exp = exp.And(a => a.Activo == _criteria.Activo);

            if (_criteria.FechaInicio != null)
                exp = exp.And(a => a.FechaInicio == _criteria.FechaInicio);

            if (_criteria.FechaFin != null)
                exp = exp.And(a => a.FechaFin == _criteria.FechaFin);
            if (_criteria.FechaRegistro != null)
                exp = exp.And(a => a.FechaRegistro == _criteria.FechaRegistro);
            if (!string.IsNullOrEmpty(_criteria.Nombre))
                exp = exp.And(a => a.Nombre.Contains(_criteria.Nombre.Trim()));

            if (_criteria.CicloEscolar != null && _criteria.CicloEscolar.CicloEscolarID != null)
                exp = exp.And(a => a.CicloEscolar.CicloEscolarID == _criteria.CicloEscolar.CicloEscolarID);
            else if (_criteria.CicloEscolarId != null)
                exp = exp.And(a => a.CicloEscolarId == _criteria.CicloEscolarId);

            if (_criteria.Escuela != null && _criteria.Escuela.EscuelaID != null)
                exp = exp.And(a => a.Escuela.EscuelaID == _criteria.Escuela.EscuelaID);
            else if (_criteria.EscuelaId != null)
                exp = exp.And(a => a.EscuelaId == _criteria.EscuelaId);
            
            exp = filters.Aggregate(exp, (current, e) => current.And(e));

            return exp;
        }
    }
}
