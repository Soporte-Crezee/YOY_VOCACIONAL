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
    public class PermisoUpdHlp
    {
        /// <summary>
        /// Actualiza de manera optimista un registro de Permiso en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="permiso">Permiso que tiene los datos nuevos</param>
        /// <param name="anterior">Permiso que tiene los datos anteriores</param>
        public void Action ( IDataContext dctx , Permiso permiso , Permiso anterior )
        {
            object myFirm = new object( );
            string sError = String.Empty;
            if ( permiso == null )
                sError += ", Permiso";
            if ( anterior == null )
                sError += ", Permiso anterior";
            if ( permiso.Accion == null )
                sError += ", Accion";
            if ( permiso.Aplicacion == null )
                sError += ", Aplicacion";
            if ( anterior.Accion == null )
                sError += ", Accion";
            if ( anterior.Aplicacion == null )
                sError += ", Aplicacion";
            if ( sError.Length > 0 )
                throw new Exception( "PermisoUpdHlp: Los siguientes campos no pueden ser vacios " + sError.Substring( 2 ) );
            if ( dctx == null )
                throw new StandardException( MessageType.Error , "" , "DataContext no puede ser nulo" , "VITADAT.Seguridad.DAO" ,
                   "PermisoUpdHlp" , "Action" , null , null );
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection( myFirm );
                sqlCmd = dctx.CreateCommand( );
            }
            catch ( Exception ex )
            {
                throw new StandardException( MessageType.Error , "" , "AccionUpdHlp: No se pudo conectar a la base de de datos" , "VITADAT.Seguridad.DAO" ,
                   "PermisoUpdHlp" , "Action" , null , null );
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder( );
            sCmd.Append( " UPDATE Permiso " );
            sCmd.Append( " SET " );
          
            sCmd.Append( " Activo = @permiso_Activo " );
            sqlParam = sqlCmd.CreateParameter( );
            sqlParam.ParameterName = "permiso_Activo";
            if ( permiso.Activo == null )
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = permiso.Activo;
            sqlParam.DbType = DbType.Boolean;
            sqlCmd.Parameters.Add( sqlParam );
            sCmd.Append( " ,AccionID = @permiso_Accion_AccionID " );
            sqlParam = sqlCmd.CreateParameter( );
            sqlParam.ParameterName = "permiso_Accion_AccionID";
            if ( permiso.Accion.AccionID == null )
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = permiso.Accion.AccionID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add( sqlParam );
            sCmd.Append( " ,AplicacionID = @permiso_Aplicacion_AplicacionID " );
            sqlParam = sqlCmd.CreateParameter( );
            sqlParam.ParameterName = "permiso_Aplicacion_AplicacionID";
            if ( permiso.Aplicacion.AplicacionID == null )
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = permiso.Aplicacion.AplicacionID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add( sqlParam );
            sCmd.Append( " WHERE " );
            sCmd.Append( " PermisoID = @anterior_PermisoID " );
            sqlParam = sqlCmd.CreateParameter( );
            sqlParam.ParameterName = "anterior_PermisoID";
            if ( anterior.PermisoID == null )
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = anterior.PermisoID;
            sqlParam.DbType = DbType.Int32;
            sqlCmd.Parameters.Add( sqlParam );
            
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
                throw new Exception( "PermisoUpdHlp: Se encontraron problemas al actualizar los datos. " + exmsg );
            }
            finally
            {
                try { dctx.CloseConnection( myFirm ); }
                catch ( Exception ) { }
            }
            if ( iRes < 1 )
                throw new Exception( "PermisoUpdHlp: Se encontraron problemas al actualizar los datos." );
        }
    } 
}
