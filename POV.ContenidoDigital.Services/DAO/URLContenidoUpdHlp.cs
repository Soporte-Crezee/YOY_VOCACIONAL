using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.ContenidosDigital.BO;

namespace POV.ContenidosDigital.DAO { 
   /// <summary>
   /// Actualiza un registro de URLContenido en la BD
   /// </summary>
   internal class URLContenidoUpdHlp { 
      /// <summary>
      /// Actualiza de manera optimista un registro de URLContenidoUpdHlp en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="uRLContenidoUpdHlp">URLContenidoUpdHlp que tiene los datos nuevos</param>
      /// <param name="anterior">URLContenidoUpdHlp que tiene los datos anteriores</param>
      public void Action(IDataContext dctx, URLContenido uRLContenido, URLContenido anterior){
         object myFirm = new object();
         String sError = string.Empty;
         if (uRLContenido == null)
            sError += ", URLContenido";
         if (anterior == null)
            sError += ", Anterior";
         if (sError.Length > 0)
            throw new Exception("URLContenidoUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (anterior.URLContenidoID == null)
            sError += ", Anterior URLContenidoID";
         if (sError.Length > 0)
            throw new Exception("URLContenidoUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.ContenidosDigital.DAO", 
         "URLContenidoUpdHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "URLContenidoUpdHlpUpdHl: Hubo un error al conectarse a la base de datos", "POV.ContenidosDigital.DAO", 
         "URLContenidoUpdHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" UPDATE URLContenido ");
         if (uRLContenido.URL == null)
            sCmd.Append(" SET URL = NULL ");
         else{ 
            // uRLContenido.URL
            sCmd.Append(" SET URL = @dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            sqlParam.Value = uRLContenido.URL;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (uRLContenido.EsPredeterminada == null)
            sCmd.Append(" ,EsPredeterminada = NULL ");
         else{ 
            // uRLContenido.EsPredeterminada
            sCmd.Append(" ,EsPredeterminada = @dbp4ram2 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram2";
            sqlParam.Value = uRLContenido.EsPredeterminada;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (uRLContenido.Nombre == null)
            sCmd.Append(" ,Nombre = NULL ");
         else{ 
            // uRLContenido.Nombre
            sCmd.Append(" ,Nombre = @dbp4ram3 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram3";
            sqlParam.Value = uRLContenido.Nombre;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (uRLContenido.Activo == null)
            sCmd.Append(" ,Activo = NULL ");
         else{ 
            // uRLContenido.Activo
            sCmd.Append(" ,Activo = @dbp4ram4 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram4";
            sqlParam.Value = uRLContenido.Activo;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
         }
         StringBuilder s_VarWHERE = new StringBuilder();
         if (anterior.URLContenidoID != null){
            s_VarWHERE.Append(" URLContenidoID = @dbp4ram5 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram5";
            sqlParam.Value = anterior.URLContenidoID;
            sqlParam.DbType = DbType.Int64;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (anterior.URL != null){
            s_VarWHERE.Append(" AND URL = @dbp4ram6 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram6";
            sqlParam.Value = anterior.URL;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (anterior.EsPredeterminada != null){
            s_VarWHERE.Append(" AND EsPredeterminada = @dbp4ram7 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram7";
            sqlParam.Value = anterior.EsPredeterminada;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (anterior.Nombre != null){
            s_VarWHERE.Append(" AND Nombre = @dbp4ram8 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram8";
            sqlParam.Value = anterior.Nombre;
            sqlParam.DbType = DbType.String;
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
            throw new Exception("URLContenidoUpdHlp: Hubo  un Error al Actualizar el Registro . " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("URLContenidoUpdHlp: Hubo  un Error al Actualizar el Registro .");
      }
   } 
}
