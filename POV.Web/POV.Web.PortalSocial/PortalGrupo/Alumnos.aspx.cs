using POV.CentroEducativo.BO;
using POV.CentroEducativo.Services;
using POV.Core.RedSocial.Implement;
using POV.Core.RedSocial.Interfaces;
using POV.Logger.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace POV.Web.PortalSocial.PortalGrupo
{
    public partial class Alumnos : System.Web.UI.Page
    {
        private IUserSession userSession;
        private IRedirector redirector;

        public Alumnos() 
        {
            userSession = new UserSession();
            redirector = new Redirector();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    if (userSession.IsLogin())
                    {
                        if (userSession.IsAlumno())
                        {
                            EFAlumnoCtrl alumnoCtrl = new EFAlumnoCtrl(null);
                            Alumno alumno = alumnoCtrl.Retrieve(new Alumno { AlumnoID = userSession.CurrentAlumno.AlumnoID }, false).FirstOrDefault();
                        
                            if (!(bool)alumno.CorreoConfirmado)
                                redirector.GoToHomeAlumno(false);
                        }
                    }
                    else
                    {
                        redirector.GoToLoginPage(false);
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerHlp.Default.Error(this, ex);
                throw ex;
            }
        }
    }
}