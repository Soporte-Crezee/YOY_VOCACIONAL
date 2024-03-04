using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.ContenidosDigital.Busqueda.BO;
using POV.Profesionalizacion.BO;

namespace POV.ContenidosDigital.Busqueda.DAO 
{ 
   /// <summary>
   /// Consulta un registro de PalabraClaveContenidoDigital en la BD
   /// </summary>
   internal class PalabraClaveContenidoDigitalRetHlp 
   { 
      /// <summary>
      /// Consulta registros de palabraClaveContenidoDigital, contenidoDigitalAgrupador en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="palabraClaveContenidoDigital">palabraClaveContenidoDigital que provee el criterio de selección para realizar la consulta</param>
      /// <param name="contenidoDigitalAgrupador">contenidoDigitalAgrupador que provee el criterio de selección para realizar la consulta</param>
      /// <returns>El DataSet que contiene la información de palabraClaveContenidoDigital, contenidoDigitalAgrupador generada por la consulta</returns>
      public DataSet Action(IDataContext dctx, PalabraClaveContenidoDigital palabraClaveContenidoDigital, ContenidoDigitalAgrupador contenidoDigitalAgrupador)
      {
         object myFirm = new object();
         string sError = String.Empty;
         if (palabraClaveContenidoDigital == null)
            sError += ", palabraClaveContenidoDigital";
         if (contenidoDigitalAgrupador == null)
            sError += ", contenidoDigitalAgrupador";
         if (sError.Length > 0)
            throw new Exception("PalabraClaveContenidoDigitalRetHlp: Los siguientes objetos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.ContenidosDigital.Busqueda.DAO", 
         "PalabraClaveContenidoDigitalRetHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try
         {
             dctx.OpenConnection(myFirm);
             sqlCmd = dctx.CreateCommand();
         }
         catch (Exception ex)
         {
             throw new StandardException(MessageType.Error, "",
                                         "PalabraClaveContenidoDigitalRetHlp: No se pudo conectar a la base de datos",
                                         "POV.ContenidosDigital.Busqueda.DAO",
                                         "PalabraClaveContenidoDigitalRetHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" SELECT PalabraClaveContenidoID, ContenidoDigitalAgrupadorID ");
         sCmd.Append(" FROM PalabraClaveContenidoDigital ");
         StringBuilder s_VarWHERE = new StringBuilder();
         if (palabraClaveContenidoDigital.PalabraClaveContenidoID != null){
            s_VarWHERE.Append(" PalabraClaveContenidoID = @dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            sqlParam.Value = palabraClaveContenidoDigital.PalabraClaveContenidoID;
            sqlParam.DbType = DbType.Int64;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (contenidoDigitalAgrupador.ContenidoDigitalAgrupadorID != null){
            s_VarWHERE.Append(" AND ContenidoDigitalAgrupadorID = @dbp4ram2 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram2";
            sqlParam.Value = contenidoDigitalAgrupador.ContenidoDigitalAgrupadorID;
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
            sqlAdapter.Fill(ds, "PalabraClaveContenidoDigital");
         } 
         catch(Exception ex)
         {
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("PalabraClaveContenidoDigitalRetHlp: Se encontraron problemas al recuperar los datos. " + exmsg);
         } 
         finally
         {
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         return ds;
      }
   } 
}
