using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Framework.Base.DataAccess;
using GP.SocialEngine.BO;
using GP.SocialEngine.Service;
using GP.SocialEngine.Utils;
using POV.Core.RedSocial.Implement;
using POV.Core.RedSocial.Interfaces;
using POV.Web.DTO;
namespace POV.Web.DTO.Services
{
   public class ReporteAbusoDTOCtrl
   {
       private IUserSession userSession;
       private IDataContext dctx;
       private ReporteAbusoCtrl reporteAbusoCtrl;
       private UsuarioSocialCtrl usuarioSocialCtrl;
       private PublicacionCtrl publicacionCtrl;
       private ComentarioCtrl comentarioCtrl;
       private GrupoSocialCtrl grupoSocialCtrl;
       private PublicacionDTOCtrl publicacionDtoCtrl;

       private ConfiguracionReporteAbuso configuracionReporte;

       public ConfiguracionReporteAbuso ConfiguracionReporte
       {
           get { return this.configuracionReporte; }
           set { this.configuracionReporte = value; }
       }
       public ReporteAbusoDTOCtrl()
       {
           userSession = new UserSession();
           dctx = new DataContext(new DataProviderFactory().GetDataProvider("POV"));
           reporteAbusoCtrl = new ReporteAbusoCtrl();
           usuarioSocialCtrl = new UsuarioSocialCtrl();
           publicacionCtrl = new PublicacionCtrl();
           comentarioCtrl = new ComentarioCtrl();
           grupoSocialCtrl = new GrupoSocialCtrl();
           publicacionDtoCtrl = new PublicacionDTOCtrl();
         
       }

       /// <summary>
       /// Obtiene una pagina reportes de abuso en estado pendiente del usuario actual(Docente).
       /// </summary>
       /// <param name="dto">Parámetros de consulta</param>
       /// <returns>Lista de reportes de abuso encontrados</returns>
       public List<reporteabusodto> GetReportesAbusoDocente(notificacioninputdto dto)
       {
           try
           {
               List<reporteabusodto> ls = new List<reporteabusodto>();

                   List<EEstadoReporteAbuso> estados = new List<EEstadoReporteAbuso> { EEstadoReporteAbuso.PENDIENTE };
                   List<ETipoContenido> tipo = new List<ETipoContenido> { ETipoContenido.PUBLICACION, ETipoContenido.COMENTARIO };
               long universidadID=0;
               if (userSession.IsDocente())
                   universidadID = (long)userSession.CurrentUser.UniversidadId;
               else universidadID = (long)userSession.CurrentAlumno.Universidades[0].UniversidadID;
                   DataSet ds = reporteAbusoCtrl.RetrievePageReportesAbusoDocente(dctx, dto.pagesize, dto.currentpage, new Dictionary<string, string>(), tipo, estados, userSession.CurrentUsuarioSocial, userSession.CurrentAlumno.AreasConocimiento, universidadID);

                   foreach (DataRow rowrpt in ds.Tables["ReporteAbuso"].Rows)
                   {
                       ReporteAbuso reporte = reporteAbusoCtrl.DataRowToReporteAbuso(rowrpt);
                       ls.Add(ReporteAbusoToDto(reporte));
                   }
               
               return ls;
           }
           catch (Exception ex)
           {
               
               POV.Logger.Service.LoggerHlp.Default.Error(this,ex);
               throw;
           }

       }

