using POV.CentroEducativo.BO;
using Framework.Base.Entity.Queries;
using System;
using System.Linq;
using System.Linq.Expressions;
using Framework.Base.Entity;

namespace POV.CentroEducativo.Queries
{
    internal class CarreraQry : IQuery<Carrera>
    {
        /// <summary>
        /// Propiedad a la que se asignan los criterios de búsqueda
        /// </summary>
        private readonly Carrera _criteria;

        /// <summary>
        /// Constructor no nulo que recibe los criterios de búsqueda
        /// </summary>
        /// <param name="criteria">Carrera que provee los criterios de búsqueda</param>
        public CarreraQry(Carrera criteria)
        {
            _criteria = criteria ?? new Carrera();
        }

        /// <summary>
        /// Clase implementada del Framework
        /// </summary>
        /// <param name="filters">Filtros del criterio de búsqueda</param>
        /// <returns>Expresión que contendra los filtros de búsqueda</returns>
        public Expression<Func<Carrera, bool>> Action(params Expression<Func<Carrera, bool>>[] filters)
        {
            Expression<Func<Carrera, bool>> exp = x => true;

            if (_criteria == null) return filters.Aggregate(exp, (current, e) => current.And(e));

            if (_criteria.CarreraID != null)
                exp = exp.And(a => a.CarreraID == _criteria.CarreraID);

            if (_criteria.ClasificadorID != null)
                exp = exp.And(a => a.ClasificadorID == _criteria.ClasificadorID);
            
            if (_criteria.Activo != null)
                exp = exp.And(a => a.Activo == _criteria.Activo);
            
            if (!string.IsNullOrEmpty(_criteria.NombreCarrera))
                exp = exp.And(a => a.NombreCarrera.Contains(_criteria.NombreCarrera.Trim()));
            
            if (!string.IsNullOrEmpty(_criteria.Descripcion))
                exp = exp.And(a => a.Descripcion.Contains(_criteria.Descripcion.Trim()));
            
            exp = filters.Aggregate(exp, (current, e) => current.And(e));

            return exp;
        }
    }
}
