using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.CentroEducativo.BO;
using POV.CentroEducativo.DAO;

namespace POV.CentroEducativo.DAO { 
   /// <summary>
   /// Actualiza un registro de Zona en la BD
   /// </summary>
   public class ZonaUpdHlp { 
      /// <summary>
      /// Actualiza de manera optimista un registro de ZonaUpdHlp en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="zonaUpdHlp">ZonaUpdHlp que tiene los datos nuevos</param>
      /// <param name="anterior">ZonaUpdHlp que tiene los datos anteriores</param>
      public void Action(IDataContext dctx, Zona zona, Zona anterior){
         object myFirm = new object();
         String sError = string.Empty;
         if (zona == null)
            sError += ", Zona";
         if (anterior == null)
            sError += ", Anterior";
         if (sError.Length > 0)
            throw new Exception("ZonaUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (anterior.ZonaID == null)
            sError += ", Anterior ZonaID";
         if (sError.Length > 0)
            throw new Exception("ZonaUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.CentroEducativo.DAO", 
         "ZonaUpdHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "ZonaUpdHlpUpdHl: Hubo un error al conectarse a la base de datos", "POV.CentroEducativo.DAO", 
         "ZonaUpdHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" UPDATE Zona ");
         if (zona.Clave == null)
            sCmd.Append(" SET Clave = NULL ");
         else{ 
            // zona.Clave
            sCmd.Append(" SET Clave = @dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            sqlParam.Value = zona.Clave;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (zona.Nombre == null)
            sCmd.Append(" ,Nombre = NULL ");
         else{ 
            // zona.Nombre
            sCmd.Append(" ,Nombre = @dbp4ram2 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram2";
            sqlParam.Value = zona.Nombre;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (zona.UbicacionID.UbicacionID == null)
            sCmd.Append(" ,UbicacionID = NULL ");
         else{ 
            // zona.UbicacionID.UbicacionID
            sCmd.Append(" ,UbicacionID = @dbp4ram3 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram3";
            sqlParam.Value = zona.UbicacionID.UbicacionID;
            sqlParam.DbType = DbType.Int64;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (anterior.ZonaID == null)
            sCmd.Append(" WHERE zonaID IS NULL ");
         else{ 
            // anterior.ZonaID
            sCmd.Append(" WHERE zonaID = @dbp4ram4 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram4";
            sqlParam.Value = anterior.ZonaID;
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
            throw new Exception("ZonaUpdHlp: Hubo  un Error al Actualizar el Registro . " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("ZonaUpdHlp: Hubo  un Error al Actualizar el Registro .");
      }
   } 
}
