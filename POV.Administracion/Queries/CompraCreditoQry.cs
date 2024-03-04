using Framework.Base.Entity.Queries;
using Framework.Base.Entity;
using POV.Administracion.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace POV.Administracion.Queries
{
    internal class CompraCreditoQry : IQuery<CompraCredito>
    {
        /// <summary>
        /// Propiedad a la que se asignan los criterios de búsqueda
        /// </summary>
        private readonly CompraCredito _criteria;

        /// <summary>
        /// Constructor no nulo que recibe los criterios de búsqueda
        /// </summary>
        /// <param name="criteria"> CompraCredito que provee los criterios de búsqueda </param>
        public CompraCreditoQry(CompraCredito criteria) 
        {
            _criteria = _criteria ?? new CompraCredito();
        }

        public Expression<Func<CompraCredito, bool>> Action(params Expression<Func<CompraCredito, bool>>[] filters) 
        {
            Expression<Func<CompraCredito, bool>> exp = x => true;

            if (_criteria == null) return filters.Aggregate(exp, (current, e) => current.And(e));
            if (_criteria.CompraCreditoID != null)
                exp = exp.And(x => x.CompraCreditoID == _criteria.CompraCreditoID);
            if (_criteria.TutorID != null)
                exp = exp.And(x => x.TutorID == _criteria.TutorID);
            if (_criteria.AlumnoID != null)
                exp = exp.And(x => x.AlumnoID == _criteria.AlumnoID);
            if (_criteria.CostoCompra != null)
                exp = exp.And(x => x.CostoCompra == _criteria.CostoCompra);
            if (!string.IsNullOrEmpty(_criteria.CodigoCompra))
                exp = exp.And(x => x.CodigoCompra == _criteria.CodigoCompra);

            exp = filters.Aggregate(exp, (current, e) => current.And(e));

            return exp;

        }
    }
}
