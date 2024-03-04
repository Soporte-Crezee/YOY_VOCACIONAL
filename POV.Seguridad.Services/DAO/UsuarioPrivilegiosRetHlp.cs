using System;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Seguridad.BO;
using POV.CentroEducativo.BO;

namespace POV.Seguridad.DAO
{ 
   /// <summary>
   /// Consulta registros de UsuarioPrivilegios en la base de datos
   /// </summary>
   public class UsuarioPrivilegiosRetHlp { 
      /// <summary>
      /// Consulta registros de UsuarioPrivilegios en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="usuarioPrivilegios">UsuarioPrivilegios que provee el criterio de selección para realizar la consulta</param>
      /// <returns>El DataSet que contiene la información de UsuarioPrivilegios generada por la consulta</returns>
      public DataSet Action(IDataContext dctx, UsuarioPrivilegios usuarioPrivilegios){
         object myFirm = new object();
         string sError = "";
         if (usuarioPrivilegios == null)
            sError += ", UsuarioPrivilegios";
         if (sError.Length > 0)
            throw new Exception("Error DA1978.- UsuarioPrivilegiosConsultar: Los siguientes campos no pueden ser vacíos: " + sError.Substring(2));
         if (usuarioPrivilegios.Usuario == null)
            sError += ", Usuario";
         if (sError.Length > 0)
            throw new Exception("Error DA1979.- UsuarioPrivilegiosConsultar: Los siguientes campos no pueden ser vacíos: " + sError.Substring(2));
         if (dctx == null)
            throw new Exception("Error DA1980.- UsuarioPrivilegiosConsultar: DataContext no puede ser nulo");

         if (usuarioPrivilegios is UsuarioEscolarPrivilegios)
         {
             if ((usuarioPrivilegios as UsuarioEscolarPrivilegios).Escuela == null)
                 (usuarioPrivilegios as UsuarioEscolarPrivilegios).Escuela = new Escuela();
             if ((usuarioPrivilegios as UsuarioEscolarPrivilegios).CicloEscolar == null)
                 (usuarioPrivilegios as UsuarioEscolarPrivilegios).CicloEscolar = new CicloEscolar();
         }
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
            throw new Exception("Error DA1981.- UsuarioPrivilegiosConsultar: Hubo un error al conectarse a la base de datos" + Environment.NewLine + ex.Message);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" SELECT UsuarioPrivilegiosID, UsuarioID, FechaCreacion, Estado, EscuelaID, CicloEscolarID ");
         sCmd.Append(" FROM UsuarioPrivilegios ");
         StringBuilder s_VarWHERE = new StringBuilder();
         if (usuarioPrivilegios.UsuarioPrivilegiosID != null){
            s_VarWHERE.Append(" UsuarioPrivilegiosID = @usuarioPrivilegios_UsuarioPrivilegiosID ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "usuarioPrivilegios_UsuarioPrivilegiosID";
            sqlParam.Value = usuarioPrivilegios.UsuarioPrivilegiosID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (usuarioPrivilegios.Usuario.UsuarioID != null){
            s_VarWHERE.Append(" AND UsuarioID = @usuarioPrivilegios_Usuario_UsuarioID ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "usuarioPrivilegios_Usuario_UsuarioID";
            sqlParam.Value = usuarioPrivilegios.Usuario.UsuarioID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (usuarioPrivilegios.FechaCreacion != null){
            s_VarWHERE.Append(" AND FechaCreacion = @usuarioPrivilegios_FechaCreacion ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "usuarioPrivilegios_FechaCreacion";
            sqlParam.Value = usuarioPrivilegios.FechaCreacion;
            sqlParam.DbType = DbType.DateTime;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (usuarioPrivilegios.Estado != null){
            s_VarWHERE.Append(" AND Estado = @usuarioPrivilegios_Estado ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "usuarioPrivilegios_Estado";
            sqlParam.Value = usuarioPrivilegios.Estado;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (usuarioPrivilegios is UsuarioEscolarPrivilegios)
         {
             if ((usuarioPrivilegios as UsuarioEscolarPrivilegios).Escuela.EscuelaID != null)
             {
                 s_VarWHERE.Append(" AND EscuelaID = @dbp4ram5 ");
                 sqlParam = sqlCmd.CreateParameter();
                 sqlParam.ParameterName = "dbp4ram5";
                 sqlParam.Value = (usuarioPrivilegios as UsuarioEscolarPrivilegios).Escuela.EscuelaID;
                 sqlParam.DbType = DbType.Int32;
                 sqlCmd.Parameters.Add(sqlParam);
             }
             if ((usuarioPrivilegios as UsuarioEscolarPrivilegios).CicloEscolar.CicloEscolarID != null)
             {
                 s_VarWHERE.Append(" AND CicloEscolarID = @dbp4ram6 ");
                 sqlParam = sqlCmd.CreateParameter();
                 sqlParam.ParameterName = "dbp4ram6";
                 sqlParam.Value = (usuarioPrivilegios as UsuarioEscolarPrivilegios).CicloEscolar.CicloEscolarID;
                 sqlParam.DbType = DbType.Int32;
                 sqlCmd.Parameters.Add(sqlParam);
             }
         }
         string s_VarWHEREres = s_VarWHERE.ToString().Trim();
         if (s_VarWHEREres.Length > 0){
            if (s_VarWHEREres.StartsWith("AND "))
               s_VarWHEREres = s_VarWHEREres.Substring(4);
            else if (s_VarWHEREres.StartsWith("OR "))
               s_VarWHEREres = s_VarWHEREres.Substring(3);
            sCmd.Append(" WHERE " + s_VarWHEREres);
         }
         sCmd.Append(" ORDER BY UsuarioPrivilegiosID ASC ");
         DataSet ds = new DataSet();
         DbDataAdapter sqlAdapter = dctx.CreateDataAdapter();
         sqlAdapter.SelectCommand = sqlCmd;
         try{
            sqlCmd.CommandText = sCmd.ToString();
            sqlAdapter.Fill(ds, "UsuarioPrivilegios");
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("Error DA1982.- UsuarioPrivilegiosConsultar: Hubo un error al consultar los registros. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         return ds;
      }
   } 
}