       /// <summary>
       /// Actualiza el estado de un reporte abuso a cancelado y reactiva el elemento reportado
       /// </summary>
       /// <param name="dto">Reporte abuso que se desea cancelar</param>
       public reporteabusodto DeleteReporteAbuso(reporteabusodto dto)
       {
           
           try
           {
               if ((dto.reportableid == null && dto.tiporeporte == null) && dto.reporteabusoid == null)
                   throw new Exception("Parámetros requeridos");

               ReporteAbuso anterior = DtoToReporteAbuso(dto);
               DataSet ds = reporteAbusoCtrl.Retrieve(dctx, anterior);
               int index = ds.Tables["ReporteAbuso"].Rows.Count;

               if (index >= 1)
               {
                   foreach (DataRow drReporte in ds.Tables[0].Rows)
                   {
                       anterior = reporteAbusoCtrl.DataRowToReporteAbuso(drReporte);

                       if (anterior != null)
                       {
                           ReporteAbuso actual = (ReporteAbuso)anterior.Clone();
                           actual.EstadoReporteAbuso = EEstadoReporteAbuso.CANCELADO;
                           actual.FechaFinReporte = DateTime.Now;
                           //Reactivar el elemento reportado

                           object firm = new object();
                           try
                           {
                               dctx.OpenConnection(firm);

                               dctx.BeginTransaction(firm);
                               reporteAbusoCtrl.Update(dctx, actual, anterior);
                               //consultar el elemento que fue reportado y reactivarlo
                               switch (actual.TipoContenido)
                               {
                                   case ETipoContenido.PUBLICACION:
                                       Publicacion anteriorPub = publicacionCtrl.LastDataRowToPublicacion(publicacionCtrl.Retrieve(dctx, new Publicacion { PublicacionID = actual.Reportable.GUID }));
                                       if (!(bool)anteriorPub.Estatus)
                                       {
                                           Publicacion actualPub = (Publicacion)anteriorPub.Clone(); actualPub.Estatus = true;
                                           publicacionCtrl.Update(dctx, actualPub, anteriorPub);

                                       }
                                       dctx.CommitTransaction(firm);
                                       dto.success = true;
                                       break;
                                   case ETipoContenido.COMENTARIO:
                                       Publicacion padre = publicacionCtrl.RetrievePublicacionPadre(dctx, new Comentario { ComentarioID = actual.Reportable.GUID });
                                       Comentario anteriorCom = comentarioCtrl.LastDataRowToComentario(comentarioCtrl.Retrieve(dctx, new Comentario { ComentarioID = actual.Reportable.GUID }, padre));
                                       if (!(bool)anteriorCom.Estatus)
                                       {
                                           Comentario actualCom = (Comentario)anteriorCom.Clone(); actualCom.Estatus = true;
                                           comentarioCtrl.Update(dctx, actualCom, anteriorCom);

                                       }
                                       dctx.CommitTransaction(firm);
                                       dto.success = true;
                                       break;
                               }
                               
                           }
                           catch (Exception ex)
                           {
                               dctx.RollbackTransaction(firm);
                               if (dctx.ConnectionState == ConnectionState.Open)
                                   dctx.CloseConnection(firm);
                               throw;
                           }
                           finally
                           {
                               if (dctx.ConnectionState == ConnectionState.Open)
                                   dctx.CloseConnection(firm);
                           }
                       }
                   }
               }
               return dto;

           }
           catch (Exception ex)
           {
               POV.Logger.Service.LoggerHlp.Default.Error(this, ex);
               dto.success = false;
               dto.error = "ocurrió un error al procesar su solicitud";
               return dto;
           }
          
       }

