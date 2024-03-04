using System;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Seguridad.BO;

namespace POV.Seguridad.DAO
{ 
   /// <summary>
   /// Actualiza una Acción en la base de datos
   /// </summary>
   public class AccionUpdHlp { 
      /// <summary>
      /// Actualiza de manera optimista un registro de Acción en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="acción">Acción que tiene los datos nuevos</param>
      /// <param name="anterior">Acción que tiene los datos anteriores</param>
      public void Action(IDataContext dctx, Accion accion, Accion anterior){
         object myFirm = new object();
         string sError = "";
         if (accion == null)
            sError += ", Acción";
         if (anterior == null)
            sError += ", Acción anterior";
         if (sError.Length > 0)
            throw new Exception("Error DA1800.- AccionUpdHlp: Los siguientes campos no pueden ser vacíos " + sError.Substring(2));
         if (accion.AccionID == null)
            sError += ", AccionID";
         if (accion.Nombre == null || accion.Nombre.Trim().Length == 0)
             sError += ", Nombre";
         if (anterior.AccionID == null)
            sError += ", AccionID anterior";
         if (anterior.Nombre == null || anterior.Nombre.Trim().Length == 0)
             sError += ", Nombre anterior";
         if (sError.Length > 0)
             throw new Exception("AccionUpdHlp: Los siguientes campos no pueden ser vacíos " + sError.Substring(2));
         if (dctx == null)
             throw new Exception("AccionUpdHlp: DataContext no puede ser nulo");
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            dctx.BeginTransaction(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
             throw new Exception("AccionUpdHlp: Hubo un error al conectarse a la base de datos" + Environment.NewLine + ex.Message);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" UPDATE Accion ");
         if (accion.Nombre == null)
            sCmd.Append(" SET Nombre = NULL ");
         else{ 
            sCmd.Append(" SET Nombre = @accion_Nombre ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "accion_Nombre";
            sqlParam.Value = accion.Nombre;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (accion.Activo == null)
             sCmd.Append(" ,Activo = NULL ");
         else
         {
             sCmd.Append(" ,Activo = @accion_Ac ");
             sqlParam = sqlCmd.CreateParameter();
             sqlParam.ParameterName = "accion_Ac";
             sqlParam.Value = accion.Nombre;
             sqlParam.DbType = DbType.String;
             sqlCmd.Parameters.Add(sqlParam);
         }
         if (anterior.AccionID == null)
            sCmd.Append(" WHERE AccionID IS NULL ");
         else{ 
            sCmd.Append(" WHERE AccionID = @anterior_AccionID ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "anterior_AccionID";
            sqlParam.Value = anterior.AccionID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (anterior.Nombre == null)
            sCmd.Append(" AND Nombre IS NULL ");
         else{ 
            sCmd.Append(" AND Nombre = @anterior_Nombre ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "anterior_Nombre";
            sqlParam.Value = anterior.Nombre;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
         }
         int iRes = 0;
         try{
            sqlCmd.CommandText = sCmd.ToString();
            iRes = sqlCmd.ExecuteNonQuery();
            dctx.CommitTransaction(myFirm);
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.RollbackTransaction(myFirm); } catch(Exception){ }
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("AccionUpdHlp: Hubo un error al actualizar el registro o fue modificado mientras era editado. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
             throw new Exception("Error DA1485.- AccionUpdHlp: Hubo un error al actualizar el registro o fue modificado mientras era editado.");
      }
   } 
}
