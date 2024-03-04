using Framework.Base.Entity.Queries;
using Framework.Base.Entity;
using POV.CentroEducativo.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace POV.CentroEducativo.Queries
{
    internal class SesionOrientacionQry : IQuery<SesionOrientacion>
    {
        /// <summary>
        /// Propiedad a la que se asignan los criterios de búsqueda
        /// </summary>
        private readonly SesionOrientacion _criteria;

        /// <summary>
        /// Constructor no nulo que recibe los criterios de búsqueda
        /// </summary>
        /// <param name="criteria"> SessionOrientacion que provee los criterios de búsqueda </param>
        public SesionOrientacionQry(SesionOrientacion criteria) 
        {
            _criteria = criteria ?? new SesionOrientacion();
        }

        /// <summary>
        /// Clase implementada del Framework
        /// </summary>
        /// <param name="filters"> Filtros del criterio de búsqueda </param>
        /// <returns> Expresión que contendra los filtros de búsqueda </returns>
        public Expression<Func<SesionOrientacion, bool>> Action(params Expression<Func<SesionOrientacion, bool>>[] filters) 
        {
            Expression<Func<SesionOrientacion, bool>> exp = x => true;

            if (_criteria == null) return filters.Aggregate(exp, (current, e) => current.And(e));

            if (_criteria.SesionOrientacionID != null)
                exp = exp.And(x => x.SesionOrientacionID == _criteria.SesionOrientacionID);

            if (_criteria.AlumnoID != null)
                exp = exp.And(x => x.AlumnoID == _criteria.AlumnoID);

            if (_criteria.DocenteID != null)
                exp = exp.And(x => x.DocenteID == _criteria.DocenteID);

            if (_criteria.EstatusSesion != null)
                exp = exp.And(x => x.EstatusSesion == _criteria.EstatusSesion);

            if (_criteria.Fecha != null)
                exp = exp.And(x => x.Fecha == _criteria.Fecha);

            if (!string.IsNullOrEmpty(_criteria.HoraFinalizado))
                exp = exp.And(x => x.HoraFinalizado == _criteria.HoraFinalizado);

            exp = filters.Aggregate(exp, (current, e) => current.And(e));

            return exp;
        }
    }
}
