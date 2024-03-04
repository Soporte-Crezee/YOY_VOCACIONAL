using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Modelo.BO;

namespace POV.Modelo.DA { 
   /// <summary>
   /// Consultar de la base de datos un modelo
   /// </summary>
   internal class ModeloDARetHlp { 
      /// <summary>
      /// Consulta registros de Modelo en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="modelo">Modelo que provee el criterio de selección para realizar la consulta</param>
      /// <returns>El DataSet que contiene la información de Modelo generada por la consulta</returns>
       internal DataSet Action(IDataContext dctx, int? modeloID, Dictionary<string, string> parametros)
       {
         object myFirm = new object();
         string sError = "";
         
         if(parametros==null)
             sError += ", parámetros";
         if (sError.Length > 0)
            throw new Exception("ModeloRetHlp: Los siguientes campos no pueden ser vacíos " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Modelo.DAO", 
         "ModeloRetHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "ModeloRetHlp: Hubo un error al conectarse a la base de datos", "POV.Modelo.DAO", 
         "ModeloRetHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" SELECT distinct ModeloID,TipoModelo,Nombre,Descripcion,EsEditable,Activo,FechaRegistro,MetodoCalificacion ");
         sCmd.Append(" FROM Modelo ");
         StringBuilder s_VarWHERE = new StringBuilder();
         if (modeloID != null){
            s_VarWHERE.Append(" ModeloID = @dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            sqlParam.Value = modeloID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (parametros.ContainsKey("TipoModelo")){
            s_VarWHERE.Append(" AND TipoModelo = @dbp4ram2 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram2";
            sqlParam.Value = int.Parse(parametros["TipoModelo"]);
            sqlParam.DbType = DbType.Int16;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (parametros.ContainsKey("Nombre")){
            s_VarWHERE.Append(" AND Nombre LIKE @dbp4ram3 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram3";
             sqlParam.Value = parametros["Nombre"];
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (parametros.ContainsKey("EsEditable")){
            s_VarWHERE.Append(" AND EsEditable =@dbp4ram4 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram4";
             sqlParam.Value = bool.Parse(parametros["EsEditable"]);
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (parametros.ContainsKey("Activo")){
            s_VarWHERE.Append(" AND Activo = @dbp4ram5 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram5";
             sqlParam.Value = bool.Parse(parametros["Activo"]);
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
         }
          if (parametros.ContainsKey("ModeloDiagnostico"))
          {
              if (bool.Parse(parametros["ModeloDiagnostico"]))
              {
                  //Filtro estático para 
                  s_VarWHERE.Append(" AND TipoModelo <> @dbp4ram6 ");
                  sqlParam = sqlCmd.CreateParameter();
                  sqlParam.ParameterName = "dbp4ram6";
                  sqlParam.Value = (int)ETipoModelo.Estandarizado;
                  sqlParam.DbType = DbType.Int16;
                  sqlCmd.Parameters.Add(sqlParam);
              }
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
         sCmd.Append(" ORDER BY ModeloID ASC ");
         DataSet ds = new DataSet();
         DbDataAdapter sqlAdapter = dctx.CreateDataAdapter();
         sqlAdapter.SelectCommand = sqlCmd;
         try{
            sqlCmd.CommandText = sCmd.Replace("@", dctx.ParameterSymbol).ToString();
            sqlAdapter.Fill(ds, "Modelo");
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("ModeloRetHlp: Hubo un error al consultar los registros. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         return ds;
      }
   } 
}
