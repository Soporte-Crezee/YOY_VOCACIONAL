using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Framework.Base.DataAccess;
using GP.SocialEngine.BO;
using GP.SocialEngine.Service;
using POV.Core.RedSocial.Implement;
using POV.Core.RedSocial.Interfaces;
using POV.Licencias.Service;
using POV.Seguridad.Service;
using POV.Seguridad.BO;
using System.Web;
using System.IO;
using POV.Comun.Service;

namespace POV.Web.DTO.Services
{
    public class MensajeDTOCtrl
    {
        private IDataContext dctx;
        private IUserSession userSession;
        private MensajeCtrl mensajeCtrl;
        private Mensaje mensaje;

        public MensajeDTOCtrl()
        {

            dctx = new DataContext((new DataProviderFactory()).GetDataProvider("POV"));
            userSession = new UserSession();
        }

        public List<mensajedto> GetLastMensajes(mensajedtoinput dto)
        {
            try
            {
                //CONSTANTES DE LA CONSULTA
                const string sort = "FechaMensaje";
                const string order = "desc";
                const int pageSize = 10;
                const int currentpage = 1;
                bool? espadre = true;

                if (dto == null) throw new Exception("GetLastMensajes: el objeto dto no puede ser nulo");
                if (userSession.CurrentUsuarioSocial == null)
                    throw new Exception("GetLastMensajes: el usuario social no puede ser nulo");

                mensajeCtrl = new MensajeCtrl();
                List<mensajedto> lsdto = new List<mensajedto>();
                List<mensajedto> ls = new List<mensajedto>();
                Dictionary<string, string> pDictionary = DtoInputToParameters(dto);

                DataSet ds = mensajeCtrl.RetriveMensajesUsuarioPaginados(dctx, espadre,
                                                                         userSession.CurrentUsuarioSocial.UsuarioSocialID,
                                                                         pageSize, currentpage, sort, order, pDictionary);

                foreach (DataRow dataRowMensaje in ds.Tables["Mensaje"].Rows)
                {
                    mensajedto dtoms = DataRowToMensajeCompuesto(dataRowMensaje);
                    lsdto.Add(dtoms);
                }
                foreach (mensajedto msdto in lsdto)
                {
                    msdto.respuestas = new List<mensajedto>();
                    msdto.destinatarios = new List<contactodto>();
                    mensajedto tmp = new mensajedto();
                    DataSet data = mensajeCtrl.Retrieve(dctx, new Mensaje { GuidConversacion = msdto.mensajeid });
                    foreach (DataRow msrow in data.Tables[0].Rows)
                    {
                        Mensaje ms = mensajeCtrl.RetrieveComplete(dctx,
                                                                  new Mensaje
                                                                      {
                                                                          MensajeID =
                                                                              (Guid)
                                                                              Convert.ChangeType(msrow["MensajeID"],
                                                                                                 typeof(Guid))
                                                                      });
                        tmp = MensajeToDto(ms);
                        if (ms.MensajeID.ToString() != ms.GuidConversacion)
                            msdto.respuestas.Add(tmp);
                        else
                            msdto.destinatarios = tmp.destinatarios != null ? tmp.destinatarios : null;
                    }
                    //total de respuestas encontradas:
                    if (msdto.respuestas != null)
                        msdto.totalrespuestas = msdto.respuestas.Count();

                    ls.Add(msdto);
                }
                return ls;
            }
            catch (Exception ex)
            {
                POV.Logger.Service.LoggerHlp.Default.Error(this, ex);
                throw ex;
            }
        }

