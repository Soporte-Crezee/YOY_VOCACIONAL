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
   /// Actualiza un registro de TipoServicio en la BD
   /// </summary>
   public class TipoServicioUpdHlp { 
      /// <summary>
      /// Actualiza de manera optimista un registro de TipoServicioUpdHlp en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="tipoServicioUpdHlp">TipoServicioUpdHlp que tiene los datos nuevos</param>
      /// <param name="anterior">TipoServicioUpdHlp que tiene los datos anteriores</param>
      public void Action(IDataContext dctx, TipoServicio tipoServicio, TipoServicio anterior){
         object myFirm = new object();
         String sError = string.Empty;
         if (tipoServicio == null)
            sError += ", TipoServicio";
         if (anterior == null)
            sError += ", Anterior";
         if (sError.Length > 0)
            throw new Exception("TipoServicioUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (anterior.TipoServicioID == null)
            sError += ", Anterior TipoServicioID";
         if (sError.Length > 0)
            throw new Exception("TipoServicioUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.CentroEducativo.DAO", 
         "TipoServicioUpdHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "TipoServicioUpdHlpUpdHl: Hubo un error al conectarse a la base de datos", "POV.CentroEducativo.DAO", 
         "TipoServicioUpdHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" UPDATE TipoServicio ");
         if (tipoServicio.Clave == null)
            sCmd.Append(" SET Clave = NULL ");
         else{ 
            // tipoServicio.Clave
            sCmd.Append(" SET Clave = @dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            sqlParam.Value = tipoServicio.Clave;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (tipoServicio.Nombre == null)
            sCmd.Append(" ,Nombre = NULL ");
         else{ 
            // tipoServicio.Nombre
            sCmd.Append(" ,Nombre = @dbp4ram2 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram2";
            sqlParam.Value = tipoServicio.Nombre;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (tipoServicio.NivelEducativoID.NivelEducativoID == null)
            sCmd.Append(" ,NivelEducativoID = NULL ");
         else{ 
            // tipoServicio.NivelEducativoID
            sCmd.Append(" ,NivelEducativoID = @dbp4ram3 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram3";
            sqlParam.Value = tipoServicio.NivelEducativoID.NivelEducativoID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (anterior.TipoServicioID == null)
            sCmd.Append(" WHERE tipoServicioID IS NULL ");
         else{ 
            // anterior.TipoServicioID
            sCmd.Append(" WHERE tipoServicioID = @dbp4ram4 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram4";
            sqlParam.Value = anterior.TipoServicioID;
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
            throw new Exception("TipoServicioUpdHlp: Hubo  un Error al Actualizar el Registro . " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("TipoServicioUpdHlp: Hubo  un Error al Actualizar el Registro .");
      }
   } 
}
