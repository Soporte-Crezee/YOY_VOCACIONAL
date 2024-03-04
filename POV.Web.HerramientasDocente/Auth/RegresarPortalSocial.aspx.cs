using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using POV.Core.HerramientasDocente.Implement;
using POV.Core.HerramientasDocente.Interfaces;
using POV.CentroEducativo.BO;
using POV.Seguridad.BO;

namespace POV.Web.HerramientasDocente.Auth
{
    public partial class RegresarPortalSocial : System.Web.UI.Page
    {
        private IUserSession userSession = new UserSession();
        private IRedirector redirector = new Redirector();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string urlPortal = @ConfigurationManager.AppSettings["POVUrlHerramientasDocente"];

                if (userSession.IsLogin())
                {
                    Docente docente = userSession.CurrentDocente;
                    string parametros = GenerarTokenYUrl(docente, userSession.CurrentUser);
                    userSession.Logout();
                    url.Value = urlPortal + parametros;
                }
                else
                {
                    url.Value = urlPortal;
                }
            }
        }

        private string GenerarTokenYUrl(Docente docente, Usuario usuario)
        {
            string strUrlAutoLogin;
            string formatoFecha = "yyyyMMddHHmmss.fff";
            DateTime fecha = System.DateTime.Now;
            string nombre = docente.Nombre;
            nombre = nombre.Trim();
            string apellido = docente.PrimerApellido;
            apellido = apellido.Trim();
            string curp = docente.Curp;
            DateTime fechaNacimiento = (DateTime)docente.FechaNacimiento;
            curp = curp.Trim();
            string token = fechaNacimiento.ToString(formatoFecha) + nombre + apellido + curp + fecha.ToString(formatoFecha);
            byte[] clave = POV.Seguridad.Utils.EncryptHash.SHA1encrypt(token);
            token = POV.Seguridad.Utils.EncryptHash.byteArrayToStringBase64(clave);
            token = System.Web.HttpUtility.UrlEncode(token);
            strUrlAutoLogin = "?cmd=return&docente=" + docente.Curp + "&identidad=" + usuario.UsuarioID + "&fechahora=" + fecha.ToString(formatoFecha) + "&token=" + token;
            return strUrlAutoLogin;
        }
    }
}