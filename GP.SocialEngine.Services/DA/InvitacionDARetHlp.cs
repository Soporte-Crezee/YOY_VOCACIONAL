using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Framework.Base.DataAccess;
using System.Data.Common;
using GP.SocialEngine.BO;

namespace GP.SocialEngine.DA
{
    public class InvitacionDARetHlp
    {
        public DataSet Action ( IDataContext dctx , UsuarioSocial usuarioSocial )
        {                   
            //variables
            string swhere = string.Empty;
            string sError = string.Empty;
            StringBuilder query = new StringBuilder( );
            StringBuilder queryControl = new StringBuilder( );
            StringBuilder queryCols = new StringBuilder( );
            DbParameter dbParameter = null;
            DbCommand dbCommand = null;
            DbCommand dbCommandControl = null;
            object myFirm = new object( );

            try
            {
                dctx.OpenConnection( myFirm );
                dbCommand = dctx.CreateCommand( );
                dbCommandControl = dctx.CreateCommand( );
            }
            catch ( Exception ex )
            {
                throw new Exception( string.Format( "PublicacionDARetHlp: No se pudo conectar con la base de datos {0}" , ex.Message ) );
            }

            #region ControlData
            queryControl.Append( " SELECT " );
            queryControl.Append( " InvitacionID, FechaRegistro, Estatus, EsSolicitud, RemitenteID, InvitadoID, Saludo " );
            queryControl.Append( " Invitacion " );

            if ( usuarioSocial.UsuarioSocialID != null )
            {
                if ( swhere.Length > 0 ) swhere += " AND ";
                swhere += "InvitadoID =  @InvitadoID";
                dbParameter = dbCommand.CreateParameter( );
                dbParameter.ParameterName = "@InvitadoID";
                dbParameter.DbType = DbType.Int64;
                dbParameter.Value = usuarioSocial.ScreenName;
                dbCommandControl.Parameters.Add( dbParameter );
            }
            if ( usuarioSocial.UsuarioSocialID!=null )
            {
                if ( swhere.Length > 0 ) swhere += " OR ";
                swhere += "RemitenteID = @RemitenteID";
                dbParameter = dbCommand.CreateParameter( );
                dbParameter.ParameterName = "@RemitenteID";
                dbParameter.DbType = DbType.Int64;
                dbParameter.Value = usuarioSocial.UsuarioSocialID;
                dbCommandControl.Parameters.Add( dbParameter );
            }
            if ( swhere.Length > 0 ) queryControl.AppendFormat( " WHERE {0}" , swhere );

            DataSet ds = new DataSet( );
            try
            {
                DbDataAdapter adapter = dctx.CreateDataAdapter( );
                dbCommandControl.CommandText = queryControl.ToString( );
                adapter.SelectCommand = dbCommandControl;
                adapter.Fill( ds , "ControlData" );               
            }
            catch ( Exception ex )
            {
                throw new Exception( string.Format( "PublicacionDARetHlp: Ocurrió un problema al recuperar los datos de control: {0}" , ex.Message ) );
            }
            #endregion
           
            return ds;
        }
    }
}
