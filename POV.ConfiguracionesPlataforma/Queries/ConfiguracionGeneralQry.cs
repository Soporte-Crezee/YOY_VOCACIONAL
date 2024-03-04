using System;
using System.Linq;
using System.Linq.Expressions;
using Framework.Base.Entity;
using Framework.Base.Entity.Queries;
using POV.ConfiguracionesPlataforma.BO;

namespace POV.ConfiguracionesPlataforma.Queries
{
    internal class ConfiguracionGeneralQry:IQuery<ConfiguracionGeneral>
    {
        /// <summary>
        /// Propiedad a la que se asignan los criterios de búsqueda
        /// </summary>
        private readonly ConfiguracionGeneral _criteria;

        /// <summary>
        /// Constructor no nulo que recibe los criterios de búsqueda
        /// </summary>
        /// <param name="criteria">Configuracion General que provee los criterios de búsqueda</param>
        public ConfiguracionGeneralQry(ConfiguracionGeneral criteria)
		{
            _criteria = criteria ?? new ConfiguracionGeneral();
		}

        /// <summary>
        /// Clase implementada del Framework
        /// </summary>
        /// <param name="filters">Filtros del criterio de búsqueda</param>
        /// <returns>Expresión que contendra los filtros de búsqueda</returns>
        public Expression<Func<ConfiguracionGeneral, bool>> Action(params Expression<Func<ConfiguracionGeneral, bool>>[] filters)
        {
            Expression<Func<ConfiguracionGeneral, bool>> exp = x => true;

            if (_criteria == null) return filters.Aggregate(exp, (current, e) => current.And(e));

            if (_criteria.ConfiguracionGeneralId != null)
                exp = exp.And(a => a.ConfiguracionGeneralId == _criteria.ConfiguracionGeneralId);

            if (_criteria.MaximoTopAlumnos != null)
                exp = exp.And(a => a.MaximoTopAlumnos == _criteria.MaximoTopAlumnos);

            if (!string.IsNullOrEmpty(_criteria.RutaServidorContenido))
                exp = exp.And(a => a.RutaServidorContenido == _criteria.RutaServidorContenido);

            if (!string.IsNullOrEmpty(_criteria.RutaPlantillas))
                exp = exp.And(a => a.RutaPlantillas == _criteria.RutaPlantillas);

            return exp;
        }
    }
}
