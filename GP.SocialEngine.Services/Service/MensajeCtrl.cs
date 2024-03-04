using System;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using GP.SocialEngine.BO;
using GP.SocialEngine.Interfaces;
using GP.SocialEngine.DAO;
using GP.SocialEngine.DA;
using POV.CentroEducativo.BO;

namespace GP.SocialEngine.Service
{
    /// <summary>
    /// Controlador del objeto Mensaje
    /// </summary>
    public class MensajeCtrl
    {
        /// <summary>
        /// Consulta registros de Mensaje en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="mensaje">Mensaje que provee el criterio de selección para realizar la consulta</param>
        /// <returns>El DataSet que contiene la información de MensajeRetHlp generada por la consulta</returns>
        public DataSet Retrieve(IDataContext dctx, Mensaje mensaje)
        {
            MensajeRetHlp da = new MensajeRetHlp();
            DataSet ds = da.Action(dctx, mensaje);
            return ds;
        }

        /// <summary>
        /// Consulta los destinatarios del Mensaje en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="mensaje">Mensaje que provee el criterio de selección para realizar la consulta</param>
        /// <param name="destinatario">Filtro de los destinatarios del mensaje valor nulo para consultar todos los destinarios del mensaje</param>
        /// <param name="activo">Estado de los destinatarios del mensaje</param>
        /// <returns>El DataSet que contiene la información de MensajeRetHlp generada por la consulta</returns>
        public DataSet RetrieveDestinatarios(IDataContext dctx, Mensaje mensaje, UsuarioSocial destinatario, bool? activo)
        {
            MensajeRetHlp da = new MensajeRetHlp();
            DataSet ds = da.Action(dctx, mensaje, destinatario, activo);
            return ds;
        }

        /// <summary>
        /// Devuelve un registro completo del Mensaje Padre
        /// <param name="mensaje">Mensaje que provee el criterio de selección para realizar la consulta</param>
        /// </summary>
        public Mensaje RetrieveComplete(IDataContext dctx, Mensaje mensaje)
        {
            if (mensaje == null || mensaje.MensajeID == null)
                throw new SystemException("RetrieveComplete:mensaje no puede ser nulo");

            DataSet ds = Retrieve(dctx, mensaje);

            if (!ds.Tables.Contains("Mensaje"))
                throw new Exception("RetrieveComplete: DataSet no tiene la tabla Mensaje");
            int index = ds.Tables["Mensaje"].Rows.Count;
            if (index != 1)
                throw new Exception("RetrieveComplete: El registro solicitado no se ha encontrado");

            Mensaje mens = LastDataRowToMensaje(ds);

            //Consulta al remitente del mensaje
            UsuarioSocialCtrl usuarioSocialCtrl = new UsuarioSocialCtrl();
            mens.Remitente = usuarioSocialCtrl.RetrieveComplete(dctx, mens.Remitente);

            //Consulta los destinatarios activos para el mensaje
            ds = RetrieveDestinatarios(dctx, mens, null, true);
            if (ds.Tables.Contains("Destinatarios"))
            {
                mens.Destinatarios = new List<UsuarioSocial>();

                foreach (DataRow drdestinario in ds.Tables[0].Rows)
                {
                    DataSet dsdestinatarios = usuarioSocialCtrl.Retrieve(dctx, new UsuarioSocial
                                                                                  {
                                                                                      UsuarioSocialID = Convert.ToInt64(drdestinario["UsuarioSocialID"])
                                                                                  }
                        );
                    if (dsdestinatarios.Tables[0].Rows.Count == 1)
                        mens.Destinatarios.Add(usuarioSocialCtrl.LastDataRowToUsuarioSocial(dsdestinatarios));
                }
            }

            return mens;
        }

