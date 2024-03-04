using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using POV.Core.RedSocial.Interfaces;
using POV.Core.RedSocial.Implement;
using GP.SocialEngine.BO;
using System.Data;
using Framework.Base.DataAccess;
using POV.Web.PortalSocial.AppCode;
using POV.CentroEducativo.BO;
using POV.CentroEducativo.Service;
using POV.CentroEducativo.DA;
using POV.Seguridad.BO;
using POV.Seguridad.Service;
using POV.Comun.BO;
using POV.Comun.Service;
using POV.Licencias.Service;
using POV.Licencias.BO;
using System.Net.Mail;
using POV.Logger.Service;
using System.IO;

namespace POV.Web.PortalSocial.CuentaUsuario
{
    public partial class ConfirmarMaestro : System.Web.UI.Page
    {
        private AccountService accountService;
        public IUserSession userSession = new UserSession();
        private readonly IDataContext dctx = ConnectionHelper.Default.Connection;
        private IRedirector redirector = new Redirector();
        EscuelaCtrl escuelaCtrl = new EscuelaCtrl();
        GrupoCicloEscolarCtrl grupoCicloEscolarCtrl = new GrupoCicloEscolarCtrl();
        GrupoCicloEscolar grupoCicloEscolar = new GrupoCicloEscolar();
        MateriaDocenteGrupoCicloEscolarDARetHlp materiaDocenteGrupoCicloEscolarDARetHlp = new MateriaDocenteGrupoCicloEscolarDARetHlp();

        private DocenteCtrl docenteCtrl;

