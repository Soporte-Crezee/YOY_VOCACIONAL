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
    /// Actualiza un Permiso en la base de datos
    /// </summary>
    public class PermisoDelHlp
    {
        /// <summary>
        /// Elimina un registro de Permiso en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="permiso">Permiso que desea eliminar</param>
        public void Action ( IDataContext dctx , Permiso permiso )
        {
            object myFirm = new object( );
            string sError = String.Empty;
            if ( permiso == null )
                sError += ", Permiso";
            if ( sError.Length > 0 )
                throw new Exception( "PermisoDel: Los siguientes campos no pueden ser vacios " + sError.Substring( 2 ) );
            if ( permiso.PermisoID == null )
                sError += ", PermisoID";
            if ( sError.Length > 0 )
                throw new Exception( "PermisoDel: Los siguientes campos no pueden ser vacios " + sError.Substring( 2 ) );
            if ( dctx == null )
                throw new StandardException( MessageType.Error , "" , "DataContext no puede ser nulo" , "VITADAT.Seguridad.DAO" ,
                   "PermisoDelHlp" , "Action" , null , null );
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection( myFirm );
                sqlCmd = dctx.CreateCommand( );
            }
            catch ( Exception ex )
            {
                throw new StandardException( MessageType.Error , "" , "PermisoDel No se pudo conectar a la base de datos" , "VITADAT.Seguridad.DAO" ,
                   "PermisoDelHlp" , "Action" , null , null );
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder( );
            sCmd.Append( " DELETE FROM Permiso " );
            if ( permiso.PermisoID == null )
                sCmd.Append( " WHERE PermisoID IS NULL " );
            else
            {
                sCmd.Append( " WHERE PermisoID = @permiso_PermisoID " );
                sqlParam = sqlCmd.CreateParameter( );
                sqlParam.ParameterName = "permiso_PermisoID";
                sqlParam.Value = permiso.PermisoID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add( sqlParam );
            }
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
                throw new Exception( "PermisoDel: Se encontraron problemas al elimiar el registro . " + exmsg );
            }
            finally
            {
                try { dctx.CloseConnection( myFirm ); }
                catch ( Exception ) { }
            }
            if ( iRes < 1 )
                throw new Exception( "PermisoDel: Se encontraron problemas al elimiar el registro ." );
        }
    } 
}