        public List<mensajedto> GetMensajesPaginados(mensajedtoinput dto)
        {
            try
            {
                //CONSTANTES DE LA CONSULTA
                const string sort = "FechaMensaje";
                const string order = "desc";
                const int pageSize = 10;
                bool? espadre = true;
                int recordCount;

                if (dto == null) throw new Exception("GetLastMensajes: el objeto dto no puede ser nulo");

                if (userSession.CurrentUsuarioSocial == null)
                    throw new Exception("GetMensajes: el usuario social no puede ser nulo");

                mensajeCtrl = new MensajeCtrl();
                List<mensajedto> lsdto = new List<mensajedto>();
                List<mensajedto> ls = new List<mensajedto>();
                Dictionary<string, string> pDictionary = DtoInputToParameters(dto);

                DataSet ds = mensajeCtrl.RetriveMensajesUsuarioPaginados(dctx, espadre,
                                                                         userSession.CurrentUsuarioSocial.UsuarioSocialID,
                                                                         pageSize, dto.currentpage, sort, order, pDictionary);

                foreach (DataRow dataRowMensaje in ds.Tables["Mensaje"].Rows)
                {
                    mensajedto dtoms = DataRowToMensajeCompuesto(dataRowMensaje);
                    lsdto.Add(dtoms);
                }

                foreach (mensajedto msdto in lsdto)
                {
                    //no se consultan los datos de control
                   // msdto.recordcount = recordCount;
                    msdto.respuestas = new List<mensajedto>();
                    msdto.destinatarios = new List<contactodto>();
                    mensajedto tmp = new mensajedto();
                    DataSet data = mensajeCtrl.Retrieve(dctx, new Mensaje { GuidConversacion = msdto.mensajeid });
                    foreach (DataRow msrow in data.Tables[0].Rows)
                    {
                        Mensaje ms = mensajeCtrl.RetrieveComplete(dctx,
                                                                  new Mensaje
                                                                      {
                                                                          MensajeID =
                                                                              (Guid)
                                                                              Convert.ChangeType(msrow["MensajeID"],
                                                                                                 typeof(Guid))
                                                                      });
                        tmp = MensajeToDto(ms);
                        if (ms.MensajeID.ToString() != ms.GuidConversacion)
                            msdto.respuestas.Add(tmp);
                        else
                            msdto.destinatarios = tmp.destinatarios != null ? tmp.destinatarios : null;
                    }
                    //total de respuestas encontradas:
                    if (msdto.respuestas != null)
                        msdto.totalrespuestas = msdto.respuestas.Count();

                    ls.Add(msdto);
                }
                return ls;
            }
            catch (Exception ex)
            {
                POV.Logger.Service.LoggerHlp.Default.Error(this, ex);
                throw ex;
            }
        }

        public List<mensajedto> GetMensajes(mensajedtoinput dto)
        {
            try
            {
                //
                if (dto == null) throw new Exception("GetLastMensajes: el objeto dto no puede ser nulo");

                if (userSession.CurrentUsuarioSocial == null)
                    throw new Exception("GetMensajes: el usuario social no puede ser nulo");

                mensajeCtrl = new MensajeCtrl();
                List<mensajedto> lsdto = new List<mensajedto>();
                Dictionary<string, string> pDictionary = DtoInputToParameters(dto);

                DataSet ds = mensajeCtrl.RetriveMensajesUsuario(dctx, null, userSession.CurrentUsuarioSocial.UsuarioSocialID,
                                                                pDictionary);

                foreach (DataRow dataRowMensaje in ds.Tables["Mensaje"].Rows)
                {
                    mensajedto dtoms = DataRowToMensajeCompuesto(dataRowMensaje);
                    lsdto.Add(dtoms);
                }
                return lsdto;
            }
            catch (Exception ex)
            {
                POV.Logger.Service.LoggerHlp.Default.Error(this, ex);
                throw ex;
            }
        }

        public mensajedto GetMensaje(mensajedtoinput dto)
        {
            try
            {
                mensajeCtrl = new MensajeCtrl();
                mensaje = new Mensaje();
                mensajedto dtomensaje = new mensajedto();
                DataSet ds = mensajeCtrl.Retrieve(dctx, new Mensaje { MensajeID = Guid.Parse(dto.mensajeid) });

                foreach (DataRow dataRowMsj in ds.Tables[0].Rows)
                {
                    mensaje = mensajeCtrl.DataRowToMensaje(dataRowMsj);
                    dtomensaje = MensajeToDto(mensaje);
                }

                return dtomensaje;
            }
            catch (Exception ex)
            {
                POV.Logger.Service.LoggerHlp.Default.Error(this, ex);
                throw ex;
            }
        }

        public List<contactodto> GetAsociadosMensaje(mensajedto dto)
        {
            try
            {
                mensajeCtrl = new MensajeCtrl();
                List<contactodto> ls = new List<contactodto>();
                DataSet ds = mensajeCtrl.RetrieveDestinatarios(dctx, DtoToMensaje(dto), userSession.CurrentUsuarioSocial,
                                                               true);

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    contactodto contactodto = new contactodto
                                                  {
                                                      usuariosocialid =
                                                          (long)Convert.ChangeType(row["USUARIOSOCIALID"], typeof(long))
                                                  };
                    ls.Add(contactodto);
                }

                return ls;
            }
            catch (Exception ex)
            {
                POV.Logger.Service.LoggerHlp.Default.Error(this, ex);
                throw ex;
            }
        }

