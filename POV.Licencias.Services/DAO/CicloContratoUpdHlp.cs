using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Licencias.BO;
using POV.CentroEducativo.BO;

namespace POV.Licencias.DAO { 
   /// <summary>
   /// Actualiza un registro de CicloContrato en la BD
   /// </summary>
   public class CicloContratoUpdHlp { 
      /// <summary>
      /// Actualiza de manera optimista un registro de CicloContratoUpdHlp en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="cicloContratoUpdHlp">CicloContratoUpdHlp que tiene los datos nuevos</param>
      /// <param name="anterior">CicloContratoUpdHlp que tiene los datos anteriores</param>
      public void Action(IDataContext dctx, CicloContrato cicloContrato, CicloContrato anterior){
         object myFirm = new object();
         String sError = string.Empty;
         if (cicloContrato == null)
            sError += ", CicloContrato";
         if (anterior == null)
            sError += ", Anterior";
         if (sError.Length > 0)
            throw new Exception("CicloContratoUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (anterior.CicloContratoID == null)
            sError += ", Anterior CicloContratoID";
         if (sError.Length > 0)
            throw new Exception("CicloContratoUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Licencias.DAO", 
         "CicloContratoUpdHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "CicloContratoUpdHlpUpdHl: Hubo un error al conectarse a la base de datos", "POV.Licencias.DAO", 
         "CicloContratoUpdHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" UPDATE CicloContrato ");
         if (cicloContrato.EstaLiberado == null)
            sCmd.Append(" SET EstaLiberado = NULL ");
         else{ 
            // cicloContrato.EstaLiberado
            sCmd.Append(" SET EstaLiberado = @dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            sqlParam.Value = cicloContrato.EstaLiberado;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (cicloContrato.Activo == null)
            sCmd.Append(" ,Activo = NULL ");
         else{ 
            // cicloContrato.Activo
            sCmd.Append(" ,Activo = @dbp4ram2 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram2";
            sqlParam.Value = cicloContrato.Activo;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (anterior.CicloContratoID == null)
            sCmd.Append(" WHERE CicloContratoID IS NULL ");
         else{ 
            // anterior.CicloContratoID
            sCmd.Append(" WHERE CicloContratoID = @dbp4ram3 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram3";
            sqlParam.Value = anterior.CicloContratoID;
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
            throw new Exception("CicloContratoUpdHlp: Hubo  un Error al Actualizar el Registro . " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("CicloContratoUpdHlp: Hubo  un Error al Actualizar el Registro .");
      }
   } 
}
