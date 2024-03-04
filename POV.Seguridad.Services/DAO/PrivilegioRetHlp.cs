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
   /// Consulta registros de Privilegio en la base de datos
   /// </summary>
   public class PrivilegioRetHlp { 
      /// <summary>
      /// Consulta registros de Privilegio en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="privilegio">Privilegio que provee el criterio de selección para realizar la consulta</param>
      /// <returns>El DataSet que contiene la información de Privilegio generada por la consulta</returns>
      public DataSet Action(IDataContext dctx, UsuarioAcceso usuarioAcceso, int? privilegioID, int? perfilID, int? permisoID){
         object myFirm = new object();
         string sError = "";
         if (usuarioAcceso == null)
            sError += ", UsuarioAcceso";
         if (sError.Length > 0)
            throw new Exception("Error DA1904.- PrivilegioInsertar: Los siguientes campos no pueden ser vacíos: " + sError.Substring(2));
         if (perfilID == null && permisoID == null)
            throw new Exception("Error DA1905.- PrivilegioConsultar: los siguientes campos no puedes ser vacíos: perfilID, permisoID");
         if (dctx == null)
            throw new Exception("Error DA1906.- PrivilegioConsultar: DataContext no puede ser nulo");
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
            throw new Exception("Error DA1907.- PrivilegioConsultar: Hubo un error al conectarse a la base de datos" + Environment.NewLine + ex.Message);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" SELECT PrivilegioID, UsuarioAccesoID, PerfilID, PermisoID ");
         sCmd.Append(" FROM Privilegio ");
         StringBuilder s_VarWHERE = new StringBuilder();
         if (privilegioID != null){
            s_VarWHERE.Append(" PrivilegioID = @privilegioID ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "privilegioID";
            sqlParam.Value = privilegioID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (usuarioAcceso.UsuarioAccesoID != null){
            s_VarWHERE.Append(" AND UsuarioAccesoID = @usuarioAcceso_UsuarioAccesoID ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "usuarioAcceso_UsuarioAccesoID";
            sqlParam.Value = usuarioAcceso.UsuarioAccesoID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (perfilID == null)
            s_VarWHERE.Append(" AND PerfilID IS NULL ");
         else{ 
            s_VarWHERE.Append(" AND PerfilID = @perfilID ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "perfilID";
            sqlParam.Value = perfilID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (permisoID == null)
            s_VarWHERE.Append(" AND PermisoID IS NULL ");
         else{ 
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
         sCmd.Append(" ORDER BY PrivilegioID ASC ");
         DataSet ds = new DataSet();
         DbDataAdapter sqlAdapter = dctx.CreateDataAdapter();
         sqlAdapter.SelectCommand = sqlCmd;
         try{
            sqlCmd.CommandText = sCmd.ToString();
            sqlAdapter.Fill(ds, "Privilegio");
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("Error DA1908.- PrivilegioConsultar: Hubo un error al consultar los registros. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         return ds;
      }
   } 
}
