using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Framework.Base.DataAccess;
using POV.CentroEducativo.BO;
using POV.ConfiguracionesPlataforma.BO;
using POV.Core.RedSocial.Implement;
using POV.Core.RedSocial.Interfaces;
using POV.Licencias.Service;
using POV.Web.Controllers.ServiciosPlataforma.Controllers;
using POV.Web.PortalSocial.AppCode;

namespace POV.Web.PortalSocial.PortalAlumno
{
    public partial class PortalAlumno : System.Web.UI.MasterPage
    {
        private IUserSession userSession;
        private IRedirector redirector;
		private readonly IDataContext dctx = ConnectionHelper.Default.Connection;
        private Alumno alumno;
        private CicloEscolar cicloEscolar;
        private ConfiguracionGeneral configuracionGeneral;

        public String divCssClass
        {
            get { return center_panel.Attributes["class"]; }
            set { center_panel.Attributes["class"] = value; }
        }

        public PortalAlumno()
        {
            userSession = new UserSession();
            redirector = new Redirector();
            alumno = new Alumno();
            cicloEscolar = new CicloEscolar();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (userSession.CurrentEscuela != null && userSession.IsLogin())//
                {
                    if (userSession.IsAlumno())
                    {
                        getInfoAlumnoSession();

                        //Add LA:
                        string hrefFavoritos = System.Configuration.ConfigurationManager.AppSettings["POVUrlHrefFavoritos"];
                        string rutaFavoritos = UrlPostsFavoritosList.HRef;
                        UrlPostsFavoritosList.HRef = rutaFavoritos + hrefFavoritos;
                    }
                }
            }
        }

        private void getInfoAlumnoSession()
        {
            alumno = new Alumno
            {
                AlumnoID = userSession.CurrentAlumno.AlumnoID
            };

            cicloEscolar = new CicloEscolar
            {
                CicloEscolarID = userSession.CurrentCicloEscolar.CicloEscolarID
            };
        }

        public void LoadControlsAlumnoMaster(long usocialID, bool esVisitante)
        {
            //AN4 -Validación Si el usuario tiene configurado el modulo de talentos para su escuela y contrato
            if (!esVisitante)
            {
                this.HplNoticias.NavigateUrl = UrlHelper.GetAlumnoNoticiasURL();
            }
            else
            {
                this.HplNoticias.Visible = false;
            }
            this.HplMuro.NavigateUrl = UrlHelper.GetAlumnoMuroURL(usocialID);
            this.HplPerfil.NavigateUrl = UrlHelper.GetPerfilURL(usocialID);

            this.ImgUser.Src = UrlHelper.GetImagenPerfilURL("normal", usocialID);
        }
    }

}
