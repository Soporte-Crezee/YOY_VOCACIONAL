using System;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Collections.Generic;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using GP.SocialEngine.BO;
using GP.SocialEngine.Interfaces;
using GP.SocialEngine.DAO;
using POV.Reactivos.BO;
using GP.SocialEngine.DA;
using POV.ContenidosDigital.BO;
using POV.CentroEducativo.BO;

namespace GP.SocialEngine.Service
{
    /// <summary>
    /// Controlador del objeto Publicacion
    /// </summary>
    public class PublicacionCtrl
    {
        /// <summary>
        /// Consulta registros de PublicacionRetHlp en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerÃ¡ acceso a la base de datos</param>
        /// <param name="publicacionRetHlp">PublicacionRetHlp que provee el criterio de selecciÃ³n para realizar la consulta</param>
        /// <returns>El DataSet que contiene la informaciÃ³n de PublicacionRetHlp generada por la consulta</returns>
        public DataSet Retrieve(IDataContext dctx, Publicacion publicacion)
        {
            PublicacionRetHlp da = new PublicacionRetHlp();
            DataSet ds = da.Action(dctx, publicacion);
            return ds;
        }

        /// <summary>
        /// Devuelve una lista paginada de publicaciones en los muros solicitados
        /// </summary>
        /// <param name="dctx"></param>
        /// <param name="pageSize"></param>
        /// <param name="currentPage"></param>
        /// <param name="sortColumn"></param>
        /// <param name="sortorder"></param>
        /// <param name="parametros"></param>
        /// <param name="muros"></param>
        /// <param name="privacidades"></param>
        /// <returns></returns>
        public List<Publicacion> RetrieveListPublicacionMuros(IDataContext dctx, int pageSize, int currentPage,
            string sortColumn, string sortorder, Dictionary<string, string> parametros, List<Int64?> muros, List<IPrivacidad> privacidades)
        {
            List<Publicacion> publicaciones = new List<Publicacion>();

            PublicacionesMurosDARetHlp da = new PublicacionesMurosDARetHlp();

            DataSet ds = da.Action(dctx, pageSize, currentPage, sortColumn, sortorder, parametros, muros, privacidades);

            if (ds.Tables[0].Rows.Count > 0)
            {
                publicaciones = DataSetToListPublicacionComplete(dctx, ds);
            }


            return publicaciones;
        }

        public List<Publicacion> RetrieveListPublicaciones(IDataContext dctx, int pageSize, int currentPage, string sortColumn, string sortorder,
            Dictionary<string, string> parametros, List<IPrivacidad> privacidades)
        {
            List<Publicacion> publicaciones = new List<Publicacion>();

            PublicacionDARetHlp da = new PublicacionDARetHlp();

            DataSet ds = da.Action(dctx, pageSize, currentPage, sortColumn, sortorder, parametros, privacidades);

            if (ds.Tables[0].Rows.Count > 0)
            {
                publicaciones = DataSetToListPublicacionComplete(dctx, ds);
            }


            return publicaciones;
        }

        public List<Publicacion> RetrieveListPublicacionesContactos(IDataContext dctx, int pageSize, int currentPage,
            string sortColumn, string sortorder, Dictionary<string, string> parametros, List<Int64?> listaUsuariosSocial, List<IPrivacidad> privacidades)
        {
            PublicacionesDARetHlp da = new PublicacionesDARetHlp();

            DataSet ds = da.Action(dctx, pageSize, currentPage, sortColumn, sortorder, parametros, listaUsuariosSocial, privacidades);
            
            
            List<Publicacion> publicaciones = new List<Publicacion>();
            if (ds.Tables[0].Rows.Count > 0)
            {
                publicaciones = DataSetToListPublicacionComplete(dctx, ds);
            }


            return publicaciones;
        }

