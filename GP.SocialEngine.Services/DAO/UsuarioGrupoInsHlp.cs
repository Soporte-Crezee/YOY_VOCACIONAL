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
   /// Inserta un registro de UsuarioGrupo en la BD
   /// </summary>
   public class UsuarioGrupoInsHlp { 
      /// <summary>
      /// Crea un registro de UsuarioGrupo en la base de datos
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="usuarioGrupo">UsuarioGrupo que desea crear</param>
      public void Action(IDataContext dctx, UsuarioGrupo usuarioGrupo, GrupoSocial grupoSocial){
         object myFirm = new object();
         string sError = String.Empty;
         if (usuarioGrupo == null)
            sError += ", UsuarioGrupo";
         if (usuarioGrupo.UsuarioSocial == null)
            sError += ", UsuarioSocial";
         if (grupoSocial == null)
            sError += ", GrupoSocial";
         if (sError.Length > 0)
            throw new Exception("UsuarioGrupo: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (usuarioGrupo.UsuarioSocial.UsuarioSocialID == null)
            sError += ", UsuarioSocialID";
         if (usuarioGrupo.FechaAsignacion == null)
            sError += ", FechaAsignacion";
         if (grupoSocial.GrupoSocialID == null)
            sError += ", GrupoSocialID";
         if (usuarioGrupo.Estatus == null)
            sError += ", Estatus";
         if (usuarioGrupo.EsModerador == null)
            sError += ", EsModerador";
         if (sError.Length > 0)
            throw new Exception("UsuarioGrupo: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "GP.SocialEngine.DAO", 
         "UsuarioGrupoInsHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "UsuarioGrupo: No se pudo conectar a la base de datos", "GP.SocialEngine.DAO", 
         "UsuarioGrupoInsHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" INSERT INTO UsuarioGrupo (UsuarioSocialID, GrupoSocialID, FechaAsignacion, Estatus,EsModerador) ");
         sCmd.Append(" VALUES ");
         sCmd.Append(" ( ");
         // usuarioGrupo.UsuarioSocial.UsuarioSocialID
         sCmd.Append(" @dbp4ram1 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram1";
         if (usuarioGrupo.UsuarioSocial.UsuarioSocialID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = usuarioGrupo.UsuarioSocial.UsuarioSocialID;
         sqlParam.DbType = DbType.Int64;
         sqlCmd.Parameters.Add(sqlParam);
         // grupoSocial.GrupoSocialID
         sCmd.Append(" ,@dbp4ram2 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram2";
         if (grupoSocial.GrupoSocialID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = grupoSocial.GrupoSocialID;
         sqlParam.DbType = DbType.Int64;
         sqlCmd.Parameters.Add(sqlParam);
         // usuarioGrupo.FechaAsignacion
         sCmd.Append(" ,@dbp4ram3 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram3";
         if (usuarioGrupo.FechaAsignacion == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = usuarioGrupo.FechaAsignacion;
         sqlParam.DbType = DbType.DateTime;
         sqlCmd.Parameters.Add(sqlParam);
         // usuarioGrupo.Estatus
         sCmd.Append(" ,@dbp4ram4 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram4";
         if (usuarioGrupo.Estatus == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = usuarioGrupo.Estatus;
         sqlParam.DbType = DbType.Boolean;
         sqlCmd.Parameters.Add(sqlParam);
         // usuarioGrupo.EsModerador
         sCmd.Append(" ,@dbp4ram5 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram5";
         if (usuarioGrupo.EsModerador == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = usuarioGrupo.EsModerador;
         sqlParam.DbType = DbType.Boolean;
         sqlCmd.Parameters.Add(sqlParam);
         sCmd.Append(" ) ");
         int iRes = 0;
         try{
            sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
            iRes = sqlCmd.ExecuteNonQuery();
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("UsuarioGrupoIndHlp: Ocurrió un error al ingresar el registro. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("UsuarioGrupoIndHlp: Ocurrió un error al ingresar el registro.");
      }
   } 
}
