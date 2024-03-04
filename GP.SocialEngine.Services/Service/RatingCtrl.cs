using System;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using GP.SocialEngine.BO;
using GP.SocialEngine.DAO;

namespace GP.SocialEngine.Service
{
    /// <summary>
    /// Servicios para acceder al Rating del usuario
    /// </summary>
    public class RatingCtrl
    {
        /// <summary>
        /// Consulta registros de RatingRetHlp en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerÃ¡ acceso a la base de datos</param>
        /// <param name="ratingRetHlp">RatingRetHlp que provee el criterio de selecciÃ³n para realizar la consulta</param>
        /// <returns>El DataSet que contiene la informaciÃ³n de RatingRetHlp generada por la consulta</returns>
        public DataSet Retrieve(IDataContext dctx, Rating Rating)
        {
            RatingRetHlp da = new RatingRetHlp();
            DataSet ds = da.Action(dctx, Rating);
            return ds;
        }
        /// <summary>
        /// Regresa un registro completo de Rating
        /// </summary>
        public Rating RetrieveComplete(IDataContext dctx, Rating rating)
        {
            UsuarioSocialCtrl usuarioSocialCtrl = new UsuarioSocialCtrl();
            try
            {
                rating = LastDataRowToRating(Retrieve(dctx, new Rating { RatingID = rating.RatingID }));
                rating.UsuarioSocial = usuarioSocialCtrl.LastDataRowToUsuarioSocial(usuarioSocialCtrl.Retrieve(dctx, new UsuarioSocial { UsuarioSocialID = rating.UsuarioSocial.UsuarioSocialID }));
                return rating;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// Crea un registro de Rating en la base de datos
        /// </summary>
        /// <param name="dctx">El DataContext que proveerÃ¡ acceso a la base de datos</param>
        /// <param name="rating">Rating que desea crear</param>
        public void Insert(IDataContext dctx, Rating Rating)
        {
            RatingInsHlp da = new RatingInsHlp();
            da.Action(dctx, Rating);
        }
        /// <summary>
        /// Actualiza de manera optimista un registro de RatingUpdHlp en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerÃ¡ acceso a la base de datos</param>
        /// <param name="ratingUpdHlp">RatingUpdHlp que tiene los datos nuevos</param>
        /// <param name="anterior">RatingUpdHlp que tiene los datos anteriores</param>
        public void Update(IDataContext dctx, Rating Rating, Rating previous)
        {
            RatingUpdHlp da = new RatingUpdHlp();
            da.Action(dctx, Rating, previous);
        }
        /// <summary>
        /// Crea un objeto de Rating a partir de los datos del Ãºltimo DataRow del DataSet.
        /// </summary>
        /// <param name="ds">El DataSet que contiene la informaciÃ³n de Rating</param>
        /// <returns>Un objeto de Rating creado a partir de los datos</returns>
        public Rating LastDataRowToRating(DataSet ds)
        {
            if (!ds.Tables.Contains("Rating"))
                throw new Exception("LastDataRowToRating: DataSet no tiene la tabla Rating");
            int index = ds.Tables["Rating"].Rows.Count;
            if (index < 1)
                throw new Exception("LastDataRowToRating: El DataSet no tiene filas");
            return this.DataRowToRating(ds.Tables["Rating"].Rows[index - 1]);
        }
        /// <summary>
        /// Crea un objeto de Rating a partir de los datos de un DataRow.
        /// </summary>
        /// <param name="row">El DataRow que contiene la informaciÃ³n de Rating</param>
        /// <returns>Un objeto de Rating creado a partir de los datos</returns>
        public Rating DataRowToRating(DataRow row)
        {
            Rating rating = new Rating();
            rating.UsuarioSocial = new UsuarioSocial();
            if (row.IsNull("RatingID"))
                rating.RatingID = null;
            else
                rating.RatingID = (long)Convert.ChangeType(row["RatingID"], typeof(long));
            if (row.IsNull("Puntuacion"))
                rating.Puntuacion = null;
            else
                rating.Puntuacion = (int)Convert.ChangeType(row["Puntuacion"], typeof(int));
            if (row.IsNull("FechaRegistro"))
                rating.FechaRegistro = null;
            else
                rating.FechaRegistro = (DateTime)Convert.ChangeType(row["FechaRegistro"], typeof(DateTime));
            if (row.IsNull("UsuarioSocialID"))
                rating.UsuarioSocial.UsuarioSocialID = null;
            else
                rating.UsuarioSocial.UsuarioSocialID = (long)Convert.ChangeType(row["UsuarioSocialID"], typeof(long));
            return rating;
        }
    }
}
