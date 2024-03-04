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
   /// Controlador del objeto UsuarioSocialRanking
   /// </summary>
   public class UsuarioSocialRankingCtrl { 
      /// <summary>
      /// Consulta registros de UsuarioSocialRankingRetHlp en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="usuarioSocialRankingRetHlp">UsuarioSocialRankingRetHlp que provee el criterio de selección para realizar la consulta</param>
      /// <returns>El DataSet que contiene la información de UsuarioSocialRankingRetHlp generada por la consulta</returns>
      public DataSet Retrieve(IDataContext dctx, UsuarioSocialRanking usuarioSocialRanking, Ranking ranking){
         UsuarioSocialRankingRetHlp da = new UsuarioSocialRankingRetHlp();
         DataSet ds = da.Action(dctx, usuarioSocialRanking, ranking);
         return ds;
      }

      public UsuarioSocialRanking RetrieveComplete(IDataContext dctx, UsuarioSocialRanking usuarioSocialRanking, Ranking ranking)
      {
          usuarioSocialRanking = this.LastDataRowToUsuarioSocialRanking(this.Retrieve(dctx, usuarioSocialRanking,ranking));

          UsuarioSocialCtrl usuarioSocialCtrl = new UsuarioSocialCtrl();

          usuarioSocialRanking.UsuarioSocial = usuarioSocialCtrl.LastDataRowToUsuarioSocial(usuarioSocialCtrl.Retrieve(dctx, usuarioSocialRanking.UsuarioSocial));

          return usuarioSocialRanking;
      }
      /// <summary>
      /// Crea un registro de UsuarioSocialRankingInsHlp en la base de datos
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="usuarioSocialRankingInsHlp">UsuarioSocialRankingInsHlp que desea crear</param>
      public void Insert(IDataContext dctx, UsuarioSocialRanking  usuarioSocialRanking, Ranking ranking){
         UsuarioSocialRankingInsHlp da = new UsuarioSocialRankingInsHlp();
         da.Action(dctx,  usuarioSocialRanking, ranking);
      }

       /// <summary>
       /// Inserta un ranking de usuario de un comentario y registra la notificacion correspondiente
       /// </summary>
       /// <param name="dctx"></param>
       /// <param name="usuarioSocialRanking"></param>
       /// <param name="comentario"></param>
      public void InsertCompleteForComentario(IDataContext dctx, UsuarioSocialRanking usuarioSocialRanking, Comentario comentario)
      {
          object myFirm = new object();
          dctx.OpenConnection(myFirm);
          dctx.BeginTransaction(myFirm);
          try
          {
              ComentarioCtrl comentarioCtrl = new ComentarioCtrl();
              comentario = comentarioCtrl.RetrieveComplete(dctx, comentario, new Publicacion());
              Insert(dctx, usuarioSocialRanking, comentario.Ranking);
              InsertNotificacionForComentario(dctx, usuarioSocialRanking, comentario);
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
       /// Inserta un ranking de usuario de una publicacion y registra la notificacion correspondiente
       /// </summary>
       /// <param name="dctx"></param>
       /// <param name="usuarioSocialRanking"></param>
       /// <param name="publicacion"></param>
      public void InsertCompleteForPublicacion(IDataContext dctx, UsuarioSocialRanking usuarioSocialRanking, Publicacion publicacion)
      {
          object myFirm = new object();
          dctx.OpenConnection(myFirm);
          dctx.BeginTransaction(myFirm);
          try
          {
              PublicacionCtrl publicacionCtrl = new PublicacionCtrl();
              publicacion = publicacionCtrl.RetrieveComplete(dctx, publicacion);
              Insert(dctx, usuarioSocialRanking, publicacion.Ranking);
              InsertNotificacionForPublicacion(dctx, usuarioSocialRanking, publicacion);
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
       /// Inserta una notificacion de incremento de ranking de un comentario
       /// </summary>
       /// <param name="dctx"></param>
       /// <param name="usuarioSocialRanking"></param>
       /// <param name="comentario"></param>
      public void InsertNotificacionForComentario(IDataContext dctx, UsuarioSocialRanking usuarioSocialRanking, Comentario comentario)
      {

          if (usuarioSocialRanking.UsuarioSocial.UsuarioSocialID == comentario.UsuarioSocial.UsuarioSocialID)
              return;
          Notificacion notificacion = new Notificacion();
          notificacion.NotificacionID = Guid.NewGuid();
          notificacion.FechaRegistro = usuarioSocialRanking.FechaRegistro;
          notificacion.Emisor = usuarioSocialRanking.UsuarioSocial;
          notificacion.Receptor = comentario.UsuarioSocial;
          notificacion.Notificable = usuarioSocialRanking;
          notificacion.TipoNotificacion = ETipoNotificacion.RANKING_COMENTARIO;
          notificacion.EstatusNotificacion = EEstatusNotificacion.NUEVO;

          NotificacionCtrl notificacionCtrl = new NotificacionCtrl();
          notificacionCtrl.Insert(dctx, notificacion);
      }
       /// <summary>
       /// Inserta una notificacion de incremento de ranking de una publicacion
       /// </summary>
       /// <param name="dctx"></param>
       /// <param name="usuarioSocialRanking"></param>
       /// <param name="publicacion"></param>
      public void InsertNotificacionForPublicacion(IDataContext dctx, UsuarioSocialRanking usuarioSocialRanking, Publicacion publicacion)
      {
          if (usuarioSocialRanking.UsuarioSocial.UsuarioSocialID == publicacion.UsuarioSocial.UsuarioSocialID)
              return;
          Notificacion notificacion = new Notificacion();
          notificacion.NotificacionID = Guid.NewGuid();
          notificacion.FechaRegistro = usuarioSocialRanking.FechaRegistro;
          notificacion.Emisor = usuarioSocialRanking.UsuarioSocial;
          notificacion.Receptor = publicacion.UsuarioSocial;
          notificacion.Notificable = usuarioSocialRanking;
          notificacion.TipoNotificacion = ETipoNotificacion.RANKING_PUBLICACION;
          notificacion.EstatusNotificacion = EEstatusNotificacion.NUEVO;

          NotificacionCtrl notificacionCtrl = new NotificacionCtrl();
          notificacionCtrl.Insert(dctx, notificacion);
      }
      /// <summary>
      /// Actualiza de manera optimista un registro de UsuarioSocialRankingUpdHlp en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="usuarioSocialRankingUpdHlp">UsuarioSocialRankingUpdHlp que tiene los datos nuevos</param>
      /// <param name="anterior">UsuarioSocialRankingUpdHlp que tiene los datos anteriores</param>
      public void Update(IDataContext dctx, UsuarioSocialRanking  usuarioSocialRanking, Ranking ranking, UsuarioSocialRanking previous){
         UsuarioSocialRankingUpdHlp da = new UsuarioSocialRankingUpdHlp();
         da.Action(dctx,  usuarioSocialRanking, ranking, previous);
      }
      /// <summary>
      /// Elimina un registro de UsuarioSocialRankingDelHlp en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="usuarioSocialRankingDelHlp">UsuarioSocialRankingDelHlp que desea eliminar</param>
      public void Delete(IDataContext dctx, UsuarioSocialRanking  usuarioSocialRanking, Ranking ranking){
         UsuarioSocialRankingDelHlp da = new UsuarioSocialRankingDelHlp();
         da.Action(dctx,  usuarioSocialRanking, ranking);
      }
      /// <summary>
      /// Crea un objeto de UsuarioSocialRanking a partir de los datos del último DataRow del DataSet.
      /// </summary>
      /// <param name="ds">El DataSet que contiene la información de UsuarioSocialRanking</param>
      /// <returns>Un objeto de UsuarioSocialRanking creado a partir de los datos</returns>
      public UsuarioSocialRanking LastDataRowToUsuarioSocialRanking(DataSet ds) {
         if (!ds.Tables.Contains("UsuarioSocialRanking"))
            throw new Exception("LastDataRowToUsuarioSocialRanking: DataSet no tiene la tabla UsuarioSocialRanking");
         int index = ds.Tables["UsuarioSocialRanking"].Rows.Count;
         if (index < 1)
            throw new Exception("LastDataRowToUsuarioSocialRanking: El DataSet no tiene filas");
         return this.DataRowToUsuarioSocialRanking(ds.Tables["UsuarioSocialRanking"].Rows[index - 1]);
      }
      /// <summary>
      /// Crea un objeto de UsuarioSocialRanking a partir de los datos de un DataRow.
      /// </summary>
      /// <param name="row">El DataRow que contiene la información de UsuarioSocialRanking</param>
      /// <returns>Un objeto de UsuarioSocialRanking creado a partir de los datos</returns>
      public UsuarioSocialRanking DataRowToUsuarioSocialRanking(DataRow row){
         UsuarioSocialRanking usuarioSocialRanking = new UsuarioSocialRanking();
         usuarioSocialRanking.UsuarioSocial = new UsuarioSocial();
         if (row.IsNull("RankingID"))
             usuarioSocialRanking.RankingID = null;
         else
             usuarioSocialRanking.RankingID = (Guid)Convert.ChangeType(row["RankingID"], typeof(Guid));
         if (row.IsNull("UsuarioSocialID"))
            usuarioSocialRanking.UsuarioSocial.UsuarioSocialID = null;
         else
            usuarioSocialRanking.UsuarioSocial.UsuarioSocialID = (long)Convert.ChangeType(row["UsuarioSocialID"], typeof(long));
         if (row.IsNull("Puntuacion"))
            usuarioSocialRanking.Puntuacion = null;
         else
            usuarioSocialRanking.Puntuacion = (int)Convert.ChangeType(row["Puntuacion"], typeof(int));
         if (row.IsNull("FechaRegistro"))
            usuarioSocialRanking.FechaRegistro = null;
         else
            usuarioSocialRanking.FechaRegistro = (DateTime)Convert.ChangeType(row["FechaRegistro"], typeof(DateTime));
         return usuarioSocialRanking;
      }
   } 
}
