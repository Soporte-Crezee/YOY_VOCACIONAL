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
   /// Guarda un registro de Zona en la BD
   /// </summary>
   public class ZonaInsHlp { 
      /// <summary>
      /// Crea un registro de Zona en la base de datos
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="zona">Zona que desea crear</param>
      public void Action(IDataContext dctx, Zona zona){
         object myFirm = new object();
         string sError = String.Empty;
         if (zona == null)
            sError += ", Zona";
         if (sError.Length > 0)
            throw new Exception("ZonaInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.CentroEducativo.DAO", 
         "ZonaInsHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "ZonaInsHlp: No se pudo conectar a la base de datos", "POV.CentroEducativo.DAO", 
         "ZonaInsHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" INSERT INTO Zona (Clave,Nombre,UbicacionID) ");
         sCmd.Append(" VALUES ");
         sCmd.Append(" ( ");
         // zona.Clave
         sCmd.Append(" @dbp4ram1 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram1";
         if (zona.Clave == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = zona.Clave;
         sqlParam.DbType = DbType.String;
         sqlCmd.Parameters.Add(sqlParam);
         // zona.Nombre
         sCmd.Append(" ,@dbp4ram2 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram2";
         if (zona.Nombre == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = zona.Nombre;
         sqlParam.DbType = DbType.String;
         sqlCmd.Parameters.Add(sqlParam);
         // zona.UbicacionID.UbicacionID
         sCmd.Append(" ,@dbp4ram3 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram3";
         if (zona.UbicacionID.UbicacionID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = zona.UbicacionID.UbicacionID;
         sqlParam.DbType = DbType.Int64;
         sqlCmd.Parameters.Add(sqlParam);
         sCmd.Append(" ) ");
         int iRes = 0;
         try{
            sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
            iRes = sqlCmd.ExecuteNonQuery();
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("ZonaInsHlp: Ocurrió un error al ingresar el registro. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("ZonaInsHlp: Ocurrió un error al ingresar el registro.");
      }
   } 
}
