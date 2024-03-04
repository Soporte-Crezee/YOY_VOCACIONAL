using POV.Core.RedSocial.Implement;
using POV.Core.RedSocial.Interfaces;
using POV.Logger.Service;
using POV.Prueba.Diagnostico.Dinamica.BO;
using POV.Prueba.Diagnostico.Dinamica.Service;
using POV.Seguridad.BO;
using POV.Web.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace POV.Web.PortalSocial.PortalDocente.Reportes
{
    public partial class BuscarAlumnosRotterUI : Page
    {
        #region Propiedades de la clase
        private DataSet DSAlumnosRotter
        {
            get { return Session["AlumnosRotter"] != null ? Session["AlumnosRotter"] as DataSet : null; }
            set { Session["AlumnosRotter"] = value; }
        }

        private IUserSession userSession;
        private IRedirector redirector;

        private ResultadoPruebaDinamicaCtrl resultadoPruebaDinamicaCtrl;
        #endregion

        public BuscarAlumnosRotterUI()
        {
            userSession = new UserSession();
            redirector = new Redirector();

            resultadoPruebaDinamicaCtrl = new ResultadoPruebaDinamicaCtrl();
        }
        #region Eventos de la página
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    if (userSession.IsLogin())

                        if (!userSession.IsAlumno() && userSession.CurrentEscuela != null)
                        {
                            LoadAlumnosRotter();
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

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            DSAlumnosRotter = resultadoPruebaDinamicaCtrl.RetrieveAlumnosPruebas(ConnectionHlp.Default.Connection, new Usuario { UsuarioID = userSession.CurrentUser.UsuarioID },
                new PruebaDinamica { PruebaID = 18 }, "%" + txtNombre.Text.Trim() + "%");
            LlenarGrid();
        }

        protected void grdAlumnosRotter_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string strAlumno = e.CommandArgument.ToString();
            string strCommandName = e.CommandName.ToString();
            switch (strCommandName)
            {
                case "completar":
                    Response.Redirect("ReporteAlumnoResultadoRotter.aspx?num=" + strAlumno, true);
                    break;
            }
            
        }

        protected void grdAlumnosRotter_Sorting(object sender, GridViewSortEventArgs e)
        {

        }

        protected void grdAlumnosRotter_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }
        #endregion

        #region Métodos auxiliares
        private void LoadAlumnosRotter()
        {            
            DSAlumnosRotter = resultadoPruebaDinamicaCtrl.RetrieveAlumnosPruebas(ConnectionHlp.Default.Connection, new Usuario { UsuarioID = userSession.CurrentUser.UsuarioID },
                new PruebaDinamica { PruebaID = 18 });
            LlenarGrid();
        }

        private void LlenarGrid() 
        {
            grdAlumnosRotter.DataSource = DSAlumnosRotter;
            grdAlumnosRotter.DataBind();
        }
        #endregion

        protected void grdAlumnosRotter_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdAlumnosRotter.PageIndex = e.NewPageIndex;
            LlenarGrid();
        }
    }
}