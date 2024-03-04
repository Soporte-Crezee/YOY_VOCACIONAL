using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Prueba.BO;
using POV.Prueba.Diagnostico.Dinamica.BO;
using POV.CentroEducativo.BO;

namespace POV.Prueba.Diagnostico.Dinamica.DAO { 
   /// <summary>
   /// Actualiza un registro de RegistroPruebaDinamica en la BD
   /// </summary>
   internal class RegistroPruebaDinamicaUpdHlp { 
      /// <summary>
      /// Actualiza de manera optimista un registro de RegistroPruebaDinamicaUpdHlp en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="registroPruebaDinamicaUpdHlp">RegistroPruebaDinamicaUpdHlp que tiene los datos nuevos</param>
      /// <param name="anterior">RegistroPruebaDinamicaUpdHlp que tiene los datos anteriores</param>
      public void Action(IDataContext dctx, RegistroPruebaDinamica registroPrueba, RegistroPruebaDinamica anterior){
         object myFirm = new object();
         String sError = string.Empty;
         if (registroPrueba == null)
            sError += ", RegistroPruebaDinamica";
         if (anterior == null)
            sError += ", Anterior";
         if (sError.Length > 0)
            throw new Exception("RegistroPruebaDinamicaUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (anterior.RegistroPruebaID == null)
             sError += ", Anterior RegistroPruebaID";
         if (sError.Length > 0)
            throw new Exception("RegistroPruebaDinamicaUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Prueba.Diagnostico.Dinamica.DAO", 
         "RegistroPruebaDinamicaUpdHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "RegistroPruebaDinamicaUpdHlpUpdHl: Hubo un error al conectarse a la base de datos", "POV.Prueba.Diagnostico.Dinamica.DAO", 
         "RegistroPruebaDinamicaUpdHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" UPDATE RegistroPruebaDinamica ");
         if (registroPrueba.EstadoPrueba != null){
            sCmd.Append(" SET EstadoPrueba = @dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            sqlParam.Value = registroPrueba.EstadoPrueba;
            sqlParam.DbType = DbType.Byte;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (registroPrueba.FechaInicio != null){
            sCmd.Append(" ,FechaInicio = @dbp4ram2 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram2";
            sqlParam.Value = registroPrueba.FechaInicio;
            sqlParam.DbType = DbType.DateTime;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (registroPrueba.FechaFin != null){
            sCmd.Append(" ,FechaFin = @dbp4ram3 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram3";
            sqlParam.Value = registroPrueba.FechaFin;
            sqlParam.DbType = DbType.DateTime;
            sqlCmd.Parameters.Add(sqlParam);
         }
         StringBuilder s_VarWHERE = new StringBuilder();
         if (anterior.RegistroPruebaID == null)
            s_VarWHERE.Append(" RegistroPruebaID IS NULL ");
         else{ 
            // anterior.RegistroPruebaID
            s_VarWHERE.Append(" RegistroPruebaID = @dbp4ram4 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram4";
            sqlParam.Value = anterior.RegistroPruebaID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (anterior.EstadoPrueba != null){
             s_VarWHERE.Append(" AND EstadoPrueba = @dbp4ram5 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram5";
            sqlParam.Value = anterior.EstadoPrueba;
            sqlParam.DbType = DbType.Byte;
            sqlCmd.Parameters.Add(sqlParam);
         }
         string s_VarWHEREres = s_VarWHERE.ToString().Trim();
         if (s_VarWHEREres.Length > 0){
            if (s_VarWHEREres.StartsWith("AND "))
               s_VarWHEREres = s_VarWHEREres.Substring(4);
            else if (s_VarWHEREres.StartsWith("OR "))
               s_VarWHEREres = s_VarWHEREres.Substring(3);
            else if (s_VarWHEREres.StartsWith(","))
               s_VarWHEREres = s_VarWHEREres.Substring(1);
            sCmd.Append(" WHERE " + s_VarWHEREres);
         }
         int iRes = 0;
         try{
            sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
            iRes = sqlCmd.ExecuteNonQuery();
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("RegistroPruebaDinamicaUpdHlp: Hubo  un Error al Actualizar el Registro . " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("RegistroPruebaDinamicaUpdHlp: Hubo  un Error al Actualizar el Registro .");
      }
   } 
}
