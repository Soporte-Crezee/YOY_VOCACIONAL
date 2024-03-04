using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Framework.Base.DataAccess;

using Framework.Base.Exceptions;
using POV.Core.RedSocial.Interfaces;
using POV.Core.RedSocial.Implement;
using POV.CentroEducativo.BO;
using POV.CentroEducativo.Service;
using GP.SocialEngine.BO;
using GP.SocialEngine.Service;
using POV.Web.PortalSocial.AppCode;
using POV.Seguridad.BO;
using POV.Logger.Service;

namespace POV.Web.PortalSocial.Social
{
    public partial class VerPublicacion : System.Web.UI.Page
    {
        private IUserSession userSession;
        private IAccountService accountService;
        private IRedirector redirector;
        private readonly IDataContext dctx = ConnectionHelper.Default.Connection;

        private NotificacionCtrl notificacionCtrl;
        private PublicacionCtrl publicacionCtrl;
        private ComentarioCtrl comentarioCtrl;
        private MensajeCtrl mensajeCtrl;
        private UsuarioSocialRankingCtrl usuarioSocialRankingCtrl;

        public VerPublicacion()
        {
            accountService = new AccountService();
            userSession = new UserSession();
            redirector = new Redirector();
            publicacionCtrl = new PublicacionCtrl();
            comentarioCtrl = new ComentarioCtrl();
            usuarioSocialRankingCtrl = new UsuarioSocialRankingCtrl();
            mensajeCtrl = new MensajeCtrl();
            notificacionCtrl = new NotificacionCtrl();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (userSession.IsLogin())
                {  
                    if (userSession.CurrentUsuarioSocial != null)
                    {
                        if (!IsPostBack)
                        {
                            Guid notificacionID;
                            hdnDefaultUrl.Value = UrlHelper.GetDefaultURL();

                            string notificacion_id = Request.QueryString["n"];
                            notificacion_id = string.IsNullOrEmpty(notificacion_id) ? "" : notificacion_id;
                            bool parseSuccess = Guid.TryParse(notificacion_id, out notificacionID);
                            bool delete = false;
                            if (parseSuccess)
                            {
                                hdnTipoPublicacionSuscripcionReactivo.Value = ((short)ETipoPublicacion.REACTIVO).ToString();
                                hdnTipoPublicacionTexto.Value = ((short)ETipoPublicacion.TEXTO).ToString();
                                hdnSessionUsuarioSocialID.Value = userSession.CurrentUsuarioSocial.UsuarioSocialID.ToString();
                                try
                                {
                                    Notificacion notificacion = notificacionCtrl.RetrieveComplete(dctx, new Notificacion { NotificacionID = notificacionID });
                                    
                                    bool update = false;
                                    switch (notificacion.TipoNotificacion)
                                    {
                                        case ETipoNotificacion.PUBLICACION:
                                           
                                            Publicacion publicacionTmp = (Publicacion) notificacion.Notificable;
                                            publicacionTmp.Estatus = true;
                                            Publicacion publicacion = publicacionCtrl.RetrieveComplete(dctx,publicacionTmp);
                                            if (publicacion.Estatus == false){  delete = true; break;}

                                            hdnPublicacionID.Value = notificacion.Notificable.GUID.ToString();
                                            printPublicacionHeader(publicacion,ETipoNotificacion.PUBLICACION);
                                        
                                            break;
                                        case ETipoNotificacion.COMENTARIO:                              
                                            Publicacion padre = publicacionCtrl.RetrievePublicacionPadre(dctx, (Comentario)notificacion.Notificable);
                                            if (padre.Estatus == false) { delete = true; break; }
                                            hdnPublicacionID.Value = padre.PublicacionID.ToString();
                                                printPublicacionHeader(padre,ETipoNotificacion.PUBLICACION);
                                            break;
                                        case ETipoNotificacion.RANKING_PUBLICACION:
                                            Publicacion publicacionTmpRanking = new Publicacion { Ranking = new Ranking { RankingID = notificacion.Notificable.GUID },Estatus = true};
                                            Publicacion publicacionRanking = publicacionCtrl.LastDataRowToPublicacion(publicacionCtrl.Retrieve(dctx,publicacionTmpRanking));
                                            if (publicacionRanking == null) { delete = true; break; }
                                            hdnPublicacionID.Value = publicacionRanking.PublicacionID.ToString();
                                                printPublicacionHeader(publicacionRanking,ETipoNotificacion.PUBLICACION);
                                            break;
                                        case ETipoNotificacion.RANKING_COMENTARIO:
                                            Comentario comentarioRanking = comentarioCtrl.LastDataRowToComentario(comentarioCtrl.Retrieve(dctx, new Comentario { Ranking = new Ranking { RankingID = notificacion.Notificable.GUID } }, new Publicacion()));
                                            Publicacion publicacionComentarioRanking = publicacionCtrl.RetrievePublicacionPadre(dctx, comentarioRanking);
                                            if (publicacionComentarioRanking.Estatus == false) { delete = true; break; }
                                            hdnPublicacionID.Value = publicacionComentarioRanking.PublicacionID.ToString();
                                                printPublicacionHeader(publicacionComentarioRanking,ETipoNotificacion.PUBLICACION);
                                            break;
                                         case ETipoNotificacion.PUBLICACION_DOCENTE:
                                            Publicacion publicacionDocente = publicacionCtrl.LastDataRowToPublicacion(publicacionCtrl.Retrieve(dctx, new Publicacion { PublicacionID = notificacion.Notificable.GUID }));
                                            if (publicacionDocente.Estatus == false) { delete = true; break; }
                                            hdnPublicacionID.Value = publicacionDocente.PublicacionID.ToString();
                                                printPublicacionHeader(publicacionDocente,ETipoNotificacion.PUBLICACION_DOCENTE);
                                            break;
                                        default:
                                            update = false;
                                            redirector.GoToHomePage(true);
                                            break;
                                    }

                                    if (update)
                                    {
                                        Notificacion copy = (Notificacion)notificacion.Clone();
                                        copy.EstatusNotificacion = EEstatusNotificacion.CONFIRMADO;
                                        notificacionCtrl.Update(dctx, copy, notificacion);
                                    }
                                    if (delete)
                                    {
                                        Notificacion copy = (Notificacion) notificacion.Clone();
                                        copy.EstatusNotificacion=EEstatusNotificacion.ELIMINADO;
                                        notificacionCtrl.Update(dctx,copy,notificacion);
                                    }

                                    if (!userSession.IsAlumno())
                                    {
                                        hdnFuente.Value = "D";
                                        pnlSeleccionContenido.Visible = true;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    redirector.GoToHomePage(true);

                                }
                            }
                            else
                            {
                                redirector.GoToHomePage(true);
                            }

                        }
                    }
                    else
                    {
                        redirector.GoToHomePage(true);
                    }
                }
                else
                {
                    redirector.GoToLoginPage(true);
                }

            }
            catch (Exception ex)
            {
                LoggerHlp.Default.Error(this, ex);
               redirector.GoToHomePage(true);
            }

           
            
        }


