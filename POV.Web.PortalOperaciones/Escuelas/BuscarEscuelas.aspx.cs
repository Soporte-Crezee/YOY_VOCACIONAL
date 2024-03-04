using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Framework.Base.Exceptions;
using POV.CentroEducativo.BO;
using POV.CentroEducativo.Service;
using POV.Comun.BO;
using POV.Comun.Service;
using POV.Core.Operaciones.Implements;
using POV.Core.Operaciones.Interfaces;
using POV.Localizacion.BO;
using POV.Localizacion.Service;
using POV.Web.PortalOperaciones.Helper;
using System.Collections.Generic;
using System.Linq;
using POV.Licencias.BO;
using POV.Licencias.Service;
using Framework.Base.DataAccess;
using POV.Web.PortalOperaciones.AppCode.Page;
using POV.Seguridad.BO;

namespace POV.Web.PortalOperaciones.Escuelas
{
    public partial class BuscarEscuelas : CatalogPage
    {
        private TipoServicioCtrl tipoServicioCtrl;
        private NivelEducativoCtrl nivelEducativoCtrl;
        private EscuelaCtrl escuelaCtrl;
        private ZonaCtrl zonaCtrl;
        private PaisCtrl paisCtrl;
        private EstadoCtrl estadoCtrl;
        private CiudadCtrl ciudadCtrl;
        private LocalidadCtrl localidadCtrl;
        private DirectorCtrl directorCtrl;
        private UbicacionCtrl ubicacionCtrl;
        private TipoNivelEducativoCtrl tipoNivelEducativoCtrl;
        private LicenciaEscuelaCtrl licenciaEscuelaCtrl;
        private ContratoCtrl contratoCtrl;
        private CicloEscolarCtrl cicloEscolarCtrl;

        #region *** propiedades de clase ***
        private Escuela LastObject
        {
            set { Session["lastEscuela"] = value; }
            get { return Session["lastEscuela"] != null ? Session["lastEscuela"] as Escuela : null; }
        }

        private LicenciaEscuela LastLicenciaEscuela
        {
            set { Session["lastLicenciaEscuela"] = value; }
            get { return Session["lastLicenciaEscuela"] != null ? Session["lastLicenciaEscuela"] as LicenciaEscuela : null; }

        }
        private DataSet DsEscuela
        {
            set { Session["escuelasCDS"] = value; }
            get { return Session["escuelasCDS"] != null ? Session["escuelasCDS"] as DataSet : null; }
        }
        #endregion

        public BuscarEscuelas()
        {
            tipoServicioCtrl = new TipoServicioCtrl();
            nivelEducativoCtrl = new NivelEducativoCtrl();
            escuelaCtrl = new EscuelaCtrl();
            zonaCtrl= new ZonaCtrl();
            paisCtrl = new PaisCtrl();
            estadoCtrl = new EstadoCtrl();
            ciudadCtrl = new CiudadCtrl();
            localidadCtrl = new LocalidadCtrl();
            directorCtrl = new DirectorCtrl();
            ubicacionCtrl = new UbicacionCtrl();
            tipoNivelEducativoCtrl = new TipoNivelEducativoCtrl();
            licenciaEscuelaCtrl = new LicenciaEscuelaCtrl();
            contratoCtrl = new ContratoCtrl();
            cicloEscolarCtrl = new CicloEscolarCtrl();
        }

        #region *** eventos de pagina ***

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!userSession.IsLogin())
                redirector.GoToLoginPage(true);

