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
    public partial class BuscarAlumnosChasideUI : Page
    {
        private DataSet DSAlumnosChaside
        {
            get { return Session["AlumnosChaside"] != null ? Session["AlumnosChaside"] as DataSet : null; }
            set { Session["AlumnosChaside"] = value; }
        }

        private IUserSession userSession;
        private IRedirector redirector;

        ResultadoPruebaDinamicaCtrl resultCtrl;

        public BuscarAlumnosChasideUI() 
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
                            LoadAlumnosChaside();
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

        protected void grdAlumnosChaside_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string strAlumno = e.CommandArgument.ToString();
            string strCommandName = e.CommandName.ToString();
            switch (strCommandName)
            {
                case "completar":
                    Response.Redirect("ReporteAlumnoResultadoChaside.aspx?num=" + strAlumno, true);
                    break;
            }
           
        }

        protected void grdAlumnosChaside_DataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void grdAlumnosChaside_Sorting(object sender, GridViewSortEventArgs e)
        {

        }

        private void LoadAlumnosChaside()
        {            
            DSAlumnosChaside = resultCtrl.RetrieveAlumnosPruebas(ConnectionHlp.Default.Connection, new Usuario { UsuarioID = userSession.CurrentUser.UsuarioID }, new PruebaDinamica { PruebaID = 17 });
            LlenarGrid();
        }

        private void LlenarGrid() 
        {
            grdAlumnosChaside.DataSource = DSAlumnosChaside;
            grdAlumnosChaside.DataBind();
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {            
            DSAlumnosChaside = resultCtrl.RetrieveAlumnosPruebas(ConnectionHlp.Default.Connection, new Usuario { UsuarioID = userSession.CurrentUser.UsuarioID }, new PruebaDinamica { PruebaID = 17 }, "%"+txtNombre.Text.Trim()+"%" );
            LlenarGrid();
        }

        protected void grdAlumnosChaside_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdAlumnosChaside.PageIndex = e.NewPageIndex;
            LlenarGrid();
        }
     }
}