        /// <summary>
        /// Convierte un data set de publicaciones en una lista con los datos completos de publicacion
        /// </summary>
        /// <param name="dctx"></param>
        /// <param name="dsPublicaciones"></param>
        /// <returns></returns>
        private List<Publicacion> DataSetToListPublicacionComplete(IDataContext dctx, DataSet dsPublicaciones)
        {
            List<Publicacion> publicaciones = new List<Publicacion>();

            SocialHubCtrl socialHubCtrl = new SocialHubCtrl();
            ComentarioCtrl comentarioCtrl = new ComentarioCtrl();
            UsuarioSocialCtrl usuarioSocialCtrl = new UsuarioSocialCtrl();
            RankingCtrl rankingCtrl = new RankingCtrl();


            if (dsPublicaciones.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow drPublicacion in dsPublicaciones.Tables[0].Rows)
                {
                    Publicacion publicacion = DataRowToPublicacion(drPublicacion);
                    publicacion.SocialHub = socialHubCtrl.LastDataRowToSocialHub(socialHubCtrl.Retrieve(dctx, new SocialHub { SocialHubID = publicacion.SocialHub.SocialHubID }));
                    if (publicacion.SocialHub.SocialProfileType == ESocialProfileType.USUARIOSOCIAL)
                    {
                        publicacion.SocialHub.SocialProfile = usuarioSocialCtrl.LastDataRowToUsuarioSocial(usuarioSocialCtrl.Retrieve(dctx, new UsuarioSocial { UsuarioSocialID = (publicacion.SocialHub.SocialProfile as UsuarioSocial).UsuarioSocialID }));
                    }

                    publicacion.Ranking = rankingCtrl.RetrieveComplete(dctx, new Ranking { RankingID = publicacion.Ranking.RankingID });
                    publicacion.UsuarioSocial = usuarioSocialCtrl.LastDataRowToUsuarioSocial(usuarioSocialCtrl.Retrieve(dctx, new UsuarioSocial { UsuarioSocialID = publicacion.UsuarioSocial.UsuarioSocialID }));
                    publicacion.ComentariosPublicacion = new List<Comentario>();
                    publicacion.Privacidades = RetrievePrivacidadPublicacion(dctx, publicacion);

                    publicacion.ComentariosPublicacion = comentarioCtrl.RetrieveListComentariosPublicacion(dctx, publicacion);


                    publicaciones.Add(publicacion);
                }
            }
            return publicaciones;
        }
        /// <summary>
        /// Devuelve un registro completo de la publicacion
        /// </summary>
        public Publicacion RetrieveComplete(IDataContext dctx, Publicacion publicacion)
        {
            SocialHubCtrl socialHubCtrl = new SocialHubCtrl();
            ComentarioCtrl comentarioCtrl = new ComentarioCtrl();
            UsuarioSocialCtrl usuarioSocialCtrl = new UsuarioSocialCtrl();
            RankingCtrl rankingCtrl = new RankingCtrl();
           
            publicacion = LastDataRowToPublicacion(Retrieve(dctx, new Publicacion { PublicacionID = publicacion.PublicacionID }));
            publicacion.SocialHub = socialHubCtrl.LastDataRowToSocialHub(socialHubCtrl.Retrieve(dctx, new SocialHub { SocialHubID = publicacion.SocialHub.SocialHubID }));
            if (publicacion.SocialHub.SocialProfileType == ESocialProfileType.USUARIOSOCIAL)
            {
                publicacion.SocialHub.SocialProfile = usuarioSocialCtrl.LastDataRowToUsuarioSocial(usuarioSocialCtrl.Retrieve(dctx, new UsuarioSocial { UsuarioSocialID = (publicacion.SocialHub.SocialProfile as UsuarioSocial).UsuarioSocialID }));
            }
  
            publicacion.Ranking = rankingCtrl.RetrieveComplete(dctx, new Ranking { RankingID = publicacion.Ranking.RankingID });
            publicacion.UsuarioSocial = usuarioSocialCtrl.LastDataRowToUsuarioSocial(usuarioSocialCtrl.Retrieve(dctx, new UsuarioSocial { UsuarioSocialID = publicacion.UsuarioSocial.UsuarioSocialID }));
            publicacion.ComentariosPublicacion = new List<Comentario>();
            publicacion.Privacidades = RetrievePrivacidadPublicacion(dctx, publicacion);

            publicacion.ComentariosPublicacion = comentarioCtrl.RetrieveListComentariosPublicacion(dctx, publicacion);
            return publicacion;
            
        }
        /// <summary>
        /// Devuele un listado de privacidades de una publicacion
        /// </summary>
        /// <param name="dctx"></param>
        /// <param name="publicacion"></param>
        /// <returns>listado de privacidades</returns>
        public List<IPrivacidad> RetrievePrivacidadPublicacion(IDataContext dctx, Publicacion publicacion)
        {
            List<IPrivacidad> privacidades = new List<IPrivacidad>();

            PublicacionPrivacidadRetHlp da = new PublicacionPrivacidadRetHlp();

            DataSet dsPrivacidad = da.Action(dctx, publicacion);

            UsuarioSocialCtrl usuarioSocialCtrl = new UsuarioSocialCtrl();
            GrupoSocialCtrl grupoSocialCtrl = new GrupoSocialCtrl();

            foreach (DataRow row in dsPrivacidad.Tables["Privacidad"].Rows)
            {
                //privacidad para un grupo social
                if (!row.IsNull("GrupoSocialID"))
                {
                    GrupoSocial grupoSocial = new GrupoSocial();
                    grupoSocial.GrupoSocialID = (long)Convert.ChangeType(row["GrupoSocialID"], typeof(long));
                    grupoSocial = grupoSocialCtrl.LastDataRowToGrupoSocial(grupoSocialCtrl.Retrieve(dctx, grupoSocial));

                    privacidades.Add(grupoSocial);
                    
                }

                //privacidad para un usuario social
                if (!row.IsNull("UsuarioSocialID"))
                {
                    UsuarioSocial usuarioSocial = new UsuarioSocial();
                    usuarioSocial.UsuarioSocialID = (long)Convert.ChangeType(row["UsuarioSocialID"], typeof(long));
                    usuarioSocial = usuarioSocialCtrl.LastDataRowToUsuarioSocial(usuarioSocialCtrl.Retrieve(dctx,usuarioSocial));

                    privacidades.Add(usuarioSocial);

                }
            }

            return privacidades;
        }
        /// <summary>
        /// Devuele la publicacion padre de un comentario
        /// </summary>
        /// <param name="dctx"></param>
        /// <param name="comentario"></param>
        /// <returns></returns>
        public Publicacion RetrievePublicacionPadre(IDataContext dctx, Comentario comentario)
        {
            Publicacion publicacion = new Publicacion();

            ComentarioCtrl comentarioCtrl = new ComentarioCtrl();

            DataSet ds = comentarioCtrl.Retrieve(dctx, comentario, new Publicacion());
            int index = ds.Tables["Comentario"].Rows.Count;
            DataRow row = ds.Tables["Comentario"].Rows[index - 1];

            publicacion.PublicacionID = (Guid)Convert.ChangeType(row["PublicacionID"], typeof(Guid));


            publicacion = LastDataRowToPublicacion(Retrieve(dctx, publicacion));

            return publicacion;
        }
        /// <summary>
        /// Crea un registro de PublicacionInsHlp en la base de datos
        /// </summary>
        /// <param name="dctx">El DataContext que proveerÃ¡ acceso a la base de datos</param>
        /// <param name="publicacionInsHlp">PublicacionInsHlp que desea crear</param>
        public void Insert(IDataContext dctx, Publicacion publicacion)
        {
            PublicacionInsHlp da = new PublicacionInsHlp();
            PublicacionPrivacidadInsHlp daPrivacidad = new PublicacionPrivacidadInsHlp();
            
            /*** validaciones ***/
            if (publicacion == null)
                throw new Exception("La publicacion no puede ser nula.");
            if (publicacion.Privacidades == null)
                throw new Exception("La lista de privacidades no puede ser nula.");
            if (publicacion.Privacidades.Count == 0)
                throw new Exception("La lista de privacidades no puede ser vacia.");

            object myFirm = new object();
            dctx.OpenConnection(myFirm);
            dctx.BeginTransaction(myFirm);

            try
            {
                
                da.Action(dctx, publicacion);

                foreach (IPrivacidad privacidad in publicacion.Privacidades) 
                {
                    daPrivacidad.Action(dctx, publicacion, privacidad);
                }
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
        /// <summary>
        /// Inserta una publicacion y registra la notificacion correspondiente
        /// </summary>
        /// <param name="dctx"></param>
        /// <param name="publicacion"></param>
        public void InsertComplete(IDataContext dctx, Publicacion publicacion, List<AreaConocimiento> areasConocimiento, long universidadID, bool esAlumno = true)
        {
            object myFirm = new object();
            dctx.OpenConnection(myFirm);
            dctx.BeginTransaction(myFirm);
            RankingCtrl ratingCtrl = new RankingCtrl();

            
            try
            {

                ratingCtrl.Insert(dctx, publicacion.Ranking);
                Insert(dctx, publicacion);


                InsertNotificacion(dctx, publicacion, areasConocimiento, universidadID, esAlumno);
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
        /// <summary>
        /// Inserta una notificacion de publicacion
        /// </summary>
        /// <param name="dctx"></param>
        /// <param name="publicacion"></param>
        public void InsertNotificacion(IDataContext dctx, Publicacion publicacion, List<AreaConocimiento> areasConocimiento, long universidadID, bool esAlumno = true)
        {
            if (publicacion == null)
                return;
            if (publicacion.PublicacionID == null)
                return;
            publicacion = RetrieveComplete(dctx, publicacion);
            //notificacion
            SocialHub socialHubPub = publicacion.SocialHub;

                //Consultar el Grupo Social
            GrupoSocialCtrl grupoSocialCtrl = new GrupoSocialCtrl();
            List<IPrivacidad> lsgrupoSocial = publicacion.Privacidades;

            //validar que el socialhub sea de un usuario
            if (socialHubPub.SocialProfileType == ESocialProfileType.USUARIOSOCIAL)
            {
                UsuarioSocial usuarioSocialMuro = (UsuarioSocial)socialHubPub.SocialProfile;
                if (lsgrupoSocial != null && lsgrupoSocial.Count == 1)
                {
                         NotificacionCtrl notificacionCtrl = new NotificacionCtrl();
                         GrupoSocial grupoSocial = (lsgrupoSocial[0] as GrupoSocial);
                         UsuarioGrupo usuarioGrupo = grupoSocialCtrl.RetrieveFriend(dctx, grupoSocial, publicacion.UsuarioSocial, areasConocimiento, null, universidadID, esAlumno);

                        //Publicación de un docente
                        if (usuarioGrupo.EsModerador != null && (bool)usuarioGrupo.EsModerador)
                        {
                             //En su muro.
                            if (usuarioSocialMuro.UsuarioSocialID == publicacion.UsuarioSocial.UsuarioSocialID)
                            {
                                grupoSocial = grupoSocialCtrl.RetrieveComplete(dctx, new GrupoSocial { GrupoSocialID = grupoSocial.GrupoSocialID }, areasConocimiento, null, universidadID);

                                var lug = grupoSocial.ListaUsuarioGrupo.GroupBy(x => x.UsuarioGrupoID).Distinct().ToList();


                                var lstUsarioGrupo = (from c in grupoSocial.ListaUsuarioGrupo 
                                         select new UsuarioGrupo()
                                         {
                                             UsuarioGrupoID = c.UsuarioGrupoID,
                                             FechaAsignacion = c.FechaAsignacion,
                                             Estatus = c.Estatus,
                                             EsModerador = c.EsModerador,
                                             UsuarioSocial = c.UsuarioSocial,
                                             DocenteID = c.DocenteID
                                         }).GroupBy(g=>g.UsuarioGrupoID).Distinct().Select(grp=>grp.First()).ToList();

                                if (grupoSocial.ListaUsuarioGrupo != null && grupoSocial.ListaUsuarioGrupo.Count > 0)
                                {
                                    foreach (UsuarioGrupo usrGrupo in lstUsarioGrupo) //grupoSocial.ListaUsuarioGrupo)
                                    {
                                        if ((usrGrupo != null && (bool)(!usrGrupo.EsModerador)))
                                        {
                                            //El usuario es un alumno y se creara una notificación para el alumno e insertarla.
                                            //Crear notificación
                                            Notificacion notificacion = new Notificacion();
                                            notificacion.NotificacionID = Guid.NewGuid();
                                            notificacion.Emisor = publicacion.UsuarioSocial;
                                            notificacion.Receptor = usrGrupo.UsuarioSocial;
                                            notificacion.FechaRegistro = publicacion.FechaPublicacion;
                                            notificacion.Notificable = publicacion;
                                            notificacion.TipoNotificacion = ETipoNotificacion.PUBLICACION_DOCENTE;
                                            notificacion.EstatusNotificacion = EEstatusNotificacion.NUEVO;
                                            //Registrar notificación.
                                            notificacionCtrl.Insert(dctx, notificacion);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                //Crear notificacion
                                Notificacion notificacion = new Notificacion();
                                notificacion.NotificacionID = Guid.NewGuid();
                                notificacion.Emisor = publicacion.UsuarioSocial;
                                notificacion.Receptor = usuarioSocialMuro;
                                notificacion.FechaRegistro = publicacion.FechaPublicacion;
                                notificacion.Notificable = publicacion;
                                notificacion.TipoNotificacion = ETipoNotificacion.PUBLICACION;
                                notificacion.EstatusNotificacion = EEstatusNotificacion.NUEVO;
                                //Registrar notificación.
                                notificacionCtrl.Insert(dctx, notificacion);
                            }
                        }
                        else{
                            //NO ES MODERADOR
                            if (usuarioSocialMuro.UsuarioSocialID != publicacion.UsuarioSocial.UsuarioSocialID)
                            {
                            //Crear notificacion
                            Notificacion notificacion = new Notificacion();
                            notificacion.NotificacionID = Guid.NewGuid();
                            notificacion.Emisor = publicacion.UsuarioSocial;
                            notificacion.Receptor = usuarioSocialMuro;
                            notificacion.FechaRegistro = publicacion.FechaPublicacion;
                            notificacion.Notificable = publicacion;
                            notificacion.TipoNotificacion = ETipoNotificacion.PUBLICACION;
                            notificacion.EstatusNotificacion = EEstatusNotificacion.NUEVO;
                          
                            //Registrar notificacion.
                            notificacionCtrl.Insert(dctx, notificacion);
                             }
                        }
                }                      
            }
        }
        /// <summary>
        /// Actualiza de manera optimista un registro de PublicacionUpdHlp en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerÃ¡ acceso a la base de datos</param>
        /// <param name="publicacionUpdHlp">PublicacionUpdHlp que tiene los datos nuevos</param>
        /// <param name="anterior">PublicacionUpdHlp que tiene los datos anteriores</param>
        public void Update(IDataContext dctx, Publicacion publicacion, Publicacion previous)
        {
            PublicacionUpdHlp da = new PublicacionUpdHlp();
            da.Action(dctx, publicacion, previous);
        }
        /// <summary>
        /// Elimina un registro completo de publicacion
        /// </summary>
        /// <param name="dctx"></param>
        /// <param name="publicacion"></param>
        public void DeleteComplete(IDataContext dctx, Publicacion publicacion)
        {
            publicacion = LastDataRowToPublicacion(Retrieve(dctx, publicacion));
            publicacion = RetrieveComplete(dctx, publicacion);

            ComentarioCtrl comentarioCtrl = new ComentarioCtrl();
            foreach (Comentario comentario in publicacion.ComentariosPublicacion)
            {
                comentarioCtrl.DeleteComplete(dctx, comentario, publicacion);
            }

            PublicacionDelHlp da = new PublicacionDelHlp();
            da.Action(dctx, publicacion);

            RankingCtrl rankignCtrl = new RankingCtrl();
            rankignCtrl.DeleteComplete(dctx, publicacion.Ranking);

            //Consultar 
        }

        /// <summary>
        /// Desactiva un registro completo de publicación, evalúa si se requiere notificar al usuarioSocial
        /// que realizo la publicación 
        /// </summary>
        /// <param name="dctx"></param>
        /// <param name="publicacion">Publicación a eliminar</param>
        /// <param name="usuarioSocial">UsuarioSocial que desea eliminar la publicación</param>
        public void RemoveComplete(IDataContext dctx, Publicacion publicacion,UsuarioSocial usuarioSocial, List<AreaConocimiento> areasConocimiento, long universidadID, bool esAlumno = true, bool dueñopublicacion = true)
        {
            if(publicacion == null)
                throw  new SystemException("RemoveComplete:publicación no puede ser nulo");
            if(publicacion.PublicacionID == null)
                throw new SystemException("RemoveComplete:publicaciónID no puede ser nulo");
            if(usuarioSocial == null)
                throw new SystemException("RemoveComplete:usuarioSocial no puede ser nulo");
            if(usuarioSocial.UsuarioSocialID == null)
                throw new SystemException("RemoveComplete:UsuarioSocialID no puede ser nulo");

            DataSet ds = Retrieve(dctx, publicacion);
            if (ds.Tables["Publicacion"].Rows.Count == 1)
            {
                publicacion = LastDataRowToPublicacion(ds);
                publicacion = RetrieveComplete(dctx, publicacion);

                //UsuarioSocial que elimina
                SocialHubCtrl socialHubCtrl = new SocialHubCtrl();
                GrupoSocialCtrl grupoSocialCtrl = new GrupoSocialCtrl();
                SocialHub socialHub = null;
                UsuarioGrupo usuarioGrupo = null;
                UsuarioGrupo usuarioGrupoPublica = null;

                ds = socialHubCtrl.RetrieveSocialHubUsuario(dctx, new SocialHub { SocialProfileType = ESocialProfileType.USUARIOSOCIAL, SocialProfile = usuarioSocial});
                if (ds.Tables["SocialHub"].Rows.Count != 1)
                    throw new SystemException("RemoveComplete:SocialHub no puede ser nulo");

                socialHub = socialHubCtrl.LastDataRowToSocialHub(ds);
                if (socialHub == null)
                    throw new SystemException("RemoveComplete:SocialHub no puede ser nulo");


                //Consultar el Grupo Social de la publicación.
                List<IPrivacidad> lsgrupoSocial = publicacion.Privacidades;
                foreach (IPrivacidad privacidad in lsgrupoSocial)
                {
                    if((privacidad.GetType() != typeof(GrupoSocial)))
                        continue;

                    //Identificar el usuarioGrupo del usuario que elimina.
                    usuarioGrupo = grupoSocialCtrl.RetrieveFriend(dctx, (GrupoSocial)privacidad, usuarioSocial, areasConocimiento, null, universidadID, esAlumno);
                    //Identificar el usuarioGrupo del usuario que publica
                    usuarioGrupoPublica = grupoSocialCtrl.RetrieveFriend(dctx, (GrupoSocial)privacidad, publicacion.UsuarioSocial, areasConocimiento, null, universidadID, dueñopublicacion);

                    if (usuarioGrupo != null && usuarioGrupoPublica != null)
                        break;
                }
                //evaluar si el usuario que elimina tiene un grupo
                if (usuarioGrupo == null || usuarioGrupoPublica == null)
                    throw new SystemException("RemoveComplete:UsuarioGrupo no puede ser nulo");

                //evaluar si el usuario tiene permisos para desactivar
                if (publicacion.UsuarioSocial.UsuarioSocialID == usuarioSocial.UsuarioSocialID || (bool)usuarioGrupo.EsModerador
                    || publicacion.SocialHub.SocialHubID == socialHub.SocialHubID)
                {
                    Publicacion publicacionAnterior = (Publicacion)publicacion.Clone();
                    publicacion.Estatus = false;
                    Update(dctx, publicacion, publicacionAnterior);

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
                            notificacion.Receptor = publicacion.UsuarioSocial;
                            notificacion.FechaRegistro = DateTime.Now;
                            notificacion.Notificable = publicacion;
                            notificacion.TipoNotificacion = ETipoNotificacion.PUBLICACION_ELIMINADA;
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
        /// Crea un objeto de Publicacion a partir de los datos del Ãºltimo DataRow del DataSet.
        /// </summary>
        /// <param name="ds">El DataSet que contiene la informaciÃ³n de Publicacion</param>
        /// <returns>Un objeto de Publicacion creado a partir de los datos</returns>
        public Publicacion LastDataRowToPublicacion(DataSet ds)
        {
            if (!ds.Tables.Contains("Publicacion"))
                throw new Exception("LastDataRowToPublicacion: DataSet no tiene la tabla Publicacion");
            int index = ds.Tables["Publicacion"].Rows.Count;
            if (index < 1)
                throw new Exception("LastDataRowToPublicacion: El DataSet no tiene filas");
            return this.DataRowToPublicacion(ds.Tables["Publicacion"].Rows[index - 1]);
        }
        /// <summary>
        /// Crea un objeto de Publicacion a partir de los datos de un DataRow.
        /// </summary>
        /// <param name="row">El DataRow que contiene la informaciÃ³n de Publicacion</param>
        /// <returns>Un objeto de Publicacion creado a partir de los datos</returns>
        public Publicacion DataRowToPublicacion(DataRow row)
        {
            Publicacion publicacion = new Publicacion();
            publicacion.Ranking = new Ranking();
            publicacion.UsuarioSocial = new UsuarioSocial();
            publicacion.SocialHub = new SocialHub();
            publicacion.ComentariosPublicacion = new List<Comentario>();
            publicacion.Privacidades = new List<IPrivacidad>();
            if (row.IsNull("PublicacionID"))
                publicacion.PublicacionID = null;
            else
                publicacion.PublicacionID = (Guid)Convert.ChangeType(row["PublicacionID"], typeof(Guid));
            if (row.IsNull("Contenido"))
                publicacion.Contenido = null;
            else
                publicacion.Contenido = (string)Convert.ChangeType(row["Contenido"], typeof(string));
            if (row.IsNull("FechaPublicacion"))
                publicacion.FechaPublicacion = null;
            else
                publicacion.FechaPublicacion = (DateTime)Convert.ChangeType(row["FechaPublicacion"], typeof(DateTime));
            if (row.IsNull("Estatus"))
                publicacion.Estatus = null;
            else
                publicacion.Estatus = (bool)Convert.ChangeType(row["Estatus"], typeof(bool));
            if (row.IsNull("SocialHubID"))
                publicacion.SocialHub.SocialHubID = null;
            else
                publicacion.SocialHub.SocialHubID = (long)Convert.ChangeType(row["SocialHubID"], typeof(long));
            if (row.IsNull("UsuarioSocialID"))
                publicacion.UsuarioSocial.UsuarioSocialID = null;
            else
                publicacion.UsuarioSocial.UsuarioSocialID = (long)Convert.ChangeType(row["UsuarioSocialID"], typeof(long));
            if (row.IsNull("RankingID"))
                publicacion.Ranking.RankingID = null;
            else
                publicacion.Ranking.RankingID = (Guid)Convert.ChangeType(row["RankingID"], typeof(Guid));
            if (row.IsNull("TipoPublicacion"))
                publicacion.TipoPublicacion = null;
            else
                publicacion.TipoPublicacion = (ETipoPublicacion)Convert.ChangeType(row["TipoPublicacion"], typeof(short));
            switch (publicacion.TipoPublicacion)
            {
                case ETipoPublicacion.REACTIVO:
                    Reactivo reactivo = new Reactivo();
                    reactivo.ReactivoID = (Guid)Convert.ChangeType(row["AppSocialID"], typeof(Guid));
                    publicacion.AppSocial = reactivo;
                    break;
                case ETipoPublicacion.APPSOCIAL:
                    {
                        if (!row.IsNull("AppSocialID"))
                        {
                            publicacion.AppSocial = new Reactivo { ReactivoID = (Guid)Convert.ChangeType(row["AppSocialID"], typeof(Guid)) };
                        }
                        else if (!row.IsNull("ContenidoDigitalID"))
                        {
                            publicacion.AppSocial = new ContenidoDigital { ContenidoDigitalID = (long)Convert.ChangeType(row["ContenidoDigitalID"], typeof(long)) };
                        }
                        break;
                    }
            }
            
            return publicacion;
        }
    }
}
