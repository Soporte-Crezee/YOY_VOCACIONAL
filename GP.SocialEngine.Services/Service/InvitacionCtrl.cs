using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Framework.Base.DataAccess;
using GP.SocialEngine.BO;
using GP.SocialEngine.DAO;

namespace GP.SocialEngine.Service
{
    /// <summary>
    /// Controlador de invitacion
    /// </summary>
    public class InvitacionCtrl
    {
        /// <summary>
        /// Crea un registro de Inserta una Invitacion en la base de datos en la base de datos
        /// </summary>
        /// <param name="dctx">El DataContext que proveerÃ¡ acceso a la base de datos</param>
        /// <param name="insertaunaInvitacionenlabasededatos">Inserta una Invitacion en la base de datos que desea crear</param>
        public void Insert(IDataContext dctx, Invitacion invitacion)
        {
            InvitacionInsHlp da = new InvitacionInsHlp();
            da.Action(dctx, invitacion);
        }
        /// <summary>
        /// Consulta registros de Consulta las Invitaciones en la Base de Datos en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerÃ¡ acceso a la base de datos</param>
        /// <param name="consultalasInvitacionesenlaBasedeDatos">Consulta las Invitaciones en la Base de Datos que provee el criterio de selecciÃ³n para realizar la consulta</param>
        /// <returns>El DataSet que contiene la informaciÃ³n de Consulta las Invitaciones en la Base de Datos generada por la consulta</returns>
        public DataSet Retrieve(IDataContext dctx, Invitacion invitacion)
        {
            InvitacionRetHlp da = new InvitacionRetHlp();
            DataSet ds = da.Action(dctx, invitacion);
            return ds;
        }
        /// <summary>
        /// Devuelve un registro completo de una invitacion
        /// </summary>
        public Invitacion RetrieveComplete(IDataContext dctx, Invitacion invitacion)
        {
            UsuarioSocialCtrl usuarioSocialCtrl = new UsuarioSocialCtrl();
            ContactoInvitadoCtrl contactoInvitadoCtrl = new ContactoInvitadoCtrl();
            invitacion = LastDataRowToInvitacion(Retrieve(dctx, invitacion));
            invitacion.Remitente = usuarioSocialCtrl.LastDataRowToUsuarioSocial(usuarioSocialCtrl.Retrieve(dctx, new UsuarioSocial { UsuarioSocialID = invitacion.Remitente.UsuarioSocialID }));
            if ((bool)invitacion.EsSolicitud)
            {
                invitacion.Invitado = usuarioSocialCtrl.LastDataRowToUsuarioSocial(usuarioSocialCtrl.Retrieve(dctx, new UsuarioSocial { UsuarioSocialID = invitacion.InvitadoID }));
            }
            else
            {
                invitacion.Invitado = contactoInvitadoCtrl.LastDataRowToContactoInvitado(contactoInvitadoCtrl.Retrieve(dctx, new ContactoInvitado { ContactoInvitadoID = invitacion.InvitadoID }));
            }
            return invitacion;
        }


        /// <summary>
        /// Verifica si existe una invitacion pendiente de dos usuarios ya sea como remitente o como invitado
        /// </summary>
        /// <param name="dctx"></param>
        /// <param name="potencialRemitente"></param>
        /// <param name="potencialInvitado"></param>
        /// <returns></returns>
        public bool ExistUsuarioSocialConInvitacionPendiente(IDataContext dctx, UsuarioSocial potencialRemitente, UsuarioSocial potencialInvitado)
        {
            bool exist = false;
            //checar que no exista una invitacion como remitente
            DataSet dsInvitacion = Retrieve(dctx, new Invitacion { Remitente = potencialRemitente, Invitado = potencialInvitado, EsSolicitud = true, Estatus = EEstatusInvitacion.PENDIENTE });
            if (dsInvitacion.Tables["Invitacion"].Rows.Count > 0)
                exist = true;
            if (!exist)
            {
                DataSet dsInvitacionInvitado = Retrieve(dctx, new Invitacion { Remitente = potencialInvitado, Invitado = potencialRemitente, Estatus = EEstatusInvitacion.PENDIENTE, EsSolicitud = true });

                if (dsInvitacionInvitado.Tables["Invitacion"].Rows.Count > 0)
                    exist = true;
            }

            return exist;
        }

