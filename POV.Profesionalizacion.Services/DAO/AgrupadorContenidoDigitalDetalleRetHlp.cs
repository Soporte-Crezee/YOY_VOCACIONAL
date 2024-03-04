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
   /// Consulta un registro de AgrupadorContenidoDigitalDetalle en la BD
   /// </summary>
   internal class AgrupadorContenidoDigitalDetalleRetHlp 
   { 
      /// <summary>
      /// Consulta registros de aAgrupadorContenidoDigital, contenidoDigital en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="aAgrupadorContenidoDigital">aAgrupadorContenidoDigital que provee el criterio de selección para realizar la consulta</param>
      /// <param name="contenidoDigital">contenidoDigital que provee el criterio de selección para realizar la consulta</param>
      /// <returns>El DataSet que contiene la información de aAgrupadorContenidoDigital, contenidoDigital generada por la consulta</returns>
      public DataSet Action(IDataContext dctx, AAgrupadorContenidoDigital aAgrupadorContenidoDigital, ContenidoDigital contenidoDigital)
      {
         object myFirm = new object();
         string sError = String.Empty;
         if (aAgrupadorContenidoDigital == null)
            sError += ", aAgrupadorContenidoDigital";
         if (contenidoDigital == null)
            sError += ", contenidoDigital";
         if (sError.Length > 0)
            throw new Exception("AgrupadorContenidoDigitalDetalleRetHlp: Los siguientes objetos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Profesionalizacion.DAO", 
         "AgrupadorContenidoDigitalDetalleRetHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try
         {
             dctx.OpenConnection(myFirm);
             sqlCmd = dctx.CreateCommand();
         }
         catch (Exception ex)
         {
             throw new StandardException(MessageType.Error, "",
                                         "AgrupadorContenidoDigitalDetalleRetHlp: No se pudo conectar a la base de datos",
                                         "POV.Profesionalizacion.DAO",
                                         "AgrupadorContenidoDigitalDetalleRetHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" SELECT AgrupadorContenidoDigitalID, ContenidoDigitalID ");
         sCmd.Append(" FROM AgrupadorContenidoDigitalDetalle ");
         StringBuilder s_VarWHERE = new StringBuilder();
         if (aAgrupadorContenidoDigital.AgrupadorContenidoDigitalID != null){
            s_VarWHERE.Append(" AgrupadorContenidoDigitalID = @dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            sqlParam.Value = aAgrupadorContenidoDigital.AgrupadorContenidoDigitalID;
            sqlParam.DbType = DbType.Int64;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (contenidoDigital.ContenidoDigitalID != null){
            s_VarWHERE.Append(" AND ContenidoDigitalID = @dbp4ram2 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram2";
            sqlParam.Value = contenidoDigital.ContenidoDigitalID;
            sqlParam.DbType = DbType.Int64;
            sqlCmd.Parameters.Add(sqlParam);
         }
         string s_VarWHEREres = s_VarWHERE.ToString().Trim();
         if (s_VarWHEREres.Length > 0){
            if (s_VarWHEREres.StartsWith("AND "))
               s_VarWHEREres = s_VarWHEREres.Substring(4);
            else if (s_VarWHEREres.StartsWith("OR "))
               s_VarWHEREres = s_VarWHEREres.Substring(3);
            else if (s_VarWHEREres.StartsWith(","))
               s_VarWHEREres = s_VarWHEREres.Substring(1);
            sCmd.Append(" WHERE " + s_VarWHEREres);
         }
         DataSet ds = new DataSet();
         DbDataAdapter sqlAdapter = dctx.CreateDataAdapter();
         sqlAdapter.SelectCommand = sqlCmd;
         try
         {
            sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
            sqlAdapter.Fill(ds, "AgrupadorContenidoDigitalDetalle");
         } 
         catch(Exception ex)
         {
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("AgrupadorContenidoDigitalDetalleRetHlp: Se encontraron problemas al recuperar los datos. " + exmsg);
         } 
         finally
         {
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         return ds;
      }
   } 
}