       /// <summary>
       /// Obtiene la publicación a la que pertenece el reporte de abuso
       /// </summary>
       /// <param name="dto">reporte abuso a consultar</param>
       /// <returns>Publicación a la que pertenece el reporte de abuso</returns>
       public publicaciondto GetPublicacionReporteAbuso(reporteabusodto dto)
       {
           publicaciondto dtopub = new publicaciondto();
           try
           {
               ReporteAbuso reporteAbuso = DtoToReporteAbuso(dto);
   
               if (reporteAbuso.ReporteAbusoID != null)
               {
                   DataSet ds = reporteAbusoCtrl.Retrieve(dctx, reporteAbuso);
                   int index = ds.Tables["ReporteAbuso"].Rows.Count;
                   if (index != 1)
                       return null;

                   reporteAbuso = reporteAbusoCtrl.LastDataRowToReporteAbuso(ds);

                   switch (reporteAbuso.TipoContenido)
                   {
                       case ETipoContenido.PUBLICACION:
                           ds = publicacionCtrl.Retrieve(dctx, new Publicacion { PublicacionID = reporteAbuso.Reportable.GUID });
                           Publicacion pub = publicacionCtrl.LastDataRowToPublicacion(ds);
                           dtopub = publicacionDtoCtrl.GetPublicacionReportada(new publicaciondto { publicacionid = pub.PublicacionID.ToString() });
                           dtopub.renderdelete = true;
                           break;
                       case ETipoContenido.COMENTARIO:
                           Publicacion padre = publicacionCtrl.RetrievePublicacionPadre(dctx, new Comentario { ComentarioID = reporteAbuso.Reportable.GUID });
                           dtopub = publicacionDtoCtrl.GetPublicacionReportada(new publicaciondto { publicacionid = padre.PublicacionID.ToString() });
                           
                           break;
                   }

                   if (dtopub.publicacionid == reporteAbuso.Reportable.GUID.ToString())
                   {
                       dtopub.renderreporteabuso = true;
                       dtopub.comentarios.ForEach(cm => cm.renderreporteabuso = false);
                   }
                   else
                   {
                       dtopub.renderreporteabuso = false;
                       foreach (comentariodto comt in dtopub.comentarios)
                       {
                           if (comt.comentarioid == reporteAbuso.Reportable.GUID.ToString())
                               comt.renderreporteabuso = true;
                           else
                               comt.renderreporteabuso = false;
                       }
                   }
                       

               }
               dtopub.success = true;
               return dtopub;
           }
           catch (Exception ex)
           {
               Logger.Service.LoggerHlp.Default.Error(this,ex);
               dtopub.success = false;
               dtopub.error = "ocurrió un error al procesar su solicitud";
               return dtopub;
           }
       }

       /// <summary>
       /// Actualiza el estado de un reporte abuso a Confirmado verifica que el elemento reportado fue desactivado
       /// </summary>
       /// <param name="dto">Reporte abuso que desea confirmar</param>
       public reporteabusodto ConfirmReporteAbuso(reporteabusodto dto)
       {
           try
           {
               if((dto.reportableid==null && dto.tiporeporte==null) && dto.reporteabusoid==null)
                   throw new Exception("Parámetros requeridos");

               ReporteAbuso anterior = DtoToReporteAbuso(dto);
               DataSet ds = reporteAbusoCtrl.Retrieve(dctx, anterior);
               int index = ds.Tables["ReporteAbuso"].Rows.Count;

               if (index >= 1)
               {
                   foreach (DataRow dr in ds.Tables["ReporteAbuso"].Rows)
                   {
                       anterior = reporteAbusoCtrl.DataRowToReporteAbuso(dr);

                       if (anterior != null)
                       {
                           ReporteAbuso actual = (ReporteAbuso)anterior.Clone();
                           actual.EstadoReporteAbuso = EEstadoReporteAbuso.CONFIRMADO;
                           actual.FechaFinReporte = DateTime.Now;
                           //Reactivar el elemento reportado

                           object firm = new object();
                           try
                           {

                               dctx.OpenConnection(firm);

                               dctx.BeginTransaction(firm);
                               reporteAbusoCtrl.Update(dctx, actual, anterior);
                               //consultar el elemento que fue reportado y reactivarlo
                               switch (actual.TipoContenido)
                               {
                                   case ETipoContenido.PUBLICACION:
                                       Publicacion anteriorPub = publicacionCtrl.LastDataRowToPublicacion(publicacionCtrl.Retrieve(dctx, new Publicacion { PublicacionID = actual.Reportable.GUID }));
                                       if ((bool)anteriorPub.Estatus)
                                       {
                                           Publicacion actualPub = (Publicacion)anteriorPub.Clone(); actualPub.Estatus = false;
                                           publicacionCtrl.Update(dctx, actualPub, anteriorPub);
                                       }
                                       dctx.CommitTransaction(firm);
                                       break;
                                   case ETipoContenido.COMENTARIO:
                                       Publicacion padre = publicacionCtrl.RetrievePublicacionPadre(dctx, new Comentario { ComentarioID = actual.Reportable.GUID });
                                       Comentario anteriorCom = comentarioCtrl.LastDataRowToComentario(comentarioCtrl.Retrieve(dctx, new Comentario { ComentarioID = actual.Reportable.GUID }, padre));
                                       if ((bool)anteriorCom.Estatus)
                                       {
                                           Comentario actualCom = (Comentario)anteriorCom.Clone(); actualCom.Estatus = false;
                                           comentarioCtrl.Update(dctx, actualCom, anteriorCom);
                                       }
                                       dctx.CommitTransaction(firm);
                                       break;
                               }
                               dto.success = true;
                               return dto;
                           }
                           catch (Exception ex)
                           {
                               dctx.RollbackTransaction(firm);
                               if (dctx.ConnectionState == ConnectionState.Open)
                                   dctx.CloseConnection(firm);

                               throw;
                           }
                           finally
                           {
                               if (dctx.ConnectionState == ConnectionState.Open)
                                   dctx.CloseConnection(firm);
                           }
                       }
                   }
               }

               return dto;
           }
           catch (Exception ex)
           {
               POV.Logger.Service.LoggerHlp.Default.Error(this, ex);
               dto.success = false;
               dto.error = "ocurrió un error al procesar su solicitud";
               return dto;
           }
          
       }

