using POV.Blog.BO;
using POV.Blog.Queries;
using POV.Modelo.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POV.Blog.Services
{
    public class PostFavoritoCtrl
    {
        /// <summary>
        /// Contexto interno de conexión a la base de datos
        /// </summary>
        private readonly Contexto _model;

        /// <summary>
        /// Firma de la conexión a la base de datos
        /// </summary>
        private readonly object _sing;

        /// <summary>
        /// Constructor encargado de crear la conexion interna a la base de datos
        /// </summary>
        /// <param name="contexto">Contexto de conexion a la base de datos</param>
        public PostFavoritoCtrl(Contexto contexto)
        {
            _sing = new object();
            _model = contexto ?? new Contexto(_sing);
        }

        /// <summary>
        /// Consulta la actividades que existen en el sistema
        /// </summary>
        /// <param name="criteria">Actividad que provee el criterio de búsqueda</param>
        /// <param name="tracking">Permite saber si se rastrearán los objetos que se obtengan de la consulta</param>
        /// <returns>Actividades que satisfacen el criterio de búsqueda</returns>
        public List<PostFavorito> Retrieve(PostFavorito criteria, bool tracking)
        {
            DbQuery<PostFavorito> qryActividades = (tracking) ? _model.PostFavoritos : _model.PostFavoritos.AsNoTracking();

            return qryActividades.Where(new PostFavoritoQry(criteria).Action()).ToList();
        }

        /// <summary>
        /// Agrega una actividad al sistema
        /// </summary>
        /// <param name="Actividad"> Actividad que se registrará</param>
        /// <returns>Resultado del registro</returns>
        public Boolean Insert(PostFavorito postFavorito)
        {
            var resultado = false;

            #region ** validaciones **
            if (postFavorito == null) throw new ArgumentNullException("postFavorito", "PostFavorito no puede ser nulo");
            if (postFavorito.AlumnoId == null) throw new ArgumentNullException("postFavorito.AlumnoId", "Identificador del Aspirante no puede ser nulo");
            if (postFavorito.PostId == null) throw new ArgumentNullException("actividad.PostId", "Identificador del Post no puede ser nulo");
            #endregion

            try
            {
                _model.PostFavoritos.Add(postFavorito);
                var afectados = _model.Commit(_sing);

                if (afectados != 0) resultado = true;
            }
            catch (Exception exception)
            {
                throw exception;
            }

            return resultado;
        }

        /// <summary>
        /// Elimina una actividad al sistema
        /// </summary>
        /// <param name="Actividad"> Actividad que se registrará</param>
        /// <returns>Resultado del registro</returns>
        public Boolean Delete(PostFavorito postFavorito)
        {
            var resultado = false;

            try
            {
                _model.PostFavoritos.Remove(postFavorito);
                var afectados = _model.Commit(_sing);

                if (afectados != 0) resultado = true;
            }
            catch (Exception exception)
            {
                throw exception;
            }

            return resultado;
        }

        /// <summary>
        /// Actualiza una Carrera
        /// </summary>
        /// <param name="Carrera">Carrera a actualizar</param>
        /// <param name="tareasEliminadas">Lista de tareas a eliminar</param>
        /// <returns></returns>
        public Boolean Update(PostFavorito postFavorito)
        {
            var resultado = false;

            #region ** validaciones **
            if (postFavorito == null) throw new ArgumentNullException("Carrera", "Carrera no puede ser nulo");
            if (postFavorito.AlumnoId == null) throw new ArgumentNullException("postFavorito.AlumnoId", "Identificador del alumno no puede ser nulo");
            if (postFavorito.BlogId == null) throw new ArgumentException("postFavorito.BlogId", "Identificador del Blog no puede ser nulo o vacio");
            if (postFavorito.PostId == null) throw new ArgumentException("postFavorito.PostId", "Identificador del post no puede ser nulo o vacio");
            if (String.IsNullOrEmpty(postFavorito.Categorias)) throw new ArgumentNullException("postFavorito.Categorias", "Categorias no puede ser nulo");

            #endregion

            var afectados = _model.Commit(_sing);
            if (afectados != 0) resultado = true;

            return resultado;
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
