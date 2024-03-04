using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.Base.DataAccess;
using POV.Web.DTO;
using GP.SocialEngine.BO;
using GP.SocialEngine.Service;
using GP.SocialEngine.DA;
using GP.SocialEngine.Utils;
using POV.Core.RedSocial.Implement;
using POV.Core.RedSocial.Interfaces;

namespace POV.Web.DTO.Services
{
    public class NotificacionDTOCtrl
    {
        private NotificacionCtrl notificacionCtrl;

        private IDataContext dctx;

        private IUserSession userSession;

        public NotificacionDTOCtrl()
        {
            dctx = new DataContext((new DataProviderFactory()).GetDataProvider("POV"));
            userSession = new UserSession();
            notificacionCtrl = new NotificacionCtrl();
        }

        public notificacionsummarydto GetTotalNotificaciones()
        
        {
            notificacionsummarydto dto = new notificacionsummarydto();
            dto.total = 0;
            dto.invitaciones = 0;
            dto.mensajes = 0;
            dto.social = 0;
            dto.reportesabuso = 0;

            UsuarioSocial usuarioSocial = userSession.CurrentUsuarioSocial;

            if (usuarioSocial != null) {
                DataSet dsNotificaciones = notificacionCtrl.Retrieve(dctx, new Notificacion { 
                    Receptor = usuarioSocial, 
                    EstatusNotificacion = EEstatusNotificacion.NUEVO });

                //Consultar las notificaciones por reporte abuso
                if (userSession.CurrentAlumno == null)
                {
                    userSession.CurrentAlumno = new CentroEducativo.BO.Alumno();
                    userSession.CurrentAlumno.AreasConocimiento = new List<CentroEducativo.BO.AreaConocimiento>();
                }

                long? universityID = 0;
                if (userSession.IsAlumno())
                    if (userSession.CurrentAlumno.Universidades.Count > 0)
                        universityID = (long)userSession.CurrentAlumno.Universidades[0].UniversidadID;
                
                if (userSession.IsDocente())
                    universityID = (long)userSession.CurrentUser.UniversidadId;

                DataSet dsRepoteAbuso = (new ReporteAbusoCtrl()).RetriveReportesAbusoINotificable(dctx, new ReporteAbuso { EstadoReporteAbuso = EEstadoReporteAbuso.PENDIENTE }, usuarioSocial, userSession.CurrentAlumno.AreasConocimiento, (long)universityID);
                if (dsRepoteAbuso!=null && dsRepoteAbuso.Tables["Notificacion"].Rows.Count > 0)
                    dsNotificaciones.Merge(dsRepoteAbuso);


                if (dsNotificaciones.Tables["Notificacion"].Rows.Count > 0)
                {
                    foreach (DataRow dr in dsNotificaciones.Tables["Notificacion"].Rows)
                    {
                        Notificacion notificacion = notificacionCtrl.DataRowToNotificacion(dr);

                        switch (notificacion.TipoNotificacion)
                        {
                            case ETipoNotificacion.PUBLICACION:
                                dto.social++;
                                break;
                            case ETipoNotificacion.COMENTARIO:
                                dto.social++;
                                break;
                            case ETipoNotificacion.INVITACION_ACEPTADA:
                                dto.social++;
                                break;
                            case ETipoNotificacion.INVITACION_RECIBIDA:
                                dto.invitaciones++;
                                break;
                            case ETipoNotificacion.RANKING_PUBLICACION:
                                dto.social++;
                                break;
                            case ETipoNotificacion.RANKING_COMENTARIO:
                                dto.social++;
                                break;
                            case ETipoNotificacion.MENSAJE:
                                dto.mensajes++;
                                break;
                            case ETipoNotificacion.RESPUESTA_MENSAJE:
                                dto.mensajes++;
                                break;
                            case ETipoNotificacion.PUBLICACION_DOCENTE:
                                dto.social++;
                                break;
                            case ETipoNotificacion.PUBLICACION_ELIMINADA:
                                dto.social++;
                                break;
                            case ETipoNotificacion.COMENTARIO_ELIMINADO:
                                dto.social++;
                                break;
                            case ETipoNotificacion.REPORTE_ABUSO:
                                dto.reportesabuso++;
                                break;
                        }
                        dto.total = dto.social + dto.mensajes;
                    }
                }
               
            }
            return dto;
        }

