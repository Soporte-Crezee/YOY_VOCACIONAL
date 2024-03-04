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
   /// Guarda un registro de URLContenido en la BD
   /// </summary>
   internal class URLContenidoInsHlp { 
      /// <summary>
      /// Crea un registro de URLContenido en la base de datos
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="uRLContenido">URLContenido que desea crear</param>
      public void Action(IDataContext dctx, ContenidoDigital contenidoDigital, URLContenido uRlContenido){
         object myFirm = new object();
         string sError = String.Empty;
         if (uRlContenido == null)
            sError += ", URLContenido";
         if (sError.Length > 0)
            throw new Exception("URLContenidoInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (contenidoDigital == null)
            sError += ", ContenidoDigital";
         if (sError.Length > 0)
            throw new Exception("URLContenidoInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (contenidoDigital.ContenidoDigitalID == null)
            sError += ", ContenidoDigitalID";
         if (sError.Length > 0)
            throw new Exception("URLContenidoInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (uRlContenido.URL == null || uRlContenido.URL.Trim().Length == 0)
            sError += ", URL";
         if (uRlContenido.EsPredeterminada == null)
            sError += ", EsPredeterminada";
         if (uRlContenido.Nombre == null || uRlContenido.Nombre.Trim().Length == 0)
            sError += ", Nombre";
         if (uRlContenido.FechaRegistro == null)
            sError += ", FechaRegistro";
         if (uRlContenido.Activo == null)
            sError += ", Activo";
         if (sError.Length > 0)
            throw new Exception("URLContenidoInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.ContenidosDigital.DAO", 
         "URLContenidoInsHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "URLContenidoInsHlp: No se pudo conectar a la base de datos", "POV.ContenidosDigital.DAO", 
         "URLContenidoInsHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" INSERT INTO URLContenido (ContenidoDigitalID, URL, EsPredeterminada, Nombre, FechaRegistro, Activo) ");
         sCmd.Append(" VALUES ");
         sCmd.Append(" ( ");
         // contenidoDigital.ContenidoDigitalID
         sCmd.Append(" @dbp4ram1 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram1";
         if (contenidoDigital.ContenidoDigitalID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = contenidoDigital.ContenidoDigitalID;
         sqlParam.DbType = DbType.Int64;
         sqlCmd.Parameters.Add(sqlParam);
         // uRlContenido.URL
         sCmd.Append(" ,@dbp4ram2 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram2";
         if (uRlContenido.URL == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = uRlContenido.URL;
         sqlParam.DbType = DbType.String;
         sqlCmd.Parameters.Add(sqlParam);
         // uRlContenido.EsPredeterminada
         sCmd.Append(" ,@dbp4ram3 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram3";
         if (uRlContenido.EsPredeterminada == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = uRlContenido.EsPredeterminada;
         sqlParam.DbType = DbType.Boolean;
         sqlCmd.Parameters.Add(sqlParam);
         // uRlContenido.Nombre
         sCmd.Append(" ,@dbp4ram4 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram4";
         if (uRlContenido.Nombre == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = uRlContenido.Nombre;
         sqlParam.DbType = DbType.String;
         sqlCmd.Parameters.Add(sqlParam);
         // uRlContenido.FechaRegistro
         sCmd.Append(" ,@dbp4ram5 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram5";
         if (uRlContenido.FechaRegistro == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = uRlContenido.FechaRegistro;
         sqlParam.DbType = DbType.DateTime;
         sqlCmd.Parameters.Add(sqlParam);
         // uRlContenido.Activo
         sCmd.Append(" ,@dbp4ram6 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram6";
         if (uRlContenido.Activo == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = uRlContenido.Activo;
         sqlParam.DbType = DbType.Boolean;
         sqlCmd.Parameters.Add(sqlParam);
         sCmd.Append(" ) ");
         int iRes = 0;
         try{
            sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
            iRes = sqlCmd.ExecuteNonQuery();
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("URLContenidoInsHlp: Ocurrió un error al ingresar el registro. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("URLContenidoInsHlp: Ocurrió un error al ingresar el registro.");
      }
   } 
}
