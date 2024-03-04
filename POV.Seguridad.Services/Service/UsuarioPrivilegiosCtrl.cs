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
using POV.CentroEducativo.BO;
using POV.CentroEducativo.Service;

namespace POV.Seguridad.Service
{
    /// <summary>
    /// Servicios para los UsuarioPrivilegios del sistema
    /// </summary>
    public class UsuarioPrivilegiosCtrl
    {
        /// <summary>
        /// Crea un registro de UsuarioPrivilegios en la base de datos
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="usuarioPrivilegios">UsuarioPrivilegios que desea crear</param>
        public void Insert(IDataContext dctx, UsuarioPrivilegios usuarioPrivilegios)
        {
            UsuarioPrivilegiosInsHlp da = new UsuarioPrivilegiosInsHlp();
            da.Action(dctx, usuarioPrivilegios);
        }


        public void InsertUsuarioEscolarPrivilegios(IDataContext dctx, Usuario usuario, Escuela escuela, CicloEscolar cicloEscolar, List<IPrivilegio> privilegios)
        {
            #region validaciones
            if (usuario == null)
                throw new Exception("El usuario es requerido");
            if (escuela == null)
                throw new Exception("La escuela es requerido");
            if (escuela.EscuelaID == null)
                throw new Exception("EscuelaID es requerido");
            if (cicloEscolar == null)
                throw new Exception("El cicloEscolar es requerido");
            if (cicloEscolar.CicloEscolarID == null)
                throw new Exception("CicloEscolarID es requerido");
            if (usuario.UsuarioID == null)
                throw new Exception("El identificador del usuario es requerido");
            if (privilegios == null)
                throw new Exception("La lista de privilegios no puede ser nula");
            if (privilegios.Count < 1)
                throw new Exception("La lista de privilegios no puede ser vacia");

            foreach (IPrivilegio privilegio in privilegios)
            {
                if (privilegio.PrivilegioID == null)
                    throw new Exception("El identificador del privilegio no puede ser nulo");
            }
            #endregion

            object myFirm = new object();
            dctx.OpenConnection(myFirm);
            dctx.BeginTransaction(myFirm);
            try
            {
                UsuarioCtrl usuarioCtrl = new UsuarioCtrl();
                
                //verificamos que el usuario privilegios exista
                UsuarioPrivilegios usuarioPrivilegios = new UsuarioEscolarPrivilegios { Usuario = usuario, Escuela = escuela, CicloEscolar = cicloEscolar };

                #region UsuarioPrivilegios
                DataSet dsUsuarioPrivilegios = Retrieve(dctx, usuarioPrivilegios);

                if (dsUsuarioPrivilegios.Tables[0].Rows.Count > 0)
                {
                    usuarioPrivilegios = LastDataRowToUsuarioPrivilegios(dsUsuarioPrivilegios);
                    //si esta desactivado, lo activa
                    if (!usuarioPrivilegios.Estado.Value)
                    {
                        usuarioPrivilegios.Estado = true;
                        Update(dctx, usuarioPrivilegios, usuarioPrivilegios);
                    }
                }
                else
                {
                    usuarioPrivilegios.FechaCreacion = DateTime.Now;
                    usuarioPrivilegios.Estado = true;
                    Insert(dctx, usuarioPrivilegios);

                    usuarioPrivilegios = LastDataRowToUsuarioPrivilegios(Retrieve(dctx, usuarioPrivilegios));
                }

                #endregion  

                UsuarioAccesoCtrl usuarioAccesoCtrl = new UsuarioAccesoCtrl();

                #region UsuarioAcceso

                DataSet dsUsuarioAcceso = usuarioAccesoCtrl.RetrieveByUsuarioPrivilegiosID(dctx, usuarioPrivilegios);
                List<IPrivilegio> listaInsertarPrivilegios = new List<IPrivilegio>();
                //verificamos que no tenga el privilegio ya asignado
                if (dsUsuarioAcceso.Tables[0].Rows.Count > 0) // si existen usuariosAcceso verificamos que no tenga repetido el privilegio
                {
                    foreach (IPrivilegio privilegio in privilegios)
                    {
                        bool exist = false;
                        foreach (DataRow drUsuarioAcceso in dsUsuarioAcceso.Tables[0].Rows)
                        {
                            UsuarioAcceso usuarioAcceso = usuarioAccesoCtrl.DataRowToUsuarioAcceso(drUsuarioAcceso);
                            usuarioAcceso = usuarioAccesoCtrl.RetrieveComplete(dctx, usuarioPrivilegios, usuarioAcceso);

                            if (usuarioAcceso.Privilegio.PrivilegioID == privilegio.PrivilegioID)
                            {
                                
                                exist = true;
                                break;
                            }
                        }

                        if (!exist)
                            listaInsertarPrivilegios.Add(privilegio);
                    }
                }
                else
                    listaInsertarPrivilegios = privilegios;

                PrivilegioCtrl pserv = new PrivilegioCtrl();
                //se insertan los privilegios que no han sido asignados
                foreach (IPrivilegio privilegio in listaInsertarPrivilegios)
                {
                    UsuarioAcceso usuarioAcceso = new UsuarioAcceso();
                    usuarioAcceso.FechaAsignacion = DateTime.Now;
                    usuarioAcceso.Estado = true;
                    usuarioAcceso.UsuarioAsigno = new Usuario();
                    usuarioAcceso.Privilegio = privilegio;

                    usuarioAccesoCtrl.Insert(dctx, usuarioPrivilegios, usuarioAcceso);
                    usuarioAcceso.UsuarioAccesoID = usuarioAccesoCtrl.LastDataRowToUsuarioAcceso(usuarioAccesoCtrl.Retrieve(dctx,usuarioPrivilegios, usuarioAcceso)).UsuarioAccesoID;
                    if (privilegio.TipoPrivilegio.CompareTo("PERFIL") == 0)
                        pserv.Insert(dctx, usuarioAcceso, privilegio.PrivilegioID, null); // Insertar el Privilegio
                    else
                        pserv.Insert(dctx, usuarioAcceso, null, privilegio.PrivilegioID);
                }

                #endregion

                dctx.CommitTransaction(myFirm);
            }
            catch (Exception ex)
            {
                dctx.RollbackTransaction(myFirm);
                dctx.CloseConnection(myFirm);
                throw ex;
            }
            finally
            {
                if (dctx.ConnectionState == ConnectionState.Open)
                    dctx.CloseConnection(myFirm);
            }
        }
        
