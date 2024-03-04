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
   /// Crea una Ciudad en la base de datos
   /// </summary>
   public class CiudadInsHlp { 
      /// <summary>
      /// Crea un registro de Ciudad en la base de datos
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="ciudad">Ciudad que desea crear</param>
      public void Action(IDataContext dctx, Ciudad ciudad){
         object myFirm = new object();
         string sError = "";
         if (ciudad == null)
            sError += ", Ciudad";
         if (ciudad.Estado == null)
            sError += ", Estado";
         if (sError.Length > 0)
            throw new Exception("CiudadInsHlp: Los siguientes campos no pueden ser vacíos: " + sError.Substring(2));
         if (ciudad.Nombre == null || ciudad.Nombre.Trim().Length == 0)
            sError += ", Nombre Ciudad";
         if (ciudad.FechaRegistro == null)
            sError += ", Fecha Registro";
         if (ciudad.Estado.EstadoID == null)
            sError += ", Estado ID";
         if (sError.Length > 0)
            throw new Exception("CiudadInsHlp: Los siguientes campos no pueden ser vacíos: " + sError.Substring(2));
         if (dctx == null)
             throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Comun.DAO", 
         "CiudadInsHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
             throw new StandardException(MessageType.Error, "", "CiudadInsHlp: Hubo un error al conectarse a la base de datos", "POV.Comun.DAO", 
         "CiudadInsHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" INSERT INTO CIUDAD(NOMBRE, FECHAREGISTRO, ESTADOID, CODIGO) ");
         sCmd.Append(" VALUES(@ciudad_Nombre ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "ciudad_Nombre";
         if (ciudad.Nombre == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = ciudad.Nombre;
         sqlParam.DbType = DbType.String;
         sqlCmd.Parameters.Add(sqlParam);
         sCmd.Append(" ,@ciudad_FechaRegistro ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "ciudad_FechaRegistro";
         if (ciudad.FechaRegistro == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = ciudad.FechaRegistro;
         sqlParam.DbType = DbType.DateTime;
         sqlCmd.Parameters.Add(sqlParam);
         sCmd.Append(" ,@ciudad_Estado_EstadoID ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "ciudad_Estado_EstadoID";
         if (ciudad.Estado.EstadoID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = ciudad.Estado.EstadoID;
         sqlParam.DbType = DbType.Int32;
         sqlCmd.Parameters.Add(sqlParam);

         sCmd.Append(" ,@ciudad_Codigo ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "ciudad_Codigo";
         if (ciudad.Codigo == null)
             sqlParam.Value = DBNull.Value;
         else
             sqlParam.Value = ciudad.Codigo;
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
            throw new Exception("CiudadInsHlp: Hubo un error al crear el registro. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("CiudadInsHlp: Hubo un error al crear el registro.");
      }
   } 
}
