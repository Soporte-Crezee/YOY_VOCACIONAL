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
    internal class ProductoCosteoQry : IQuery<ProductoCosteo>
    {
        /// <summary>
        /// Propiedad a la que se asignan los criterios de búsqueda
        /// </summary>
        private readonly ProductoCosteo _criteria;

        /// <summary>
        /// Constructor no nulo que recibe los criterios de búsqueda
        /// </summary>
        /// <param name="criteria">CostoProducto que provee los criterios de busqueda</param>
        public ProductoCosteoQry(ProductoCosteo criteria) 
        {
            _criteria = criteria ?? new ProductoCosteo();
        }

        public Expression<Func<ProductoCosteo, bool>> Action(params Expression<Func<ProductoCosteo, bool>>[] filters) 
        {
            Expression<Func<ProductoCosteo, bool>> exp = x => true;

            if (_criteria == null) return filters.Aggregate(exp, (current, e) => current.And(e));
            if (_criteria.ProductoID != null)
                exp = exp.And(x => x.ProductoID == _criteria.ProductoID);
            if (!String.IsNullOrEmpty(_criteria.Nombre))
                exp = exp.And(x => x.Nombre == _criteria.Nombre);
            if (!String.IsNullOrEmpty(_criteria.Descripcion))
                exp = exp.And(x => x.Descripcion == _criteria.Descripcion);
            if (_criteria.TipoProducto != null)
                exp = exp.And(x => x.TipoProducto == _criteria.TipoProducto);
           
            exp = filters.Aggregate(exp, (current, e) => current.And(e));

            return exp;
        }
    }
}
