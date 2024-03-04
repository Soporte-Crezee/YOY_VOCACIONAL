using POV.CentroEducativo.BO;
using POV.Core.RedSocial.Implement;
using POV.Core.RedSocial.Interfaces;
using POV.Expediente.Reports.Reports;
using POV.Seguridad.BO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace POV.Web.PortalSocial.PortalDocente.Reportes
{
    public partial class DetalleReporteResultado : System.Web.UI.Page
    {
        private Alumno SS_AlumnoCarga
        {
            get { return (Alumno)this.Session["AlumnoCarga"]; }
            set { this.Session["AlumnoCarga"] = value; }
        }

        private Usuario SS_UsuarioCarga
        {
            get { return (Usuario)this.Session["UsuarioCarga"]; }
            set { this.Session["UsuarioCarga"] = value; }
        }

        private decimal SS_RespuestaHabitos
        {
            get { return (decimal)this.Session["RespuestaHabitos"]; }
            set { this.Session["RespuestaHabitos"] = value; }
        }

        private decimal SS_RespuestaDominos
        {
            get { return (decimal)this.Session["RespuestaDominos"]; }
            set { this.Session["RespuestaDominos"] = value; }
        }

        private DataSet SS_RespuestaTerman
        {
            get { return (DataSet)this.Session["RespuestaTerman"]; }
            set { this.Session["RespuestaTerman"] = value; }
        }

        private Dictionary<string, string> SS_RespuestaKuder
        {
            get { return (Dictionary<string, string>)this.Session["RespuestaKuder"]; }
            set { this.Session["RespuestaKuder"] = value; }
        }

        private Dictionary<string, string> SS_RespuestaAllport
        {
            get { return (Dictionary<string, string>)this.Session["RespuestaAllport"]; }
            set { this.Session["RespuestaAllport"] = value; }
        }

        private IRedirector redirector;
        private IUserSession userSession;

        public DetalleReporteResultado() 
        {
            userSession = new UserSession();
            redirector = new Redirector();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!userSession.IsLogin())
                {
                    redirector.GoToLoginPage(true);
                }
                else
                {
                    FillBack();
                }
            }
            
            if (SS_AlumnoCarga == null || SS_UsuarioCarga == null)
            {
                redirector.GoToHomePage(false);
            }

            ExpedienteAlumnoRpt report = new ExpedienteAlumnoRpt(this.SS_AlumnoCarga, this.SS_UsuarioCarga, this.SS_RespuestaHabitos, this.SS_RespuestaDominos, this.SS_RespuestaTerman, this.SS_RespuestaKuder, this.SS_RespuestaAllport);

            rptVAlumnos.Report = report;
        }

        private void FillBack()
        {
            if (HttpContext.Current.Request.UrlReferrer != null)
            {
                lnkBack.NavigateUrl = HttpContext.Current.Request.UrlReferrer.PathAndQuery;
            }
            else
            {
                lnkBack.NavigateUrl = "~/Default.aspx";
            }
        }
    }
}