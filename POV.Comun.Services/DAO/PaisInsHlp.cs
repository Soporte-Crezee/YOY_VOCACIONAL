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
   /// Crea un País en la base de datos
   /// </summary>
   public class PaisInsHlp { 
      /// <summary>
      /// Crea un registro de País en la base de datos
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="país">País que desea crear</param>
      public void Action(IDataContext dctx, Pais pais){
         object myFirm = new object();
         string sError = "";
         if (pais == null)
            sError += ", País";
         if (sError.Length > 0)
            throw new Exception("PaisInsHlp: Los siguientes campos no pueden ser vacíos: " + sError.Substring(2));
         if (pais.Nombre == null || pais.Nombre.Trim().Length == 0)
            sError += ", Nombre";
         if (pais.FechaRegistro == null)
            sError += ", Fecha Registro";
         if (sError.Length > 0)
            throw new Exception("PaisInsHlp: Los siguientes campos no pueden ser vacíos: " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "VITADAT.Comun.DAO", 
         "PaisInsHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "PaisInsHlp: Hubo un error al conectarse a la base de datos", "VITADAT.Comun.DAO", 
         "PaisInsHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" INSERT INTO PAIS(NOMBRE, FECHAREGISTRO,CODIGO) ");
         sCmd.Append(" VALUES(@pais_Nombre ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "pais_Nombre";
         if (pais.Nombre == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = pais.Nombre;
         sqlParam.DbType = DbType.String;
         sqlCmd.Parameters.Add(sqlParam);
         sCmd.Append(" ,@pais_FechaRegistro ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "pais_FechaRegistro";
         if (pais.FechaRegistro == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = pais.FechaRegistro;
         sqlParam.DbType = DbType.DateTime;
         sqlCmd.Parameters.Add(sqlParam);

         sCmd.Append(" ,@pais_Codigo ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "pais_Codigo";
         if (pais.Codigo == null)
             sqlParam.Value = DBNull.Value;
         else
             sqlParam.Value = pais.Codigo;
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
            throw new Exception("PaisInsHlp: Hubo un error al crear el registro. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("PaisInsHlp: Hubo un error al crear el registro.");
      }
   } 
}
