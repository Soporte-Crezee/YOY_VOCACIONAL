using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using POV.Prueba.DTO.BO;
using POV.Prueba.DTO.Service;
using POV.Prueba.BO;
using POV.Expediente.BO;
using System.Web;
using POV.Web.Portal.Pruebas.Helper;
using Framework.Base.DataAccess;
using System.Configuration;
using POV.ConfiguracionActividades.BO;
using POV.Licencias.BO;
using POV.CentroEducativo.BO;
using POV.Licencias.Service;
using POV.ServiciosActividades.Controllers;
using System.IO;
using System.Collections;

namespace POV.Web.Portal.Pruebas.wcf
{
    [ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class ReactivoService
    {
        #region Variables de sesión
        private AResultadoPrueba Session_ResultadoPrueba
        {
            get
            {
                AResultadoPrueba prueba = null;
                if (HttpContext.Current.Session["ResultadoPrueba"] != null)
                    prueba = HttpContext.Current.Session["ResultadoPrueba"] as AResultadoPrueba;
                return prueba;
            }
            set { HttpContext.Current.Session["ResultadoPrueba"] = value; }
        }
        private long? Session_AsignacionActividadId
        {
            get { return HttpContext.Current.Session["AsignacionActividadId"] as long?; }
            set { HttpContext.Current.Session["AsignacionActividadId"] = value; }
        }

        private long? Session_TareaRealizadaId
        {
            get { return HttpContext.Current.Session["TareaRealizadaId"] as long?; }
            set { HttpContext.Current.Session["TareaRealizadaId"] = value; }
        }

        private Alumno Session_Alumno
        {
            get { return (Alumno)HttpContext.Current.Session["Alumno"]; }
            set { HttpContext.Current.Session["Alumno"] = value; }
        }
        private LicenciaEscuela Session_LicenciaEscuela
        {
            get { return (LicenciaEscuela)HttpContext.Current.Session["Session_LicenciaEscuela"]; }
            set { HttpContext.Current.Session["Session_LicenciaEscuela"] = value; }
        }

        private APrueba Session_Prueba
        {
            get { return (APrueba)HttpContext.Current.Session["Prueba"]; }
            set { HttpContext.Current.Session["Prueba"] = value; }
        }

        #endregion

        [OperationContract]
        public List<reactivodto> GetNextReactivo(reactivodto dto)
        {
            try
            {
                IDataContext dctx = ConnectionHlp.Default.Connection;
                string urlImage = string.Empty;
                if (this.Session_ResultadoPrueba.Prueba.TipoPrueba == ETipoPrueba.Estandarizada)
                    urlImage = @ConfigurationManager.AppSettings["POVRutaImgReactivosEstandarizado"];
                else if (this.Session_ResultadoPrueba.Prueba.TipoPrueba == ETipoPrueba.Dinamica)
                    urlImage = @ConfigurationManager.AppSettings["POVRutaImgReactivosDinamico"];

                PruebaDTOCtrl dtoCtrl = new PruebaDTOCtrl(urlImage, this.Session_ResultadoPrueba as IResultadoPrueba);

                List<reactivodto> result = dtoCtrl.GetNextReactivo(dctx, dto, Session_Prueba);


                if (result.FirstOrDefault().esfinal.Value)
                {
                    if (Session_AsignacionActividadId != null && Session_TareaRealizadaId != null)
                    {
                        RealizarActividadesController controller = new RealizarActividadesController();

                        controller.FinalizarTarea(new AsignacionActividad
                        {
                            AsignacionActividadId = Session_AsignacionActividadId,
                            TareasRealizadas = new List<TareaRealizada> { new TareaRealizada { TareaRealizadaId = Session_TareaRealizadaId.Value,
                        ResultadoPruebaId = Session_ResultadoPrueba.ResultadoPruebaID
                        } }

                        });
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                Logger.Service.LoggerHlp.Default.Error(this, ex);
                throw ex;
            }
        }

        [OperationContract]
        public int RegistrarRespuesta(respuestareactivodto dto)
        {
            IDataContext dctx = ConnectionHlp.Default.Connection;
            string urlImage = string.Empty;
            if (this.Session_ResultadoPrueba.Prueba.TipoPrueba == ETipoPrueba.Estandarizada)
                urlImage = @ConfigurationManager.AppSettings["POVRutaImgReactivosEstandarizado"];
            else if (this.Session_ResultadoPrueba.Prueba.TipoPrueba == ETipoPrueba.Dinamica)
                urlImage = @ConfigurationManager.AppSettings["POVRutaImgReactivosDinamico"];

            PruebaDTOCtrl dtoCtrl = new PruebaDTOCtrl(urlImage, this.Session_ResultadoPrueba as IResultadoPrueba);
            try
            {
                return dtoCtrl.RegistrarRespuesta(dctx, dto);
            }
            catch (Exception ex)
            {
                Logger.Service.LoggerHlp.Default.Error(this, ex);
                throw ex;
            }
        }

        [OperationContract]
        public POV.Web.DTO.alumnodto RedireccionSocial(POV.Web.DTO.alumnodto alumno)
        {
            try
            {
                string ruta = System.Web.Hosting.HostingEnvironment.MapPath("~/redireccion.txt");
                StreamReader objReader = new StreamReader(ruta);
                string sLine = "";
                ArrayList arrText = new ArrayList();
                while (sLine != null)
                {
                    sLine = objReader.ReadLine();
                    if (sLine != null)
                        arrText.Add(sLine);
                }
                string dominio = "";
                dominio = String.Format("{0}://{1}:{2}/", HttpContext.Current.Request.Url.Scheme, HttpContext.Current.Request.Url.Host, HttpContext.Current.Request.Url.Port);
                var pathBase = dominio + (string)arrText[0];

                string strUrlAutoLogin;
                string formatoFecha = "yyyyMMddHHmmss.fff";
                DateTime fecha = System.DateTime.Now;
                string nombre = alumno.nombre;
                nombre = nombre.Trim();
                string apellido = alumno.primerapellido;
                apellido = apellido.Trim();
                string curp = alumno.curp;
                DateTime fechaNacimiento = Convert.ToDateTime(alumno.fechanacimiento);
                curp = curp.Trim();
                string token = fechaNacimiento.ToString(formatoFecha) + nombre + apellido + curp + fecha.ToString(formatoFecha);
                byte[] clave = POV.Seguridad.Utils.EncryptHash.SHA1encrypt(token);
                token = POV.Seguridad.Utils.EncryptHash.byteArrayToStringBase64(clave);
                token = System.Web.HttpUtility.UrlEncode(token);
                strUrlAutoLogin = "?alumno=" + alumno.curp + "&fechahora=" + fecha.ToString(formatoFecha) + "&token=" + token + "&portal=pruebas";

                string redirect = pathBase + strUrlAutoLogin;
                alumno.redirect = redirect;
                return alumno;
            }
            catch (Exception ex)
            {
                Logger.Service.LoggerHlp.Default.Error(this, ex);
                throw ex;
            }
        }

        [OperationContract]
        public reactivoesatusbardto GetTotalReactivos()
        {
            try
            {
                IDataContext dctx = ConnectionHlp.Default.Connection;
                string urlImage = string.Empty;

                if (this.Session_ResultadoPrueba.Prueba.TipoPrueba == ETipoPrueba.Estandarizada)
                    urlImage = @ConfigurationManager.AppSettings["POVRutaImgReactivosEstandarizado"];
                else if (this.Session_ResultadoPrueba.Prueba.TipoPrueba == ETipoPrueba.Dinamica)
                    urlImage = @ConfigurationManager.AppSettings["POVRutaImgReactivosDinamico"];

                PruebaDTOCtrl dtoCtrl = new PruebaDTOCtrl(urlImage, this.Session_ResultadoPrueba as IResultadoPrueba);

                reactivoesatusbardto totalReactivos = dtoCtrl.GetEstatusReactivos();

                return totalReactivos;
            }
            catch (Exception ex)
            {
                Logger.Service.LoggerHlp.Default.Error(this, ex);
                throw ex;
            }
        }
    }
}
