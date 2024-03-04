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
   /// Consulta registros de UsuarioAcceso en la base de datos
   /// </summary>
   public class UsuarioAccesoRetHlp { 
      /// <summary>
      /// Consulta registros de UsuarioAcceso en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="usuarioAcceso">UsuarioAcceso que provee el criterio de selección para realizar la consulta</param>
      /// <returns>El DataSet que contiene la información de UsuarioAcceso generada por la consulta</returns>
      public DataSet Action(IDataContext dctx, UsuarioPrivilegios usuarioPrivilegios, UsuarioAcceso usuarioAcceso){
         object myFirm = new object();
         string sError = "";
         if (usuarioAcceso == null)
            sError += ", UsuarioAcceso";
         if (usuarioPrivilegios == null)
            sError += ", UsuarioPrivilegios";
         if (sError.Length > 0)
            throw new Exception("Error DA1921.- UsuarioAccesoConsultar: Los siguientes campos no pueden ser vacíos " + sError.Substring(2));
         if (usuarioPrivilegios.UsuarioPrivilegiosID == null)
            sError += ", UsuarioPrivilegios.UsuarioPrivilegiosID";
         if (usuarioAcceso.UsuarioAsigno == null)
            sError += ", UsuarioAsigno";
        
         if (sError.Length > 0)
            throw new Exception("Error DA1922.- UsuarioAccesoConsultar: Los siguientes campos no pueden ser vacíos " + sError.Substring(2));
         if (dctx == null)
            throw new Exception("Error DA1923.- UsuarioAccesoConsultar: DataContext no puede ser nulo");
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
            throw new Exception("Error DA1924.- UsuarioAccesoConsultar: Hubo un error al conectarse a la base de datos" + Environment.NewLine + ex.Message);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" SELECT a.UsuarioAccesoID, a.UsuarioPrivilegiosID, a.FechaAsignacion, a.UsuarioAsignoID ");
         sCmd.Append(" FROM UsuarioAcceso a ");
         StringBuilder s_VarWHERE = new StringBuilder();
         if (usuarioAcceso.UsuarioAccesoID != null){
             s_VarWHERE.Append(" a.UsuarioAccesoID = @usuarioAcceso_UsuarioAccesoID ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "usuarioAcceso_UsuarioAccesoID";
            sqlParam.Value = usuarioAcceso.UsuarioAccesoID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (usuarioPrivilegios.UsuarioPrivilegiosID != null){
             s_VarWHERE.Append(" AND a.UsuarioPrivilegiosID = @usuarioPrivilegios_UsuarioPrivilegiosID ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "usuarioPrivilegios_UsuarioPrivilegiosID";
            sqlParam.Value = usuarioPrivilegios.UsuarioPrivilegiosID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (usuarioAcceso.FechaAsignacion != null){
             s_VarWHERE.Append(" AND a.FechaAsignacion = @usuarioAcceso_FechaAsignacion ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "usuarioAcceso_FechaAsignacion";
            sqlParam.Value = usuarioAcceso.FechaAsignacion;
            sqlParam.DbType = DbType.DateTime;
            sqlCmd.Parameters.Add(sqlParam);
         }
         if (usuarioAcceso.UsuarioAsigno.UsuarioID != null){
             s_VarWHERE.Append(" AND a.UsuarioAsignoID = @usuarioAcceso_UsuarioAsigno_UsuarioID ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "usuarioAcceso_UsuarioAsigno_UsuarioID";
            sqlParam.Value = usuarioAcceso.UsuarioAsigno.UsuarioID;
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
         sCmd.Append(" ORDER BY a.UsuarioAccesoID ASC ");
         DataSet ds = new DataSet();
         DbDataAdapter sqlAdapter = dctx.CreateDataAdapter();
         sqlAdapter.SelectCommand = sqlCmd;
         try{
            sqlCmd.CommandText = sCmd.ToString();
            sqlAdapter.Fill(ds, "UsuarioAcceso");
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("Error DA1925.- UsuarioAccesoConsultar: Hubo un error al consultar los registros. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         return ds;
      }

      public DataSet ActionPrivilegio(IDataContext dctx, UsuarioPrivilegios usuarioPrivilegios, UsuarioAcceso usuarioAcceso)
      {
          object myFirm = new object();
          string sError = "";
          if (usuarioAcceso == null)
              sError += ", UsuarioAcceso";
          if (usuarioPrivilegios == null)
              sError += ", UsuarioPrivilegios";
          if (sError.Length > 0)
              throw new Exception("Error DA1921.- UsuarioAccesoConsultar: Los siguientes campos no pueden ser vacíos " + sError.Substring(2));
          if (usuarioPrivilegios.UsuarioPrivilegiosID == null)
              sError += ", UsuarioPrivilegios.UsuarioPrivilegiosID";
          if (usuarioAcceso.UsuarioAsigno == null)
              sError += ", UsuarioAsigno";

          if (sError.Length > 0)
              throw new Exception("Error DA1922.- UsuarioAccesoConsultar: Los siguientes campos no pueden ser vacíos " + sError.Substring(2));
          if (dctx == null)
              throw new Exception("Error DA1923.- UsuarioAccesoConsultar: DataContext no puede ser nulo");
          DbCommand sqlCmd = null;
          try
          {
              dctx.OpenConnection(myFirm);
              sqlCmd = dctx.CreateCommand();
          }
          catch (Exception ex)
          {
              throw new Exception("Error DA1924.- UsuarioAccesoConsultar: Hubo un error al conectarse a la base de datos" + Environment.NewLine + ex.Message);
          }
          DbParameter sqlParam;
          StringBuilder sCmd = new StringBuilder();
          sCmd.Append(" SELECT a.UsuarioAccesoID, a.UsuarioPrivilegiosID, a.FechaAsignacion, a.UsuarioAsignoID ");
          sCmd.Append(" ,b.PrivilegioID Privilegio_PrivilegioID, b.PerfilID Privilegio_PerfilID, b.PermisoID Privilegio_PermisoID ");
          sCmd.Append(" FROM UsuarioAcceso a ");
          sCmd.Append(" JOIN Privilegio b ON (a.UsuarioAccesoID = b.UsuarioAccesoID) ");
          StringBuilder s_VarWHERE = new StringBuilder();
          if (usuarioAcceso.UsuarioAccesoID != null)
          {
              s_VarWHERE.Append(" a.UsuarioAccesoID = @usuarioAcceso_UsuarioAccesoID ");
              sqlParam = sqlCmd.CreateParameter();
              sqlParam.ParameterName = "usuarioAcceso_UsuarioAccesoID";
              sqlParam.Value = usuarioAcceso.UsuarioAccesoID;
              sqlParam.DbType = DbType.Int32;
              sqlCmd.Parameters.Add(sqlParam);
          }
          if (usuarioPrivilegios.UsuarioPrivilegiosID != null)
          {
              s_VarWHERE.Append(" AND a.UsuarioPrivilegiosID = @usuarioPrivilegios_UsuarioPrivilegiosID ");
              sqlParam = sqlCmd.CreateParameter();
              sqlParam.ParameterName = "usuarioPrivilegios_UsuarioPrivilegiosID";
              sqlParam.Value = usuarioPrivilegios.UsuarioPrivilegiosID;
              sqlParam.DbType = DbType.Int32;
              sqlCmd.Parameters.Add(sqlParam);
          }
          if (usuarioAcceso.FechaAsignacion != null)
          {
              s_VarWHERE.Append(" AND a.FechaAsignacion = @usuarioAcceso_FechaAsignacion ");
              sqlParam = sqlCmd.CreateParameter();
              sqlParam.ParameterName = "usuarioAcceso_FechaAsignacion";
              sqlParam.Value = usuarioAcceso.FechaAsignacion;
              sqlParam.DbType = DbType.DateTime;
              sqlCmd.Parameters.Add(sqlParam);
          }
          if (usuarioAcceso.UsuarioAsigno.UsuarioID != null)
          {
              s_VarWHERE.Append(" AND a.UsuarioAsignoID = @usuarioAcceso_UsuarioAsigno_UsuarioID ");
              sqlParam = sqlCmd.CreateParameter();
              sqlParam.ParameterName = "usuarioAcceso_UsuarioAsigno_UsuarioID";
              sqlParam.Value = usuarioAcceso.UsuarioAsigno.UsuarioID;
              sqlParam.DbType = DbType.Int32;
              sqlCmd.Parameters.Add(sqlParam);
          }
          string s_VarWHEREres = s_VarWHERE.ToString().Trim();
          if (s_VarWHEREres.Length > 0)
          {
              if (s_VarWHEREres.StartsWith("AND "))
                  s_VarWHEREres = s_VarWHEREres.Substring(4);
              else if (s_VarWHEREres.StartsWith("OR "))
                  s_VarWHEREres = s_VarWHEREres.Substring(3);
              sCmd.Append(" WHERE " + s_VarWHEREres);
          }
          sCmd.Append(" ORDER BY a.UsuarioAccesoID ASC ");
          DataSet ds = new DataSet();
          DbDataAdapter sqlAdapter = dctx.CreateDataAdapter();
          sqlAdapter.SelectCommand = sqlCmd;
          try
          {
              sqlCmd.CommandText = sCmd.ToString();
              sqlAdapter.Fill(ds, "UsuarioAcceso");
          }
          catch (Exception ex)
          {
              string exmsg = ex.Message;
              try { dctx.CloseConnection(myFirm); }
              catch (Exception) { }
              throw new Exception("Error DA1925.- UsuarioAccesoConsultar: Hubo un error al consultar los registros. " + exmsg);
          }
          finally
          {
              try { dctx.CloseConnection(myFirm); }
              catch (Exception) { }
          }
          return ds;
      }
   } 
}