        /// <summary>
        /// Inserta un mensaje privado.
        /// </summary>
        /// <param name="dto">objeto de transferencia de datos</param>
        /// <returns></returns>
        public mensajedto SaveMensaje(mensajedto dto)
        {
            try
            {
                mensajeCtrl = new MensajeCtrl();
                mensaje = new Mensaje();
                mensaje = DtoToMensaje(dto);

                //valores requeridos para insertar un mensaje.
                mensaje.MensajeID = Guid.NewGuid();
                mensaje.GuidConversacion = mensaje.MensajeID.ToString();
                mensaje.FechaMensaje = DateTime.Now;
                mensaje.Remitente = userSession.CurrentUsuarioSocial;
                mensaje.Estatus = true;


                mensajeCtrl.InsertComplete(dctx, mensaje, userSession.CurrentAlumno.AreasConocimiento, (long)userSession.CurrentAlumno.Universidades[0].UniversidadID);

                #region Envio de las notificaciones
                //Se obtienen los destinatarios.
                LicenciaEscuelaCtrl licenciaEscuelaCtrl = new LicenciaEscuelaCtrl();
                UsuarioCtrl usuarioCtrl = new UsuarioCtrl();
                CorreoCtrl correoCtrl = new CorreoCtrl();
                var correos = new List<Usuario>();
                foreach (var item in mensaje.Destinatarios)
                {
                    if (item.UsuarioSocialID != 0)
                    {
                        var usuarioDestino = licenciaEscuelaCtrl.RetrieveUsuario(dctx, new UsuarioSocial { UsuarioSocialID = item.UsuarioSocialID }, false);
                        Usuario usuario = usuarioCtrl.LastDataRowToUsuario(usuarioCtrl.Retrieve(dctx, new Usuario() { UsuarioID = usuarioDestino.UsuarioID }));
                        if (!String.IsNullOrEmpty(usuario.Email))
                        {
                            correos.Add(usuario);
                        }
                    }
                }

                #region variables
                string urllogo = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgLogo"];
                string urlimgbackground = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgBackground"];
                string urlimgface = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgFace"];
                string urlimgtwit = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgTwit"];
                string urlimglinked = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgLinked"];
                string urlimginstagram = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgInstagram"];
                const string altimg = "YOY - Email";
                const string titulo = "Mensaje Privado";
                string dominio = String.Format("{0}://{1}:{2}/", HttpContext.Current.Request.Url.Scheme, HttpContext.Current.Request.Url.Host, HttpContext.Current.Request.Url.Port);
                string linkportal = System.Configuration.ConfigurationManager.AppSettings["POVUrlAspirante"];
                #endregion variables

                #region Template HTML
                foreach (var destinatario in correos)
                {
                    string cuerpo = string.Empty;
                    var serverPath = System.Web.Hosting.HostingEnvironment.MapPath(System.Configuration.ConfigurationManager.AppSettings["POVUrlEmailNotificacionMP"]);
                    using (StreamReader reader = new StreamReader(serverPath))
                    {
                        cuerpo = reader.ReadToEnd();
                    }

                    cuerpo = cuerpo.Replace("{title}", titulo);
                    cuerpo = cuerpo.Replace("{altimg}", altimg);
                    cuerpo = cuerpo.Replace("{urlimgbackground}", urlimgbackground);
                    cuerpo = cuerpo.Replace("{urllogo}", urllogo);
                    cuerpo = cuerpo.Replace("{linkportal}", linkportal);
                    cuerpo = cuerpo.Replace("{mensaje}", "Has recibido un nuevo mensaje privado de {remitente}");
                    cuerpo = cuerpo.Replace("{remitente}", mensaje.Remitente.ScreenName);
                    cuerpo = cuerpo.Replace("{urlimgface}", urlimgface);
                    cuerpo = cuerpo.Replace("{urlimgtwit}", urlimgtwit);
                    cuerpo = cuerpo.Replace("{urlimglinked}", urlimglinked);
                    cuerpo = cuerpo.Replace("{urlimginstagram}", urlimginstagram);
                #endregion Template HTML

                    #region Envio de correo

                    var usuarioSocial = licenciaEscuelaCtrl.RetrieveUserSocial(dctx, new Usuario { UsuarioID = destinatario.UsuarioID });
                    cuerpo = cuerpo.Replace("{destinatario}", usuarioSocial.ScreenName);
                    List<string> tos = new List<string>();
                    tos.Add(destinatario.Email);
                    try
                    {
                        correoCtrl.sendMessage(tos, "YOY - Mensaje Privado", cuerpo, null, new List<string>(), new List<string>());
                    }
                    catch { }
                }
                #endregion Envio de correo
                #endregion Envio de las notificaciones
                //Consultar el mensaje insertado
                DataSet dsmensajes = mensajeCtrl.Retrieve(dctx, mensaje);
                if (dsmensajes.Tables[0].Rows.Count == 1)
                {
                    mensajedto dtomensaje = MensajeToDto(mensajeCtrl.LastDataRowToMensaje(dsmensajes));
                    return dtomensaje;
                }
                return null;
            }
            catch (Exception ex)
            {
                POV.Logger.Service.LoggerHlp.Default.Error(this, ex);
                throw ex;
            }
        }

