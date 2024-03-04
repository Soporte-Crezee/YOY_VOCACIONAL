using Framework.Base.Entity;
using Framework.Base.Entity.Queries;
using POV.Administracion.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace POV.Administracion.Queries
{
    internal class CompraProductoQry : IQuery<CompraProducto>
    {
        /// <summary>
        /// Propiedad a la que se asignan los criterio de busqueda
        /// </summary>
        private readonly CompraProducto _criteria;

        /// <summary>
        /// Constructor no nulo que recibe los criterios de busqueda
        /// </summary>
        /// <param name="criteria"> CompraProducto que provee los criterios de busqueda </param>
        public CompraProductoQry(CompraProducto criteria) 
        {
            _criteria = criteria ?? new CompraProducto();
        }

        public Expression<Func<CompraProducto, bool>> Action(params Expression<Func<CompraProducto, bool>>[] filters) 
        {
            Expression<Func<CompraProducto, bool>> exp = x => true;

            if (_criteria == null) return filters.Aggregate(exp, (current, e) => current.And(e));
            if (_criteria.CompraProductoID != null)
                exp = exp.And(x => x.CompraProductoID == _criteria.CompraProductoID);
            if (_criteria.UniversidadID != null)
                exp = exp.And(x => x.UniversidadID == _criteria.UniversidadID);
            if (_criteria.CostoCompra != null)
                exp = exp.And(x => x.CostoCompra == _criteria.CostoCompra);
            if (!String.IsNullOrEmpty(_criteria.CodigoCompra))
                exp = exp.And(x => x.CodigoCompra == _criteria.CodigoCompra);

            exp = filters.Aggregate(exp, (current, e) => current.And(e));

            return exp;

        }
    }
}
