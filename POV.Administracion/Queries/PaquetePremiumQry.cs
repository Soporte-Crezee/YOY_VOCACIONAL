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
    internal class PaquetePremiumQry : IQuery<PaquetePremium>
    {
        /// <summary>
        /// Propiedad a la que se asignan los criterios de busqueda
        /// </summary>
        private readonly PaquetePremium _criteria;

        /// <summary>
        /// Constructor no nulo que recibe los criterios de busqueda
        /// </summary>
        /// <param name="criteria"> PaquetePremium que provee los criterios de busqueda</param>
        public PaquetePremiumQry(PaquetePremium criteria) 
        {
            _criteria = criteria ?? new PaquetePremium();
        }

        public Expression<Func<PaquetePremium, bool>> Action(params Expression<Func<PaquetePremium, bool>>[] filters) 
        {
            Expression<Func<PaquetePremium, bool>> exp = x => true;

            if (_criteria == null) return filters.Aggregate(exp, (current, e) => current.And(e));
            if (_criteria.PaquetePremiumID != null)
                exp = exp.And(a => a.PaquetePremiumID == _criteria.PaquetePremiumID);
            if (!string.IsNullOrEmpty(_criteria.Nombre))
                exp = exp.And(a => a.Nombre == _criteria.Nombre);
            if (_criteria.CostoPaquete != null)
                exp = exp.And(a => a.CostoPaquete == _criteria.CostoPaquete);
            if (_criteria.Estatus != null)
                exp = exp.And(a => a.Estatus == _criteria.Estatus);

            exp = filters.Aggregate(exp, (current, e) => current.And(e));

            return exp;
        }
    }
}
