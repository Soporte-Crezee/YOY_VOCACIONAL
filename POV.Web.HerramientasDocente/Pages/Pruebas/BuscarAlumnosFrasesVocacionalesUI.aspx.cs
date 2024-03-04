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
    public partial class BuscarAlumnosFrasesVocacionalesUI : PageBase
    {
        private DataSet DSAlumnosFrasesVocacionales
        {
            get { return Session["AlumnosFrasesVocacionales"] != null ? Session["AlumnosFrasesVocacionales"] as DataSet : null; }
            set { Session["AlumnosFrasesVocacionales"] = value; }
        }

        ResultadoPruebaDinamicaCtrl resultCtrl;

        protected void Page_Load(object sender, EventArgs e)
        {
            resultCtrl = new ResultadoPruebaDinamicaCtrl();
            if (!IsPostBack)
            LoadAlumnosFrasesVocacionales();
        }

        protected override void AuthorizeUser()
        {

        }

        protected void grdAlumnosFrasesVocacionales_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string strAlumno = e.CommandArgument.ToString();
            string strCommandName = e.CommandName.ToString();
            switch (strCommandName)
            {
                case "completar":
                    Response.Redirect("RegistrarSumarioGeneralFrasesVocacionalesUI.aspx?num=" + strAlumno, true);
                    break;
            }
        }

        protected void grdAlumnosFrasesVocacionales_DataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void grdAlumnosFrasesVocacionales_Sorting(object sender, GridViewSortEventArgs e)
        {

        }

        private void LoadAlumnosFrasesVocacionales()
        {
            DSAlumnosFrasesVocacionales = resultCtrl.RetrieveAlumnosPruebas(ConnectionHlp.Default.Connection, new Usuario { UsuarioID = userSession.CurrentUser.UsuarioID }, new PruebaDinamica { PruebaID = 20 });
            LlenarGrid();
        }

        private void LlenarGrid()
        {
            grdAlumnosFrasesVocacionales.DataSource = DSAlumnosFrasesVocacionales;
            grdAlumnosFrasesVocacionales.DataBind();
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            DSAlumnosFrasesVocacionales = resultCtrl.RetrieveAlumnosPruebas(ConnectionHlp.Default.Connection, new Usuario { UsuarioID = userSession.CurrentUser.UsuarioID }, new PruebaDinamica { PruebaID = 20 }, "%" + txtNombre.Text.Trim() + "%");
            LlenarGrid();
        }

        protected void grdAlumnosFrasesVocacionales_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdAlumnosFrasesVocacionales.PageIndex = e.NewPageIndex;
            LlenarGrid();


        }
     }
}