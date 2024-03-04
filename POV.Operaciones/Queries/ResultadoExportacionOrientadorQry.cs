using Framework.Base.Entity.Queries;
using Framework.Base.Entity;
using POV.Operaciones.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace POV.Operaciones.Queries
{
    internal class ResultadoExportacionOrientadorQry : IQuery<ResultadoExportacionOrientador>
    {
        /// <summary>
        /// Propiedad a la que se asignan los criterios de búsqueda
        /// </summary>
        private readonly ResultadoExportacionOrientador _criteria;

        /// <summary>
        /// Constructor no nulo que recibe los criterios de búsqueda
        /// </summary>
        /// <param name="criteria"> ResultadoExportacionOrientador que provee los criterios de busqueda </param>
        public ResultadoExportacionOrientadorQry(ResultadoExportacionOrientador criteria) 
        {
            _criteria = criteria ?? new ResultadoExportacionOrientador();
        }

        /// <summary>
        /// Clase implementada del Framework
        /// </summary>
        /// <param name="filters"> Filtros del criterio de búsqueda </param>
        /// <returns> Expresión que contendrá los filtro de búsqueda </returns>
        public Expression<Func<ResultadoExportacionOrientador, bool>> Action(params Expression<Func<ResultadoExportacionOrientador, bool>>[] filters) 
        {
            Expression<Func<ResultadoExportacionOrientador, bool>> exp = x => true;

            if (_criteria == null) return filters.Aggregate(exp, (current, e) => current.And(e));

            if (!string.IsNullOrEmpty(_criteria.Curp))
                exp = exp.And(x => x.Curp == _criteria.Curp);

            if (_criteria.FechaRegistro != null)
                exp = exp.And(x => x.FechaRegistro == _criteria.FechaRegistro);

            exp = filters.Aggregate(exp, (current, e) => current.And(e));

            return exp;
        }
    }
}
