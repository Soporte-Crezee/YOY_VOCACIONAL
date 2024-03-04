using Framework.Base.DataAccess;
using POV.CentroEducativo.BO;
using POV.CentroEducativo.Services;
using POV.Core.RedSocial.Implement;
using POV.Core.RedSocial.Interfaces;
using POV.Seguridad.Service;
using POV.Web.PortalSocial.AppCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace POV.Web.PortalSocial.CuentaUsuario
{
    public partial class ActivacionUsuario : System.Web.UI.Page
    {
        private IUserSession userSession;
        private IRedirector redirector;
        private readonly IDataContext dctx = ConnectionHelper.Default.Connection;
        private UsuarioCtrl usuarioCtrl;
        private AccountService accountService;

        public ActivacionUsuario()
        {
            userSession = new UserSession();
            redirector = new Redirector();
            accountService = new AccountService();
            usuarioCtrl = new UsuarioCtrl();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (userSession.CurrentUser != null && userSession.LoggedIn && userSession.CurrentAlumno.NivelEscolar == ENivelEscolar.Superior)
                {
                    //Usuario usuario = usuarioCtrl.RetrieveComplete(dctx, new Usuario { UsuarioID = userSession.CurrentUser.UsuarioID });
                    EFAlumnoCtrl efAlumnoCtrl = new EFAlumnoCtrl(null);
                    Alumno alumno = efAlumnoCtrl.Retrieve(userSession.CurrentAlumno, false).FirstOrDefault();
                    if ((bool)alumno.EstatusPago)
                    {
                        Response.Write("<script>window.location='..Auth/ValidarDiagnostica.aspx';</script>");
                    }
                }
                else
                {
                    redirector.GoToLoginPage(true);
                }
            }
        }
    }
}