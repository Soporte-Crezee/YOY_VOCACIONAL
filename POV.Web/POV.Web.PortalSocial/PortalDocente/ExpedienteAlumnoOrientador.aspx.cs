using POV.Administracion.Service;
using POV.CentroEducativo.BO;
using POV.CentroEducativo.DAO;
using POV.CentroEducativo.Service;
using POV.Comun.BO;
using POV.Comun.Service;
using POV.Core.RedSocial.Implement;
using POV.Core.RedSocial.Interfaces;
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
using POV.Web.Administracion.Helper;
using POV.Web.DTO;
using POV.Web.PortalSocial.AppCode;
using POV.Web.ServiciosActividades.Controllers;
using DevExpress.Data.Browsing;
using Framework.Base.DataAccess;
using GP.SocialEngine.BO;
using GP.SocialEngine.Service;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using POV.Expediente.Services;
using POV.CentroEducativo.Services;
using System.IO;

namespace POV.Web.PortalSocial.PortalDocente
{
    public partial class ExpedienteAlumnoOrientador : System.Web.UI.Page
    {
        #region Propiedades
        public List<Alumno> ListAlumno
        {
            get { return Session["ListAlumno"] != null ? Session["ListAlumno"] as List<Alumno> : null; }
            set { Session["ListAlumno"] = value; }
        }

        private IUserSession userSession;
        private IRedirector redirector;
        private readonly IDataContext dctx = ConnectionHelper.Default.Connection;

        private AlumnoCtrl alumnoCtrl;

        private UsuarioExpedienteCtrl usuarioExpedienteCtrl;

        #endregion

        public ExpedienteAlumnoOrientador()
        {
            userSession = new UserSession();
            redirector = new Redirector();

            alumnoCtrl = new AlumnoCtrl();

            usuarioExpedienteCtrl = new UsuarioExpedienteCtrl();
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
                            this.hdnSessionSocialHubID.Value = userSession.SocialHub.SocialHubID.ToString();
                            this.hdnSessionUsuarioSocialID.Value = userSession.CurrentUsuarioSocial.UsuarioSocialID.ToString();
                            hdnSocialHubID.Value = userSession.SocialHub.SocialHubID.ToString();
                            hdnUsuarioSocialID.Value = userSession.CurrentUsuarioSocial.UsuarioSocialID.ToString();
                            hdnTipoPublicacionSuscripcionReactivo.Value = ((short)ETipoPublicacion.REACTIVO).ToString();
                            hdnTipoPublicacionTexto.Value = ((short)ETipoPublicacion.TEXTO).ToString();

                            LlenarDropAlumnosUsuarios();
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

        #region Utilizando Vista

        private void LlenarDropAlumnosUsuarios()
        {
            UsuarioExpediente usuarioExpediente = new UsuarioExpediente();
            usuarioExpediente.UsuarioID = userSession.CurrentUser.UsuarioID;
            List<Alumno> listaPreAsignados = new List<Alumno>();
            DataSet ds = usuarioExpedienteCtrl.Retrieve(dctx, usuarioExpediente);
            EFAlumnoCtrl eFAlumnoCtrl = new EFAlumnoCtrl(null);
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    UsuarioExpediente usExp = usuarioExpedienteCtrl.DataRowToUsuarioExpediente(ds.Tables[0].Rows[i]);
                    // Obtener el alumno
                    Alumno alumno = eFAlumnoCtrl.Retrieve(new Alumno { AlumnoID = usExp.AlumnoID }, false).FirstOrDefault();
                    if (alumno != null)
                    {
                        listaPreAsignados.Add(alumno);
                    }
                }
            }
            ListAlumno = listaPreAsignados.Where(x => x.DatosCompletos == true).ToList();
            gvAlumnos.DataSource = ListAlumno.ToList();
            gvAlumnos.DataBind();
        }        
        #endregion

        protected void btnBuscarAlumno_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtAlumno.Text.Trim() != string.Empty)
                {
                    gvAlumnos.DataSource = ListAlumno.Where(x => x.NombreCompletoAlumno.ToLower().Contains(txtAlumno.Text.Trim().ToLower())).ToList();
                }
                else
                {
                    gvAlumnos.DataSource = ListAlumno;
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
            string strAlumno = e.CommandArgument.ToString();
            Response.Redirect("DetalleExpedienteAlumnoOrientador.aspx?num=" + strAlumno, true);
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

        protected void gvAlumnos_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}