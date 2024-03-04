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
    public partial class BuscarAlumnosSacksUI : System.Web.UI.Page
    {
        #region Propiedades de la clase
        private DataSet DSAlumnosSacks
        {
            get { return Session["AlumnosSacks"] != null ? Session["AlumnosSacks"] as DataSet : null; }
            set { Session["AlumnosSacks"] = value; }
        }

        private IUserSession userSession;
        private IRedirector redirector;

        private ResultadoPruebaDinamicaCtrl resultCtrl;
        #endregion

        public BuscarAlumnosSacksUI()
        {
            userSession = new UserSession();
            redirector = new Redirector();

            resultCtrl = new ResultadoPruebaDinamicaCtrl();
        }

        #region Eventos de la pagina
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    if (userSession.IsLogin())
                        if (!userSession.IsAlumno() && userSession.CurrentEscuela != null)
                        {
                            LoadAlumnosSacks();
                        }
                        else
                            redirector.GoToHomePage(true);
                    else
                        redirector.GoToLoginPage(true);
                }
                else
                {
                    if (!userSession.IsLogin())
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
            DSAlumnosSacks = resultCtrl.RetrieveAlumnosPruebasSacks(ConnectionHlp.Default.Connection, new Usuario { UsuarioID = userSession.CurrentUser.UsuarioID }, new PruebaDinamica { PruebaID = 12 }, "%" + txtNombre.Text.Trim() + "%");
            LlenarGrid();
        }

        protected void grdAlumnosSacks_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string strAlumno = e.CommandArgument.ToString();
            string strCommandName = e.CommandName.ToString();
            switch (strCommandName)
            {
                case "completar":
                    Response.Redirect("ReporteAlumnoResultadoSacks.aspx?num=" + strAlumno, true);
                    break;
            }

        }

        protected void grdAlumnosSacks_Sorting(object sender, GridViewSortEventArgs e)
        {

        }

        protected void grdAlumnosSacks_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }
        #endregion
        #region Métodos Auxiliares
        private void LoadAlumnosSacks()
        {
            DSAlumnosSacks = resultCtrl.RetrieveAlumnosPruebasSacks(ConnectionHlp.Default.Connection, new Usuario { UsuarioID = userSession.CurrentUser.UsuarioID }, new PruebaDinamica { PruebaID = 12 });
            LlenarGrid();
        }

        private void LlenarGrid()
        {
            grdAlumnosSacks.DataSource = DSAlumnosSacks;
            grdAlumnosSacks.DataBind();
        }
        #endregion

        protected void grdAlumnosSacks_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdAlumnosSacks.PageIndex = e.NewPageIndex;
            LlenarGrid();
        }
    }
}