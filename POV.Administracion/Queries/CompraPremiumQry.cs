using POV.Administracion.BO;
using Framework.Base.Entity.Queries;
using Framework.Base.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace POV.Administracion.Queries
{
    internal class CompraPremiumQry : IQuery<CompraPremium>
    {
        /// <summary>
        /// Propiedad a la que se asignan los criterios de búsqueda
        /// </summary>
        private readonly CompraPremium _criteria;
        /// <summary>
        /// Constructor no nulo que recibe los criterios de búsqueda
        /// </summary>
        /// <param name="criteria">Carrera que provee los criterios de búsqueda</param>
        public CompraPremiumQry(CompraPremium criteria) 
        {
            _criteria = criteria ?? new CompraPremium();
        }

        public Expression<Func<CompraPremium, bool>> Action(params Expression<Func<CompraPremium, bool>>[] filters)
        {
            Expression<Func<CompraPremium, bool>> exp = x => true;

            if (_criteria == null) return filters.Aggregate(exp, (current, e) => current.And(e));
            if (_criteria.CompraPremiumID != null)
                exp = exp.And(a => a.CompraPremiumID == _criteria.CompraPremiumID);
            if (_criteria.TutorID != null)
                exp = exp.And(a => a.TutorID == _criteria.TutorID);
            if (_criteria.AlumnoID != null)
                exp = exp.And(a => a.AlumnoID == _criteria.AlumnoID);
            if (_criteria.CompraPremiumID != null)
                exp = exp.And(a => a.CompraPremiumID == _criteria.CompraPremiumID);
            if (_criteria.CostoCompra != null)
                exp = exp.And(x => x.CostoCompra == _criteria.CostoCompra);
            if (!string.IsNullOrEmpty(_criteria.CodigoCompra))
                exp = exp.And(x => x.CodigoCompra == _criteria.CodigoCompra);
            if (_criteria.CodigoPaquete != null)
                exp = exp.And(x => x.CodigoPaquete == _criteria.CodigoPaquete);

            exp = filters.Aggregate(exp, (current, e) => current.And(e));

            return exp;
        }
    }
}
