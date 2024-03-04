using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Prueba.BO;
using POV.Prueba.Diagnostico.Dinamica.BO;

namespace POV.Prueba.Diagnostico.Dinamica.DA { 
   /// <summary>
   /// Consulta un registro de RespuestaPreguntaDinamica en la BD
   /// </summary>
   internal class RespuestaOpcionDinamicaDetalleDARetHlp { 
      /// <summary>
      /// Consulta registros de RespuestaPreguntaDinamica en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="respuestaPreguntaDinamica">RespuestaPreguntaDinamica que provee el criterio de selección para realizar la consulta</param>
      /// <returns>El DataSet que contiene la información de RespuestaPreguntaDinamica generada por la consulta</returns>
      public DataSet Action(IDataContext dctx, RespuestaPreguntaDinamica respuestaPreguntaDinamica){
         object myFirm = new object();
         string sError = String.Empty;
         if (respuestaPreguntaDinamica == null)
            sError += ", RespuestaPreguntaDinamica";
         if (sError.Length > 0)
            throw new Exception("RespuestaOpcionDinamicaDetalleDARetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Prueba.Diagnostico.Dinamica.DAO", 
         "RespuestaOpcionDinamicaDetalleDARetHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "RespuestaOpcionDinamicaDetalleDARetHlp: No se pudo conectar a la base de datos", "POV.Prueba.Diagnostico.Dinamica.DAO", 
         "RespuestaOpcionDinamicaDetalleDARetHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" SELECT RespuestaPreguntaID, OpcionRespuestaPlantillaID ");
         sCmd.Append(" FROM RespuestaOpcionDinamicaDetalle ");
         StringBuilder s_VarWHERE = new StringBuilder();
         if (respuestaPreguntaDinamica.RespuestaPreguntaID != null){
            s_VarWHERE.Append(" RespuestaPreguntaID = @dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            sqlParam.Value = respuestaPreguntaDinamica.RespuestaPreguntaID;
            sqlParam.DbType = DbType.Int32;
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
         try{
            sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
            sqlAdapter.Fill(ds, "Opciones");
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("RespuestaOpcionDinamicaDetalleDARetHlp: Se encontraron problemas al recuperar los datos. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         return ds;
      }
   } 
}
