using POV.CentroEducativo.BO;
using POV.CentroEducativo.Service;
using POV.Comun.BO;
using POV.Comun.Service;
using POV.Licencias.BO;
using POV.Licencias.Service;
using POV.Logger.Service;
using POV.Modelo.BO;
using POV.Modelo.Service;
using POV.Prueba.Diagnostico.Dinamica.BO;
using POV.Prueba.Diagnostico.Dinamica.Service;
using POV.Seguridad.BO;
using Framework.Base.DataAccess;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using POV.Web.PortalUniversidad.AppCode.Page;
using Framework.Base.Exceptions;
using POV.Web.PortalUniversidad.Helper;

namespace POV.Web.PortalUniversidades.Pages
{
    public partial class ExpedienteAlumnoUniversidad : CatalogPage
    {
        #region propiedades
        public InfoAlumnoUsuario verExpediente
        {
            get { return Session["verExpediente"] as InfoAlumnoUsuario; }
            set { Session["verExpediente"] = value; }
        }

        public List<InfoAlumnoUsuario> ListAlumno
        {
            get { return Session["ListAlumno"] != null ? Session["ListAlumno"] as List<InfoAlumnoUsuario> : null; }
            set { Session["ListAlumno"] = value; }
        }

        public InfoAlumnoUsuario selecciones
        {
            get { return Session["selecciones"] as InfoAlumnoUsuario; }
            set { Session["selecciones"] = value; }
        }

        private readonly IDataContext dctx = ConnectionHlp.Default.Connection;

        private LicenciaEscuelaCtrl licenciaEscuelaCtrl;

        private EstadoCtrl estadoCtrl;

        private GrupoCicloEscolarCtrl grupoCicloEscolarCtrl;

        private ModeloCtrl modeloCtrl;
        private PruebaDinamicaCtrl pruebaDinamicaCtrl;

        InfoAlumnoUsuario alumnoUsuario;
        InfoAlumnoUsuarioCtrl infoAlumnoUsuarioCtrl;
        #endregion
        public ExpedienteAlumnoUniversidad()
        {
            licenciaEscuelaCtrl = new LicenciaEscuelaCtrl();

            estadoCtrl = new EstadoCtrl();

            grupoCicloEscolarCtrl = new GrupoCicloEscolarCtrl();

            modeloCtrl = new ModeloCtrl();
            pruebaDinamicaCtrl = new PruebaDinamicaCtrl();

            alumnoUsuario = new InfoAlumnoUsuario();
            infoAlumnoUsuarioCtrl = new InfoAlumnoUsuarioCtrl();
        }

