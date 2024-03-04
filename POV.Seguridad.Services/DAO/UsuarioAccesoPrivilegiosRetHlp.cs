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
   /// Consulta registros de UsuarioAcceso y Privilegios en la base de datos
   /// </summary>
   public class UsuarioAccesoPrivilegiosRetHlp { 
      /// <summary>
      /// Consulta registros de UsuarioAcceso en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="usuarioAcceso">UsuarioAcceso que provee el criterio de selección para realizar la consulta</param>
      /// <returns>El DataSet que contiene la información de UsuarioAcceso generada por la consulta</returns>
      public DataSet Action(IDataContext dctx, UsuarioPrivilegios usuarioPrivilegios){
         object myFirm = new object();
         string sError = "";
         if (usuarioPrivilegios == null)
            sError += ", UsuarioPrivilegios";
         if (sError.Length > 0)
            throw new Exception("Error DA1945.- UsuarioAccesoPrivilegiosConsultar: Los siguientes campos no pueden ser vacíos " + sError.Substring(2));
         if (usuarioPrivilegios.UsuarioPrivilegiosID == null)
            sError += ", UsuarioPrivilegios.UsuarioPrivilegiosID";
         if (sError.Length > 0)
            throw new Exception("Error DA1946.- UsuarioAccesoPrivilegiosConsultar: Los siguientes campos no pueden ser vacíos " + sError.Substring(2));
         if (dctx == null)
            throw new Exception("Error DA1947.- UsuarioAccesoPrivilegiosConsultar: DataContext no puede ser nulo");
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
            throw new Exception("Error DA1948.- UsuarioAccesoPrivilegiosConsultar: Hubo un error al conectarse a la base de datos" + Environment.NewLine + ex.Message);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" SELECT a.UsuarioAccesoID UsuarioAccesoID, a.FechaAsignacion FechaAsignacion, a.UsuarioAsignoID UsuarioAsignoID, ");
         sCmd.Append(" b.PrivilegioID Privilegio_PrivilegioID, b.PerfilID Privilegio_PerfilID, b.PermisoID Privilegio_PermisoID ");
         sCmd.Append(" FROM UsuarioAcceso a, Privilegio b ");
         if (usuarioPrivilegios.UsuarioPrivilegiosID == null)
            sCmd.Append(" WHERE a.UsuarioPrivilegiosID IS NULL ");
         else{ 
            sCmd.Append(" WHERE a.UsuarioPrivilegiosID = @usuarioPrivilegios_UsuarioPrivilegiosID ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "usuarioPrivilegios_UsuarioPrivilegiosID";
            sqlParam.Value = usuarioPrivilegios.UsuarioPrivilegiosID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
         }
         sCmd.Append(" AND a.UsuarioAccesoID = b.UsuarioAccesoID ");
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
            throw new Exception("Error DA1949.- UsuarioAccesoPrivilegiosConsultar: Hubo un error al consultar los registros. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         return ds;
      }
   } 
}
