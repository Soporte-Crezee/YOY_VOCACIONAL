using POV.Administracion.Service;
using POV.CentroEducativo.BO;
using POV.CentroEducativo.Service;
using POV.CentroEducativo.Services;
using POV.Comun.BO;
using POV.Comun.Service;
using POV.Core.PadreTutor.Implements;
using POV.Core.PadreTutor.Interfaces;
using POV.Expediente.BO;
using POV.Expediente.Service;
using POV.Licencias.BO;
using POV.Licencias.Service;
using POV.Localizacion.BO;
using POV.Localizacion.Service;
using POV.Logger.Service;
using POV.Modelo.BO;
using POV.Prueba.Diagnostico.Dinamica.BO;
using POV.Prueba.Diagnostico.Service;
using POV.Seguridad.BO;
using POV.Seguridad.Service;
using POV.Web.PortalTutor.Helper;
using Framework.Base.DataAccess;
using GP.SocialEngine.BO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using POV.Expediente.Services;
using System.Text;
using System.IO;

namespace POV.Web.PortalTutor.Pages
{
    public partial class ExpedienteAlumnoTutor : System.Web.UI.Page
    {
        #region Propiedades
        public List<Alumno> ListAlumno
        {
            get { return Session["ListAlumno"] != null ? Session["ListAlumno"] as List<Alumno> : null; }
            set { Session["ListAlumno"] = value; }
        }

        private IUserSession userSession;
        private IRedirector redirector;
        private readonly IDataContext dctx = ConnectionHlp.Default.Connection;
                
        private AlumnoCtrl alumnoCtrl;
        #endregion

        public ExpedienteAlumnoTutor()
        {
            userSession = new UserSession();
            redirector = new Redirector();

            alumnoCtrl = new AlumnoCtrl();
        }

        #region Eventos de la página
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    if (userSession.IsLogin())
                    {
                        if ((bool)userSession.CurrentTutor.CorreoConfirmado && (bool)userSession.CurrentTutor.DatosCompletos)
                            ConsultarAlumnoUsuario();
                        else
                            redirector.GoToHomePage(true);
                    }
                    else
                        redirector.GoToLoginPage(true);
                }
                else
                {
                    if (!userSession.IsLogin()) //es tutor
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

        protected void btnBuscarAlumno_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtAlumno.Text.Trim() != string.Empty)
                {
                    gvAlumnos.DataSource = ListAlumno.Where(x => x.NombreCompletoAlumno.Replace('á', 'a').Replace('é', 'e').Replace('í', 'i').Replace('ó', 'o').Replace('u', 'u').ToLower().Contains(txtAlumno.Text.Replace('á', 'a').Replace('é', 'e').Replace('í', 'i').Replace('ó', 'o').Replace('u', 'u').Trim().ToLower())).ToList(); ;
                }
                else
                {
                    gvAlumnos.DataSource = ListAlumno.Where(x => x.DatosCompletos == true).ToList();
                }
                gvAlumnos.DataBind();
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void gvAlumnos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                string strAlumno = e.CommandArgument.ToString();
                Response.Redirect("DetalleExpedienteAlumnoTutor.aspx?num=" + strAlumno, true);
            }
            catch (Exception ex)
            {
                throw ex;
            } 
        }

        protected void gvAlumnos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow) 
            {
                TableCell statusCell = e.Row.Cells[1];
                if (statusCell.Text == "SEMESTRE_1")
                    statusCell.Text = "Semestre 1";
                if (statusCell.Text == "SEMESTRE_2")
                    statusCell.Text = "Semestre 2";
                if (statusCell.Text == "SEMESTRE_3")
                    statusCell.Text = "Semestre 3";
                if (statusCell.Text == "SEMESTRE_4")
                    statusCell.Text = "Semestre 4";
                if (statusCell.Text == "SEMESTRE_5")
                    statusCell.Text = "Semestre 5";
                if (statusCell.Text == "SEMESTRE_6")
                    statusCell.Text = "Semestre 6";
            }
        }
        #endregion

        #region Métodos Auxiliares
        private void ConsultarAlumnoUsuario()
        {
            TutorCtrl tutCtrl = new TutorCtrl(null);
            List<TutorAlumno> lista = userSession.CurrentTutor.Alumnos.ToList();
            LlenarDropAlumnosUsuarios(lista);
        }

        private void LlenarDropAlumnosUsuarios(List<TutorAlumno> lista)
        {
            EFAlumnoCtrl eFAlumnoCtrl = new EFAlumnoCtrl(null);
            List<Alumno> alumnos = new List<Alumno>();
            if (lista.Count > 0)
            {
                foreach (TutorAlumno alumno in lista)
                {
                    Alumno seleccionado = eFAlumnoCtrl.Retrieve(new Alumno { AlumnoID = alumno.AlumnoID }, false).FirstOrDefault();
                    if (seleccionado.AlumnoID != null)
                        alumnos.Add(seleccionado);
                }
            }
            ListAlumno = alumnos;
            gvAlumnos.DataSource = ListAlumno.Where(x => x.DatosCompletos == true).ToList();
            gvAlumnos.DataBind();
        }
        #endregion
    }
}