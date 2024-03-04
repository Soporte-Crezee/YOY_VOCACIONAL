using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.ContenidosDigital.BO;
using POV.Profesionalizacion.BO;

namespace POV.Profesionalizacion.DAO 
{ 
   /// <summary>
   /// Guarda un registro de AgrupadorContenidoDigitalDetalle en la BD
   /// </summary>
   internal class AgrupadorContenidoDigitalDetalleInsHlp 
   { 
      /// <summary>
      /// Crea un registro de aAgrupadorContenidoDigital, contenidoDigital en la base de datos
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="aAgrupadorContenidoDigital">aAgrupadorContenidoDigital</param>
      /// <param name="contenidoDigital">contenidoDigital</param>
      public void Action(IDataContext dctx, AAgrupadorContenidoDigital aAgrupadorContenidoDigital, ContenidoDigital contenidoDigital)
      {
         object myFirm = new object();
         string sError = String.Empty;
         if (aAgrupadorContenidoDigital == null)
            sError += ", aAgrupadorContenidoDigital";
         if (contenidoDigital == null)
            sError += ", contenidoDigital";
         if (sError.Length > 0)
            throw new Exception("AgrupadorContenidoDigitalDetalleInsHlp: Los siguientes objetos no pueden ser vacios " + sError.Substring(2));
         if (aAgrupadorContenidoDigital.AgrupadorContenidoDigitalID == null)
            sError += ", aAgrupadorContenidoDigital.AgrupadorContenidoDigitalID";
         if (contenidoDigital.ContenidoDigitalID == null)
            sError += ", contenidoDigital.ContenidoDigitalID";
         if (sError.Length > 0)
             throw new Exception("AgrupadorContenidoDigitalDetalleInsHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Profesionalizacion.DAO", 
         "AgrupadorContenidoDigitalDetalleInsHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try
         {
             dctx.OpenConnection(myFirm);
             sqlCmd = dctx.CreateCommand();
         }
         catch (Exception ex)
         {
             throw new StandardException(MessageType.Error, "",
                                         "AgrupadorContenidoDigitalDetalleInsHlp: No se pudo conectar a la base de datos",
                                         "POV.Profesionalizacion.DAO",
                                         "AgrupadorContenidoDigitalDetalleInsHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" INSERT INTO AgrupadorContenidoDigitalDetalle (AgrupadorContenidoDigitalID, ContenidoDigitalID) ");
         sCmd.Append(" VALUES ");
         sCmd.Append(" ( ");
         // aAgrupadorContenidoDigital.AgrupadorContenidoDigitalID
         sCmd.Append(" @dbp4ram1 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram1";
         if (aAgrupadorContenidoDigital.AgrupadorContenidoDigitalID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = aAgrupadorContenidoDigital.AgrupadorContenidoDigitalID;
         sqlParam.DbType = DbType.Int64;
         sqlCmd.Parameters.Add(sqlParam);
         // contenidoDigital.ContenidoDigitalID
         sCmd.Append(" ,@dbp4ram2 ");
         sqlParam = sqlCmd.CreateParameter();
         sqlParam.ParameterName = "dbp4ram2";
         if (contenidoDigital.ContenidoDigitalID == null)
            sqlParam.Value = DBNull.Value;
         else 
            sqlParam.Value = contenidoDigital.ContenidoDigitalID;
         sqlParam.DbType = DbType.Int64;
         sqlCmd.Parameters.Add(sqlParam);
         sCmd.Append(" ) ");
         int iRes = 0;
         try
         {
            sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
            iRes = sqlCmd.ExecuteNonQuery();
         } 
         catch(Exception ex)
         {
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } 
            catch(Exception){ }
            throw new Exception("AgrupadorContenidoDigitalDetalleInsHlp: Ocurrio un error al ingresar el registro. " + exmsg);
         } 
         finally
         {
            try{ dctx.CloseConnection(myFirm); } 
            catch(Exception){ }
         }
         if (iRes < 1)
            throw new Exception("AgrupadorContenidoDigitalDetalleInsHlp: Ocurrio un error al ingresar el registro.");
      }
   } 
}
