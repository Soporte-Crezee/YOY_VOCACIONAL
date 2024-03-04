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
    public partial class BuscarAlumnosTermanUI : Page
    {
        private DataSet DSAlumnosTerman
        {
            get { return Session["AlumnosTerman"] != null ? Session["AlumnosTerman"] as DataSet : null; }
            set { Session["AlumnosTerman"] = value; }
        }

        private IUserSession userSession;
        private IRedirector redirector;

        ResultadoPruebaDinamicaCtrl resultCtrl;

        public BuscarAlumnosTermanUI()
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
                            LoadAlumnosTerman();
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

        protected void grdAlumnosTerman_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string strAlumno = e.CommandArgument.ToString();
            string strCommandName = e.CommandName.ToString();
            switch (strCommandName)
            {
                case "completar":
                    Response.Redirect("ReporteAlumnoResultadoTerman.aspx?num=" + strAlumno, true);
                    break;
            }

        }

        protected void grdAlumnosTerman_DataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void grdAlumnosTerman_Sorting(object sender, GridViewSortEventArgs e)
        {

        }

        private void LoadAlumnosTerman()
        {
            DSAlumnosTerman = resultCtrl.RetrieveAlumnosPruebas(ConnectionHlp.Default.Connection, new Usuario { UsuarioID = userSession.CurrentUser.UsuarioID }, new PruebaDinamica { PruebaID = 11 });
            LlenarGrid();
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            DSAlumnosTerman = resultCtrl.RetrieveAlumnosPruebas(ConnectionHlp.Default.Connection, new Usuario { UsuarioID = userSession.CurrentUser.UsuarioID }, new PruebaDinamica { PruebaID = 11 }, "%" + txtNombre.Text.Trim() + "%");
            LlenarGrid();
        }

        private void LlenarGrid()
        {
            grdAlumnosTerman.DataSource = DSAlumnosTerman;
            grdAlumnosTerman.DataBind();
        }

        protected void grdAlumnosTerman_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdAlumnosTerman.PageIndex = e.NewPageIndex;
            LlenarGrid();
        }
    }
}