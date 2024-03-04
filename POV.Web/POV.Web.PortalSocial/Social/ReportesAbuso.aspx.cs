using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Framework.Base.DataAccess;
using POV.Core.RedSocial.Implement;
using POV.Core.RedSocial.Interfaces;

namespace POV.Web.PortalSocial.Social
{
    public partial class ReportesAbuso : System.Web.UI.Page
    {
        private IUserSession userSession;
        private IRedirector redirector;

        public ReportesAbuso()
        {
            userSession = new UserSession();
            redirector = new Redirector();
            

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (userSession.CurrentUser != null && userSession.IsLogin())
                {
                    if(userSession.IsAlumno())
                        redirector.GoToNotFound(true);

                    if (!Page.IsPostBack)
                    {

                    }
                }
                else
                {
                 redirector.GoToLoginPage(true);   
                }

            }
            catch (Exception ex)
            {
                POV.Logger.Service.LoggerHlp.Default.Error(this,ex);
                redirector.GoToHomeDocente(true);
            }
        }
    }
}