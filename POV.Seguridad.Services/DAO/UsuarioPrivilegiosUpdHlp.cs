using System;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Seguridad.BO;
using POV.CentroEducativo.BO;

namespace POV.Seguridad.DAO
{ 
   /// <summary>
   /// Actualiza un UsuarioPrivilegios en la base de datos
   /// </summary>
   public class UsuarioPrivilegiosUpdHlp { 
      /// <summary>
      /// Actualiza de manera optimista un registro de UsuarioPrivilegios en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="usuarioPrivilegios">UsuarioPrivilegios que tiene los datos nuevos</param>
      /// <param name="anterior">UsuarioPrivilegios que tiene los datos anteriores</param>
      public void Action(IDataContext dctx, UsuarioPrivilegios usuarioPrivilegios, UsuarioPrivilegios anterior){
         object myFirm = new object();
         string sError = "";
         if (usuarioPrivilegios == null)
            sError += ", UsuarioPrivilegios";
         if (anterior == null)
            sError += ", UsuarioPrivilegios anterior";
         if (sError.Length > 0)
            throw new Exception("Error DA1972.- UsuarioPrivilegiosActualizar: Los siguientes campos no pueden ser vacíos " + sError.Substring(2));
         if (usuarioPrivilegios.UsuarioPrivilegiosID == null)
            sError += ", UsuarioPrivilegiosID";
         
         if (anterior.UsuarioPrivilegiosID == null)
            sError += ", UsuarioPrivilegiosID anterior";
         
         if (sError.Length > 0)
            throw new Exception("Error DA1973.- UsuarioPrivilegiosActualizar: Los siguientes campos no pueden ser vacíos " + sError.Substring(2));
         if (dctx == null)
            throw new Exception("Error DA1974.- UsuarioPrivilegiosActualizar: DataContext no puede ser nulo");
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            dctx.BeginTransaction(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
            throw new Exception("Error DA1975.- UsuarioPrivilegiosActualizar: Hubo un error al conectarse a la base de datos" + Environment.NewLine + ex.Message);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" UPDATE UsuarioPrivilegios ");
         if (usuarioPrivilegios.Estado == null)
            sCmd.Append(" SET Estado = NULL ");
         else{ 
            sCmd.Append(" SET Estado = @usuarioPrivilegios_Estado ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "usuarioPrivilegios_Estado";
            sqlParam.Value = usuarioPrivilegios.Estado;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (usuarioPrivilegios is UsuarioEscolarPrivilegios)
         {
             sCmd.Append(" ,CicloEscolarID = @dbp4ram2 ");
             sqlParam = sqlCmd.CreateParameter();
             sqlParam.ParameterName = "dbp4ram2 ";
             sqlParam.Value = (usuarioPrivilegios as UsuarioEscolarPrivilegios).CicloEscolar.CicloEscolarID;
             sqlParam.DbType = DbType.Int32;
             sqlCmd.Parameters.Add(sqlParam);
         }
         if (anterior.UsuarioPrivilegiosID == null)
            sCmd.Append(" WHERE UsuarioPrivilegiosID IS NULL ");
         else{ 
            sCmd.Append(" WHERE UsuarioPrivilegiosID = @anterior_UsuarioPrivilegiosID ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "anterior_UsuarioPrivilegiosID";
            sqlParam.Value = anterior.UsuarioPrivilegiosID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
         }
         
         if (anterior.FechaCreacion == null)
            sCmd.Append(" AND FechaCreacion IS NULL ");
         else{ 
            sCmd.Append(" AND FechaCreacion = @anterior_FechaCreacion ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "anterior_FechaCreacion";
            sqlParam.Value = anterior.FechaCreacion;
            sqlParam.DbType = DbType.DateTime;
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
            throw new Exception("Error DA1976.- UsuarioPrivilegiosActualizar: Hubo un error al actualizar el registro o fue modificado mientras era editado. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("Error DA1977.- UsuarioPrivilegiosActualizar: Hubo un error al actualizar el registro o fue modificado mientras era editado.");
      }
   } 
}
