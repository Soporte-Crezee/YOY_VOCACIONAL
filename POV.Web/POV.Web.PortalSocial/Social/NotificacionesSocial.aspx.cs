using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Framework.Base.DataAccess;
using GP.SocialEngine.BO;
using GP.SocialEngine.Service;
using POV.Core.RedSocial.Implement;
using POV.Core.RedSocial.Interfaces;
using POV.CentroEducativo.Services;
using POV.CentroEducativo.BO;

namespace POV.Web.PortalSocial.Social
{
    public partial class NotificacionesSocial : System.Web.UI.Page
    {

        private IUserSession userSession;
        private IRedirector redirector;
        readonly IDataContext dctx = new DataContext((new DataProviderFactory()).GetDataProvider("POV"));
        private NotificacionCtrl notificacionCtrl;

        public NotificacionesSocial()
        {
            notificacionCtrl= new NotificacionCtrl();
            userSession = new UserSession();
            redirector = new Redirector();
            
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (userSession != null && userSession.IsLogin())
                {
                    if (!Page.IsPostBack)
                    {
                        if (userSession.IsAlumno()) 
                        {
                            EFAlumnoCtrl alumnoCtrl = new EFAlumnoCtrl(null);
                            Alumno alumno = alumnoCtrl.Retrieve(new Alumno { AlumnoID = userSession.CurrentAlumno.AlumnoID }, false).FirstOrDefault();
                        
                            if (!(bool)alumno.CorreoConfirmado)
                                redirector.GoToHomeAlumno(true);
                        }
                    }
                    
                }
                else
                {
                    redirector.GoToLoginPage(true);
                }
            }
            catch
            {
                redirector.GoToHomePage(true);
            }

        }
    }
}