using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using System.Drawing;
using GP.SocialEngine.BO;
using GP.SocialEngine.Service;
using POV.Core.RedSocial.Interfaces;
using POV.Core.RedSocial.Implement;
using Framework.Base.DataAccess;
using POV.Seguridad.BO;
using POV.Seguridad.Service;
using POV.CentroEducativo.BO;
using POV.CentroEducativo.Service;
using POV.Localizacion.Service;
using POV.Comun.BO;
using POV.Comun.Service;
using POV.Web.PortalSocial.AppCode;
using POV.Logger.Service;
using System.Configuration;
using POV.Localizacion.BO;
using System.Globalization;
using POV.CentroEducativo.Services;
using POV.Modelo.Context;
using POV.Licencias.Service;

namespace POV.Web.PortalSocial.PortalGrupo
{
    public partial class OrientacionVocacional : System.Web.UI.Page
    {        
        private IUserSession userSession;
        private IRedirector redirector;
        private UsuarioCtrl usuarioCtrl;
        private AlumnoCtrl alumnoCtrl;
        private readonly IDataContext dctx = ConnectionHelper.Default.Connection;

        public OrientacionVocacional()
        {
            userSession = new UserSession();
            redirector = new Redirector();
            usuarioCtrl = new UsuarioCtrl();
            alumnoCtrl = new AlumnoCtrl();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (userSession.IsLogin())
                {
                    CargarDatos();
                    //SolicitudOrientacionVocacional solicitud = (SolicitudOrientacionVocacional)FindControl("solicitud");
                    //Button btnSolicitar = (Button)solicitud.FindControl("btnSolicitar");
                    //btnSolicitar.Text = "Solicitando";
                    //btnSolicitar.Visible = true;
                    //lblFecha = (Label)Solicitud.FindControl("lblfecha");
                    //txtNombreEvento = (TextBox)Solicitud.FindControl("txtNombreEvento");
                }
                else
                    redirector.GoToLoginPage(true);
            }
        }

        private void CargarDatos()
        { }

        protected void Solicitud_SolicitarClicked(object sender, EventArgs e) 
        {
            //if (this.Solicitud.Fecha != null/* && this.Solicitud.HInicio != null && this.Solicitud.HFin != null*/)
            //{
                //string ffff = Solicitud.Fecha.ToString();
                //Solicitud.SolicitarClicked(sender);
            //}
        }
    }
}