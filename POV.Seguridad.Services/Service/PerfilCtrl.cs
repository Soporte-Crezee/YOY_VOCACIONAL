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
    /// Servicios para acceder a Perfiles
    /// </summary>
    public class PerfilCtrl
    {
        /// <summary>
        /// Consulta registros de perfil en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="perfil">perfil que provee el criterio de selección para realizar la consulta</param>
        /// <returns>El DataSet que contiene la información de perfil generada por la consulta</returns>
        public DataSet Retrieve ( IDataContext dctx , Perfil perfil )
        {
            PerfilRetHlp da = new PerfilRetHlp( );
            DataSet ds = da.Action( dctx , perfil );
            return ds;
        }
        /// <summary>
        /// Crea un registro de perfil en la base de datos
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="perfil">perfil que desea crear</param>
        public void Insert ( IDataContext dctx , Perfil perfil )
        {
            PerfilInsHlp da = new PerfilInsHlp( );
            da.Action( dctx , perfil );
        }
        /// <summary>
        /// Crea un registro de Perfil en la base de datos
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="perfil">Perfil que desea crear</param>
        public void InsertComplete(IDataContext dctx, Perfil perfil)
        {
            object miFirma = new object();
            dctx.OpenConnection(miFirma);
            dctx.BeginTransaction(miFirma);
            try
            {
                PerfilInsHlp da = new PerfilInsHlp();
                da.Action(dctx, perfil);
                Perfil p2 = this.LastDataRowToPerfil(this.Retrieve(dctx, perfil));
                perfil.PerfilID = p2.PerfilID;
                PerfilPermisoCtrl pps = new PerfilPermisoCtrl();
                foreach (IPrivilegio priv in perfil.Privilegios)
                {
                    if (!(priv is Permiso)) // Solo se acepta Permiso en un Perfil
                        continue;
                    pps.Insert(dctx, perfil.PerfilID, priv.PrivilegioID);
                }
            }
            catch (Exception e)
            {
                dctx.RollbackTransaction(miFirma);
                dctx.CloseConnection(miFirma);
                throw e;
            }
            dctx.CommitTransaction(miFirma);
            dctx.CloseConnection(miFirma);
        }

        /// <summary>
        /// Consulta registros de Perfil con sus Permisos completos en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="perfilID">El identificador del Perfil para realizar la consulta</param>
        /// <returns>El Perfil con todos sus permisos completos</returns>
        public Perfil RetrieveComplete(IDataContext dctx, int perfilID)
        {
            Perfil perfil = new Perfil();
            perfil.PerfilID = perfilID;
            
            DataSet ds = Retrieve(dctx, perfil);
            perfil = LastDataRowToPerfil(ds);
            PerfilPermisoCtrl ppServ = new PerfilPermisoCtrl();
            DataSet ds2 = ppServ.RetrieveComplete(dctx, (int)perfil.PerfilID);
            foreach (DataRow dr in ds2.Tables["PerfilPermiso"].Rows)
                perfil.AgregarPrivilegio(ppServ.DataRowToPermiso(dr));
            return perfil;
        }

        public void Update(IDataContext dctx, Perfil perfil, Perfil previous)
        {

                PerfilUpdHlp da = new PerfilUpdHlp();
                da.Action(dctx, perfil, previous);

        }

        /// <summary>
        /// Actualiza de manera optimista un registro de Perfil en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="perfil">Perfil que tiene los datos nuevos</param>
        /// <param name="anterior">Perfil que tiene los datos anteriores</param>
        public void UpdateComplete(IDataContext dctx, Perfil perfil, Perfil anterior)
        {
            object miFirma = new object();
            dctx.OpenConnection(miFirma);
            dctx.BeginTransaction(miFirma);
            try
            {
                PerfilUpdHlp pa = new PerfilUpdHlp();
                pa.Action(dctx, perfil, anterior);
                PerfilPermisoCtrl pps = new PerfilPermisoCtrl();
                pps.DeleteByPerfil(dctx, (int)perfil.PerfilID);
                foreach (IPrivilegio priv in perfil.Privilegios)
                {
                    if (!(priv is Permiso)) // Solo se acepta Permiso en un Perfil
                        continue;
                    pps.Insert(dctx, perfil.PerfilID, priv.PrivilegioID);
                }
            }
            catch (Exception e)
            {
                dctx.RollbackTransaction(miFirma);
                dctx.CloseConnection(miFirma);
                throw e;
            }
            dctx.CommitTransaction(miFirma);
            dctx.CloseConnection(miFirma);
        }
        /// <summary>
        /// Elimina un registro de Perfil en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="perfil">Perfil que desea eliminar</param>
        public void Delete(IDataContext dctx, Perfil perfil)
        {
            PerfilDelHlp da = new PerfilDelHlp();
            da.Action(dctx, perfil);
        }
        /// <summary>
        /// Elimina un objeto de Perfil a partir de los datos con todos sus permisos 
        /// </summary>
        /// <param name="dctx">es un IDataContext </param>
        /// <param name="perfil">Perfil que se desea borrar </param>
        /// 

        public void DeleteComplete(IDataContext dctx, Perfil perfil)
        {
            object miFirma = new object();
            dctx.OpenConnection(miFirma);
            dctx.BeginTransaction(miFirma);
            try
            {
                PerfilPermisoCtrl pps = new PerfilPermisoCtrl();
                pps.DeleteByPerfil(dctx, (int)perfil.PerfilID);
                Delete(dctx, perfil);

            }
            catch (Exception e)
            {
                dctx.RollbackTransaction(miFirma);
                dctx.CloseConnection(miFirma);
                throw e;
            }
            dctx.CommitTransaction(miFirma);
            dctx.CloseConnection(miFirma);
        }
        /// <summary>
        /// Crea un objeto de perfil a partir de los datos del último DataRow del DataSet.
        /// </summary>
        /// <param name="ds">El DataSet que contiene la información de perfil</param>
        /// <returns>Un objeto de perfil creado a partir de los datos</returns>
        public Perfil LastDataRowToPerfil ( DataSet ds )
        {
            if ( !ds.Tables.Contains( "Perfil" ) )
                throw new Exception( "LastDataRowToPerfil: DataSet no tiene la tabla Perfil" );
            int index = ds.Tables[ "Perfil" ].Rows.Count;
            if ( index < 1 )
                throw new Exception( "LastDataRowToPerfil: El DataSet no tiene filas" );
            return this.DataRowToPerfil( ds.Tables[ "Perfil" ].Rows[ index - 1 ] );
        }
        /// <summary>
        /// Crea un objeto de perfil a partir de los datos de un DataRow.
        /// </summary>
        /// <param name="row">El DataRow que contiene la información de perfil</param>
        /// <returns>Un objeto de perfil creado a partir de los datos</returns>
        public Perfil DataRowToPerfil ( DataRow row )
        {
            Perfil perfil = new Perfil( );
            if ( row.IsNull( "PerfilID" ) )
                perfil.PerfilID = null;
            else
                perfil.PerfilID = ( int )Convert.ChangeType( row[ "PerfilID" ] , typeof( int ) );
            if (row.IsNull("Nombre"))
                perfil.Nombre = null;
            else
                perfil.Nombre = (string)Convert.ChangeType(row["Nombre"], typeof(string));
            if ( row.IsNull( "Operaciones" ) )
                perfil.Operaciones = null;
            else
                perfil.Operaciones = ( bool )Convert.ChangeType( row[ "Operaciones" ] , typeof( bool ) );
            if ( row.IsNull( "Descripcion" ) )
                perfil.Descripcion = null;
            else
                perfil.Descripcion = ( string )Convert.ChangeType( row[ "Descripcion" ] , typeof( string ) );
            if (row.IsNull("Estatus"))
                perfil.Estatus = null;
            else
                perfil.Estatus = (bool)Convert.ChangeType(row["Estatus"], typeof(bool));
            return perfil;
        }
    } 
}
