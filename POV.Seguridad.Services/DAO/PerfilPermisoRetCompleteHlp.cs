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
   /// Consulta registros de PerfilPermiso, Aplicacion y Accion en la base de datos
   /// </summary>
   public class PerfilPermisoRetCompleteHlp { 
      /// <summary>
      /// Consulta registros de PerfilPermiso en la base de datos.
      /// </summary>
      /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
      /// <param name="perfilPermiso">PerfilPermiso que provee el criterio de selección para realizar la consulta</param>
      /// <returns>El DataSet que contiene la información de PerfilPermiso generada por la consulta</returns>
      public DataSet Action(IDataContext dctx, int? perfilID){
         object myFirm = new object();
         if (dctx == null)
            throw new Exception("Error DA1869.- PerfilPermisoConsultarCompleto: DataContext no puede ser nulo");
         DbCommand sqlCmd = null;
         try{
            dctx.OpenConnection(myFirm);
            sqlCmd = dctx.CreateCommand();
         } catch(Exception ex){
            throw new Exception("Error DA1870.- PerfilPermisoConsultar: Hubo un error al conectarse a la base de datos" + Environment.NewLine + ex.Message);
         }
         DbParameter sqlParam;
         StringBuilder sCmd = new StringBuilder();
         sCmd.Append(" SELECT a.PerfilPermisoID PerfilPermiso_PerfilPermisoID, ");
         sCmd.Append(" b.PermisoID Permiso_PermisoID, ");
         sCmd.Append(" c.AplicacionID Aplicacion_AplicacionID, c.Nombre Aplicacion_Descripcion, ");
         sCmd.Append(" d.AccionID Accion_AccionID, d.Nombre Accion_Descripcion ");
         sCmd.Append(" FROM PerfilPermiso a, Permiso b, Aplicacion c, Accion d ");
         if (perfilID == null)
            sCmd.Append(" WHERE a.PerfilID IS NULL ");
         else{ 
            sCmd.Append(" WHERE a.PerfilID = @perfilID ");
            sqlParam = sqlCmd.CreateParameter();
            sqlParam.ParameterName = "perfilID";
            sqlParam.Value = perfilID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add(sqlParam);
         }
         sCmd.Append(" AND a.PermisoID = b.PermisoID ");
         sCmd.Append(" AND b.AplicacionID = c.AplicacionID ");
         sCmd.Append(" AND b.AccionID = d.AccionID ");
         sCmd.Append(" ORDER BY a.PerfilPermisoID ASC ");
         DataSet ds = new DataSet();
         DbDataAdapter sqlAdapter = dctx.CreateDataAdapter();
         sqlAdapter.SelectCommand = sqlCmd;
         try{
            sqlCmd.CommandText = sCmd.ToString();
            sqlAdapter.Fill(ds, "PerfilPermiso");
         } catch(Exception ex){
            string exmsg = ex.Message;
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
            throw new Exception("Error DA1871.- PerfilPermisoConsultar: Hubo un error al consultar los registros. " + exmsg);
         } finally{
            try{ dctx.CloseConnection(myFirm); } catch(Exception){ }
         }
         return ds;
      }
   } 
}
