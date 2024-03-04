using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Globalization;
using System.Web.UI.WebControls;
using Framework.Base.Exceptions;
using POV.CentroEducativo.BO;
using POV.CentroEducativo.Service;
using POV.Core.Operaciones.Implements;
using POV.Core.Operaciones.Interfaces;
using POV.Localizacion.BO;
using POV.Localizacion.Service;
using POV.Comun.BO;
using POV.Comun.Service;
using POV.Web.PortalOperaciones.Helper;
using POV.Licencias.BO;
using POV.Licencias.Service;
using POV.Seguridad.BO;
using POV.Seguridad.Service;
using POV.Seguridad.Utils;
using Framework.Base.DataAccess;
using GP.SocialEngine.BO;
using GP.SocialEngine.Service;
using POV.Operaciones.Service;
using POV.Web.PortalOperaciones.AppCode.Page;

namespace POV.Web.PortalOperaciones.Escuelas
{
    public partial class NuevaEscuela : PageBase
    {
        private EscuelaCtrl escuelaCtrl;
        private UbicacionCtrl ubicacionCtrl;
        private PaisCtrl paisCtrl;
        private EstadoCtrl estadoCtrl;
        private CiudadCtrl ciudadCtrl;
        private LocalidadCtrl localidadCtrl;
        private ZonaCtrl zonaCtrl;
        private TipoServicioCtrl tipoServicioCtrl;
        private NivelEducativoCtrl nivelEducativoCtrl;
        private DirectorCtrl directorCtrl;
        private TipoNivelEducativoCtrl tipoNivelEducativoCtrl;
        private CicloContratoCtrl cicloContratoCtrl;
        private ContratoCtrl contratoCtrl;
        private LicenciaEscuelaCtrl licenciaEscuelaCtrl;
        private CicloEscolarCtrl cicloEscolarCtrl;
        private ModuloFuncionalCtrl moduloFuncionalCtrl;

        #region *** propiedades de clase ***
        private DataSet DSDirectores
        {
            get { return Session["directores"] != null ? (DataSet)Session["directores"] : null; }
            set { Session["directores"] = value; }
        }
        private Director SelectedDirector
        {
            get { return Session["selecteddirector"] != null ? (Director)Session["selecteddirector"] : null; }
            set { Session["selecteddirector"] = value; }
        }
        #endregion

        public NuevaEscuela()
        {
            escuelaCtrl = new EscuelaCtrl();
            ubicacionCtrl = new UbicacionCtrl();
            paisCtrl = new PaisCtrl();
            estadoCtrl = new EstadoCtrl();
            ciudadCtrl = new CiudadCtrl();
            localidadCtrl = new LocalidadCtrl();
            zonaCtrl = new ZonaCtrl();
            ubicacionCtrl = new UbicacionCtrl();
            tipoServicioCtrl = new TipoServicioCtrl();
            nivelEducativoCtrl = new NivelEducativoCtrl();
            directorCtrl = new DirectorCtrl();
            tipoNivelEducativoCtrl = new TipoNivelEducativoCtrl();
            contratoCtrl = new ContratoCtrl();
            licenciaEscuelaCtrl = new LicenciaEscuelaCtrl();
            cicloEscolarCtrl = new CicloEscolarCtrl();
            cicloContratoCtrl = new CicloContratoCtrl();
            moduloFuncionalCtrl = new ModuloFuncionalCtrl();
        }

        #region *** eventos de pagina ***
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!userSession.IsLogin())
                    redirector.GoToLoginPage(true);

