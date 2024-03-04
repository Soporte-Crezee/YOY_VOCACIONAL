using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using POV.Comun.BO;
using POV.Operaciones.Reports;
using POV.Core.Operaciones.Implements;
using POV.Core.Operaciones.Interfaces;
using POV.Seguridad.BO;
using POV.Licencias.BO;

namespace POV.Web.PortalOperaciones.Reportes
{
    public partial class CargaEscuelasReport : System.Web.UI.Page
    {

        private IUserSession userSession;
        private IRedirector redirector;

        public CargaEscuelasReport()
        {
            userSession = new UserSession();
            redirector = new Redirector();
        }

        private DataTable SS_ResultadoCarga
        {
            get { return (DataTable)this.Session["ResultadoCargaEscuelas"]; }
            set { this.Session["ResultadoCargaEscuelas"] = value; }
        }

        private DataTable SS_ListadoCarga
        {
            get { return (DataTable)this.Session["ListadoCargaEscuelas"]; }
            set { this.Session["ListadoCargaEscuelas"] = value; }
        }

        private Pais SS_PaisCarga
        {
            get { return (Pais)this.Session["PaisCarga"]; }
            set { this.Session["PaisCarga"] = value; }
        }

        private Estado SS_EstadoCarga
        {
            get { return (Estado)this.Session["EstadoCarga"]; }
            set { this.Session["EstadoCarga"] = value; }
        }

        private Contrato SS_ContratoCarga
        {
            get { return (Contrato)this.Session["ContratoCarga"]; }
            set { this.Session["ContratoCarga"] = value; }
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
                    

                    string usuario = userSession.CurrentUser.NombreUsuario;

                    this.LblNombreUsuario.Text = usuario;
                    FillBack();
                }
            }

            if (SS_PaisCarga == null || SS_EstadoCarga == null || SS_ListadoCarga == null || SS_ResultadoCarga == null)
            {
                redirector.GoToHomePage(false);
                return;
            }

            CargaEscuelasRpt report = new CargaEscuelasRpt(this.SS_PaisCarga, this.SS_EstadoCarga, this.SS_ContratoCarga, this.SS_ListadoCarga, this.SS_ResultadoCarga);
            rptVEscuelas.Report = report;
        }


        private void FillBack()
        {
            if (HttpContext.Current.Request.UrlReferrer != null)
                lnkBack.NavigateUrl = HttpContext.Current.Request.UrlReferrer.PathAndQuery;
            else
                lnkBack.NavigateUrl = "~/Default.aspx";
        }
    }
}