        /// <summary>
        /// Crea un regitro de UsuarioPrivilegios con sus respectivos
        /// UsuarioAcceso en la base de datos
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="usuarioPrivilegios">UsuarioPrivilegios que desea crear</param>
        private void InsertComplete(IDataContext dctx, UsuarioPrivilegios usuarioPrivilegios)
        {
            object firma = new object();
            dctx.OpenConnection(firma);
            dctx.BeginTransaction(firma);
            UsuarioAccesoCtrl uaserv = new UsuarioAccesoCtrl();
            PrivilegioCtrl pserv = new PrivilegioCtrl();
            UsuarioAcceso ua2;
            try
            {
                UsuarioCtrl us = new UsuarioCtrl();
                us.Insert(dctx, usuarioPrivilegios.Usuario);
                usuarioPrivilegios.Usuario = us.LastDataRowToUsuario(us.Retrieve(dctx, usuarioPrivilegios.Usuario));

                this.Insert(dctx, usuarioPrivilegios); // Insertar el UsuarioPrivilegios
                UsuarioPrivilegios up2 = this.LastDataRowToUsuarioPrivilegios(this.Retrieve(dctx, usuarioPrivilegios));
                usuarioPrivilegios.UsuarioPrivilegiosID = up2.UsuarioPrivilegiosID;
                foreach (UsuarioAcceso ua1 in usuarioPrivilegios.UsuarioAccesos)
                {
                    uaserv.Insert(dctx, usuarioPrivilegios, ua1); // Insertar el UsuarioAcceso
                    ua2 = uaserv.LastDataRowToUsuarioAcceso(uaserv.Retrieve(dctx, usuarioPrivilegios, ua1));
                    ua1.UsuarioAccesoID = ua2.UsuarioAccesoID;
                    if (ua1.Privilegio.TipoPrivilegio.CompareTo("PERFIL") == 0)
                        pserv.Insert(dctx, ua1, ua1.Privilegio.PrivilegioID, null); // Insertar el Privilegio
                    else
                        pserv.Insert(dctx, ua1, null, ua1.Privilegio.PrivilegioID);
                }
            }
            catch (Exception e)
            {
                dctx.RollbackTransaction(firma);
                dctx.CloseConnection(firma);
                throw e;
            }
            dctx.CommitTransaction(firma);
            dctx.CloseConnection(firma);
        }
        /// <summary>
        /// Consulta registros de UsuarioPrivilegios en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="usuarioPrivilegios">UsuarioPrivilegios que provee el criterio de selección para realizar la consulta</param>
        /// <returns>El DataSet que contiene la información de UsuarioPrivilegios generada por la consulta</returns>
        public DataSet Retrieve(IDataContext dctx, UsuarioPrivilegios usuarioPrivilegios)
        {
            UsuarioPrivilegiosRetHlp da = new UsuarioPrivilegiosRetHlp();
            DataSet ds = da.Action(dctx, usuarioPrivilegios);
            return ds;
        }

