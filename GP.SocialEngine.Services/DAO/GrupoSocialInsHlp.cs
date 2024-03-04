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
   /// Guarda un registro de GrupoSocial en la BD
   /// </summary>
   public class GrupoSocialInsHlp { 
      /// <summary>
      /// Crea un registro de GrupoSocial en la base de datos
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="grupoSocial">GrupoSocial que desea crear</param>
      public void Action(IDataContext dctx, GrupoSocial grupoSocial){
         object myFirm = new object();
         string sError = String.Empty;
         if (grupoSocial == null)
            sError += ", GrupoSocial";
         if (sError.Length > 0)
            throw new Exception("GrupoSocialInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (grupoSocial.Nombre == null || grupoSocial.Nombre.Trim().Length == 0)
            sError += ", Nombre";
         if (grupoSocial.GrupoSocialGuid == null)
            sError += ", GrupoSocialGuid";
         if (grupoSocial.FechaCreacion == null)
            sError += ", FechaCreación";
         if (grupoSocial.NumeroMiembros == null)
            sError += ", NumeroMiebros";
         if (sError.Length > 0)
            throw new Exception("GrupoSocialInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "GP.SocialEngine.DAO", 
         "GrupoSocialInsHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "GrupoSocialInsHlp: No se pudo conectar a la base de datos", "GP.SocialEngine.DAO", 
         "GrupoSocialInsHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" INSERT INTO GrupoSocial (GrupoSocialGuid, Nombre, Descripcion, FechaCreacion, NumeroMiembros, TipoGrupoSocial) ");
         sCmd.Append(" VALUES ");
         sCmd.Append(" ( ");
         // grupoSocial.GrupoSocialGuid
         sCmd.Append(" @dbp4ram1 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram1";
         if (grupoSocial.GrupoSocialGuid == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = grupoSocial.GrupoSocialGuid;
         sqlParam.DbType = DbType.Guid;
         sqlCmd.Parameters.Add(sqlParam);
         // grupoSocial.Nombre
         sCmd.Append(" ,@dbp4ram2 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram2";
         if (grupoSocial.Nombre == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = grupoSocial.Nombre;
         sqlParam.DbType = DbType.String;
         sqlCmd.Parameters.Add(sqlParam);
         // grupoSocial.Descripcion
         sCmd.Append(" ,@dbp4ram3 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram3";
         if (grupoSocial.Descripcion == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = grupoSocial.Descripcion;
         sqlParam.DbType = DbType.String;
         sqlCmd.Parameters.Add(sqlParam);
         // grupoSocial.FechaCreacion
         sCmd.Append(" ,@dbp4ram4 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram4";
         if (grupoSocial.FechaCreacion == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = grupoSocial.FechaCreacion;
         sqlParam.DbType = DbType.DateTime;
         sqlCmd.Parameters.Add(sqlParam);
         // grupoSocial.NumeroMiembros
         sCmd.Append(" ,@dbp4ram5 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram5";
         if (grupoSocial.NumeroMiembros == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = grupoSocial.NumeroMiembros;
         sqlParam.DbType = DbType.Int32;
         sqlCmd.Parameters.Add(sqlParam);
         // grupoSocial.TipoGrupoSocial
         sCmd.Append(" ,@dbp4ram6 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram6";
         if (grupoSocial.TipoGrupoSocial == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = grupoSocial.TipoGrupoSocial;
         sqlParam.DbType = DbType.Int16;
         sqlCmd.Parameters.Add(sqlParam);
         sCmd.Append(" ) ");
         int iRes = 0;
         try{
            sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
            iRes = sqlCmd.ExecuteNonQuery();
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("GrupoSocialInsHlp: Ocurrió un error al ingresar el registro. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("GrupoSocialInsHlp: Ocurrió un error al ingresar el registro.");
      }
   } 
}
