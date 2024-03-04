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
    /// Consultar Registros de Permisos en la base de datos
    /// </summary>
    public class PermisoRetHlp
    {
        /// <summary>
        /// Consulta registros de Permiso en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="permiso">Permiso que provee el criterio de selección para realizar la consulta</param>
        /// <returns>El DataSet que contiene la información de Permiso generada por la consulta</returns>
        public DataSet Action ( IDataContext dctx , Permiso permiso )
        {
            object myFirm = new object( );
            string sError = String.Empty;
            if ( permiso == null )
                sError += ", Permiso";
            if ( sError.Length > 0 )
                throw new Exception( "PermisoRet: Los siguientes campos no pueden ser vacios " + sError.Substring( 2 ) );
            if ( permiso.Accion == null )
            {
                permiso.Accion = new Accion( );
            }
            if ( permiso.Aplicacion == null )
            {
                permiso.Aplicacion = new Aplicacion( );
            }
            if ( dctx == null )
                throw new StandardException( MessageType.Error , "" , "DataContext no puede ser nulo" , "VITADAT.Seguridad.DAO" ,
                   "PermisoRetHlp" , "Action" , null , null );
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection( myFirm );
                sqlCmd = dctx.CreateCommand( );
            }
            catch ( Exception ex )
            {
                throw new StandardException( MessageType.Error , "" , "PermisoRet No se pudo conectar a la base de datos" , "Seguridad.DAO" ,
                   "PermisoRetHlp" , "Action" , null , null );
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder( );
            sCmd.Append( " SELECT " );
            sCmd.Append( " p.PermisoID, p.Nombre, p.Activo, " );
            sCmd.Append( " ap.AplicacionID, ap.Nombre NombreAplicacion, " );
            sCmd.Append( " ac.AccionID, ac.Nombre NombreAccion " );
            sCmd.Append( " FROM Permiso p, Aplicacion ap, Accion ac " );
            sCmd.Append( " WHERE " );
            sCmd.Append( " p.AplicacionID = ap.AplicacionID " );
            sCmd.Append( " AND p.AccionID = ac.AccionID " );
            if ( permiso.Accion.AccionID != null )
            {
                sCmd.Append( " AND ac.AccionID = @permiso_Accion_AccionID " );
                sqlParam = sqlCmd.CreateParameter( );
                sqlParam.ParameterName = "permiso_Accion_AccionID";
                sqlParam.Value = permiso.Accion.AccionID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add( sqlParam );
            }
            if ( permiso.Aplicacion.AplicacionID != null )
            {
                sCmd.Append( " AND ap.AplicacionID = @permiso_Aplicacion_AplicacionID " );
                sqlParam = sqlCmd.CreateParameter( );
                sqlParam.ParameterName = "permiso_Aplicacion_AplicacionID";
                sqlParam.Value = permiso.Aplicacion.AplicacionID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add( sqlParam );
            }
            if ( permiso.PermisoID != null )
            {
                sCmd.Append( " AND p.PermisoID= @permiso_PermisoID " );
                sqlParam = sqlCmd.CreateParameter( );
                sqlParam.ParameterName = "permiso_PermisoID";
                sqlParam.Value = permiso.PermisoID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add( sqlParam );
            }
            
            if ( permiso.Activo != null )
            {
                sCmd.Append( " AND p.Activo = @permiso_Activo " );
                sqlParam = sqlCmd.CreateParameter( );
                sqlParam.ParameterName = "permiso_Activo";
                sqlParam.Value = permiso.Activo;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add( sqlParam );
            }
            DataSet ds = new DataSet( );
            DbDataAdapter sqlAdapter = dctx.CreateDataAdapter( );
            sqlAdapter.SelectCommand = sqlCmd;
            try
            {
                sqlCmd.CommandText = sCmd.Replace( "@" , dctx.ParameterSymbol ).ToString( );
                sqlAdapter.Fill( ds , "Permiso" );
            }
            catch ( Exception ex )
            {
                string exmsg = ex.Message;
                try { dctx.CloseConnection( myFirm ); }
                catch ( Exception ) { }
                throw new Exception( "PermisoRetHlp: Se encontraron problemas al recuperar los datos. " + exmsg );
            }
            finally
            {
                try { dctx.CloseConnection( myFirm ); }
                catch ( Exception ) { }
            }
            return ds;
        }
    } 
}