       /// <summary>
       /// Registra un reporte de abuso
       /// </summary>
       /// <param name="dto"></param>
       public reporteabusodto InsertReporteAbuso(reporteabusodto dto)
       {

           try
           {
               ReporteAbuso reporte = DtoToReporteAbuso(dto);

               object firm = new object();

               try
               {
                   dctx.OpenConnection(firm);
                   dctx.BeginTransaction(firm);

                  
                   //Desactivar el elemento reportado
                   switch (reporte.TipoContenido)
                   {
                           case ETipoContenido.PUBLICACION:
                           Publicacion anterior = publicacionCtrl.LastDataRowToPublicacion(publicacionCtrl.Retrieve(dctx,  new Publicacion  {PublicacionID = reporte.Reportable.GUID}));
                           Publicacion publicacion = (Publicacion)anterior.Clone(); publicacion.Estatus=false;
                           publicacionCtrl.Update(dctx,publicacion,anterior);

                           reporte.Reportable = publicacion;
                           reporte.Reportado = publicacion.UsuarioSocial;
                           reporte.Reportante = userSession.CurrentUsuarioSocial;
                           reporte.TipoContenido = ETipoContenido.PUBLICACION;
                           reporte.GrupoSocial = userSession.CurrentGrupoSocial;
                           break;
                           case ETipoContenido.COMENTARIO:
                           Publicacion padre = publicacionCtrl.RetrievePublicacionPadre(dctx, new Comentario { ComentarioID = reporte.Reportable.GUID });
                           Comentario anteriorCom = comentarioCtrl.LastDataRowToComentario(comentarioCtrl.Retrieve(dctx, new Comentario { ComentarioID = reporte.Reportable.GUID }, padre));
                           Comentario comentario = (Comentario)anteriorCom.Clone(); comentario.Estatus=false;
                           comentarioCtrl.Update(dctx,comentario,anteriorCom);

                           reporte.Reportable = comentario;
                           reporte.Reportado = comentario.UsuarioSocial;
                           reporte.Reportante = userSession.CurrentUsuarioSocial;
                           reporte.TipoContenido = ETipoContenido.COMENTARIO;
                           reporte.GrupoSocial = userSession.CurrentGrupoSocial;
                           break;
                   }

                   //Insertar el reporte de abuso
                   //Campos requeridos
                   reporte.GUID = Guid.NewGuid();
                   reporte.FechaReporte = DateTime.Now;
                   reporte.EstadoReporteAbuso = EEstadoReporteAbuso.PENDIENTE;
                 
                   reporteAbusoCtrl.Insert(dctx, reporte);
                   DataSet ds = reporteAbusoCtrl.Retrieve(dctx, reporte);
                   int index = ds.Tables["ReporteAbuso"].Rows.Count;
                   if (index != 1)
                       throw new Exception("Ocurrió un error mientras se registraba el reporte de abuso");
                   reporte = reporteAbusoCtrl.LastDataRowToReporteAbuso(ds);

                   dctx.CommitTransaction(firm);
                   dto.success = true;
                   return dto;
               }
               catch (Exception ex)
               {
                   dctx.RollbackTransaction(firm);
                   if(dctx.ConnectionState == ConnectionState.Open)
                       dctx.CloseConnection(firm);

                   throw;
               }
               finally
               {
                   if(dctx.ConnectionState==ConnectionState.Open)
                       dctx.CloseConnection(firm);
               }
           }
           catch (Exception ex)
           {
               POV.Logger.Service.LoggerHlp.Default.Error(this, ex);

               dto.success = false;
               dto.error = "ocurrió un error al procesar su solicitud";

               return dto;
           }
           
       }


