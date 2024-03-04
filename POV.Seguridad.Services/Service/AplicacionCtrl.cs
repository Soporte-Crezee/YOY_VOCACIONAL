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
    /// Servicios para acceder a Aplicaciones
    /// </summary>
    public class AplicacionCtrl
    {
        /// <summary>
        /// Consulta registros de aplicacion en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="aplicacion">aplicacion que provee el criterio de selección para realizar la consulta</param>
        /// <returns>El DataSet que contiene la información de aplicacion generada por la consulta</returns>
        public DataSet Retrieve ( IDataContext dctx , Aplicacion aplicacion )
        {
            AplicacionRetHlp da = new AplicacionRetHlp( );
            DataSet ds = da.Action( dctx , aplicacion );
            return ds;
        }
        /// <summary>
        /// Crea un registro de aplicacion en la base de datos
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="aplicacion">aplicacion que desea crear</param>
        public void Insert ( IDataContext dctx , Aplicacion aplicacion )
        {
            AplicacionInsHlp da = new AplicacionInsHlp( );
            da.Action( dctx , aplicacion );
        }
        /// <summary>
        /// Crea un objeto de Aplicacion a partir de los datos del último DataRow del DataSet.
        /// </summary>
        /// <param name="ds">El DataSet que contiene la información de Aplicacion</param>
        /// <returns>Un objeto de Aplicacion creado a partir de los datos</returns>
        public Aplicacion LastDataRowToAplicacion ( DataSet ds )
        {
            if ( !ds.Tables.Contains( "Aplicacion" ) )
                throw new Exception( "LastDataRowToAplicacion: DataSet no tiene la tabla Aplicacion" );
            int index = ds.Tables[ "Aplicacion" ].Rows.Count;
            if ( index < 1 )
                throw new Exception( "LastDataRowToAplicacion: El DataSet no tiene filas" );
            return this.DataRowToAplicacion( ds.Tables[ "Aplicacion" ].Rows[ index - 1 ] );
        }
        /// <summary>
        /// Crea un objeto de Aplicacion a partir de los datos de un DataRow.
        /// </summary>
        /// <param name="row">El DataRow que contiene la información de Aplicacion</param>
        /// <returns>Un objeto de Aplicacion creado a partir de los datos</returns>
        public Aplicacion DataRowToAplicacion ( DataRow row )
        {
            Aplicacion aplicacion = new Aplicacion( );
            if ( row.IsNull( "AplicacionID" ) )
                aplicacion.AplicacionID = null;
            else
                aplicacion.AplicacionID = ( int )Convert.ChangeType( row[ "AplicacionID" ] , typeof( int ) );
            if ( row.IsNull( "Nombre" ) )
                aplicacion.Nombre = null;
            else
                aplicacion.Nombre = ( String )Convert.ChangeType( row[ "Nombre" ] , typeof( String ) );
            if ( row.IsNull( "Activo" ) )
                aplicacion.Activo = null;
            else
                aplicacion.Activo = ( bool )Convert.ChangeType( row[ "Activo" ] , typeof( bool ) );
            return aplicacion;
        }
    } 
}
