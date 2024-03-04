using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Profesionalizacion.BO;

namespace POV.Profesionalizacion.DAO { 
   /// <summary>
   /// Elimina un registro de AAgrupadorContenidoDigital en la BD
   /// </summary>
   internal class AgrupadorContenidoDigitalDelHlp { 
      /// <summary>
      /// Elimina un registro de AAgrupadorContenidoDigital en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="agrupadorContenidoDigital">AAgrupadorContenidoDigital que desea eliminar</param>
      public void Action(IDataContext dctx, AAgrupadorContenidoDigital agrupadorContenidoDigital){
         object myFirm = new object();
         string sError = String.Empty;
         if (agrupadorContenidoDigital == null)
            sError += ", AAgrupadorContenidoDigital";
         if (sError.Length > 0)
            throw new Exception("AgrupadorContenidoDigitalDelHlp: Los siguientes objetos no pueden ser vacios " + sError.Substring(2));
         if (agrupadorContenidoDigital.AgrupadorContenidoDigitalID == null)
            sError += ", AgrupadorContenidoDigitalID";
         if (sError.Length > 0)
            throw new Exception("AgrupadorContenidoDigitalDelHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Profesionalizacion.DAO", 
         "AgrupadorContenidoDigitalDelHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "AgrupadorContenidoDigitalDelHlp: No se pudo conectar a la base de datos", "POV.Profesionalizacion.DAO", 
         "AgrupadorContenidoDigitalDelHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" UPDATE AgrupadorContenidoDigital ");
         sCmd.Append(" SET EstatusProfesionalizacion = @dbp4ram1 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram1";
         sqlParam.Value = EEstatusProfesionalizacion.INACTIVO;
         sqlParam.DbType = DbType.Byte;
         sqlCmd.Parameters.Add(sqlParam);

         if (agrupadorContenidoDigital.AgrupadorContenidoDigitalID == null)
             sCmd.Append(" WHERE AgrupadorContenidoDigitalID IS NULL ");
         else
         {
             // agrupadorContenidoDigital.AgrupadorContenidoDigitalID
             sCmd.Append(" WHERE AgrupadorContenidoDigitalID = @dbp4ram2 ");
             sqlParam = sqlCmd.CreateParameter();
             sqlParam.ParameterName = "dbp4ram2";
             sqlParam.Value = agrupadorContenidoDigital.AgrupadorContenidoDigitalID;
             sqlParam.DbType = DbType.Int64;
             sqlCmd.Parameters.Add(sqlParam);
         }
         int iRes = 0;
         try{
            sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
            iRes = sqlCmd.ExecuteNonQuery();
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("AgrupadorContenidoDigitalDelHlp: Ocurrió un error al ingresar el registro. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("AgrupadorContenidoDigitalDelHlp: Ocurrió un error al ingresar el registro.");
      }
   } 
}
