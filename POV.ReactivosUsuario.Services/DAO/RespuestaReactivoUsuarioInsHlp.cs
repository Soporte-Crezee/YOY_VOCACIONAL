using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.ReactivosUsuario.BO;
using POV.ReactivosUsuario.DAO;
using POV.Reactivos.BO;
using GP.SocialEngine.BO;
namespace POV.ReactivosUsuario.DAO { 
   /// <summary>
   /// Guarda un registro de RespuestaReactivoUsuario en la BD
   /// </summary>
   public class RespuestaReactivoUsuarioInsHlp { 
      /// <summary>
      /// Crea un registro de RespuestaReactivoUsuario en la base de datos
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="respuestaReactivoUsuario">RespuestaReactivoUsuario que desea crear</param>
      public void Action(IDataContext dctx, RespuestaReactivoUsuario respuestaReactivoUsuario){
         object myFirm = new object();
         string sError = String.Empty;
         if (respuestaReactivoUsuario == null)
            sError += ", RespuestaReactivoUsuario";
         if (sError.Length > 0)
            throw new Exception("RespuestaReactivoUsuarioInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (respuestaReactivoUsuario.RespuestaReactivoUsuarioID == null)
            sError += ", RespuestaReactivoUsuarioID";
         if (sError.Length > 0)
            throw new Exception("RespuestaReactivoUsuarioInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (respuestaReactivoUsuario.Reactivo == null)
            sError += ", Reactivo";
         if (sError.Length > 0)
            throw new Exception("RespuestaReactivoUsuarioInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (respuestaReactivoUsuario.Reactivo.ReactivoID == null)
            sError += ", ReactivoID";
         if (sError.Length > 0)
            throw new Exception("RespuestaReactivoUsuarioInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (respuestaReactivoUsuario.UsuarioSocial == null)
            sError += ", UsuarioSocial";
         if (sError.Length > 0)
            throw new Exception("RespuestaReactivoUsuarioInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (respuestaReactivoUsuario.UsuarioSocial.UsuarioSocialID == null)
            sError += ", UsuarioSocialID";
         if (sError.Length > 0)
            throw new Exception("RespuestaReactivoUsuarioInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (respuestaReactivoUsuario.FechaRegistro == null)
            sError += ", FechaRegistro";
         if (sError.Length > 0)
            throw new Exception("RespuestaReactivoUsuarioInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.ReactivosUsuario.DAO", 
         "RespuestaReactivoUsuarioInsHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "RespuestaReactivoUsuarioInsHlp: No se pudo conectar a la base de datos", "POV.ReactivosUsuario.DAO", 
         "RespuestaReactivoUsuarioInsHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" INSERT INTO RespuestaReactivoUsuario (RespuestaReactivoUsuarioID, ReactivoID, UsuarioSocialID, PrimeraCalificacion, UltimaCalificacion, FechaRegistro,UltimaActualizacion, NumeroIntentos, EstadoReactivoUsuario) ");
         sCmd.Append(" VALUES ");
         sCmd.Append(" ( ");
         // respuestaReactivoUsuario.RespuestaReactivoUsuarioID
         sCmd.Append(" @dbp4ram1 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram1";
         if (respuestaReactivoUsuario.RespuestaReactivoUsuarioID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = respuestaReactivoUsuario.RespuestaReactivoUsuarioID;
         sqlParam.DbType = DbType.Guid;
         sqlCmd.Parameters.Add(sqlParam);
         // respuestaReactivoUsuario.Reactivo.ReactivoID
         sCmd.Append(" ,@dbp4ram2 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram2";
         if (respuestaReactivoUsuario.Reactivo.ReactivoID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = respuestaReactivoUsuario.Reactivo.ReactivoID;
         sqlParam.DbType = DbType.Guid;
         sqlCmd.Parameters.Add(sqlParam);
         // respuestaReactivoUsuario.UsuarioSocial.UsuarioSocialID
         sCmd.Append(" ,@dbp4ram3 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram3";
         if (respuestaReactivoUsuario.UsuarioSocial.UsuarioSocialID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = respuestaReactivoUsuario.UsuarioSocial.UsuarioSocialID;
         sqlParam.DbType = DbType.Int64;
         sqlCmd.Parameters.Add(sqlParam);
         // respuestaReactivoUsuario.PrimeraCalificacion
         sCmd.Append(" ,@dbp4ram4 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram4";
         if (respuestaReactivoUsuario.PrimeraCalificacion == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = respuestaReactivoUsuario.PrimeraCalificacion;
         sqlParam.DbType = DbType.Decimal;
         sqlCmd.Parameters.Add(sqlParam);
         // respuestaReactivoUsuario.UltimaCalificacion
         sCmd.Append(" ,@dbp4ram5 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram5";
         if (respuestaReactivoUsuario.UltimaCalificacion == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = respuestaReactivoUsuario.UltimaCalificacion;
         sqlParam.DbType = DbType.Decimal;
         sqlCmd.Parameters.Add(sqlParam);
         // respuestaReactivoUsuario.FechaRegistro
         sCmd.Append(" ,@dbp4ram6 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram6";
         if (respuestaReactivoUsuario.FechaRegistro == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = respuestaReactivoUsuario.FechaRegistro;
         sqlParam.DbType = DbType.DateTime;
         sqlCmd.Parameters.Add(sqlParam);
         // respuestaReactivoUsuario.UltimaActualizacion
         sCmd.Append(" ,@dbp4ram7 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram7";
         if (respuestaReactivoUsuario.UltimaActualizacion == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = respuestaReactivoUsuario.UltimaActualizacion;
         sqlParam.DbType = DbType.DateTime;
         sqlCmd.Parameters.Add(sqlParam);
         // respuestaReactivoUsuario.NumeroIntentos EstadoReactivoUsuario
         sCmd.Append(" ,@dbp4ram8 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram8";
         if (respuestaReactivoUsuario.NumeroIntentos == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = respuestaReactivoUsuario.NumeroIntentos;
         sqlParam.DbType = DbType.Int32;
         sqlCmd.Parameters.Add(sqlParam);
         // respuestaReactivoUsuario.EstadoReactivoUsuario 
         sCmd.Append(" ,@dbp4ram9 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram9";
         if (respuestaReactivoUsuario.EstadoReactivoUsuario == null)
             sqlParam.Value = DBNull.Value;
         else
             sqlParam.Value = respuestaReactivoUsuario.EstadoReactivoUsuario;
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
            throw new Exception("RespuestaReactivoUsuarioInsHlp: Ocurrió un error al ingresar el registro. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("RespuestaReactivoUsuarioInsHlp: Ocurrió un error al ingresar el registro.");
      }
   } 
}
