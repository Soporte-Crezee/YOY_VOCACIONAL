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
    /// Consultar Registros de Aplicaciones en la base de datos
    /// </summary>
    public class AplicacionRetHlp
    {
        /// <summary>
        /// Consulta registros de Aplicacion en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="aplicacion">Aplicacion que provee el criterio de selección para realizar la consulta</param>
        /// <returns>El DataSet que contiene la información de Aplicacion generada por la consulta</returns>
        public DataSet Action ( IDataContext dctx , Aplicacion aplicacion )
        {
            object myFirm = new object( );
            string sError = String.Empty;
            if ( aplicacion == null )
                sError += ", Aplicacion";
            if ( sError.Length > 0 )
                throw new Exception( "AplicacionRetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring( 2 ) );
            if ( dctx == null )
                throw new StandardException( MessageType.Error , "" , "DataContext no puede ser nulo" , "DAO" ,
                   "AplicacionRetHlp" , "Action" , null , null );
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection( myFirm );
                sqlCmd = dctx.CreateCommand( );
            }
            catch ( Exception ex )
            {
                throw new StandardException( MessageType.Error , "" , "AplicacionRetHlp: No se pudo conectar a la base de datos" , "DAO" ,
                   "AplicacionRetHlp" , "Action" , null , null );
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder( );
            sCmd.Append( " SELECT AplicacionID, Nombre, Activo " );
            sCmd.Append( " FROM Aplicacion " );
            StringBuilder s_VarWHERE = new StringBuilder( );
            if ( aplicacion.AplicacionID != null )
            {
                s_VarWHERE.Append( " AplicacionID= @aplicacion_AplicacionID " );
                sqlParam = sqlCmd.CreateParameter( );
                sqlParam.ParameterName = "aplicacion_AplicacionID";
                sqlParam.Value = aplicacion.AplicacionID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add( sqlParam );
            }
            if ( aplicacion.Nombre != null )
            {
                s_VarWHERE.Append( " AND Nombre LIKE @aplicacion_Nombre " );
                sqlParam = sqlCmd.CreateParameter( );
                sqlParam.ParameterName = "aplicacion_Nombre";
                sqlParam.Value = aplicacion.Nombre;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add( sqlParam );
            }
            if ( aplicacion.Activo != null )
            {
                s_VarWHERE.Append( " AND Activo = @aplicacion_Activo " );
                sqlParam = sqlCmd.CreateParameter( );
                sqlParam.ParameterName = "aplicacion_Activo";
                sqlParam.Value = aplicacion.Activo;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add( sqlParam );
            }
            string s_VarWHEREres = s_VarWHERE.ToString( ).Trim( );
            if ( s_VarWHEREres.Length > 0 )
            {
                if ( s_VarWHEREres.StartsWith( "AND " ) )
                    s_VarWHEREres = s_VarWHEREres.Substring( 4 );
                else if ( s_VarWHEREres.StartsWith( "OR " ) )
                    s_VarWHEREres = s_VarWHEREres.Substring( 3 );
                else if ( s_VarWHEREres.StartsWith( "," ) )
                    s_VarWHEREres = s_VarWHEREres.Substring( 1 );
                sCmd.Append( " WHERE " + s_VarWHEREres );
            }
            sCmd.Append( " ORDER BY AplicacionID ASC " );
            DataSet ds = new DataSet( );
            DbDataAdapter sqlAdapter = dctx.CreateDataAdapter( );
            sqlAdapter.SelectCommand = sqlCmd;
            try
            {
                sqlCmd.CommandText = sCmd.Replace( "@" , dctx.ParameterSymbol ).ToString( );
                sqlAdapter.Fill( ds , "Aplicacion" );
            }
            catch ( Exception ex )
            {
                string exmsg = ex.Message;
                try { dctx.CloseConnection( myFirm ); }
                catch ( Exception ) { }
                throw new Exception( "AplicacionRetHlp: Se encontraron problemas al recuperar los datos. " + exmsg );
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
