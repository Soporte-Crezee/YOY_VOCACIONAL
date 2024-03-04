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
using System.Web.UI;
using System.Web.UI.WebControls;

namespace POV.Web.PortalSocial.PortalDocente.Reportes
{
    public partial class BuscarAlumnosInteligenciasMultiplesUI : Page
    {
        private DataSet DSAlumnosInteligenciasMultiples
        {
            get { return Session["AlumnosInteligenciasMultiples"] != null ? Session["AlumnosInteligenciasMultiples"] as DataSet : null; }
            set { Session["AlumnosInteligenciasMultiples"] = value; }
        }

        private IUserSession userSession;
        private IRedirector redirector;

        ResultadoPruebaDinamicaCtrl resultCtrl;

      

        public BuscarAlumnosInteligenciasMultiplesUI()
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
                            LoadAlumnosInteligenciasMultiples();
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

        protected void grdAlumnosInteligenciasMultiples_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string strAlumno = e.CommandArgument.ToString();
            string strCommandName = e.CommandName.ToString();
            switch (strCommandName)
            {
                case "completar":
                    Response.Redirect("ReporteAlumnoResultadoInteligenciasMultiples.aspx?num=" + strAlumno, true);
                    break;
            }

        }

        protected void grdAlumnosInteligenciasMultiples_DataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void grdAlumnosInteligenciasMultiples_Sorting(object sender, GridViewSortEventArgs e)
        {

        }

        private void LoadAlumnosInteligenciasMultiples()
        {


            DSAlumnosInteligenciasMultiples = resultCtrl.RetrieveAlumnosPruebas(ConnectionHlp.Default.Connection, new Usuario { UsuarioID = userSession.CurrentUser.UsuarioID }, new PruebaDinamica { PruebaID = 37 });
            LlenarGrid();
        }

        private void LlenarGrid()
        {


            grdAlumnosInteligenciasMultiples.DataSource = DSAlumnosInteligenciasMultiples;
            grdAlumnosInteligenciasMultiples.DataBind();
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {

            DSAlumnosInteligenciasMultiples = resultCtrl.RetrieveAlumnosPruebas(ConnectionHlp.Default.Connection, new Usuario { UsuarioID = userSession.CurrentUser.UsuarioID }, new PruebaDinamica { PruebaID = 37 }, "%" + txtNombre.Text.Trim() + "%");
            LlenarGrid();

        }

        protected void grdAlumnosInteligenciasMultiples_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdAlumnosInteligenciasMultiples.PageIndex = e.NewPageIndex;
            LlenarGrid();
        }

    }
}