        /// <summary>
        /// Inserta una Respuesta del mensaje privado.
        /// </summary>
        /// <param name="dto">objeto de transferencia de datos</param>
        /// <returns></returns>
        public mensajedto SaveRespuestaMensaje(mensajedto dto)
        {
            try
            {
                if (dto == null)
                    throw new Exception("SaveRespuestaMensaje:el mensajedto no puede ser nulo");

                if (string.IsNullOrEmpty(dto.mensajeid))
                    throw new Exception("SaveRespuestaMensaje:no se ha proporcionado el identificador del mensaje");

                Guid guid;
                mensajeCtrl = new MensajeCtrl();
                bool esmensajeid = Guid.TryParse(dto.mensajeid, out guid);
                if (!esmensajeid)
                    throw new Exception("SaveRespuestaMensaje:no se ha proporcionado el identificador del mensaje valido");
                DataSet ds = mensajeCtrl.Retrieve(dctx, new Mensaje { MensajeID = guid, Estatus = true });
                if (ds.Tables[0].Rows.Count != 1)
                    throw new Exception("SaveRespuestaMensaje:no se ha proporcionado el identificador del mensaje valido");

                Mensaje mstmp = mensajeCtrl.LastDataRowToMensaje(ds);
                mensaje = DtoToMensaje(dto);

                //valores requeridos para insertar un mensaje.
                mensaje.MensajeID = Guid.NewGuid();
                mensaje.FechaMensaje = DateTime.Now;
                mensaje.GuidConversacion = mstmp.GuidConversacion;
                mensaje.Remitente = userSession.CurrentUsuarioSocial;
                mensaje.Estatus = true;
                mensajeCtrl.InsertRespuestaComplete(dctx, mensaje, userSession.CurrentAlumno.AreasConocimiento, (long)userSession.CurrentAlumno.Universidades[0].UniversidadID);
                //Consultar el mensaje insertado
                DataSet dsmensajes = mensajeCtrl.Retrieve(dctx, mensaje);
                if (dsmensajes.Tables[0].Rows.Count == 1)
                {
                    Mensaje ms = mensajeCtrl.LastDataRowToMensaje(dsmensajes);
                    ms.Remitente = userSession.CurrentUsuarioSocial;
                    mensajedto dtomensaje = MensajeToDto(ms);
                    return dtomensaje;
                }
                return null;
            }
            catch (Exception ex)
            {
                POV.Logger.Service.LoggerHlp.Default.Error(this, ex);
                throw ex;
            }
        }

