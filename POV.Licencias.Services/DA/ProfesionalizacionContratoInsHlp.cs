using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Licencias.BO;
using POV.Profesionalizacion.BO;

namespace POV.Licencias.DA { 
   /// <summary>
   /// Guarda un registro de Contrato y EjeTematico en la BD
   /// </summary>
   internal class ProfesionalizacionContratoInsHlp { 
      /// <summary>
      /// Crea un registro de ProfesionalizacionContratoInsHlp en la base de datos
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="profesionalizacionContratoInsHlp">ProfesionalizacionContratoInsHlp que desea crear</param>
      public void Action(IDataContext dctx, Contrato contrato, EjeTematico ejeTematico){
         object myFirm = new object();
         string sError = String.Empty;
         if (contrato == null)
            sError += ", contrato";
         if (ejeTematico == null)
            sError += ", ejeTematico";
         if (sError.Length > 0)
            throw new Exception("ProfesionalizacionContratoInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Licencias.DA", 
         "ProfesionalizacionContratoInsHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "ProfesionalizacionContratoInsHlp: No se pudo conectar a la base de datos", "POV.Licencias.DA", 
         "ProfesionalizacionContratoInsHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" INSERT INTO EjeContrato (ContratoID,EjeTematicoID) ");
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
         // ejeTematicoID.ContratoID
         sCmd.Append(" ,@dbp4ram2 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram2";
         if (ejeTematico.EjeTematicoID == null)
            sqlParam.Value = DBNull.Value;
         else
             sqlParam.Value = ejeTematico.EjeTematicoID;
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
            throw new Exception("ProfesionalizacionContratoInsHlp: Ocurrió un error al ingresar el registro. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("ProfesionalizacionContratoInsHlp: Ocurrió un error al ingresar el registro.");
      }
   } 
}
