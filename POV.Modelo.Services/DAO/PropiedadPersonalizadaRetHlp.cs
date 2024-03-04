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
   /// Consulta un registro de PropiedadPersonalizada en la BD
   /// </summary>
   internal class PropiedadPersonalizadaRetHlp { 
      /// <summary>
      /// Consulta registros de PropiedadPersonalizada en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="propiedadPersonalizada">PropiedadPersonalizada que provee el criterio de selección para realizar la consulta</param>
      /// <returns>El DataSet que contiene la información de PropiedadPersonalizada generada por la consulta</returns>
      public DataSet Action(IDataContext dctx, PropiedadPersonalizada propiedad, ModeloDinamico modelo){
         object myFirm = new object();
         string sError = String.Empty;
         if (propiedad == null)
            sError += ", PropiedadPersonalizada";
         if (modelo == null)
            sError += ", ModeloDinamico";
         if (sError.Length > 0)
            throw new Exception("PropiedadPersonalizadaRetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring(2));
         if (dctx == null)
      throw new StandardException(MessageType.Error, "", "DataContext no puede ser nulo", "POV.Modelo.DAO", 
         "PropiedadPersonalizadaRetHlp", "Action", null, null);
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
      throw new StandardException(MessageType.Error, "", "PropiedadPersonalizadaRetHlp: No se pudo conectar a la base de datos", "POV.Modelo.DAO", 
         "PropiedadPersonalizadaRetHlp", "Action", null, null);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" SELECT PropiedadID, ModeloID, Nombre, Descripcion, EsVisible, Activo, FechaRegistro ");
         sCmd.Append(" FROM PropiedadPersonalizada pp ");
         StringBuilder s_VarWHERE = new StringBuilder();
         if (propiedad.PropiedadID != null){
            s_VarWHERE.Append(" pp.PropiedadID = @dbp4ram1 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram1";
            sqlParam.Value = propiedad.PropiedadID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (modelo.ModeloID != null){
            s_VarWHERE.Append(" AND pp.ModeloID = @dbp4ram2 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram2";
            sqlParam.Value = modelo.ModeloID;
            sqlParam.DbType = DbType.Int64;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (propiedad.Nombre != null){
            s_VarWHERE.Append(" AND pp.Nombre = @dbp4ram3 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram3";
            sqlParam.Value = propiedad.Nombre;
            sqlParam.DbType = DbType.String;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (propiedad.Activo != null){
            s_VarWHERE.Append(" AND pp.Activo = @dbp4ram4 ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "dbp4ram4";
            sqlParam.Value = propiedad.Activo;
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
            sqlAdapter.Fill(ds, "PropiedadPersonalizada");
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("PropiedadPersonalizadaRetHlp: Se encontraron problemas al recuperar los datos. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         return ds;
      }
   } 
}