        /// <summary>
        /// Inserta una Mensaje y registra las notificaciones correspondiente
        /// </summary>
        public void InsertComplete(IDataContext dctx, Mensaje mensaje, List<AreaConocimiento> areasConocimiento, long universidadID)
        {
            if (mensaje == null)
                throw new SystemException("InsertComplete:mensaje no puede ser nulo");

            if (mensaje.Remitente == null || mensaje.Remitente.UsuarioSocialID == null)
                throw new SystemException("InsertComplete:el remitente no puede ser nulo");

            if ((mensaje.Destinatarios == null || mensaje.Destinatarios.Count <= 0) & mensaje.Remitente == null)
                throw new SystemException("InsertComplete:no existen destinatarios agregados");

            if (mensaje.MensajeID == null)
            {
                mensaje.MensajeID = Guid.NewGuid();
                mensaje.GuidConversacion = mensaje.MensajeID.ToString();
            }

            object firma = new object();

            try
            {
                dctx.OpenConnection(firma);
                dctx.BeginTransaction(firma);

                //validar que el usuario social existe y pertenece a un grupo
                UsuarioSocialCtrl usuarioSocialCtrl = new UsuarioSocialCtrl();
                UsuarioSocial usuarioSocial = usuarioSocialCtrl.RetrieveComplete(dctx, new UsuarioSocial { UsuarioSocialID = mensaje.Remitente.UsuarioSocialID, Estatus = true });

                if (usuarioSocial == null)
                    throw new SystemException("InsertComplete:remitente no encontrado");

                SocialHubCtrl socialHubCtrl = new SocialHubCtrl();
                DataSet dsocialHub = socialHubCtrl.RetrieveSocialHubUsuario(dctx, new SocialHub { SocialProfile = usuarioSocial, SocialProfileType = ESocialProfileType.USUARIOSOCIAL });

                SocialHub socialHub = socialHubCtrl.LastDataRowToSocialHub(dsocialHub);
                GrupoSocialCtrl grupoSocialCtrl = new GrupoSocialCtrl();
                List<GrupoSocial> lsgrupoSocial = grupoSocialCtrl.RetrieveGruposSocialSocialHub(dctx, socialHub);

                if (lsgrupoSocial.Count != 1)
                    throw new SystemException("InsertComplete:error al consultar el GrupoSocial");

                Insert(dctx, mensaje);

                //Inserta el remitente a la lista de asociados al mensaje
                Insert(dctx, mensaje, mensaje.Remitente, true);
                InsertNotificacion(dctx, mensaje, mensaje.Remitente);

                //Inserta los destinatarios asociados al mensaje privado
                if (mensaje.Destinatarios != null)
                    foreach (UsuarioSocial usrdestinatario in mensaje.Destinatarios)
                    {
                        //validar si pertenece al grupo
                        UsuarioGrupo usuarioGrupo = grupoSocialCtrl.RetrieveFriend(dctx, lsgrupoSocial[0], usrdestinatario, areasConocimiento, null, universidadID );
                        if (usuarioGrupo != null)
                        {
                            Insert(dctx, mensaje, usrdestinatario, true);
                            //Inserta las notificaciones.
                            InsertNotificacion(dctx, mensaje, usrdestinatario);
                        }
                    }

                dctx.CommitTransaction(firma);
            }
            catch (Exception ex)
            {
                dctx.RollbackTransaction(firma);
                dctx.CloseConnection(firma);
                throw ex;
            }

            finally
            {
                if (dctx.ConnectionState == ConnectionState.Open)
                    dctx.CloseConnection(firma);
            }
        }

        /// <summary>
        /// Inserta una Mensaje y registra las notificaciones correspondiente
        /// </summary>
        public void InsertRespuestaComplete(IDataContext dctx, Mensaje mensaje, List<AreaConocimiento> areasConocimiento, long universidadID)
        {
            if (mensaje == null)
                throw new SystemException("InsertRespuestaComplete:mensaje no puede ser nulo");
            if (mensaje.Remitente == null || mensaje.Remitente.UsuarioSocialID == null)
                throw new SystemException("InsertRespuesta:el remitente no puede ser nulo");
            if (string.IsNullOrEmpty(mensaje.GuidConversacion))
                throw new SystemException("InsertRespuesta:el guid de referencia del mensaje no puede ser nulo o vacio");
            if (mensaje.MensajeID == null)
                mensaje.MensajeID = Guid.NewGuid();

            Guid GUI;
            if (!Guid.TryParse(mensaje.GuidConversacion, out GUI))
                throw new SystemException("InsertRespuesta:el guid de referencia del mensaje no tiene el formato correcto");

            object firma = new object();

            try
            {
                //validar que los usuarios sociales pertenecen al grupo.
                dctx.OpenConnection(firma);
                dctx.BeginTransaction(firma);

                //validar que el usuario social existe y pertenece a un grupo
                UsuarioSocialCtrl usuarioSocialCtrl = new UsuarioSocialCtrl();
                UsuarioSocial usuarioSocial = usuarioSocialCtrl.RetrieveComplete(dctx, new UsuarioSocial { UsuarioSocialID = mensaje.Remitente.UsuarioSocialID, Estatus = true });

                if (usuarioSocial == null)
                    throw new SystemException("InsertRespuestaComplete:remitente no encontrado");

                SocialHubCtrl socialHubCtrl = new SocialHubCtrl();
                DataSet dsocialHub = socialHubCtrl.RetrieveSocialHubUsuario(dctx, new SocialHub { SocialProfile = usuarioSocial, SocialProfileType = ESocialProfileType.USUARIOSOCIAL });

                SocialHub socialHub = socialHubCtrl.LastDataRowToSocialHub(dsocialHub);
                GrupoSocialCtrl grupoSocialCtrl = new GrupoSocialCtrl();
                List<GrupoSocial> lsgrupoSocial = grupoSocialCtrl.RetrieveGruposSocialSocialHub(dctx, socialHub);

                if (lsgrupoSocial.Count != 1)
                    throw new SystemException("InsertRespuestaComplete:error al consultar el GrupoSocial");

                //validar si pertenece al grupo
                UsuarioGrupo usuarioGrupo = grupoSocialCtrl.RetrieveFriend(dctx, lsgrupoSocial[0], usuarioSocial, areasConocimiento, null, universidadID);
                if (usuarioGrupo != null)
                {
                    Insert(dctx, mensaje);
                    //Consultar destinatarios
                    Mensaje mensajetmp = RetrieveComplete(dctx, new Mensaje { MensajeID = GUI });
                    if (mensajetmp.Destinatarios != null)
                        foreach (UsuarioSocial usrdestinatario in mensajetmp.Destinatarios)
                            InsertNotificacionRespuesta(dctx, mensaje, usrdestinatario);

                    dctx.CommitTransaction(firma);

                }
            }
            catch (Exception ex)
            {
                dctx.RollbackTransaction(firma);
                dctx.CloseConnection(firma);
                throw ex;
            }

            finally
            {
                if (dctx.ConnectionState == ConnectionState.Open)
                    dctx.CloseConnection(firma);
            }
        }

