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

namespace POV.Web.PortalSocial.PortalDocente
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
                    if (!userSession.IsAlumno() && userSession.CurrentEscuela != null)
                    {
                        this.hdnSessionSocialHubID.Value = userSession.SocialHub.SocialHubID.ToString();
                        this.hdnSessionUsuarioSocialID.Value = userSession.CurrentUsuarioSocial.UsuarioSocialID.ToString();
                        hdnSocialHubID.Value = userSession.SocialHub.SocialHubID.ToString();
                        hdnUsuarioSocialID.Value = userSession.CurrentUsuarioSocial.UsuarioSocialID.ToString();
                        hdnTipoPublicacionSuscripcionReactivo.Value = ((short)ETipoPublicacion.REACTIVO).ToString();
                        hdnTipoPublicacionTexto.Value = ((short)ETipoPublicacion.TEXTO).ToString();

                        CargarDatos();
                    }
                    else
                        redirector.GoToHomePage(true);
                }
                else
                    redirector.GoToLoginPage(true);
            }
        }

        private void CargarDatos()
        {
        }
        
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            
        }

        protected void btnGuardarCambios_Click(object sender, EventArgs e)
        {
            
        }

        protected void grdEventsCalendar_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName.Trim())
            {
                case "VerDetalle": 
                    {
                        try
                        {                            
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "viewDetalleEvent();", true);
                        }
                        catch (Exception)
                        {
                            
                            throw;
                        }
                        break;
                    }
            }
        }
    }
}