using Framework.Base.DataAccess;
using GP.SocialEngine.BO;
using GP.SocialEngine.Service;
using POV.CentroEducativo.BO;
using POV.CentroEducativo.Service;
using POV.CentroEducativo.Services;
using POV.Comun.Service;
using POV.Core.RedSocial.Implement;
using POV.Core.RedSocial.Interfaces;
using POV.Licencias.Service;
using POV.Logger.Service;
using POV.Seguridad.BO;
using POV.Seguridad.Service;
using POV.Web.DTO;
using POV.Web.DTO.Services;
using POV.Web.PortalSocial.AppCode;
using POV.Web.PortalSocial.wcf;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace POV.Web.PortalSocial.PortalGrupo
{
    public partial class SolicitudOrientacionVocacional : System.Web.UI.UserControl
    {
        private string fecha;
        public string Fecha
        {
            get { return fecha; }
            set { fecha = value; }
        }

        public Usuario Orientador
        {
            get { return Session["Orientador"] as Usuario; }
            set { Session["Orientador"] = value; }
        }

        private IUserSession userSession;
        private IRedirector redirector;
        private UsuarioCtrl usuarioCtrl;
        private AlumnoCtrl alumnoCtrl;
        private readonly IDataContext dctx = ConnectionHelper.Default.Connection;
        //private Usuario usuario;

        public SolicitudOrientacionVocacional() 
        {
            userSession = new UserSession();
            redirector = new Redirector();
            usuarioCtrl = new UsuarioCtrl();
            alumnoCtrl = new AlumnoCtrl();
        }

        #region Eventos de la pagina
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (userSession.IsLogin())
                {                    
                    CargarDatos();
                }
                else
                    redirector.GoToLoginPage(true);
            }
        }

        public event EventHandler SolicitarClicked;
        protected virtual void OnClick(object sender) 
        {
            if (this.SolicitarClicked != null)
            {
                this.SolicitarClicked(sender, new EventArgs());
            }
        }
        
        protected void btnSolicitar_Click(object sender, EventArgs e)
        {
            try
            {
                
                // validar horas de trabajo del orientador
                // validar dias de trabajo del orientador
                // validar descanso del orientador
                string nombre = txtNombreEvento.Text;
                fecha=this.hdnFecha.Value;
                string hInicio = this.hdnInicio.Value; //hdnInicio.ToString();
                string hFin = this.hdnFin.Value;
                //string correoOri = Orientador.Email.ToString();
                string correoOri = "k.tato@hotmail.com";

                OnClick(sender);

                enviarSolicitud(correoOri, userSession.CurrentAlumno, fecha, hInicio, hFin);                
            }
            catch (Exception)
            {                
                throw;
            }
        }
        #endregion

        #region Metodos Auxiliares
        protected void CargarDatos()
        {
            this.hdnSessionSocialHubID.Value = userSession.SocialHub.SocialHubID.ToString();
            this.hdnSessionUsuarioSocialID.Value = userSession.CurrentUsuarioSocial.UsuarioSocialID.ToString();

            #region *** validaciones del usuario que llega del request ***
            Int64 usuarioSocialID;

            string user = Request.QueryString["u"];
            user = string.IsNullOrEmpty(user) ? "0" : user;

            if (!Int64.TryParse(user, out usuarioSocialID))
            {
                redirector.GoToHomePage(true);
                return;
            }
            #endregion

            hdnTipoPublicacionSuscripcionReactivo.Value = ((short)ETipoPublicacion.REACTIVO).ToString();
            hdnTipoPublicacionTexto.Value = ((short)ETipoPublicacion.TEXTO).ToString();

            if (usuarioSocialID <= 0 || usuarioSocialID == userSession.CurrentUsuarioSocial.UsuarioSocialID) // el 
            {
                redirector.GoToHomePage(true);
            }
            else
            {
                UsuarioSocial usuarioSocial = new UsuarioSocialCtrl().RetrieveComplete(dctx, new UsuarioSocial { UsuarioSocialID = usuarioSocialID });
                //validamos que el usuario exista
                if (usuarioSocial != null)
                {
                    UsuarioCtrl userCtrl = new UsuarioCtrl();
                    LicenciaEscuelaCtrl licenciaEscuelaCtrl = new LicenciaEscuelaCtrl();
                    Usuario usuario = licenciaEscuelaCtrl.RetrieveUsuario(dctx, usuarioSocial);
                    usuario = userCtrl.LastDataRowToUsuario(userCtrl.Retrieve(dctx, new Usuario() { UsuarioID = usuario.UsuarioID }));
                    //Session["UsuarioIDDocente"] = usuario;
                    Orientador = usuario;
                    this.hdnSessionUsuarioID.Value = usuario.UsuarioID.ToString();
                    txtNombreEvento.Enabled = false;
                    txtNombreEvento.Text = "Solicitud de orientación vocacional";
                    //configcalendardto dto = new configcalendardto();
                    //dto.usuarioid = usuario.UsuarioID;
                
                    //OrientacionVocacionalDTOCtrl ovs = new OrientacionVocacionalDTOCtrl();
                    //ovs.GetConfiguracionAgendaOrientador(dto);
                    
                    //// para ver el muro del usuario, el visitante debe pertenecer al grupo 
                    //GrupoSocialCtrl grupoSocialCtrl = new GrupoSocialCtrl();
                    ////verificamos que en el grupo del visitante se encuentre el propietario del muro
                    ////ademas no debe ser docente
                    //UsuarioGrupo usuarioGrupo = new UsuarioGrupo();
                    //if (userSession.IsDocente())
                    //    usuarioGrupo = grupoSocialCtrl.RetrieveFriend(dctx, userSession.CurrentGrupoSocial, usuarioSocial, userSession.CurrentAlumno.AreasConocimiento, userSession.CurrentDocente.DocenteID, userSession.CurrentUser.UniversidadId);
                    //else
                    //    usuarioGrupo = grupoSocialCtrl.RetrieveFriend(dctx, userSession.CurrentGrupoSocial, usuarioSocial, userSession.CurrentAlumno.AreasConocimiento);

                    //    if ((bool)usuarioGrupo.EsModerador) // si es su companiero y  es docente
                    //    {
                    //        SocialHubCtrl socialHubCtrl = new SocialHubCtrl();
                    //        DataSet dsSocialHub = socialHubCtrl.RetrieveSocialHubUsuario(dctx, new SocialHub { SocialProfile = usuarioSocial, SocialProfileType = ESocialProfileType.USUARIOSOCIAL });

                    //        SocialHub socialHub = socialHubCtrl.LastDataRowToSocialHub(dsSocialHub);

                    //        hdnSocialHubID.Value = socialHub.SocialHubID.ToString();
                    //        hdnUsuarioSocialID.Value = ((UsuarioSocial)socialHub.SocialProfile).UsuarioSocialID.Value.ToString();

                    //        //Orientador
                    //        this.LblNombreEscuela.Text = string.Format(" Docente De:{0} ", userSession.CurrentEscuela.NombreEscuela);
                    //        this.LblNombreDocente.Text = usuarioSocial.ScreenName;

                    //        this.ImgUser.ImageUrl = UrlHelper.GetImagenPerfilURL("normal", (long)usuarioSocial.UsuarioSocialID);

                    //        UniversidadCtrl uniCtrl = new UniversidadCtrl(null);
                    //        UsuarioCtrl userCtrl = new UsuarioCtrl();

                    //        LicenciaEscuelaCtrl licenciaEscuelaCtrl = new LicenciaEscuelaCtrl();
                    //        Docente docente = licenciaEscuelaCtrl.RetrieveDocente(dctx, usuarioSocial);
                    //        string siglasUniversidad = string.Empty;

                    //        Usuario usuario = licenciaEscuelaCtrl.RetrieveUsuario(dctx, usuarioSocial);
                    //        usuario = userCtrl.LastDataRowToUsuario(userCtrl.Retrieve(dctx, new Usuario() { UsuarioID = usuario.UsuarioID }));

                    //        if (usuario.UniversidadId != null)
                    //        {
                    //            var universidadUsuario = uniCtrl.Retrieve(new Universidad() { UniversidadID = usuario.UniversidadId }, false).FirstOrDefault();
                    //            siglasUniversidad = " (" + universidadUsuario.Siglas + ")";
                    //            this.LblNombreUniversidad.Text = universidadUsuario.NombreUniversidad;
                    //        }

                    //        this.LblNombreUsuario.Text = string.Format("{0} {1}", usuarioSocial.ScreenName, siglasUniversidad);
                    //        GrupoCicloEscolarCtrl grupoCtrl = new GrupoCicloEscolarCtrl();
                    //        List<Materia> materias = grupoCtrl.RetrieveMateriasDocente(dctx, docente, new GrupoCicloEscolar { GrupoSocialID = userSession.CurrentGrupoSocial.GrupoSocialID });

                    //        string sMaterias = string.Empty;

                    //        foreach (Materia materia in materias)
                    //        {
                    //            if (sMaterias.Length > 0)
                    //                sMaterias += ", ";
                    //            sMaterias += materia.Titulo;
                    //        }
                    //        this.HplPerfil.NavigateUrl = UrlHelper.GetPerfilURL((long)usuarioSocial.UsuarioSocialID);
                    //        this.LblAsignatura.Text = string.Format(" Asignatura(s): {0}.", sMaterias);
                    //    }
                    //    else
                    //        redirector.GoToHomePage(true);
                }
                else
                    redirector.GoToHomePage(true);
            }
        }
        
        // Enviar correo de solicitud de sesion
        protected void enviarSolicitud(string email, Alumno alumno, string dia, string inicio, string fin) 
        {
            CorreoCtrl correoCtrl = new CorreoCtrl();
            #region Variables
            string urlimg = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgEmail"];
            const string altimg="POV - Email";
            const string titulo = "Solicitud";
            string dominio = String.Format("{0}://{1}:{2}/", HttpContext.Current.Request.Url.Scheme, HttpContext.Current.Request.Url.Host, HttpContext.Current.Request.Url.Port);
            string linkportal = ConfigurationManager.AppSettings["POVUrlPortalAspirante"];
            // redireccion
            string location = "OrientacionVocacional.aspx?u=" + Orientador.UsuarioID;
            #endregion

            string cuerpo = string.Empty;

            using (StreamReader reader=new StreamReader(Server.MapPath("~/Files/protocolo/EmailTemplateSolicitudOrientacion.html")))
            {
                cuerpo = reader.ReadToEnd();
            }

            cuerpo = cuerpo.Replace("{altimg}", altimg);
            cuerpo = cuerpo.Replace("{urlimg}", urlimg);
            cuerpo = cuerpo.Replace("{title}", titulo);
            cuerpo = cuerpo.Replace("{linkportal}", linkportal);
            cuerpo = cuerpo.Replace("{nombre}", string.Format("{0}", alumno.NombreCompletoAlumno));
            cuerpo = cuerpo.Replace("{dia}", dia);
            cuerpo = cuerpo.Replace("{inicio}", inicio);
            cuerpo = cuerpo.Replace("{fin}", fin);

            List<string> tos = new List<string>();
            tos.Add(email);
            AlternateView texto = null;
            List<string> archivos = new List<string>();
            List<string> copias = new List<string>();
            try
            {
                correoCtrl.sendMessage(tos, "POV - Solicitud", cuerpo, texto, archivos, copias);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('¡La solicitud ha sido enviada correctamente!'); window.location='" + location + "'", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Lo sentimos el correo no pudo ser enviado, intentelo mas tarde.');", true);
                LoggerHlp.Default.Error(this, ex);
            }
        }
        #endregion

        //protected void btnCancelar_Click(object sender, EventArgs e)
        //{

        //}

        //protected void btnGuardarCambios_Click(object sender, EventArgs e)
        //{

        //}

    }
}