        /// <summary>
        /// Inserta una notificación de mensaje
        /// </summary>
        private void InsertNotificacion(IDataContext dctx, Mensaje mensaje, UsuarioSocial destinatario)
        {
            if (mensaje == null || mensaje.MensajeID == null)
                return;

            if (destinatario == null || destinatario.UsuarioSocialID == null)
                return;

            //validar que el mensaje sea un mensaje padre
            if (mensaje.MensajeID != Guid.Parse(mensaje.GuidConversacion))
                return;

            //validar que el usuario social no sea el remitente
            if (mensaje.Remitente.UsuarioSocialID == destinatario.UsuarioSocialID)
                return;

            NotificacionCtrl notificacionCtrl = new NotificacionCtrl();
            Notificacion notificacion = new Notificacion
                                            {
                                                NotificacionID = Guid.NewGuid(),
                                                Emisor = mensaje.Remitente,
                                                Receptor = destinatario,
                                                FechaRegistro = mensaje.FechaMensaje,
                                                Notificable = mensaje,
                                                TipoNotificacion = ETipoNotificacion.MENSAJE,
                                                EstatusNotificacion = EEstatusNotificacion.NUEVO
                                            };


            //Registrar notificación.
            notificacionCtrl.Insert(dctx, notificacion);
        }

        /// <summary>
        /// Inserta una notificación de una Respuesta de un mensaje
        /// </summary>
        private void InsertNotificacionRespuesta(IDataContext dctx, Mensaje mensaje, UsuarioSocial destinatario)
        {
            if (mensaje == null)
                return;

            if (destinatario == null || destinatario.UsuarioSocialID == null)
                return;

            if (string.IsNullOrEmpty(mensaje.GuidConversacion))
                return;

            //validar que el usuario social no sea el remitente
            if (mensaje.Remitente.UsuarioSocialID == destinatario.UsuarioSocialID)
                return;

            NotificacionCtrl notificacionCtrl = new NotificacionCtrl();
            //validar que no tenga una notificación activa para el mismo mensaje.
            DataSet dsnotificacion = notificacionCtrl.Retrieve(dctx, new Notificacion
                                                                         {
                                                                             Notificable = new Mensaje
                                                                                               {
                                                                                                   MensajeID = Guid.Parse(mensaje.GuidConversacion.ToString())
                                                                                               }
                                                                             ,
                                                                             EstatusNotificacion = EEstatusNotificacion.NUEVO,
                                                                             Receptor = new UsuarioSocial { UsuarioSocialID = destinatario.UsuarioSocialID }
                                                                         });
            if (dsnotificacion.Tables[0].Rows.Count >= 1)
                return;

            Notificacion notificacion = new Notificacion
            {
                NotificacionID = Guid.NewGuid(),
                Emisor = mensaje.Remitente,
                Receptor = destinatario,
                FechaRegistro = mensaje.FechaMensaje,
                Notificable = new Mensaje { MensajeID = Guid.Parse(mensaje.GuidConversacion) },
                TipoNotificacion = ETipoNotificacion.RESPUESTA_MENSAJE,
                EstatusNotificacion = EEstatusNotificacion.NUEVO
            };


            //Registrar notificación.
            notificacionCtrl.Insert(dctx, notificacion);
        }

