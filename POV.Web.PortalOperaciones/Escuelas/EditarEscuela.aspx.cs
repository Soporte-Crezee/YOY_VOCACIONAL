using System;
using System.Data;
using System.Linq;
using System.Collections.Generic;
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
using POV.Licencias.BO;
using POV.Licencias.Service;
using Framework.Base.DataAccess;
using POV.Seguridad.BO;
using POV.Seguridad.Service;
using POV.Seguridad.Utils;
using GP.SocialEngine.BO;
using GP.SocialEngine.Service;
using POV.Operaciones.Service;
using POV.Web.PortalOperaciones.AppCode.Page;

namespace POV.Web.PortalOperaciones.Escuelas
{
    public partial class EditarEscuela : PageBase
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
        private LicenciaEscuelaCtrl licenciaEscuelaCtrl;


        #region Variables Sesion

        private LicenciaEscuela LastLicenciaEscuela
        {
            set { Session["lastLicenciaEscuela"] = value; }
            get { return Session["lastLicenciaEscuela"] != null ? Session["lastLicenciaEscuela"] as LicenciaEscuela : null; }

        }

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
        private Escuela LastObject
        {
            get { return Session["lastEscuela"] != null ? (Escuela)Session["lastEscuela"] : null; }
            set { Session["lastEscuela"] = value; }
        }
        #endregion

        #region *** propiedades de clase ***

        #endregion

