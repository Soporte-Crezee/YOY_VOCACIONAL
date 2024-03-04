using conekta;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using Newtonsoft.Json.Linq;
using POV.Administracion.BO;
using POV.Administracion.Services;
using POV.CentroEducativo.BO;
using POV.CentroEducativo.Service;
using POV.CentroEducativo.Services;
using POV.Comun.BO;
using POV.Comun.Service;
using POV.Core.PadreTutor.Implements;
using POV.Core.PadreTutor.Interfaces;
using POV.Expediente.BO;
using POV.Expediente.Services;
using POV.Licencias.Service;
using POV.Logger.Service;
using POV.Modelo.BO;
using POV.Seguridad.BO;
using POV.Seguridad.Service;
using POV.Web.PortalTutor.Helper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace POV.Web.PortalTutor.Pages
{
    public partial class Tutorados : System.Web.UI.Page
    {
        private IUserSession userSession;
        private IRedirector redirector;
        private readonly IDataContext dctx = ConnectionHlp.Default.Connection;
        private CompraPremiumCtrl compraPremiumCtrl;
        private InfoAlumnoUsuario alumnoUsuario;
        private InfoAlumnoUsuarioCtrl infoAlumnoUsuarioCtrl;
        private LicenciaEscuelaCtrl licenciaEscuelaCtrl;
        private UsuarioExpedienteCtrl usuarioExpedienteCtrl;

        private string nombreCompleto = string.Empty;
        private string telefono = string.Empty;
        private string correo = string.Empty;

        private TutorCtrl tutorCtrl;

        private List<TutorAlumno> Session_Alumnos
        {
            get { return (List<TutorAlumno>)this.Session["LIST_ALUMNO"]; }
            set { this.Session["LIST_ALUMNO"] = value; }
        }

        private List<Alumno> Session_Alumnos_Seleccionados
        {
            get { return (List<Alumno>)this.Session["LIST_ALUMNOS"]; }
            set { this.Session["LIST_ALUMNOS"] = value; }
        }

        private NotaCompra Session_NotaCompra
        {
            get { return (NotaCompra)this.Session["NOTACOMPRA"]; }
            set { this.Session["NOTACOMPRA"] = value; }
        }

        private Alumno Session_AlumnoAsignado
        {
            get { return (Alumno)this.Session["ALUMNOASIGNADO"]; }
            set { this.Session["ALUMNOASIGNADO"] = value; }
        }

        public Tutorados()
        {
            userSession = new UserSession();
            redirector = new Redirector();

            alumnoUsuario = new InfoAlumnoUsuario();
            infoAlumnoUsuarioCtrl = new InfoAlumnoUsuarioCtrl();
            compraPremiumCtrl = new CompraPremiumCtrl(null);
            licenciaEscuelaCtrl = new LicenciaEscuelaCtrl();
            usuarioExpedienteCtrl = new UsuarioExpedienteCtrl();

            tutorCtrl = new TutorCtrl(null);

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    if (userSession.IsLogin())
                    {
                        if ((bool)userSession.CurrentTutor.DatosCompletos && (bool)userSession.CurrentTutor.CorreoConfirmado)
                        {
                            // Llaves de produccion habilitadas
                            // Llaves de produccion publica y privada, respectivamente
                            // key_VXUQtCKLAXyZ3S8y6eWmjyA (Publica) key_QSuFnZEmUUJAiSkVzbr1XQ (privada)
                            // reemplazarlas por la de desarrollo
                            // Llaves de desarrollo publica y privada, respectivamente
                            // key_PV5xao1PfvTm3ydFbw186CA (Publica) key_cixWekt58XwyhoFzwS4csw (Privada)
                            // aspx Conekta.setPublishableKey y codebehind conekta.Api.apiKey
                            conekta.Api.apiKey = "key_QSuFnZEmUUJAiSkVzbr1XQ";
                            conekta.Api.version = "2.0.0";

                            ConsultarAlumnoUsuario(userSession.CurrentTutor);
                        }
                        else 
                        {
                            redirector.GoToHomePage(true);
                        }
                    }
                    else
                    {
                        redirector.GoToLoginPage(true);
                    }
                }
                else
                {
                    if (!userSession.IsLogin())
                    {
                        redirector.GoToLoginPage(true);
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerHlp.Default.Error(this, ex);
                throw ex;
            }
        }

        private void ConsultarAlumnoUsuario(Tutor tutor)
        {
            TutorCtrl tutCtrl = new TutorCtrl(null);
            Session_Alumnos = tutCtrl.Retrieve(tutor, false).FirstOrDefault().Alumnos.ToList();//userSession.CurrentTutor.Alumnos.ToList();

            LlenarDropAlumnosUsuarios(Session_Alumnos);
            DoFillExpireCard();
        }

        void DoFillExpireCard()
        {
            
            #region Drop Mes
            for (int i = 1; i < 15; i++)
            {
                ddlVenceAnioCC.Items.Add(new ListItem(((DateTime.Now.Year-1) + i).ToString().Trim(), (DateTime.Now.Year + i).ToString().Trim()));
                ddlVenceAnioMSS.Items.Add(new ListItem(((DateTime.Now.Year - 1) + i).ToString().Trim(), (DateTime.Now.Year + i).ToString().Trim()));
            }
            #endregion
        }

        public InfoAlumnoUsuario InterfaceToFiltroAlumnoUsuario()
        {
            InfoAlumnoUsuario info = new InfoAlumnoUsuario();
            info.clasificador = new Clasificador();
            info.estado = new Estado();

            return info;
        }

        private void LlenarDropAlumnosUsuarios(List<TutorAlumno> lista)
        {
            gvTutorados.DataSource = lista.GroupBy(test => test.AlumnoID).Select(grp => grp.First()).ToList();
            gvTutorados.DataBind();
        }

        protected void btnBuscarAlumno_Click(object sender, EventArgs e)
        {
            try
            {
                if (TxtNombreTutorado.Text.Trim() != string.Empty)
                {
                    gvTutorados.DataSource = Session_Alumnos.Where(x => x.Alumno.Nombre.ToLower().Contains(TxtNombreTutorado.Text.Trim().ToLower())).ToList();
                }
                else
                {
                    gvTutorados.DataSource = Session_Alumnos;
                }
                gvTutorados.DataBind();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alert('Ha ocurrido un error en la carga de tutorados');", true);
                LoggerHlp.Default.Error(this, ex);
            }
        }

        protected void BtnEnviar_Click(object sender, EventArgs e)
        {
            try
            {
                string EmailAspirante = TxtEmailAspirante.Text.Trim();
                //Formato Incorrecto
                if ((EmailAspirante.Length > 0 && EmailAspirante.Trim().Length < 100) && ValidateEmailRegex(EmailAspirante))
                    enviarCorreo(EmailAspirante, userSession.CurrentTutor);
                else
                {
                    if (!string.IsNullOrEmpty(EmailAspirante))
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alert('El correo electrónico tiene un formato incorrecto.');", true);
                    else
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alert('El correo electrónico es requerido.');", true);
                }
            }
            catch (Exception ex)
            {
                LoggerHlp.Default.Error(this, ex);
            }
        }

        protected void gvTutorados_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                EEstadoPago estadoPago = (EEstadoPago)Enum.Parse(typeof(EEstadoPago), (e.Row.Cells[1].Text));

                CheckBox cb = (CheckBox)e.Row.FindControl("chkSeleccionado");
                Label lbl = (Label)e.Row.FindControl("Label4");

                switch (estadoPago)
                {
                    case EEstadoPago.NO_PAGADO:
                        cb.Checked = false;
                        cb.Enabled = true;
                        cb.Attributes.Add("style", "display: block");
                        lbl.Attributes.Add("style", "display: none");
                        lbl.Text = string.Empty;
                        break;
                    case EEstadoPago.PENDIENTE:
                        cb.Checked = true;
                        cb.Enabled = false;
                        cb.Attributes.Add("style", "display: none");
                        lbl.Attributes.Add("style", "display: block");
                        lbl.Text = "PENDIENTE";
                        break;
                    case EEstadoPago.PAGADO:
                        cb.Checked = true;
                        cb.Enabled = false;
                        cb.Attributes.Add("style", "display: none");
                        lbl.Attributes.Add("style", "display: block");
                        lbl.Text = "PAGADO";
                        break;
                }
                if (cb.Checked == true)
                {
                    cb.Enabled = false;
                    //cb.Attributes.Add("style", "display: none");
                }

            }
        }

        private bool ValidateEmailRegex(string email)
        {
            string patternLenient = @"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";
            Regex reLenient = new Regex(patternLenient);

            bool match = reLenient.IsMatch(email);
            return match;
        }

        //Enviar Correo de registro exitoso
        private void enviarCorreo(string email, Tutor tutor)
        {
            CorreoCtrl correoCtrl = new CorreoCtrl();
            #region Variables
            string urllogo = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgLogo"];
            string urlimgbackground = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgBackground"];
            string urlimgface = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgFace"];
            string urlimgtwit = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgTwit"];
            string urlimglinked = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgLinked"];
            string urlimginstagram = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgInstagram"];
            const string altimg = "YOY - Email";
            const string titulo = "invitación";
            string dominio = String.Format("{0}://{1}:{2}/", HttpContext.Current.Request.Url.Scheme, HttpContext.Current.Request.Url.Host, HttpContext.Current.Request.Url.Port);
            string linkportal = ConfigurationManager.AppSettings["POVUrlPortalAspirante"];
            string location = ConfigurationManager.AppSettings["POVUrlTutorados"];
            #endregion

            string cuerpo = string.Empty;
            using (StreamReader reader = new StreamReader(Server.MapPath("~/Files/protocolo/EmailTemplateInviteAspirante.html")))
            {
                cuerpo = reader.ReadToEnd();
            }

            cuerpo = cuerpo.Replace("{urlimgbackground}", urlimgbackground);
            cuerpo = cuerpo.Replace("{urllogo}", urllogo);
            cuerpo = cuerpo.Replace("{title}", titulo);
            cuerpo = cuerpo.Replace("{linkportal}", linkportal);
            cuerpo = cuerpo.Replace("{tutor}", string.Format("{0} {1} {2}", tutor.Nombre, tutor.PrimerApellido, tutor.SegundoApellido));
            cuerpo = cuerpo.Replace("{codigo}", tutor.Codigo);
            cuerpo = cuerpo.Replace("{urlimgface}", urlimgface);
            cuerpo = cuerpo.Replace("{urlimgtwit}", urlimgtwit);
            cuerpo = cuerpo.Replace("{urlimglinked}", urlimglinked);
            cuerpo = cuerpo.Replace("{urlimginstagram}", urlimginstagram);

            List<string> tos = new List<string>();
            tos.Add(email);
            AlternateView texto = null;
            List<string> archivos = new List<string>();
            List<string> copias = new List<string>();
            try
            {
                correoCtrl.sendMessage(tos, "YOY - Invitación", cuerpo, texto, archivos, copias);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alert('¡La invitación ha sido enviada correctamente!'); window.location='" + location + "';", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alert('Lo sentimos el correo no pudo ser enviado, intentelo mas tarde.');", true);
                LoggerHlp.Default.Error(this, ex);
            }
        }

        protected void gvTutorados_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName.Trim())
            {
                case "Asignar":
                    {
                        try
                        {
                            Session_AlumnoAsignado = new EFAlumnoCtrl(null).Retrieve(new Alumno() { AlumnoID = long.Parse(e.CommandArgument.ToString()) }, true).FirstOrDefault();
                            Session_AlumnoAsignado.Premium = true;
                            var alumno = e.CommandArgument as Alumno;
                            var tutorCredito = tutorCtrl.Retrieve(new Tutor { TutorID = userSession.CurrentTutor.TutorID }, false).FirstOrDefault();

                            ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "viewModal();", true);
                        }
                        catch (Exception ex)
                        {
                            this.ShowMessage(ex.Message, MessageType.Error);
                        }
                        break;
                    }
                default: { ShowMessage("Comando no encontrado" + e.CommandName.Trim(), MessageType.Error); break; }

            }
        }

        //Enviar Correo de registro exitoso
        private void enviarNotificacion(string email, Alumno alumno, double? creditoAsignado)
        {
            CorreoCtrl correoCtrl = new CorreoCtrl();
            #region Variables
            string urllogo = ConfigurationManager.AppSettings["POVUrlImgLogo"];
            string urlimgbackground = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgBackground"];
            string urlimgface = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgFace"];
            string urlimgtwit = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgTwit"];
            string urlimglinked = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgLinked"];
            string urlimginstagram = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgInstagram"];
            const string altimg = "YOY - Email";
            const string titulo = "Notificación";
            string dominio = String.Format("{0}://{1}:{2}/", HttpContext.Current.Request.Url.Scheme, HttpContext.Current.Request.Url.Host, HttpContext.Current.Request.Url.Port);
            string linkportal = ConfigurationManager.AppSettings["POVUrlPortalAspirante"];
            string location = ConfigurationManager.AppSettings["POVUrlTutorados"];
            #endregion

            string monto = creditoAsignado.ToString();

            string cuerpo = string.Empty;
            using (StreamReader reader = new StreamReader(Server.MapPath("~/Files/protocolo/EmailTemplateNotificacionCredito.html")))
            {
                cuerpo = reader.ReadToEnd();
            }
            cuerpo = cuerpo.Replace("{urllogo}", urllogo);
            cuerpo = cuerpo.Replace("{urlimgbackground}", urlimgbackground);
            cuerpo = cuerpo.Replace("{title}", titulo);
            cuerpo = cuerpo.Replace("{linkportal}", linkportal);
            cuerpo = cuerpo.Replace("{urlimgface}", urlimgface);
            cuerpo = cuerpo.Replace("{urlimgtwit}", urlimgtwit);
            cuerpo = cuerpo.Replace("{urlimglinked}", urlimglinked);
            cuerpo = cuerpo.Replace("{urlimginstagram}", urlimginstagram);
            if (!string.IsNullOrEmpty(monto))
                cuerpo = cuerpo.Replace("{monto}", monto);
            cuerpo = cuerpo.Replace("{nombre}", string.Format("{0}", alumno.NombreCompletoAlumno));
            cuerpo = cuerpo.Replace("{tutor}", string.Format("{0} {1} {2}", userSession.CurrentTutor.Nombre, userSession.CurrentTutor.PrimerApellido, userSession.CurrentTutor.SegundoApellido));

            List<string> tos = new List<string>();
            tos.Add(email);
            AlternateView texto = null;
            List<string> archivos = new List<string>();
            List<string> copias = new List<string>();
            try
            {
                correoCtrl.sendMessage(tos, "YOY - Notificación", cuerpo, texto, archivos, copias);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alert('¡La notificación ha sido enviada correctamente!'); window.location='" + location + "';", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alert('Lo sentimos el correo no pudo ser enviado, intentelo mas tarde.');", true);
                LoggerHlp.Default.Error(this, ex);
            }
        }

        private void EnviarCorreoPagoTarjeta(NotaCompra nota, string strWhoCall = "CreditCard")
        {
            CorreoCtrl correoCtrl = new CorreoCtrl();
            #region Variables
            string urllogo = ConfigurationManager.AppSettings["POVUrlImgLogo"];
            string urlimgbackground = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgBackground"];
            string urlimgface = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgFace"];
            string urlimgtwit = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgTwit"];
            string urlimglinked = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgLinked"];
            string urlimginstagram = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgInstagram"];
            
            
            const string titulo = "Pago de derecho";
            string dominio = String.Format("{0}://{1}:{2}/", HttpContext.Current.Request.Url.Scheme, HttpContext.Current.Request.Url.Host, HttpContext.Current.Request.Url.Port);
            string linkportal = ConfigurationManager.AppSettings["POVUrlPortalAspirante"];
            string location = ConfigurationManager.AppSettings["POVUrlTutorados"];
            #endregion

            string cuerpo = string.Empty;
            using (StreamReader reader = new StreamReader(Server.MapPath("~/Files/protocolo/EmailTemplatePagoTarjeta.html")))
            {
                cuerpo = reader.ReadToEnd();
            }
            cuerpo = cuerpo.Replace("{urllogo}", urllogo);
            cuerpo = cuerpo.Replace("{urlimgbackground}", urlimgbackground);
            cuerpo = cuerpo.Replace("{title}", titulo);
            cuerpo = cuerpo.Replace("{linkportal}", linkportal);
            cuerpo = cuerpo.Replace("{nombre}", string.Format("{0} {1} {2}", userSession.CurrentTutor.Nombre, userSession.CurrentTutor.PrimerApellido, userSession.CurrentTutor.SegundoApellido));
            if (strWhoCall == "MesesSinIntereses")
                cuerpo = cuerpo.Replace("{plazo}", "a " + ddlPlazo.SelectedValue.ToString().Trim() + " meses sin intereses ");
            else
                cuerpo = cuerpo.Replace("{plazo}", string.Empty);
            cuerpo = cuerpo.Replace("{cantidad}", nota.Cantidad.ToString().Trim());
            CostoProductoCtrl ctrl = new CostoProductoCtrl(null);
            CostoProducto costo = ctrl.Retrieve(new CostoProducto() { CostoProductoId = nota.CostoProductoID }, false).FirstOrDefault();
            cuerpo = cuerpo.Replace("{costo}", String.Format("{0:C}", costo.Precio));
            cuerpo = cuerpo.Replace("{total}", String.Format("{0:C}", (costo.Precio*nota.Cantidad)));
            string tutorados = "<ul>";
            foreach (Alumno al in Session_Alumnos_Seleccionados)
            {
                tutorados = tutorados + "<li>" + al.NombreCompletoAlumno + "</li>";
            }
            tutorados = tutorados + "</ul>";
            cuerpo = cuerpo.Replace("{tutorados}", tutorados);
            cuerpo = cuerpo.Replace("{urlimgface}", urlimgface);
            cuerpo = cuerpo.Replace("{urlimgtwit}", urlimgtwit);
            cuerpo = cuerpo.Replace("{urlimglinked}", urlimglinked);
            cuerpo = cuerpo.Replace("{urlimginstagram}", urlimginstagram);

            List<string> tos = new List<string>();
            tos.Add(userSession.CurrentTutor.CorreoElectronico);
            AlternateView texto = null;
            List<string> archivos = new List<string>();
            List<string> copias = new List<string>();
            try
            {
                correoCtrl.sendMessage(tos, "YOY - Nota de compra", cuerpo, texto, archivos, copias);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alert('¡La notificación ha sido enviada correctamente!'); window.location='" + location + "';", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alert('Lo sentimos el correo no pudo ser enviado, intentelo mas tarde.');", true);
                LoggerHlp.Default.Error(this, ex);
            }
        }

        private void EnviarCorreoPagoOXXO(NotaCompra nota)
        {
            CorreoCtrl correoCtrl = new CorreoCtrl();
            #region Variables
            string urllogo = ConfigurationManager.AppSettings["POVUrlImgLogo"];
            string urlimgbackground = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgBackground"];
            string urlimgface = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgFace"];
            string urlimgtwit = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgTwit"];
            string urlimglinked = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgLinked"];
            string urlimginstagram = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgInstagram"];
            const string titulo = "Pago de derecho";
            string dominio = String.Format("{0}://{1}:{2}/", HttpContext.Current.Request.Url.Scheme, HttpContext.Current.Request.Url.Host, HttpContext.Current.Request.Url.Port);
            string linkportal = ConfigurationManager.AppSettings["POVUrlPortalAspirante"];
            string location = ConfigurationManager.AppSettings["POVUrlTutorados"];
            #endregion

            string cuerpo = string.Empty;
            using (StreamReader reader = new StreamReader(Server.MapPath("~/Files/protocolo/EmailTemplatePagoOXXO.html")))
            {
                cuerpo = reader.ReadToEnd();
            }
            cuerpo = cuerpo.Replace("{urllogo}", urllogo);
            cuerpo = cuerpo.Replace("{urlimgbackground}", urlimgbackground);
            cuerpo = cuerpo.Replace("{title}", titulo);
            cuerpo = cuerpo.Replace("{linkportal}", linkportal);
            cuerpo = cuerpo.Replace("{nombre}", string.Format("{0} {1} {2}", userSession.CurrentTutor.Nombre, userSession.CurrentTutor.PrimerApellido, userSession.CurrentTutor.SegundoApellido));
            CostoProductoCtrl ctrl = new CostoProductoCtrl(null);
            CostoProducto costo = ctrl.Retrieve(new CostoProducto() { CostoProductoId = nota.CostoProductoID }, false).FirstOrDefault();
            cuerpo = cuerpo.Replace("{cantidad}", String.Format("{0:C}", (costo.Precio * nota.Cantidad)));
            cuerpo = cuerpo.Replace("{referencia}", txtReferenciaOXXO.Text);
            cuerpo = cuerpo.Replace("{urlimgface}", urlimgface);
            cuerpo = cuerpo.Replace("{urlimgtwit}", urlimgtwit);
            cuerpo = cuerpo.Replace("{urlimglinked}", urlimglinked);
            cuerpo = cuerpo.Replace("{urlimginstagram}", urlimginstagram);

            List<string> tos = new List<string>();
            tos.Add(userSession.CurrentTutor.CorreoElectronico);
            AlternateView texto = null;
            List<string> archivos = new List<string>();
            List<string> copias = new List<string>();
            try
            {
                correoCtrl.sendMessage(tos, "YOY - Nota de compra", cuerpo, texto, archivos, copias);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alert('Lo sentimos el correo no pudo ser enviado, intentelo mas tarde.');", true);
                LoggerHlp.Default.Error(this, ex);
            }
        }
        protected void gvPlanesComprados_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName.Trim())
            {
                case "Asignar":
                    {
                        try
                        {
                            #region Asignando paquete premium comprado al alumno
                            var compraPremiumAsignada = compraPremiumCtrl.Retrieve(new CompraPremium() { CompraPremiumID = int.Parse(e.CommandArgument.ToString()) }, true).FirstOrDefault();
                            compraPremiumAsignada.AlumnoID = Session_AlumnoAsignado.AlumnoID;
                            compraPremiumCtrl.Update(compraPremiumAsignada);
                            #endregion

                            #region Asignar Orientador y activando al alumno como premium
                            EFAlumnoCtrl efAlumnoCtrl = new EFAlumnoCtrl(null);
                            var alumno = efAlumnoCtrl.Retrieve(Session_AlumnoAsignado, true).FirstOrDefault();
                            DocenteCtrl docenteCtrl = new DocenteCtrl();
                            var docente = docenteCtrl.GetDocenteMenosCarga(ConnectionHlp.Default.Connection, new Docente() { Estatus = true });

                            //Actualiza 
                            alumno.Premium = true;
                            efAlumnoCtrl.Update(alumno);
                            #endregion

                            //Vicula Orientador-Alumno
                            Usuario usuarioDocente = licenciaEscuelaCtrl.RetrieveUsuarios(ConnectionHlp.Default.Connection, docente).Where(x => x.UniversidadId == null).FirstOrDefault();
                            UsuarioExpediente usuarioExpediente = new UsuarioExpediente();
                            usuarioExpediente.AlumnoID = alumno.AlumnoID;
                            usuarioExpediente.UsuarioID = usuarioDocente.UsuarioID;
                            usuarioExpedienteCtrl.Insert(ConnectionHlp.Default.Connection, usuarioExpediente);

                            #region Enviar Notificacion al aspirante de asignacion paquete y orientador
                            //Recuperar correo del alumno
                            Usuario usuarioAlumno = licenciaEscuelaCtrl.RetrieveUsuario(dctx, alumno);
                            usuarioAlumno = new UsuarioCtrl().RetrieveComplete(ConnectionHlp.Default.Connection, usuarioAlumno);
                            //Enviar notificacion
                            enviarNotificacion(usuarioAlumno.Email, alumno, null);
                            #endregion

                        }
                        catch (Exception ex)
                        {
                            this.ShowMessage(ex.Message, MessageType.Error);
                        }
                        break;
                    }
                default: { ShowMessage("Comando no encontrado", MessageType.Error); break; }

            }
        }

        #region *****Message  Showing*****
        /// <summary>
        /// Desplega el mensaje de error/advertencia/información en la UI
        /// </summary>
        /// <param name="message">Mensaje a desplegar</param>
        /// <param name="messageType">Tipo de mensaje</param>
        private void ShowMessage(string message, MessageType messageType)
        {
            string type = string.Empty;

            switch (messageType)
            {
                case MessageType.Error:
                    type = "1";
                    break;
                case MessageType.Information:
                    type = "3";
                    break;
                case MessageType.Warning:
                    type = "2";
                    break;
            }

            ShowMessage(message, type);
        }
        /// <summary>
        /// Desplega el mensaje de error/advertencia/información en la UI
        /// </summary>
        /// <param name="message">Mensaje a desplegar</param>
        /// <param name="typeNotification">1: Error, 2: Advertencia, 3: Información</param>
        private void ShowMessage(string message, string typeNotification)
        {
            //Se ubican los controles que manejan el desplegado de error/advertencia/información
            if (Page.Master == null) return;
            Control m = Page.Master.FindControl("hdnLastMessage");
            Control t = Page.Master.FindControl("hdnShowMessage");

            if (m == null)
                throw new Exception("No se pudo desplegar correctamente el error.\nNo se encontró el control 'hdnLastMessage' en la MasterPage.\nEl error original es:\n" + message);
            if (t == null)
                throw new Exception("No se pudo desplegar correctamente el error.\nNo se encontró el control 'hdnShowMessage' en la MasterPage.\nEl error original es:\n" + message);

            if (m.GetType() != typeof(HiddenField) || t.GetType() != typeof(HiddenField))
                throw new Exception("No se pudo desplegar correctamente el error.\nAlguno de los controles de la MasterPage para el manejo de errores no es HiddenField.\nEl error original es:\n" + message);

            //Si el HiddenField del mensaje de error ya tiene un mensaje guardado, se da un 'enter' y se concatena el nuevo mensaje (errores acumulados)
            //En caso contrario, se pone el encabezado y se concatena el nuevo mensaje
            if (((HiddenField)m).Value != null && ((HiddenField)m).Value.Trim().CompareTo("") != 0)
                ((HiddenField)m).Value += "<br />";


            ((HiddenField)m).Value += message.Replace("\n", "<br />");
            ((HiddenField)t).Value = typeNotification;
        }
        #endregion

        protected Customer CreaCustomer (string token){
            Customer customer = new Customer();
            try
            {
                 customer.create(@"{
                    ""name"":""Fulanito Pérez"",
                    ""email"":""fulanito@conekta.com"",
                    ""phone"":""+521818181818"",
                    ""payment_sources"":[{
                      ""type"": ""card"",
                      ""token_id"":""tok_test_visa_4242""
                    }]
                  }");
            }
            catch (ConektaException e)
            {
                customer = null;
                foreach (JObject obj in e.details)
                {
                    System.Console.WriteLine("\n [ERROR]:\n");
                    System.Console.WriteLine("message:\t" + obj.GetValue("message"));
                    System.Console.WriteLine("debug:\t" + obj.GetValue("debug_message"));
                    System.Console.WriteLine("code:\t" + obj.GetValue("code"));
                }
            }
            return customer;
        }

        #region Pagos en una sola exhibicion con tarjeta de Credito/Debito
        protected void btnPagoTarjetas_Click(object sender, EventArgs e)
        {
            try
            {

                Session_NotaCompra = ProcesarPagoSeleccionado("CreditoDebito");

                ddlVenceAnioCC.SelectedIndex = 0;

                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "viewModal(1);", true);

            }
            catch (Exception ex)
            {
                this.ShowMessage(ex.Message, MessageType.Error);
            }
        }

        protected void btnPagarCC_Click(object sender, EventArgs e)
        {
            Order ord = new Order();
            try
            {
                
                ord = PagoCreditCard();
                if (ord != null)
                {
                    DoInsertNotaCompra(Session_NotaCompra);
                    DoUpdateAlumnos(Session_Alumnos_Seleccionados, EEstadoPago.PAGADO, true);
                    ConsultarAlumnoUsuario(userSession.CurrentTutor);
                    EnviarCorreoPagoTarjeta(Session_NotaCompra);
                }                
            }
            catch (Exception ex)
            {
                this.ShowMessage(ex.Message, MessageType.Error);
            }
        }

        protected Order PagoCreditCard()
        {
            string nombreCompleto = (userSession.CurrentTutor.Nombre + " " + userSession.CurrentTutor.PrimerApellido + " " + userSession.CurrentTutor.SegundoApellido).Trim();
            string telefono = userSession.CurrentUser.TelefonoReferencia.Trim();
            string correo = userSession.CurrentTutor.CorreoElectronico.Trim();

            Order order = new Order();
            try
            {
                string strJSONOrder = @"{
	                ""currency"":""MXN"",
				    ""customer_info"": {
					    ""name"": ""[Nombre]"",
					    ""phone"": ""[Telefono]"",
					    ""email"": ""[EMail]""
				    },
	                ""line_items"": [{
	                  ""name"": ""Pago de derecho para acceso a YOY"",
	                  ""unit_price"": [Costo],
	                  ""quantity"": [Cantidad]
	                }]
			    }";

                strJSONOrder = strJSONOrder.Replace("[Nombre]", nombreCompleto);
                strJSONOrder = strJSONOrder.Replace("[Telefono]", telefono);
                strJSONOrder = strJSONOrder.Replace("[EMail]", correo);
                strJSONOrder = strJSONOrder.Replace("[Costo]", (double.Parse(txtCostoUnitario.Text.Replace("$", "").Replace(",", "")) * 100).ToString());
                strJSONOrder = strJSONOrder.Replace("[Cantidad]", txtAlumnosSeleccionados.Text);
                order = new conekta.Order().create(strJSONOrder);

                string strJSONCharge = @"{
				    ""payment_method"": {
					    ""type"": ""card"",
					    ""token_id"": ""[Token]""
				    },
				    ""amount"": [Total]
			    }";
                strJSONCharge = strJSONCharge.Replace("[Total]", (double.Parse(txtTotalPagar.Text.Replace("$", "").Replace(",", "")) * 100).ToString());
                strJSONCharge = strJSONCharge.Replace("[Token]", hdnToken.Value.ToString());
                
                order.createCharge(strJSONCharge);


            }
            catch (ConektaException e)
            {
                order = null;
                foreach (JObject obj in e.details)
                {
                    System.Console.WriteLine("\n [ERROR]:\n");
                    System.Console.WriteLine("message:\t" + obj.GetValue("message"));
                    System.Console.WriteLine("debug:\t" + obj.GetValue("debug_message"));
                    System.Console.WriteLine("code:\t" + obj.GetValue("code"));
                    throw new Exception(obj.GetValue("message").ToString());
                }
            } 
            return order;
        }
        #endregion

        #region Pagos en efectivo con OXXO
        protected void btnPagoOxxo_Click(object sender, EventArgs e)
        {
            Order ord = new Order();
            try
            {

                Session_NotaCompra = ProcesarPagoSeleccionado("OXXO");

                ord = PagoOXXO();
                if (ord != null)
                {
                    txtReferenciaOXXO.Text = ((dynamic)((Newtonsoft.Json.Linq.JObject)(ord.charges.data[0]))).payment_method.reference;
                    hdnIDReferenciaOXXO.Value = ((dynamic)((Newtonsoft.Json.Linq.JObject)(ord.charges.data[0]))).id;
                    DoInsertNotaCompra(Session_NotaCompra);
                    DoUpdateAlumnos(Session_Alumnos_Seleccionados, EEstadoPago.PENDIENTE, false);
                    EnviarCorreoPagoOXXO(Session_NotaCompra);
                    ConsultarAlumnoUsuario(userSession.CurrentTutor);
                }


                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "viewModal(2);", true);

            }
            catch (Exception ex)
            {
                this.ShowMessage(ex.Message, MessageType.Error);
            }
        }

        protected Order PagoOXXO()
        {
            string nombreCompleto = (userSession.CurrentTutor.Nombre + " " + userSession.CurrentTutor.PrimerApellido + " " + userSession.CurrentTutor.SegundoApellido).Trim();
            string telefono = userSession.CurrentUser.TelefonoReferencia.Trim();
            string correo = userSession.CurrentTutor.CorreoElectronico.Trim();

            Order order = new Order();
            try
            {
                string strJSON = @"{
	                ""currency"":""MXN"",
				    ""customer_info"": {
					    ""name"": ""[Nombre]"",
					    ""phone"": ""[Telefono]"",
					    ""email"": ""[EMail]""
				    },
	                ""line_items"": [{
	                    ""name"": ""[Pago de derecho de acceso a YOY]"",
	                    ""unit_price"": [Costo],
	                    ""quantity"": [Cantidad]
	                }],
				    ""charges"": [{
					    ""payment_method"": {
						    ""type"": ""oxxo_cash""
					    },
					    ""amount"": [Total]
				    }]
	            }";
                strJSON = strJSON.Replace("[Nombre]", nombreCompleto);
                strJSON = strJSON.Replace("[Telefono]", telefono);
                strJSON = strJSON.Replace("[EMail]", correo); 
                strJSON = strJSON.Replace("[Costo]", (double.Parse(txtCostoUnitarioOXXO.Text.Replace("$", "").Replace(",", "")) * 100).ToString());
                strJSON = strJSON.Replace("[Cantidad]", txtCantidadOXXO.Text);
                strJSON = strJSON.Replace("[Total]", (double.Parse(txtTotalPagaOXXO.Text.Replace("$", "").Replace(",", "")) * 100).ToString());

                order = new conekta.Order().create(strJSON);
            }
            catch (ConektaException e)
            {
                order = null;
                foreach (JObject obj in e.details)
                {
                    System.Console.WriteLine("\n [ERROR]:\n");
                    System.Console.WriteLine("message:\t" + obj.GetValue("message"));
                    System.Console.WriteLine("debug:\t" + obj.GetValue("debug_message"));
                    System.Console.WriteLine("code:\t" + obj.GetValue("code"));
                    throw new Exception(obj.GetValue("message").ToString());
                }
            }
            return order;
        }
        #endregion

        #region Pago a Meses sin intereses con tarjeta de credito
        protected void btnPagoMeses_Click(object sender, EventArgs e)
        {
            try
            {
                Session_NotaCompra = ProcesarPagoSeleccionado("MesesSinIntereses");

                ddlVenceAnioMSS.SelectedIndex = 0;

                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "viewModal(3);", true);
            }
            catch (Exception ex)
            {
                this.ShowMessage(ex.Message, MessageType.Error);
            }

        }

        protected void btnPagarMSS_Click(object sender, EventArgs e)
        {
            Order ord = new Order();
            try
            {
                ord = PagoMesesSinIntereses();
                if (ord != null)
                {
                    DoInsertNotaCompra(Session_NotaCompra);
                    DoUpdateAlumnos(Session_Alumnos_Seleccionados, EEstadoPago.PAGADO, true);
                    ConsultarAlumnoUsuario(userSession.CurrentTutor);
                    EnviarCorreoPagoTarjeta(Session_NotaCompra, "MesesSinIntereses");
                }
            }
            catch (Exception ex)
            {
                this.ShowMessage(ex.Message, MessageType.Error);
            }
        }

        protected Order PagoMesesSinIntereses()
        {
            string nombreCompleto = (userSession.CurrentTutor.Nombre + " " + userSession.CurrentTutor.PrimerApellido + " " + userSession.CurrentTutor.SegundoApellido).Trim();
            string telefono = userSession.CurrentUser.TelefonoReferencia.Trim();
            string correo = userSession.CurrentTutor.CorreoElectronico.Trim();

            Order order = new Order();
            try
            {
                string strJSONOrder = @"{
	                ""currency"":""MXN"",
				    ""customer_info"": {
					    ""name"": ""[Nombre]"",
					    ""phone"": ""[Telefono]"",
					    ""email"": ""[EMail]""
				    },
	                ""line_items"": [{
	                  ""name"": ""Pago de derecho para acceso a YOY"",
	                  ""unit_price"": [Costo],
	                  ""quantity"": [Cantidad]
	                }]
			    }";

                strJSONOrder = strJSONOrder.Replace("[Nombre]", nombreCompleto);
                strJSONOrder = strJSONOrder.Replace("[Telefono]", telefono);
                strJSONOrder = strJSONOrder.Replace("[EMail]", correo);
                strJSONOrder = strJSONOrder.Replace("[Costo]", (double.Parse(txtCostoUnitarioMSS.Text.Replace("$", "").Replace(",", "")) * 100).ToString());
                strJSONOrder = strJSONOrder.Replace("[Cantidad]", txtCantidaMSS.Text);
                order = new conekta.Order().create(strJSONOrder);

                string strJSONCharge = @"{
				    ""payment_method"": {
					    ""type"": ""card"",
					    ""token_id"": ""[Token]"",
					    ""monthly_installments"": ""[Plazo]""
				    },
				    ""amount"": [Total]
			    }";
                strJSONCharge = strJSONCharge.Replace("[Total]", (double.Parse(txtTotalPagoMSS.Text.Replace("$", "").Replace(",", "")) * 100).ToString());
                strJSONCharge = strJSONCharge.Replace("[Token]", hdnToken.Value.ToString());
                strJSONCharge = strJSONCharge.Replace("[Plazo]", ddlPlazo.SelectedValue.ToString().Trim());
                order.createCharge(strJSONCharge);


            }
            catch (ConektaException e)
            {
                order = null;
                foreach (JObject obj in e.details)
                {
                    System.Console.WriteLine("\n [ERROR]:\n");
                    System.Console.WriteLine("message:\t" + obj.GetValue("message"));
                    System.Console.WriteLine("debug:\t" + obj.GetValue("debug_message"));
                    System.Console.WriteLine("code:\t" + obj.GetValue("code"));
                    throw new Exception(obj.GetValue("message").ToString());
                }
            }
            return order;
        }
        #endregion

        protected NotaCompra ProcesarPagoSeleccionado(string strWhoCall)
        {
            int countCheckeds = 0;
            Alumno alumno = new Alumno();
            List<Alumno> lstAlumno = new List<Alumno>();
            Session_Alumnos_Seleccionados = new List<Alumno>();
            foreach (GridViewRow row in gvTutorados.Rows)
            {
                CheckBox check = row.FindControl("chkSeleccionado") as CheckBox;

                if (check.Checked)
                {
                    int alumnoID = Convert.ToInt16(row.Cells[0].Text);
                    countCheckeds++;
                    List<TutorAlumno> tutAlumno= Session_Alumnos.Where(x => x.Alumno.AlumnoID == alumnoID & x.Alumno.EstatusPago == EEstadoPago.NO_PAGADO).ToList();
                    if (tutAlumno.Count != 0){
                        Session_Alumnos_Seleccionados.Add(tutAlumno.FirstOrDefault().Alumno);
                    }
                }
            }

            ProductoCosteoCtrl prod = new ProductoCosteoCtrl(null);
            List<ProductoCosteo> lstProductoCosteo = (prod.RetrieveWithRelationship(new ProductoCosteo(), false)).ToList();

            CostoProducto miProducto = new CostoProducto();

            if (Session_Alumnos_Seleccionados.Count() == 0)
            {
                throw new Exception("No hay alumnos seleccionados");
            }
            if (Session_Alumnos_Seleccionados.Count() == 1)
            {
                miProducto = lstProductoCosteo.Where(x => x.Nombre == "Paquete 1").FirstOrDefault().CostoProducto.Where(y => y.FechaFin == null).FirstOrDefault();
            }

            if (Session_Alumnos_Seleccionados.Count() > 1)
            {
                miProducto = lstProductoCosteo.Where(x => x.Nombre == "Paquete 2").FirstOrDefault().CostoProducto.Where(y => y.FechaFin == null).FirstOrDefault();
            }

            switch (strWhoCall)
            {
                case "CreditoDebito":
                    txtAlumnosSeleccionados.Text = Session_Alumnos_Seleccionados.Count().ToString();
                    txtCostoUnitario.Text = String.Format("{0:C}", miProducto.Precio);
                    txtTotalPagar.Text = String.Format("{0:C}", (Session_Alumnos_Seleccionados.Count() * miProducto.Precio));
                    break;
                case "OXXO":
                    txtCantidadOXXO.Text = Session_Alumnos_Seleccionados.Count().ToString();
                    txtCostoUnitarioOXXO.Text = String.Format("{0:C}", miProducto.Precio);
                    txtTotalPagaOXXO.Text = String.Format("{0:C}", (Session_Alumnos_Seleccionados.Count() * miProducto.Precio));
                    break;
                case "MesesSinIntereses":
                    txtCantidaMSS.Text = Session_Alumnos_Seleccionados.Count().ToString();
                    txtCostoUnitarioMSS.Text = String.Format("{0:C}", miProducto.Precio);
                    txtTotalPagoMSS.Text = String.Format("{0:C}", (Session_Alumnos_Seleccionados.Count() * miProducto.Precio));
                    break;
            }

            NotaCompra notaCompra = new NotaCompra()
            {
                TutorID = userSession.CurrentTutor.TutorID,
                FechaCompra = DateTime.Now,
                CostoProductoID = miProducto.CostoProductoId,
                Cantidad = Session_Alumnos_Seleccionados.Count(),
                ConceptoCompra = "Pago de derecho para acceso a YOY"
            };
            
            return notaCompra;
        }

        protected void DoInsertNotaCompra(NotaCompra notaCompra)
        {
            NotaCompraCtrl notaCompraCtrl = new NotaCompraCtrl(null);
            notaCompraCtrl.Insert(notaCompra);
        }

        protected void DoUpdateAlumnos(List<Alumno> alumnos, EEstadoPago EstatusPago, bool notificacionPago){
            EFAlumnoCtrl efAlumnoCtrl = new EFAlumnoCtrl(null);

            foreach (Alumno alumno in alumnos)
            {
                Alumno alumnoUpd = efAlumnoCtrl.Retrieve(alumno, true).FirstOrDefault();
                alumnoUpd.EstatusPago = EstatusPago;
                if (EstatusPago == EEstadoPago.PENDIENTE)
                {
                    alumnoUpd.ReferenciaOXXO = txtReferenciaOXXO.Text;
                    alumnoUpd.IDReferenciaOXXO = hdnIDReferenciaOXXO.Value.ToString();
                }
                efAlumnoCtrl.Update(alumnoUpd);
            }

            TutorCtrl tutorCtrl = new TutorCtrl(null);
            Tutor tutor = tutorCtrl.Retrieve(userSession.CurrentTutor, true).FirstOrDefault();
            tutor.NotificacionPago = notificacionPago;
            tutorCtrl.Update(tutor);
        }
    }
}