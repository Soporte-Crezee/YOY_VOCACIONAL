using System;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Seguridad.BO;

namespace POV.Seguridad.DAO
{ 
   /// <summary>
   /// Consulta registros de PerfilPermiso en la base de datos
   /// </summary>
   public class PerfilPermisoRetHlp { 
      /// <summary>
      /// Consulta registros de PerfilPermiso en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="perfilPermiso">PerfilPermiso que provee el criterio de selección para realizar la consulta</param>
      /// <returns>El DataSet que contiene la información de PerfilPermiso generada por la consulta</returns>
      public DataSet Action(IDataContext dctx, int? perfilPermisoID, int? perfilID, int? permisoID){
         object myFirm = new object();
         if (dctx == null)
            throw new Exception("Error DA1866.- PerfilPermisoConsultar: DataContext no puede ser nulo");
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
            throw new Exception("Error DA1867.- PerfilPermisoConsultar: Hubo un error al conectarse a la base de datos" + Environment.NewLine + ex.Message);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" SELECT PerfilPermisoID, PerfilID, PermisoID ");
         sCmd.Append(" FROM PerfilPermiso ");
         StringBuilder s_VarWHERE = new StringBuilder();
         if (perfilPermisoID != null){
            s_VarWHERE.Append(" PerfilPermisoID = @perfilPermisoID ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "perfilPermisoID";
            sqlParam.Value = perfilPermisoID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (perfilID != null){
            s_VarWHERE.Append(" AND PerfilID = @perfilID ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "perfilID";
            sqlParam.Value = perfilID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (permisoID != null){
            s_VarWHERE.Append(" AND PermisoID = @permisoID ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "permisoID";
            sqlParam.Value = permisoID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
         }
         string s_VarWHEREres = s_VarWHERE.ToString().Trim();
         if (s_VarWHEREres.Length > 0){
            if (s_VarWHEREres.StartsWith("AND "))
               s_VarWHEREres = s_VarWHEREres.Substring(4);
            else if (s_VarWHEREres.StartsWith("OR "))
               s_VarWHEREres = s_VarWHEREres.Substring(3);
            sCmd.Append(" WHERE " + s_VarWHEREres);
         }
         sCmd.Append(" ORDER BY PerfilPermisoID ASC ");
         DataSet ds = new DataSet();
         DbDataAdapter sqlAdapter = dctx.CreateDataAdapter();
         sqlAdapter.SelectCommand = sqlCmd;
         try{
            sqlCmd.CommandText = sCmd.ToString();
            sqlAdapter.Fill(ds, "PerfilPermiso");
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("Error DA1868.- PerfilPermisoConsultar: Hubo un error al consultar los registros. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         return ds;
      }
   } 
}
