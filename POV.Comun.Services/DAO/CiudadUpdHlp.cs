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
   /// Actualiza un Ciudad en la base de datos
   /// </summary>
   public class CiudadUpdHlp { 
      /// <summary>
      /// Actualiza de manera optimista un registro de Ciudad en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="ciudad">Ciudad que tiene los datos nuevos</param>
      /// <param name="anterior">Ciudad que tiene los datos anteriores</param>
      public void Action(IDataContext dctx, Ciudad ciudad, Ciudad anterior){
         object myFirm = new object();
         string sError = "";
         if (ciudad == null)
            sError += ", Ciudad";
         if (ciudad.Estado == null)
            sError += ", Estado";
         if (anterior == null)
            sError += ", Anterior Ciudad";
         if (anterior.Estado == null)
            sError += ", Anterior Estado";
         if (sError.Length > 0)
            throw new Exception("CiudadUpdHlp: Los siguientes campos no pueden ser vacíos " + sError.Substring(2));
         if (ciudad.CiudadID == null)
            sError += ", CiudadID";
         if (ciudad.Nombre == null || ciudad.Nombre.Trim().Length == 0)
            sError += ", Nombre Ciudad";
         if (ciudad.FechaRegistro == null)
            sError += ", Fecha Registro";
         if (ciudad.Estado.EstadoID == null)
            sError += ", EstadoID";
         if (anterior.CiudadID == null)
            sError += ", Anterior CiudadID";
         if (anterior.Nombre == null || anterior.Nombre.Trim().Length == 0)
            sError += ", Anterior Nombre Ciudad";
         if (anterior.FechaRegistro == null)
            sError += ", Anterior Fecha Registro";
         if (anterior.Estado.EstadoID == null)
            sError += ", Anterior EstadoID";
         if (sError.Length > 0)
            throw new Exception("CiudadUpdHlp: Los siguientes campos no pueden ser vacíos " + sError.Substring(2));
         if (dctx == null)
             throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Comun.DAO", 
         "CiudadUpdHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
             throw new StandardException(MessageType.Error, "", "CiudadUpdHlp: Hubo un error al conectarse a la base de datos", "POV.Comun.DAO", 
         "CiudadUpdHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" UPDATE CIUDAD ");
         if (ciudad.Nombre == null)
            sCmd.Append(" SET NOMBRE = NULL ");
         else{ 
            sCmd.Append(" SET NOMBRE = @ciudad_Nombre ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "ciudad_Nombre";
            sqlParam.Value = ciudad.Nombre;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (ciudad.Estado.EstadoID == null)
            sCmd.Append(" ,ESTADOID = NULL ");
         else{ 
            sCmd.Append(" ,ESTADOID = @ciudad_Estado_EstadoID ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "ciudad_Estado_EstadoID";
            sqlParam.Value = ciudad.Estado.EstadoID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (ciudad.Codigo == null)
             sCmd.Append(" , Codigo = NULL ");
         else
         {
             sCmd.Append(" , Codigo = @anterior_Codigo ");
             sqlParam = sqlCmd.CreateParameter();
             sqlParam.ParameterName = "anterior_Codigo";
             sqlParam.Value = ciudad.Codigo;
             sqlParam.DbType = DbType.String;
             sqlCmd.Parameters.Add(sqlParam);
         }
         if (anterior.CiudadID == null)
            sCmd.Append(" WHERE CIUDADID IS NULL ");
         else{ 
            sCmd.Append(" WHERE CIUDADID = @anterior_CiudadID ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "anterior_CiudadID";
            sqlParam.Value = anterior.CiudadID;
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
         if (anterior.Estado.EstadoID == null)
            sCmd.Append(" AND ESTADOID IS NULL ");
         else{ 
            sCmd.Append(" AND ESTADOID = @anterior_Estado_EstadoID ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "anterior_Estado_EstadoID";
            sqlParam.Value = anterior.Estado.EstadoID;
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
            throw new Exception("CiudadUpdHlp: Hubo un error al actualizar el registro o fue modificado mientras era editado. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("CiudadUpdHlp: Hubo un error al actualizar el registro o fue modificado mientras era editado.");
      }
   } 
}