        /// <summary>
        /// Elimina al usuario social del la lista de asociados al mensaje.
        /// </summary>
        /// <param name="dto">objeto de transferencia de datos con la informacion del mensaje</param>
        /// <returns>mensajedto que se elimino para el usuario social</returns>
        public mensajedto RemoveAsociadoMensaje(mensajedto dto)
        {
            try
            {
                //elimina al usuario usuariosocialid
                if (dto == null)
                    throw new Exception("RemoveMensaje:El mensajedto no puede ser nulo");


                mensajeCtrl = new MensajeCtrl();
                mensaje = new Mensaje();

                mensaje = DtoToMensaje(dto);
                mensaje.Destinatarios = new List<UsuarioSocial>();
                mensaje.Destinatarios.Add(userSession.CurrentUsuarioSocial);
                mensajeCtrl = new MensajeCtrl();
                Mensaje anterior = mensajeCtrl.RetrieveComplete(dctx, mensaje);
                mensajeCtrl.RemoveAsociadosMensaje(dctx, mensaje, anterior);

                mensaje = mensajeCtrl.RetrieveComplete(dctx, mensaje);
                return MensajeToDto(mensaje);
            }
            catch (Exception ex)
            {
                POV.Logger.Service.LoggerHlp.Default.Error(this, ex);
                throw ex;
            }
        }

        /// <summary>
        /// Convierte los parámetros de un objeto mensajedtoinput a un Dictionary
        /// </summary>
        /// <param name="dto">objeto mensajedtoinput</param>
        /// <returns>Dictionary con los parámetros </returns>
        private Dictionary<string, string> DtoInputToParameters(mensajedtoinput dto)
        {

            if (dto == null)
                throw new Exception("El mensaje no puede ser nulo");

            Dictionary<string, string> parameters = new Dictionary<string, string>();

            if (dto.remitenteid != null && dto.remitenteid > 0)
                parameters.Add("RemitenteID", dto.remitenteid.ToString());

            if (dto.estatus != null && dto.estatus <= 1)
                parameters.Add("Estatus", dto.estatus.ToString());

            if (!string.IsNullOrEmpty(dto.mensajeid))
                parameters.Add("MensajeID", dto.mensajeid);

            int rcount = 0;
            if (dto.recordcount != null && int.TryParse(dto.recordcount.ToString(), out rcount) && rcount > 0)
                parameters.Add("RecordCount", dto.recordcount.ToString());

            return parameters;

        }

        /// <summary>
        /// Convierte un objeto mensajedto a un objeto Mensaje
        /// </summary>
        /// <param name="dto">objeto mensajedto</param>
        /// <returns>Mensaje</returns>
        private Mensaje DtoToMensaje(mensajedto dto)
        {
            Mensaje toMensaje = new Mensaje();
            if (dto == null)
                throw new Exception("El mensaje no puede ser nulo");

            if (!string.IsNullOrEmpty(dto.mensajeid))
                toMensaje.MensajeID = Guid.Parse(dto.mensajeid);

            if (!string.IsNullOrEmpty(dto.guidconversacion))
                toMensaje.GuidConversacion = dto.guidconversacion;

            if (dto.remitenteid != null)
                toMensaje.Remitente = new UsuarioSocial { UsuarioSocialID = Convert.ToInt64(dto.remitenteid) };

            if (dto.fechamensaje != null)
                toMensaje.FechaMensaje = Convert.ToDateTime(dto.fechamensaje);

            if (!string.IsNullOrEmpty(dto.asunto))
                toMensaje.Asunto = dto.asunto;

            if (!string.IsNullOrEmpty(dto.contenido))
                toMensaje.Contenido = dto.contenido;

            if (!string.IsNullOrEmpty(dto.destinatariosstring))
            {
                try
                {
                    string[] sdestinatarios = dto.destinatariosstring.Split('-');
                    dto.destinatarios = new List<contactodto>();
                    foreach (string id in sdestinatarios)
                        dto.destinatarios.Add(new contactodto { usuariosocialid = Convert.ToInt64(id) });
                }
                catch
                {

                }
            }
            if (dto.destinatarios != null && dto.destinatarios.Count > 0)
            {
                toMensaje.Destinatarios = new List<UsuarioSocial>();
                foreach (contactodto contactodto in dto.destinatarios)
                    toMensaje.Destinatarios.Add(new UsuarioSocial { UsuarioSocialID = contactodto.usuariosocialid });
            }

            return toMensaje;

        }

