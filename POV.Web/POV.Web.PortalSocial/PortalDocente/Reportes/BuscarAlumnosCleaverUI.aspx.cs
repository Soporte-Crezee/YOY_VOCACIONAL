using POV.Core.RedSocial.Implement;
using POV.Core.RedSocial.Interfaces;
using POV.Logger.Service;
using POV.Prueba.Diagnostico.Dinamica.BO;
using POV.Prueba.Diagnostico.Dinamica.Service;
using POV.Seguridad.BO;
using POV.Web.Helper;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace POV.Web.PortalSocial.PortalDocente.Reportes
{
    public partial class BuscarAlumnosCleaverUI : Page
    {
        private DataSet DSAlumnosCleaver
        {
            get { return Session["AlumnosCleaver"] != null ? Session["AlumnosCleaver"] as DataSet : null; }
            set { Session["AlumnosCleaver"] = value; }
        }

        private IUserSession userSession;
        private IRedirector redirector;

        ResultadoPruebaDinamicaCtrl resultCtrl;

        public BuscarAlumnosCleaverUI() 
        {
            userSession = new UserSession();
            redirector = new Redirector();

            resultCtrl = new ResultadoPruebaDinamicaCtrl();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    if (userSession.IsLogin())
                        if (!userSession.IsAlumno() && userSession.CurrentEscuela != null)
                        {
                            LoadAlumnosCleaver();
                        }
                        else
                            redirector.GoToHomePage(true);
                    else
                        redirector.GoToLoginPage(true);
                }
                else
                {
                    if (!userSession.IsLogin()) //es alumno
                    {
                        redirector.GoToLoginPage(true);
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerHlp.Default.Error(this, ex);
                throw ex;
            }
            
            
        }

        protected void grdAlumnosCleaver_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string strAlumno = e.CommandArgument.ToString();
            string strCommandName = e.CommandName.ToString();
            switch (strCommandName)
            {
                 case "completar":
                    Response.Redirect("ReporteAlumnoResultadoCleaver.aspx?num=" + strAlumno, true);
                    break;
            }
            
        }

        protected void grdAlumnosCleaver_DataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void grdAlumnosCleaver_Sorting(object sender, GridViewSortEventArgs e)
        {

        }

        private void LoadAlumnosCleaver()
        {
            DSAlumnosCleaver = resultCtrl.RetrieveAlumnosPruebas(ConnectionHlp.Default.Connection, new Usuario { UsuarioID = userSession.CurrentUser.UsuarioID }, new PruebaDinamica { PruebaID = 14 });
            LlenarGrid();
        }

        private void LlenarGrid() 
        {
            grdAlumnosCleaver.DataSource = DSAlumnosCleaver;
            grdAlumnosCleaver.DataBind();
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            DSAlumnosCleaver = resultCtrl.RetrieveAlumnosPruebas(ConnectionHlp.Default.Connection, new Usuario { UsuarioID = userSession.CurrentUser.UsuarioID }, new PruebaDinamica { PruebaID = 14 }, "%"+txtNombre.Text.Trim()+"%" );
            LlenarGrid();
        }

        protected void grdAlumnosCleaver_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdAlumnosCleaver.PageIndex = e.NewPageIndex;
            LlenarGrid();
        }
     }
}