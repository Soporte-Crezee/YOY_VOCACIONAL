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

namespace POV.Web.PortalSocial.Social
{
    public partial class VerReporteAbuso : System.Web.UI.Page
    {
        private IRedirector redirector;
        private IUserSession userSession;
        private ReporteAbusoCtrl reporteAbusoCtrl;
        private readonly IDataContext dctx;
        public VerReporteAbuso()
        {
            redirector = new Redirector();
            userSession = new UserSession();
            reporteAbusoCtrl = new ReporteAbusoCtrl();
            dctx = new DataContext(new DataProviderFactory().GetDataProvider("POV"));
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (userSession != null && userSession.IsLogin())
                {
                    if (!Page.IsPostBack)
                    {
                        string strid = Request.QueryString["n"];

                        if (!string.IsNullOrEmpty(strid))
                        {
                            //Obtener el reporte de abuso
                            ReporteAbuso reporte = reporteAbusoCtrl.RetrieveComplete(dctx, new ReporteAbuso { ReporteAbusoID = Guid.Parse(strid) });
                            hdnReportableID.Value = reporte.Reportable.GUID.ToString();
                            hdnReporteAbusoID.Value = reporte.ReporteAbusoID.ToString();
                            hdnTipoReporteAbusoID.Value = ((short) reporte.TipoContenido).ToString();
                        }

                    }

                }
                else
                {
                    redirector.GoToHomeDocente(true);
                }

            }
            catch (Exception ex)
            {
                POV.Logger.Service.LoggerHlp.Default.Error(this,ex);
                throw;
            }
        }
        
    }
}