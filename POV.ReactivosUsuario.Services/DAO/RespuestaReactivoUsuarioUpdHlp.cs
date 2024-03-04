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
   /// Actualiza un registro de RespuestaReactivoUsuario en la BD
   /// </summary>
   public class RespuestaReactivoUsuarioUpdHlp { 
      /// <summary>
      /// Actualiza de manera optimista un registro de RespuestaReactivoUsuarioUpdHlp en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="respuestaReactivoUsuarioUpdHlp">RespuestaReactivoUsuarioUpdHlp que tiene los datos nuevos</param>
      /// <param name="anterior">RespuestaReactivoUsuarioUpdHlp que tiene los datos anteriores</param>
      public void Action(IDataContext dctx, RespuestaReactivoUsuario respuestaReactivoUsuario, RespuestaReactivoUsuario anterior){
         object myFirm = new object();
         String sError = string.Empty;
         if (respuestaReactivoUsuario == null)
            sError += ", RespuestaReactivoUsuario";
         if (anterior == null)
            sError += ", Anterior";
         if (sError.Length > 0)
            throw new Exception("RespuestaReactivoUsuarioUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (anterior.RespuestaReactivoUsuarioID == null)
            sError += ", Anterior RespuestaReactivoUsuarioID";
         if (sError.Length > 0)
            throw new Exception("RespuestaReactivoUsuarioUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.ReactivosUsuario.DAO", 
         "RespuestaReactivoUsuarioUpdHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "RespuestaReactivoUsuarioUpdHlpUpdHl: Hubo un error al conectarse a la base de datos", "POV.ReactivosUsuario.DAO", 
         "RespuestaReactivoUsuarioUpdHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" UPDATE RespuestaReactivoUsuario ");
         sCmd.Append(" SET ");
         if (respuestaReactivoUsuario.PrimeraCalificacion != null){
            sCmd.Append(" PrimeraCalificacion = @dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            sqlParam.Value = respuestaReactivoUsuario.PrimeraCalificacion;
            sqlParam.DbType = DbType.Decimal;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (respuestaReactivoUsuario.UltimaCalificacion != null){
            sCmd.Append(" ,UltimaCalificacion = @dbp4ram2 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram2";
            sqlParam.Value = respuestaReactivoUsuario.UltimaCalificacion;
            sqlParam.DbType = DbType.Decimal;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (respuestaReactivoUsuario.FechaRegistro != null){
            sCmd.Append(" ,FechaRegistro = @dbp4ram3 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram3";
            sqlParam.Value = respuestaReactivoUsuario.FechaRegistro;
            sqlParam.DbType = DbType.DateTime;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (respuestaReactivoUsuario.UltimaActualizacion != null){
            sCmd.Append(" ,UltimaActualizacion = @dbp4ram4 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram4";
            sqlParam.Value = respuestaReactivoUsuario.UltimaActualizacion;
            sqlParam.DbType = DbType.DateTime;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (respuestaReactivoUsuario.NumeroIntentos != null){
            sCmd.Append(" ,NumeroIntentos = @dbp4ram5 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram5";
            sqlParam.Value = respuestaReactivoUsuario.NumeroIntentos;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (respuestaReactivoUsuario.EstadoReactivoUsuario != null)
         {
             sCmd.Append(" ,EstadoReactivoUsuario = @dbp4ram7 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram7";
            sqlParam.Value = respuestaReactivoUsuario.EstadoReactivoUsuario;
            sqlParam.DbType = DbType.Int16;
            sqlCmd.Parameters.Add(sqlParam);
         }
          
         if (anterior.RespuestaReactivoUsuarioID == null)
            sCmd.Append(" WHERE RespuestaReactivoUsuarioID IS NULL ");
         else{ 
            // anterior.RespuestaReactivoUsuarioID
            sCmd.Append(" WHERE RespuestaReactivoUsuarioID = @dbp4ram6 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram6";
            sqlParam.Value = anterior.RespuestaReactivoUsuarioID;
            sqlParam.DbType = DbType.Guid;
            sqlCmd.Parameters.Add(sqlParam);
         }
         int iRes = 0;
         try{
            sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
            iRes = sqlCmd.ExecuteNonQuery();
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("RespuestaReactivoUsuarioUpdHlp: Hubo  un Error al Actualizar el Registro . " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("RespuestaReactivoUsuarioUpdHlp: Hubo  un Error al Actualizar el Registro .");
      }
   } 
}
