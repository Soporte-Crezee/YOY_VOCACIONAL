using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using GP.SocialEngine.BO;
using GP.SocialEngine.DAO;

namespace GP.SocialEngine.Service { 
   /// <summary>
   /// Controlador del objeto Notificacion
   /// </summary>
   public class NotificacionCtrl { 
      /// <summary>
      /// Consulta registros de NotificacionRetHlp en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="notificacionRetHlp">NotificacionRetHlp que provee el criterio de selección para realizar la consulta</param>
      /// <returns>El DataSet que contiene la información de NotificacionRetHlp generada por la consulta</returns>
      public DataSet Retrieve(IDataContext dctx, Notificacion notificacion){
         NotificacionRetHlp da = new NotificacionRetHlp();
         DataSet ds = da.Action(dctx, notificacion);
         return ds;
      }

      public Notificacion RetrieveComplete(IDataContext dctx, Notificacion notificacion)
      {
          notificacion = LastDataRowToNotificacion(Retrieve(dctx, notificacion));

          UsuarioSocialCtrl usuarioSocialCtrl = new UsuarioSocialCtrl();

          notificacion.Emisor = usuarioSocialCtrl.LastDataRowToUsuarioSocial(usuarioSocialCtrl.Retrieve(dctx, notificacion.Emisor));
          notificacion.Receptor = usuarioSocialCtrl.LastDataRowToUsuarioSocial(usuarioSocialCtrl.Retrieve(dctx, notificacion.Emisor));
          MensajeCtrl mensajeCtrl;
          PublicacionCtrl publicacionCtrl;
          ComentarioCtrl comentarioCtrl;
          switch (notificacion.TipoNotificacion)
          {
              case ETipoNotificacion.PUBLICACION:
                  publicacionCtrl = new PublicacionCtrl();
                  notificacion.Notificable = publicacionCtrl.LastDataRowToPublicacion(publicacionCtrl.Retrieve(dctx, new Publicacion { PublicacionID = notificacion.Notificable.GUID }));
                  break;
              case ETipoNotificacion.COMENTARIO:
                  comentarioCtrl = new ComentarioCtrl();
                  notificacion.Notificable = comentarioCtrl.LastDataRowToComentario(comentarioCtrl.Retrieve(dctx, new Comentario { ComentarioID = notificacion.Notificable.GUID }, new Publicacion()));
                  break;
              case ETipoNotificacion.INVITACION_ACEPTADA:
                  InvitacionCtrl invitacionCtrl = new InvitacionCtrl();
                  notificacion.Notificable = invitacionCtrl.LastDataRowToInvitacion(invitacionCtrl.Retrieve(dctx, new Invitacion { InvitacionID = notificacion.Notificable.GUID }));
                  break;
              case ETipoNotificacion.INVITACION_RECIBIDA:
                  InvitacionCtrl invitacionCtrl1 = new InvitacionCtrl();
                  notificacion.Notificable = invitacionCtrl1.LastDataRowToInvitacion(invitacionCtrl1.Retrieve(dctx, new Invitacion { InvitacionID = notificacion.Notificable.GUID }));
                  break;
              case ETipoNotificacion.RANKING_PUBLICACION:
                  UsuarioSocialRankingCtrl usuarioSocialRankingCtrl = new UsuarioSocialRankingCtrl();
                  notificacion.Notificable = usuarioSocialRankingCtrl.LastDataRowToUsuarioSocialRanking(usuarioSocialRankingCtrl.Retrieve(dctx, new UsuarioSocialRanking {UsuarioSocial = notificacion.Emisor}, new Ranking { RankingID = notificacion.Notificable.GUID }));
                  break;
              case ETipoNotificacion.RANKING_COMENTARIO:
                   UsuarioSocialRankingCtrl usuarioSocialRankingCtrl1 = new UsuarioSocialRankingCtrl();
                  notificacion.Notificable = usuarioSocialRankingCtrl1.LastDataRowToUsuarioSocialRanking(usuarioSocialRankingCtrl1.Retrieve(dctx, new UsuarioSocialRanking {UsuarioSocial = notificacion.Emisor}, new Ranking { RankingID = notificacion.Notificable.GUID }));
                  break;
              case ETipoNotificacion.MENSAJE:
                   mensajeCtrl = new MensajeCtrl();
                   notificacion.Notificable = mensajeCtrl.RetrieveComplete(dctx,
                                                                          new Mensaje
                                                                              {
                                                                                  MensajeID = notificacion.Notificable.GUID
                                                                            });
                  break;
              case ETipoNotificacion.RESPUESTA_MENSAJE:
                   mensajeCtrl = new MensajeCtrl();
                   notificacion.Notificable = mensajeCtrl.RetrieveComplete(dctx,
                                                                          new Mensaje
                                                                          {
                                                                              MensajeID = notificacion.Notificable.GUID
                                                                          });
                  break;
              case ETipoNotificacion.PUBLICACION_DOCENTE:
                  publicacionCtrl = new PublicacionCtrl();
                  notificacion.Notificable = publicacionCtrl.LastDataRowToPublicacion(publicacionCtrl.Retrieve(dctx, new Publicacion { PublicacionID = notificacion.Notificable.GUID }));
                  break;
              case ETipoNotificacion.PUBLICACION_ELIMINADA:
                  publicacionCtrl = new PublicacionCtrl();
                  notificacion.Notificable = publicacionCtrl.LastDataRowToPublicacion(publicacionCtrl.Retrieve(dctx, new Publicacion { PublicacionID = notificacion.Notificable.GUID }));
                  break;
              case ETipoNotificacion.COMENTARIO_ELIMINADO:
                  comentarioCtrl = new ComentarioCtrl();
                  notificacion.Notificable = comentarioCtrl.LastDataRowToComentario(comentarioCtrl.Retrieve(dctx, new Comentario { ComentarioID = notificacion.Notificable.GUID }, new Publicacion()));
                  break;
                
          }
          return notificacion;
      }
      /// <summary>
      /// Crea un registro de NotificacionInsHlp en la base de datos
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="notificacionInsHlp">NotificacionInsHlp que desea crear</param>
      public void Insert(IDataContext dctx, Notificacion  notificacion){
         NotificacionInsHlp da = new NotificacionInsHlp();
         da.Action(dctx,  notificacion);
      }
      /// <summary>
      /// Actualiza de manera optimista un registro de NotificacionUpdHlp en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="notificacionUpdHlp">NotificacionUpdHlp que tiene los datos nuevos</param>
      /// <param name="anterior">NotificacionUpdHlp que tiene los datos anteriores</param>
      public void Update(IDataContext dctx, Notificacion  notificacion, Notificacion previous){
         NotificacionUpdHlp da = new NotificacionUpdHlp();
         da.Action(dctx,  notificacion, previous);
      }