        /// <summary>
        /// Crea un registro de MensajeInsHlp en la base de datos
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="mensajeInsHlp">MensajeInsHlp que desea crear</param>
        /// 
        /// 
        public void Insert(IDataContext dctx, Mensaje mensaje)
        {
            MensajeInsHlp da = new MensajeInsHlp();
            da.Action(dctx, mensaje);
        }

        public void Insert(IDataContext dctx, Mensaje mensaje, UsuarioSocial destinatario, bool? activo)
        {
            MensajeInsHlp da = new MensajeInsHlp();
            da.Action(dctx, mensaje, destinatario, activo);
        }
        /// <summary>
        /// Actualiza el estado de un registro de MensajeUsuariosSociales en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="mensaje">Objeto con los datos de identificación del mensaje</param>
        /// <param name="destinatario">UsuarioSocial asociado al mensaje</param>
        /// <para name="activo">Estado del la asociación del destinatario al mensaje</para>
        public void Update(IDataContext dctx, Mensaje mensaje, UsuarioSocial destinatario, bool? activo)
        {
            MensajeUpdHlp da = new MensajeUpdHlp();
            da.Action(dctx, mensaje, destinatario, activo);
        }

        /// <summary>
        /// Actualiza de manera optimista un registro de Mensaje en la base de datos.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="mensaje">Mensaje que tiene los datos nuevos</param>
        /// <param name="anterior">Mensaje que tiene los datos anteriores</param>
        public void Update(IDataContext dctx, Mensaje mensaje, Mensaje previous)
        {
            MensajeUpdHlp da = new MensajeUpdHlp();
            da.Action(dctx, mensaje, previous);
        }

        /// <summary>
        /// Eliminar la asociación del los usuarios con el mensaje 
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="mensaje">El mensaje que tiene los datos nuevos</param>
        public void RemoveAsociadosMensaje(IDataContext dctx, Mensaje mensaje, Mensaje anterior)
        {

            if (mensaje == null)
                throw new SystemException("UpdateAsociadosMensaje:mensaje no puede ser nulo");

            if (anterior == null)
                throw new SystemException("UpdateAsociadosMensaje:mensaje anterior no puede ser nulo");

            if (mensaje.Destinatarios == null || mensaje.Destinatarios.Count <= 0)
                throw new SystemException("UpdateAsociadosMensaje:no existen destinatarios agregados para eliminar");

            object firma = new object();

            try
            {

                dctx.OpenConnection(firma);
                dctx.BeginTransaction(firma);

                //Consultar los usuarios sociales.
                UsuarioSocialCtrl usuarioSocialCtrl = new UsuarioSocialCtrl();
                Mensaje mensjtemp = new Mensaje { MensajeID = mensaje.MensajeID };
                mensjtemp.Destinatarios = new List<UsuarioSocial>();
                foreach (UsuarioSocial usrdestinatio in mensaje.Destinatarios)
                {
                    mensjtemp.Destinatarios.Add(usuarioSocialCtrl.RetrieveComplete(dctx, usrdestinatio));
                }

                //Validar que los usuarios sociales pertenecen al grupo.

                //Consultar el mensaje original y sus asociados
                Mensaje msanterior = RetrieveComplete(dctx, anterior);
                if (msanterior.Destinatarios != null)
                    foreach (UsuarioSocial usuarioSocial in msanterior.Destinatarios)
                    {
                        bool exite = mensjtemp.Destinatarios.Exists((delegate(UsuarioSocial usuario)
                                                                         {
                                                                             return usuario.UsuarioSocialID == usuarioSocial.UsuarioSocialID;
                                                                         }
                                                                      )
                                                                    );
                        if (exite)
                            Update(dctx, mensjtemp, usuarioSocial, false);
                    }
                dctx.CommitTransaction(firma);
            }
            catch (Exception ex)
            {
                dctx.RollbackTransaction(firma);
                dctx.CloseConnection(firma);
                throw ex;
            }

            finally
            {
                if (dctx.ConnectionState == ConnectionState.Open)
                    dctx.CloseConnection(firma);
            }




        }

