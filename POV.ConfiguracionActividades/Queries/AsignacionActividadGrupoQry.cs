using System;
using System.Linq;
using System.Linq.Expressions;
using Framework.Base.Entity;
using Framework.Base.Entity.Queries;
using POV.ConfiguracionActividades.BO;

namespace POV.ConfiguracionActividades.Queries
{
	internal class AsignacionActividadGrupoQry :IQuery<AsignacionActividadGrupo>
	{
        /// <summary>
        /// Propiedad a la que se asignan los criterios de búsqueda
        /// </summary>
		private readonly AsignacionActividadGrupo _criteria;

        /// <summary>
        /// Constructor no nulo que recibe los criterios de búsqueda
        /// </summary>
        /// <param name="criteria">Asignación de Actividad de Grupo que provee los criterios de búsqueda</param>
		public AsignacionActividadGrupoQry(AsignacionActividadGrupo criteria)
		{
			_criteria = criteria ?? new AsignacionActividadGrupo();
		}

        /// <summary>
        /// Clase implementada del Framework
        /// </summary>
        /// <param name="filters">Filtros del criterio de búsqueda</param>
        /// <returns>Expresión que contendra los filtros de búsqueda</returns>
		public Expression<Func<AsignacionActividadGrupo, bool>> Action(params Expression<Func<AsignacionActividadGrupo, bool>>[] filters)
		{
			Expression<Func<AsignacionActividadGrupo, bool>> exp = x => true;

			if (_criteria == null) return filters.Aggregate(exp, (current, e) => current.And(e));

			if (_criteria.AsignacionActividadGrupoID != null)
                exp = exp.And(a => a.AsignacionActividadGrupoID == _criteria.AsignacionActividadGrupoID);
				
			if (_criteria.ActividadDocente != null && _criteria.ActividadDocente.ActividadID != null)
                exp = exp.And(a => a.ActividadDocente.ActividadID == _criteria.ActividadDocente.ActividadID);
            else if (_criteria.ActividadID != null)
                exp = exp.And(a => a.ActividadID == _criteria.ActividadID);

            if (_criteria.GrupoCicloEscolar != null && _criteria.GrupoCicloEscolar.GrupoCicloEscolarID != null)
                exp = exp.And(a => a.GrupoCicloEscolar.GrupoCicloEscolarID == _criteria.GrupoCicloEscolar.GrupoCicloEscolarID);
            else if (_criteria.GrupoCicloEscolarID != null)
                exp = exp.And(a => a.GrupoCicloEscolarID == _criteria.GrupoCicloEscolarID);

			if (_criteria.FechaCreacion != null)
				exp = exp.And(a => a.FechaCreacion == _criteria.FechaCreacion);

			if (_criteria.FechaFin != null)
				exp = exp.And(a => a.FechaFin == _criteria.FechaFin);

			if (_criteria.FechaInicio != null)
				exp = exp.And(a => a.FechaInicio == _criteria.FechaInicio);

			exp = filters.Aggregate(exp, (current, e) => current.And(e));

            return exp;
		}
	}
}
