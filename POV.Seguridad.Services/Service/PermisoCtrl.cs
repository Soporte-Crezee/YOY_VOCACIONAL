using System;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;
using System.Linq;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Seguridad.BO;
using POV.Seguridad.DAO;

namespace POV.Seguridad.Service
{
    /// <summary>
    /// Servicios para acceder a Permisos
    /// </summary>
    public class PermisoCtrl
    {
        /// <summary>
        /// Consulta registros de permiso en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="permiso">permiso que provee el criterio de selección para realizar la consulta</param>
        /// <returns>El DataSet que contiene la información de permiso generada por la consulta</returns>
        public DataSet Retrieve ( IDataContext dctx , Permiso permiso )
        {
            PermisoRetHlp da = new PermisoRetHlp( );
            DataSet ds = da.Action( dctx , permiso );
            return ds;
        }
        /// <summary>
        /// Crea un registro de permiso en la base de datos
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="permiso">permiso que desea crear</param>
        public void Insert ( IDataContext dctx , Permiso permiso )
        {
            PermisoInsHlp da = new PermisoInsHlp( );
            da.Action( dctx , permiso );
        }
        /// <summary>
        /// Crea un objeto de permiso a partir de los datos del último DataRow del DataSet.
        /// </summary>
        /// <param name="ds">El DataSet que contiene la información de permiso</param>
        /// <returns>Un objeto de permiso creado a partir de los datos</returns>
        public Permiso LastDataRowToPermiso ( DataSet ds )
        {
            if ( !ds.Tables.Contains( "Permiso" ) )
                throw new Exception( "LastDataRowToPermiso: DataSet no tiene la tabla Permiso" );
            int index = ds.Tables[ "Permiso" ].Rows.Count;
            if ( index < 1 )
                throw new Exception( "LastDataRowToPermiso: El DataSet no tiene filas" );
            return this.DataRowToPermiso( ds.Tables[ "Permiso" ].Rows[ index - 1 ] );
        }
        /// <summary>
        /// Crea un objeto de permiso a partir de los datos de un DataRow.
        /// </summary>
        /// <param name="row">El DataRow que contiene la información de permiso</param>
        /// <returns>Un objeto de permiso creado a partir de los datos</returns>
        public Permiso DataRowToPermiso ( DataRow row )
        {
            Permiso permiso = new Permiso( );
            permiso.Aplicacion = new Aplicacion( );
            permiso.Accion = new Accion( );
            if ( row.IsNull( "PermisoID" ) )
                permiso.PermisoID = null;
            else
                permiso.PermisoID = ( int )Convert.ChangeType( row[ "PermisoID" ] , typeof( int ) );

            if ( row.IsNull( "AplicacionID" ) )
                permiso.Aplicacion.AplicacionID = null;
            else
                permiso.Aplicacion.AplicacionID = ( int )Convert.ChangeType( row[ "AplicacionID" ] , typeof( int ) );
            if ( row.IsNull( "NombreAplicacion" ) )
                permiso.Aplicacion.Nombre = null;
            else
                permiso.Aplicacion.Nombre = ( string )Convert.ChangeType( row[ "NombreAplicacion" ] , typeof( string ) );
            if ( row.IsNull( "AccionID" ) )
                permiso.Accion.AccionID = null;
            else
                permiso.Accion.AccionID = ( int )Convert.ChangeType( row[ "AccionID" ] , typeof( int ) );
            if ( row.IsNull( "NombreAccion" ) )
                permiso.Accion.Nombre = null;
            else
                permiso.Accion.Nombre = ( string )Convert.ChangeType( row[ "NombreAccion" ] , typeof( string ) );
            if ( row.IsNull( "Activo" ) )
                permiso.Activo = null;
            else
                permiso.Activo = ( Boolean )Convert.ChangeType( row[ "Activo" ] , typeof( Boolean ) );
            return permiso;
        }
        public List<Permiso> DataSetToListPermisos ( DataSet ds )
        {
            if ( !ds.Tables.Contains( "Permiso" ) )
                throw new Exception( "DataSetToListPermisos: DataSet no tiene la tabla Permiso" );
            var permisoCtrl = new PermisoCtrl( );
            var listaPermisos = ( from DataRow dr in ds.Tables[ "Permiso" ].Rows select permisoCtrl.DataRowToPermiso( dr ) ).ToList( );
            return listaPermisos;
        }
    } 
}