        /// <summary>
        /// Convierte un objeto Mensaje a un objeto mensajedto
        /// </summary>
        /// <param name="objmensaje">mensaje</param>
        /// <returns>mensajedto</returns>
        private mensajedto MensajeToDto(Mensaje objmensaje)
        {
            mensajedto mensajedto = new mensajedto();

            if (objmensaje.MensajeID != null)
                mensajedto.mensajeid = objmensaje.MensajeID.ToString();

            if (objmensaje.GuidConversacion != null)
                mensajedto.guidconversacion = objmensaje.GuidConversacion;

            if (objmensaje.Remitente != null && objmensaje.Remitente.UsuarioSocialID != null)
            {
                mensajedto.remitenteid = objmensaje.Remitente.UsuarioSocialID;
                mensajedto.remitenteurlperfil = "../Files/ImagenUsuario/ImagenPerfil.aspx?img=thumb&usr=" +
                                                objmensaje.Remitente.UsuarioSocialID;
            }

            if (objmensaje.Remitente != null && objmensaje.Remitente.ScreenName != null)
                mensajedto.remitentenombre = objmensaje.Remitente.ScreenName;

            if (objmensaje.Asunto != null)
                mensajedto.asunto = objmensaje.Asunto;

            if (objmensaje.Contenido != null)
                mensajedto.contenido = objmensaje.Contenido;

            if (objmensaje.Estatus != null)
                mensajedto.estatus = Convert.ToInt32(objmensaje.Estatus);

            if (objmensaje.FechaMensaje != null)
            {
                mensajedto.fechamensaje = objmensaje.FechaMensaje.ToString();
                mensajedto.fechaformated = string.Format("{0:f}", objmensaje.FechaMensaje);
            }

            mensajedto.destinatarios = new List<contactodto>();


            if (objmensaje.Destinatarios != null && objmensaje.Destinatarios.Count > 0)
                foreach (UsuarioSocial destinatario in objmensaje.Destinatarios)
                {
                    if (destinatario == null)
                        continue;

                    contactodto usurdto = new contactodto
                                              {
                                                  usuariosocialid = destinatario.UsuarioSocialID,
                                                  screenname = destinatario.ScreenName
                                              };

                    mensajedto.destinatarios.Add(usurdto);
                }

            return mensajedto;
        }

        /// <summary>
        /// Crea un objeto de Mensaje a partir de los datos de un DataRow.
        /// </summary>
        /// <param name="row">El DataRow que contiene la información de Mensaje</param>
        /// <returns>Un objeto de Mensaje creado a partir de los datos</returns>
        private mensajedto DataRowToMensajeCompuesto(DataRow row)
        {
            Mensaje objmensaje = new Mensaje();

            objmensaje.Remitente = new UsuarioSocial();
            objmensaje.Destinatarios = new List<UsuarioSocial>();
            if (row.IsNull("MensajeID"))
                objmensaje.MensajeID = null;
            else
                objmensaje.MensajeID = (Guid)Convert.ChangeType(row["MensajeID"], typeof(Guid));
            if (row.IsNull("GuidConversacion"))
                objmensaje.GuidConversacion = null;
            else
                objmensaje.GuidConversacion = (string)Convert.ChangeType(row["GuidConversacion"], typeof(string));
            if (row.IsNull("Contenido"))
                objmensaje.Contenido = null;
            else
                objmensaje.Contenido = (string)Convert.ChangeType(row["Contenido"], typeof(string));
            if (row.IsNull("FechaMensaje"))
                objmensaje.FechaMensaje = null;
            else
                objmensaje.FechaMensaje = (DateTime)Convert.ChangeType(row["FechaMensaje"], typeof(DateTime));
            if (row.IsNull("Estatus"))
                objmensaje.Estatus = null;
            else
                objmensaje.Estatus = (bool)Convert.ChangeType(row["Estatus"], typeof(bool));
            if (row.IsNull("Asunto"))
                objmensaje.Asunto = null;
            else
                objmensaje.Asunto = (string)Convert.ChangeType(row["Asunto"], typeof(string));



            if (row.IsNull("RemitenteID"))
                objmensaje.Remitente.UsuarioSocialID = null;
            else
                objmensaje.Remitente.UsuarioSocialID = (long)Convert.ChangeType(row["RemitenteID"], typeof(long));


            if (row.IsNull("RemitenteScreenName"))
                objmensaje.Remitente.ScreenName = null;
            else
                objmensaje.Remitente.ScreenName =
                    (string)Convert.ChangeType(row["RemitenteScreenName"], typeof(string));

            mensajedto mensajedto = MensajeToDto(objmensaje);
            return mensajedto;
        }
    }
}

