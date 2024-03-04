using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Profesionalizacion.BO;

namespace POV.Profesionalizacion.DA { 
   /// <summary>
   /// Consulta los registros de ContenidoDigital por AgrupadorContenidoDigital en la BD
   /// </summary>
   internal class AgrupadorContenidoDetalleRetHlp { 
      /// <summary>
      /// Consulta los registros de ContenidoDigital por AAgrupadorContenidoDigital en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="aAgrupadorContenidoDigital">AAgrupadorContenidoDigital que provee el criterio de selección para realizar la consulta</param>
      /// <returns>El DataSet que contiene la información de AAgrupadorContenidoDigital generada por la consulta</returns>
      public DataSet Action(IDataContext dctx, AAgrupadorContenidoDigital agrupadorContenido){
         object myFirm = new object();
         string sError = String.Empty;
         if (agrupadorContenido == null)
            sError += ", AAgrupadorContenidoDigital";
         if (sError.Length > 0)
            throw new Exception("AgrupadorContenidoDetalleRetHlp: Los siguientes objetos no pueden ser vacios " + sError.Substring(2));
         if (agrupadorContenido.AgrupadorContenidoDigitalID == null)
            sError += ", AgrupadorContenidoID";
         if (sError.Length > 0)
            throw new Exception("AgrupadorContenidoDetalleRetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Profesionalizacion.DAO", 
         "AgrupadorContenidoDetalleRetHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "AgrupadorContenidoDetalleRetHlp: No se pudo conectar a la base de datos", "POV.Profesionalizacion.DAO", 
         "AgrupadorContenidoDetalleRetHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" SELECT AgrupadorContenidoDigitalID, ContenidoDigitalID ");
         sCmd.Append(" FROM AgrupadorContenidoDigitalDetalle ");
         sCmd.Append(" WHERE ");
         StringBuilder s_Var = new StringBuilder();
         if (agrupadorContenido.AgrupadorContenidoDigitalID != null){
            s_Var.Append(" AgrupadorContenidoDigitalID = @dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            sqlParam.Value = agrupadorContenido.AgrupadorContenidoDigitalID;
            sqlParam.DbType = DbType.Int64;
            sqlCmd.Parameters.Add(sqlParam);
         }
         s_Var.Append("  ");
         string s_Varres = s_Var.ToString().Trim();
         if (s_Varres.Length > 0){
            if (s_Varres.StartsWith("AND "))
               s_Varres = s_Varres.Substring(4);
            else if (s_Varres.StartsWith("OR "))
               s_Varres = s_Varres.Substring(3);
            else if (s_Varres.StartsWith(","))
               s_Varres = s_Varres.Substring(1);
            sCmd.Append("  " + s_Varres);
         }
         DataSet ds = new DataSet();
         DbDataAdapter sqlAdapter = dctx.CreateDataAdapter();
         sqlAdapter.SelectCommand = sqlCmd;
         try{
            sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
            sqlAdapter.Fill(ds, "AgrupadorContenidoDetalle");
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("AgrupadorContenidoDetalleRetHlp: Se encontraron problemas al recuperar los datos. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         return ds;
      }
   } 
}
