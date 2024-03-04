using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Prueba.Diagnostico.Dinamica.BO;
using POV.Prueba.BO;

namespace POV.Prueba.Diagnostico.Dinamica.DAO { 
   /// <summary>
   /// Modifica un registro de ReactivoBancoReactivosDinamico en la BD
   /// </summary>
   internal class ReactivoBancoReactivosDinamicoUpdHlp { 
      /// <summary>
      /// Actualiza de manera optimista un registro de Modifica un registro de ReactivoBancoReactivosDinamico en la base de datos en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="modificaunregistrodeReactivoBancoReactivosDinamicoenlabasededatos">Modifica un registro de ReactivoBancoReactivosDinamico en la base de datos que tiene los datos nuevos</param>
      /// <param name="anterior">Modifica un registro de ReactivoBancoReactivosDinamico en la base de datos que tiene los datos anteriores</param>
      public void Action(IDataContext dctx, ReactivoBanco reactivoBanco){
         object myFirm = new object();
         string sError = String.Empty;
         if (reactivoBanco == null)
            sError += ", reactivoBanco";
         if (sError.Length > 0)
            throw new Exception("ReactivoBancoReactivosDinamicoUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (reactivoBanco.ReactivoBancoID == null)
            sError += ", reactivoBanco.ReactivoBancoID";
         if (sError.Length > 0)
            throw new Exception("ReactivoBancoReactivosDinamicoUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Prueba.Diagnostico.Dinamica.DAO", 
         "ReactivoBancoReactivosDinamicoUpdHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "ReactivoBancoReactivosDinamicoUpdHlp: No se pudo conectar a la base de datos", "POV.Prueba.Diagnostico.Dinamica.DAO", 
         "ReactivoBancoReactivosDinamicoUpdHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" UPDATE ReactivosBancoReactivosDinamico ");
         if (reactivoBanco.Orden == null)
            sCmd.Append(" SET Orden = NULL ");
         else{ 
            // reactivoBanco.Orden
            sCmd.Append(" SET Orden = @dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            sqlParam.Value = reactivoBanco.Orden;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (reactivoBanco.EstaSeleccionado == null)
            sCmd.Append(" ,EstaSeleccionado = NULL ");
         else{ 
            // reactivoBanco.EstaSeleccionado
            sCmd.Append(" ,EstaSeleccionado = @dbp4ram2 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram2";
            sqlParam.Value = reactivoBanco.EstaSeleccionado;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (reactivoBanco.Activo == null)
            sCmd.Append(" ,Activo = NULL ");
         else{ 
            // reactivoBanco.Activo
            sCmd.Append(" ,Activo = @dbp4ram3 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram3";
            sqlParam.Value = reactivoBanco.Activo;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (reactivoBanco.ReactivoBancoID == null)
            sCmd.Append(" WHERE ReactivoBancoID IS NULL ");
         else{ 
            // reactivoBanco.ReactivoBancoID
            sCmd.Append(" WHERE ReactivoBancoID = @dbp4ram4 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram4";
            sqlParam.Value = reactivoBanco.ReactivoBancoID;
            sqlParam.DbType = DbType.Int64;
            sqlCmd.Parameters.Add(sqlParam);
         }
         int iRes = 0;
         try{
            sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
            iRes = sqlCmd.ExecuteNonQuery();
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("ReactivoBancoReactivosDinamicoUpdHlp: Ocurrió un error al ingresar el registro. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("ReactivoBancoReactivosDinamicoUpdHlp: Ocurrió un error al ingresar el registro.");
      }
   } 
}
