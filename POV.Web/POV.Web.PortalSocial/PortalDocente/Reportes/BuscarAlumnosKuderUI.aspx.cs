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
    public partial class BuscarAlumnosKuderUI : Page
    {
        private DataSet DSAlumnosKuder
        {
            get { return Session["AlumnosKuder"] != null ? Session["AlumnosKuder"] as DataSet : null; }
            set { Session["AlumnosKuder"] = value; }
        }

        private IUserSession userSession;
        private IRedirector redirector;

        ResultadoPruebaDinamicaCtrl resultCtrl;

        public BuscarAlumnosKuderUI() 
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
                            LoadAlumnosKuder();
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

        protected void grdAlumnosKuder_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string strAlumno = e.CommandArgument.ToString();
            string strCommandName = e.CommandName.ToString();
            switch (strCommandName)
            {
                case "completar":
                    Response.Redirect("ReporteAlumnoResultadoKuder.aspx?num=" + strAlumno, true);
                    break;
            }
           
        }

        protected void grdAlumnosKuder_DataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void grdAlumnosKuder_Sorting(object sender, GridViewSortEventArgs e)
        {

        }

        private void LoadAlumnosKuder()
        {            
            DSAlumnosKuder = resultCtrl.RetrieveAlumnosPruebas(ConnectionHlp.Default.Connection, new Usuario { UsuarioID = userSession.CurrentUser.UsuarioID }, new PruebaDinamica { PruebaID = 13 });
            LlenarGrid();
        }
        
        private void LlenarGrid() 
        {
            grdAlumnosKuder.DataSource = DSAlumnosKuder;
            grdAlumnosKuder.DataBind();
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {            
            DSAlumnosKuder = resultCtrl.RetrieveAlumnosPruebas(ConnectionHlp.Default.Connection, new Usuario { UsuarioID = userSession.CurrentUser.UsuarioID }, new PruebaDinamica { PruebaID = 13 }, "%" + txtNombre.Text.Trim() + "%");
            LlenarGrid();
        }

        protected void grdAlumnosKuder_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdAlumnosKuder.PageIndex = e.NewPageIndex;
            LlenarGrid();
        }
     }
}