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
   /// Guarda un registro de CicloContrato en la BD
   /// </summary>
   public class CicloContratoInsHlp { 
      /// <summary>
      /// Crea un registro de CicloContrato en la base de datos
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="cicloContrato">CicloContrato que desea crear</param>
      public void Action(IDataContext dctx, Contrato contrato, CicloContrato cicloContrato){
         object myFirm = new object();
         string sError = String.Empty;
         if (cicloContrato == null)
            sError += ", CicloContrato";
         if (sError.Length > 0)
            throw new Exception("CicloContratoInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (contrato == null)
            sError += ", Contrato";
         if (sError.Length > 0)
            throw new Exception("CicloContratoInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (contrato.ContratoID == null)
            sError += ", ContratoID";
         if (sError.Length > 0)
            throw new Exception("CicloContratoInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (cicloContrato.CicloEscolar == null)
            sError += ", CicloEscolar";
         if (sError.Length > 0)
            throw new Exception("CicloContratoInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (cicloContrato.CicloEscolar.CicloEscolarID == null)
            sError += ", CicloEscolarID";
         if (sError.Length > 0)
            throw new Exception("CicloContratoInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (cicloContrato.EstaLiberado == null)
            sError += ", EstaLiberado";
         if (cicloContrato.Activo == null)
            sError += ", Activo";
         if (cicloContrato.FechaRegistro == null)
            sError += ", FechaRegistro";
         if (sError.Length > 0)
            throw new Exception("CicloContratoInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Licencias.DAO", 
         "CicloContratoInsHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "CicloContratoInsHlp: No se pudo conectar a la base de datos", "POV.Licencias.DAO", 
         "CicloContratoInsHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" INSERT INTO CicloContrato (ContratoID, CicloEscolarID, EstaLiberado, Activo, FechaRegistro) ");
         sCmd.Append(" VALUES ");
         sCmd.Append(" ( ");
         // contrato.ContratoID
         sCmd.Append(" @dbp4ram1 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram1";
         if (contrato.ContratoID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = contrato.ContratoID;
         sqlParam.DbType = DbType.Int64;
         sqlCmd.Parameters.Add(sqlParam);
         // cicloContrato.CicloEscolar.CicloEscolarID
         sCmd.Append(" ,@dbp4ram2 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram2";
         if (cicloContrato.CicloEscolar.CicloEscolarID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = cicloContrato.CicloEscolar.CicloEscolarID;
         sqlParam.DbType = DbType.Int32;
         sqlCmd.Parameters.Add(sqlParam);
         // cicloContrato.EstaLiberado
         sCmd.Append(" ,@dbp4ram3 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram3";
         if (cicloContrato.EstaLiberado == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = cicloContrato.EstaLiberado;
         sqlParam.DbType = DbType.Boolean;
         sqlCmd.Parameters.Add(sqlParam);
         // cicloContrato.Activo
         sCmd.Append(" ,@dbp4ram4 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram4";
         if (cicloContrato.Activo == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = cicloContrato.Activo;
         sqlParam.DbType = DbType.Boolean;
         sqlCmd.Parameters.Add(sqlParam);
         // cicloContrato.FechaRegistro
         sCmd.Append(" ,@dbp4ram5 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram5";
         if (cicloContrato.FechaRegistro == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = cicloContrato.FechaRegistro;
         sqlParam.DbType = DbType.DateTime;
         sqlCmd.Parameters.Add(sqlParam);
         sCmd.Append(" ) ");
         int iRes = 0;
         try{
            sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
            iRes = sqlCmd.ExecuteNonQuery();
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("CicloContratoInsHlp: Ocurrió un error al ingresar el registro. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("CicloContratoInsHlp: Ocurrió un error al ingresar el registro.");
      }
   } 
}
