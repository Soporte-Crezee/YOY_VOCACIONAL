using System;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using GP.SocialEngine.BO;
using GP.SocialEngine.DAO;
using POV.Reactivos.BO;
using POV.ContenidosDigital.BO;
using POV.CentroEducativo.BO;

namespace GP.SocialEngine.Service
{
    /// <summary>
    /// Servicios para acceder a Comentarios
    /// </summary>
    public class ComentarioCtrl
    {
        /// <summary>
        /// Consulta registros de Comentario en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="comentario">Comentario que provee el criterio de selección para realizar la consulta</param>
        /// <returns>El DataSet que contiene la información de Comentario generada por la consulta</returns>
        public DataSet Retrieve(IDataContext dctx, Comentario comentario, Publicacion publicacion)
        {
            ComentarioRetHlp da = new ComentarioRetHlp();
            DataSet ds = da.Action(dctx, comentario, publicacion);
            return ds;
        }


        public List<Comentario> RetrieveListComentariosPublicacion(IDataContext dctx, Publicacion publicacion)
        {
            List<Comentario> comentarios = new List<Comentario>();

            UsuarioSocialCtrl usuarioSocialCtrl = new UsuarioSocialCtrl();
            RankingCtrl rankingCtrl = new RankingCtrl();
            DataSet dsComentarios = Retrieve(dctx, new Comentario { Estatus = true }, new Publicacion { PublicacionID = publicacion.PublicacionID });
            foreach (DataRow rowComentario in dsComentarios.Tables["Comentario"].Rows)
            {
                Comentario comentario = DataRowToComentario(rowComentario);

                comentario.UsuarioSocial = usuarioSocialCtrl.LastDataRowToUsuarioSocial(usuarioSocialCtrl.Retrieve(dctx, new UsuarioSocial { UsuarioSocialID = comentario.UsuarioSocial.UsuarioSocialID }));
                comentario.Ranking = rankingCtrl.RetrieveComplete(dctx, new Ranking { RankingID = comentario.Ranking.RankingID });

                comentarios.Add(comentario);
            }
            return comentarios;
        }
        /// <summary>
        /// Regresa un registro completo de Comentario
        /// </summary>
        public Comentario RetrieveComplete(IDataContext dctx, Comentario comentario, Publicacion publicacion)
        {
            UsuarioSocialCtrl usuarioSocialCtrl = new UsuarioSocialCtrl();
            RankingCtrl rankingCtrl = new RankingCtrl();
            try
            {
                comentario = LastDataRowToComentario(this.Retrieve(dctx, new Comentario { ComentarioID = comentario.ComentarioID }, new Publicacion { PublicacionID = publicacion.PublicacionID }));
                comentario.UsuarioSocial = usuarioSocialCtrl.LastDataRowToUsuarioSocial(usuarioSocialCtrl.Retrieve(dctx, new UsuarioSocial { UsuarioSocialID = comentario.UsuarioSocial.UsuarioSocialID }));
                comentario.Ranking = rankingCtrl.RetrieveComplete(dctx, new Ranking { RankingID = comentario.Ranking.RankingID });
                return comentario;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// Devueve un Comentario completo
        /// </summary>
        public Comentario RetrieveComplete(IDataContext dctx, Comentario comentario)
        {
            RankingCtrl rankingCtrl = new RankingCtrl();
            UsuarioSocialCtrl usuarioSocialCtrl = new UsuarioSocialCtrl();
            comentario.Ranking = new Ranking();
            comentario.UsuarioSocial = new UsuarioSocial();
            try
            {
                comentario = LastDataRowToComentario(Retrieve(dctx, new Comentario { ComentarioID = comentario.ComentarioID }, null));
                comentario.Ranking = rankingCtrl.RetrieveComplete(dctx, new Ranking { RankingID = comentario.Ranking.RankingID });
                comentario.UsuarioSocial = usuarioSocialCtrl.LastDataRowToUsuarioSocial(usuarioSocialCtrl.Retrieve(dctx, new UsuarioSocial { UsuarioSocialID = comentario.UsuarioSocial.UsuarioSocialID }));
                return comentario;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// Crea un registro de Comentario en la base de datos
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="comentario">Comentario que desea crear</param>
        public void Insert(IDataContext dctx, Comentario comentario, Publicacion publicacion)
        {
            ComentarioInsHlp da = new ComentarioInsHlp();
            da.Action(dctx, comentario, publicacion);
        }
        /// <summary>
        /// Inserta un comentario y la notificacion correspondiente
        /// </summary>
        /// <param name="dctx"></param>
        /// <param name="comentario"></param>
        /// <param name="publicacion"></param>
        public void InsertComplete(IDataContext dctx, Comentario comentario, Publicacion publicacion)
        {
            object myFirm = new object();
            dctx.OpenConnection(myFirm);
            dctx.BeginTransaction(myFirm);
            try
            {
                Insert(dctx, comentario, publicacion);
                InsertNotificacion(dctx, comentario, publicacion);
                dctx.CommitTransaction(myFirm);
            }
            catch (Exception ex)
            {

                dctx.RollbackTransaction(myFirm);
                dctx.CloseConnection(myFirm);
                throw ex;
            }
            finally
            {
                if (dctx.ConnectionState == ConnectionState.Open)
                    dctx.CloseConnection(myFirm);
            }
        }
        public void InsertNotificacion(IDataContext dctx, Comentario comentario, Publicacion publicacion)
        {
            if (comentario == null)
                return;
            if (publicacion == null)
                return;

            PublicacionCtrl publicacionCtrl = new PublicacionCtrl();

            Publicacion publicacionPadre = publicacionCtrl.LastDataRowToPublicacion(publicacionCtrl.Retrieve(dctx, publicacion));
            //verificar que el comentario no sea del dueño de la publicación
            if (publicacionPadre.UsuarioSocial.UsuarioSocialID != comentario.UsuarioSocial.UsuarioSocialID)
            {
                NotificacionCtrl notificacionCtrl = new NotificacionCtrl();

                Notificacion notificacion = new Notificacion();
                notificacion.NotificacionID = Guid.NewGuid();
                notificacion.FechaRegistro = comentario.FechaComentario;
                notificacion.Emisor = comentario.UsuarioSocial;
                notificacion.Receptor = publicacionPadre.UsuarioSocial;
                notificacion.Notificable = comentario;
                notificacion.TipoNotificacion = ETipoNotificacion.COMENTARIO;
                notificacion.EstatusNotificacion = EEstatusNotificacion.NUEVO;


                notificacionCtrl.Insert(dctx, notificacion);
            }
        }
        /// <summary>
        /// Actualiza de manera optimista un registro de Comentario en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="comentario">Comentario que tiene los datos nuevos</param>
        /// <param name="anterior">Comentario que tiene los datos anteriores</param>
        public void Update(IDataContext dctx, Comentario comentario, Comentario previous)
        {
            ComentarioUpdHlp da = new ComentarioUpdHlp();
            da.Action(dctx, comentario, previous);
        }
        /// <summary>
        /// Crea un objeto de Comentario a partir de los datos del último DataRow del DataSet.
        /// </summary>
        /// <param name="ds">El DataSet que contiene la información de Comentario</param>
        /// <returns>Un objeto de Comentario creado a partir de los datos</returns>
        public Comentario LastDataRowToComentario(DataSet ds)
        {
            if (!ds.Tables.Contains("Comentario"))
                throw new Exception("LastDataRowToComentario: DataSet no tiene la tabla Comentario");
            int index = ds.Tables["Comentario"].Rows.Count;
            if (index < 1)
                throw new Exception("LastDataRowToComentario: El DataSet no tiene filas");
            return this.DataRowToComentario(ds.Tables["Comentario"].Rows[index - 1]);
        }

        public void DeleteComplete(IDataContext dctx, Comentario comentario, Publicacion publicacion)
        {
            comentario = this.RetrieveComplete(dctx, comentario);

            ComentarioDelHlp da = new ComentarioDelHlp();
            da.Action(dctx, comentario, publicacion);

            RankingCtrl rankingCtrl = new RankingCtrl();

            rankingCtrl.DeleteComplete(dctx, comentario.Ranking);
        }

        /// <summary>
        /// Desactiva un registro completo de comentario, evalúa si se requiere notificar al usuarioSocial
        /// que realizo el comentario
        /// </summary>
        /// <param name="dctx"></param>
        /// <param name="comentario">Comentario a eliminar</param>
        /// <param name="usuarioSocial">UsuarioSocial que desea eliminar el comentario</param>
        public void RemoveComplete(IDataContext dctx, Comentario comentario, UsuarioSocial usuarioSocial, List<AreaConocimiento> areasConocimiento, long universidadID, bool esAlumno = true, bool dueñocomentario = true)
        {
            if (comentario == null)
                throw new SystemException("RemoveComplete:comentario no puede ser nulo");
            if (comentario.ComentarioID == null)
                throw new SystemException("RemoveComplete:comentarioID no puede ser nulo");
            if (usuarioSocial == null)
                throw new SystemException("RemoveComplete:usuarioSocial no puede ser nulo");
            if (usuarioSocial.UsuarioSocialID == null)
                throw new SystemException("RemoveComplete:UsuarioSocialID no puede ser nulo");


            DataSet ds = Retrieve(dctx, comentario, new Publicacion());
            if (ds.Tables["Comentario"].Rows.Count == 1)
            {
                PublicacionCtrl publicacionCtrl = new PublicacionCtrl();

                //UsuarioSocial que elimina
                UsuarioSocialCtrl usuarioSocialCtrl = new UsuarioSocialCtrl();
                SocialHubCtrl socialHubCtrl = new SocialHubCtrl();
                GrupoSocialCtrl grupoSocialCtrl = new GrupoSocialCtrl();
                SocialHub socialHub = null;
                UsuarioGrupo usuarioGrupo = null;
                UsuarioGrupo usuarioGrupoPublica = null;

                comentario = LastDataRowToComentario(ds);
                comentario = RetrieveComplete(dctx, comentario);

                Publicacion publicacion = publicacionCtrl.RetrievePublicacionPadre(dctx, comentario);
                publicacion = publicacionCtrl.RetrieveComplete(dctx, publicacion);


                usuarioSocial = usuarioSocialCtrl.RetrieveComplete(dctx, usuarioSocial);

                ds = socialHubCtrl.RetrieveSocialHubUsuario(dctx, new SocialHub { SocialProfileType = ESocialProfileType.USUARIOSOCIAL, SocialProfile = usuarioSocial });
                if (ds.Tables["SocialHub"].Rows.Count != 1)
                    throw new SystemException("RemoveComplete:SocialHub no puede ser nulo");

                socialHub = socialHubCtrl.LastDataRowToSocialHub(ds);
                if (socialHub == null)
                    throw new SystemException("RemoveComplete:SocialHub no puede ser nulo");

                //Consultar el Grupo Social de la publicación.
                List<IPrivacidad> lsgrupoSocial = publicacion.Privacidades;
                foreach (IPrivacidad privacidad in lsgrupoSocial)
                {
                    if ((privacidad.GetType() != typeof(GrupoSocial)))
                        continue;

                    //Identificar el usuarioGrupo del usuario que elimina.
                    usuarioGrupo = grupoSocialCtrl.RetrieveFriend(dctx, (GrupoSocial)privacidad, usuarioSocial, areasConocimiento, null, universidadID, esAlumno);

                    //Identificar el usuarioGrupo del usuario que publica
                    usuarioGrupoPublica = grupoSocialCtrl.RetrieveFriend(dctx, (GrupoSocial)privacidad, comentario.UsuarioSocial, areasConocimiento, null, universidadID, dueñocomentario);
                    if (usuarioGrupo != null && usuarioGrupoPublica != null)
                        break;
                }
                //evaluar si el usuario que elimina tiene un grupo
                if (usuarioGrupo == null || usuarioGrupoPublica == null)
                    throw new SystemException("RemoveComplete:UsuarioGrupo no puede ser nulo");

                //evaluar si el usuario tiene permisos para desactivar
                if (comentario.UsuarioSocial.UsuarioSocialID == usuarioSocial.UsuarioSocialID || (bool)usuarioGrupo.EsModerador
                    || publicacion.SocialHub.SocialHubID == socialHub.SocialHubID)
                {
                    Comentario comentarioAnterior = (Comentario)comentario.Clone();
                    comentario.Estatus = false;
                    Update(dctx, comentario, comentarioAnterior);


                    //El usuario que desactiva es docente.
                    if ((bool)usuarioGrupo.EsModerador)
                    {
                        //El usuario que publicó es un alumno
                        if ((bool)(!usuarioGrupoPublica.EsModerador))
                        {
                            //El usuario es un alumno y se creara una notificación para el alumno .
                            //Crear notificación
                            NotificacionCtrl notificacionCtrl = new NotificacionCtrl();
                            Notificacion notificacion = new Notificacion();
                            notificacion.NotificacionID = Guid.NewGuid();
                            notificacion.Emisor = usuarioSocial;
                            notificacion.Receptor = comentario.UsuarioSocial;
                            notificacion.FechaRegistro = DateTime.Now;
                            notificacion.Notificable = comentario;
                            notificacion.TipoNotificacion = ETipoNotificacion.COMENTARIO_ELIMINADO;
                            notificacion.EstatusNotificacion = EEstatusNotificacion.NUEVO;
                            //Registrar notificación.
                            notificacionCtrl.Insert(dctx, notificacion);
                        }
                    }
                }
                else
                {
                    throw new Exception("RemoveComple:no tiene permiso para eliminar la publicación");
                }

            }
        }

        /// <summary>
        /// Crea un objeto de Comentario a partir de los datos de un DataRow.
        /// </summary>
        /// <param name="row">El DataRow que contiene la información de Comentario</param>
        /// <returns>Un objeto de Comentario creado a partir de los datos</returns>
        public Comentario DataRowToComentario(DataRow row)
        {
            Comentario comentario = new Comentario();
            comentario.UsuarioSocial = new UsuarioSocial();
            comentario.Ranking = new Ranking();
            if (row.IsNull("ComentarioID"))
                comentario.ComentarioID = null;
            else
                comentario.ComentarioID = (Guid)Convert.ChangeType(row["ComentarioID"], typeof(Guid));
            if (row.IsNull("Cuerpo"))
                comentario.Cuerpo = null;
            else
                comentario.Cuerpo = (String)Convert.ChangeType(row["Cuerpo"], typeof(String));
            if (row.IsNull("FechaComentario"))
                comentario.FechaComentario = null;
            else
                comentario.FechaComentario = (DateTime)Convert.ChangeType(row["FechaComentario"], typeof(DateTime));
            if (row.IsNull("Estatus"))
                comentario.Estatus = null;
            else
                comentario.Estatus = (Boolean)Convert.ChangeType(row["Estatus"], typeof(Boolean));
            if (row.IsNull("UsuarioSocialID"))
                comentario.UsuarioSocial.UsuarioSocialID = null;
            else
                comentario.UsuarioSocial.UsuarioSocialID = (long)Convert.ChangeType(row["UsuarioSocialID"], typeof(long));
            if (row.IsNull("RankingID"))
                comentario.Ranking.RankingID = null;
            else
                comentario.Ranking.RankingID = (Guid)Convert.ChangeType(row["RankingID"], typeof(Guid));

            if (row.IsNull("TipoPublicacion"))
                comentario.TipoPublicacion = null;
            else
                comentario.TipoPublicacion = (ETipoPublicacion)Convert.ChangeType(row["TipoPublicacion"], typeof(short));

            if (comentario.TipoPublicacion != null)
            {
                switch (comentario.TipoPublicacion)
                {
                    case ETipoPublicacion.APPSOCIAL:
                        {
                            if (!row.IsNull("ReactivoID"))
                            {
                                comentario.AppSocial = new Reactivo { ReactivoID = (Guid)Convert.ChangeType(row["ReactivoID"], typeof(Guid)) };
                            }
                            else if (!row.IsNull("ContenidoDigitalID"))
                            {
                                comentario.AppSocial = new ContenidoDigital { ContenidoDigitalID = (long)Convert.ChangeType(row["ContenidoDigitalID"], typeof(long)) };
                            }
                            break;
                        }
                }
            }
            return comentario;
        }
    }
}
