using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.CentroEducativo.BO;
using POV.CentroEducativo.Service;
using POV.Comun.BO;
using POV.Comun.Service;
using POV.Localizacion.BO;
using POV.Localizacion.Service;
using POV.Operaciones.Service;
using POV.Web.PortalOperaciones.Helper;
using POV.Logger.Service;
using POV.Licencias.BO;
using POV.Licencias.Service;
using POV.Seguridad.BO;
using POV.Web.PortalOperaciones.AppCode.Page;

namespace POV.Web.PortalOperaciones.Escuelas
{
    public partial class RegistrarEscuela : PageBase
    {
        private EstadoCtrl estadoCtrl;
        private PaisCtrl paisCtrl;
        private CargarEscuelaCtrl cargarEscuelaCtrl;
        private ContratoCtrl contratoCtrl;
        private LicenciaEscuelaCtrl licenciaEscuelaCtrl;
        private CicloEscolarCtrl cicloEscolarCtrl;
        private CicloContratoCtrl cicloContratoCtrl;
        private ModuloFuncionalCtrl moduloFuncionalCtrl;

        #region *** propiedades de clase ***
        private DataTable SS_ResultadoCarga
        {
            get { return (DataTable)this.Session["ResultadoCargaEscuelas"]; }
            set { this.Session["ResultadoCargaEscuelas"] = value; }
        }

        private DataTable SS_ListadoCarga
        {
            get { return (DataTable)this.Session["ListadoCargaEscuelas"]; }
            set { this.Session["ListadoCargaEscuelas"] = value; }
        }

        private Pais SS_PaisCarga
        {
            get { return (Pais)this.Session["PaisCarga"]; }
            set { this.Session["PaisCarga"] = value; }
        }

        private Estado SS_EstadoCarga
        {
            get { return (Estado)this.Session["EstadoCarga"]; }
            set { this.Session["EstadoCarga"] = value; }
        }

        private Contrato SS_ContratoCarga
        {
            get { return (Contrato)this.Session["ContratoCarga"]; }
            set { this.Session["ContratoCarga"] = value; }
        }
        #endregion

        public RegistrarEscuela()
        {
            paisCtrl = new PaisCtrl();
            estadoCtrl = new EstadoCtrl();
            cargarEscuelaCtrl = new CargarEscuelaCtrl();
            contratoCtrl = new ContratoCtrl();
            licenciaEscuelaCtrl = new LicenciaEscuelaCtrl();
            cicloContratoCtrl = new CicloContratoCtrl();
            cicloEscolarCtrl = new CicloEscolarCtrl();
            moduloFuncionalCtrl = new ModuloFuncionalCtrl();
            if (!string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["POVUrlImgNatware"]))
                cargarEscuelaCtrl.UrlImgNatware =
                    System.Configuration.ConfigurationManager.AppSettings["POVUrlImgNatware"];

