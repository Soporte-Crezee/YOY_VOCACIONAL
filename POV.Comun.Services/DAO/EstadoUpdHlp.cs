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
   /// Actualiza un Estado en la base de datos
   /// </summary>
   public class EstadoUpdHlp { 
      /// <summary>
      /// Actualiza de manera optimista un registro de Estado en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="estado">Estado que tiene los datos nuevos</param>
      /// <param name="anterior">Estado que tiene los datos anteriores</param>
      public void Action(IDataContext dctx, Estado estado, Estado anterior){
         object myFirm = new object();
         string sError = "";
         if (estado == null)
            sError += ", Estado";
         if (estado.Pais == null)
            sError += ", Pais";
         if (anterior == null)
            sError += ", Anterior Estado";
         if (anterior.Pais == null)
            sError += ", Anterior Pais";
         if (sError.Length > 0)
            throw new Exception("EstadoUpdHlp: Los siguientes campos no pueden ser vacíos " + sError.Substring(2));
         if (estado.EstadoID == null)
            sError += ", EstadoID";
         if (estado.Nombre == null || estado.Nombre.Trim().Length == 0)
            sError += ", Nombre Estado";
         if (estado.FechaRegistro == null)
            sError += ", Fecha Registro";
         if (estado.Pais.PaisID == null)
            sError += ", Pais PaisID";
         if (anterior.EstadoID == null)
            sError += ", Anterior EstadoID";
         if (anterior.Nombre == null || anterior.Nombre.Trim().Length == 0)
            sError += ", Anterior Nombre Estado";
         if (anterior.FechaRegistro == null)
            sError += ", Anterior Fecha Registro";
         if (anterior.Pais.PaisID == null)
            sError += ", Anterior PaisID";
         if (sError.Length > 0)
            throw new Exception("EstadoUpdHlp: Los siguientes campos no pueden ser vacíos " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "VITADAT.Comun.DAO", 
         "EstadoUpdHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "EstadoUpdHlp: Hubo un error al conectarse a la base de datos", "VITADAT.Comun.DAO", 
         "EstadoUpdHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" UPDATE ESTADO ");
         if (estado.Nombre == null)
            sCmd.Append(" SET NOMBRE = NULL ");
         else{ 
            sCmd.Append(" SET NOMBRE = @estado_Nombre ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "estado_Nombre";
            sqlParam.Value = estado.Nombre;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (estado.Pais.PaisID == null)
            sCmd.Append(" ,PAISID = NULL ");
         else{ 
            sCmd.Append(" ,PAISID = @estado_Pais_PaisID ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "estado_Pais_PaisID";
            sqlParam.Value = estado.Pais.PaisID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (estado.Codigo == null)
             sCmd.Append(" , Codigo = NULL ");
         else
         {
             sCmd.Append(" , Codigo = @anterior_Codigo ");
             sqlParam = sqlCmd.CreateParameter();
             sqlParam.ParameterName = "anterior_Codigo";
             sqlParam.Value = estado.Codigo;
             sqlParam.DbType = DbType.String;
             sqlCmd.Parameters.Add(sqlParam);
         }
         if (anterior.EstadoID == null)
            sCmd.Append(" WHERE ESTADOID IS NULL ");
         else{ 
            sCmd.Append(" WHERE ESTADOID = @anterior_EstadoID ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "anterior_EstadoID";
            sqlParam.Value = anterior.EstadoID;
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
         if (anterior.Pais.PaisID == null)
            sCmd.Append(" AND PAISID IS NULL ");
         else{ 
            sCmd.Append(" AND PAISID = @anterior_Pais_PaisID ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "anterior_Pais_PaisID";
            sqlParam.Value = anterior.Pais.PaisID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
         }
         int iRes = 0;
         try{
            sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
            iRes = sqlCmd.ExecuteNonQuery();
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("EstadoUpdHlp: Hubo un error al actualizar el registro o fue modificado mientras era editado. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("EstadoUpdHlp: Hubo un error al actualizar el registro o fue modificado mientras era editado.");
      }
   } 
}
