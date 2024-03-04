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
    /// Crea una aplicacion en la base de datos
    /// </summary>
    public class AplicacionInsHlp
    {
        /// <summary>
        /// Crea un registro de Aplicacion en la base de datos
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="aplicacion">Aplicacion que desea crear</param>
        public void Action ( IDataContext dctx , Aplicacion aplicacion )
        {
            object myFirm = new object( );
            string sError = String.Empty;
            if ( aplicacion == null )
                sError += ", aplicacion";
            if ( sError.Length > 0 )
                throw new Exception( "AplicacionInsHlp: Los siguientes campos no pueden ser vacios: " + sError.Substring( 2 ) + " " );
            if ( aplicacion.Nombre == null || aplicacion.Nombre.Trim( ).Length == 0 )
                sError += ", Nombre";
            if ( sError.Length > 0 )
                throw new Exception( "AplicacionInsHlp: Los siguientes campos no pueden ser vacios: " + sError.Substring( 2 ) + " " );
            if ( dctx == null )
                throw new StandardException( MessageType.Error , "" , "DataContext no puede ser nulo" , "Seguridad.DAO" ,
                   "AplicacionInsHlp" , "Action" , null , null );
            DbCommand cmd = null;
            try
            {
                dctx.OpenConnection( myFirm );
                cmd = dctx.CreateCommand( );
            }
            catch ( Exception ex )
            {
                throw new StandardException( MessageType.Error , "" , "AplicacionIns: No se pudo conectar con la base de datos" , "DAO" ,
                   "AplicacionInsHlp" , "Action" , null , null );
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder( );
            sCmd.Append( " INSERT INTO Aplicacion (Nombre, Activo) " );
            sCmd.Append( " VALUES (@aplicacion_Nombre " );
            sqlParam = cmd.CreateParameter( );
            sqlParam.ParameterName = "aplicacion_Nombre";
            if ( aplicacion.Nombre == null )
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = aplicacion.Nombre;
            sqlParam.DbType = DbType.String;
            cmd.Parameters.Add( sqlParam );
            sCmd.Append( " , @aplicacion_Activo " );
            sqlParam = cmd.CreateParameter( );
            sqlParam.ParameterName = "aplicacion_Activo";
            if ( aplicacion.Activo == null )
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = aplicacion.Activo;
            sqlParam.DbType = DbType.Boolean;
            cmd.Parameters.Add( sqlParam );
            sCmd.Append( " ) " );
            int iRes = 0;
            try
            {
                cmd.CommandText = sCmd.Replace( "@" , dctx.ParameterSymbol ).ToString( );
                iRes = cmd.ExecuteNonQuery( );
            }
            catch ( Exception ex )
            {
                string exmsg = ex.Message;
                try { dctx.CloseConnection( myFirm ); }
                catch ( Exception ) { }
                throw new Exception( "AplicacionInsHlp: Se encontraron problemas al crear el registro. " + exmsg );
            }
            finally
            {
                try { dctx.CloseConnection( myFirm ); }
                catch ( Exception ) { }
            }
            if ( iRes < 1 )
                throw new Exception( "AplicacionInsHlp: Se encontraron problemas al crear el registro." );
        }
    } 
}
