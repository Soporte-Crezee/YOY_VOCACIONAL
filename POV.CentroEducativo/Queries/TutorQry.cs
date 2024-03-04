using POV.CentroEducativo.BO;
using Framework.Base.Entity.Queries;
using Framework.Base.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace POV.CentroEducativo.Queries
{
    internal class TutorQry : IQuery<Tutor>
    {
        /// <summary>
        /// Propiedad a la que se asignan los criterios de búsqueda
        /// </summary>
        private readonly Tutor _criteria;
        /// <summary>
        /// Constructor no nulo que recibe los criterios de búsqueda
        /// </summary>
        /// <param name="criteria">Carrera que provee los criterios de búsqueda</param>
        public TutorQry(Tutor criteria) 
        {
            _criteria = criteria ?? new Tutor();
        }

        public Expression<Func<Tutor, bool>> Action(params Expression<Func<Tutor, bool>>[] filters) 
        {
            Expression<Func<Tutor, bool>> exp = x => true;

            if (_criteria == null) return filters.Aggregate(exp, (current, e) => current.And(e));
            if (_criteria.TutorID != null)
                exp = exp.And(a => a.TutorID == _criteria.TutorID);
            if (!string.IsNullOrEmpty(_criteria.Nombre))
                exp = exp.And(x => x.Nombre == _criteria.Nombre);
            if (!string.IsNullOrEmpty(_criteria.PrimerApellido))
                exp = exp.And(x => x.PrimerApellido == _criteria.PrimerApellido);
            if (!string.IsNullOrEmpty(_criteria.SegundoApellido))
                exp = exp.And(x => x.SegundoApellido == _criteria.SegundoApellido);
            if(_criteria.Sexo!=null)
                exp = exp.And(x => x.Sexo == _criteria.Sexo);
            if (!string.IsNullOrEmpty(_criteria.Codigo))
                exp = exp.And(x => x.Codigo == _criteria.Codigo);
            if (!string.IsNullOrEmpty(_criteria.CorreoElectronico))
                exp = exp.And(x => x.CorreoElectronico == _criteria.CorreoElectronico);
            if (!string.IsNullOrEmpty(_criteria.Curp))
                exp = exp.And(x => x.Curp == _criteria.Curp);
            if (_criteria.NotificacionPago != null)
                exp = exp.And(x => x.NotificacionPago == _criteria.NotificacionPago);

            exp = filters.Aggregate(exp, (current, e) => current.And(e));
           
            return exp;
        }
    }
}
