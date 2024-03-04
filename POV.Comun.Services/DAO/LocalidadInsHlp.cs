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
   /// Crea una Localidad en la base de datos
   /// </summary>
   public class LocalidadInsHlp { 
      /// <summary>
      /// Crea un registro de Localidad en la base de datos
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="localidad">Localidad que desea crear</param>
      public void Action(IDataContext dctx, Localidad localidad){
         object myFirm = new object();
         string sError = "";
         if (localidad == null)
            sError += ", Localidad";
         if (localidad.Ciudad == null)
            sError += ", Ciudad";
         if (sError.Length > 0)
            throw new Exception("LocalidadInsHlp: Los siguientes campos no pueden ser vacíos: " + sError.Substring(2));
         if (localidad.Nombre == null || localidad.Nombre.Trim().Length == 0)
            sError += ", Nombre";
         if (localidad.FechaRegistro == null)
            sError += ", Fecha Registro";
         if (localidad.Ciudad.CiudadID == null)
            sError += ", CiudadID";
         if (sError.Length > 0)
            throw new Exception("LocalidadInsHlp: Los siguientes campos no pueden ser vacíos: " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "VITADAT.Comun.DAO", 
         "LocalidadInsHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "LocalidadInsHlp: Hubo un error al conectarse a la base de datos", "VITADAT.Comun.DAO", 
         "LocalidadInsHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" INSERT INTO LOCALIDAD(NOMBRE, FECHAREGISTRO, CIUDADID, CODIGO) ");
         sCmd.Append(" VALUES(@localidad_Nombre ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "localidad_Nombre";
         if (localidad.Nombre == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = localidad.Nombre;
         sqlParam.DbType = DbType.String;
         sqlCmd.Parameters.Add(sqlParam);
         sCmd.Append(" ,@localidad_FechaRegistro ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "localidad_FechaRegistro";
         if (localidad.FechaRegistro == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = localidad.FechaRegistro;
         sqlParam.DbType = DbType.DateTime;
         sqlCmd.Parameters.Add(sqlParam);
         sCmd.Append(" ,@localidad_Ciudad_CiudadID ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "localidad_Ciudad_CiudadID";
         if (localidad.Ciudad.CiudadID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = localidad.Ciudad.CiudadID;
         sqlParam.DbType = DbType.Int32;
         sqlCmd.Parameters.Add(sqlParam);

         sCmd.Append(" ,@localidad_Codigo ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "localidad_Codigo";
         if (localidad.Codigo == null)
             sqlParam.Value = DBNull.Value;
         else
             sqlParam.Value = localidad.Codigo;
         sqlParam.DbType = DbType.String;
         sqlCmd.Parameters.Add(sqlParam);

         sCmd.Append(" ) ");
         int iRes = 0;
         try{
            sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
            iRes = sqlCmd.ExecuteNonQuery();
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("LocalidadInsHlp: Hubo un error al crear el registro. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("LocalidadInsHlp: Hubo un error al crear el registro.");
      }
   } 
}
