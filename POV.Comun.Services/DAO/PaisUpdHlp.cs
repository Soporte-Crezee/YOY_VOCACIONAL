using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Comun.BO;

namespace POV.Comun.DAO
{ 
   /// <summary>
   /// Actualiza un País en la base de datos
   /// </summary>
   public class PaisUpdHlp { 
      /// <summary>
      /// Actualiza de manera optimista un registro de País en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="país">País que tiene los datos nuevos</param>
      /// <param name="anterior">País que tiene los datos anteriores</param>
      public void Action(IDataContext dctx, Pais pais, Pais anterior){
         object myFirm = new object();
         string sError = "";
         if (pais == null)
            sError += ", País";
         if (anterior == null)
            sError += ", País anterior";
         if (sError.Length > 0)
            throw new Exception("PaisUpdHlp: Los siguientes campos no pueden ser vacíos " + sError.Substring(2));
         if (pais.PaisID == null)
            sError += ", PaisID";
         if (pais.Nombre == null || pais.Nombre.Trim().Length == 0)
            sError += ", NombrePais";
         if (pais.FechaRegistro == null)
            sError += ", Fecha Registro";
         if (anterior.PaisID == null)
            sError += ", Anterior PaisID";
         if (anterior.Nombre == null || anterior.Nombre.Trim().Length == 0)
            sError += ", Anterior Nombre Pais";
         if (anterior.FechaRegistro == null)
            sError += ", Anterior Fecha Registro";
         if (sError.Length > 0)
            throw new Exception("PaisUpdHlp: Los siguientes campos no pueden ser vacíos " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "VITADAT.Comun.DAO", 
         "PaisUpdHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "PaisUpdHlp: Hubo un error al conectarse a la base de datos", "VITADAT.Comun.DAO", 
         "PaisUpdHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" UPDATE PAIS ");
         if (pais.Nombre == null)
            sCmd.Append(" SET NOMBRE = NULL ");
         else{ 
            sCmd.Append(" SET NOMBRE = @pais_Nombre ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "pais_Nombre";
            sqlParam.Value = pais.Nombre;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (pais.Codigo == null)
             sCmd.Append(" , Codigo = NULL ");
         else
         {
             sCmd.Append(" , Codigo = @anterior_Codigo ");
             sqlParam = sqlCmd.CreateParameter();
             sqlParam.ParameterName = "anterior_Codigo";
             sqlParam.Value = pais.Codigo;
             sqlParam.DbType = DbType.String;
             sqlCmd.Parameters.Add(sqlParam);
         }
         if (anterior.PaisID == null)
            sCmd.Append(" WHERE PAISID IS NULL ");
         else{ 
            sCmd.Append(" WHERE PAISID = @anterior_PaisID ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "anterior_PaisID";
            sqlParam.Value = anterior.PaisID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (anterior.Nombre == null)
            sCmd.Append(" AND NOMBRE IS NULL ");
         else{ 
            sCmd.Append(" AND NOMBRE = @anterior_Nombre ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "anterior_Nombre";
            sqlParam.Value = anterior.Nombre;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (anterior.FechaRegistro == null)
            sCmd.Append(" AND FECHAREGISTRO IS NULL ");
         else{ 
            sCmd.Append(" AND FECHAREGISTRO = @anterior_FechaRegistro ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "anterior_FechaRegistro";
            sqlParam.Value = anterior.FechaRegistro;
            sqlParam.DbType = DbType.DateTime;
            sqlCmd.Parameters.Add(sqlParam);
         }
         
         int iRes = 0;
         try{
            sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
            iRes = sqlCmd.ExecuteNonQuery();
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("PaisUpdHlp: Hubo un error al actualizar el registro o fue modificado mientras era editado. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("PaisUpdHlp: Hubo un error al actualizar el registro o fue modificado mientras era editado.");
      }
   } 
}