        #region Eventos de la clase

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (userSession != null && userSession.CurrentUniversidad != null && userSession.CurrentCicloEscolar != null)
                {
                    if (!IsPostBack)
                    {
                        verExpediente = null;
                        LoadAreasConocimiento();
                        LoadEstado();
                        LoadAlumnosUsuario(userSession.CurrentUniversidad);
                        if (selecciones != null)
                        {
                            InfoAlumnoUsuario recuper = selecciones;
                            RestablecerEstados(recuper);
                            LoadAlumnosUsuario(userSession.CurrentUniversidad);
                        }
                    }
                }
                else
                    redirector.GoToLoginPage(true);
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message, MessageType.Error);
                LoggerHlp.Default.Error(this, ex);
            }
        }

        protected void btnBuscarAlumno_Click(object sender, EventArgs e)
        {
            try
            {
                LoadAlumnosUsuario(userSession.CurrentUniversidad);
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message, MessageType.Error);
            }
        }

        protected void gvAlumnos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string comando = e.CommandName;

            var id = int.Parse(e.CommandArgument.ToString());
            if (comando == "VerExpediente")
            {
                InfoAlumnoUsuario vExp = new InfoAlumnoUsuario();
                vExp.AlumnoID = id;
                verExpediente = vExp;
                selecciones = InterfaceToFiltroAlumnoUsuario();
                Response.Redirect("~/Pages/DetalleExpedienteAlumnoUniversidad.aspx");
            }
        }

        protected void gvAlumnos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow) 
            {
                TableCell statusCell = e.Row.Cells[2];
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
        #region Metodo Auxiliares
        private void LoadAreasConocimiento()
        {
            ddlAreasConocimiento.DataSource = GetAreasConocimientoAlumno();
            ddlAreasConocimiento.DataValueField = "ClasificadorID";
            ddlAreasConocimiento.DataTextField = "Nombre";
            ddlAreasConocimiento.DataBind();
            ddlAreasConocimiento.Items.Insert(0, new ListItem("Seleccionar", ""));
        }

        private DataSet GetAreasConocimientoAlumno()
        {
            ArrayList arrAreaConocimiento = new ArrayList();
            Escuela escuela = userSession.CurrentEscuela;
            CicloEscolar cicloEscolar = userSession.CurrentCicloEscolar;
            Contrato contrato = userSession.Contrato;

            ContratoCtrl contratoCtrl = new ContratoCtrl();
            List<PruebaContrato> pruebas = contratoCtrl.RetrievePruebasAsignadoContrato(dctx, contrato, new CicloContrato { CicloEscolar = cicloEscolar });
            PruebaContrato pruebaPivoteContrato = pruebas.FirstOrDefault(item => item.TipoPruebaContrato == ETipoPruebaContrato.Pivote);

            var pruebaDinamica = (PruebaDinamica)pruebaPivoteContrato.Prueba;
            PruebaDinamica pruebaDinamicaModelo = pruebaDinamicaCtrl.RetrieveComplete(dctx, pruebaDinamica, false);
            DataSet DsClasificadores = modeloCtrl.RetrieveClasificador(dctx, new Clasificador { Activo = true }, new ModeloDinamico { ModeloID = pruebaDinamicaModelo.Modelo.ModeloID });
            return DsClasificadores;
        }

        private void LoadEstado()
        {
            ddlSearchEstado.DataSource = GetEstado();
            ddlSearchEstado.DataValueField = "EstadoID";
            ddlSearchEstado.DataTextField = "Nombre";
            ddlSearchEstado.DataBind();
            ddlSearchEstado.Items.Insert(0, new ListItem("Seleccionar", ""));
        }

        private DataSet GetEstado()
        {
            DataSet ds = estadoCtrl.Retrieve(dctx, new Estado());
            return ds;
        }

        private void LoadAlumnosUsuario(Universidad universidad)
        {
            List<InfoAlumnoUsuario> getUsuario = getDataAlumnoUsuarioList().GroupBy(test => test.AlumnoID).Select(grp => grp.First()).ToList();
            List<InfoAlumnoUsuario> relacionados = new List<InfoAlumnoUsuario>();
            foreach (InfoAlumnoUsuario item in getUsuario)
            {
                if (universidad.Alumnos.FirstOrDefault(x => x.AlumnoID == item.AlumnoID) != null) 
                {
                    if (item.DatosCompletos == true)
                    {
                        item.Nombre = item.Nombre + " " + item.PrimerApellido + " " + item.SegundoApellido;
                        relacionados.Add(item);
                    }
                }
            }
            ListAlumno = relacionados;
            LlenarAlumnoUniversidad(ListAlumno);
        }

        private void LlenarAlumnoUniversidad(List<InfoAlumnoUsuario> lista) 
        {
            gvAlumnos.DataSource = lista.ToList();
            gvAlumnos.DataBind();
        }

        private List<InfoAlumnoUsuario> getDataAlumnoUsuarioList()
        {
            InfoAlumnoUsuario infoAlumno = InterfaceToFiltroAlumnoUsuario();
            List<InfoAlumnoUsuario> listaAlumnoUsuario = infoAlumnoUsuarioCtrl.Retrieve(dctx, infoAlumno).Distinct().ToList();
            
            return listaAlumnoUsuario;
        }

        public InfoAlumnoUsuario InterfaceToFiltroAlumnoUsuario()
        {
            InfoAlumnoUsuario info = new InfoAlumnoUsuario();
            info.clasificador = new Clasificador();
            info.estado = new Estado();
            if (!String.IsNullOrEmpty(ddlAreasConocimiento.SelectedValue))
                info.clasificador.ClasificadorID = int.Parse(ddlAreasConocimiento.SelectedValue);
            if (!String.IsNullOrEmpty(ddlSearchEstado.SelectedValue))
                info.estado.EstadoID = int.Parse(ddlSearchEstado.SelectedValue);
            string escuelaProcedencia = txtEscuelaSearch.Text.Trim();
            if (!String.IsNullOrEmpty(escuelaProcedencia))
            {
                info.Escuela = escuelaProcedencia;
            }
            if (!String.IsNullOrEmpty(ddlSearchNivel.SelectedValue))
                info.Grado = (EGrado)Convert.ChangeType(ddlSearchNivel.SelectedValue,typeof(byte));
            return info;
        }

        private void RestablecerEstados(InfoAlumnoUsuario infoSessionSelected) 
        {
            ddlAreasConocimiento.SelectedValue = infoSessionSelected.clasificador.ClasificadorID != null ? infoSessionSelected.clasificador.ClasificadorID.ToString() : null;
            ddlSearchNivel.SelectedValue = infoSessionSelected.Grado != null ? ((byte)infoSessionSelected.Grado).ToString() : null;
            txtEscuelaSearch.Text = infoSessionSelected.Escuela;
            ddlSearchEstado.SelectedValue = infoSessionSelected.estado.EstadoID != null ? infoSessionSelected.estado.EstadoID.ToString() : null;
        }
        #endregion

        #region Autorizacion de la pagina
        protected override void DisplayCreateAction()
        {
            throw new NotImplementedException();
        }
        protected override void DisplayReadAction()
        {
            gvAlumnos.Visible = true;
        }
        protected override void DisplayUpdateAction()
        {
            throw new NotImplementedException();
        }
        protected override void DisplayDeleteAction()
        {
            throw new NotImplementedException();
        }
        protected override void AuthorizeUser()
        {
            // ejemplifanco la ejecucion
            List<Permiso> permisos = userSession.CurrentPrivilegiosUniversidad.GetPermisos();
            bool acceso = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.PUACCESOXPEDIENTE) != null;
            bool lectura = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.PUACCESOXPEDIENTE) != null;

            if (!acceso)
                redirector.GoToHomePage(true);
            if (lectura)
                DisplayReadAction();
        }
        #endregion

        #region Message Showing
        /// <summary>
        /// Desplega el mensaje de error/advertencia/informacion en UI
        /// </summary>
        /// <param name="message"> Mensaje a desplegar </param>
        /// <param name="messageType"> Tipo de mensaje </param>
        private void ShowMessage(string message, MessageType messageType) 
        {
            string type = string.Empty;
            switch (messageType)
            {
                case MessageType.Error:
                    type = "1";
                    break;
                case MessageType.Information:
                    type = "3";
                    break;
                case MessageType.Warning:
                    type = "2";
                    break;
            }
            ShowMessage(message, type);
        }

        /// <summary>
        /// Desplega el mensaje de error/advertencia/informacion en la UI
        /// </summary>
        /// <param name="message"> Mensaje a desplegar </param>
        /// <param name="typeNotification"> 1:Error, 2:Advertencia, 3:Informacion</param>
        private void ShowMessage(string message, string typeNotification) 
        {
            // Se ubican los controles que menajan el desplegado de error/advertencia/informacion
            if (Page.Master == null) return;
            Control m = Page.Master.FindControl("hdnLastMessage");
            Control t = Page.Master.FindControl("hdnShowMessage");

            if (m == null)
                throw new Exception("No se pudo desplegar correctamente el error.\nNo se encontró el control 'hdnLastMessage' en la MasterPage.\nEl error original es:\n" + message);
            if(t==null)
                throw new Exception("No se pudo desplegar correctamente el error.\nNo se encontró el control 'hdnShowMessage' en la MasterPage.\nEl error original es:\n" + message);
            if (m.GetType() != typeof(HiddenField) || t.GetType() != typeof(HiddenField))
                throw new Exception("No se pudo desplegar correctamente el error.\nAlguno de los controles de la MasterPage para el manejo de errores no es HiddenField.\nEl error original es:\n" + message);

            //Si el HiddenField del mensaje de error ya tiene un mensaje guardado, se da un 'enter' y se concatena el nuevo mensaje (errores acumulados)
            //En caso contrario, se pone el encabezado y se concatena el nuevo mensaje
            if (((HiddenField)m).Value != null && ((HiddenField)m).Value.Trim().CompareTo("") != 0)
                ((HiddenField)m).Value += "<br />";


            ((HiddenField)m).Value += message.Replace("\n", "<br />");
            ((HiddenField)t).Value = typeNotification;
        }
        #endregion

        
    }
}