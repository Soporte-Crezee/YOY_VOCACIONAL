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
   /// Consulta un registro de URLContenido en la BD
   /// </summary>
   internal class URLContenidoRetHlp { 
      /// <summary>
      /// Consulta registros de URLContenido en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="uRLContenido">URLContenido que provee el criterio de selección para realizar la consulta</param>
      /// <returns>El DataSet que contiene la información de URLContenido generada por la consulta</returns>
      public DataSet Action(IDataContext dctx, ContenidoDigital contenidoDigital, URLContenido uRlContenido){
         object myFirm = new object();
         string sError = String.Empty;
         if (uRlContenido == null)
            sError += ", URLContenido";
         if (sError.Length > 0)
            throw new Exception("URLContenidoRetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (contenidoDigital == null)
            sError += ", ContenidoDigital";
         if (sError.Length > 0)
            throw new Exception("URLContenidoRetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.ContenidoDigital.DAO", 
         "URLContenidoRetHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "URLContenidoRetHlp: No se pudo conectar a la base de datos", "POV.ContenidoDigital.DAO", 
         "URLContenidoRetHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" SELECT URLContenidoID, ContenidoDigitalID, URL, EsPredeterminada, Nombre, FechaRegistro, Activo ");
         sCmd.Append(" FROM URLContenido ");
         StringBuilder s_VarWHERE = new StringBuilder();
         if (uRlContenido.URLContenidoID != null){
            s_VarWHERE.Append(" URlContenidoID = @dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            sqlParam.Value = uRlContenido.URLContenidoID;
            sqlParam.DbType = DbType.Int64;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (contenidoDigital.ContenidoDigitalID != null){
            s_VarWHERE.Append(" AND ContenidoDigitalID  = @dbp4ram2 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram2";
            sqlParam.Value = contenidoDigital.ContenidoDigitalID;
            sqlParam.DbType = DbType.Int64;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (uRlContenido.URL != null){
            s_VarWHERE.Append(" AND URL  = @dbp4ram3 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram3";
            sqlParam.Value = uRlContenido.URL;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (uRlContenido.Nombre != null){
            s_VarWHERE.Append(" AND Nombre = @dbp4ram4 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram4";
            sqlParam.Value = uRlContenido.Nombre;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (uRlContenido.EsPredeterminada != null){
            s_VarWHERE.Append(" AND EsPredeterminada  = @dbp4ram5 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram5";
            sqlParam.Value = uRlContenido.EsPredeterminada;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (uRlContenido.FechaRegistro != null){
            s_VarWHERE.Append(" AND FechaRegistro = @dbp4ram6 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram6";
            sqlParam.Value = uRlContenido.FechaRegistro;
            sqlParam.DbType = DbType.DateTime;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (uRlContenido.Activo != null){
            s_VarWHERE.Append(" AND Activo = @dbp4ram7 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram7";
            sqlParam.Value = uRlContenido.Activo;
            sqlParam.DbType = DbType.Boolean;
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
         DataSet ds = new DataSet();
         DbDataAdapter sqlAdapter = dctx.CreateDataAdapter();
         sqlAdapter.SelectCommand = sqlCmd;
         try{
            sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
            sqlAdapter.Fill(ds, "URLContenido");
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("URLContenidoRetHlp: Se encontraron problemas al recuperar los datos. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         return ds;
      }
   } 
}
