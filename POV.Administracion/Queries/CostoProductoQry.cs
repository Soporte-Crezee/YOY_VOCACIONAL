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
    internal class CostoProductoQry : IQuery<CostoProducto>
    {
        /// <summary>
        /// Propiedad a la que se asignan los criterios de búsqueda
        /// </summary>
        private readonly CostoProducto _criteria;

        /// <summary>
        /// Constructor no nulo que recibe los criterios de búsqueda
        /// </summary>
        /// <param name="criteria">CostoProducto que provee los criterios de busqueda</param>
        public CostoProductoQry(CostoProducto criteria) 
        {
            _criteria = criteria ?? new CostoProducto();
        }

        public Expression<Func<CostoProducto, bool>> Action(params Expression<Func<CostoProducto, bool>>[] filters) 
        {
            Expression<Func<CostoProducto, bool>> exp = x => true;

            if (_criteria == null) return filters.Aggregate(exp, (current, e) => current.And(e));
            if (_criteria.CostoProductoId != null)
                exp = exp.And(x => x.CostoProductoId == _criteria.CostoProductoId);
            if (_criteria.ProductoID != null)
                exp = exp.And(x => x.ProductoID == _criteria.ProductoID);
            if (_criteria.Precio != null)
                exp = exp.And(x => x.Precio == _criteria.Precio);
            if(_criteria.FechaInicio!=null)
                exp = exp.And(x => x.FechaInicio == _criteria.FechaInicio);
            if (_criteria.FechaFin == null)
                exp = exp.And(x => x.FechaFin == _criteria.FechaFin);
            if (_criteria.Producto != null)
            {
                if (!string.IsNullOrEmpty(_criteria.Producto.Nombre))
                    exp = exp.And(x => x.Producto.Nombre == _criteria.Producto.Nombre);
                if (_criteria.Producto.TipoProducto != null)
                    exp = exp.And(x => x.Producto.TipoProducto == _criteria.Producto.TipoProducto);
            }
           
            exp = filters.Aggregate(exp, (current, e) => current.And(e));

            return exp;
        }
    }
}