            if (!string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["POVUrlPortalAdministrativo"]))
                cargarEscuelaCtrl.UrlPortalAdministrativo =
                    System.Configuration.ConfigurationManager.AppSettings["POVUrlPortalAdministrativo"];
        }

        #region *** eventos de pagina ***
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            
            LoadContratos();
            LoadModulosFuncionales();
            if (cbContrato.Items.Count == 1)
                ShowMessage("No existen contratos en el sistema.", MessageType.Information);

        }

        protected void cbContrato_SelectedIndexChanged(object sender, EventArgs e)
        {
            Contrato contrato = GetContratoFromUI();
            LoadInfoContrato(contrato);
            
        }

        

        protected void cbCiclo_SelectedIndexChanged(object sender, EventArgs e)
        {
            CicloContrato ciclo = GetCicloEscolarFromUI();
            Contrato contrato = GetContratoFromUI();
            if (contrato != null && contrato.ContratoID != null && ciclo.CicloContratoID != null)
            {
                ciclo = cicloContratoCtrl.RetrieveComplete(ConnectionHlp.Default.Connection, contrato, ciclo);
                TxtInicioCiclo.Text = String.Format("{0:dd/MM/yyyy}", ciclo.CicloEscolar.InicioCiclo);
                TxtFinCiclo.Text = String.Format("{0:dd/MM/yyyy}", ciclo.CicloEscolar.FinCiclo);
                TxtDescripcionCiclo.Text = ciclo.CicloEscolar.Descripcion;
                TxtPais.Text = ciclo.CicloEscolar.UbicacionID.Pais.Nombre;
                TxtEstado.Text = ciclo.CicloEscolar.UbicacionID.Estado.Nombre;
            }
        }

        protected void btnCargar_OnClick(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(fuArchivoEscuelas.PostedFile.FileName))
                this.ShowMessage("Es requerido seleccionar la ubicación del archivo a cargar.", MessageType.Warning);

            if (!fuArchivoEscuelas.HasFile || !fuArchivoEscuelas.FileName.EndsWith("xls", true, CultureInfo.InvariantCulture))
                this.ShowMessage("Es requerido seleccionar un archivo con extensión xls.", MessageType.Warning);

            

            if (this.cbCicloEscolar.SelectedValue == "0")
                this.ShowMessage("Es requerido seleccionar un Ciclo Escolar", MessageType.Warning);
            if (this.cbContrato.SelectedValue == "")
                this.ShowMessage("Es requerido seleccionar un contrato", MessageType.Warning);



            if (this.ExisteError())
                return;

            //CicloEscolar cicloEscolar = cicloEscolarCtrl.LastDataRowToCicloEscolar(cicloEscolarCtrl.Retrieve(ConnectionHlp.Default.Connection, GetCicloEscolarFromUI()));
            Contrato contrato = contratoCtrl.LastDataRowToContrato(contratoCtrl.Retrieve(ConnectionHlp.Default.Connection, GetContratoFromUI()));
            CicloContrato ciclo = cicloContratoCtrl.RetrieveComplete(ConnectionHlp.Default.Connection, contrato, GetCicloEscolarFromUI());
            if (ciclo.CicloEscolar.InicioCiclo < contrato.InicioContrato)
                this.ShowMessage("El inicio del ciclo debe ser mayor al inicio del contrato", MessageType.Warning);
            if (ciclo.CicloEscolar.FinCiclo > contrato.FinContrato)
                this.ShowMessage("El fin del ciclo debe ser menor al fin del contrato", MessageType.Warning);
            if (this.ExisteError())
                return;

            string rutaArchivo = "";
            try
            {
                List<ModuloFuncional> modulosFuncional = GetModulosFuncionalesSeleccionadosFromUI();
                FileManagerCtrl file = new FileManagerCtrl();
                file.CreateFolder(Server.MapPath(@"~/Content/Temp/"));
                rutaArchivo = MapPath("~/Content/Temp/") + string.Format("{0}{1}", DateTime.Now.Ticks, fuArchivoEscuelas.FileName);
                fuArchivoEscuelas.SaveAs(rutaArchivo);

                IDataContext dctxConexionTran = new DataContext((new DataProviderFactory()).GetDataProvider("POV"));
                this.SS_ResultadoCarga = cargarEscuelaCtrl.CargarArchivoPedido(dctxConexionTran, rutaArchivo, ciclo.CicloEscolar.UbicacionID.Pais, ciclo.CicloEscolar.UbicacionID.Estado, ciclo.CicloEscolar, contrato, modulosFuncional);
                this.SS_ListadoCarga = cargarEscuelaCtrl.DsEscuelas.Tables[0];

                this.SS_PaisCarga = ciclo.CicloEscolar.UbicacionID.Pais;
                this.SS_EstadoCarga = ciclo.CicloEscolar.UbicacionID.Estado;
                this.SS_ContratoCarga = contrato;

                this.Response.Redirect("~/Reportes/CargaEscuelasReport.aspx");
            }
            catch (ThreadAbortException tae)
            {
            }
            catch (Exception ex)
            {
                LoggerHlp.Default.Error(this, ex);
                this.ShowMessage(ex.Message, MessageType.Error);
            }
            finally
            {
                try
                {
                    if (rutaArchivo != string.Empty && File.Exists(rutaArchivo))
                        File.Delete(rutaArchivo);
                }
                catch
                {
                    ;
                }
            }

        }
        #endregion

        #region *** validaciones ***

        #endregion

        #region *** Data to UserInterface ***
        private void LoadContratos()
        {
            cbContrato.Items.Clear();

            cbContrato.Items.Add(new ListItem("Seleccionar", ""));

            DataSet ds = contratoCtrl.Retrieve(ConnectionHlp.Default.Connection, new Contrato { Estatus = true });
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbContrato.DataSource = ds;
                cbContrato.DataTextField = "Clave";
                cbContrato.DataValueField = "ContratoID";
            }

            cbContrato.DataBind();
        }
        
        private void LoadCicloEscolar(Contrato contrato)
        {
            cbCicloEscolar.Items.Clear();
            cbCicloEscolar.Items.Add(new ListItem("Seleccionar", "0"));

            if (contrato != null && contrato.ContratoID != null)
            {
                List<CicloContrato> ciclosContrato = cicloContratoCtrl.RetrieveListCicloContratoComplete(ConnectionHlp.Default.Connection, contrato, true);
                

                    foreach(CicloContrato ciclo in ciclosContrato)
                    {
                        cbCicloEscolar.Items.Add(
                            new ListItem(ciclo.CicloEscolar.Titulo,ciclo.CicloContratoID.ToString()));
                    }
                
            }

            cbCicloEscolar.DataBind();
            cbCicloEscolar.SelectedValue = "0";
        }

        private void LoadInfoContrato(Contrato contrato)
        {
            if (contrato.ContratoID != null)
            {
                contrato = contratoCtrl.LastDataRowToContrato(contratoCtrl.Retrieve(ConnectionHlp.Default.Connection, contrato));

                if (contrato.LicenciasLimitadas.Value)
                {
                    List<LicenciaEscuela> licencias = licenciaEscuelaCtrl.RetriveLicenciaEscuela(ConnectionHlp.Default.Connection, new LicenciaEscuela { Contrato = contrato });

                    int asignadas = licencias.Sum(item => item.LicenciasAsignadas());

                    int disponibles = contrato.NumeroLicencias.Value - asignadas;
                    TxtLicenciasIlimitadas.Text = "No";
                    TxtNumeroLicencias.Text = contrato.NumeroLicencias.ToString();
                    TxtLicenciasDisponibles.Text = disponibles.ToString();
                }
                else
                {
                    TxtNumeroLicencias.Text = "No aplica";
                    TxtLicenciasDisponibles.Text = "No aplica";
                    TxtLicenciasIlimitadas.Text = "Sí";
                }
                TxtNombreRepresentante.Text = contrato.Cliente.Representante;
                TxtNombreCliente.Text = contrato.Cliente.Nombre;
                TxtInicioContrato.Text = String.Format("{0:dd/MM/yyyy}", contrato.InicioContrato);
                TxtFinContrato.Text = String.Format("{0:dd/MM/yyyy}", contrato.FinContrato);

                LoadCicloEscolar(contrato);
            }
            else
            {
                TxtNombreRepresentante.Text = "";
                TxtNombreCliente.Text = "";
                TxtLicenciasIlimitadas.Text = "";
                TxtNumeroLicencias.Text = "";
                TxtLicenciasDisponibles.Text = "";
                TxtInicioContrato.Text = "";
                TxtFinContrato.Text = "";
                cbCicloEscolar.Items.Clear();
            }
        }

        private void LoadModulosFuncionales()
        {
            DataSet ds = moduloFuncionalCtrl.Retrieve(ConnectionHlp.Default.Connection, new ModuloFuncional());
            grdModulosFuncionales.DataSource = ds;
            grdModulosFuncionales.DataBind();
        }
        #endregion

        #region *** UserInterface to Data ***
        private Contrato GetContratoFromUI()
        {
            Contrato contrato = new Contrato();

            long contratoID = 0;

            if (long.TryParse(cbContrato.SelectedValue, out contratoID))
            {
                contrato.ContratoID = contratoID;
                contrato.Clave = cbContrato.SelectedItem.Text;
            }
            return contrato;
        }

        
        public CicloContrato GetCicloEscolarFromUI()
        {
            CicloContrato cicloContrato = new CicloContrato();
            int cicloEscolarId = 0;
            int.TryParse(cbCicloEscolar.SelectedValue, out cicloEscolarId);
            if (cicloEscolarId > 0)
            {
                cicloContrato.CicloContratoID = cicloEscolarId;
            }
            return cicloContrato;
        }

        private List<ModuloFuncional> GetModulosFuncionalesSeleccionadosFromUI()
        {
            List<ModuloFuncional> modulos = new List<ModuloFuncional>();

            modulos = (from row in grdModulosFuncionales.Rows.Cast<GridViewRow>()
                       let checado = (CheckBox)row.FindControl("cbSeleccionado")
                       where checado.Checked
                       select new ModuloFuncional
                       {
                           ModuloFuncionalId = Convert.ToInt32(((HiddenField)row.FindControl("hdnModuloId")).Value)
                       }
                       ).ToList();

            return modulos;

        }
        #endregion

        #region *** metodos auxiliares ***

        #endregion

        #region *****Message  Showing*****
        /// <summary>
        /// Desplega el mensaje de error/advertencia/información en la UI
        /// </summary>
        /// <param name="message">Mensaje a desplegar</param>
        /// <param name="messageType">Tipo de mensaje</param>
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
        /// Desplega el mensaje de error/advertencia/información en la UI
        /// </summary>
        /// <param name="message">Mensaje a desplegar</param>
        /// <param name="typeNotification">1: Error, 2: Advertencia, 3: Información</param>
        private void ShowMessage(string message, string typeNotification)
        {
            //Se ubican los controles que manejan el desplegado de error/advertencia/información
            if (Page.Master == null) return;
            Control m = Page.Master.FindControl("hdnLastMessage");
            Control t = Page.Master.FindControl("hdnShowMessage");

            if (m == null)
                throw new Exception("No se pudo desplegar correctamente el error.\nNo se encontró el control 'hdnLastMessage' en la MasterPage.\nEl error original es:\n" + message);
            if (t == null)
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

        private bool ExisteError()
        {
            //Se ubican los controles que manejan el desplegado de error/advertencia/información
            if (Page.Master == null)
                return false;

            Control m = Page.Master.FindControl("hdnLastMessage");
            Control t = Page.Master.FindControl("hdnShowMessage");

            if (m == null)
                throw new Exception("No se pudo desplegar correctamente el error.\nNo se encontró el control 'hdnLastMessage' en la MasterPage");
            if (t == null)
                throw new Exception("No se pudo desplegar correctamente el error.\nNo se encontró el control 'hdnShowMessage' en la MasterPage.");

            if (m.GetType() != typeof(HiddenField) || t.GetType() != typeof(HiddenField))
                throw new Exception("No se pudo desplegar correctamente el error.\nAlguno de los controles de la MasterPage para el manejo de errores no es HiddenField.");

            return ((HiddenField)m).Value.Trim().Length > 0;
        }
        #endregion

        #region AUTORIZACION DE LA PAGINA

        protected override void AuthorizeUser()
        {
            //ejemplificando la ejecucion
            List<Permiso> permisos = userSession.CurrentPrivilegios.GetPermisos();

            bool acceso = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POACCESOESCUELAS) != null;
            bool creacion = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POINSERTARESCUELAS) != null;

            if (!acceso)
                redirector.GoToHomePage(true);
            if (!creacion)
                redirector.GoToHomePage(true);
        }
        #endregion
    }
}