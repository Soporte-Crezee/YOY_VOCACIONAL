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
    internal class DocenteQry:IQuery<Docente>
    {
        /// <summary>
        /// Propiedad a la que se asignan los criterios de búsqueda
        /// </summary>
        private readonly Docente _criteria;

        /// <summary>
        /// Constructor no nulo que recibe los criterios de búsqueda
        /// </summary>
        /// <param name="criteria">Docente que provee los criterios de búsqueda</param>
        public DocenteQry(Docente criteria)
        {
            _criteria = criteria ?? new Docente();
        }

        /// <summary>
        /// Clase implementada del Framework
        /// </summary>
        /// <param name="filters">Filtros del criterio de búsqueda</param>
        /// <returns>Expresión que contendra los filtros de búsqueda</returns>
        public Expression<Func<Docente, bool>> Action(params Expression<Func<Docente, bool>>[] filters)
        {
            Expression<Func<Docente, bool>> exp = x => true;

            if (_criteria == null) return filters.Aggregate(exp, (current, e) => current.And(e));

            if (_criteria.DocenteID != null)
                exp = exp.And(a => a.DocenteID == _criteria.DocenteID);

            if (_criteria.Nombre != null)
                exp = exp.And(a => a.Nombre == _criteria.Nombre);
            
            if (_criteria.PrimerApellido != null)
                exp = exp.And(a => a.PrimerApellido == _criteria.PrimerApellido);
            
            if (!string.IsNullOrEmpty(_criteria.SegundoApellido))
                exp = exp.And(a => a.SegundoApellido.Contains(_criteria.SegundoApellido.Trim()));

            if (_criteria.Sexo != null)
                exp = exp.And(a => a.Sexo == _criteria.Sexo);

            if (_criteria.Estatus != null)
                exp = exp.And(a => a.Estatus == _criteria.Estatus);

            if (_criteria.Curp != null)
                exp = exp.And(a => a.Curp == _criteria.Curp);

            if (_criteria.PermiteAsignaciones != null)
                exp = exp.And(a => a.PermiteAsignaciones == _criteria.PermiteAsignaciones);

            exp = filters.Aggregate(exp, (current, e) => current.And(e));

            return exp;
        }
    }
}
