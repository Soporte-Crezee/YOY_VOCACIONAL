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
    /// Crea un perfil en la base de datos
    /// </summary>
    public class PerfilInsHlp
    {
        /// <summary>
        /// Crea un registro de perfil en la base de datos
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="perfil">perfil que desea crear</param>
        public void Action ( IDataContext dctx , Perfil perfil )
        {
            object myFirm = new object( );
            string sError = String.Empty;
            if ( perfil == null )
                sError += ", Perfil";
            if ( sError.Length > 0 )
                throw new Exception( "PerfilInsHlp: Los siguientes campos no pueden ser vacios: " + sError.Substring( 2 ) + " " );
            if (perfil.Descripcion == null || perfil.Descripcion.Trim().Length == 0)
                sError += ", Nombre";
            if ( perfil.Operaciones == null )
                sError += ", Operaciones";
            if ( perfil.Descripcion == null || perfil.Descripcion.Trim( ).Length == 0 )
                sError += ", Descripcion";
            if ( sError.Length > 0 )
                throw new Exception( "PerfilInsHlp: Los siguientes campos no pueden ser vacios: " + sError.Substring( 2 ) + " " );
            if ( dctx == null )
                throw new StandardException( MessageType.Error , "" , "DataContext no puede ser nulo" , "VITADAT.Seguridad.DAO" ,
                   "PerfilInsHlp" , "Action" , null , null );
            DbCommand cmd = null;
            try
            {
                dctx.OpenConnection( myFirm );
                cmd = dctx.CreateCommand( );
            }
            catch ( Exception ex )
            {
                throw new StandardException( MessageType.Error , "" , "PerfilInsHlp: No se pudo conectar con la base de datos" , "VITADAT.Seguridad.DAO" ,
                   "PerfilInsHlp" , "Action" , null , null );
            }
            DbParameter sqlParam;
            StringBuilder sCmd = new StringBuilder( );
            sCmd.Append( " INSERT INTO Perfil (Nombre, Operaciones,Descripcion, Estatus) " );
            sCmd.Append( " VALUES ( " );
            sCmd.Append( " @perfil_Nombre " );
            sqlParam = cmd.CreateParameter( );
            sqlParam.ParameterName = "perfil_Nombre";
            if (perfil.Nombre == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = perfil.Nombre;
            sqlParam.DbType = DbType.String;
            cmd.Parameters.Add( sqlParam );
            sCmd.Append( " ,@perfil_Operaciones " );
            sqlParam = cmd.CreateParameter( );
            sqlParam.ParameterName = "perfil_Operaciones";
            if ( perfil.Operaciones == null )
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = perfil.Operaciones;
            sqlParam.DbType = DbType.Boolean;
            cmd.Parameters.Add( sqlParam );
            sCmd.Append( " ,@perfil_Descripcion " );
            sqlParam = cmd.CreateParameter( );
            sqlParam.ParameterName = "perfil_Descripcion";
            if ( perfil.Descripcion == null )
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = perfil.Descripcion;
            sqlParam.DbType = DbType.String;
            cmd.Parameters.Add( sqlParam );
            sCmd.Append(" ,@perfil_Estatus ");
            sqlParam = cmd.CreateParameter();
            sqlParam.ParameterName = "perfil_Estatus";
            if (perfil.Estatus == null)
                sqlParam.Value = DBNull.Value;
            else
                sqlParam.Value = perfil.Estatus;
            sqlParam.DbType = DbType.Boolean;
            cmd.Parameters.Add(sqlParam);
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
                throw new Exception( "PerfilInsHlp: Se encontraron problemas al crear el registro. " + exmsg );
            }
            finally
            {
                try { dctx.CloseConnection( myFirm ); }
                catch ( Exception ) { }
            }
            if ( iRes < 1 )
                throw new Exception( "PerfilInsHlp: Se encontraron problemas al crear el registro." );
        }
    } 
}