        /// <summary>
        /// Consulta y despliega los datos de donde se realizo la publicación para las  notificaciones.
        /// </summary>
        /// <param name="publicacion">publicación</param>
        private void printPublicacionHeader(Publicacion publicacion,ETipoNotificacion tipoNotificacion)
        {
            if(publicacion==null)
                return;
            if(publicacion.SocialHub == null)
                return;

            lblTipo.Text = "Publicación ";
            if (userSession != null)
            {
                //obtener los datos del usuario propietario del muro donde se publico
                SocialHubCtrl socialHubCtrl = new SocialHubCtrl();
                GrupoSocialCtrl grupoSocialCtrl = new GrupoSocialCtrl();
                UsuarioGrupoCtrl usuarioGrupoCtrl = new UsuarioGrupoCtrl();
                PublicacionCtrl publicacionCtrl = new PublicacionCtrl();

                publicacion = publicacionCtrl.RetrieveComplete(dctx,
                                                               new Publicacion
                                                                   {PublicacionID = publicacion.PublicacionID});

                SocialHub socialHub = socialHubCtrl.RetrieveSocialHubUsuarioComplete(dctx, publicacion.SocialHub);

                GrupoSocial grupoSocial = (GrupoSocial)publicacion.Privacidades[0];

                long universidadID = 0;
                if (userSession.IsDocente())
                    universidadID = (long)userSession.CurrentUser.UniversidadId;
                else
                    universidadID = (long)userSession.CurrentAlumno.Universidades[0].UniversidadID;

                UsuarioGrupo usuarioGrupo = grupoSocialCtrl.RetrieveFriend(dctx,grupoSocial, (UsuarioSocial)socialHub.SocialProfile, userSession.CurrentAlumno.AreasConocimiento, null, universidadID);
                usuarioGrupo = usuarioGrupoCtrl.RetrieveComplete(dctx, usuarioGrupo, userSession.CurrentAlumno.AreasConocimiento, universidadID);

                lblNombreMuroPublicacion.Text = string.Format(" en los apuntes del {0} : {1}",
                                                              (bool) usuarioGrupo.EsModerador ? "orientador" : "estudiante",
                                                              usuarioGrupo.UsuarioSocial.ScreenName);
                 lblNombreGrupo.Text = string.Format(" Nombre del grupo : {0}",grupoSocial.Nombre);

            }
            else
            {
                redirector.GoToHomePage(true);
            }
        }

  
    }
}