       public reporteabusodto validateInsertReporteAbuso()
       {
           //validar si el usuario puede realizar un reporte
           reporteabusodto dto = new reporteabusodto();
           try
           {
              if(configuracionReporte==null)
                  configuracionReporte = new ConfiguracionReporteAbuso(); //Inicializa con los valores default

               DataSet dsReportesAbuso = reporteAbusoCtrl.RetrieveLastReportesAbuso(dctx, new ReporteAbuso { Reportante = new UsuarioSocial { UsuarioSocialID = userSession.CurrentUsuarioSocial.UsuarioSocialID } }, configuracionReporte);
               if (dsReportesAbuso.Tables[0].Rows.Count >= configuracionReporte.MaximoReportes)
               {
                   dto.reportanteid = userSession.CurrentUsuarioSocial.UsuarioSocialID;
                   dto.reportantenombre = userSession.CurrentUsuarioSocial.ScreenName;
                   dto.success = false;

                   DateTime fechaLimite = DateTime.Now.AddMinutes((int)configuracionReporte.TiempoCancelacionMinutos);

                   dto.error = string.Format("El usuario {0} alcanzó su límite de {1} reportes de abuso.{2} Intentarlo nuevamente aproximadamente ({3}) ", dto.reportantenombre, configuracionReporte.MaximoReportes, System.Environment.NewLine, (new TimeBeforeDateFormat().Format(fechaLimite)));
               }
               else
               {
                   dto.success = true;
               }
               return dto;
           }
           catch (Exception ex)
           {
               Logger.Service.LoggerHlp.Default.Error(this,ex);
               dto.success = false;
               dto.error = "ocurrió un error al procesar su solicitud";
               return dto;
           }
          
       }
       
