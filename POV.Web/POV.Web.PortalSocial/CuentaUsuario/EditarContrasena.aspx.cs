using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using POV.Core.RedSocial.Interfaces;
using POV.Core.RedSocial.Implement;
using Framework.Base.DataAccess;
using POV.Web.PortalSocial.AppCode;
using POV.Seguridad.BO;
using POV.Seguridad.Service;
using POV.Seguridad.Utils;
using POV.CentroEducativo.Services;
using POV.CentroEducativo.BO;


namespace POV.Web.PortalSocial.CuentaUsuario
{
    public partial class EditarContrasena : System.Web.UI.Page
    {
        private IUserSession userSession;
        private AccountService accountService;
        private IRedirector redirector;
        private readonly IDataContext dctx = ConnectionHelper.Default.Connection;
        private const int MIN_CARACTARES = 6;

        public EditarContrasena()
        {
            userSession = new UserSession();
            accountService = new AccountService();
            redirector = new Redirector();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Page.Header.DataBind();

            if (!IsPostBack)
            {
                if (!userSession.IsLogin())
                {
                    redirector.GoToLoginPage(true);
                }
                else
                {
                    if (userSession.IsAlumno())
                    {
                        EFAlumnoCtrl alumnoCtrl = new EFAlumnoCtrl(null);
                        Alumno alumno = alumnoCtrl.Retrieve(new Alumno { AlumnoID = userSession.CurrentAlumno.AlumnoID }, false).FirstOrDefault();
                        if ((bool)alumno.CorreoConfirmado)
                        {
                            LblNombreUsuario.Text = userSession.CurrentUser.NombreUsuario;
                        }
                        else
                            redirector.GoToHomeAlumno(true);
                    }
                    else
                        LblNombreUsuario.Text = userSession.CurrentUser.NombreUsuario;

                }
            }
        }

        protected void GuardarBtn_OnClick(object sender, EventArgs e)
        {
            bool isValid = true;
            ActualPassTxt.CssClass = "form-control";
            NuevaPassTxt.CssClass = "form-control";
            RepNuevaPassTxt.CssClass = "form-control";
            NuevaPassErrorTxt.Text = "";
            ActualPassErrorTxt.Text = "";
            RepNuevaPassErrorTxt.Text = "";
            lblPassActual.CssClass = "col-sm-5 control-label";
            lblPassNueva.CssClass = "col-sm-5 control-label";
            lblPassNuevaRepetir.CssClass = "col-sm-5 control-label";

            if (string.IsNullOrEmpty(ActualPassTxt.Text.Trim()))
            {
                ActualPassTxt.CssClass = "form-control error";
                lblPassActual.CssClass = "col-sm-5 control-label error_label";
                ActualPassErrorTxt.Text = "Campo Requerido.";
                isValid = false;
            }
            if (isValid && ActualPassTxt.Text.Trim().Length < MIN_CARACTARES)
            {
                lblPassActual.CssClass = "col-sm-5 control-label error_label";
                ActualPassTxt.CssClass = "form-control error";
                ActualPassErrorTxt.Text = "Mínimo de carácteres " + MIN_CARACTARES;
                isValid = false;
            }
            if (isValid && string.IsNullOrEmpty(NuevaPassTxt.Text.Trim()))
            {
                lblPassNueva.CssClass = "col-sm-5 control-label error_label";
                NuevaPassTxt.CssClass = "form-control error";
                NuevaPassErrorTxt.Text = "Campo Requerido.";
                isValid = false;
            }
            if (isValid && NuevaPassTxt.Text.Trim().Length < MIN_CARACTARES)
            {
                lblPassNueva.CssClass = "col-sm-5 control-label error_label";
                NuevaPassTxt.CssClass = "form-control error";
                NuevaPassErrorTxt.Text = "Mínimo de carácteres " + MIN_CARACTARES;
                isValid = false;
            }
            if (isValid && string.IsNullOrEmpty(RepNuevaPassTxt.Text.Trim()))
            {
                lblPassNuevaRepetir.CssClass = "col-sm-5 control-label error_label";
                RepNuevaPassTxt.CssClass = "form-control error";
                RepNuevaPassErrorTxt.Text = "Campo Requerido.";
                isValid = false;
            }

            if (isValid && RepNuevaPassTxt.Text.Trim().CompareTo(NuevaPassTxt.Text.Trim()) != 0)
            {
                lblPassNuevaRepetir.CssClass = "col-sm-5 control-label error_label";
                RepNuevaPassTxt.CssClass = "form-control error";
                RepNuevaPassErrorTxt.Text = "Debe coincidir con la nueva contraseña.";
                isValid = false;
            }

            if (isValid)
            {
                UsuarioCtrl usuarioCtrl = new UsuarioCtrl();
                DataSet ds = usuarioCtrl.Retrieve(dctx, new Usuario { UsuarioID = userSession.CurrentUser.UsuarioID });

                Usuario usuario = usuarioCtrl.LastDataRowToUsuario(ds);

                Byte[] passwd = EncryptHash.SHA1encrypt(ActualPassTxt.Text);
                if (EncryptHash.compareByteArray(usuario.Password, passwd))
                {
                    Byte[] newpasswd = EncryptHash.SHA1encrypt(NuevaPassTxt.Text);
                    Usuario newUsuario = (Usuario)usuario.Clone();
                    newUsuario.Password = newpasswd;
                    usuarioCtrl.Update(dctx, newUsuario, usuario);
                    redirector.GoToHomePage(true);
                    return;
                }
                else
                {
                    lblPassActual.CssClass = "col-sm-5 control-label error_label";
                    ActualPassTxt.CssClass = "form-control error";
                    ActualPassErrorTxt.Text = "La contraseña no coincide con la registrada.";
                }
            }
        }

        protected void CancelBtn_OnClick(object sender, EventArgs e)
        {
            redirector.GoToHomePage(true);
            return;
        }
    }
}