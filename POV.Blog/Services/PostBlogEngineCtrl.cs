using POV.Blog.BO;
using POV.Blog.Queries;
using POV.Modelo.ContextBlog;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace POV.Blog.Services
{
    public class PostBlogEngineCtrl
    {
        /// <summary>
        /// Contexto interno de conexión a la base de datos
        /// </summary>
        private readonly ContextoBlog _model;

        /// <summary>
        /// Firma de la conexión a la base de datos
        /// </summary>
        private readonly object _sing;

        /// <summary>
        /// Constructor encargado de crear la conexion interna a la base de datos
        /// </summary>
        /// <param name="contexto">Contexto de conexion a la base de datos</param>
        public PostBlogEngineCtrl(ContextoBlog contexto)
        {
            _sing = new object();
            _model = contexto ?? new ContextoBlog(_sing);
        }

        /// <summary>
        /// Consulta la actividades que existen en el sistema
        /// </summary>
        /// <param name="criteria">Actividad que provee el criterio de búsqueda</param>
        /// <param name="tracking">Permite saber si se rastrearán los objetos que se obtengan de la consulta</param>
        /// <returns>Actividades que satisfacen el criterio de búsqueda</returns>
        public List<PostBlogEngine> Retrieve(PostBlogEngine criteria, bool tracking)
        {
            DbQuery<PostBlogEngine> qryPostBlogEngine = (tracking) ? _model.PostsBlogEngine : _model.PostsBlogEngine.AsNoTracking();

            return qryPostBlogEngine.Where(new PostBlogEngineQry(criteria).Action()).ToList();
        }
        /// <summary>
        /// Método para cerrar la conexión del controlador con la base de datos
        /// </summary>
        public void Dispose()
        {
            _model.Disposing(_sing);
        }
    }
}
