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
    public partial class BuscarAlumnosBullyingUI : Page
    {
        private DataSet DSAlumnosBullying
        {
            get { return Session["AlumnosBullying"] != null ? Session["AlumnosBullying"] as DataSet : null; }
            set { Session["AlumnosBullying"] = value; }
        }

        private IUserSession userSession;
        private IRedirector redirector;

        ResultadoPruebaDinamicaCtrl resultCtrl;

        List<PruebaDinamica> lista;

        public BuscarAlumnosBullyingUI()
        {
            userSession = new UserSession();
            redirector = new Redirector();
            resultCtrl = new ResultadoPruebaDinamicaCtrl();
            lista = new List<PruebaDinamica>();
            
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
                            LoadAlumnosBullying();
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

        protected void grdAlumnosBullying_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string strAlumno = e.CommandArgument.ToString();
            string strCommandName = e.CommandName.ToString();
            switch (strCommandName)
            {
                case "completar":
                    Response.Redirect("ReporteAlumnoResultadoBullying.aspx?num=" + strAlumno, true);
                    break;
            }

        }

        protected void grdAlumnosBullying_DataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void grdAlumnosBullying_Sorting(object sender, GridViewSortEventArgs e)
        {

        }

        private void LoadAlumnosBullying()
        {
            //DSAlumnosBullying = resultCtrl.RetrieveAlumnosPruebas(ConnectionHlp.Default.Connection, new Usuario { UsuarioID = userSession.CurrentUser.UsuarioID }, new PruebaDinamica { PruebaID = 11 });
            //List<PruebaDinamica> lista = new List<PruebaDinamica>();
            #region Pruebas Bullying
            lista.Add(new PruebaDinamica { PruebaID = 22 });
            lista.Add(new PruebaDinamica { PruebaID = 23 });
            lista.Add(new PruebaDinamica { PruebaID = 24 });
            lista.Add(new PruebaDinamica { PruebaID = 25 });
            lista.Add(new PruebaDinamica { PruebaID = 26 });
            lista.Add(new PruebaDinamica { PruebaID = 27 });
            lista.Add(new PruebaDinamica { PruebaID = 28 });
            lista.Add(new PruebaDinamica { PruebaID = 29 });
            lista.Add(new PruebaDinamica { PruebaID = 30 });
            lista.Add(new PruebaDinamica { PruebaID = 31 });
            lista.Add(new PruebaDinamica { PruebaID = 32 });
            lista.Add(new PruebaDinamica { PruebaID = 33 });
            #endregion
            DSAlumnosBullying = resultCtrl.RetrieveAlumnosPruebasBullying(ConnectionHlp.Default.Connection, new Usuario { UsuarioID = userSession.CurrentUser.UsuarioID }, lista);
            LlenarGrid();
        }

        private void LlenarGrid()
        {
            grdAlumnosBullying.DataSource = DSAlumnosBullying;
            grdAlumnosBullying.DataBind();
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            #region Pruebas Bullying
            lista.Add(new PruebaDinamica { PruebaID = 22 });
            lista.Add(new PruebaDinamica { PruebaID = 23 });
            lista.Add(new PruebaDinamica { PruebaID = 24 });
            lista.Add(new PruebaDinamica { PruebaID = 25 });
            lista.Add(new PruebaDinamica { PruebaID = 26 });
            lista.Add(new PruebaDinamica { PruebaID = 27 });
            lista.Add(new PruebaDinamica { PruebaID = 28 });
            lista.Add(new PruebaDinamica { PruebaID = 29 });
            lista.Add(new PruebaDinamica { PruebaID = 30 });
            lista.Add(new PruebaDinamica { PruebaID = 31 });
            lista.Add(new PruebaDinamica { PruebaID = 32 });
            lista.Add(new PruebaDinamica { PruebaID = 33 });
            #endregion
            DSAlumnosBullying = resultCtrl.RetrieveAlumnosPruebasBullying(ConnectionHlp.Default.Connection, new Usuario { UsuarioID = userSession.CurrentUser.UsuarioID }, lista, "%" + txtNombre.Text.Trim() + "%");
            LlenarGrid();
        }

        protected void grdAlumnosBullying_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdAlumnosBullying.PageIndex = e.NewPageIndex;
            LlenarGrid();
        }
    }
}