      /// <summary>
      /// Elimina un registro de NotificacionDelHlp en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="notificacionDelHlp">NotificacionDelHlp que desea eliminar</param>
      public void Delete(IDataContext dctx, Notificacion  notificacion){
         NotificacionDelHlp da = new NotificacionDelHlp();
         da.Action(dctx,  notificacion);
      }
      /// <summary>
      /// Crea un objeto de Notificacion a partir de los datos del último DataRow del DataSet.
      /// </summary>
      /// <param name="ds">El DataSet que contiene la información de Notificacion</param>
      /// <returns>Un objeto de Notificacion creado a partir de los datos</returns>
      public Notificacion LastDataRowToNotificacion(DataSet ds) {
         if (!ds.Tables.Contains("Notificacion"))
            throw new Exception("LastDataRowToNotificacion: DataSet no tiene la tabla Notificacion");
         int index = ds.Tables["Notificacion"].Rows.Count;
         if (index < 1)
            throw new Exception("LastDataRowToNotificacion: El DataSet no tiene filas");
         return this.DataRowToNotificacion(ds.Tables["Notificacion"].Rows[index - 1]);
      }
      /// <summary>
      /// Crea un objeto de Notificacion a partir de los datos de un DataRow.
      /// </summary>
      /// <param name="row">El DataRow que contiene la información de Notificacion</param>
      /// <returns>Un objeto de Notificacion creado a partir de los datos</returns>
      public Notificacion DataRowToNotificacion(DataRow row){
         Notificacion notificacion = new Notificacion();
         notificacion.Emisor = new UsuarioSocial();
         notificacion.Receptor = new UsuarioSocial();
         if (row.IsNull("NotificacionID"))
            notificacion.NotificacionID = null;
         else
            notificacion.NotificacionID = (Guid)Convert.ChangeType(row["NotificacionID"], typeof(Guid));
         if (row.IsNull("FechaRegistro"))
            notificacion.NotificacionID = null;
         else
            notificacion.FechaRegistro = (DateTime)Convert.ChangeType(row["FechaRegistro"], typeof(DateTime));
         if (row.IsNull("EmisorID"))
            notificacion.Emisor.UsuarioSocialID = null;
         else
            notificacion.Emisor.UsuarioSocialID = (long)Convert.ChangeType(row["EmisorID"], typeof(long));
         if (row.IsNull("ReceptorID"))
            notificacion.Receptor.UsuarioSocialID = null;
         else
            notificacion.Receptor.UsuarioSocialID = (long)Convert.ChangeType(row["ReceptorID"], typeof(long));
         if (row.IsNull("EstatusNotificacion"))
             notificacion.ToShortEstatusNotificacion = null;
         else
             notificacion.ToShortEstatusNotificacion = (short)Convert.ChangeType(row["EstatusNotificacion"], typeof(short));
         if (row.IsNull("TipoNotificacion"))
             notificacion.ToShortTipoNotificacion = null;
         else
         {
             notificacion.ToShortTipoNotificacion = (short)Convert.ChangeType(row["TipoNotificacion"], typeof(short));
             if (row.IsNull("NotificableID"))
                 notificacion.Notificable = null;
             else
             {
                 Guid NotificableID = (Guid)Convert.ChangeType(row["NotificableID"], typeof(Guid));
                 
                 switch (notificacion.TipoNotificacion)
                 {
                     case ETipoNotificacion.PUBLICACION:
                         notificacion.Notificable = new Publicacion{PublicacionID = NotificableID};
                         break;
                     case ETipoNotificacion.COMENTARIO:
                         notificacion.Notificable = new Comentario {ComentarioID = NotificableID};
                         break;
                     case ETipoNotificacion.INVITACION_ACEPTADA:
                         notificacion.Notificable = new Invitacion {InvitacionID = NotificableID};
                         break;
                     case ETipoNotificacion.INVITACION_RECIBIDA:
                         notificacion.Notificable = new Invitacion {InvitacionID = NotificableID};
                         break;
                     case ETipoNotificacion.RANKING_PUBLICACION:
                         notificacion.Notificable = new UsuarioSocialRanking {RankingID = NotificableID};
                         break;
                     case ETipoNotificacion.RANKING_COMENTARIO:
                         notificacion.Notificable = new UsuarioSocialRanking {RankingID = NotificableID};
                         break;
                     case ETipoNotificacion.MENSAJE:
                         notificacion.Notificable = new Mensaje {MensajeID = NotificableID};
                         break;
                    case ETipoNotificacion.RESPUESTA_MENSAJE:
                         notificacion.Notificable = new Mensaje {MensajeID = NotificableID};
                         break;
                    case ETipoNotificacion.PUBLICACION_DOCENTE:
                         notificacion.Notificable = new Publicacion{PublicacionID = NotificableID};
                         break;
                    case ETipoNotificacion.PUBLICACION_ELIMINADA:
                         notificacion.Notificable = new Publicacion {PublicacionID = NotificableID};
                         break;
                    case ETipoNotificacion.COMENTARIO_ELIMINADO:
                         notificacion.Notificable = new Comentario {ComentarioID = NotificableID};
                         break;
                    case ETipoNotificacion.REPORTE_ABUSO:
                         notificacion.Notificable = new ReporteAbuso {ReporteAbusoID = NotificableID};
                         break;
                 }
             }
         }
         return notificacion;
      }

      public void UpdateEstadoNotificacionesRecibidas(IDataContext dctx,EEstatusNotificacion nuevoEstado,EEstatusNotificacion previoEstado,UsuarioSocial usuarioPropietario)
      {
          //verificar notificaciones para actualizar
          DataSet ds = Retrieve(dctx, new Notificacion { EstatusNotificacion = previoEstado, Receptor = usuarioPropietario });
          int index = ds.Tables["Notificacion"].Rows.Count;
          if (index > 0)
          {
              EstadoNotificacionesUpdHlp da = new EstadoNotificacionesUpdHlp();
              da.Action(dctx, nuevoEstado, previoEstado, usuarioPropietario);
          }
         
      }
   } 
}
