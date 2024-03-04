using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Prueba.BO;

namespace POV.Prueba.Diagnostico.DAO { 
   /// <summary>
   /// Actualiza un registro de APrueba en la BD
   /// </summary>
   internal class PruebaUpdHlp { 
      /// <summary>
      /// Actualiza de manera optimista un registro de PruebaUpdHlp en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="pruebaUpdHlp">PruebaUpdHlp que tiene los datos nuevos</param>
      /// <param name="anterior">PruebaUpdHlp que tiene los datos anteriores</param>
      public void Action(IDataContext dctx, APrueba prueba, APrueba anterior){
         object myFirm = new object();
         String sError = string.Empty;
         if (prueba == null)
            sError += ", APrueba";
         if (anterior == null)
            sError += ", Anterior";
         if (sError.Length > 0)
            throw new Exception("PruebaUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (anterior.PruebaID == null)
            sError += ", Anterior APruebaID";
         if (sError.Length > 0)
            throw new Exception("PruebaUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Prueba.Diagnostico.DAO", 
         "PruebaUpdHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "PruebaUpdHlpUpdHl: Hubo un error al conectarse a la base de datos", "POV.Prueba.Diagnostico.DAO", 
         "PruebaUpdHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" UPDATE Prueba ");
         if (prueba.Clave != null){
            sCmd.Append(" SET Clave = @dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            sqlParam.Value = prueba.Clave;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (prueba.Nombre != null){
            sCmd.Append(" ,Nombre = @dbp4ram2 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram2";
            sqlParam.Value = prueba.Nombre;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (prueba.Instrucciones != null){
            sCmd.Append(" ,Instrucciones = @dbp4ram3 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram3";
            sqlParam.Value = prueba.Instrucciones;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (prueba.EstadoLiberacionPrueba != null)
         {
             sCmd.Append(" ,EstadoLiberacion = @dbp4ram4 ");
             sqlParam = sqlCmd.CreateParameter();
             sqlParam.ParameterName = "dbp4ram4";
             sqlParam.Value = prueba.EstadoLiberacionPrueba;
             sqlParam.DbType = DbType.Byte;
             sqlCmd.Parameters.Add(sqlParam);
         }
         sCmd.Append(" ,EsPremium = @dbp4ram6 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram6";
         sqlParam.Value = prueba.EsPremium;
         sqlParam.DbType = DbType.Boolean;
         sqlCmd.Parameters.Add(sqlParam);

          if (anterior.PruebaID == null)
            sCmd.Append(" WHERE PruebaID IS NULL ");
         else{ 
            // anterior.PruebaID
            sCmd.Append(" WHERE PruebaID = @dbp4ram5 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram5";
            sqlParam.Value = anterior.PruebaID;
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
            throw new Exception("PruebaUpdHlp: Hubo  un Error al Actualizar el Registro . " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("PruebaUpdHlp: Hubo  un Error al Actualizar el Registro .");
      }
   } 
}
