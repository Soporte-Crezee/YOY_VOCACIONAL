// Clase motor de red social
using System;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using GP.SocialEngine.BO;
using GP.SocialEngine.DAO;

namespace GP.SocialEngine.Service { 
   /// <summary>
   /// Controlador de Usuario Social
   /// </summary>
   public class UsuarioSocialCtrl { 
      /// <summary>
      /// Consulta registros de usuarioSocial en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="usuarioSocial">usuarioSocial que provee el criterio de selección para realizar la consulta</param>
      /// <returns>El DataSet que contiene la información de usuarioSocial generada por la consulta</returns>
      public DataSet Retrieve(IDataContext dctx, UsuarioSocial usuarioSocial){
         UsuarioSocialRetHlp da = new UsuarioSocialRetHlp();
         DataSet ds = da.Action(dctx, usuarioSocial);
         return ds;
      }
      /// <summary>
      /// Crea un registro de usuarioSocial en la base de datos
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="usuarioSocial">usuarioSocial que desea crear</param>
      public void Insert(IDataContext dctx, UsuarioSocial usuarioSocial){
         UsuarioSocialInsHlp da = new UsuarioSocialInsHlp();
         da.Action(dctx, usuarioSocial);
      }
      /// <summary>
      /// Actualiza de manera optimista un registro de usuarioSocial en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="usuarioSocial">usuarioSocial que tiene los datos nuevos</param>
      /// <param name="anterior">usuarioSocial que tiene los datos anteriores</param>
      public void Update(IDataContext dctx, UsuarioSocial usuarioSocial, UsuarioSocial previous){
         UsuarioSocialUpdHlp da = new UsuarioSocialUpdHlp();
         da.Action(dctx, usuarioSocial, previous);
      }
      /// <summary>
      /// Consulta registros de usuarioSocial en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="usuarioSocial">usuarioSocial que provee el criterio de selección para realizar la consulta</param>
      /// <returns>El DataSet que contiene la información de usuarioSocial generada por la consulta</returns>
      public DataSet RetriveFriends(IDataContext dctx, SocialHub socialHub){
         RetriveFriendsHlp da = new RetriveFriendsHlp();
         DataSet ds = da.Action(dctx, socialHub);
         return ds;
      }
      /// <summary>
      /// Regresa un resgistro completp de UsuarioSocial
      /// </summary>
      public UsuarioSocial RetrieveComplete(IDataContext dctx, UsuarioSocial usuarioSocial){
              #region RetriveComplete
              usuarioSocial = new UsuarioSocial { UsuarioSocialID = usuarioSocial.UsuarioSocialID };
              try
              {

                  DataSet ds = Retrieve(dctx, new UsuarioSocial { UsuarioSocialID = usuarioSocial.UsuarioSocialID });
                  if (ds.Tables["UsuarioSocial"].Rows.Count > 0)
                  {
                      usuarioSocial = LastDataRowToUsuarioSocial(ds);
                  }
                  else
                  {
                      usuarioSocial = null;
                  }
              }
              catch ( Exception ex )
              {
                  usuarioSocial = null;
              }
              return usuarioSocial;
              #endregion
      }

