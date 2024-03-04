using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using POV.Core.RedSocial.Implement;
using POV.Core.RedSocial.Interfaces;
using POV.Logger.Service;
using POV.CentroEducativo.Services;
using POV.CentroEducativo.BO;
namespace POV.Web.PortalSocial.Social
{
    public partial class NuevoMensaje : System.Web.UI.Page
    {
        
        private IUserSession userSession;
        private IRedirector redirector;

        public NuevoMensaje()
        {
            userSession = new UserSession();
            redirector = new Redirector();
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {
                if (userSession.IsLogin())
                {
                    EFAlumnoCtrl alumnoCtrl = new EFAlumnoCtrl(null);
                    Alumno alumno = alumnoCtrl.Retrieve(new Alumno { AlumnoID = userSession.CurrentAlumno.AlumnoID }, false).FirstOrDefault();
                        
                    if ((bool)alumno.CorreoConfirmado)
                    {
                        if (userSession.CurrentUsuarioSocial != null && userSession.CurrentGrupoSocial != null)
                        {

                        }
                        else
                        {
                            redirector.GoToHomePage(true);
                        }
                    }
                    else
                        redirector.GoToHomeAlumno(true);

                }
                else
                {
                    redirector.GoToLoginPage(true);
                }
            }
            catch (Exception ex)
            {
             LoggerHlp.Default.Error(this,ex);
                throw;
            }
        }
    }
}