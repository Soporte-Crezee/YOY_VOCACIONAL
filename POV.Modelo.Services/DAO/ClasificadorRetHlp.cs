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
   /// Consulta un registro de Clasificador en la BD
   /// </summary>
   internal class ClasificadorRetHlp { 
      /// <summary>
      /// Consulta registros de Clasificador en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="clasificador">Clasificador que provee el criterio de selección para realizar la consulta</param>
      /// <returns>El DataSet que contiene la información de Clasificador generada por la consulta</returns>
      public DataSet Action(IDataContext dctx, Clasificador clasificador, ModeloDinamico modelo){
         object myFirm = new object();
         string sError = String.Empty;
         if (clasificador == null)
            sError += ", Clasificador";
         if (modelo == null)
            sError += ", ModeloDinamico";
         if (sError.Length > 0)
            throw new Exception("ClasificadorRetHlp: Los siguientes objetos no pueden ser vacios " + sError.Substring(2));
      if (modelo == null) {
         modelo = new ModeloDinamico();
      }
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Modelo.DAO", 
         "ClasificadorRetHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "ClasificadorRetHlp: No se pudo conectar a la base de datos", "POV.Modelo.DAO", 
         "ClasificadorRetHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" SELECT ClasificadorID, ModeloID, Nombre, Descripcion, Activo, FechaRegistro ");
         sCmd.Append(" FROM Clasificador ");
         StringBuilder s_VarWHERE = new StringBuilder();
         if (clasificador.ClasificadorID != null){
            s_VarWHERE.Append(" ClasificadorID = @dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            sqlParam.Value = clasificador.ClasificadorID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (modelo.ModeloID != null){
            s_VarWHERE.Append(" AND ModeloID = @dbp4ram2 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram2";
            sqlParam.Value = modelo.ModeloID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (clasificador.Nombre != null){
            s_VarWHERE.Append(" AND Nombre LIKE @dbp4ram3 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram3";
            sqlParam.Value = clasificador.Nombre;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (clasificador.Activo != null){
            s_VarWHERE.Append(" AND Activo = @dbp4ram4 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram4";
            sqlParam.Value = clasificador.Activo;
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
            sqlAdapter.Fill(ds, "Clasificador");
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("ClasificadorRetHlp: Se encontraron problemas al recuperar los datos. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         return ds;
      }
   } 
}