      public void InsertCreateSocialHub ( IDataContext dctx , UsuarioSocial usuarioSocial )
      {

          UsuarioSocialCtrl usuarioSocialCtrl = new UsuarioSocialCtrl( );
          SocialHubCtrl socialHubCtrl = new SocialHubCtrl( );
          object myFirm = new object( );
          dctx.OpenConnection( myFirm );
          dctx.BeginTransaction( myFirm );

          try
          {
              Insert( dctx , usuarioSocial );
              usuarioSocial = usuarioSocialCtrl.LastDataRowToUsuarioSocial( usuarioSocialCtrl.Retrieve( dctx , usuarioSocial ) );

              //Insertar la informacion Social del elemento
              InformacionSocialCtrl informacionSocialCtrl = new InformacionSocialCtrl( );
              InformacionSocial informacionSocial = new InformacionSocial { FechaRegistro=System.DateTime.Now, IMMSN=usuarioSocial.Email };

              informacionSocialCtrl.Insert( dctx, informacionSocial);

              informacionSocial = informacionSocialCtrl.LastDataRowToInformacionSocial( informacionSocialCtrl.Retrieve(dctx, informacionSocial) );

              SocialHub socialHub = new SocialHub
              {
                  Alias = usuarioSocial.LoginName ,
                  FechaRegistro = DateTime.Now ,
                  SocialProfile = usuarioSocial,
                  InformacionSocial=informacionSocial,
                  SocialProfileType=ESocialProfileType.USUARIOSOCIAL                  
              };

              socialHubCtrl.InsertSocialHubUsuario( dctx , socialHub );
              socialHub = socialHubCtrl.LastDataRowToSocialHub( socialHubCtrl.RetrieveSocialHubUsuario( dctx , socialHub ) );
          }
          catch ( Exception )
          {
              dctx.RollbackTransaction( myFirm );
              dctx.CloseConnection( myFirm );
              throw;
          }
          dctx.CommitTransaction( myFirm );
          dctx.CloseConnection( myFirm );
      }

      private bool ValidateEmailForInsert ( IDataContext dctx , UsuarioSocial usuarioSocial )
      {
          DataSet ds = Retrieve( dctx , usuarioSocial );
          return ds.Tables[ "UsuarioSocial" ].Rows.Count <= 0;
      }

      /// <summary>
      /// Crea un objeto de UsuarioSocial a partir de los datos del último DataRow del DataSet.
      /// </summary>
      /// <param name="ds">El DataSet que contiene la información de UsuarioSocial</param>
      /// <returns>Un objeto de UsuarioSocial creado a partir de los datos</returns>
      public UsuarioSocial LastDataRowToUsuarioSocial(DataSet ds) {
         if (!ds.Tables.Contains("UsuarioSocial"))
            throw new Exception("LastDataRowToUsuarioSocial: DataSet no tiene la tabla UsuarioSocial");
         int index = ds.Tables["UsuarioSocial"].Rows.Count;
         if (index < 1)
            throw new Exception("LastDataRowToUsuarioSocial: El DataSet no tiene filas");
         return this.DataRowToUsuarioSocial(ds.Tables["UsuarioSocial"].Rows[index - 1]);
      }
      /// <summary>
      /// Crea un objeto de UsuarioSocial a partir de los datos de un DataRow.
      /// </summary>
      /// <param name="row">El DataRow que contiene la información de UsuarioSocial</param>
      /// <returns>Un objeto de UsuarioSocial creado a partir de los datos</returns>
      public UsuarioSocial DataRowToUsuarioSocial(DataRow row){
         UsuarioSocial usuarioSocial = new UsuarioSocial();
         if (row.IsNull("UsuarioSocialID"))
            usuarioSocial.UsuarioSocialID = null;
         else
            usuarioSocial.UsuarioSocialID = (long)Convert.ChangeType(row["UsuarioSocialID"], typeof(long));
         if (row.IsNull("Email"))
            usuarioSocial.Email = null;
         else
            usuarioSocial.Email = (string)Convert.ChangeType(row["Email"], typeof(string));
         if (row.IsNull("LoginName"))
            usuarioSocial.LoginName = null;
         else
            usuarioSocial.LoginName = (string)Convert.ChangeType(row["LoginName"], typeof(string));
         if (row.IsNull("ScreenName"))
            usuarioSocial.ScreenName = null;
         else
            usuarioSocial.ScreenName = (string)Convert.ChangeType(row["ScreenName"], typeof(string));
         if (row.IsNull("Estatus"))
            usuarioSocial.Estatus = null;
         else
            usuarioSocial.Estatus = (bool)Convert.ChangeType(row["Estatus"], typeof(bool));
         if (row.IsNull("FechaNacimiento"))
             usuarioSocial.FechaNacimiento = null;
         else
             usuarioSocial.FechaNacimiento = (DateTime) Convert.ChangeType(row["FechaNacimiento"], typeof (DateTime));
         return usuarioSocial;
      }
   } 
}
