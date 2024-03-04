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
   /// Actualiza un Perfil en la base de datos
   /// </summary>
   public class PerfilUpdHlp { 
      /// <summary>
      /// Actualiza de manera optimista un registro de Perfil en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="perfil">Perfil que tiene los datos nuevos</param>
      /// <param name="anterior">Perfil que tiene los datos anteriores</param>
      public void Action(IDataContext dctx, Perfil perfil, Perfil anterior){
         object myFirm = new object();
         string sError = "";
         if (perfil == null)
            sError += ", Perfil";
         if (anterior == null)
            sError += ", Perfil anterior";
         if (sError.Length > 0)
            throw new Exception("Error DA1844.- PerfilActualizar: Los siguientes campos no pueden ser vacíos " + sError.Substring(2));
         if (perfil.PerfilID == null)
            sError += ", PerfilID";
         if (perfil.Descripcion == null || perfil.Descripcion.Trim().Length == 0)
            sError += ", Descripcion";
        
         if (anterior.PerfilID == null)
            sError += ", PerfilID anterior";
         if (anterior.Descripcion == null || anterior.Descripcion.Trim().Length == 0)
            sError += ", Descripcion anterior";
         
         if (sError.Length > 0)
            throw new Exception("Error DA1845.- PerfilActualizar: Los siguientes campos no pueden ser vacíos " + sError.Substring(2));
         if (dctx == null)
            throw new Exception("Error DA1846.- PerfilActualizar: DataContext no puede ser nulo");
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            dctx.BeginTransaction(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
            throw new Exception("Error DA1847.- PerfilActualizar: Hubo un error al conectarse a la base de datos" + Environment.NewLine + ex.Message);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" UPDATE Perfil ");
         if (perfil.Nombre == null)
             sCmd.Append(" SET Nombre = NULL ");
         else
         {
             sCmd.Append(" SET Nombre = @perfil_Nombre ");
             sqlParam = sqlCmd.CreateParameter();
             sqlParam.ParameterName = "perfil_Nombre";
             sqlParam.Value = perfil.Nombre;
             sqlParam.DbType = DbType.String;
             sqlCmd.Parameters.Add(sqlParam);
         }
         if (perfil.Descripcion == null)
            sCmd.Append(" ,Descripcion = NULL ");
         else{ 
            sCmd.Append(" ,Descripcion = @perfil_Descripcion ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "perfil_Descripcion";
            sqlParam.Value = perfil.Descripcion;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (perfil.Operaciones == null)
             sCmd.Append(" ,Operaciones = NULL ");
         else{
             sCmd.Append(" ,Operaciones = @perfil_Operaciones ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "perfil_Operaciones";
            sqlParam.Value = perfil.Operaciones;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (perfil.Estatus == null)
             sCmd.Append(" ,Estatus = NULL ");
         else
         {
             sCmd.Append(" ,Estatus = @perfil_Estatus ");
             sqlParam = sqlCmd.CreateParameter();
             sqlParam.ParameterName = "perfil_Estatus";
             sqlParam.Value = perfil.Estatus;
             sqlParam.DbType = DbType.Boolean;
             sqlCmd.Parameters.Add(sqlParam);
         }
         if (anterior.PerfilID == null)
            sCmd.Append(" WHERE PerfilID IS NULL ");
         else{ 
            sCmd.Append(" WHERE PerfilID = @anterior_PerfilID ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "anterior_PerfilID";
            sqlParam.Value = anterior.PerfilID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (anterior.Descripcion == null)
            sCmd.Append(" AND Descripcion IS NULL ");
         else{ 
            sCmd.Append(" AND Descripcion = @anterior_Descripcion ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "anterior_Descripcion";
            sqlParam.Value = anterior.Descripcion;
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
            throw new Exception("Error DA1848.- PerfilActualizar: Hubo un error al actualizar el registro o fue modificado mientras era editado. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("Error DA1849.- PerfilActualizar: Hubo un error al actualizar el registro o fue modificado mientras era editado.");
      }
   } 
}