            if (!IsPostBack)
            {
                LoadTurno();
                LoadAmbito();
                LoadControl();
                LoadContratos();
                LoadPaises(new Ubicacion { Pais = new Pais() });
                LoadTipoNivelEducativo(new TipoNivelEducativo());
                LoadEscuelas(new Escuela { Estatus = true }, new Contrato(), new CicloEscolar());

            }
        }
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                try
                {
                    ValidateData();
                }
                catch (Exception ex)
                {

                    this.ShowMessage(ex.Message, MessageType.Error);
                    return;
                }
                Escuela escuela = UserInterfaceToData();
                escuela.Estatus = true;

                Contrato contrato = GetContratoFromUI();

                CicloEscolar ciclo = new CicloEscolar();

                LoadEscuelas(escuela, contrato, ciclo);
            }
            catch (Exception ex)
            {

                POV.Logger.Service.LoggerHlp.Default.Error(this, ex);
            }

        }
        protected void grdEscuela_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName.Trim())
            {
                case "eliminar":
                    {
                        LicenciaEscuela licencia = new LicenciaEscuela { LicenciaEscuelaID = long.Parse(e.CommandArgument.ToString()) };
                        DoDelete(licencia);
                    }
                    break;
                case "editar":
                    {
                        LastLicenciaEscuela = new LicenciaEscuela { LicenciaEscuelaID = long.Parse(e.CommandArgument.ToString()) };
                        
                        Response.Redirect("~/Escuelas/EditarEscuela.aspx", true);
                    }
                    break;
                case "updLicencia":
                    {
                        LastLicenciaEscuela = new LicenciaEscuela { LicenciaEscuelaID = long.Parse(e.CommandArgument.ToString()) };
                        
                        Response.Redirect("~/Escuelas/ActualizarLicenciaEscuela.aspx", true);

                        break;
                    }
                case "Sort":
                    {
                        break;
                    }
                default: { ShowMessage("Comando no encontrado", MessageType.Error); break; }
            }


        }

        protected void grdEscuelas_Sorting(object sender, GridViewSortEventArgs e)
        {
            DataSet dataSet = DsEscuela;

            if (dataSet != null)
            {

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
                    LoadCiudades(new Ubicacion { Ciudad = new Ciudad { Estado = new Estado { EstadoID = int.Parse(CbEstado.SelectedItem.Value) } } });
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
                if (CbLocalidad.SelectedIndex > 0)
                {
                    Zona zona = new Zona();
                    zona.UbicacionID = new Ubicacion();
                    zona.UbicacionID.Pais = new Pais { PaisID = int.Parse(CbPais.SelectedItem.Value) };
                    zona.UbicacionID.Estado = new Estado { EstadoID = int.Parse(CbEstado.SelectedItem.Value) };
                    zona.UbicacionID.Ciudad = new Ciudad { CiudadID = int.Parse(CbMunicipio.SelectedItem.Value) };
                    zona.UbicacionID.Localidad = new Localidad { LocalidadID = int.Parse(CbLocalidad.SelectedItem.Value) };

                    //Consultar Ubicación
                    DataSet ds = ubicacionCtrl.RetrieveExacto(ConnectionHlp.Default.Connection, zona.UbicacionID);
                    if (ds.Tables[0].Rows.Count == 1)
                    {
                        zona.UbicacionID = ubicacionCtrl.LastDataRowToUbicacion(ds);
                        LoadZonas(zona);
                    }
                }

            }
            catch (Exception ex)
            {

                POV.Logger.Service.LoggerHlp.Default.Error(this, ex);
            }
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
        #endregion

        #region *** validaciones ***
        private void ValidateData()
        {
            string sError = string.Empty;
            //Campos requeridos
            if (txtClaveEscuela.Text.Trim().Length > 50)
                sError += " ,Clave Escuela";

            if (txtNombreEscuela.Text.Trim().Length > 50)
                sError += " ,Nombre Escuela";


            if (sError.Length > 0)
            {
                sError = sError.Substring(2);
                throw new Exception(string.Format("Los siguientes campos exceden la longitud permitida {0}", sError));
            }

        }    
        #endregion

        #region *** Data to UserInterface ***
        private void LoadTurno()
        {
            var valenum = Enum.GetValues(typeof(ETurno));
            foreach (byte en in valenum)
            {
                ETurno turn = (ETurno)en;
                CbTurno.Items.Add(new ListItem(turn.ToString(), en.ToString()));
            }
            CbTurno.Items.Insert(0, new ListItem(" ", " "));
        }
        private void LoadAmbito()
        {
            var valenum = Enum.GetValues(typeof(EAmbito));
            foreach (byte en in valenum)
            {
                EAmbito ambito = (EAmbito)en;
                CbAmbito.Items.Add(new ListItem(ambito.ToString(), en.ToString()));
            }
            CbAmbito.Items.Insert(0, new ListItem(" ", " "));
        }
        private void LoadControl()
        {
            var enu = Enum.GetValues(typeof(EControl));
            foreach (byte contl in enu)
            {
                EControl control = (EControl)contl;
                CbControl.Items.Add(new ListItem(control.ToString(), contl.ToString()));
            }
            CbControl.Items.Insert(0, new ListItem(" ", " "));
        }
        private void LoadTipoNivelEducativo(TipoNivelEducativo tipoNivelEducativo)
        {
            DataSet ds = tipoNivelEducativoCtrl.Retrieve(ConnectionHlp.Default.Connection, tipoNivelEducativo);
            CbTipoNivelEducativo.DataSource = ds;
            CbTipoNivelEducativo.DataValueField = "TipoNivelEducativoID";
            CbTipoNivelEducativo.DataTextField = "Nombre";
            CbTipoNivelEducativo.DataBind();
            CbTipoNivelEducativo.Items.Insert(0, new ListItem(" ", " "));

        }
        private void LoadNivelEducativo(NivelEducativo nivelEducativo)
        {
            DataSet ds = nivelEducativoCtrl.Retrieve(ConnectionHlp.Default.Connection, nivelEducativo);
            CbNivel.DataSource = ds;
            CbNivel.DataValueField = "NivelEducativoID";
            CbNivel.DataTextField = "Titulo";
            CbNivel.DataBind();
            CbNivel.Items.Insert(0, new ListItem(" ", " "));
        }
        private void LoadTipoServicio(TipoServicio tipoServicio)
        {
            DataSet ds = tipoServicioCtrl.Retrieve(ConnectionHlp.Default.Connection, tipoServicio);
            CbTipoServicio.DataSource = ds;
            CbTipoServicio.DataValueField = "TipoServicioID";
            CbTipoServicio.DataTextField = "Nombre";
            CbTipoServicio.DataBind();
            CbTipoServicio.Items.Insert(0, new ListItem(" ", " "));
        }
        private void LoadZonas(Zona zona)
        {
            //Zona

            DataSet ds = zonaCtrl.Retrieve(ConnectionHlp.Default.Connection, zona);
            CbZona.DataSource = ds;
            CbZona.DataValueField = "ZonaID";
            CbZona.DataTextField = "Nombre";
            CbZona.DataBind();
            CbZona.Items.Insert(0, new ListItem(" ", " "));
        }
        private void LoadEscuelas(Escuela filter, Contrato contrato, CicloEscolar cicloEscolar)
        {
            if (filter == null)
                return;
            DataSet ds = licenciaEscuelaCtrl.RetrieveFilterByEscuela(ConnectionHlp.Default.Connection,
                new LicenciaEscuela { Activo = true, Escuela = filter, Contrato = contrato, CicloEscolar = cicloEscolar });
            
            #region Crear Estructura Escuela
            DataSet dsCompose = new DataSet();
            dsCompose.Tables.Add(new DataTable("Escuelas"));
            dsCompose.Tables["Escuelas"].Columns.Add(new DataColumn("LicenciaEscuelaID", typeof(long)));
            dsCompose.Tables["Escuelas"].Columns.Add(new DataColumn("ClaveContrato", typeof(string)));
            dsCompose.Tables["Escuelas"].Columns.Add(new DataColumn("EscuelaID", typeof(int)));
            dsCompose.Tables["Escuelas"].Columns.Add(new DataColumn("Clave", typeof(string)));
            dsCompose.Tables["Escuelas"].Columns.Add(new DataColumn("NombreEscuela", typeof(string)));
            dsCompose.Tables["Escuelas"].Columns.Add(new DataColumn("Estatus", typeof(string)));
            dsCompose.Tables["Escuelas"].Columns.Add(new DataColumn("Turno", typeof(string)));
            dsCompose.Tables["Escuelas"].Columns.Add(new DataColumn("TurnoID", typeof(byte)));
            dsCompose.Tables["Escuelas"].Columns.Add(new DataColumn("Ambito", typeof(string)));
            dsCompose.Tables["Escuelas"].Columns.Add(new DataColumn("AmbitoID", typeof(byte)));
            dsCompose.Tables["Escuelas"].Columns.Add(new DataColumn("Control", typeof(string)));
            dsCompose.Tables["Escuelas"].Columns.Add(new DataColumn("ControlID", typeof(byte)));
            dsCompose.Tables["Escuelas"].Columns.Add(new DataColumn("TipoNivelEducativo", typeof(string)));
            dsCompose.Tables["Escuelas"].Columns.Add(new DataColumn("TipoNivelEducativoID", typeof(int)));
            dsCompose.Tables["Escuelas"].Columns.Add(new DataColumn("NivelEducativo", typeof(string)));
            dsCompose.Tables["Escuelas"].Columns.Add(new DataColumn("NivelEducativoID", typeof(int)));
            dsCompose.Tables["Escuelas"].Columns.Add(new DataColumn("TipoServicio", typeof(string)));
            dsCompose.Tables["Escuelas"].Columns.Add(new DataColumn("TipoServicioID", typeof(int)));

            dsCompose.Tables["Escuelas"].Columns.Add(new DataColumn("DirectorID", typeof(int)));
            dsCompose.Tables["Escuelas"].Columns.Add(new DataColumn("NombreDirector", typeof(string)));
            dsCompose.Tables["Escuelas"].Columns.Add(new DataColumn("Pais", typeof(string)));
            dsCompose.Tables["Escuelas"].Columns.Add(new DataColumn("PaisID", typeof(int)));
            dsCompose.Tables["Escuelas"].Columns.Add(new DataColumn("Estado", typeof(string)));
            dsCompose.Tables["Escuelas"].Columns.Add(new DataColumn("EstadoID", typeof(int)));
            dsCompose.Tables["Escuelas"].Columns.Add(new DataColumn("Ciudad", typeof(string)));
            dsCompose.Tables["Escuelas"].Columns.Add(new DataColumn("CiudadID", typeof(int)));
            dsCompose.Tables["Escuelas"].Columns.Add(new DataColumn("Localidad", typeof(string)));
            dsCompose.Tables["Escuelas"].Columns.Add(new DataColumn("LocalidadID", typeof(int)));
            dsCompose.Tables["Escuelas"].Columns.Add(new DataColumn("Zona", typeof(string)));
            dsCompose.Tables["Escuelas"].Columns.Add(new DataColumn("ZonaID", typeof(int)));
            #endregion

            foreach (DataRow drEsc in ds.Tables[0].Rows)
            {
                Escuela escuelaActual = escuelaCtrl.DataRowToEscuela(drEsc);


                escuelaActual = escuelaCtrl.RetrieveComplete(ConnectionHlp.Default.Connection, new Escuela { EscuelaID = escuelaActual.EscuelaID });
                NivelEducativo nivelEducativoEsc = nivelEducativoCtrl.RetriveComplete(ConnectionHlp.Default.Connection, escuelaActual.TipoServicio.NivelEducativoID);
                Ubicacion ubicacionEsc = ubicacionCtrl.RetrieveComplete(ConnectionHlp.Default.Connection, escuelaActual.Ubicacion);
                Contrato contratoEsc = contratoCtrl.LastDataRowToContrato(contratoCtrl.Retrieve(ConnectionHlp.Default.Connection, new Contrato { ContratoID = (long)Convert.ChangeType(drEsc["ContratoID"], typeof(long)) }));

                escuelaActual.Ubicacion = ubicacionEsc;
                DataRow dr = dsCompose.Tables["Escuelas"].NewRow();

                #region Agregar filas a la Escuela
                dr.SetField("LicenciaEscuelaID", (long)Convert.ChangeType(drEsc["LicenciaEscuelaID"], typeof(long)));
                dr.SetField("EscuelaID", escuelaActual.EscuelaID);
                dr.SetField("ClaveContrato", contratoEsc.Clave);
                dr.SetField("Clave", escuelaActual.Clave);
                dr.SetField("NombreEscuela", escuelaActual.NombreEscuela.Length > 20 ? string.Format("{0}...", escuelaActual.NombreEscuela.Substring(0, 20)) : escuelaActual.NombreEscuela);
                dr.SetField("Estatus", escuelaActual.Estatus != null && (bool)escuelaActual.Estatus ? "ACTIVO" : "INACTIVO");
                dr.SetField("Turno", escuelaActual.Turno.ToString());
                dr.SetField("TurnoID", escuelaActual.Turno != null ? (byte)escuelaActual.Turno : 0);
                dr.SetField("Ambito", escuelaActual.Ambito.ToString());
                dr.SetField("AmbitoID", escuelaActual.Ambito != null ? (byte)escuelaActual.Ambito : 0);
                dr.SetField("Control", escuelaActual.Control);
                dr.SetField("ControlID", escuelaActual.Control != null ? (byte)escuelaActual.Control : 0);
                dr.SetField("TipoNivelEducativo", nivelEducativoEsc.TipoNivelEducativoID != null ? nivelEducativoEsc.TipoNivelEducativoID.Nombre : string.Empty);
                dr.SetField("TipoNivelEducativoID", nivelEducativoEsc.TipoNivelEducativoID != null ? nivelEducativoEsc.TipoNivelEducativoID.TipoNivelEducativoID : null);
                dr.SetField("NivelEducativo", nivelEducativoEsc.Titulo);
                dr.SetField("NivelEducativoID", nivelEducativoEsc.NivelEducativoID);
                dr.SetField("TipoServicio", escuelaActual.TipoServicio != null ? escuelaActual.TipoServicio.Nombre : string.Empty);
                dr.SetField("TipoServicioID", escuelaActual.TipoServicio != null ? escuelaActual.TipoServicio.TipoServicioID : null);

                //Director

                dr.SetField("DirectorID", escuelaActual.DirectorID != null ? escuelaActual.DirectorID.DirectorID : null);
                dr.SetField("NombreDirector", escuelaActual.DirectorID != null ? string.Format("{0} {1} {2}", escuelaActual.DirectorID.Nombre, escuelaActual.DirectorID.PrimerApellido, escuelaActual.DirectorID.SegundoApellido).Trim() : string.Empty);


                dr.SetField("Pais", escuelaActual.Ubicacion.Pais != null ? escuelaActual.Ubicacion.Pais.Nombre : string.Empty);
                dr.SetField("PaisID", escuelaActual.Ubicacion.Pais != null ? escuelaActual.Ubicacion.Pais.PaisID : null);
                dr.SetField("Estado", escuelaActual.Ubicacion.Estado != null ? escuelaActual.Ubicacion.Estado.Nombre : string.Empty);
                dr.SetField("EstadoID", escuelaActual.Ubicacion.Estado != null ? escuelaActual.Ubicacion.Estado.EstadoID : null);
                dr.SetField("Ciudad", escuelaActual.Ubicacion.Ciudad != null ? escuelaActual.Ubicacion.Ciudad.Nombre : string.Empty);
                dr.SetField("CiudadID", escuelaActual.Ubicacion.Ciudad != null ? escuelaActual.Ubicacion.Ciudad.CiudadID : null);
                dr.SetField("Localidad", escuelaActual.Ubicacion.Localidad != null ? escuelaActual.Ubicacion.Localidad.Nombre : string.Empty);
                dr.SetField("LocalidadID", escuelaActual.Ubicacion.Localidad != null ? escuelaActual.Ubicacion.Localidad.LocalidadID : null);
                dr.SetField("Zona", escuelaActual.ZonaID != null ? escuelaActual.ZonaID.Nombre : string.Empty);
                dr.SetField("ZonaID", escuelaActual.ZonaID != null ? escuelaActual.ZonaID.ZonaID : null);
                #endregion

                dsCompose.Tables["Escuelas"].Rows.Add(dr);
            }

            DsEscuela = dsCompose;

            grdEscuela.DataSource = DsEscuela;
            grdEscuela.DataBind();

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
            CbPais.Items.Insert(0, new ListItem(" ", " "));

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
            CbEstado.Items.Insert(0, new ListItem(" ", " "));
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
            CbMunicipio.Items.Insert(0, new ListItem(" ", " "));
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
            CbLocalidad.Items.Insert(0, new ListItem(" ", " "));
        }

        private void LoadContratos()
        {
            cbContrato.Items.Clear();

            cbContrato.Items.Add(new ListItem("Seleccionar", ""));

            DataSet ds = contratoCtrl.Retrieve(ConnectionHlp.Default.Connection, new Contrato());
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbContrato.DataSource = ds;
                cbContrato.DataTextField = "Clave";
                cbContrato.DataValueField = "ContratoID";
            }

            cbContrato.DataBind();
        }
        #endregion

        #region *** UserInterface to Data ***
        private Escuela UserInterfaceToData()
        {
            Escuela escuela = new Escuela();
            //Información de la escuela
            escuela.NombreEscuela = !string.IsNullOrEmpty(txtNombreEscuela.Text.Trim()) ? txtNombreEscuela.Text.Trim() : null;
            escuela.Clave = !string.IsNullOrEmpty(txtClaveEscuela.Text.Trim()) ? txtClaveEscuela.Text.Trim() : null;
            escuela.Turno = CbTurno.SelectedIndex > 0 ? (ETurno?)byte.Parse(CbTurno.SelectedItem.Value) : null;

            escuela.Ambito = CbAmbito.SelectedIndex > 0 ? (EAmbito?)byte.Parse(CbAmbito.SelectedItem.Value) : null;
            escuela.TipoServicio = CbTipoServicio.SelectedIndex > 0 ? new TipoServicio { TipoServicioID = int.Parse(CbTipoServicio.SelectedItem.Value) } : null;

            NivelEducativo nivel = CbNivel.SelectedIndex > 0 ? new NivelEducativo { NivelEducativoID = int.Parse(CbNivel.SelectedItem.Value) } : null;
            TipoNivelEducativo tipoNivel = CbTipoNivelEducativo.SelectedIndex > 0 ? new TipoNivelEducativo { TipoNivelEducativoID = int.Parse(CbTipoNivelEducativo.SelectedItem.Value) } : null;
            if (nivel == null)
                nivel = new NivelEducativo { TipoNivelEducativoID = tipoNivel };
            else
                nivel.TipoNivelEducativoID = tipoNivel;

            if (escuela.TipoServicio == null)
                escuela.TipoServicio = new TipoServicio { NivelEducativoID = nivel };
            else
                escuela.TipoServicio.NivelEducativoID = nivel;

            escuela.Control = CbControl.SelectedIndex > 0 ? (EControl?)byte.Parse(CbControl.SelectedItem.Value) : null;
            escuela.ZonaID = CbZona.SelectedIndex > 0 ? new Zona { ZonaID = int.Parse(CbZona.SelectedItem.Value) } : null;


            //Ubicación
            escuela.Ubicacion = new Ubicacion();
            escuela.Ubicacion.Pais = new Pais { PaisID = CbPais.SelectedIndex > 0 ? int.Parse(CbPais.SelectedItem.Value) : (int?)null };
            escuela.Ubicacion.Estado = new Estado { EstadoID = CbEstado.SelectedIndex > 0 ? int.Parse(CbEstado.SelectedItem.Value) : (int?)null };
            escuela.Ubicacion.Ciudad = new Ciudad { CiudadID = CbMunicipio.SelectedIndex > 0 ? int.Parse(CbMunicipio.SelectedItem.Value) : (int?)null };
            escuela.Ubicacion.Localidad = new Localidad { LocalidadID = CbLocalidad.SelectedIndex > 0 ? int.Parse(CbLocalidad.SelectedItem.Value) : (int?)null };

            return escuela;
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
        #endregion

        #region *** metodos auxiliares ***
        private void DoDelete(LicenciaEscuela licenciaEscuela)
        {
            IDataContext dctx = ConnectionHlp.Default.Connection;

            bool reload = false;
            object myFirm = new object();
            dctx.OpenConnection(myFirm);
            dctx.BeginTransaction(myFirm);
            try
            {
                licenciaEscuela = licenciaEscuelaCtrl.LastDataRowToLicenciaEscuela(licenciaEscuelaCtrl.Retrieve(dctx, licenciaEscuela));
                Escuela anterior = escuelaCtrl.RetrieveComplete(dctx, new Escuela { EscuelaID = licenciaEscuela.Escuela.EscuelaID });
                Escuela elimescuela = (Escuela)anterior.CloneAll();
                elimescuela.Estatus = false;
                escuelaCtrl.Update(dctx, elimescuela, anterior);

                LicenciaEscuela anteriorLicencia = (LicenciaEscuela)licenciaEscuela.Clone();
                anteriorLicencia.CicloEscolar = licenciaEscuela.CicloEscolar;
                anteriorLicencia.Contrato = licenciaEscuela.Contrato;
                anteriorLicencia.Escuela = licenciaEscuela.Escuela;

                licenciaEscuela.Activo = false;

                licenciaEscuelaCtrl.Update(dctx, licenciaEscuela, anteriorLicencia);

                dctx.CommitTransaction(myFirm);
                reload = true;
            }
            catch (Exception ex)
            {
                dctx.RollbackTransaction(myFirm);
                dctx.CloseConnection(myFirm);
                POV.Logger.Service.LoggerHlp.Default.Error(this, ex);
                ShowMessage("Ocurrió un error al eliminar el registro", MessageType.Error);
            }
            finally
            {
                if (dctx.ConnectionState == ConnectionState.Open)
                    dctx.CloseConnection(myFirm);
            }

            if (reload)
                LoadEscuelas(new Escuela { Estatus = true }, new Contrato(), new CicloEscolar());

        } 
        #endregion

        #region AUTORIZACION DE LA PAGINA

        protected void grdEscuela_DataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (RenderEdit)
                {
                    ImageButton btnEdit = (ImageButton)e.Row.FindControl("btnEdit");
                    btnEdit.Visible = true;

                    ImageButton btnActualizar = (ImageButton)e.Row.FindControl("btnUpdateLicencia");
                    btnActualizar.Visible = true;
                }

                if (RenderDelete)
                {
                    ImageButton btnDelete = (ImageButton)e.Row.FindControl("btnDelete");
                    btnDelete.Visible = true;
                }
            }
        }

        private bool RenderEdit = false;
        private bool RenderDelete = false;

        protected override void DisplayCreateAction()
        {
            PnlCreate.Visible = true;
        }

        protected override void DisplayReadAction()
        {

            grdEscuela.Visible = true;
        }

        protected override void DisplayUpdateAction()
        {
            RenderEdit = true;
        }

        protected override void DisplayDeleteAction()
        {
            RenderDelete = true;
        }

        protected override void AuthorizeUser()
        {
            //ejemplificando la ejecucion
            List<Permiso> permisos = userSession.CurrentPrivilegios.GetPermisos();

            bool acceso = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POACCESOESCUELAS) != null;
            bool lectura = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POCONSULTARESCUELAS) != null;
            bool creacion = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POINSERTARESCUELAS) != null;
            bool delete = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POELIMINARESCUELAS) != null;
            bool edit = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POEDITARESCUELAS) != null;

            if (!acceso)
                redirector.GoToHomePage(true);
            if (lectura)
                DisplayReadAction();
            if (creacion)
                DisplayCreateAction();
            if (delete)
                DisplayDeleteAction();
            if (edit)
                DisplayUpdateAction();
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