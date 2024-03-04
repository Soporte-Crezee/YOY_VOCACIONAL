using System;
using System.Linq;
using System.Linq.Expressions;
using Framework.Base.Entity;
using Framework.Base.Entity.Queries;
using POV.ConfiguracionActividades.BO;
using POV.Seguridad.BO;

namespace POV.ConfiguracionActividades.Queries
{
    internal class ActividadDocenteQry : IQuery<ActividadDocente>
    {
        /// <summary>
        /// Propiedad a la que se asignan los criterios de búsqueda
        /// </summary>
        private readonly ActividadDocente _criteria;

        /// <summary>
        /// Constructor no nulo que recibe los criterios de búsqueda
        /// </summary>
        /// <param name="criteria">Actividad que provee los criterios de búsqueda</param>
        public ActividadDocenteQry(ActividadDocente criteria)
        {
            _criteria = criteria ?? new ActividadDocente();
        }

        /// <summary>
        /// Clase implementada del Framework
        /// </summary>
        /// <param name="filters">Filtros del criterio de búsqueda</param>
        /// <returns>Expresión que contendra los filtros de búsqueda</returns>
        public Expression<Func<ActividadDocente, bool>> Action(params Expression<Func<ActividadDocente, bool>>[] filters)
        {
            Expression<Func<ActividadDocente, bool>> exp = x => true;

            if (_criteria == null) return filters.Aggregate(exp, (current, e) => current.And(e));

            if (_criteria.ActividadID != null)
                exp = exp.And(a => a.ActividadID == _criteria.ActividadID);

            if (_criteria.Activo != null)
                exp = exp.And(a => a.Activo == _criteria.Activo);

            if (!string.IsNullOrEmpty(_criteria.Nombre))
                exp = exp.And(a => a.Nombre.Contains(_criteria.Nombre.Trim()));
            if (!string.IsNullOrEmpty(_criteria.Descripcion))
                exp = exp.And(a => a.Descripcion.Contains(_criteria.Descripcion.Trim()));
            
            if (_criteria.Escuela != null && _criteria.Escuela.EscuelaID != null)
                exp = exp.And(a => a.Escuela.EscuelaID == _criteria.Escuela.EscuelaID);
            else if (_criteria.EscuelaId != null)
                exp = exp.And(a => a.EscuelaId == _criteria.EscuelaId);

            if (_criteria.Docente != null && _criteria.Docente.DocenteID != null)
                exp = exp.And(a => a.Docente.DocenteID == _criteria.Docente.DocenteID);
            else if (_criteria.DocenteId != null)
                exp = exp.And(a => a.DocenteId == _criteria.DocenteId);

            if (_criteria.Usuario != null && _criteria.Usuario.UsuarioID != null)
                exp = exp.And(a => a.Usuario.UsuarioID == _criteria.Usuario.UsuarioID);
            else if (_criteria.UsuarioId != null)
                exp = exp.And(a => a.UsuarioId == _criteria.UsuarioId);

            exp = filters.Aggregate(exp, (current, e) => current.And(e));

            return exp;
        }
    }
}
