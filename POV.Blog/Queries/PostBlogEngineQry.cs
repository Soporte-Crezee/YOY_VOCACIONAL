using System;
using System.Linq;
using System.Linq.Expressions;
using Framework.Base.Entity;
using Framework.Base.Entity.Queries;
using POV.Blog.BO;
using System.Text.RegularExpressions;

namespace POV.Blog.Queries
{
    public class PostBlogEngineQry: IQuery<PostBlogEngine>
    {
        /// <summary>
        /// Propiedad a la que se asignan los criterios de búsqueda
        /// </summary>
        private readonly PostBlogEngine _criteria;

        /// <summary>
        /// Constructor no nulo que recibe los criterios de búsqueda
        /// </summary>
        /// <param name="criteria">Actividad que provee los criterios de búsqueda</param>
        public PostBlogEngineQry(PostBlogEngine criteria)
        {
            _criteria = criteria ?? new PostBlogEngine();
        }

        /// <summary>
        /// Clase implementada del Framework
        /// </summary>
        /// <param name="filters">Filtros del criterio de búsqueda</param>
        /// <returns>Expresión que contendra los filtros de búsqueda</returns>
        public Expression<Func<PostBlogEngine, bool>> Action(params Expression<Func<PostBlogEngine, bool>>[] filters)
        {
            Expression<Func<PostBlogEngine, bool>> exp = x => true;

            if (_criteria == null) return filters.Aggregate(exp, (current, e) => current.And(e));

            if (_criteria.BlogId != null)
                exp = exp.And(a => a.BlogId == _criteria.BlogId);

            if (_criteria.PostId != null)
                exp = exp.And(a => a.PostId == _criteria.PostId);

            if (_criteria.Categoria != null)
                exp = exp.And(a => a.Categoria == _criteria.Categoria);

            exp = filters.Aggregate(exp, (current, e) => current.And(e));

            return exp;
        }

    }
}
