using Framework.Base.DataAccess;
using POV.CentroEducativo.BO;
using POV.CentroEducativo.Services;
using POV.Comun.Service;
using POV.Licencias.Service;
using POV.Logger.Service;
using POV.Seguridad.BO;
using POV.Seguridad.Service;
using POV.Web.DTO;
using POV.Web.DTO.Services;
using POV.Web.Helper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using System.Web;

namespace POV.Web.HerramientasDocente.wcf
{
    [ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class OrientacionVocacionalService
    {
        private readonly IDataContext dctx = ConnectionHlp.Default.Connection;
        // Para usar HTTP GET, agregue el atributo [WebGet]. (El valor predeterminado de ResponseFormat es WebMessageFormat.Json)
        // Para crear una operación que devuelva XML,
        //     agregue [WebGet(ResponseFormat=WebMessageFormat.Xml)]
        //     e incluya la siguiente línea en el cuerpo de la operación:
        //         WebOperationContext.Current.OutgoingResponse.ContentType = "text/xml";
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
            const string titulo = "Reembolso";
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

            cuerpo = cuerpo.Replace("{urllogo}", urllogo);
            cuerpo = cuerpo.Replace("{urlimgbackground}", urlimgbackground);
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
                correoCtrl.sendMessage(tos, "YOY - Reembolso", cuerpo, texto, archivos, copias);
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

        // Agregue aquí más operaciones y márquelas con [OperationContract]
    }
}