        public EditarEscuela()
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
            licenciaEscuelaCtrl = new LicenciaEscuelaCtrl();
        }

        #region *** eventos de pagina ***
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!userSession.IsLogin())
                    redirector.GoToLoginPage(true);

                if (LastLicenciaEscuela == null)
                    redirector.GoToConsultarEscuelas(true);

                if (!IsPostBack)
                {
                    SelectedDirector = null;
                    DSDirectores = null;
                    LoadEscuela();
                    LoadDirectores(new Director());
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
        protected void btnConsultar_Click(object sender, EventArgs e)
        {
            try
            {
                //Cargar directores del sistema
                LoadDirectores(new Director());
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


                }
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
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                DoUpdate();

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

            if (sError.Length > 0)
            {
                sError = sError.Substring(2);
                throw new Exception(string.Format("Los siguientes campos son requeridos {0}", sError));
            }

            //longitud
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
                if (actescuela.EscuelaID == LastObject.EscuelaID)
                    return;
                if ((bool)actescuela.Estatus)
                    throw new Exception("La clave de la escuela ya está registrada para el turno seleccionado, por favor verifique");
            }

        }
        #endregion

        #region *** Data to UserInterface ***
        #region Ubicacion

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
            CbMunicipio.Items.Insert(0, new ListItem(" ", "0"));
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
            CbLocalidad.Items.Insert(0, new ListItem(" ", "0"));
        }
        #endregion

        private void LoadEscuela()
        {
            LastLicenciaEscuela = licenciaEscuelaCtrl.LastDataRowToLicenciaEscuela(licenciaEscuelaCtrl.Retrieve(ConnectionHlp.Default.Connection, LastLicenciaEscuela));

            LastObject = escuelaCtrl.RetrieveComplete(ConnectionHlp.Default.Connection, LastLicenciaEscuela.Escuela);
            Escuela escuela = LastObject;
            //Ubicación
            if (escuela.Ubicacion != null)
            {
                LoadPaises(new Ubicacion { Pais = new Pais { PaisID = escuela.Ubicacion.Pais.PaisID } });
                CbPais.SelectedValue = escuela.Ubicacion.Pais != null ? escuela.Ubicacion.Pais.PaisID.ToString() : null;

                LoadEstados(new Ubicacion { Estado = new Estado { Pais = escuela.Ubicacion.Pais } });
                CbEstado.SelectedValue = escuela.Ubicacion.Estado != null ? escuela.Ubicacion.Estado.EstadoID.ToString() : null;

                LoadCiudades(new Ubicacion { Ciudad = new Ciudad { Estado = escuela.Ubicacion.Estado } });
                CbMunicipio.SelectedValue = escuela.Ubicacion.Ciudad != null ? escuela.Ubicacion.Ciudad.CiudadID.ToString() : null;

                LoadLocalidades(new Ubicacion { Localidad = new Localidad { Ciudad = escuela.Ubicacion.Ciudad } });
                CbLocalidad.SelectedValue = escuela.Ubicacion.Localidad != null ? escuela.Ubicacion.Localidad.LocalidadID.ToString() : null;

                LoadZonas(new Zona { UbicacionID = escuela.ZonaID.UbicacionID });
                CbZona.SelectedValue = escuela.ZonaID != null ? escuela.ZonaID.ZonaID.ToString() : null;

            }
            //Datos de la escuela
            txtClaveEscuela.Text = escuela.Clave;
            txtNombreEscuela.Text = escuela.NombreEscuela;

            //Turno
            LoadTurno();
            CbTurno.SelectedValue = escuela.Turno != null ? escuela.ToShortTurno.ToString() : null;

            //Ámbito
            LoadAmbito();
            CbAmbito.SelectedValue = escuela.Ambito != null ? ((byte)escuela.Ambito).ToString() : null;
            //Control
            LoadControl();
            CbControl.SelectedValue = escuela.Control != null ? ((byte)escuela.Control).ToString() : null;

            //Tipo Escuela
            NivelEducativo nivelEducativoEsc = nivelEducativoCtrl.RetriveComplete(ConnectionHlp.Default.Connection, escuela.TipoServicio.NivelEducativoID);
            LoadTipoNivelEducativo(new TipoNivelEducativo());
            CbTipoNivelEducativo.SelectedValue = nivelEducativoEsc.TipoNivelEducativoID != null ? nivelEducativoEsc.TipoNivelEducativoID.TipoNivelEducativoID.ToString() : null;

            LoadNivelEducativo(new NivelEducativo { TipoNivelEducativoID = nivelEducativoEsc.TipoNivelEducativoID });
            CbNivel.SelectedValue = nivelEducativoEsc.NivelEducativoID != null ? nivelEducativoEsc.NivelEducativoID.ToString() : null;

            LoadTipoServicio(new TipoServicio { NivelEducativoID = nivelEducativoEsc });
            CbTipoServicio.SelectedValue = escuela.TipoServicio != null ? escuela.TipoServicio.TipoServicioID.ToString() : null;

            //Datos del Director
            SelectedDirector = escuela.DirectorID;
            txtCurp.Text = escuela.DirectorID.Curp;
            txtNombreDirector.Text = string.Format("{0} {1} {2}", escuela.DirectorID.Nombre, escuela.DirectorID.PrimerApellido, escuela.DirectorID.SegundoApellido).Trim();

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
        #endregion

        #region *** metodos auxiliares ***
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
        private void DoUpdate()
        {
            IDataContext dctx = ConnectionHlp.Default.Connection;
            object myFirm = new object();
            dctx.OpenConnection(myFirm);
            dctx.BeginTransaction(myFirm);

            try
            {
                ValidateData();
                ValidateNoRepetido();

                Escuela escuela = UserInterfaceToData();

                #region insertar ubicacion
                Ubicacion ubicacion = new Ubicacion();
                DataSet dsUbicacion = ubicacionCtrl.RetrieveExacto(dctx, escuela.Ubicacion);
                int index = dsUbicacion.Tables["Ubicacion"].Rows.Count;
                if (index == 1)
                    ubicacion = ubicacionCtrl.LastDataRowToUbicacion(dsUbicacion);
                //si no existe se inserta la ubicacion
                if (ubicacion.UbicacionID == null)
                {
                    ubicacionCtrl.Insert(dctx, escuela.Ubicacion);
                    escuela.Ubicacion = ubicacionCtrl.LastDataRowToUbicacion(ubicacionCtrl.RetrieveExacto(dctx, escuela.Ubicacion));
                }
                else
                    escuela.Ubicacion = ubicacion;
                #endregion

                //Recuperar escuela y actualizar
                DataSet ds = escuelaCtrl.Retrieve(dctx, new Escuela { EscuelaID = LastObject.EscuelaID });

                if (ds.Tables["Escuela"].Rows.Count != 1)
                    throw new Exception("Ocurrió un error mientras se actualizaba la escuela");

                Escuela anterior = escuelaCtrl.LastDataRowToEscuela(ds);

                escuela.FechaRegistro = anterior.FechaRegistro;
                escuela.EscuelaID = LastObject.EscuelaID;
                escuela.Estatus = true;
                escuelaCtrl.Update(dctx, escuela, anterior);

                #region actualizacion de licencia director
                Usuario usuario = null;
                bool envioCorreo = false;
                string pwsTemporal = string.Empty;
                if (escuela.DirectorID.DirectorID != anterior.DirectorID.DirectorID) // cambio el director
                {

                    escuela.DirectorID = directorCtrl.LastDataRowToDirector(directorCtrl.Retrieve(dctx, escuela.DirectorID));
                    usuario = licenciaEscuelaCtrl.RetrieveUsuario(dctx, escuela.DirectorID);


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

                    usuarioPrivilegiosCtrl.InsertUsuarioEscolarPrivilegios(dctx, usuario, LastLicenciaEscuela.Escuela, LastLicenciaEscuela.CicloEscolar, privilegios);

                    #endregion

                    licenciaEscuelaCtrl.UpdateDirectorLicenciaEscuela(dctx, LastLicenciaEscuela, escuela.DirectorID, usuario);



                }
                #endregion
                dctx.CommitTransaction(myFirm);

                LastObject = null;
                LastLicenciaEscuela = null;
                SelectedDirector = null;
                DSDirectores = null;
                redirector.GoToConsultarEscuelas(false);




                if (envioCorreo)
                    new CargarEscuelaCtrl().EnviarCorreo(usuario, pwsTemporal, escuela.DirectorID.Clave);
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
        #endregion

        #region AUTORIZACION DE LA PAGINA

        protected override void AuthorizeUser()
        {
            //ejemplificando la ejecucion
            List<Permiso> permisos = userSession.CurrentPrivilegios.GetPermisos();

            bool acceso = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POACCESOESCUELAS) != null;
            bool lectura = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POCONSULTARESCUELAS) != null;
            bool edit = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POEDITARESCUELAS) != null;

            if (!acceso)
                redirector.GoToHomePage(true);
            if (!lectura)
                redirector.GoToHomePage(true);
            if (!edit)
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