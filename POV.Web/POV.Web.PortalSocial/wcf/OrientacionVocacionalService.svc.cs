using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using POV.Web.DTO;
using POV.Web.DTO.Services;
using POV.Comun.Service;
using System.Web;
using System.Configuration;
using System.IO;
using System.Net.Mail;
using POV.Logger.Service;
using POV.Seguridad.BO;
using POV.Seguridad.Service;
using Framework.Base.DataAccess;
using POV.Web.PortalSocial.AppCode;
using POV.Licencias.Service;
using POV.CentroEducativo.BO;
using POV.CentroEducativo.Service;
using POV.CentroEducativo.Services;

namespace POV.Web.PortalSocial.wcf
{
    [ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class OrientacionVocacionalService
    {
        private readonly IDataContext dctx = ConnectionHelper.Default.Connection;

        [OperationContract]
        public configcalendardto GuardarConfiguracionAgenda(configcalendardto dto)
        {
            OrientacionVocacionalDTOCtrl orientacionVocacionalDTOCtrl = new OrientacionVocacionalDTOCtrl();
            var result = orientacionVocacionalDTOCtrl.GuardarConfiguracionAgenda(dto);

            return result;
        }

        [OperationContract]
        public configcalendardto GetConfiguracionAgenda(configcalendardto dto)
        {
            OrientacionVocacionalDTOCtrl orientacionVocacionalDTOCtrl = new OrientacionVocacionalDTOCtrl();
            var result = orientacionVocacionalDTOCtrl.GetConfiguracionAgenda(dto);

            return result;
        }

        [OperationContract]
        public configcalendardto GetConfiguracionAgendaOrientador(configcalendardto dto) 
        {
            OrientacionVocacionalDTOCtrl orientacionVocacionalDTOCtrl = new OrientacionVocacionalDTOCtrl();
            var result = orientacionVocacionalDTOCtrl.GetConfiguracionAgendaOrientador(dto);

            return result;
        }

        [OperationContract]
        public eventcalendardto GuardarEvento(eventcalendardto dto) 
        {
            OrientacionVocacionalDTOCtrl orientacionVocacionalDTOCtrl = new OrientacionVocacionalDTOCtrl();
            var result = orientacionVocacionalDTOCtrl.GuardarEvento(dto);

            return result;
        }

        [OperationContract]
        public eventcalendardto GuardarEventoAlumno(eventcalendardto dto)
        {
            OrientacionVocacionalDTOCtrl orientacionVocacionalDTOCtrl = new OrientacionVocacionalDTOCtrl();
            var result = orientacionVocacionalDTOCtrl.GuardarEventoAlumno(dto);

            return result;
        }

        [OperationContract]
        public eventcalendardto EnviarCorreoSolicitud(eventcalendardto dto)
        {
            UsuarioCtrl userCtrl = new UsuarioCtrl();
            LicenciaEscuelaCtrl licenciaEscuelaCtrl = new LicenciaEscuelaCtrl();
            Usuario usuario = userCtrl.LastDataRowToUsuario(userCtrl.Retrieve(dctx, new Usuario() { UsuarioID = dto.usuarioid }));
            Docente orientador = licenciaEscuelaCtrl.RetrieveUsuarioOrientador(dctx, usuario);

                CorreoCtrl correoCtrl = new CorreoCtrl();
                #region Variables
                string urllogo = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgLogo"];
                string urlimgbackground = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgBackground"];
                string urlimgface = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgFace"];
                string urlimgtwit = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgTwit"];
                string urlimglinked = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgLinked"];
                string urlimginstagram = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgInstagram"];
                const string altimg = "YOY - Email";
                const string titulo = "Solicitud";
                string dominio = String.Format("{0}://{1}:{2}/", HttpContext.Current.Request.Url.Scheme, HttpContext.Current.Request.Url.Host, HttpContext.Current.Request.Url.Port);
                string linkportal = ConfigurationManager.AppSettings["POVUrlOrientador"];
                #endregion

                string cuerpo = string.Empty;
                var serverPath = System.Web.Hosting.HostingEnvironment.MapPath(ConfigurationManager.AppSettings["POVUrlEmailTemplateSolicitud"]);
                using (StreamReader reader = new StreamReader(serverPath))
                {
                    cuerpo = reader.ReadToEnd();
                }

                var horaInicioSesion = dto.hrsinicio.Split('T');
                var hrIn = horaInicioSesion[1];
                var horaFinSesion = dto.hrsfin.Split('T');
                var hrFn = horaFinSesion[1];                

                cuerpo = cuerpo.Replace("{altimg}", altimg);
                cuerpo = cuerpo.Replace("{urlimgbackground}", urlimgbackground);
                cuerpo = cuerpo.Replace("{urllogo}", urllogo);
                cuerpo = cuerpo.Replace("{title}", titulo);
                cuerpo = cuerpo.Replace("{linkportal}", linkportal);
                cuerpo = cuerpo.Replace("{orientador}", string.Format("{0}", orientador.NombreCompletoDocente));
                cuerpo = cuerpo.Replace("{aspirante}", string.Format("{0}", dto.nombrecompletoalumno));
                cuerpo = cuerpo.Replace("{dia}", dto.fecha);
                cuerpo = cuerpo.Replace("{inicio}", hrIn);
                cuerpo = cuerpo.Replace("{fin}", hrFn);
                cuerpo = cuerpo.Replace("{urlimgface}", urlimgface);
                cuerpo = cuerpo.Replace("{urlimgtwit}", urlimgtwit);
                cuerpo = cuerpo.Replace("{urlimglinked}", urlimglinked);
                cuerpo = cuerpo.Replace("{urlimginstagram}", urlimginstagram);

                List<string> tos = new List<string>();
                tos.Add(usuario.Email);
                AlternateView texto = null;
                List<string> archivos = new List<string>();
                List<string> copias = new List<string>();
                try
                {
                    correoCtrl.sendMessage(tos, "YOY - Solicitud", cuerpo, texto, archivos, copias);
                    dto.Success = "El correo se envió correctamente";
                    dto.Error = "";
                }
                catch (Exception ex)
                {
                    LoggerHlp.Default.Error(this, ex);
                    dto.Error = "Ha ocurrido un error mientras se enviaba el correo, intente más tarde";
                    return dto;
                }
                return dto;
        }

        [OperationContract]
        public eventcalendardto EnviarCorreoConfirmacion(eventcalendardto dto)
        {
            UsuarioCtrl usuarioCtrl = new UsuarioCtrl();
            LicenciaEscuelaCtrl licenciaEscuelaCtrl = new LicenciaEscuelaCtrl();
            EFAlumnoCtrl eFAlumnoCtrl = new EFAlumnoCtrl(null);
            Alumno alumno = eFAlumnoCtrl.Retrieve(new Alumno { AlumnoID = dto.alumnoid }, false).FirstOrDefault();
            Usuario usuario = licenciaEscuelaCtrl.RetrieveUsuario(dctx, alumno);
            usuario = usuarioCtrl.RetrieveComplete(dctx, usuario);
            Docente docente = licenciaEscuelaCtrl.RetrieveUsuarioOrientador(dctx, new Usuario { UsuarioID = dto.usuarioid });

            CorreoCtrl correoCtrl = new CorreoCtrl();
            #region Variables
            string urllogo = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgLogo"];
            string urlimgbackground = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgBackground"];
            string urlimgface = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgFace"];
            string urlimgtwit = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgTwit"];
            string urlimglinked = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgLinked"];
            string urlimginstagram = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgInstagram"];
            const string altimg = "YOY - Email";
            const string titulo = "Confirmación de solicitud";
            string dominio = String.Format("{0}://{1}:{2}/", HttpContext.Current.Request.Url.Scheme, HttpContext.Current.Request.Url.Host, HttpContext.Current.Request.Url.Port);
            string linkportal = ConfigurationManager.AppSettings["POVUrlAspirante"];
            #endregion

            string cuerpo = string.Empty;
            var serverPath = System.Web.Hosting.HostingEnvironment.MapPath(ConfigurationManager.AppSettings["POVUrlEmailTemplateConfirmacionOrientacion"]);
            using (StreamReader reader = new StreamReader(serverPath))
            {
                cuerpo = reader.ReadToEnd();
            }

            var horaInicioSesion = dto.hrsinicio.Split('T');
            var hrIn = horaInicioSesion[1];
            var horaFinSesion = dto.hrsfin.Split('T');
            var hrFn = horaFinSesion[1];

            cuerpo = cuerpo.Replace("{altimg}", altimg);
            cuerpo = cuerpo.Replace("{urlimgbackground}", urlimgbackground);
            cuerpo = cuerpo.Replace("{urllogo}", urllogo);
            cuerpo = cuerpo.Replace("{title}", titulo);
            cuerpo = cuerpo.Replace("{linkportal}", linkportal);
            cuerpo = cuerpo.Replace("{alumno}", string.Format("{0}", dto.nombrecompletoalumno));
            cuerpo = cuerpo.Replace("{orientador}", string.Format("{0}", docente.NombreCompletoDocente));
            cuerpo = cuerpo.Replace("{dia}", dto.fecha);
            cuerpo = cuerpo.Replace("{inicio}", hrIn);
            cuerpo = cuerpo.Replace("{fin}", hrFn);
            cuerpo = cuerpo.Replace("{usuarioSkype}", string.Format("{0}", docente.UsuarioSkype));
            cuerpo = cuerpo.Replace("{urlimgface}", urlimgface);
            cuerpo = cuerpo.Replace("{urlimgtwit}", urlimgtwit);
            cuerpo = cuerpo.Replace("{urlimglinked}", urlimglinked);
            cuerpo = cuerpo.Replace("{urlimginstagram}", urlimginstagram);

            List<string> tos = new List<string>();
            tos.Add(usuario.Email);
            AlternateView texto = null;
            List<string> archivos = new List<string>();
            List<string> copias = new List<string>();
            try
            {
                correoCtrl.sendMessage(tos, "YOY - Confirmación", cuerpo, texto, archivos, copias);
                dto.Success = "El correo se envió correctamente correctamente";
                dto.Error = "";
            }
            catch (Exception ex)
            {
                LoggerHlp.Default.Error(this, ex);
                dto.Error = "Ha ocurrido un error mientras se enviaba el correo, intente más tarde";
                return dto;
            }
            return dto;
        }

        [OperationContract]
        public List<eventcalendardto> GetEvento(eventcalendardto dto) 
        {
            OrientacionVocacionalDTOCtrl orientacionVocacionalDTOCtrl = new OrientacionVocacionalDTOCtrl();
            var result = orientacionVocacionalDTOCtrl.GetEvento(dto);

            return result;
        }

        [OperationContract]
        public List<eventcalendardto> GetEventoOrientador(eventcalendardto dto)
        {
            OrientacionVocacionalDTOCtrl orientacionVocacionalDTOCtrl = new OrientacionVocacionalDTOCtrl();
            var result = orientacionVocacionalDTOCtrl.GetEventoOrientador(dto);

            return result;
        }

        [OperationContract]
        public List<eventcalendardto> GetSolicitudes(eventcalendardto dto)
        {
            OrientacionVocacionalDTOCtrl orientacionVocacionalDTOCtrl = new OrientacionVocacionalDTOCtrl();
            var result = orientacionVocacionalDTOCtrl.GetSolicitudes(dto);

            return result;
        }

        [OperationContract]
        public eventcalendardto EliminarSesion(eventcalendardto dto)
        {
            OrientacionVocacionalDTOCtrl orientacionVocacionalDTOCtrl = new OrientacionVocacionalDTOCtrl();
            var result = orientacionVocacionalDTOCtrl.EliminarSesion(dto);

            return result;
        }

        [OperationContract]
        public eventcalendardto EnviarCorreoCancelacion(eventcalendardto dto)
        {
            UsuarioCtrl usuarioCtrl = new UsuarioCtrl();
            LicenciaEscuelaCtrl licenciaEscuelaCtrl = new LicenciaEscuelaCtrl();
            EFAlumnoCtrl eFAlumnoCtrl = new EFAlumnoCtrl(null);
            Alumno alumno = eFAlumnoCtrl.Retrieve(new Alumno { AlumnoID = dto.alumnoid }, false).FirstOrDefault();
            Usuario usuario = licenciaEscuelaCtrl.RetrieveUsuario(dctx, alumno);
            usuario = usuarioCtrl.RetrieveComplete(dctx, usuario);
            Docente docente = licenciaEscuelaCtrl.RetrieveUsuarioOrientador(dctx, new Usuario { UsuarioID = dto.usuarioid });

            CorreoCtrl correoCtrl = new CorreoCtrl();
            #region Variables
            string urllogo = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgLogo"];
            string urlimgbackground = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgBackground"];
            string urlimgface = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgFace"];
            string urlimgtwit = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgTwit"];
            string urlimglinked = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgLinked"];
            string urlimginstagram = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgInstagram"];
            const string altimg = "YOY - Email";
            const string titulo = "Cancelación de solicitud";
            string dominio = String.Format("{0}://{1}:{2}/", HttpContext.Current.Request.Url.Scheme, HttpContext.Current.Request.Url.Host, HttpContext.Current.Request.Url.Port);
            string linkportal = ConfigurationManager.AppSettings["POVUrlAspirante"];
            #endregion

            string cuerpo = string.Empty;
            var serverPath = System.Web.Hosting.HostingEnvironment.MapPath(ConfigurationManager.AppSettings["POVUrlEmailTemplateCancelacionOrientacion"]);
            using (StreamReader reader = new StreamReader(serverPath))
            {
                cuerpo = reader.ReadToEnd();
            }

            var horaInicioSesion = dto.hrsinicio.Split('T');
            var hrIn = horaInicioSesion[1];
            var horaFinSesion = dto.hrsfin.Split('T');
            var hrFn = horaFinSesion[1];

            cuerpo = cuerpo.Replace("{altimg}", altimg);
            cuerpo = cuerpo.Replace("{urlimgbackground}", urlimgbackground);
            cuerpo = cuerpo.Replace("{urlogo}", urllogo);
            cuerpo = cuerpo.Replace("{title}", titulo);
            cuerpo = cuerpo.Replace("{linkportal}", linkportal);
            cuerpo = cuerpo.Replace("{alumno}", string.Format("{0}", dto.nombrecompletoalumno));
            cuerpo = cuerpo.Replace("{orientador}", string.Format("{0}", docente.NombreCompletoDocente));
            cuerpo = cuerpo.Replace("{dia}", dto.fecha);
            cuerpo = cuerpo.Replace("{inicio}", hrIn);
            cuerpo = cuerpo.Replace("{fin}", hrFn);
            cuerpo = cuerpo.Replace("{urlimgface}", urlimgface);
            cuerpo = cuerpo.Replace("{urlimgtwit}", urlimgtwit);
            cuerpo = cuerpo.Replace("{urlimglinked}", urlimglinked);
            cuerpo = cuerpo.Replace("{urlimginstagram}", urlimginstagram);

            List<string> tos = new List<string>();
            tos.Add(usuario.Email);
            AlternateView texto = null;
            List<string> archivos = new List<string>();
            List<string> copias = new List<string>();
            try
            {
                correoCtrl.sendMessage(tos, "YOY - Cancelación", cuerpo, texto, archivos, copias);
                dto.Success = "La sesion se canceló correctamente";
                dto.Error = "";
            }
            catch (Exception ex)
            {
                LoggerHlp.Default.Error(this, ex);
                dto.Error = "Ha ocurrido un error mientras se guardaba la configuración, intente más tarde";
                return dto;
            }
            return dto;

        }

        [OperationContract]
        public List<sesionorientaciondto> GetSesionOrientacionAlumno(sesionorientaciondto dto) 
        {
            OrientacionVocacionalDTOCtrl orientacionVocacionalDTOCtrl = new OrientacionVocacionalDTOCtrl();
            var result = orientacionVocacionalDTOCtrl.GetSesionOrientacionAlumno(dto);

            return result;
        }

        [OperationContract]
        public List<sesionorientaciondto> GetSesionOrientacionDocente(sesionorientaciondto dto) 
        {
            OrientacionVocacionalDTOCtrl orientacionVocacionalDTOCtrl = new OrientacionVocacionalDTOCtrl();
            var result = orientacionVocacionalDTOCtrl.GetSesionOrientacionDocente(dto);

            return result;
        }

        [OperationContract]
        public sesionorientaciondto UpdateSesionOrientacion(sesionorientaciondto dto) 
        {
            OrientacionVocacionalDTOCtrl orientacionVocacionalDTOCtrl = new OrientacionVocacionalDTOCtrl();
            var result = orientacionVocacionalDTOCtrl.UpdateSesionOrientacion(dto);

            return result;
        }

        [OperationContract]
        public sesionorientaciondto FinalizarSesionOrientacion(sesionorientaciondto dto)
        {
            OrientacionVocacionalDTOCtrl orientacionVocacionalDTOCtrl = new OrientacionVocacionalDTOCtrl();
            var result = orientacionVocacionalDTOCtrl.FinalizarSesionOrientacion(dto);

            return result;
        }

        [OperationContract]
        public sesionorientaciondto EnviarCorreoReembolso(sesionorientaciondto dto)
        {
            UsuarioCtrl usuarioCtrl = new UsuarioCtrl();
            LicenciaEscuelaCtrl licenciaEscuelaCtrl = new LicenciaEscuelaCtrl();
            EFAlumnoCtrl eFAlumnoCtrl = new EFAlumnoCtrl(null);
            EFDocenteCtrl eFDocenteCtrl = new EFDocenteCtrl(null);
            Alumno alumno = eFAlumnoCtrl.Retrieve(new Alumno { AlumnoID = dto.alumnoid }, false).FirstOrDefault();
            Usuario usuario = licenciaEscuelaCtrl.RetrieveUsuario(dctx, alumno);
            usuario = usuarioCtrl.RetrieveComplete(dctx, usuario);
            Docente docente = eFDocenteCtrl.Retrieve(new Docente { DocenteID = dto.docenteid }, false).FirstOrDefault();

            CorreoCtrl correoCtrl = new CorreoCtrl();
            #region Variables
            string urllogo = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgLogo"];
            string urlimgbackground = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgBackground"];
            string urlimgface = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgFace"];
            string urlimgtwit = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgTwit"];
            string urlimglinked = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgLinked"];
            string urlimginstagram = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgInstagram"];
            const string altimg = "YOY - Email";
            const string titulo = "Reagendar";
            string dominio = String.Format("{0}://{1}:{2}/", HttpContext.Current.Request.Url.Scheme, HttpContext.Current.Request.Url.Host, HttpContext.Current.Request.Url.Port);
            string linkportal = ConfigurationManager.AppSettings["POVUrlAspirante"];
            #endregion

            string cuerpo = string.Empty;
            var serverPath = System.Web.Hosting.HostingEnvironment.MapPath(ConfigurationManager.AppSettings["POVUrlEmailTemplateReembolso"]);
            using (StreamReader reader = new StreamReader(serverPath))
            {
                cuerpo = reader.ReadToEnd();
            }

            var Fecha = dto.fecha;

            cuerpo = cuerpo.Replace("{altimg}", altimg);
            cuerpo = cuerpo.Replace("{urlimgbackground}", urlimgbackground);
            cuerpo = cuerpo.Replace("{urllogo}", urllogo);
            cuerpo = cuerpo.Replace("{title}", titulo);
            cuerpo = cuerpo.Replace("{linkportal}", linkportal);
            cuerpo = cuerpo.Replace("{aspirante}", string.Format("{0}", alumno.NombreCompletoAlumno));
            cuerpo = cuerpo.Replace("{orientador}", string.Format("{0}", docente.NombreCompletoDocente));
            cuerpo = cuerpo.Replace("{dia}", dto.fecha);
            cuerpo = cuerpo.Replace("{urlimgface}", urlimgface);
            cuerpo = cuerpo.Replace("{urlimgtwit}", urlimgtwit);
            cuerpo = cuerpo.Replace("{urlimglinked}", urlimglinked);
            cuerpo = cuerpo.Replace("{urlimginstagram}", urlimginstagram);

            List<string> tos = new List<string>();
            tos.Add(usuario.Email);
            AlternateView texto = null;
            List<string> archivos = new List<string>();
            List<string> copias = new List<string>();
            try
            {
                correoCtrl.sendMessage(tos, "YOY - Reagendar", cuerpo, texto, archivos, copias);
                dto.Success = "El correo se envió correctamente";
                dto.Error = "";
            }
            catch (Exception ex)
            {
                LoggerHlp.Default.Error(this, ex);
                dto.Error = "Ha ocurrido un error mientras se enviaba el correo, intente más tarde";
                return dto;
            }
            return dto;

        }

        [OperationContract]
        public respuestaencuestadto GuardarEncuestaSatisfaccion(respuestaencuestadto dto)
        {
            OrientacionVocacionalDTOCtrl orientacionVocacionalDTOCtrl = new OrientacionVocacionalDTOCtrl();
            var result = orientacionVocacionalDTOCtrl.GuardarEncuestaSatisfaccion(dto);

            return result;
        }        
        // Agregue aquí más operaciones y márquelas con [OperationContract]
    }
}
