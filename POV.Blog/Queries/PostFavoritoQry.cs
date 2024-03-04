using System;
using System.Linq;
using System.Linq.Expressions;
using Framework.Base.Entity;
using Framework.Base.Entity.Queries;
using POV.Blog.BO;

namespace POV.Blog.Queries
{
    public class PostFavoritoQry : IQuery<PostFavorito>
    {
        /// <summary>
        /// Propiedad a la que se asignan los criterios de búsqueda
        /// </summary>
        private readonly PostFavorito _criteria;

        /// <summary>
        /// Constructor no nulo que recibe los criterios de búsqueda
        /// </summary>
        /// <param name="criteria">Actividad que provee los criterios de búsqueda</param>
        public PostFavoritoQry(PostFavorito criteria)
        {
            _criteria = criteria ?? new PostFavorito();
        }

        /// <summary>
        /// Clase implementada del Framework
        /// </summary>
        /// <param name="filters">Filtros del criterio de búsqueda</param>
        /// <returns>Expresión que contendra los filtros de búsqueda</returns>
        public Expression<Func<PostFavorito, bool>> Action(params Expression<Func<PostFavorito, bool>>[] filters)
        {
            Expression<Func<PostFavorito, bool>> exp = x => true;

            if (_criteria == null) return filters.Aggregate(exp, (current, e) => current.And(e));

            if (_criteria.AlumnoId != null)
                exp = exp.And(a => a.AlumnoId == _criteria.AlumnoId);

            if (_criteria.BlogId != null)
                exp = exp.And(a => a.BlogId == _criteria.BlogId);

            if (_criteria.PostId != null)
                exp = exp.And(a => a.PostId == _criteria.PostId);
            
            exp = filters.Aggregate(exp, (current, e) => current.And(e));

            return exp;
        }
    }
}
