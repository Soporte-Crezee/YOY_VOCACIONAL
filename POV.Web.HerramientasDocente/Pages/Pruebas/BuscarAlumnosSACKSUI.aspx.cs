using POV.AppCode.Page;
using POV.Prueba.Diagnostico.Dinamica.BO;
using POV.Prueba.Diagnostico.Dinamica.Service;
using POV.Seguridad.BO;
using POV.Web.Helper;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace POV.Web.HerramientasDocente.Pages.Pruebas
{
    public partial class BuscarAlumnosSACKSUI : PageBase
    {
        private DataSet DSAlumnosSACKS
        {
            get { return Session["AlumnosSACKS"] != null ? Session["AlumnosSACKS"] as DataSet : null; }
            set { Session["AlumnosSACKS"] = value; }
        }

        ResultadoPruebaDinamicaCtrl resultCtrl;

        protected void Page_Load(object sender, EventArgs e)
        {
            resultCtrl = new ResultadoPruebaDinamicaCtrl();
            if (!IsPostBack)
            LoadAlumnosSACKS();
        }

        protected override void AuthorizeUser()
        {

        }

        protected void grdAlumnosSACKS_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string strAlumno = e.CommandArgument.ToString();
            string strCommandName = e.CommandName.ToString();
            switch (strCommandName)
            {
                case "completar":
                    Response.Redirect("RegistrarSumarioGeneralSACKSUI.aspx?num=" + strAlumno, true);
                    break;
            }
        }

        protected void grdAlumnosSACKS_DataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void grdAlumnosSACKS_Sorting(object sender, GridViewSortEventArgs e)
        {

        }

        private void LoadAlumnosSACKS()
        {
            DSAlumnosSACKS = resultCtrl.RetrieveAlumnosPruebas(ConnectionHlp.Default.Connection, new Usuario { UsuarioID = userSession.CurrentUser.UsuarioID }, new PruebaDinamica { PruebaID = 12 });
            LlenarGrid();
        }

        private void LlenarGrid()
        {
            grdAlumnosSACKS.DataSource = DSAlumnosSACKS;
            grdAlumnosSACKS.DataBind();
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            DSAlumnosSACKS = resultCtrl.RetrieveAlumnosPruebas(ConnectionHlp.Default.Connection, new Usuario { UsuarioID = userSession.CurrentUser.UsuarioID }, new PruebaDinamica { PruebaID = 12 }, "%"+txtNombre.Text.Trim()+"%" );
            LlenarGrid();
        }

        protected void grdAlumnosSACKS_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdAlumnosSACKS.PageIndex = e.NewPageIndex;
            LlenarGrid();
        }
     }
}