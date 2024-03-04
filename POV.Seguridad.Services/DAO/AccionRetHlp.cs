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
    /// Consultar Registros de Acciones en la base de datos
    /// </summary>
    public class AccionRetHlp
    {
        /// <summary>
        /// Consulta registros de Accion en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="accion">Accion que provee el criterio de selección para realizar la consulta</param>
        /// <returns>El DataSet que contiene la información de Accion generada por la consulta</returns>
        public DataSet Action ( IDataContext dctx , Accion accion )
        {
            object myFirm = new object( );
            string sError = String.Empty;
            if ( accion == null )
                sError += ", Accion";
            if ( sError.Length > 0 )
                throw new Exception( "AccionRetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring( 2 ) );
            if ( dctx == null )
                throw new StandardException( MessageType.Error , "" , "DataContext no puede ser nulo" , "VITADAT.Seguridad.DAO" ,
                   "AccionRetHlp" , "Action" , null , null );
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection( myFirm );
                sqlCmd = dctx.CreateCommand( );
            }
            catch ( Exception ex )
            {
                throw new StandardException( MessageType.Error , "" , "AccionRetHlp: No se pudo conectar a la base de datos" , "VITADAT.Seguridad.DAO" ,
                   "AccionRetHlp" , "Action" , null , null );
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder( );
            sCmd.Append( " SELECT AccionID, Nombre, Activo FROM Accion " );
            StringBuilder s_VarWHERE = new StringBuilder( );
            if ( accion.AccionID != null )
            {
                s_VarWHERE.Append( " AccionID= @accion_AccionID " );
                sqlParam = sqlCmd.CreateParameter( );
                sqlParam.ParameterName = "accion_AccionID";
                sqlParam.Value = accion.AccionID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add( sqlParam );
            }
            if ( accion.Nombre != null )
            {
                s_VarWHERE.Append( " AND Nombre LIKE @accion_Nombre " );
                sqlParam = sqlCmd.CreateParameter( );
                sqlParam.ParameterName = "accion_Nombre";
                sqlParam.Value = accion.Nombre;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add( sqlParam );
            }
            if ( accion.Activo != null )
            {
                s_VarWHERE.Append( " AND Activo = @accion_Activo " );
                sqlParam = sqlCmd.CreateParameter( );
                sqlParam.ParameterName = "accion_Activo";
                sqlParam.Value = accion.Activo;
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
            DataSet ds = new DataSet( );
            DbDataAdapter sqlAdapter = dctx.CreateDataAdapter( );
            sqlAdapter.SelectCommand = sqlCmd;
            try
            {
                sqlCmd.CommandText = sCmd.Replace( "@" , dctx.ParameterSymbol ).ToString( );
                sqlAdapter.Fill( ds , "Accion" );
            }
            catch ( Exception ex )
            {
                string exmsg = ex.Message;
                try { dctx.CloseConnection( myFirm ); }
                catch ( Exception ) { }
                throw new Exception( "AccionRetHlp: Se encontraron problemas al recuperar los datos. " + exmsg );
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
