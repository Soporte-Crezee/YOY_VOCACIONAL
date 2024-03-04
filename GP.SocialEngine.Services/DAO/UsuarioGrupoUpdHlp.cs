using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using GP.SocialEngine.BO;

namespace GP.SocialEngine.DAO { 
   /// <summary>
   /// Actualiza un registro de UsuarioGrupo en la BD
   /// </summary>
   public class UsuarioGrupoUpdHlp { 
      /// <summary>
      /// Actualiza de manera optimista un registro de UsuarioGrupoUpdHlp en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="usuarioGrupoUpdHlp">UsuarioGrupoUpdHlp que tiene los datos nuevos</param>
      /// <param name="anterior">UsuarioGrupoUpdHlp que tiene los datos anteriores</param>
      public void Action(IDataContext dctx, UsuarioGrupo usuarioGrupo, UsuarioGrupo anterior){
         object myFirm = new object();
         String sError = string.Empty;
         if (usuarioGrupo == null)
            sError += ", usuarioGrupo";
         if (anterior == null)
            sError += ", Anterior";
         if (sError.Length > 0)
            throw new Exception("UsuarioGrupoUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (usuarioGrupo.Estatus == null)
            sError += ", Estatus";
      if (anterior.UsuarioSocial.UsuarioSocialID == null) {
         anterior.UsuarioSocial = new UsuarioSocial();
      }
         if (anterior.UsuarioSocial.UsuarioSocialID == null)
            sError += ", Anterior UsuariosocailID";
         if (anterior.UsuarioGrupoID == null)
            sError += ", Anterior UsuarioGrupoID";
         if (anterior.FechaAsignacion == null)
            sError += ", Anterior FechaAsignacion";
         if (anterior.Estatus == null)
            sError += ", Anterior Estatus";
         if (sError.Length > 0)
            throw new Exception("UsuarioGrupoUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "GP.SocialEngine.DAO", 
         "UsuarioGrupoUpdHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "UsuarioGrupoUpdHl: Hubo un error al conectarse a la base de datos", "GP.SocialEngine.DAO", 
         "UsuarioGrupoUpdHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" UPDATE UsuarioGrupo ");
         if (usuarioGrupo.Estatus == null)
            sCmd.Append(" SET Estatus = NULL ");
         else{ 
            // usuarioGrupo.Estatus
            sCmd.Append(" SET Estatus = @dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            sqlParam.Value = usuarioGrupo.Estatus;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (anterior.UsuarioGrupoID == null)
            sCmd.Append(" WHERE UsuarioGrupoID IS NULL ");
         else{ 
            // anterior.UsuarioGrupoID
            sCmd.Append(" WHERE UsuarioGrupoID = @dbp4ram2 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram2";
            sqlParam.Value = anterior.UsuarioGrupoID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (anterior.UsuarioSocial.UsuarioSocialID == null)
            sCmd.Append(" AND UsuarioSocialID IS NULL ");
         else{ 
            // anterior.UsuarioSocial.UsuarioSocialID
            sCmd.Append(" AND UsuarioSocialID = @dbp4ram3 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram3";
            sqlParam.Value = anterior.UsuarioSocial.UsuarioSocialID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (anterior.FechaAsignacion == null)
            sCmd.Append(" AND fechaAsignacion IS NULL ");
         else{ 
            // anterior.FechaAsignacion
            sCmd.Append(" AND fechaAsignacion = @dbp4ram4 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram4";
            sqlParam.Value = anterior.FechaAsignacion;
            sqlParam.DbType = DbType.DateTime;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (anterior.Estatus == null)
            sCmd.Append(" AND Estatus IS NULL ");
         else{ 
            // anterior.Estatus
            sCmd.Append(" AND Estatus = @dbp4ram5 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram5";
            sqlParam.Value = anterior.Estatus;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
         }
         int iRes = 0;
         try{
            sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
            iRes = sqlCmd.ExecuteNonQuery();
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("UsuarioGrupoUpdHlp: Hubo  un Error al Actualizar el Registro . " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("UsuarioGrupoUpdHlp: Hubo  un Error al Actualizar el Registro .");
      }
   } 
}