        /// <summary>
        /// Crea un objeto de Mensaje a partir de los datos del último DataRow del DataSet.
        /// </summary>
        /// <param name="ds">El DataSet que contiene la información de Mensaje</param>
        /// <returns>Un objeto de Mensaje creado a partir de los datos</returns>
        public Mensaje LastDataRowToMensaje(DataSet ds)
        {
            if (!ds.Tables.Contains("Mensaje"))
                throw new Exception("LastDataRowToMensaje: DataSet no tiene la tabla Mensaje");
            int index = ds.Tables["Mensaje"].Rows.Count;
            if (index < 1)
                throw new Exception("LastDataRowToMensaje: El DataSet no tiene filas");
            return this.DataRowToMensaje(ds.Tables["Mensaje"].Rows[index - 1]);
        }
        /// <summary>
        /// Crea un objeto de Mensaje a partir de los datos de un DataRow.
        /// </summary>
        /// <param name="row">El DataRow que contiene la información de Mensaje</param>
        /// <returns>Un objeto de Mensaje creado a partir de los datos</returns>
        public Mensaje DataRowToMensaje(DataRow row)
        {
            Mensaje mensaje = new Mensaje();

            mensaje.Remitente = new UsuarioSocial();
            mensaje.Destinatarios = new List<UsuarioSocial>();
            if (row.IsNull("MensajeID"))
                mensaje.MensajeID = null;
            else
                mensaje.MensajeID = (Guid)Convert.ChangeType(row["MensajeID"], typeof(Guid));
            if (row.IsNull("GuidConversacion"))
                mensaje.GuidConversacion = null;
            else
                mensaje.GuidConversacion = (string)Convert.ChangeType(row["GuidConversacion"], typeof(string));
            if (row.IsNull("Contenido"))
                mensaje.Contenido = null;
            else
                mensaje.Contenido = (string)Convert.ChangeType(row["Contenido"], typeof(string));
            if (row.IsNull("FechaMensaje"))
                mensaje.FechaMensaje = null;
            else
                mensaje.FechaMensaje = (DateTime)Convert.ChangeType(row["FechaMensaje"], typeof(DateTime));
            if (row.IsNull("Estatus"))
                mensaje.Estatus = null;
            else
                mensaje.Estatus = (bool)Convert.ChangeType(row["Estatus"], typeof(bool));
            if (row.IsNull("Asunto"))
                mensaje.Asunto = null;
            else
                mensaje.Asunto = (string)Convert.ChangeType(row["Asunto"], typeof(string));

            if (row.IsNull("RemitenteID"))
                mensaje.Remitente = null;
            else
                mensaje.Remitente = new UsuarioSocial { UsuarioSocialID = (long)Convert.ChangeType(row["RemitenteID"], typeof(long)) };

            return mensaje;
        }

        /// <summary>
        /// Devuelve los mensajes en los que se encuentre asociados el usuario.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="esPadre">Indica si se consultaran los mensajes padres o los hijos o nulo para ambos</param>
        /// <param name="usuarioSocialId">Usuario asociado a los mensajes</param>
        /// <param name="parameters">Filtros a aplicar a los mensajes</param>
        /// <returns>El DataSet que contiene la información de Mensaje generada por la consulta</returns>
        public DataSet RetriveMensajesUsuario(IDataContext dctx, bool? esPadre, long? usuarioSocialId, Dictionary<string, string> parameters)
        {
            MensajeDARetHlp da = new MensajeDARetHlp();
            DataSet ds = da.Action(dctx, esPadre, usuarioSocialId, parameters);

            return ds;
        }

        /// <summary>
        /// Devuelve los mensajes en los que se encuentre asociados el usuario paginados.
        /// </summary>
        /// <param name="dctx">El DataContext que proveerá acceso a la base de datos</param>
        /// <param name="esPadre">Indica si se consultaran los mensajes padres o los hijos o nulo para ambos</param>
        /// <param name="usuarioSocialId">Usuario que participa en los mensajes</param>
        /// <param name="pageSize">Tamaño de la pagina</param>
        /// <param name="currentPage">Pagina Actual</param>
        /// <param name="sortColumn">Tamaño de la pagina</param>
        /// <param name="sortOrder">Ordenamiento</param>
        /// <param name="parameters">Filtros</param>
        /// <returns>El DataSet que contiene la información de Mensaje generada por la consulta</returns>
        public DataSet RetriveMensajesUsuarioPaginados(IDataContext dctx, bool? esPadre, long? usuarioSocialId, int pageSize, int currentPage, string sortColumn, string sortOrder, Dictionary<string, string> parameters)
        {
            MensajeDARetHlp da = new MensajeDARetHlp();
            DataSet ds = da.Action(dctx, esPadre, usuarioSocialId, pageSize, currentPage, sortColumn, sortOrder, parameters);
            return ds;
        }






    }
}