        public DataSet RetrieveByUsuario(IDataContext dctx, UsuarioPrivilegios usuarioPrivilegios)
        {
            UsuarioPrivilegiosUsuarioRetHlp da = new UsuarioPrivilegiosUsuarioRetHlp();
            DataSet ds = da.Action(dctx, usuarioPrivilegios);

            return ds;
        }

        /// <summary>
        /// Consulta registros de UsuarioPrivilegios en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="usuarioPrivilegios">UsuarioPrivilegios que provee el criterio de selección para realizar la consulta</param>
        /// <returns>El UsuarioPrivilegios generada por la consulta</returns>
        public UsuarioPrivilegios RetrieveComplete(IDataContext dctx, UsuarioPrivilegios usuarioPrivilegios)
        {
            UsuarioAccesoCtrl uaserv = new UsuarioAccesoCtrl();
            PerfilCtrl perfilServ = new PerfilCtrl();
            PermisoCtrl permisoServ = new PermisoCtrl();
            UsuarioCtrl userv = new UsuarioCtrl();
            UsuarioAcceso ua;
            Perfil perfil;
            Permiso permiso;
            IPrivilegio privilegio = null;
            DataSet dsPrivilegios = this.Retrieve(dctx, usuarioPrivilegios);
            UsuarioPrivilegios up = null;
            if (dsPrivilegios.Tables[0].Rows.Count < 1)
                return up;

            up = this.LastDataRowToUsuarioPrivilegios(dsPrivilegios);

            up.Usuario = userv.LastDataRowToUsuario(userv.Retrieve(dctx, up.Usuario));
            DataSet uads = uaserv.RetrieveByUsuarioPrivilegiosID(dctx, up);
            foreach (DataRow dr in uads.Tables["UsuarioAcceso"].Rows)
            {
                if (dr.IsNull("Privilegio_PerfilID") && dr.IsNull("Privilegio_PermisoID"))
                    throw new Exception("Error CT2412.- UsuarioPrivilegiosServicios.ConsultarCompleto: PerfilID y PermisoID son nulos para " + dr["Privilegio_PrivilegioID"].ToString());
                ua = uaserv.DataRowToUsuarioAcceso(dr);
                if (!dr.IsNull("Privilegio_PerfilID"))
                {
                    perfil = perfilServ.RetrieveComplete(dctx, (int)dr["Privilegio_PerfilID"]);
                    privilegio = perfil;
                }
                else if (!dr.IsNull("Privilegio_PermisoID"))
                {
                    permiso = new Permiso();
                    permiso.PermisoID = (int?)dr["Privilegio_PermisoID"];
                    permiso.Accion = new Accion();
                    permiso.Aplicacion = new Aplicacion();
                    permiso = permisoServ.LastDataRowToPermiso(permisoServ.Retrieve(dctx, permiso));
                    privilegio = permiso;
                }
                ua.Privilegio = privilegio;
                up.AgregarUsuarioAcceso(ua);
            }
            return up;
        }
        /// <summary>
        /// Actualiza de manera optimista un registro de UsuarioPrivilegios en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="usuarioPrivilegios">UsuarioPrivilegios que tiene los datos nuevos</param>
        /// <param name="anterior">UsuarioPrivilegios que tiene los datos anteriores</param>
        public void Update(IDataContext dctx, UsuarioPrivilegios usuarioPrivilegios, UsuarioPrivilegios previous)
        {
            UsuarioPrivilegiosUpdHlp da = new UsuarioPrivilegiosUpdHlp();
            da.Action(dctx, usuarioPrivilegios, previous);
        }
        /// <summary>
        /// Actualiza de manera optimista un registro de UsuarioPrivilegios en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="usuarioPrivilegios">UsuarioPrivilegios que tiene los datos nuevos</param>
        /// <param name="anterior">UsuarioPrivilegios que tiene los datos anteriores</param>
        public void UpdateComplete(IDataContext dctx, UsuarioPrivilegios usuarioPrivilegios, UsuarioPrivilegios anterior)
        {
            object firma = new object();
            dctx.OpenConnection(firma);
            dctx.BeginTransaction(firma);
            UsuarioAccesoCtrl uaserv = new UsuarioAccesoCtrl();
            PrivilegioCtrl pserv = new PrivilegioCtrl();
            UsuarioAcceso ua3;
            try
            {
                this.Update(dctx, usuarioPrivilegios, anterior);
                foreach (UsuarioAcceso ua2 in anterior.UsuarioAccesos)
                { // Eliminar los Privilegio
                    pserv.Delete(dctx, ua2);
                }

                if (anterior.UsuarioAccesos.Count > 0)
                 uaserv.DeleteByUsuarioPrivilegios(dctx, usuarioPrivilegios); // Eliminar los UsuarioAcceso
                foreach (UsuarioAcceso ua1 in usuarioPrivilegios.UsuarioAccesos)
                {
                    ua1.UsuarioAccesoID = null;
                    uaserv.Insert(dctx, usuarioPrivilegios, ua1); // Insertar el UsuarioAcceso
                    ua3 = uaserv.LastDataRowToUsuarioAcceso(uaserv.Retrieve(dctx, usuarioPrivilegios, ua1));
                    ua1.UsuarioAccesoID = ua3.UsuarioAccesoID;
                    if (ua1.Privilegio.TipoPrivilegio.CompareTo("PERFIL") == 0)
                        pserv.Insert(dctx, ua1, ua1.Privilegio.PrivilegioID, null); // Insertar el Privilegio
                    else
                        pserv.Insert(dctx, ua1, null, ua1.Privilegio.PrivilegioID);
                }
            }
            catch (Exception e)
            {
                dctx.RollbackTransaction(firma);
                dctx.CloseConnection(firma);
                throw e;
            }
            dctx.CommitTransaction(firma);
            dctx.CloseConnection(firma);
        }
        /// <summary>
        /// Crea un objeto de UsuarioPrivilegios a partir de los datos del último DataRow del DataSet.
        /// </summary>
        /// <param name="ds">El DataSet que contiene la información de UsuarioPrivilegios</param>
        /// <returns>Un objeto de UsuarioPrivilegios creado a partir de los datos</returns>
        public UsuarioPrivilegios LastDataRowToUsuarioPrivilegios(DataSet ds)
        {
            if (!ds.Tables.Contains("UsuarioPrivilegios"))
                throw new Exception("Error CT2413.- LastDataRowToUsuarioPrivilegios: DataSet no tiene la tabla UsuarioPrivilegios");
            int index = ds.Tables["UsuarioPrivilegios"].Rows.Count;
            if (index < 1)
                throw new Exception("Error CT2414.- LastDataRowToUsuarioPrivilegios: El DataSet no tiene filas");
            return this.DataRowToUsuarioPrivilegios(ds.Tables["UsuarioPrivilegios"].Rows[index - 1]);
        }
        /// <summary>
        /// Crea un objeto de UsuarioPrivilegios a partir de los datos de un DataRow.
        /// </summary>
        /// <param name="row">El DataRow que contiene la información de UsuarioPrivilegios</param>
        /// <returns>Un objeto de UsuarioPrivilegios creado a partir de los datos</returns>
        public UsuarioPrivilegios DataRowToUsuarioPrivilegios(DataRow row)
        {

            UsuarioPrivilegios usuarioPrivilegios;

            if (!row.IsNull("EscuelaID") && !row.IsNull("CicloEscolarID"))
            {
                usuarioPrivilegios = new UsuarioEscolarPrivilegios();
                (usuarioPrivilegios as UsuarioEscolarPrivilegios).Escuela = new Escuela();
                (usuarioPrivilegios as UsuarioEscolarPrivilegios).CicloEscolar = new CicloEscolar();

                (usuarioPrivilegios as UsuarioEscolarPrivilegios).Escuela.EscuelaID = (int)Convert.ChangeType(row["EscuelaID"], typeof(int));
                (usuarioPrivilegios as UsuarioEscolarPrivilegios).CicloEscolar.CicloEscolarID = (int)Convert.ChangeType(row["CicloEscolarID"], typeof(int));
                
            }
            else
            {
                usuarioPrivilegios = new UsuarioPrivilegios();
            }

            usuarioPrivilegios.Usuario = new Usuario();
            if (row.IsNull("UsuarioPrivilegiosID"))
                usuarioPrivilegios.UsuarioPrivilegiosID = null;
            else
                usuarioPrivilegios.UsuarioPrivilegiosID = (int)Convert.ChangeType(row["UsuarioPrivilegiosID"], typeof(int));
            if (row.IsNull("UsuarioID"))
                usuarioPrivilegios.Usuario.UsuarioID = null;
            else
                usuarioPrivilegios.Usuario.UsuarioID = (int)Convert.ChangeType(row["UsuarioID"], typeof(int));
            if (row.IsNull("FechaCreacion"))
                usuarioPrivilegios.FechaCreacion = null;
            else
                usuarioPrivilegios.FechaCreacion = (DateTime)Convert.ChangeType(row["FechaCreacion"], typeof(DateTime));
            if (row.IsNull("Estado"))
                usuarioPrivilegios.Estado = null;
            else
                usuarioPrivilegios.Estado = (bool)Convert.ChangeType(row["Estado"], typeof(bool));
            return usuarioPrivilegios;
        }
    }
}
