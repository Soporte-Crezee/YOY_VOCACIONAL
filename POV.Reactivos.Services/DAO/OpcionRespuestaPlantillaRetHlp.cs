using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Reactivos.BO;
using POV.Reactivos.DAO;

namespace POV.Reactivos.DAO { 
   /// <summary>
   /// Consulta un registro de OpcionRespuestaPlantilla en la BD
   /// </summary>
   public class OpcionRespuestaPlantillaRetHlp { 
      /// <summary>
      /// Consulta registros de OpcionRespuestaPlantilla en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="opcionRespuestaPlantilla">OpcionRespuestaPlantilla que provee el criterio de selección para realizar la consulta</param>
      /// <returns>El DataSet que contiene la información de OpcionRespuestaPlantilla generada por la consulta</returns>
      public DataSet Action(IDataContext dctx, OpcionRespuestaPlantilla opcionRespuestaPlantilla, RespuestaPlantilla respuestaPlantilla){
         object myFirm = new object();
         string sError = String.Empty;
      if (respuestaPlantilla == null) {
         respuestaPlantilla = new RespuestaPlantillaOpcionMultiple();
      }
         if (opcionRespuestaPlantilla == null)
            sError += ", OpcionRespuestaPlantilla";
         if (sError.Length > 0)
            throw new Exception("OpcionRespuestaPlantillaRetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Reactivos.DAO", 
         "OpcionRespuestaPlantillaRetHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "OpcionRespuestaPlantillaRetHlp: No se pudo conectar a la base de datos", "POV.Reactivos.DAO", 
         "OpcionRespuestaPlantillaRetHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" SELECT OpcionRespuestaPlantillaID, Texto, ImagenUrl, EsPredeterminado, EsOpcionCorrecta, RespuestaPlantillaID, Activo ");
         sCmd.Append(" FROM OpcionRespuestaPlantilla ");
         StringBuilder s_VarWHERE = new StringBuilder();
         if (opcionRespuestaPlantilla.OpcionRespuestaPlantillaID != null){
            s_VarWHERE.Append(" OpcionRespuestaPlantillaID = @dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            sqlParam.Value = opcionRespuestaPlantilla.OpcionRespuestaPlantillaID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (opcionRespuestaPlantilla.Texto != null){
            s_VarWHERE.Append(" AND Texto = @dbp4ram2 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram2";
            sqlParam.Value = opcionRespuestaPlantilla.Texto;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (opcionRespuestaPlantilla.ImagenUrl != null){
            s_VarWHERE.Append(" AND ImagenUrl = @dbp4ram3 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram3";
            sqlParam.Value = opcionRespuestaPlantilla.ImagenUrl;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (opcionRespuestaPlantilla.EsPredeterminado != null){
            s_VarWHERE.Append(" AND EsPredeterminado = @dbp4ram4 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram4";
            sqlParam.Value = opcionRespuestaPlantilla.EsPredeterminado;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (opcionRespuestaPlantilla.EsOpcionCorrecta != null){
            s_VarWHERE.Append(" AND EsOpcionCorrecta = @dbp4ram5 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram5";
            sqlParam.Value = opcionRespuestaPlantilla.EsOpcionCorrecta;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (respuestaPlantilla.RespuestaPlantillaID != null){
            s_VarWHERE.Append(" AND RespuestaPlantillaID = @dbp4ram6 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram6";
            sqlParam.Value = respuestaPlantilla.RespuestaPlantillaID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (opcionRespuestaPlantilla.Activo != null){
            s_VarWHERE.Append(" AND Activo = @dbp4ram7 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram7";
            sqlParam.Value = opcionRespuestaPlantilla.Activo;
            sqlParam.DbType = DbType.Boolean;
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
            sqlAdapter.Fill(ds, "OpcionRespuestaPlantilla");
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("OpcionRespuestaPlantillaRetHlp: Se encontraron problemas al recuperar los datos. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         return ds;
      }
   } 
}
