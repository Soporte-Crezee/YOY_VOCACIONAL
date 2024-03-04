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
    /// Consultar Registros de Perfiles en la base de datos
    /// </summary>

    public class PerfilRetHlp
    {
        /// <summary>
        /// Consulta registros de perfil en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="perfil">perfil que provee el criterio de selección para realizar la consulta</param>
        /// <returns>El DataSet que contiene la información de perfil generada por la consulta</returns>
        public DataSet Action ( IDataContext dctx , Perfil perfil )
        {
            object myFirm = new object( );
            string sError = String.Empty;
            if ( perfil == null )
                sError += ", Perfil";
            if ( sError.Length > 0 )
                throw new Exception( "PerfilRetHlp: Los siguientes campos no pueden ser vacios " + sError.Substring( 2 ) );
            if ( dctx == null )
                throw new StandardException( MessageType.Error , "" , "DataContext no puede ser nulo" , "VITADAT.Seguridad.DAO" ,
                   "PerfilRetHlp" , "Action" , null , null );
            DbCommand sqlCmd = null;
            try
            {
                dctx.OpenConnection( myFirm );
                sqlCmd = dctx.CreateCommand( );
            }
            catch ( Exception ex )
            {
                throw new StandardException( MessageType.Error , "" , "PerfilRetHlp: No se pudo conectar a la base de datos" , "VITADAT.Seguridad.DAO" ,
                   "PerfilRetHlp" , "Action" , null , null );
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder( );
            sCmd.Append( " SELECT PerfilID, Nombre, Operaciones, Descripcion, Estatus FROM Perfil " );
            StringBuilder s_Varwhere = new StringBuilder( );
            if ( perfil.PerfilID != null )
            {
                s_Varwhere.Append( " PerfilID= @perfil_PerfilID " );
                sqlParam = sqlCmd.CreateParameter( );
                sqlParam.ParameterName = "perfil_PerfilID";
                sqlParam.Value = perfil.PerfilID;
                sqlParam.DbType = DbType.Int32;
                sqlCmd.Parameters.Add( sqlParam );
            }
            if (perfil.Nombre != null)
            {
                s_Varwhere.Append(" AND Nombre LIKE @perfil_Nombre ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "perfil_Nombre";
                sqlParam.Value = perfil.Nombre;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add(sqlParam);
            }
            if ( perfil.Operaciones != null )
            {
                s_Varwhere.Append( " AND Operaciones = @perfil_Operaciones " );
                sqlParam = sqlCmd.CreateParameter( );
                sqlParam.ParameterName = "perfil_Operaciones";
                sqlParam.Value = perfil.Operaciones;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add( sqlParam );
            }
            if ( perfil.Descripcion != null )
            {
                s_Varwhere.Append(" AND Descripcion LIKE @perfil_Descripcion ");
                sqlParam = sqlCmd.CreateParameter( );
                sqlParam.ParameterName = "perfil_Descripcion";
                sqlParam.Value = perfil.Descripcion;
                sqlParam.DbType = DbType.String;
                sqlCmd.Parameters.Add( sqlParam );
            }
            if (perfil.Estatus != null)
            {
                s_Varwhere.Append(" AND Estatus = @perfil_Estatus ");
                sqlParam = sqlCmd.CreateParameter();
                sqlParam.ParameterName = "perfil_Estatus";
                sqlParam.Value = perfil.Estatus;
                sqlParam.DbType = DbType.Boolean;
                sqlCmd.Parameters.Add(sqlParam);
            }
            string s_Varwhereres = s_Varwhere.ToString( ).Trim( );
            if ( s_Varwhereres.Length > 0 )
            {
                if ( s_Varwhereres.StartsWith( "AND " ) )
                    s_Varwhereres = s_Varwhereres.Substring( 4 );
                else if ( s_Varwhereres.StartsWith( "OR " ) )
                    s_Varwhereres = s_Varwhereres.Substring( 3 );
                else if ( s_Varwhereres.StartsWith( "," ) )
                    s_Varwhereres = s_Varwhereres.Substring( 1 );
                sCmd.Append( " where " + s_Varwhereres );
            }
            DataSet ds = new DataSet( );
            DbDataAdapter sqlAdapter = dctx.CreateDataAdapter( );
            sqlAdapter.SelectCommand = sqlCmd;
            try
            {
                sqlCmd.CommandText = sCmd.Replace( "@" , dctx.ParameterSymbol ).ToString( );
                sqlAdapter.Fill( ds , "Perfil" );
            }
            catch ( Exception ex )
            {
                string exmsg = ex.Message;
                try { dctx.CloseConnection( myFirm ); }
                catch ( Exception ) { }
                throw new Exception( "PerfilRetHlp: Se encontraron problemas al recuperar los datos de perfil. " + exmsg );
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