        public List<notificaciondto> GetNotificacionesSocial(notificacioninputdto dto)
        {
            try
            {
                List<notificaciondto> notificaciones = new List<notificaciondto>();

                UsuarioSocial usuarioSession = userSession.CurrentUsuarioSocial;

                if (usuarioSession != null && usuarioSession.UsuarioSocialID != null)
                {
                    NotificacionDARetHlp da = new NotificacionDARetHlp();

                    dto.sort = "FechaRegistro";
                    dto.order = "DESC";
                    dto.receptorid = usuarioSession.UsuarioSocialID;

                    List<ETipoNotificacion> tipos = new List<ETipoNotificacion>();
                    tipos.Add(ETipoNotificacion.INVITACION_ACEPTADA);
                    tipos.Add(ETipoNotificacion.COMENTARIO);
                    tipos.Add(ETipoNotificacion.MENSAJE);
                    tipos.Add(ETipoNotificacion.RESPUESTA_MENSAJE);
                    tipos.Add(ETipoNotificacion.PUBLICACION);
                    tipos.Add(ETipoNotificacion.RANKING_COMENTARIO);
                    tipos.Add(ETipoNotificacion.RANKING_PUBLICACION);
                    tipos.Add(ETipoNotificacion.PUBLICACION_DOCENTE);
                    tipos.Add(ETipoNotificacion.PUBLICACION_ELIMINADA);
                    tipos.Add(ETipoNotificacion.COMENTARIO_ELIMINADO);

                    List<EEstatusNotificacion> estados = new List<EEstatusNotificacion>();
                    estados.Add(EEstatusNotificacion.NUEVO);
                    estados.Add(EEstatusNotificacion.CONFIRMADO);

                    DataSet ds = da.Action(dctx, dto.pagesize, dto.currentpage, dto.sort, dto.order, dtoInputToParemeters(dto), tipos, estados);

                    foreach (DataRow dr in ds.Tables["Notificacion"].Rows)
                    {
                        Notificacion notificacion = notificacionCtrl.RetrieveComplete(dctx, new Notificacion { NotificacionID = notificacionCtrl.DataRowToNotificacion(dr).NotificacionID });
                        notificaciones.Add(NotificacionToDto(notificacion));

                        if (notificacion.TipoNotificacion == ETipoNotificacion.INVITACION_ACEPTADA && notificacion.EstatusNotificacion == EEstatusNotificacion.NUEVO)
                        {
                            Notificacion copy = (Notificacion)notificacion.Clone();
                            copy.EstatusNotificacion = EEstatusNotificacion.CONFIRMADO;
                            notificacionCtrl.Update(dctx, copy, notificacion);

                        }
                    }
                }
                return notificaciones;
            }
            catch (Exception ex)
            {
                POV.Logger.Service.LoggerHlp.Default.Error(this, ex);
                throw ex;
            }
        }

        public notificaciondto DeleteNotificacion(notificaciondto dto)
        {
            try
            {
                Guid notificacionID = new Guid();
                bool isValid = dto.notificacionid != null && Guid.TryParse(dto.notificacionid, out notificacionID);

                if (isValid)
                {
                    Notificacion notificacion = new Notificacion { NotificacionID = notificacionID};
                    notificacion = notificacionCtrl.LastDataRowToNotificacion(notificacionCtrl.Retrieve(dctx, notificacion));
                    Notificacion copy = (Notificacion)notificacion;
                    copy.EstatusNotificacion = EEstatusNotificacion.ELIMINADO;
                    notificacionCtrl.Update(dctx, copy, notificacion);
                    dto = new notificaciondto();
                    dto.notificacionid = notificacion.NotificacionID.ToString();
                }
                else
                {
                    dto.notificacionid = "-1";
                }

            }
            catch (Exception ex)
            {
                POV.Logger.Service.LoggerHlp.Default.Error(this, ex);
                dto.notificacionid = "-1";
            }
            return dto;
        }

