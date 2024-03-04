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
   /// Crea un Estado en la base de datos
   /// </summary>
   public class EstadoInsHlp { 
      /// <summary>
      /// Crea un registro de Estado en la base de datos
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="estado">Estado que desea crear</param>
      public void Action(IDataContext dctx, Estado estado){
         object myFirm = new object();
         string sError = "";
         if (estado == null)
            sError += ", Estado";
         if (estado.Pais == null)
            sError += ", Pais";
         if (sError.Length > 0)
            throw new Exception("EstadoInsHlp: Los siguientes campos no pueden ser vacíos: " + sError.Substring(2));
         if (estado.Nombre == null || estado.Nombre.Trim().Length == 0)
            sError += ", Nombre Estado";
         if (estado.FechaRegistro == null)
            sError += ", Fecha Registro";
         if (estado.Pais.PaisID == null)
            sError += ", Pais ID";
         if (sError.Length > 0)
            throw new Exception("EstadoInsHlp: Los siguientes campos no pueden ser vacíos: " + sError.Substring(2));
         if (dctx == null)
             throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Comun.DAO", 
         "EstadoInsHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
             throw new StandardException(MessageType.Error, "", "EstadoInsHlp: Hubo un error al conectarse a la base de datos", "POV.Comun.DAO", 
         "EstadoInsHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" INSERT INTO ESTADO(NOMBRE, FECHAREGISTRO, PAISID, CODIGO) ");
         sCmd.Append(" VALUES(@estado_Nombre ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "estado_Nombre";
         if (estado.Nombre == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = estado.Nombre;
         sqlParam.DbType = DbType.String;
         sqlCmd.Parameters.Add(sqlParam);
         sCmd.Append(" ,@estado_FechaRegistro ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "estado_FechaRegistro";
         if (estado.FechaRegistro == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = estado.FechaRegistro;
         sqlParam.DbType = DbType.DateTime;
         sqlCmd.Parameters.Add(sqlParam);
         sCmd.Append(" ,@estado_Pais_PaisID ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "estado_Pais_PaisID";
         if (estado.Pais.PaisID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = estado.Pais.PaisID;
         sqlParam.DbType = DbType.Int32;
         sqlCmd.Parameters.Add(sqlParam);
          sCmd.Append(" ,@pais_Codigo ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "pais_Codigo";
         if (estado.Codigo == null)
             sqlParam.Value = DBNull.Value;
         else
             sqlParam.Value = estado.Codigo;
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
            throw new Exception("EstadoInsHlp: Hubo un error al crear el registro. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("EstadoInsHlp: Hubo un error al crear el registro.");
      }
   } 
}
