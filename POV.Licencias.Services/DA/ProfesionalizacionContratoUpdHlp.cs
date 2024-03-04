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
   /// Actualiza un registro de EjeContrato en la BD
   /// </summary>
    internal class ProfesionalizacionContratoUpdHlp
    { 
      /// <summary>
      /// Actualiza de manera optimista un registro de ProfesionalizacionContratoUpdHlp en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="profesionalizacionContratoUpdHlp">ProfesionalizacionContratoUpdHlp que tiene los datos nuevos</param>
      /// <param name="anterior">ProfesionalizacionContratoUpdHlp que tiene los datos anteriores</param>
      public void Action(IDataContext dctx, Contrato contrato, EjeTematico ejeTematico, Contrato contratoAnterior, EjeTematico ejeTematicoAnterior){
         object myFirm = new object();
         String sError = string.Empty;
         if (contrato == null)
            sError += ", contrato";
         if (ejeTematico == null)
            sError += ", ejeTematico";
         if (contratoAnterior == null)
            sError += ", contratoAnterior";
         if (ejeTematicoAnterior == null)
            sError += ", ejeTematicoAnterior";
         if (sError.Length > 0)
            throw new Exception("ProfesionalizacionContratoUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (contratoAnterior.ContratoID == null)
            sError += ", contratoAnterior ContradoID";
         if (ejeTematicoAnterior.EjeTematicoID == null)
            sError += ", ejeTematicoAnterior EjeTematicoID";
         if (sError.Length > 0)
            throw new Exception("ProfesionalizacionContratoUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Licencias.DA", 
         "ProfesionalizacionContratoUpdHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "ProfesionalizacionContratoUpdHlpUpdHl: Hubo un error al conectarse a la base de datos", "POV.Licencias.DA", 
         "ProfesionalizacionContratoUpdHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" UPDATE EjeContrato ");
         if (contrato.ContratoID == null)
            sCmd.Append(" SET ContradoID = NULL ");
         else{ 
            // contrato.ContradoID
            sCmd.Append(" SET ContradoID = @dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            sqlParam.Value = contrato.ContratoID;
            sqlParam.DbType = DbType.Int64;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (ejeTematico.EjeTematicoID == null)
            sCmd.Append(" ,EjeTematicoID = NULL ");
         else{ 
            // ejeTematico.EjeTematicoID
            sCmd.Append(" ,EjeTematicoID = @dbp4ram2 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram2";
            sqlParam.Value = ejeTematico.EjeTematicoID;
            sqlParam.DbType = DbType.Int64;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (contratoAnterior.ContratoID == null)
            sCmd.Append(" ,WHERE ContratoID = NULL ");
         else{ 
            // contratoAnterior.ContradoID
            sCmd.Append(" ,WHERE ContratoID = @dbp4ram3 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram3";
            sqlParam.Value = contratoAnterior.ContratoID;
            sqlParam.DbType = DbType.Int64;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (ejeTematicoAnterior.EjeTematicoID == null)
            sCmd.Append(" AND EjeTematicoID = NULL ");
         else{ 
            // ejeTematicoAnterior.EjeTematicoID
            sCmd.Append(" AND EjeTematicoID = @dbp4ram4 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram4";
            sqlParam.Value = ejeTematicoAnterior.EjeTematicoID;
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
            throw new Exception("ProfesionalizacionContratoUpdHlp: Hubo  un Error al Actualizar el Registro . " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("ProfesionalizacionContratoUpdHlp: Hubo  un Error al Actualizar el Registro .");
      }
   } 
}