       private reporteabusodto ReporteAbusoToDto(ReporteAbuso reporteAbuso)
       {
           reporteabusodto dto = new reporteabusodto();
           try
           {
               if (reporteAbuso.ReporteAbusoID != null)
                   dto.reporteabusoid = reporteAbuso.ReporteAbusoID.ToString();

               if (reporteAbuso.Reportante != null && reporteAbuso.Reportante.UsuarioSocialID != null)
                   dto.reportanteid = reporteAbuso.Reportante.UsuarioSocialID;

               if (reporteAbuso.FechaReporte != null)
               {
                   dto.fechainicio = reporteAbuso.FechaReporte;
                   dto.fechainicioformated = new TimeAgoDateFormat().Format((DateTime)reporteAbuso.FechaReporte);
               }
               if (reporteAbuso.FechaFinReporte != null)
               {
                   dto.fechafin = reporteAbuso.FechaFinReporte;
                   dto.fechafinformated = new TimeAgoDateFormat().Format((DateTime)reporteAbuso.FechaFinReporte);
               }

               if (reporteAbuso.Reportado != null && reporteAbuso.Reportado.UsuarioSocialID != null)
               {
                   dto.reportadoid = reporteAbuso.Reportado.UsuarioSocialID;
                   UsuarioSocial usuarioreportado = usuarioSocialCtrl.LastDataRowToUsuarioSocial(usuarioSocialCtrl.Retrieve(dctx, new UsuarioSocial { UsuarioSocialID = reporteAbuso.Reportado.UsuarioSocialID }));
                   reporteAbuso.Reportado = usuarioreportado;
                   dto.reportadonombre = reporteAbuso.Reportado.ScreenName;

                   long universidadID = 0;
                   if (userSession.IsDocente())
                       universidadID = (long)userSession.CurrentUser.UniversidadId;
                   else universidadID = (long)userSession.CurrentAlumno.Universidades[0].UniversidadID;
                   
                   //si el alumno reportado pertenece al grupo del usuario
                   UsuarioGrupo usuarioGrupoEmisor = grupoSocialCtrl.RetrieveFriend(dctx, new UsuarioSocial { UsuarioSocialID = userSession.CurrentUsuarioSocial.UsuarioSocialID }, reporteAbuso.Reportado, userSession.CurrentAlumno.AreasConocimiento, universidadID);
                   dto.rendereportadolink = usuarioGrupoEmisor != null;
               }
               if (reporteAbuso.TipoContenido != null)
               {
                   switch (reporteAbuso.TipoContenido)
                   {
                       case ETipoContenido.PUBLICACION:
                           if (reporteAbuso.Reportable != null && reporteAbuso.Reportable.GUID != null)
                           {
                               dto.tiporeporte = (short)reporteAbuso.TipoContenido;
                               dto.reportableid = reporteAbuso.Reportable.GUID.ToString();
                               dto.url = reporteAbuso.URLReferencia;
                               dto.textonotificacion = reporteAbuso.TextoNotificacion;
                               Publicacion publicacion = publicacionCtrl.LastDataRowToPublicacion(publicacionCtrl.Retrieve(dctx, new Publicacion { PublicacionID = reporteAbuso.Reportable.GUID }));
                               dto.contenido = publicacion.Contenido.Length > 50 ? publicacion.Contenido.Substring(0, 50) : publicacion.Contenido;
                           }
                           break;
                       case ETipoContenido.COMENTARIO:
                           if (reporteAbuso.Reportable != null)
                           {
                               dto.tiporeporte = (short)reporteAbuso.TipoContenido;
                               dto.reportableid = reporteAbuso.Reportable.GUID.ToString();
                               dto.url = reporteAbuso.URLReferencia;
                               dto.textonotificacion = reporteAbuso.TextoNotificacion;
                               Publicacion padre = publicacionCtrl.RetrievePublicacionPadre(dctx, new Comentario { ComentarioID = reporteAbuso.Reportable.GUID });
                               Comentario comentario = comentarioCtrl.LastDataRowToComentario(comentarioCtrl.Retrieve(dctx, new Comentario { ComentarioID = reporteAbuso.Reportable.GUID }, padre));
                               dto.contenido = comentario.Cuerpo.Length > 50 ? comentario.Cuerpo.Substring(0, 50) : comentario.Cuerpo;

                           }
                           break;
                   }
               }
                return dto;
           }
           catch (Exception ex)
           {
               POV.Logger.Service.LoggerHlp.Default.Error(this,ex);

               throw;
           }
        
          
       }

       private ReporteAbuso DtoToReporteAbuso(reporteabusodto dto)
       {
           ReporteAbuso reporte = new ReporteAbuso();

           if (dto.reporteabusoid != null)
               reporte.ReporteAbusoID = Guid.Parse(dto.reporteabusoid);

           if (dto.reportanteid != null)
               reporte.Reportante = new UsuarioSocial {UsuarioSocialID = dto.reportanteid};

           if (dto.reportadoid != null)
               reporte.Reportado = new UsuarioSocial {UsuarioSocialID = dto.reportadoid};

           if (dto.estatusreporte != null)
               reporte.EstadoReporteAbuso = (EEstadoReporteAbuso) dto.estatusreporte;

           if (dto.tiporeporte != null)
           {
               reporte.TipoContenido = (ETipoContenido)dto.tiporeporte;
               if (dto.reportableid != null)
               {
                   switch (reporte.TipoContenido)
                   {
                       case ETipoContenido.PUBLICACION:
                           reporte.Reportable = new Publicacion {PublicacionID = Guid.Parse(dto.reportableid)};
                           break;
                        case ETipoContenido.COMENTARIO:
                           reporte.Reportable = new Comentario {ComentarioID = Guid.Parse(dto.reportableid)};
                           break;
                   }
               }
           }
           if (dto.fechafin != null)
               reporte.FechaFinReporte = dto.fechafin;
           if (dto.fechainicio != null)
               reporte.FechaReporte = dto.fechainicio;

           return reporte;

       }

      
   }
}
