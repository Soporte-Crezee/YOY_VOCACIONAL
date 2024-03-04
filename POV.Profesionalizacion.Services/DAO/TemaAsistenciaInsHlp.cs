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
   /// Guarda un registro de TemaAsistencia en la BD
   /// </summary>
    internal class TemaAsistenciaInsHlp
    { 
      /// <summary>
      /// Crea un registro de TemaAsistencia en la base de datos
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="temaAsistencia">TemaAsistencia que desea crear</param>
      public void Action(IDataContext dctx, TemaAsistencia temaAsistencia){
         object myFirm = new object();
         string sError = String.Empty;
         if (temaAsistencia == null)
            sError += ", TemaAsistencia";
         if (sError.Length > 0)
            throw new Exception("TemaAsistenciaInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Profesionalizacion.DAO", 
         "TemaAsistenciaInsHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "TemaAsistenciaInsHlp: No se pudo conectar a la base de datos", "POV.Profesionalizacion.DAO", 
         "TemaAsistenciaInsHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" INSERT INTO TemaAsistencia (Nombre,Descripcion,FechaRegistro,Activo) ");
         sCmd.Append(" VALUES ");
         sCmd.Append(" ( ");
         // temaAsistencia.Nombre
         sCmd.Append(" @dbp4ram1 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram1";
         if (temaAsistencia.Nombre == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = temaAsistencia.Nombre;
         sqlParam.DbType = DbType.String;
         sqlCmd.Parameters.Add(sqlParam);
         // temaAsistencia.Descripcion
         sCmd.Append(" ,@dbp4ram2 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram2";
         if (temaAsistencia.Descripcion == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = temaAsistencia.Descripcion;
         sqlParam.DbType = DbType.String;
         sqlCmd.Parameters.Add(sqlParam);
         // temaAsistencia.FechaRegistro
         sCmd.Append(" ,@dbp4ram3 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram3";
         if (temaAsistencia.FechaRegistro == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = temaAsistencia.FechaRegistro;
         sqlParam.DbType = DbType.DateTime;
         sqlCmd.Parameters.Add(sqlParam);
         // temaAsistencia.Activo
         sCmd.Append(" ,@dbp4ram4 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram4";
         if (temaAsistencia.Activo == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = temaAsistencia.Activo;
         sqlParam.DbType = DbType.Byte;
         sqlCmd.Parameters.Add(sqlParam);
         sCmd.Append(" ) ");
         int iRes = 0;
         try{
            sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
            iRes = sqlCmd.ExecuteNonQuery();
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("TemaAsistenciaInsHlp: Ocurrió un error al ingresar el registro. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("TemaAsistenciaInsHlp: Ocurrió un error al ingresar el registro.");
      }
   } 
}
