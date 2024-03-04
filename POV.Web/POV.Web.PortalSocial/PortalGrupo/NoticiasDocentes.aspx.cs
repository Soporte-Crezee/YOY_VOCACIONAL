using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using POV.Core.RedSocial.Interfaces;
using POV.Core.RedSocial.Implement;
using GP.SocialEngine.BO;
using GP.SocialEngine.Service;
using Framework.Base.DataAccess;
using POV.Web.PortalSocial.AppCode;
using System.Data;
using POV.CentroEducativo.Services;
using POV.CentroEducativo.BO;

namespace POV.Web.PortalSocial.PortalGrupo
{
    public partial class NoticiasDocentes : System.Web.UI.Page
    {
        private IUserSession userSession;
        private IRedirector redirector;
        private readonly IDataContext dctx = ConnectionHelper.Default.Connection;

        public NoticiasDocentes()
        {
            userSession = new UserSession();
            redirector = new Redirector();

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (userSession.IsLogin() && userSession.IsAlumno())
            {
                EFAlumnoCtrl alumnoCtrl = new EFAlumnoCtrl(null);
                Alumno alumno = alumnoCtrl.Retrieve(new Alumno { AlumnoID = userSession.CurrentAlumno.AlumnoID }, false).FirstOrDefault();
                if ((bool)alumno.CorreoConfirmado)
                {

                    this.hdnSessionSocialHubID.Value = userSession.SocialHub.SocialHubID.ToString();
                    this.hdnSessionUsuarioSocialID.Value = userSession.CurrentUsuarioSocial.UsuarioSocialID.ToString();
                    hdnSocialHubID.Value = userSession.SocialHub.SocialHubID.ToString();
                    hdnUsuarioSocialID.Value = userSession.CurrentUsuarioSocial.UsuarioSocialID.ToString();
                    hdnTipoPublicacionSuscripcionReactivo.Value = ((short)ETipoPublicacion.REACTIVO).ToString();
                    hdnTipoPublicacionTexto.Value = ((short)ETipoPublicacion.TEXTO).ToString();
                }
                else
                    redirector.GoToHomeAlumno(true);
            }
            else
                redirector.GoToLoginPage(true);
        }
    }
}