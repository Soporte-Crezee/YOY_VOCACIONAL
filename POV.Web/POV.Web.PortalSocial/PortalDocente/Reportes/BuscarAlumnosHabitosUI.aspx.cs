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
    public partial class BuscarAlumnosHabitosUI : Page
    {
        private DataSet DSAlumnosHabitos
        {
            get { return Session["AlumnosHabitos"] != null ? Session["AlumnosHabitos"] as DataSet : null; }
            set { Session["AlumnosHabitos"] = value; }
        }

        private IUserSession userSession;
        private IRedirector redirector;

        ResultadoPruebaDinamicaCtrl resultCtrl;

        public BuscarAlumnosHabitosUI() 
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
                            LoadAlumnosHabitos();
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

        protected void grdAlumnosHabitos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string strAlumno = e.CommandArgument.ToString();
            string strCommandName = e.CommandName.ToString();
            switch (strCommandName)
            {
                case "completar":
                    Response.Redirect("ReporteAlumnoResultadoHabitos.aspx?num=" + strAlumno, true);
                    break;
            }
            
        }

        protected void grdAlumnosHabitos_DataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void grdAlumnosHabitos_Sorting(object sender, GridViewSortEventArgs e)
        {

        }

        private void LoadAlumnosHabitos()
        {            
            DSAlumnosHabitos = resultCtrl.RetrieveAlumnosPruebas(ConnectionHlp.Default.Connection, new Usuario { UsuarioID = userSession.CurrentUser.UsuarioID }, new PruebaDinamica { PruebaID = 9 });
            LlenarGrid();
        }

        private void LlenarGrid() 
        {
            grdAlumnosHabitos.DataSource = DSAlumnosHabitos;
            grdAlumnosHabitos.DataBind();        
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            DSAlumnosHabitos = resultCtrl.RetrieveAlumnosPruebas(ConnectionHlp.Default.Connection, new Usuario { UsuarioID = userSession.CurrentUser.UsuarioID }, new PruebaDinamica { PruebaID = 9 }, "%" + txtNombre.Text.Trim() + "%");
            LlenarGrid();
        }

        protected void grdAlumnosHabitos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdAlumnosHabitos.PageIndex = e.NewPageIndex;
            LlenarGrid();
        }
     }
}