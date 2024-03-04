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
   /// Guarda un registro de TipoServicio en la BD
   /// </summary>
   public class TipoServicioInsHlp { 
      /// <summary>
      /// Crea un registro de TipoServicio en la base de datos
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="tipoServicio">TipoServicio que desea crear</param>
      public void Action(IDataContext dctx, TipoServicio tipoServicio){
         object myFirm = new object();
         string sError = String.Empty;
         if (tipoServicio == null)
            sError += ", TipoServicio";
         if (sError.Length > 0)
            throw new Exception("TipoServicioInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.CentroEducativo.DAO", 
         "TipoServicioInsHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "TipoServicioInsHlp: No se pudo conectar a la base de datos", "POV.CentroEducativo.DAO", 
         "TipoServicioInsHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" INSERT INTO TipoServicio (Clave,Nombre,NivelEducativoID) ");
         sCmd.Append(" VALUES ");
         sCmd.Append(" ( ");
         // tipoServicio.Clave
         sCmd.Append(" @dbp4ram1 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram1";
         if (tipoServicio.Clave == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = tipoServicio.Clave;
         sqlParam.DbType = DbType.String;
         sqlCmd.Parameters.Add(sqlParam);
         // tipoServicio.Nombre
         sCmd.Append(" ,@dbp4ram2 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram2";
         if (tipoServicio.Nombre == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = tipoServicio.Nombre;
         sqlParam.DbType = DbType.String;
         sqlCmd.Parameters.Add(sqlParam);
         // tipoServicio.NivelEducativoID
         sCmd.Append(" ,@dbp4ram3 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram3";
         if (tipoServicio.NivelEducativoID.NivelEducativoID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = tipoServicio.NivelEducativoID.NivelEducativoID;
         sqlParam.DbType = DbType.Int32;
         sqlCmd.Parameters.Add(sqlParam);
         sCmd.Append(" ) ");
         int iRes = 0;
         try{
            sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
            iRes = sqlCmd.ExecuteNonQuery();
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("TipoServicioInsHlp: Ocurrió un error al ingresar el registro. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("TipoServicioInsHlp: Ocurrió un error al ingresar el registro.");
      }
   } 
}
