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
    /// Crea un permiso en la base de datos
    /// </summary>
    public class PermisoInsHlp
    {
        /// <summary>
        /// Crea un registro de Permiso en la base de datos
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="permiso">Permiso que desea crear</param>
        public void Action ( IDataContext dctx , Permiso permiso )
        {
            object myFirm = new object( );
            string sError = String.Empty;
            if ( permiso == null )
                sError += ", Permiso";
            if ( permiso.Accion == null )
                sError += ", Accion";
            if ( permiso.Aplicacion == null )
                sError += ", Aplicacion";
            if ( sError.Length > 0 )
                throw new Exception( "PermisoInsHlp: Los siguientes campos no pueden ser vacios: " + sError.Substring( 2 ) + " " );
            
            if ( permiso.Activo == null )
                sError += ", Activo";
            if ( sError.Length > 0 )
                throw new Exception( "PermisoInsHlp: Los siguientes campos no pueden ser vacios: " + sError.Substring( 2 ) + " " );
            if ( dctx == null )
                throw new StandardException( MessageType.Error , "" , "DataContext no puede ser nulo" , "VITADAT.Seguridad.DAO" ,
                   "PermisoInsHlp" , "Action" , null , null );
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection( myFirm );
                sqlCmd = dctx.CreateCommand( );
            }
            catch ( Exception ex )
            {
                throw new StandardException( MessageType.Error , "" , "permisoIns: No se pudo conectar con la base de datos" , "VITADAT.Seguridad.DAO" ,
                   "PermisoInsHlp" , "Action" , null , null );
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder( );
            sCmd.Append( " INSERT INTO Permiso (Activo, AccionID, AplicacionID) " );
            sCmd.Append( " VALUES " );
            sCmd.Append( " ( " );
            
            sCmd.Append( " @permiso_Activo " );
            sqlParam = sqlCmd.CreateParameter( );
            sqlParam.ParameterName = "permiso_Activo";
            if ( permiso.Activo == null )
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = permiso.Activo;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add( sqlParam );
            sCmd.Append( " ,@permiso_Accion_AccionID " );
            sqlParam = sqlCmd.CreateParameter( );
            sqlParam.ParameterName = "permiso_Accion_AccionID";
            if ( permiso.Accion.AccionID == null )
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = permiso.Accion.AccionID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add( sqlParam );
            sCmd.Append( " ,@permiso_Aplicacion_AplicacionID " );
            sqlParam = sqlCmd.CreateParameter( );
            sqlParam.ParameterName = "permiso_Aplicacion_AplicacionID";
            if ( permiso.Aplicacion.AplicacionID == null )
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = permiso.Aplicacion.AplicacionID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add( sqlParam );
            sCmd.Append( " ) " );
            int iRes = 0;
            try
            {
                sqlCmd.CommandText = sCmd.Replace( "@" , dctx.ParameterSymbol ).ToString( );
                iRes = sqlCmd.ExecuteNonQuery( );
            }
            catch ( Exception ex )
            {
                string exmsg = ex.Message;
                try { dctx.CloseConnection( myFirm ); }
                catch ( Exception ) { }
                throw new Exception( "PermisoInsHlp: Se encontraron problemas al crear el registro. " + exmsg );
            }
            finally
            {
                try { dctx.CloseConnection( myFirm ); }
                catch ( Exception ) { }
            }
            if ( iRes < 1 )
                throw new Exception( "PermisoInsHlp: Se encontraron problemas al crear el registro." );
        }
    } 
}
