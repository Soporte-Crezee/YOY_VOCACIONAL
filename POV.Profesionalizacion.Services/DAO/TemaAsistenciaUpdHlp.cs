using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Profesionalizacion.BO;

namespace POV.Profesionalizacion.DAO { 
   /// <summary>
   /// Actualiza un registro de TemaAsistencia en la BD
   /// </summary>
    internal class TemaAsistenciaUpdHlp
    { 
      /// <summary>
      /// Actualiza de manera optimista un registro de TemaAsistenciaUpdHlp en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="temaAsistenciaUpdHlp">TemaAsistenciaUpdHlp que tiene los datos nuevos</param>
      /// <param name="anterior">TemaAsistenciaUpdHlp que tiene los datos anteriores</param>
      public void Action(IDataContext dctx, TemaAsistencia temaAsistencia, TemaAsistencia anterior){
         object myFirm = new object();
         String sError = string.Empty;
         if (temaAsistencia == null)
            sError += ", TemaAsistencia";
         if (anterior == null)
            sError += ", Anterior";
         if (sError.Length > 0)
            throw new Exception("TemaAsistenciaUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (anterior.TemaAsistenciaID == null)
            sError += ", Anterior TemaAsistenciaID";
         if (sError.Length > 0)
            throw new Exception("TemaAsistenciaUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Profesionalizacion.DAO", 
         "TemaAsistenciaUpdHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "TemaAsistenciaUpdHlpUpdHl: Hubo un error al conectarse a la base de datos", "POV.Profesionalizacion.DAO", 
         "TemaAsistenciaUpdHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" UPDATE TemaAsistencia ");
         if (temaAsistencia.Nombre == null)
            sCmd.Append(" SET Nombre = NULL ");
         else{ 
            // temaAsistencia.Nombre
            sCmd.Append(" SET Nombre = @dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            sqlParam.Value = temaAsistencia.Nombre;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (temaAsistencia.Descripcion == null)
            sCmd.Append(" ,Descripcion = NULL ");
         else{ 
            // temaAsistencia.Descripcion
            sCmd.Append(" ,Descripcion = @dbp4ram2 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram2";
            sqlParam.Value = temaAsistencia.Descripcion;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (temaAsistencia.FechaRegistro != null){
            sCmd.Append(" ,FechaRegistro = @dbp4ram3 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram3";
            sqlParam.Value = temaAsistencia.FechaRegistro;
            sqlParam.DbType = DbType.DateTime;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (temaAsistencia.Activo != null){
            sCmd.Append(" ,Activo = @dbp4ram4 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram4";
            sqlParam.Value = temaAsistencia.Activo;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (anterior.TemaAsistenciaID == null)
            sCmd.Append(" WHERE temaAsistenciaID IS NULL ");
         else{ 
            // anterior.TemaAsistenciaID
            sCmd.Append(" WHERE temaAsistenciaID = @dbp4ram5 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram5";
            sqlParam.Value = anterior.TemaAsistenciaID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
         }
         int iRes = 0;
         try{
            sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
            iRes = sqlCmd.ExecuteNonQuery();
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("TemaAsistenciaUpdHlp: Hubo  un Error al Actualizar el Registro . " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("TemaAsistenciaUpdHlp: Hubo  un Error al Actualizar el Registro .");
      }
   } 
}