        public void ConfirmNotificacion()
        {
            try
            {
                if (userSession.CurrentUsuarioSocial != null && userSession.CurrentUsuarioSocial.UsuarioSocialID != null)
                {
                   notificacionCtrl.UpdateEstadoNotificacionesRecibidas(dctx,EEstatusNotificacion.CONFIRMADO,EEstatusNotificacion.NUEVO, userSession.CurrentUsuarioSocial);
                }
              
            }
            catch (Exception ex)
            {
                POV.Logger.Service.LoggerHlp.Default.Error(this, ex);
            }
           
        }

        private Dictionary<string, string> dtoInputToParemeters(notificacioninputdto dto)
        {
            Dictionary<string, string> parametros = new Dictionary<string, string>();
            if (dto.emisorid != null && dto.emisorid > 0)
                parametros.Add("EmisorID", dto.emisorid.ToString());
            if (dto.receptorid != null && dto.receptorid > 0)
                parametros.Add("ReceptorID", dto.receptorid.ToString());
            return parametros;
        }

        private notificaciondto NotificacionToDto(Notificacion notificacion)
        {
            notificaciondto dto = new notificaciondto();
            GrupoSocialCtrl grupoSocialCtrl = new GrupoSocialCtrl();

            dto.textonotificable = "";
            if (notificacion.NotificacionID != null)
                dto.notificacionid = notificacion.NotificacionID.ToString();
            if (notificacion.Emisor != null && notificacion.Emisor.UsuarioSocialID != null)
            {
                dto.emisorid = notificacion.Emisor.UsuarioSocialID;
                dto.emisorname = notificacion.Emisor.ScreenName;
                long universidadID = 0;
                if (userSession.IsDocente())
                    universidadID = (long)userSession.CurrentUser.UniversidadId;
                else
                    universidadID = (long)userSession.CurrentAlumno.Universidades[0].UniversidadID;
                UsuarioGrupo usuarioGrupoEmisor = grupoSocialCtrl.RetrieveFriend(dctx, notificacion.Emisor, notificacion.Emisor, userSession.CurrentAlumno.AreasConocimiento, universidadID);
                if (usuarioGrupoEmisor != null)
                    dto.renderemisorlink = userSession.IsAlumno() || !(bool)usuarioGrupoEmisor.EsModerador;
                else
                    dto.renderemisorlink = false;
            }

            if (notificacion.FechaRegistro != null)
                dto.fecharegistro = new TimeAgoDateFormat().Format((DateTime)notificacion.FechaRegistro);

            if (notificacion.EstatusNotificacion != null)
                dto.estatus = notificacion.ToShortEstatusNotificacion;
            if (notificacion.TipoNotificacion != null)
            {
                dto.tiponotificacion = notificacion.ToShortTipoNotificacion;

                if (notificacion.Notificable != null)
                {
                    switch (notificacion.TipoNotificacion)
                    {
                        case ETipoNotificacion.PUBLICACION:
                            dto.notificableid = notificacion.Notificable.GUID.ToString();
                            dto.textonotificacion = notificacion.Notificable.TextoNotificacion;
                            dto.url = notificacion.Notificable.URLReferencia;
                            dto.urllabel = "apuntes";
                            dto.textonotificable = ((Publicacion)notificacion.Notificable).Contenido;
                            dto.textonotificable = dto.textonotificable.Length > 30 ? dto.textonotificable.Substring(0, 27) + "..." : dto.textonotificable;
                            break;
                        case ETipoNotificacion.COMENTARIO:
                            dto.notificableid = notificacion.Notificable.GUID.ToString();
                            dto.textonotificacion = notificacion.Notificable.TextoNotificacion;
                            dto.url = notificacion.Notificable.URLReferencia;
                            dto.urllabel = "publicación";                            
                            dto.textonotificable = ((Comentario)notificacion.Notificable).Cuerpo;
                            dto.textonotificable = dto.textonotificable.Length > 30 ? dto.textonotificable.Substring(0,27) + "..." : dto.textonotificable;
                            
                            break;
                        case ETipoNotificacion.INVITACION_ACEPTADA:
                            dto.notificableid = notificacion.Notificable.GUID.ToString();
                            dto.textonotificacion = notificacion.Notificable.TextoNotificacion;
                            dto.url = notificacion.Notificable.URLReferencia;
                            break;
                        case ETipoNotificacion.INVITACION_RECIBIDA:
                            dto.notificableid = notificacion.Notificable.GUID.ToString();
                            dto.textonotificacion = notificacion.Notificable.TextoNotificacion;
                            break;
                        case ETipoNotificacion.RANKING_PUBLICACION:
                            dto.notificableid = notificacion.Notificable.GUID.ToString();
                            dto.textonotificacion = notificacion.Notificable.TextoNotificacion;
                            dto.url = notificacion.Notificable.URLReferencia;
                            dto.urllabel = "publicación";
                            break;
                        case ETipoNotificacion.RANKING_COMENTARIO:
                            dto.notificableid = notificacion.Notificable.GUID.ToString();
                            dto.textonotificacion = notificacion.Notificable.TextoNotificacion;
                            dto.url = notificacion.Notificable.URLReferencia;
                            dto.urllabel = "comentario";
                            break;
                        case ETipoNotificacion.MENSAJE:
                            dto.notificableid = notificacion.Notificable.GUID.ToString();
                            dto.textonotificacion = notificacion.Notificable.TextoNotificacion;
                            dto.url = notificacion.Notificable.URLReferencia;
                            dto.urllabel = "mensaje";
                            dto.textonotificable = "";
                            break;
                        case ETipoNotificacion.RESPUESTA_MENSAJE:
                            dto.notificableid = notificacion.Notificable.GUID.ToString();
                            dto.textonotificacion = GP.SocialEngine.Properties.Resources.RespuestaMensajeNotificacion;
                            dto.url = notificacion.Notificable.URLReferencia;
                            dto.urllabel = " mensaje";
                            break;

                        case ETipoNotificacion.PUBLICACION_DOCENTE:
                            dto.notificableid = notificacion.Notificable.GUID.ToString();
                            dto.textonotificacion = GP.SocialEngine.Properties.Resources.PublicacionDocenteNotificacion;
                            dto.url = notificacion.Notificable.URLReferencia;
                            dto.urllabel = "publicación";
                            dto.textonotificable = ((Publicacion)notificacion.Notificable).Contenido;
                            dto.textonotificable = dto.textonotificable.Length > 30 ? dto.textonotificable.Substring(0, 27) + "..." : dto.textonotificable;

                            if (notificacion.Emisor != null && !string.IsNullOrEmpty(notificacion.Emisor.ScreenName))
                                  dto.emisorname = string.Format(" El Docente {0} ", dto.emisorname);

                            break;
                       case ETipoNotificacion.PUBLICACION_ELIMINADA:
                            dto.notificableid = notificacion.Notificable.GUID.ToString();
                            dto.textonotificacion = GP.SocialEngine.Properties.Resources.PublicacionEliminadaNotificacion;
                            dto.textonotificable = ((Publicacion)notificacion.Notificable).Contenido;
                            dto.textonotificable = dto.textonotificable.Length > 30 ? dto.textonotificable.Substring(0, 27) + "..." : dto.textonotificable;
                            if (notificacion.Emisor != null && !string.IsNullOrEmpty(notificacion.Emisor.ScreenName))
                                dto.emisorname = string.Format(" El Docente {0} ", dto.emisorname);

                            break;
                       case ETipoNotificacion.COMENTARIO_ELIMINADO:
                            dto.notificableid = notificacion.Notificable.GUID.ToString();
                            dto.textonotificacion = GP.SocialEngine.Properties.Resources.ComentarioEliminadoNotificacion;
                            dto.textonotificable = ((Comentario) notificacion.Notificable).Cuerpo;
                            dto.textonotificable = dto.textonotificable.Length > 30 ? dto.textonotificable.Substring(0, 27) + "..." : dto.textonotificable;
                            if (notificacion.Emisor != null && !string.IsNullOrEmpty(notificacion.Emisor.ScreenName))
                                dto.emisorname = string.Format(" El Docente {0} ", dto.emisorname);
                            break;
                    }
                }
            }
            return dto;
        }

        
    }
}
