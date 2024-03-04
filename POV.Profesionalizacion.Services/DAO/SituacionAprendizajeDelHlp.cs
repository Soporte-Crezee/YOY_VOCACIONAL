using System;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Profesionalizacion.BO;

namespace POV.Profesionalizacion.DAO { 
   /// <summary>
   /// Actualiza en la base de datos
   /// </summary>
   public class SituacionAprendizajeDelHlp {
       /// <summary>
       /// Actualiza de manera optimista un registro de situacionAprendizaje en la base de datos.
       /// </summary>
       /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
       /// <param name="ejeTematico">EjeTematico al que pertenece la situación</param>
       /// <param name="situacionAprendizaje">situacionAprendizaje que tiene los datos nuevos</param>
       /// <param name="anterior">situacionAprendizaje que tiene los datos anteriores</param>
       public void Action(IDataContext dctx, EjeTematico ejeTematico,SituacionAprendizaje situacionAprendizaje){
         object myFirm = new object();
         string sError = String.Empty;
         if (situacionAprendizaje == null)
            sError += ", SituacionAprendizaje";
         if (ejeTematico == null)
            sError += ", ejeTematico";
         if (sError.Length > 0)
            throw new Exception("SituacionAprendizajeDelHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (situacionAprendizaje.SituacionAprendizajeID == null)
            sError += ", SituacionAprendizajeID";
         if (ejeTematico.EjeTematicoID == null)
            sError += ", EjeTematicoID";
         if (sError.Length > 0)
            throw new Exception("SituacionAprendizajeDelHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Profesionalizacion.DAO", 
         "SituacionAprendizajeDelHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "SituacionAprendizajeDelHlp: Ocurrió un error al conectarse a la base de datos", "POV.Profesionalizacion.DAO", 
         "SituacionAprendizajeDelHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" UPDATE SITUACIONAPRENDIZAJE ");
         // situacionAprendizaje.EstatusProfesionalizacion
         sCmd.Append(" SET ESTATUSPROFESIONALIZACION =@dbp4ram1 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram1";
         sqlParam.Value = EEstatusProfesionalizacion.INACTIVO;
         sqlParam.DbType = DbType.Byte;
         sqlCmd.Parameters.Add(sqlParam);
         if (situacionAprendizaje.SituacionAprendizajeID == null)
            sCmd.Append(" WHERE SITUACIONAPRENDIZAJEID IS NULL ");
         else{ 
            // situacionAprendizaje.SituacionAprendizajeID
            sCmd.Append(" WHERE SITUACIONAPRENDIZAJEID = @dbp4ram2 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram2";
            sqlParam.Value = situacionAprendizaje.SituacionAprendizajeID;
            sqlParam.DbType = DbType.Int64;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (ejeTematico.EjeTematicoID == null)
            sCmd.Append(" AND EJETEMATICOID IS NULL ");
         else{ 
            // ejeTematico.EjeTematicoID
            sCmd.Append(" AND EJETEMATICOID = @dbp4ram3 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram3";
            sqlParam.Value = ejeTematico.EjeTematicoID;
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
            throw new Exception("SituacionAprendizajeDelHlp: Ocurrio un error al actualizar el situacionAprendizaje o fue modificado mientras era editado. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("SituacionAprendizajeDelHlp: Ocurrio un error al actualizar el situacionAprendizaje o fue modificado mientras era editado.");
      }
   } 
}