        public bool ExistInvitacion(IDataContext dctx, UsuarioSocial potencialRemitente, UsuarioSocial potencialInvitado)
        {
            bool exist = false;

            DataSet dsInvitacion = Retrieve(dctx, new Invitacion { Remitente = potencialRemitente, Invitado = potencialInvitado, EsSolicitud = true, Estatus = EEstatusInvitacion.PENDIENTE });
            if (dsInvitacion.Tables["Invitacion"].Rows.Count > 0)
                exist = true;

            return exist;
        }

        /// <summary>
        /// Inserta una nueva o actualiza una invitación, si la verificacion determina que no esta en estado PENDIENTE 
        /// </summary>
        /// <param name="dctx"></param>
        /// <param name="invitacion"></param>
        public void InsertOrUpdateInvitacion(IDataContext dctx, Invitacion invitacion)
        {
            object myFirm = new object();
            dctx.OpenConnection(myFirm);
            dctx.BeginTransaction(myFirm);
            try
            {
                //checar que no exista la invitacion.

                DataSet dsInvitacion = Retrieve(dctx, new Invitacion { Remitente = invitacion.Remitente, Invitado = invitacion.Invitado, EsSolicitud = true });
                if (dsInvitacion.Tables["Invitacion"].Rows.Count <= 0)
                {
                    //checar que no tenga una invitacion del invitado pendiente.
                    DataSet dsInvitacionInvitado = Retrieve(dctx, new Invitacion { Remitente = (UsuarioSocial)invitacion.Invitado, Invitado = invitacion.Remitente, Estatus = EEstatusInvitacion.PENDIENTE, EsSolicitud = true });

                    if (dsInvitacionInvitado.Tables["Invitacion"].Rows.Count <= 0)
                    {
                        //de parte del invitado no existe una invitacion para mi, por lo tanto insertamos
                        Insert(dctx, invitacion);
                        InsertNotificacionInvitacionRecibida(dctx, invitacion);
                    }
                    else
                    {
                        Invitacion invitacionBD = LastDataRowToInvitacion(dsInvitacionInvitado);
                        if (invitacionBD.Estatus != EEstatusInvitacion.PENDIENTE)
                        {
                            invitacionBD.Estatus = EEstatusInvitacion.PENDIENTE;
                            Update(dctx, invitacionBD, invitacionBD);
                            InsertNotificacionInvitacionRecibida(dctx, invitacionBD);
                        }

                    }

                }
                else
                {
                    Invitacion invitacionBD = LastDataRowToInvitacion(dsInvitacion);

                    // existe la  invitacion pero no esta pendiente( en algun momento del tiempo fue aceptada o rechazada)
                    if (invitacionBD.Estatus != EEstatusInvitacion.PENDIENTE)
                    {
                        invitacionBD.Estatus = EEstatusInvitacion.PENDIENTE;
                        Update(dctx, invitacionBD, invitacionBD);
                        InsertNotificacionInvitacionRecibida(dctx, invitacionBD);
                    }
                }
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
        /// Inserta una notificacion de invitacion de contacto recibida
        /// </summary>
        /// <param name="dctx"></param>
        /// <param name="invitacion"></param>
        public void InsertNotificacionInvitacionRecibida(IDataContext dctx, Invitacion invitacion)
        {
            Notificacion notificacion = new Notificacion();
            notificacion.NotificacionID = Guid.NewGuid();
            notificacion.FechaRegistro = invitacion.FechaRegistro;
            notificacion.Emisor = invitacion.Remitente;
            notificacion.Receptor = (UsuarioSocial)invitacion.Invitado;
            notificacion.Notificable = invitacion;
            notificacion.TipoNotificacion = ETipoNotificacion.INVITACION_RECIBIDA;
            notificacion.EstatusNotificacion = EEstatusNotificacion.NUEVO;

            NotificacionCtrl notificacionCtrl = new NotificacionCtrl();
            notificacionCtrl.Insert(dctx, notificacion);
        }
        /// <summary>
        /// Actualiza una invitacion aceptada y notifica que fue aceptada.
        /// </summary>
        /// <param name="dctx"></param>
        /// <param name="invitacion"></param>
        public void UpdateCompleteInvitacionAceptada(IDataContext dctx, Invitacion invitacion, Invitacion previous)
        {
            if (invitacion.Estatus != EEstatusInvitacion.ACEPTADA)
                return;
            object myFirm = new object();
            dctx.OpenConnection(myFirm);
            dctx.BeginTransaction(myFirm);
            try
            {
                Update(dctx, invitacion, previous);
                InsertNotificacionInvitacionAceptada(dctx,invitacion);
                NotificacionCtrl notificacionCtrl = new NotificacionCtrl();

                Notificacion notificacionUpd = notificacionCtrl.LastDataRowToNotificacion(notificacionCtrl.Retrieve(dctx,  new Notificacion { Emisor = invitacion.Remitente, Receptor = (UsuarioSocial)invitacion.Invitado, TipoNotificacion= ETipoNotificacion.INVITACION_RECIBIDA }));

                notificacionUpd.EstatusNotificacion = EEstatusNotificacion.CONFIRMADO;
                
                notificacionCtrl.Update(dctx, notificacionUpd, notificacionUpd);

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
        /// Inserta una notificacion de invitacion aceptada
        /// </summary>
        /// <param name="dctx"></param>
        /// <param name="invitacion"></param>
        public void InsertNotificacionInvitacionAceptada(IDataContext dctx, Invitacion invitacion)
        {
            if ((bool)invitacion.EsSolicitud && invitacion.Estatus == EEstatusInvitacion.ACEPTADA)
            {
                Notificacion notificacion = new Notificacion();
                notificacion.NotificacionID = Guid.NewGuid();
                notificacion.FechaRegistro = DateTime.Now;
                notificacion.Emisor = (UsuarioSocial)invitacion.Invitado;
                notificacion.Receptor = invitacion.Remitente;
                notificacion.Notificable = invitacion;
                notificacion.TipoNotificacion = ETipoNotificacion.INVITACION_ACEPTADA;
                notificacion.EstatusNotificacion = EEstatusNotificacion.NUEVO;

                NotificacionCtrl notificacionCtrl = new NotificacionCtrl();
                notificacionCtrl.Insert(dctx, notificacion);
            }
        }

        /// <summary>
        /// Actualiza la invitacion a un estado de rechazada y elimina la notificacion asociada.
        /// </summary>
        /// <param name="dctx"></param>
        /// <param name="invitacion"></param>
        public void RechazarInvitacion(IDataContext dctx, Invitacion invitacion)
        {
            object myFirm = new object();
            dctx.OpenConnection(myFirm);
            dctx.BeginTransaction(myFirm);
            try
            {

                invitacion = LastDataRowToInvitacion(Retrieve(dctx, invitacion));
                invitacion.Estatus = EEstatusInvitacion.RECHAZADA;
                Update(dctx, invitacion, invitacion);

                NotificacionCtrl notificacionCtrl = new NotificacionCtrl();
                notificacionCtrl.Delete(dctx, new Notificacion { Emisor = invitacion.Remitente, Receptor = (UsuarioSocial)invitacion.Invitado, TipoNotificacion= ETipoNotificacion.INVITACION_RECIBIDA });

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
        /// Actualiza de manera optimista un registro de Actualiza Las invitaciones de la base de datos en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerÃ¡ acceso a la base de datos</param>
        /// <param name="actualizaLasinvitacionesdelabasededatos">Actualiza Las invitaciones de la base de datos que tiene los datos nuevos</param>
        /// <param name="anterior">Actualiza Las invitaciones de la base de datos que tiene los datos anteriores</param>
        public void Update(IDataContext dctx, Invitacion invitacion, Invitacion previous)
        {
            InvitacionUpdHlp da = new InvitacionUpdHlp();
            da.Action(dctx, invitacion, previous);
        }
        /// <summary>
        /// Crea un objeto de mapea un DataRow a Invitacion a partir de los datos del Ãºltimo DataRow del DataSet.
        /// </summary>
        /// <param name="ds">El DataSet que contiene la informaciÃ³n de mapea un DataRow a Invitacion</param>
        /// <returns>Un objeto de mapea un DataRow a Invitacion creado a partir de los datos</returns>
        public Invitacion LastDataRowToInvitacion(DataSet ds)
        {
            if (!ds.Tables.Contains("Invitacion"))
                throw new Exception("LastDataRowToInvitacion: DataSet no tiene la tabla Invitacion");
            int index = ds.Tables["Invitacion"].Rows.Count;
            if (index < 1)
                throw new Exception("LastDataRowToInvitacion: El DataSet no tiene filas");
            return this.DataRowToInvitacion(ds.Tables["Invitacion"].Rows[index - 1]);
        }


        
        /// <summary>
        /// Crea un objeto de mapea un DataRow a Invitacion a partir de los datos de un DataRow.
        /// </summary>
        /// <param name="row">El DataRow que contiene la informaciÃ³n de mapea un DataRow a Invitacion</param>
        /// <returns>Un objeto de mapea un DataRow a Invitacion creado a partir de los datos</returns>
        public Invitacion DataRowToInvitacion(DataRow row)
        {
            Invitacion invitacion = new Invitacion();
            invitacion.Remitente = new UsuarioSocial();
            if (row.IsNull("InvitacionID"))
                invitacion.InvitacionID = null;
            else
                invitacion.InvitacionID = (Guid)Convert.ChangeType(row["InvitacionID"], typeof(Guid));

            if (row.IsNull("RemitenteID"))
                invitacion.Remitente.UsuarioSocialID = null;
            else
                invitacion.Remitente.UsuarioSocialID = (long)Convert.ChangeType(row["RemitenteID"], typeof(long));
            if (row.IsNull("Estatus"))
                invitacion.ConvertEstatus = null;
            else
                invitacion.ConvertEstatus = (short)Convert.ChangeType(row["Estatus"], typeof(short));
            if (row.IsNull("FechaRegistro"))
                invitacion.FechaRegistro = null;
            else
                invitacion.FechaRegistro = (DateTime)Convert.ChangeType(row["FechaRegistro"], typeof(DateTime));
            if (row.IsNull("Saludo"))
                invitacion.Saludo = null;
            else
                invitacion.Saludo = (string)Convert.ChangeType(row["Saludo"], typeof(string));
            if (row.IsNull("EsSolicitud"))
                invitacion.EsSolicitud = null;
            else
                invitacion.EsSolicitud = (bool)Convert.ChangeType(row["EsSolicitud"], typeof(bool));
            if (row.IsNull("EsSolicitud"))
                invitacion.EsSolicitud = null;
            else
            {
                invitacion.EsSolicitud = (bool)Convert.ChangeType(row["EsSolicitud"], typeof(bool));
                if ((bool)invitacion.EsSolicitud)
                {
                    invitacion.Invitado = new UsuarioSocial();
                    if (row.IsNull("InvitadoID"))
                        ((UsuarioSocial)invitacion.Invitado).UsuarioSocialID = null;
                    else
                        ((UsuarioSocial)invitacion.Invitado).UsuarioSocialID = (long)Convert.ChangeType(row["InvitadoID"], typeof(long));
                }
                else
                {
                    invitacion.Invitado = new ContactoInvitado();
                    if (row.IsNull("InvitadoID"))
                        ((ContactoInvitado)invitacion.Invitado).ContactoInvitadoID = null;
                    else
                        ((ContactoInvitado)invitacion.Invitado).ContactoInvitadoID = (long)Convert.ChangeType(row["InvitadoID"], typeof(long));
                }
            }
            return invitacion;
        }
    }
}
