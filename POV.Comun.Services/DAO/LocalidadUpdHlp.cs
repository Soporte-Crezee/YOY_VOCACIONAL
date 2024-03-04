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
   /// Actualiza una Localidad en la base de datos
   /// </summary>
   public class LocalidadUpdHlp { 
      /// <summary>
      /// Actualiza de manera optimista un registro de Localidad en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="localidad">Localidad que tiene los datos nuevos</param>
      /// <param name="anterior">Localidad que tiene los datos anteriores</param>
      public void Action(IDataContext dctx, Localidad localidad, Localidad anterior){
         object myFirm = new object();
         string sError = "";
         if (localidad == null)
            sError += ", Localidad";
         if (localidad.Ciudad == null)
            sError += ", Ciudad";
         if (anterior == null)
            sError += ", Anterior Localidad";
         if (anterior.Ciudad == null)
            sError += ", Anterior Ciudad";
         if (sError.Length > 0)
            throw new Exception("LocalidadUpdHlp: Los siguientes campos no pueden ser vacíos " + sError.Substring(2));
         if (localidad.LocalidadID == null)
            sError += ", LocalidadID";
         if (localidad.Nombre == null || localidad.Nombre.Trim().Length == 0)
            sError += ", Nombre";
         if (localidad.FechaRegistro == null)
            sError += ", Fecha Registro";
         if (localidad.Ciudad.CiudadID == null)
            sError += ", CiudadID";
         if (anterior.LocalidadID == null)
            sError += ", Anterior LocalidadID";
         if (anterior.Nombre == null || anterior.Nombre.Trim().Length == 0)
            sError += ", Anterior Nombre";
         if (anterior.FechaRegistro == null)
            sError += ", Anterior Fecha Registro";
         if (anterior.Ciudad.CiudadID == null)
            sError += ", Anterior CiudadID";
         if (sError.Length > 0)
            throw new Exception("LocalidadUpdHlp: Los siguientes campos no pueden ser vacíos " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "VITADAT.Comun.DAO", 
         "LocalidadUpdHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "LocalidadUpdHlp: Hubo un error al conectarse a la base de datos", "VITADAT.Comun.DAO", 
         "LocalidadUpdHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" UPDATE LOCALIDAD ");
         if (localidad.Nombre == null)
            sCmd.Append(" SET NOMBRE = NULL ");
         else{ 
            sCmd.Append(" SET NOMBRE = @localidad_Nombre ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "localidad_Nombre";
            sqlParam.Value = localidad.Nombre;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (localidad.Ciudad.CiudadID == null)
            sCmd.Append(" ,CIUDADID = NULL ");
         else{ 
            sCmd.Append(" ,CIUDADID = @localidad_Ciudad_CiudadID ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "localidad_Ciudad_CiudadID";
            sqlParam.Value = localidad.Ciudad.CiudadID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (localidad.Codigo == null)
             sCmd.Append(" , Codigo = NULL ");
         else
         {
             sCmd.Append(" , Codigo = @anterior_Codigo ");
             sqlParam = sqlCmd.CreateParameter();
             sqlParam.ParameterName = "anterior_Codigo";
             sqlParam.Value = localidad.Codigo;
             sqlParam.DbType = DbType.String;
             sqlCmd.Parameters.Add(sqlParam);
         }
         if (anterior.LocalidadID == null)
            sCmd.Append(" WHERE LOCALIDADID IS NULL ");
         else{ 
            sCmd.Append(" WHERE LOCALIDADID = @anterior_LocalidadID ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "anterior_LocalidadID";
            sqlParam.Value = anterior.LocalidadID;
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
         if (anterior.Ciudad.CiudadID == null)
            sCmd.Append(" AND CIUDADID IS NULL ");
         else{ 
            sCmd.Append(" AND CIUDADID = @anterior_Ciudad_CiudadID ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "anterior_Ciudad_CiudadID";
            sqlParam.Value = anterior.Ciudad.CiudadID;
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
            throw new Exception("LocalidadUpdHlp: Hubo un error al actualizar el registro o fue modificado mientras era editado. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("LocalidadUpdHlp: Hubo un error al actualizar el registro o fue modificado mientras era editado.");
      }
   } 
}