                if (!IsPostBack)
                {
                    SelectedDirector = null;
                    DSDirectores = null;

                    LoadTurno();
                    LoadAmbito();
                    LoadControl();
                    LoadPaises(new Ubicacion { Pais = new Pais() });
                    LoadTipoNivelEducativo(new TipoNivelEducativo());
                    //Cargar directores del sistema
                    LoadDirectores(new Director());



                    LoadContratos();
                    LoadModulosFuncionales();
                    if (cbContrato.Items.Count == 1)
                        ShowMessage("No existen contratos en el sistema.", MessageType.Information);

                }
            }
            catch (Exception ex)
            {

                POV.Logger.Service.LoggerHlp.Default.Error(this, ex);
            }
        }

        protected void cbContrato_SelectedIndexChanged(object sender, EventArgs e)
        {
            Contrato contrato = GetContratoFromUI();
            LoadInfoContrato(contrato);

        }

        protected void CbTipoNivelEducativo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (CbTipoNivelEducativo.SelectedIndex > 0)
                {
                    LoadNivelEducativo(new NivelEducativo { TipoNivelEducativoID = new TipoNivelEducativo { TipoNivelEducativoID = int.Parse(CbTipoNivelEducativo.SelectedItem.Value) } });
                }
            }
            catch (Exception ex)
            {

                POV.Logger.Service.LoggerHlp.Default.Error(this, ex);
            }
        }
        protected void CbNivel_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (CbNivel.SelectedIndex > 0)
                {
                    LoadTipoServicio(new TipoServicio { NivelEducativoID = new NivelEducativo { NivelEducativoID = int.Parse(CbNivel.SelectedItem.Value) } });
                }
            }
            catch (Exception ex)
            {

                POV.Logger.Service.LoggerHlp.Default.Error(this, ex);
            }
        }
        protected void btnConsultar_Click(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {

                POV.Logger.Service.LoggerHlp.Default.Error(this, ex);
            }
        }
        protected void btnConsultarDirector_Click(object sender, EventArgs e)
        {
            try
            {
                Director director = DirectorUserInterfaceToData();
                LoadDirectores(director);
            }
            catch (Exception ex)
            {

                POV.Logger.Service.LoggerHlp.Default.Error(this, ex);
            }
        }

        protected void CbPais_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (CbPais.SelectedIndex > 0)
                {
                    LoadEstados(new Ubicacion { Estado = new Estado { Pais = new Pais { PaisID = int.Parse(CbPais.SelectedItem.Value) } } });
                }
                else
                {
                    CbEstado.ClearSelection();
                    CbMunicipio.ClearSelection();
                    CbLocalidad.ClearSelection();
                    CbZona.ClearSelection();
                }

            }
            catch (Exception ex)
            {

                POV.Logger.Service.LoggerHlp.Default.Error(this, ex);
            }
        }
        protected void CbEstado_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (CbEstado.SelectedIndex > 0)
                {
                    LoadCiudades(new Ubicacion { Ciudad = new Ciudad { Estado = new Estado { EstadoID = int.Parse(CbEstado.SelectedItem.Value) } } });

                    Zona zona = new Zona();
                    zona.UbicacionID = new Ubicacion();
                    zona.UbicacionID.Pais = new Pais { PaisID = int.Parse(CbPais.SelectedItem.Value) };
                    zona.UbicacionID.Estado = new Estado { EstadoID = int.Parse(CbEstado.SelectedItem.Value) };


                    //Consultar Ubicación
                    DataSet ds = ubicacionCtrl.RetrieveExacto(ConnectionHlp.Default.Connection, zona.UbicacionID);
                    if (ds.Tables[0].Rows.Count == 1)
                    {
                        zona.UbicacionID = ubicacionCtrl.LastDataRowToUbicacion(ds);
                        LoadZonas(zona);
                    }


                }
                else
                {
                    CbMunicipio.ClearSelection();
                    CbLocalidad.ClearSelection();
                    CbZona.ClearSelection();
                }
            }
            catch (Exception ex)
            {
                POV.Logger.Service.LoggerHlp.Default.Error(this, ex);
            }
        }
        protected void CbMunicipio_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (CbMunicipio.SelectedIndex > 0)
                    LoadLocalidades(new Ubicacion { Localidad = new Localidad { Ciudad = new Ciudad { CiudadID = int.Parse(CbMunicipio.SelectedItem.Value) } } });
                else
                {
                    CbLocalidad.ClearSelection();
                }

            }
            catch (Exception ex)
            {

                POV.Logger.Service.LoggerHlp.Default.Error(this, ex);
            }
        }
        protected void CbLocalidad_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {

                POV.Logger.Service.LoggerHlp.Default.Error(this, ex);
            }
        }

        protected void grdDirectores_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                switch (e.CommandName)
                {
                    case "agregar":
                        Director aDirector = new Director { DirectorID = int.Parse((string)e.CommandArgument) };
                        AddDirector(aDirector);
                        break;
                }
            }
            catch (Exception ex)
            {

                POV.Logger.Service.LoggerHlp.Default.Error(this, ex);
            }
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

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                DoInsert();


            }
            catch (Exception ex)
            {

                POV.Logger.Service.LoggerHlp.Default.Error(this, ex);
            }

        }
        #endregion

        #region *** validaciones ***
        private void ValidateData()
        {
            string sError = string.Empty;
            //Campos requeridos
            if (txtClaveEscuela.Text.Trim().Length <= 0)
                sError += " ,Clave Escuela";

            if (txtNombreEscuela.Text.Trim().Length <= 0)
                sError += " ,Nombre Escuela";

            if (txtNombreDirector.Text.Trim().Length <= 0 || txtCurp.Text.Trim().Length <= 0)
                sError += " ,Director";

            if ((CbPais.SelectedIndex <= 0)
                || (CbEstado.SelectedIndex <= 0)
                || (CbMunicipio.SelectedIndex <= 0)
                || (CbLocalidad.SelectedIndex <= 0))
                sError += " ,Ubicación";

            if (CbZona.SelectedIndex <= 0)
                sError += " ,Zona";

            if (CbTurno.SelectedIndex <= 0)
                sError += " ,Turno";
            if (CbAmbito.SelectedIndex <= 0)
                sError += " ,Ámbito";

            if (CbControl.SelectedIndex <= 0)
                sError += " ,Control";
            if (GetCicloEscolarFromUI().CicloContratoID == null)
                sError += " ,Ciclo Escolar";

            if (GetContratoFromUI().ContratoID == null)
                sError += " ,Contrato";
            if (sError.Length > 0)
            {
                sError = sError.Substring(2);
                throw new Exception(string.Format("Los siguientes campos son requeridos {0}", sError));
            }

            //longitud
            if (txtClaveEscuela.Text.Trim().Length > 50)
                sError += " ,Clave Escuela";
            if (txtNombreEscuela.Text.Trim().Length > 50)
                sError += " ,Nombre Escuela";

            if (sError.Length > 0)
            {
                throw new Exception(string.Format("Los siguientes campos exceden la longitud permitida {0}", sError));
            }
            //formato

        }
        private void ValidateNoRepetido()
        {
            Escuela escuela = UserInterfaceToData();
            DataSet ds = escuelaCtrl.Retrieve(ConnectionHlp.Default.Connection, new Escuela { Clave = escuela.Clave, Turno = escuela.Turno });
            if (ds.Tables["Escuela"].Rows.Count >= 1)
            {
                Escuela actescuela = escuelaCtrl.LastDataRowToEscuela(ds);
                if ((bool)actescuela.Estatus)
                    throw new Exception("La clave de la escuela ya está registrada para el turno seleccionado, por favor verifique");
            }

        }
        #endregion

        #region *** Data to UserInterface ***

        private void LoadCicloEscolar(Contrato contrato)
        {
            cbCicloEscolar.Items.Clear();
            cbCicloEscolar.Items.Add(new ListItem("Seleccionar", "0"));
            if (contrato != null && contrato.ContratoID != null)
            {
                List<CicloContrato> ciclosContrato = cicloContratoCtrl.RetrieveListCicloContratoComplete(ConnectionHlp.Default.Connection, contrato, true);


                foreach (CicloContrato ciclo in ciclosContrato)
                {
                    cbCicloEscolar.Items.Add(
                        new ListItem(ciclo.CicloEscolar.Titulo, ciclo.CicloContratoID.ToString()));
                }

            }

            cbCicloEscolar.DataBind();
            cbCicloEscolar.SelectedValue = "0";
        }

        private void LoadTurno()
        {
            var valenum = Enum.GetValues(typeof(ETurno));
            foreach (byte en in valenum)
            {
                ETurno turn = (ETurno)en;
                CbTurno.Items.Add(new ListItem(turn.ToString(), en.ToString()));
            }
            CbTurno.Items.Insert(0, new ListItem(" ", "-1"));
        }
        private void LoadAmbito()
        {
            var valenum = Enum.GetValues(typeof(EAmbito));
            foreach (byte en in valenum)
            {
                EAmbito ambito = (EAmbito)en;
                CbAmbito.Items.Add(new ListItem(ambito.ToString(), en.ToString()));
            }
            CbAmbito.Items.Insert(0, new ListItem(" ", "-1"));
        }
        private void LoadControl()
        {
            var enu = Enum.GetValues(typeof(EControl));
            foreach (byte contl in enu)
            {
                EControl control = (EControl)contl;
                CbControl.Items.Add(new ListItem(control.ToString(), contl.ToString()));
            }
            CbControl.Items.Insert(0, new ListItem(" ", "-1"));
        }
        private void LoadTipoNivelEducativo(TipoNivelEducativo tipoNivelEducativo)
        {
            DataSet ds = tipoNivelEducativoCtrl.Retrieve(ConnectionHlp.Default.Connection, tipoNivelEducativo);
            CbTipoNivelEducativo.DataSource = ds;
            CbTipoNivelEducativo.DataValueField = "TipoNivelEducativoID";
            CbTipoNivelEducativo.DataTextField = "Nombre";
            CbTipoNivelEducativo.DataBind();
            CbTipoNivelEducativo.Items.Insert(0, new ListItem(" ", "0"));

        }
        private void LoadNivelEducativo(NivelEducativo nivelEducativo)
        {
            DataSet ds = nivelEducativoCtrl.Retrieve(ConnectionHlp.Default.Connection, nivelEducativo);
            CbNivel.DataSource = ds;
            CbNivel.DataValueField = "NivelEducativoID";
            CbNivel.DataTextField = "Titulo";
            CbNivel.DataBind();
            CbNivel.Items.Insert(0, new ListItem(" ", "0"));
        }
        private void LoadTipoServicio(TipoServicio tipoServicio)
        {
            DataSet ds = tipoServicioCtrl.Retrieve(ConnectionHlp.Default.Connection, tipoServicio);
            CbTipoServicio.DataSource = ds;
            CbTipoServicio.DataValueField = "TipoServicioID";
            CbTipoServicio.DataTextField = "Nombre";
            CbTipoServicio.DataBind();
            CbTipoServicio.Items.Insert(0, new ListItem(" ", "0"));
        }
        private void LoadZonas(Zona zona)
        {
            //Zona

            DataSet ds = zonaCtrl.Retrieve(ConnectionHlp.Default.Connection, zona);
            CbZona.DataSource = ds;
            CbZona.DataValueField = "ZonaID";
            CbZona.DataTextField = "Nombre";
            CbZona.DataBind();
            CbZona.Items.Insert(0, new ListItem(" ", "0"));
        }
        private void LoadDirectores(Director filter)
        {
            if (filter == null)
                return;
            filter.Estatus = true;
            DataSet ds = directorCtrl.Retrieve(ConnectionHlp.Default.Connection, filter);
            ds.Tables[0].Columns.Add(new DataColumn("NombreCompleto", typeof(string)));
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Director director = directorCtrl.DataRowToDirector(dr);
                dr["NombreCompleto"] = string.Format("{0} {1} {2}", director.Nombre, director.PrimerApellido, director.SegundoApellido).Trim();
            }
            DSDirectores = ds;
            grdDirectores.DataSource = DSDirectores;
            grdDirectores.DataBind();


        }

        private void LoadPaises(Ubicacion filter)
        {
            if (filter == null || filter.Pais == null)
                return;
            DataSet ds = paisCtrl.Retrieve(ConnectionHlp.Default.Connection, filter.Pais);
            CbPais.DataSource = ds;
            CbPais.DataValueField = "PaisID";
            CbPais.DataTextField = "Nombre";
            CbPais.DataBind();
            CbPais.Items.Insert(0, new ListItem("", "0"));

        }
        private void LoadEstados(Ubicacion filter)
        {
            if (filter == null || filter.Estado == null)
                return;
            DataSet ds = estadoCtrl.Retrieve(ConnectionHlp.Default.Connection, filter.Estado);
            CbEstado.DataSource = ds;
            CbEstado.DataValueField = "EstadoID";
            CbEstado.DataTextField = "Nombre";
            CbEstado.DataBind();
            CbEstado.Items.Insert(0, new ListItem("", "0"));
        }
        private void LoadCiudades(Ubicacion filter)
        {
            if (filter == null || filter.Ciudad == null)
                return;
            DataSet ds = ciudadCtrl.Retrieve(ConnectionHlp.Default.Connection, filter.Ciudad);
            CbMunicipio.DataSource = ds;
            CbMunicipio.DataValueField = "CiudadID";
            CbMunicipio.DataTextField = "Nombre";
            CbMunicipio.DataBind();
            CbMunicipio.Items.Insert(0, new ListItem("", "0"));
        }
        private void LoadLocalidades(Ubicacion filter)
        {
            if (filter == null || filter.Localidad == null)
                return;
            DataSet ds = localidadCtrl.Retrieve(ConnectionHlp.Default.Connection, filter.Localidad);
            CbLocalidad.DataSource = ds;
            CbLocalidad.DataValueField = "LocalidadID";
            CbLocalidad.DataTextField = "Nombre";
            CbLocalidad.DataBind();
            CbLocalidad.Items.Insert(0, new ListItem("", "0"));
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

        private void LoadModulosFuncionales()
        {
            DataSet ds = moduloFuncionalCtrl.Retrieve(ConnectionHlp.Default.Connection, new ModuloFuncional());
            grdModulosFuncionales.DataSource = ds;
            grdModulosFuncionales.DataBind();
        }
        #endregion

        #region *** UserInterface to Data ***
        private Ubicacion GetUbicacion(Ubicacion ubicacion)
        {
            //Consultar Ubicación
            DataSet ds = ubicacionCtrl.RetrieveExacto(ConnectionHlp.Default.Connection, ubicacion);
            int index = ds.Tables["Ubicacion"].Rows.Count;
            if (index == 1)
                return ubicacionCtrl.LastDataRowToUbicacion(ds);
            return null;
        }

        private Escuela UserInterfaceToData()
        {
            Escuela escuela = new Escuela();
            //Información de la escuela
            escuela.NombreEscuela = txtNombreEscuela.Text.Trim();
            escuela.Clave = txtClaveEscuela.Text.Trim();
            escuela.Turno = CbTurno.SelectedIndex > 0 ? (ETurno?)byte.Parse(CbTurno.SelectedItem.Value) : null;

            escuela.Ambito = CbAmbito.SelectedIndex > 0 ? (EAmbito?)byte.Parse(CbAmbito.SelectedItem.Value) : null;
            escuela.TipoServicio = CbTipoServicio.SelectedIndex > 0 ? new TipoServicio { TipoServicioID = int.Parse(CbTipoServicio.SelectedItem.Value) } : null;
            escuela.Control = CbControl.SelectedIndex > 0 ? (EControl?)byte.Parse(CbControl.SelectedItem.Value) : null;
            escuela.ZonaID = CbZona.SelectedIndex > 0 ? new Zona { ZonaID = int.Parse(CbZona.SelectedItem.Value) } : null;


            //Ubicación
            escuela.Ubicacion = new Ubicacion();
            escuela.Ubicacion.Pais = new Pais { PaisID = CbPais.SelectedIndex > 0 ? int.Parse(CbPais.SelectedItem.Value) : (int?)null };
            escuela.Ubicacion.Estado = new Estado { EstadoID = CbEstado.SelectedIndex > 0 ? int.Parse(CbEstado.SelectedItem.Value) : (int?)null };
            escuela.Ubicacion.Ciudad = new Ciudad { CiudadID = CbMunicipio.SelectedIndex > 0 ? int.Parse(CbMunicipio.SelectedItem.Value) : (int?)null };
            escuela.Ubicacion.Localidad = new Localidad { LocalidadID = CbLocalidad.SelectedIndex > 0 ? int.Parse(CbLocalidad.SelectedItem.Value) : (int?)null };

            //Datos del director
            if (SelectedDirector != null && SelectedDirector.DirectorID != null && SelectedDirector.DirectorID > 0)
                escuela.DirectorID = new Director { DirectorID = SelectedDirector.DirectorID };

            return escuela;
        }
        private Director DirectorUserInterfaceToData()
        {
            Director director = new Director();
            director.Curp = !string.IsNullOrEmpty(txtCurpConsultar.Text.Trim()) ? txtCurpConsultar.Text.Trim() : null;
            director.Nombre = !string.IsNullOrEmpty(txtNombreDirectorConsultar.Text.Trim()) ? txtNombreDirectorConsultar.Text.Trim() : null;
            director.PrimerApellido = !string.IsNullOrEmpty(txtPrimerApellidoConsultar.Text.Trim()) ? txtPrimerApellidoConsultar.Text.Trim() : null;
            director.SegundoApellido = !string.IsNullOrEmpty(txtSegundoApellidoConsultar.Text.Trim()) ? txtSegundoApellidoConsultar.Text.Trim() : null;

            return director;
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
        private void DoInsert()
        {
            IDataContext dctx = ConnectionHlp.Default.Connection;
            object myFirm = new object();
            dctx.OpenConnection(myFirm);
            dctx.BeginTransaction(myFirm);
            try
            {
                

                ValidateData();
                ValidateNoRepetido();
                List<ModuloFuncional> modulos = GetModulosFuncionalesSeleccionadosFromUI();
                Escuela escuela = UserInterfaceToData();
                
                ContratoCtrl contratoCtrl = new ContratoCtrl();

                Contrato cContrato = contratoCtrl.LastDataRowToContrato(contratoCtrl.Retrieve(dctx, GetContratoFromUI()));

                CicloContrato ciclo = cicloContratoCtrl.RetrieveComplete(ConnectionHlp.Default.Connection, cContrato, GetCicloEscolarFromUI());


                if (ciclo.CicloEscolar.InicioCiclo < cContrato.InicioContrato)
                    throw new Exception("El inicio del ciclo debe ser mayor al inicio del contrato");
                if (ciclo.CicloEscolar.FinCiclo > cContrato.FinContrato)
                    throw new Exception("El fin del ciclo debe ser menor al fin del contrato");
                #region insertar ubicacion
                Ubicacion ubicacion = new Ubicacion();
                DataSet dsUbicacion = ubicacionCtrl.RetrieveExacto(dctx, escuela.Ubicacion);
                int index = dsUbicacion.Tables["Ubicacion"].Rows.Count;
                if (index == 1)
                    ubicacion = ubicacionCtrl.LastDataRowToUbicacion(dsUbicacion);
                //si no existe se inserta la ubicacion
                if (ubicacion.UbicacionID == null)
                {
                    escuela.Ubicacion.FechaRegistro = DateTime.Now;
                    ubicacionCtrl.Insert(dctx, escuela.Ubicacion);
                    escuela.Ubicacion = ubicacionCtrl.LastDataRowToUbicacion(ubicacionCtrl.RetrieveExacto(dctx, escuela.Ubicacion));
                }
                else
                    escuela.Ubicacion = ubicacion;
                #endregion

                DataSet dsEscuela = escuelaCtrl.Retrieve(dctx, new Escuela { Clave = escuela.Clave, Turno = escuela.Turno, ToShortTurno = escuela.ToShortTurno });
                if (dsEscuela.Tables[0].Rows.Count < 1) //no existe la escuela
                {
                    escuela.FechaRegistro = DateTime.Now;
                    escuela.Estatus = true;
                    escuelaCtrl.Insert(dctx, escuela);
                    escuela = escuelaCtrl.LastDataRowToEscuela(escuelaCtrl.Retrieve(dctx, escuela)); // obtenemos Escuela

                }
                else
                {
                    //Recuperar escuela y actualizar
                    Escuela anterior = escuelaCtrl.DataRowToEscuela(dsEscuela.Tables["Escuela"].Rows[index - 1]);
                    escuela.FechaRegistro = anterior.FechaRegistro;
                    escuela.EscuelaID = anterior.EscuelaID;
                    escuela.Estatus = true;
                    escuelaCtrl.Update(dctx, escuela, anterior);
                }

                escuela.DirectorID = directorCtrl.LastDataRowToDirector(directorCtrl.Retrieve(dctx, escuela.DirectorID));

                LicenciaEscuela licenciaEscuela = new LicenciaEscuela();
                licenciaEscuela.CicloEscolar = ciclo.CicloEscolar;
                licenciaEscuela.Escuela = escuela;
                licenciaEscuela.Activo = true;


                DataSet dsLicenciaEscuela = licenciaEscuelaCtrl.Retrieve(dctx, licenciaEscuela);
                bool NoLicenciaEscuela = dsLicenciaEscuela.Tables[0].Rows.Count == 0;

                if (!NoLicenciaEscuela)
                {
                    throw new Exception("La escuela ya esta registrada en el ciclo " + licenciaEscuela.CicloEscolar.InicioCiclo.Value.Year.ToString(CultureInfo.InvariantCulture) + "-" + licenciaEscuela.CicloEscolar.FinCiclo.Value.Year.ToString(CultureInfo.InvariantCulture));
                }

                licenciaEscuela.NumeroLicencias = 0;
                licenciaEscuela.Contrato = cContrato;
                licenciaEscuela.ModulosFuncionales = modulos;
                Usuario usuario = null;
                usuario = licenciaEscuelaCtrl.RetrieveUsuario(dctx, escuela.DirectorID);

                bool envioCorreo = false;
                string pwsTemporal = string.Empty;
                UsuarioCtrl usuarioCtrl = new UsuarioCtrl();
                if (usuario.UsuarioID != null)
                {
                    usuario = usuarioCtrl.LastDataRowToUsuario(usuarioCtrl.Retrieve(dctx, usuario));

                    if (usuario.EsActivo == false)
                    {
                        Usuario original = (Usuario)usuario.Clone();
                        usuario.EsActivo = true;

                        usuarioCtrl.Update(dctx, usuario, original);
                    }
                }
                else
                {
                    usuario.NombreUsuario = usuarioCtrl.GenerarNombreUsuarioUnico(dctx, escuela.DirectorID.Nombre, escuela.DirectorID.PrimerApellido, escuela.DirectorID.FechaNacimiento.Value);
                    usuario.Email = escuela.DirectorID.Correo;
                    pwsTemporal = new PasswordProvider(8).GetNewPassword();
                    usuario.Password = EncryptHash.SHA1encrypt(pwsTemporal);
                    usuario.EsActivo = true;
                    usuario.FechaCreacion = DateTime.Now;
                    usuario.PasswordTemp = true;

                    //Consultar Termino Activo
                    TerminoCtrl terminoCtrl = new TerminoCtrl();
                    DataSet dsTermino = (terminoCtrl.Retrieve(dctx, new Termino { Estatus = true }));

                    usuario.Termino = dsTermino.Tables[0].Rows.Count >= 1
                                          ? terminoCtrl.LastDataRowToTermino(dsTermino)
                                          : new Termino();
                    usuario.AceptoTerminos = false;

                    usuarioCtrl.Insert(dctx, usuario);
                    usuario = usuarioCtrl.LastDataRowToUsuario(usuarioCtrl.Retrieve(dctx, usuario));

                    envioCorreo = true;




                }

                UsuarioPrivilegiosCtrl usuarioPrivilegiosCtrl = new UsuarioPrivilegiosCtrl();

                #region registrar usuario privilegios

                //asignamos el perfil alumno a la lista de privilegios
                Perfil perfil = new Perfil { PerfilID = (int)EPerfil.DIRECTOR };

                List<IPrivilegio> privilegios = new List<IPrivilegio>();
                privilegios.Add(perfil);

                usuarioPrivilegiosCtrl.InsertUsuarioEscolarPrivilegios(dctx, usuario, licenciaEscuela.Escuela, licenciaEscuela.CicloEscolar, privilegios);

                #endregion

                licenciaEscuelaCtrl.Insert(dctx, licenciaEscuela);
                dsLicenciaEscuela = licenciaEscuelaCtrl.Retrieve(dctx, licenciaEscuela);
                licenciaEscuela = licenciaEscuelaCtrl.LastDataRowToLicenciaEscuela(dsLicenciaEscuela);

                licenciaEscuelaCtrl.InsertLicenciaDirector(dctx, licenciaEscuela, escuela.DirectorID, usuario);

                dctx.CommitTransaction(myFirm);

                if (envioCorreo)
                {
                    CargarEscuelaCtrl cargaCtrl = new CargarEscuelaCtrl();
                    cargaCtrl.UrlPortalAdministrativo = System.Configuration.ConfigurationManager.AppSettings["POVUrlPortalAdministrativo"];
                    cargaCtrl.UrlImgNatware = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgNatware"];
                    cargaCtrl.EnviarCorreo(usuario, pwsTemporal, escuela.DirectorID.Clave);
                }
                SelectedDirector = null;
                DSDirectores = null;

                txtRedirect.Value = "BuscarEscuelas.aspx";
                ShowMessage("La escuela y su licencia se han registrado con éxito", MessageType.Information);

            }
            catch (Exception ex)
            {
                dctx.RollbackTransaction(myFirm);
                dctx.CloseConnection(myFirm);
                POV.Logger.Service.LoggerHlp.Default.Error(this, ex);
                this.ShowMessage(ex.Message, MessageType.Error);

            }
            finally
            {
                if (dctx.ConnectionState == ConnectionState.Open)
                    dctx.CloseConnection(myFirm);
            }


        }
        private void DoInsertUbicacionEscuela(Escuela escuela)
        {
            //Consultar Ubicación
            Ubicacion ub = GetUbicacion(escuela.Ubicacion);
            if (ub == null)
            {
                //Insertar Ubicación
                ubicacionCtrl.Insert(ConnectionHlp.Default.Connection, escuela.Ubicacion);
            }
        }
        private void AddDirector(Director director)
        {
            if (director == null || director.DirectorID == null)
                return;
            //Consulta director
            director.Estatus = true;
            DataSet ds = directorCtrl.Retrieve(ConnectionHlp.Default.Connection, director);
            if (ds.Tables["Director"].Rows.Count != 1)
                return;
            SelectedDirector = directorCtrl.LastDataRowToDirector(ds);
            txtCurp.Text = SelectedDirector.Curp;
            txtNombreDirector.Text = (string.Format("{0} {1} {2}", SelectedDirector.Nombre, SelectedDirector.PrimerApellido, SelectedDirector.SegundoApellido)).Trim();

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

        #region Mostrar Mensajes
        /// <summary>
        /// Desplega el mensaje de error/advertencia/información en la UI
        /// </summary>
        /// <param name="message">Mensaje a desplegar</param>
        /// <param name="typeNotification">1: Error, 2: Advertencia, 3: Información</param>
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
        #endregion

    }
}