        public ConfirmarMaestro()
        {
            docenteCtrl = new DocenteCtrl();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Form["__EVENTTARGET"] == "BtnEnviar_Click") BtnEnviar_Click(sender, e);
            if (!IsPostBack)
            {                
                if (userSession.CurrentUser != null && userSession.LoggedIn)
                {
                    Docente docente = docenteCtrl.LastDataRowToDocente(docenteCtrl.Retrieve(dctx, new Docente { DocenteID = userSession.CurrentDocente.DocenteID }));
                    if (!docente.EstatusIdentificacion.Value)
                    {
                        DatosPersonales();
                        List<EscuelaListItem> items = new List<EscuelaListItem>();
                        Dictionary<string, string> escuelas = new Dictionary<string, string>();
                        foreach (LicenciaEscuela licenciaEscuela in userSession.LicenciasDocente)
                        {
                            EscuelaListItem item = new EscuelaListItem();
                            Escuela escuela = escuelaCtrl.LastDataRowToEscuela(escuelaCtrl.Retrieve(dctx, licenciaEscuela.Escuela));
                            item.IDEscuela = escuela.EscuelaID;
                            item.Escuela = escuela.NombreEscuela;
                            item.Turno = escuela.Turno.ToString();
                            item.Clave = escuela.Clave;
                            item.LicenciaID = licenciaEscuela.LicenciaEscuelaID.ToString();
                            List<GrupoListItem> grupos = new List<GrupoListItem>();
                            grupos = CargarGruposPorEscuela(escuela);
                            item.Grupos = grupos;
                            items.Add(item);


                        }
                        this.rptEscuelas.DataSource = items;
                        this.rptEscuelas.DataBind();
                    }
                    else
                        redirector.GoToAceptarTerminos(true);
                }
                else
                {
                    redirector.GoToLoginPage(true);
                }
            }
        }
        protected void cargarDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Repeater rptGrupo = (Repeater)e.Item.FindControl("rptGrupos");
                EscuelaListItem EscuelaGrupo = (EscuelaListItem)e.Item.DataItem;
                rptGrupo.DataSource = EscuelaGrupo.Grupos;
                rptGrupo.DataBind();
            }
        }
        private List<GrupoListItem> CargarGruposPorEscuela(Escuela escuela)
        {
            GrupoCtrl grupoCtrl = new GrupoCtrl();
            grupoCicloEscolar.Escuela = new Escuela { EscuelaID = escuela.EscuelaID };
            DataSet dsGrupoCicloEscolar = grupoCicloEscolarCtrl.Retrieve(dctx, grupoCicloEscolar);
            List<GrupoListItem> grupos = new List<GrupoListItem>();
            foreach (DataRow row in dsGrupoCicloEscolar.Tables[0].Rows)//obtener los id de cicloescolar de la escuela
            {
                //obtener los id de ciclo escolar de la escuela y del maestro
                DataSet dsCiclosEscuelaMaestro = materiaDocenteGrupoCicloEscolarDARetHlp.Action(dctx, userSession.CurrentDocente, new GrupoCicloEscolar { GrupoCicloEscolarID = row.Field<Guid>("GrupoCicloEscolarID") });
                foreach (DataRow row2 in dsCiclosEscuelaMaestro.Tables[0].Rows)
                {
                    DataSet dsCicloGrupo = grupoCicloEscolarCtrl.Retrieve(dctx, new GrupoCicloEscolar { GrupoCicloEscolarID = row2.Field<Guid>("GrupoCicloEscolarID") });
                    foreach (DataRow row3 in dsCicloGrupo.Tables[0].Rows)
                    {
                        Grupo miGrupo = new Grupo();
                        miGrupo.GrupoID = row3.Field<Guid>("GrupoID");
                        miGrupo = grupoCtrl.LastDataRowToGrupo(grupoCtrl.Retrieve(dctx, miGrupo, escuela));
                        GrupoListItem grupo = new GrupoListItem();
                        grupo.Nombre = miGrupo.Nombre;
                        grupo.Grado = miGrupo.Grado.ToString();
                        if (!grupos.Any(x => x.Nombre == grupo.Nombre && x.Grado == grupo.Grado))
                            grupos.Add(grupo);

                    }
                }
            }
            return grupos;
        }


        private void DatosPersonales()
        {
            DateTime fechaNacimiento = (DateTime)userSession.CurrentDocente.FechaNacimiento;
            lblCurp.Text = userSession.CurrentDocente.Curp;
            lblNombre.Text = userSession.CurrentDocente.Nombre;
            lblApellidos.Text = userSession.CurrentDocente.PrimerApellido + " " + userSession.CurrentDocente.SegundoApellido;
            lblFechaNacimiento.Text = fechaNacimiento.ToString("dd/MM/yyyy");
            if ((bool)userSession.CurrentDocente.Sexo)
                lblGenero.Text = "Hombre";
            else
                lblGenero.Text = "Mujer";
            lblCorreo.Text = userSession.CurrentUser.Email;
            txtNombreUsuario.Text = userSession.CurrentUser.NombreUsuario;
            txtAntNombreUsuario.Text = userSession.CurrentUser.NombreUsuario;
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            Docente nuevoDocente = (Docente)userSession.CurrentDocente.Clone();
            nuevoDocente.EstatusIdentificacion = true;
            Usuario nuevoUsuario = (Usuario)userSession.CurrentUser.Clone();
            nuevoUsuario.NombreUsuario = txtNombreUsuario.Text;
            try
            {
                nuevoUsuario = UsuarioToObjeto(nuevoUsuario);
                UsuarioCtrl usuarioCtrl = new UsuarioCtrl();
                usuarioCtrl.Update(dctx, nuevoUsuario, userSession.CurrentUser);
                DocenteCtrl docenteCtrl = new DocenteCtrl();
                docenteCtrl.Update(dctx, nuevoDocente, userSession.CurrentDocente);
                if (nuevoUsuario.NombreUsuario != userSession.CurrentUser.NombreUsuario)
                {
                    if (nuevoUsuario.Email != null)
                    {
                        EnviarCorreo(nuevoUsuario);
                        Response.Write("<script>alert('Tu usuario ha sido actualizado.Te hemos enviado un correo con tu nuevo usuario.');window.location='AceptarTerminos.aspx';</script>");
                    }
                    else
                    {
                        Response.Write("<script>alert('Tu usuario ha sido actualizado.');window.location='AceptarTerminos.aspx';</script>");
                    }
                }
                else
                {
                    Response.Write("<script>window.location='AceptarTerminos.aspx';</script>");
                }
            }
            catch (Exception ex)
            {
                /*prueba*/
                if (ex.Source == "Usuario")
                    lblMensajeError.Text = "El usuario no esta disponible";
                if (ex.Source == "usuarioTamano")
                    lblMensajeError.Text = "El usuario tiene que ser mínimo de 6 caracteres y máximo de 50 caracteres";
                if (ex.Source == "whitespace")
                    lblMensajeError.Text = "El usuario no puede tener espacios en blanco.";
            }

        }

        private void EnviarCorreo(Usuario usuarioNuevo)
        {
            CorreoCtrl correoCtrl = new CorreoCtrl();
            #region variables
            string urllogo = ConfigurationManager.AppSettings["POVUrlImgLogo"];
            string urlimgbackground = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgBackground"];
            string urlimgface = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgFace"];
            string urlimgtwit = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgTwit"];
            string urlimglinked = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgLinked"];
            string urlimginstagram = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgInstagram"];
            const string titulo = "Nuevo usuario";
            const string imgalt = "YOY - SOCIAL";
            string linkportal = ConfigurationManager.AppSettings["POVUrlOrientador"];
            #endregion
            string cuerpo = string.Empty;

            using (StreamReader reader = new StreamReader(Server.MapPath("~/Files/protocolo/EmailTemplateNuevoUser.html")))
            {
                cuerpo = reader.ReadToEnd();
            }

            cuerpo = cuerpo.Replace("{urllogo}", urllogo);
            cuerpo = cuerpo.Replace("{urlimgbackground}", urlimgbackground);
            cuerpo = cuerpo.Replace("{title}", titulo);
            cuerpo = cuerpo.Replace("{linkportal}", linkportal);
            cuerpo = cuerpo.Replace("{nombreusuario}", usuarioNuevo.NombreUsuario);
            cuerpo = cuerpo.Replace("{urlimgface}", urlimgface);
            cuerpo = cuerpo.Replace("{urlimgtwit}", urlimgtwit);
            cuerpo = cuerpo.Replace("{urlimglinked}", urlimglinked);
            cuerpo = cuerpo.Replace("{urlimginstagram}", urlimginstagram);

            List<string> tos = new List<string>();
            tos.Add(usuarioNuevo.Email);
            AlternateView texto = null;
            List<string> archivos = new List<string>();
            List<string> copias = new List<string>();

            try
            {
                correoCtrl.sendMessage(tos, "Cambio de Nombre de Usuario", cuerpo, texto, archivos, copias);
            }
            catch (Exception ex)
            {
                ex.Source = "EnviarCorreo";
                Response.Write("<script>alert('Tu usuario ha sido actualizado. Por alguna razón no pudimos enviarte un correo con tu nuevo usuario, te recomendamos apuntarlo.');window.location='../Auth/ValidarDiagnostica.aspx';</script>");
            }

        }

        protected Usuario UsuarioToObjeto(Usuario usuarioNuevo)
        {
            if (txtNombreUsuario.Text.Trim().Contains(" "))
            {
                var ex = new Exception();
                ex.Source = "whitespace";
                throw (ex);
            }

            if (txtNombreUsuario.Text.Trim() == "" || (txtNombreUsuario.Text.Count() < 6 || txtNombreUsuario.Text.Count() > 50))
            {
                var ex = new Exception();
                ex.Source = "usuarioTamano";
                throw (ex);
            }
            else
                usuarioNuevo.NombreUsuario = txtNombreUsuario.Text.Trim();
            return usuarioNuevo;
        }

        protected void BtnEnviar_Click(object sender, EventArgs e)
        {
            try
            {
                string mensaje = txtDatosIncorrectos.Text.Trim();
                //Formato Incorrecto
                if (mensaje.Length > 0)
                {
                    enviarCorreo(mensaje);
                    accountService = new AccountService();
                    accountService.Logout();
                }
                else
                {
                    if (string.IsNullOrEmpty(mensaje))
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alert('El mensaje no debe estar vacio');", true);
                }
            }
            catch (Exception ex)
            {
                LoggerHlp.Default.Error(this, ex);
            }
        }

        //Enviar Correo de registro exitoso
        private void enviarCorreo(string mensaje)
        {
            CorreoCtrl correoCtrl = new CorreoCtrl();
            #region Variables
            string urlimg = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgEmail"]; ;
            string urlimgbackground = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgBackground"];
            string urlimgface = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgFace"];
            string urlimgtwit = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgTwit"];
            string urlimglinked = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgLinked"];
            string urlimginstagram = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgInstagram"];
            const string titulo = "Datos incorrectos de orientador";
            string dominio = String.Format("{0}://{1}:{2}/", HttpContext.Current.Request.Url.Scheme, HttpContext.Current.Request.Url.Host, HttpContext.Current.Request.Url.Port);
            string linkportal = ConfigurationManager.AppSettings["POVUrlOrientador"];
            string location = ConfigurationManager.AppSettings["POVUrlLocation"];
            #endregion

            string cuerpo = string.Empty;
            using (StreamReader reader = new StreamReader(Server.MapPath("~/Files/protocolo/EmailTemplateDatosIncorrectos.html")))
            {
                cuerpo = reader.ReadToEnd();
            }
            cuerpo = cuerpo.Replace("{urlimage}", urlimg);
            cuerpo = cuerpo.Replace("{urlimgbackground}", urlimgbackground);
            cuerpo = cuerpo.Replace("{title}", titulo);
            cuerpo = cuerpo.Replace("{linkportal}", linkportal);
            cuerpo = cuerpo.Replace("{tipopersona}", "orientador");
            cuerpo = cuerpo.Replace("{nombre}", userSession.CurrentDocente.NombreCompletoDocente);
            cuerpo = cuerpo.Replace("{mensaje}", mensaje);
            cuerpo = cuerpo.Replace("{urlimgface}", urlimgface);
            cuerpo = cuerpo.Replace("{urlimgtwit}", urlimgtwit);
            cuerpo = cuerpo.Replace("{urlimglinked}", urlimglinked);
            cuerpo = cuerpo.Replace("{urlimginstagram}", urlimginstagram);

            List<string> tos = new List<string>();
            tos.Add(ConfigurationManager.AppSettings["EmailSoporte"]);
            AlternateView texto = null;
            List<string> archivos = new List<string>();
            List<string> copias = new List<string>();
            try
            {
                correoCtrl.sendMessage(tos, "YOY - Confirmar datos", cuerpo, texto, archivos, copias);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alert('Lo sentimos el correo no pudo ser enviado, intentelo mas tarde.');", true);
                LoggerHlp.Default.Error(this, ex);
            }
        }
    }
}