using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Modelo.BO;

namespace POV.Modelo.DAO { 
   /// <summary>
   /// Consulta un registro de PropiedadClasificador en la BD
   /// </summary>
   internal class PropiedadClasificadorRetHlp { 
      /// <summary>
      /// Consulta registros de PropiedadClasificador en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="propiedadClasificador">PropiedadClasificador que provee el criterio de selección para realizar la consulta</param>
      /// <returns>El DataSet que contiene la información de PropiedadClasificador generada por la consulta</returns>
      public DataSet Action(IDataContext dctx, Clasificador clasificador, PropiedadClasificador propiedadClasifica){
         object myFirm = new object();
         string sError = String.Empty;
         if (clasificador == null)
            sError += ", Clasificador";
         if (propiedadClasifica == null)
            sError += ", PropiedadClasificador";
         if (sError.Length > 0)
            throw new Exception("PropiedadClasificadorRetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
      if (propiedadClasifica.Propiedad == null) {
         propiedadClasifica.Propiedad = new PropiedadPersonalizada();
      }
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Modelo.DAO", 
         "PropiedadClasificadorRetHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "PropiedadClasificadorRetHlp: No se pudo conectar a la base de datos", "POV.Modelo.DAO", 
         "PropiedadClasificadorRetHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" SELECT ClasificadorID, PropiedadID, Descripcion, Activo, FechaRegistro ");
         sCmd.Append(" FROM PropiedadClasificador ");
         StringBuilder s_VarWHERE = new StringBuilder();
         if (propiedadClasifica.Propiedad.PropiedadID != null){
            s_VarWHERE.Append(" PropiedadID = @dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            sqlParam.Value = propiedadClasifica.Propiedad.PropiedadID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (clasificador.ClasificadorID != null){
            s_VarWHERE.Append(" AND ClasificadorID = @dbp4ram2 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram2";
            sqlParam.Value = clasificador.ClasificadorID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (propiedadClasifica.Activo != null){
            s_VarWHERE.Append(" AND Activo = @dbp4ram3 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram3";
            sqlParam.Value = propiedadClasifica.Activo;
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
            sqlAdapter.Fill(ds, "PropiedadClasificador");
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("PropiedadClasificadorRetHlp: Se encontraron problemas al recuperar los datos. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         return ds;
      }
   } 
}
