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
    public partial class BuscarAlumnosRavenUI : Page
    {
        private DataSet DSAlumnosRaven
        {
            get { return Session["AlumnosRaven"] != null ? Session["AlumnosRaven"] as DataSet : null; }
            set { Session["AlumnosRaven"] = value; }
        }

        private IUserSession userSession;
        private IRedirector redirector;

        ResultadoPruebaDinamicaCtrl resultCtrl;

        public BuscarAlumnosRavenUI()
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
                            LoadAlumnosRaven();
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

        protected void grdAlumnosRaven_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string strAlumno = e.CommandArgument.ToString();
            string strCommandName = e.CommandName.ToString();
            switch (strCommandName)
            {
                case "completar":
                    Response.Redirect("ReporteAlumnoResultadoRaven.aspx?num=" + strAlumno, true);
                    break;
            }

        }

        protected void grdAlumnosRaven_DataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void grdAlumnosRaven_Sorting(object sender, GridViewSortEventArgs e)
        {

        }

        private void LoadAlumnosRaven()
        {
            DSAlumnosRaven = resultCtrl.RetrieveAlumnosPruebas(ConnectionHlp.Default.Connection, new Usuario { UsuarioID = userSession.CurrentUser.UsuarioID }, new PruebaDinamica { PruebaID = 19 });
            LlenarGrid();
        }

        private void LlenarGrid()
        {
            grdAlumnosRaven.DataSource = DSAlumnosRaven;
            grdAlumnosRaven.DataBind();
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            DSAlumnosRaven = resultCtrl.RetrieveAlumnosPruebas(ConnectionHlp.Default.Connection, new Usuario { UsuarioID = userSession.CurrentUser.UsuarioID }, new PruebaDinamica { PruebaID = 19 }, "%" + txtNombre.Text.Trim() + "%");
            LlenarGrid();
        }

        protected void grdAlumnosRaven_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdAlumnosRaven.PageIndex = e.NewPageIndex;
            LlenarGrid();
        }
    }
}