using System;
using System.Linq;
using System.Linq.Expressions;
using Framework.Base.Entity;
using Framework.Base.Entity.Queries;
using POV.ConfiguracionesPlataforma.BO;

namespace POV.ConfiguracionesPlataforma.Queries
{
    internal class PlantillaLudicaQry: IQuery<PlantillaLudica>
    {
        /// <summary>
        /// Propiedad a la que se asignan los criterios de búsqueda
        /// </summary>
        private readonly PlantillaLudica _criteria;

           /// <summary>
        /// Constructor no nulo que recibe los criterios de búsqueda
        /// </summary>
        /// <param name="criteria">Asignación de Actividad que provee los criterios de búsqueda</param>
        public PlantillaLudicaQry(PlantillaLudica criteria)
		{
            _criteria = criteria ?? new PlantillaLudica();
		}

        /// <summary>
        /// Clase implementada del Framework
        /// </summary>
        /// <param name="filters">Filtros del criterio de búsqueda</param>
        /// <returns>Expresión que contendra los filtros de búsqueda</returns>
        public Expression<Func<PlantillaLudica, bool>> Action(params Expression<Func<PlantillaLudica, bool>>[] filters)
        {
            Expression<Func<PlantillaLudica, bool>> exp = x => true;

            if (_criteria == null) return filters.Aggregate(exp, (current, e) => current.And(e));

            if (_criteria.PlantillaLudicaId != null)
                exp = exp.And(a => a.PlantillaLudicaId == _criteria.PlantillaLudicaId);

            if (!string.IsNullOrEmpty(_criteria.Nombre))
                exp = exp.And(a => a.Nombre.Contains( _criteria.Nombre.Trim()));

            if (_criteria.EsPredeterminado != null)
                exp = exp.And(a => a.EsPredeterminado == _criteria.EsPredeterminado);

            if (_criteria.FechaRegistro != null)
                exp = exp.And(a => a.FechaRegistro == _criteria.FechaRegistro);

            if (_criteria.Activo != null)
                exp = exp.And(a => a.Activo == _criteria.Activo);

            return exp;
        }
    }
}
