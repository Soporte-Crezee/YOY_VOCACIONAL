using Framework.Base.Entity;
using Framework.Base.Entity.Queries;
using POV.CentroEducativo.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace POV.CentroEducativo.Queries
{
    internal class NotaCompraQry : IQuery<NotaCompra>
    {
        /// <summary>
        /// Propiedad a la que se asignan los criterios de búsqueda
        /// </summary>
        private readonly NotaCompra _criteria;

        /// <summary>
        /// Constructor no nulo que recibe los criterios de búsqueda
        /// </summary>
        /// <param name="criteria">NotaCompra que provee los criterios de búsqueda</param>
        public NotaCompraQry(NotaCompra criteria) 
        {
            _criteria = criteria ?? new NotaCompra();
        }

        /// <summary>
        /// Clase implementada del Framework
        /// </summary>
        /// <param name="filters">Filtros del criterio de búsqueda</param>
        /// <returns>Expresión que contendra los filtros de búsqueda</returns>
        public Expression<Func<NotaCompra, bool>> Action(params Expression<Func<NotaCompra, bool>>[] filters) 
        {
            Expression<Func<NotaCompra, bool>> exp = x => true;

            if (_criteria == null) return filters.Aggregate(exp, (current, e) => current.And(e));

            if (_criteria.TutorID != null)
                exp = exp.And(n => n.TutorID == _criteria.TutorID);
            if (_criteria.AlumnoID != null)
                exp = exp.And(n => n.AlumnoID == _criteria.AlumnoID);
            if (_criteria.FechaCompra != null)
                exp = exp.And(n => n.FechaCompra == _criteria.FechaCompra);
            if (_criteria.CostoProductoID != null)
                exp = exp.And(n => n.CostoProductoID == _criteria.CostoProductoID);
            if (_criteria.Cantidad != null)
                exp = exp.And(n => n.Cantidad == _criteria.Cantidad);
            if (_criteria.ConceptoCompra != null)
                exp = exp.And(n => n.ConceptoCompra == _criteria.ConceptoCompra);

            exp = filters.Aggregate(exp, (current, e) => current.And(e));

            return exp;
        }
    }
}
