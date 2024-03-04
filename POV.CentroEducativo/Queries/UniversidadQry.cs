using POV.CentroEducativo.BO;
using Framework.Base.Entity.Queries;
using System;
using System.Linq;
using System.Linq.Expressions;
using Framework.Base.Entity;

namespace POV.CentroEducativo.Queries
{
    internal class UniversidadQry : IQuery<Universidad>
    {
        /// <summary>
        /// Propiedad a la que se asignan los criterios de búsqueda
        /// </summary>
        private readonly Universidad _criteria;

        /// <summary>
        /// Constructor no nulo que recibe los criterios de búsqueda
        /// </summary>
        /// <param name="criteria">Universidad que provee los criterios de búsqueda</param>
        public UniversidadQry(Universidad criteria)
        {
            _criteria = criteria ?? new Universidad();
        }

        /// <summary>
        /// Clase implementada del Framework
        /// </summary>
        /// <param name="filters">Filtros del criterio de búsqueda</param>
        /// <returns>Expresión que contendra los filtros de búsqueda</returns>
        public Expression<Func<Universidad, bool>> Action(params Expression<Func<Universidad, bool>>[] filters)
        {
            Expression<Func<Universidad, bool>> exp = x => true;

            if (_criteria == null) return filters.Aggregate(exp, (current, e) => current.And(e));

            if (_criteria.UniversidadID != null)
                exp = exp.And(a => a.UniversidadID == _criteria.UniversidadID);

            if(_criteria.Activo != null)
                exp = exp.And(a => a.Activo == _criteria.Activo);
            
            if (!string.IsNullOrEmpty(_criteria.NombreUniversidad))
                exp = exp.And(a => a.NombreUniversidad== _criteria.NombreUniversidad.Trim());
            
            if (!string.IsNullOrEmpty(_criteria.Descripcion))
                exp = exp.And(a => a.Descripcion == _criteria.Descripcion.Trim());


            if (!string.IsNullOrEmpty(_criteria.ClaveEscolar))
                exp = exp.And(a => a.ClaveEscolar == _criteria.ClaveEscolar.Trim());

            if (!string.IsNullOrEmpty(_criteria.Direccion))
                exp = exp.And(a => a.Direccion == _criteria.Direccion.Trim());
            
            exp = filters.Aggregate(exp, (current, e) => current.And(e));

            return exp;
        }
    }
}

