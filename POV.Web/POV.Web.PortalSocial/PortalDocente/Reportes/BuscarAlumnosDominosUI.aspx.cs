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
    public partial class BuscarAlumnosDominosUI : Page
    {
        private DataSet DSAlumnosDominos
        {
            get { return Session["AlumnosDominos"] != null ? Session["AlumnosDominos"] as DataSet : null; }
            set { Session["AlumnosDominos"] = value; }
        }

        private IUserSession userSession;
        private IRedirector redirector;

        ResultadoPruebaDinamicaCtrl resultCtrl;

        public BuscarAlumnosDominosUI()
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
                            LoadAlumnosDominos();
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

        protected void grdAlumnosDominos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string strAlumno = e.CommandArgument.ToString();
            string strCommandName = e.CommandName.ToString();
            switch (strCommandName)
            {
                case "completar":
                    Response.Redirect("ReporteAlumnoResultadoDominos.aspx?num=" + strAlumno, true);
                    break;
            }

        }

        protected void grdAlumnosDominos_DataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void grdAlumnosDominos_Sorting(object sender, GridViewSortEventArgs e)
        {

        }

        private void LoadAlumnosDominos()
        {
            DSAlumnosDominos = resultCtrl.RetrieveAlumnosPruebas(ConnectionHlp.Default.Connection, new Usuario { UsuarioID = userSession.CurrentUser.UsuarioID }, new PruebaDinamica { PruebaID = 10 });
            LlenarGrid();
        }

        private void LlenarGrid()
        {
            grdAlumnosDominos.DataSource = DSAlumnosDominos;
            grdAlumnosDominos.DataBind();
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            DSAlumnosDominos = resultCtrl.RetrieveAlumnosPruebas(ConnectionHlp.Default.Connection, new Usuario { UsuarioID = userSession.CurrentUser.UsuarioID }, new PruebaDinamica { PruebaID = 10 }, "%" + txtNombre.Text.Trim() + "%");
            LlenarGrid();
        }

        protected void grdAlumnosDominos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdAlumnosDominos.PageIndex = e.NewPageIndex;
            LlenarGrid();
        }
    }
}