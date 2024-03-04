using System;
using System.Linq;
using System.Linq.Expressions;
using Framework.Base.Entity;
using Framework.Base.Entity.Queries;
using POV.ConfiguracionesPlataforma.BO;

namespace POV.ConfiguracionesPlataforma.Queries
{
    internal class PreferenciaUsuarioQry: IQuery<PreferenciaUsuario>
    {
        /// <summary>
        /// Propiedad a la que se asignan los criterios de búsqueda
        /// </summary>
        private readonly PreferenciaUsuario _criteria;

           /// <summary>
        /// Constructor no nulo que recibe los criterios de búsqueda
        /// </summary>
        /// <param name="criteria">Perfil de usuario que provee los criterios de búsqueda</param>
        public PreferenciaUsuarioQry(PreferenciaUsuario criteria)
		{
            _criteria = criteria ?? new PreferenciaUsuario();
		}

        /// <summary>
        /// Clase implementada del Framework
        /// </summary>
        /// <param name="filters">Filtros del criterio de búsqueda</param>
        /// <returns>Expresión que contendra los filtros de búsqueda</returns>
        public Expression<Func<PreferenciaUsuario, bool>> Action(params Expression<Func<PreferenciaUsuario, bool>>[] filters)
        {
            Expression<Func<PreferenciaUsuario, bool>> exp = x => true;

            if (_criteria == null) return filters.Aggregate(exp, (current, e) => current.And(e));

            if (_criteria.PreferenciaUsuarioId != null)
                exp = exp.And(a => a.PreferenciaUsuarioId == _criteria.PreferenciaUsuarioId);

            if ((_criteria.Usuarios != null && _criteria.Usuarios.UsuarioID != null))
            {
                exp = exp.And(a => a.UsuarioId == _criteria.Usuarios.UsuarioID);
            }
            else if (_criteria.UsuarioId != null) exp = exp.And(a => a.UsuarioId == _criteria.UsuarioId);

            if ((_criteria.PlantillasLudicas != null && _criteria.PlantillasLudicas.PlantillaLudicaId != null))
            {
                exp = exp.And(a => a.PlantillaLudicaId == _criteria.PlantillaLudicaId);
            }
            else if (_criteria.PlantillaLudicaId != null) exp = exp.And(a => a.PlantillaLudicaId == _criteria.PlantillaLudicaId) ;

            if (_criteria.FechaRegistro != null)
                exp = exp.And(a => a.FechaRegistro == _criteria.FechaRegistro);

            return exp;
        }
    }
}
