using System;
using System.Text;
using System.Data;
using System.Data.Common;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Seguridad.BO;
using POV.Seguridad.DAO;

namespace POV.Seguridad.Service
{
    /// <summary>
    /// Servicios para accedes a Acciones
    /// </summary>
    public class AccionCtrl
    {
        /// <summary>
        /// Consulta registros de Accion en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="accion">Accion que provee el criterio de selección para realizar la consulta</param>
        /// <returns>El DataSet que contiene la información de Accion generada por la consulta</returns>
        public DataSet Retrieve ( IDataContext dctx , Accion accion )
        {
            AccionRetHlp da = new AccionRetHlp( );
            DataSet ds = da.Action( dctx , accion );
            return ds;
        }
        /// <summary>
        /// Crea un objeto de Accion a partir de los datos del último DataRow del DataSet.
        /// </summary>
        /// <param name="ds">El DataSet que contiene la información de Accion</param>
        /// <returns>Un objeto de Accion creado a partir de los datos</returns>
        public Accion LastDataRowToAccion ( DataSet ds )
        {
            if ( !ds.Tables.Contains( "Accion" ) )
                throw new Exception( "LastDataRowToAccion: DataSet no tiene la tabla Accion" );
            int index = ds.Tables[ "Accion" ].Rows.Count;
            if ( index < 1 )
                throw new Exception( "LastDataRowToAccion: El DataSet no tiene filas" );
            return this.DataRowToAccion( ds.Tables[ "Accion" ].Rows[ index - 1 ] );
        }
        /// <summary>
        /// Crea un objeto de Accion a partir de los datos de un DataRow.
        /// </summary>
        /// <param name="row">El DataRow que contiene la información de Accion</param>
        /// <returns>Un objeto de Accion creado a partir de los datos</returns>
        public Accion DataRowToAccion ( DataRow row )
        {
            Accion accion = new Accion( );
            if ( row.IsNull( "AccionID" ) )
                accion.AccionID = null;
            else
                accion.AccionID = ( int )Convert.ChangeType( row[ "AccionID" ] , typeof( int ) );
            if ( row.IsNull( "Nombre" ) )
                accion.Nombre = null;
            else
                accion.Nombre = ( String )Convert.ChangeType( row[ "Nombre" ] , typeof( String ) );
            if ( row.IsNull( "Activo" ) )
                accion.Activo = null;
            else
                accion.Activo = ( bool )Convert.ChangeType( row[ "Activo" ] , typeof( bool ) );
            return accion;
        }
    } 
}
