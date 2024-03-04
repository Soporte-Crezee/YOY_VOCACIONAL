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
    public partial class ActualizarLicenciaEscuela : PageBase
    {
        #region variables de controladores
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

        private ContratoCtrl contratoCtrl;
        private LicenciaEscuelaCtrl licenciaEscuelaCtrl;
        private CicloEscolarCtrl cicloEscolarCtrl;
        private CicloContratoCtrl cicloContratoCtrl;
        private ModuloFuncionalCtrl moduloFuncionalCtrl;

        private IRedirector redirector;
        private IUserSession userSession;
        #endregion

        #region variable de conexion a BD
        private IDataContext dctx = ConnectionHlp.Default.Connection;
        #endregion

        #region variables de session
        private LicenciaEscuela LastLicenciaEscuela
        {
            set { Session["lastLicenciaEscuela"] = value; }
            get { return Session["lastLicenciaEscuela"] != null ? Session["lastLicenciaEscuela"] as LicenciaEscuela : null; }

        }
        private DataSet DsCiclos
        {
            set { Session["DsCiclos"] = value; }
            get { return Session["DsCiclos"] != null ? Session["DsCiclos"] as DataSet : null; }
        }
        #endregion

        public ActualizarLicenciaEscuela()
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
            userSession = new UserSession();
            cicloContratoCtrl = new CicloContratoCtrl();
            moduloFuncionalCtrl = new ModuloFuncionalCtrl();
        }

        #region *** eventos de pagina ***
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!userSession.IsLogin())
                redirector.GoToLoginPage(true);

            if (!IsPostBack)
            {

                if (LastLicenciaEscuela == null)
                    redirector.GoToConsultarEscuelas(true);

                LoadContratos();
                if (cbContrato.Items.Count == 1)
                    ShowMessage("No existen contratos en el sistema.", MessageType.Information);

                LoadLicenciaEscuela();
            }
        }

        protected void cbContrato_SelectedIndexChanged(object sender, EventArgs e)
        {
            Contrato contrato = GetContratoFromUI();
            LoadInfoContrato(contrato);
            LoadCiclos(contrato);

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
        protected void grdCiclos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName.Trim())
            {
                case "SelecContenidos":
                    {
                        CicloEscolar cicloEscolar = new CicloEscolar();
                        cicloEscolar.CicloEscolarID = int.Parse(e.CommandArgument.ToString());
                        LoadInfoCicloEscolar(cicloEscolar);

                    }
                    break;

                case "Sort":
                    {
                        break;
                    }
                default:
                    {
                        ShowMessage("Comando no Encontrado", MessageType.Error);
                        break;
                    }
            }
        }

        protected void grdCiclos_DataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView dataRowView = e.Row.DataItem as DataRowView;

                int cicloID = (int)Convert.ChangeType(dataRowView["CicloEscolarID"], typeof(int));
                if (cicloID == LastLicenciaEscuela.CicloEscolar.CicloEscolarID)
                {
                    e.Row.Visible = false;
                }
            }
        }

        #endregion

        #region *** validaciones ***
        private void ValidateData()
        {
            string sError = string.Empty;

            if (GetCicloEscolarFromUI().CicloEscolarID == null)
                sError += " ,Ciclo Escolar";

            if (GetContratoFromUI().ContratoID == null)
                sError += " ,Contrato";
            if (sError.Length > 0)
            {
                sError = sError.Substring(2);
                throw new Exception(string.Format("Los siguientes campos son requeridos {0}", sError));
            }


        }
        #endregion

        #region *** Data to UserInterface ***
        private void LoadLicenciaEscuela()
        {
            LicenciaEscuela licencia = licenciaEscuelaCtrl.LastDataRowToLicenciaEscuela(licenciaEscuelaCtrl.Retrieve(dctx, LastLicenciaEscuela));
            licencia.CicloEscolar = cicloEscolarCtrl.RetrieveComplete(dctx, licencia.CicloEscolar);
            licencia.Escuela = escuelaCtrl.RetrieveComplete(dctx, licencia.Escuela);
            licencia.Escuela.Ubicacion = ubicacionCtrl.RetrieveComplete(dctx, licencia.Escuela.Ubicacion);
            licencia.Contrato = contratoCtrl.LastDataRowToContrato(contratoCtrl.Retrieve(dctx, licencia.Contrato));
            licencia.Escuela.DirectorID = directorCtrl.LastDataRowToDirector(directorCtrl.Retrieve(dctx, licencia.Escuela.DirectorID));
            licencia.Escuela.TipoServicio.NivelEducativoID = nivelEducativoCtrl.RetriveComplete(dctx, licencia.Escuela.TipoServicio.NivelEducativoID);
            licencia.ModulosFuncionales = licenciaEscuelaCtrl.RetrieveModulosFuncionalesLicenciaEscuela(dctx, licencia);
            LastLicenciaEscuela = licencia;

            txtClaveEscuela.Text = licencia.Escuela.Clave;
            txtTurno.Text = licencia.Escuela.Turno.Value.ToString();
            txtNombreEscuela.Text = licencia.Escuela.NombreEscuela;
            txtAmbito.Text = licencia.Escuela.Ambito.Value.ToString();
            txtControl.Text = licencia.Escuela.Control.Value.ToString();
            txtTipoServicio.Text = licencia.Escuela.TipoServicio.Nombre;
            txtUbicacion.Text = string.Format("{0}, {1}, {2}", licencia.Escuela.Ubicacion.Ciudad.Nombre, licencia.Escuela.Ubicacion.Estado.Nombre, licencia.Escuela.Ubicacion.Pais.Nombre).Trim();
   
            cbContrato.SelectedValue = licencia.Contrato.ContratoID.ToString();
            LoadInfoContrato(licencia.Contrato);

            LoadInfoCicloEscolar(licencia.CicloEscolar);
            LoadCiclos(licencia.Contrato);

            LoadModulosFuncionales(licencia.ModulosFuncionales);
        }

        private void LoadInfoCicloEscolar(CicloEscolar ciclo)
        {
            ciclo = cicloEscolarCtrl.LastDataRowToCicloEscolar(cicloEscolarCtrl.Retrieve(dctx, ciclo));
            txtID.Text = ciclo.CicloEscolarID.ToString();
            txtNombreCiclo.Text = ciclo.Titulo;
            TxtInicioCiclo.Text = String.Format("{0:dd/MM/yyyy}", ciclo.InicioCiclo);
            TxtFinCiclo.Text = String.Format("{0:dd/MM/yyyy}", ciclo.FinCiclo);
            TxtDescripcionCiclo.Text = ciclo.Descripcion;
        }
        private void LoadInfoContrato(Contrato contrato)
        {
            if (contrato.ContratoID != null)
            {
                contrato = contratoCtrl.LastDataRowToContrato(contratoCtrl.Retrieve(dctx, contrato));

                if (contrato.LicenciasLimitadas.Value)
                {
                    List<LicenciaEscuela> licencias = licenciaEscuelaCtrl.RetriveLicenciaEscuela(dctx, new LicenciaEscuela { Contrato = contrato });

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
            }
        }
        private void LoadContratos()
        {
            cbContrato.Items.Clear();

            cbContrato.Items.Add(new ListItem("Seleccionar", ""));

            DataSet ds = contratoCtrl.Retrieve(dctx, new Contrato { Estatus = true});
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
        private Contrato GetContratoFromUI()
        {
            Contrato contrato = new Contrato();
            contrato.Estatus = true;
            long contratoID = 0;

            if (long.TryParse(cbContrato.SelectedValue, out contratoID))
            {
                contrato.ContratoID = contratoID;
                contrato.Clave = cbContrato.SelectedItem.Text;
            }
            return contrato;
        }

       
        public CicloEscolar GetCicloEscolarFromUI()
        {
            CicloEscolar cicloEscolar = new CicloEscolar();
                int cicloEscolarId = 0;
                int.TryParse(txtID.Text.Trim(), out cicloEscolarId);
                if (cicloEscolarId > 0)
                {
                    cicloEscolar.CicloEscolarID = cicloEscolarId;
 
                }
            return cicloEscolar;
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
            object myFirm = new object();
            dctx.OpenConnection(myFirm);
            dctx.BeginTransaction(myFirm);
            try
            {
                ValidateData();

                Contrato cContrato = contratoCtrl.LastDataRowToContrato(contratoCtrl.Retrieve(dctx, GetContratoFromUI()));
                CicloEscolar cCicloEscolar = cicloEscolarCtrl.LastDataRowToCicloEscolar(cicloEscolarCtrl.Retrieve(dctx, GetCicloEscolarFromUI()));

                if (cCicloEscolar.InicioCiclo < cContrato.InicioContrato)
                    throw new Exception("El inicio del ciclo debe ser mayor al inicio del contrato");
                if (cCicloEscolar.FinCiclo > cContrato.FinContrato)
                    throw new Exception("El fin del ciclo debe ser menor al fin del contrato");


                //si cambia el ciclo escolar solo se actualiza la licencia escuela, 
                //si cambia de contrato se crea una nueva licencia escuela
                LicenciaEscuela licenciaAnterior = licenciaEscuelaCtrl.LastDataRowToLicenciaEscuela(licenciaEscuelaCtrl.Retrieve(dctx, LastLicenciaEscuela));
                Escuela escuela = escuelaCtrl.RetrieveComplete(dctx, licenciaAnterior.Escuela);
                List<ModuloFuncional> modulosSeleccionados = GetModulosFuncionalesSeleccionadosFromUI();
                bool envioCorreo = false;
                string pwsTemporal = string.Empty;
                Usuario usuario = null;
                if (cContrato.ContratoID != licenciaAnterior.Contrato.ContratoID) //si cambio el contrato se crea una nueva licencia
                {
                    LicenciaEscuela licenciaEscuela = new LicenciaEscuela();
                    licenciaEscuela.CicloEscolar = cCicloEscolar;
                    licenciaEscuela.Contrato = cContrato;
                    licenciaEscuela.Escuela = escuela;
                    licenciaEscuela.Activo = true;
                    licenciaEscuela.ModulosFuncionales = modulosSeleccionados;

                    DataSet dsLicenciaEscuela = licenciaEscuelaCtrl.Retrieve(dctx, licenciaEscuela);
                    bool NoLicenciaEscuela = dsLicenciaEscuela.Tables[0].Rows.Count == 0;

                    if (!NoLicenciaEscuela)
                    {
                        throw new Exception("La escuela ya esta registrada en el ciclo " + licenciaEscuela.CicloEscolar.InicioCiclo.Value.Year.ToString(CultureInfo.InvariantCulture) + "-" + licenciaEscuela.CicloEscolar.FinCiclo.Value.Year.ToString(CultureInfo.InvariantCulture));
                    }

                    licenciaEscuela.NumeroLicencias = 0;


                    #region usuario

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
                        usuario.NombreUsuario = escuela.DirectorID.Curp;
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
                    #endregion

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


                    //se da de baja la anterior licencia
                    LicenciaEscuela licenciaAnteriorCopy = (LicenciaEscuela)licenciaAnterior.Clone();
                    licenciaAnteriorCopy.CicloEscolar = licenciaAnterior.CicloEscolar;
                    licenciaAnteriorCopy.Contrato = licenciaAnterior.Contrato;
                    licenciaAnteriorCopy.Escuela = licenciaAnterior.Escuela;

                    licenciaAnterior.Activo = false;

                    licenciaEscuelaCtrl.Update(dctx, licenciaAnterior, licenciaAnteriorCopy);
                }
                else if (licenciaAnterior.CicloEscolar.CicloEscolarID != cCicloEscolar.CicloEscolarID) // si cambio el ciclo escolar se actualiza la licencia escuela
                {
                    LicenciaEscuela licenciaAnteriorCopy = (LicenciaEscuela)licenciaAnterior.Clone();
                    licenciaAnteriorCopy.CicloEscolar = licenciaAnterior.CicloEscolar;
                    licenciaAnteriorCopy.Contrato = licenciaAnterior.Contrato;
                    licenciaAnteriorCopy.Escuela = licenciaAnterior.Escuela;

                    licenciaAnterior.Activo = true;
                    licenciaAnterior.CicloEscolar = cCicloEscolar;
                    licenciaAnterior.ModulosFuncionales = modulosSeleccionados;
                    licenciaEscuelaCtrl.ActualizarCicloEscolarLicenciaEscuela(dctx, licenciaAnterior, licenciaAnteriorCopy);

                }
                else //si no cambia ni el contrato ni el ciclo escolar se actualiza unicamente los modulos seleccionados
                {
                    LicenciaEscuela licenciaAnteriorCopy = (LicenciaEscuela)licenciaAnterior.Clone();
                    licenciaAnteriorCopy.CicloEscolar = licenciaAnterior.CicloEscolar;
                    licenciaAnteriorCopy.Contrato = licenciaAnterior.Contrato;
                    licenciaAnteriorCopy.Escuela = licenciaAnterior.Escuela;
                    licenciaAnterior.ModulosFuncionales = modulosSeleccionados;
                    licenciaEscuelaCtrl.Update(dctx, licenciaAnterior, licenciaAnteriorCopy);
                }





                dctx.CommitTransaction(myFirm);

                if (envioCorreo)
                    new CargarEscuelaCtrl().EnviarCorreo(usuario, pwsTemporal, escuela.DirectorID.Clave);



                txtRedirect.Value = "BuscarEscuelas.aspx";
                ShowMessage("La escuela y su licencia se han actualizado con éxito", MessageType.Information);


                dctx.CommitTransaction(myFirm);
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
        private void LoadCiclos(Contrato contrato)
        {
            DsCiclos = cicloContratoCtrl.Retrieve(dctx, contrato, new CicloContrato { Activo = true });
            DsCiclos.Tables[0].Columns.Add("Titulo");
            DsCiclos.Tables[0].Columns.Add("InicioCiclo");
            DsCiclos.Tables[0].Columns.Add("FinCiclo");

            if (DsCiclos.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in DsCiclos.Tables[0].Rows)
                {
                    int cicloEscolarID = (int)Convert.ChangeType(dr["CicloEscolarID"], typeof(int));
                   
                    CicloEscolar cicloEscolar = cicloEscolarCtrl.LastDataRowToCicloEscolar(cicloEscolarCtrl.Retrieve(dctx, new CicloEscolar { CicloEscolarID = cicloEscolarID, Activo = true }));
                    dr["Titulo"] = cicloEscolar.Titulo;
                    dr["InicioCiclo"] = String.Format("{0:dd/MM/yyyy}",cicloEscolar.InicioCiclo);
                    dr["FinCiclo"] = String.Format("{0:dd/MM/yyyy}",cicloEscolar.FinCiclo);
                    
                }
            }
            grdCiclosContrato.DataSource = DsCiclos;
            grdCiclosContrato.DataBind();
        }


        private void LoadModulosFuncionales(List<ModuloFuncional> modulosAsignados)
        {
            DataSet ds = moduloFuncionalCtrl.Retrieve(ConnectionHlp.Default.Connection, new ModuloFuncional());
            DataTable dtModuloFuncionales = new DataTable();
            dtModuloFuncionales.Columns.Add("ModuloFuncionalID", typeof(int));
            dtModuloFuncionales.Columns.Add("Clave", typeof(string));
            dtModuloFuncionales.Columns.Add("Nombre", typeof(string));
            dtModuloFuncionales.Columns.Add("Descripcion", typeof(string));
            dtModuloFuncionales.Columns.Add("Seleccionado", typeof(bool));

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                ModuloFuncional modulo = moduloFuncionalCtrl.DataRowToModuloFuncional(dr);
                DataRow drModulo = dtModuloFuncionales.NewRow();

                drModulo[0] =  modulo.ModuloFuncionalId;
                drModulo[1] =  modulo.Clave;
                drModulo[2] =  modulo.Nombre;
                drModulo[3] =  modulo.Descripcion;
                drModulo[4] = modulosAsignados.FirstOrDefault(m => m.ModuloFuncionalId == modulo.ModuloFuncionalId) != null ? true : false;

                dtModuloFuncionales.Rows.Add(drModulo);
            }
            grdModulosFuncionales.DataSource = dtModuloFuncionales;
            grdModulosFuncionales.DataBind();
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
    }
}