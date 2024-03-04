using POV.CentroEducativo.BO;
using POV.CentroEducativo.Service;
using POV.Comun.BO;
using POV.Comun.Service;
using POV.Expediente.BO;
using POV.Expediente.Service;
using POV.Licencias.BO;
using POV.Licencias.Service;
using POV.Localizacion.BO;
using POV.Localizacion.Service;
using POV.Logger.Service;
using POV.Modelo.BO;
using POV.Modelo.Service;
using POV.Prueba.Diagnostico.Dinamica.BO;
using POV.Prueba.Diagnostico.Dinamica.Service;
using POV.Prueba.Diagnostico.Service;
using POV.Seguridad.BO;
using POV.Seguridad.Service;
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
using POV.Expediente.Services;
using POV.CentroEducativo.Services;

namespace POV.Web.PortalUniversidad.Pages
{
    public partial class DetalleExpedienteAlumnoUniversidad : CatalogPage
    {
        #region Propiedades
        public InfoAlumnoUsuario verExpediente
        {
            get { return Session["verExpediente"] as InfoAlumnoUsuario; }
            set { Session["verExpediente"] = value; }
        }

        public InfoAlumnoUsuario selecciones
        {
            get { return Session["selecciones"] as InfoAlumnoUsuario; }
            set { Session["selecciones"] = value; }
        }

        private readonly IDataContext dctx = ConnectionHlp.Default.Connection;

        private AlumnoCtrl alumnoCtrl;
        private ExpedienteEscolarCtrl expedienteEscolarCtrl;
        private UsuarioCtrl usuarioCtrl;
        private LicenciaEscuelaCtrl licenciaEscuelaCtrl;
        private GrupoCicloEscolarCtrl grupoCicloEscolarCtrl;

        private UbicacionCtrl ubicacionCtrl;
        private PaisCtrl paisCtrl;
        private EstadoCtrl estadoCtrl;
        private CiudadCtrl ciudadCtrl;

        private ModeloCtrl modeloCtrl;
        private PruebaDinamicaCtrl pruebaDinamicaCtrl;

        private InfoAlumnoUsuario alumnoUsuario;
        private InfoAlumnoUsuarioCtrl infoAlumnoUsuarioCtrl;

        private RespuestaAlumnoCtrl respuestaAlumnoCtrl;
        private ResultadoPruebasOrientacionVocacionalCtrl resultadoPruebasOrientacionVocacionalCtrl;

        private UsuarioExpedienteCtrl usuarioExpedienteCtrl;
        private EFDocenteCtrl eFDocenteCtrl;
        #endregion

        public DetalleExpedienteAlumnoUniversidad()
        {
            alumnoCtrl = new AlumnoCtrl();
            expedienteEscolarCtrl = new ExpedienteEscolarCtrl();
            usuarioCtrl = new UsuarioCtrl();
            licenciaEscuelaCtrl = new LicenciaEscuelaCtrl();
            grupoCicloEscolarCtrl = new GrupoCicloEscolarCtrl();

            ubicacionCtrl = new UbicacionCtrl();
            paisCtrl = new PaisCtrl();
            estadoCtrl = new EstadoCtrl();
            ciudadCtrl = new CiudadCtrl();

            modeloCtrl = new ModeloCtrl();
            pruebaDinamicaCtrl = new PruebaDinamicaCtrl();

            alumnoUsuario = new InfoAlumnoUsuario();
            infoAlumnoUsuarioCtrl = new InfoAlumnoUsuarioCtrl();

            respuestaAlumnoCtrl = new RespuestaAlumnoCtrl();
            resultadoPruebasOrientacionVocacionalCtrl = new ResultadoPruebasOrientacionVocacionalCtrl();

            usuarioExpedienteCtrl = new UsuarioExpedienteCtrl();
            eFDocenteCtrl = new EFDocenteCtrl(null);
        }

        #region Eventos de la pagina
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (userSession != null && userSession.CurrentUniversidad != null && userSession.CurrentCicloEscolar != null)
                {
                    if (!IsPostBack)
                    {
                        if (verExpediente != null)
                        {
                            //        // Implementado por CG *************************************
                            selecciones = selecciones;
                            CargarDatos();
                        }
                        else 
                        {
                            redirector.GoToLoginPage(true);
                        }
                    }
                }
                else
                {
                    redirector.GoToLoginPage(true);
                }
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message, MessageType.Error);
                LoggerHlp.Default.Error(this, ex);
            }

        }

        protected void ddlEstado_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlEstado.SelectedIndex > 0)
                {
                    LoadCiudades(new Ubicacion { Ciudad = new Ciudad { Estado = new Estado { EstadoID = int.Parse(ddlEstado.SelectedItem.Value) } } });
                }
                else
                {
                    ddlMunicipio.ClearSelection();
                }
            }
            catch (Exception ex)
            {
                POV.Logger.Service.LoggerHlp.Default.Error(this, ex);
            }
        }

        protected void ddlPais_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlPais.SelectedIndex > 0)
                {
                    LoadEstados(new Ubicacion { Estado = new Estado { Pais = new Pais { PaisID = int.Parse(ddlPais.SelectedItem.Value) } } });
                }
                else
                {
                    ddlEstado.ClearSelection();
                    ddlMunicipio.ClearSelection();
                }

            }
            catch (Exception ex)
            {

                POV.Logger.Service.LoggerHlp.Default.Error(this, ex);
            }
        }
        #endregion

        #region Metodos Auxiliares
        private void CargarDatos()
        {

            InfoAlumnoUsuario infoAlumno = new InfoAlumnoUsuario();
            infoAlumno.AlumnoID = verExpediente.AlumnoID;

            Alumno alumnoEstudiante = new Alumno();
            alumnoEstudiante.AlumnoID = infoAlumno.AlumnoID;

            // Consultar alumno y asignarle los datos a la interfaz
            Alumno alumno = alumnoCtrl.LastDataRowToAlumno(alumnoCtrl.Retrieve(dctx, alumnoEstudiante));

            Usuario usuarioAlumno = licenciaEscuelaCtrl.RetrieveUsuario(dctx, new Alumno { AlumnoID = alumno.AlumnoID, Curp = alumno.Curp });

            Contrato contrato = userSession.Contrato;

            ContratoCtrl contratoCtrl = new ContratoCtrl();
            List<PruebaContrato> pruebas = contratoCtrl.RetrievePruebasAsignadoContrato(dctx, contrato, new CicloContrato { CicloEscolar = userSession.CurrentCicloEscolar });
            PruebaContrato pruebaPivoteContrato = pruebas.FirstOrDefault(item => item.TipoPruebaContrato == ETipoPruebaContrato.Pivote);

            var pruebaDinamica = new PruebaDinamica();

            // Areas de conocimiento
            ArrayList arrAreaConocimiento = new ArrayList();
            List<Clasificador> areasConocimiento = new List<Clasificador>();

            // Datos del alumno
            txtNombre.Text = alumno.Nombre + " " + alumno.PrimerApellido + " " + alumno.SegundoApellido;
            txtFechNacimiento.Text = String.Format("{0:dd/MM/yyyy}", alumno.FechaNacimiento);
            txtEscuela.Text = alumno.Escuela;
            if (alumno.Grado != null)
            {
                if (alumno.Grado == EGrado.SEMESTRE_1)
                    txtNivEstudio.Text = "Semestre 1";
                else if (alumno.Grado == EGrado.SEMESTRE_2)
                    txtNivEstudio.Text = "Semestre 2";
                else if (alumno.Grado == EGrado.SEMESTRE_3)
                    txtNivEstudio.Text = "Semestre 3";
                else if (alumno.Grado == EGrado.SEMESTRE_4)
                    txtNivEstudio.Text = "Semestre 4";
                else if (alumno.Grado == EGrado.SEMESTRE_5)
                    txtNivEstudio.Text = "Semestre 5";
                else if (alumno.Grado == EGrado.SEMESTRE_6)
                    txtNivEstudio.Text = "Semestre 6";
            }

            #region Lista Usuarios de docente asignados
            Docente docente = new Docente();
            DataSet ds = usuarioExpedienteCtrl.Retrieve(dctx, new UsuarioExpediente { AlumnoID = alumno.AlumnoID });
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    UsuarioExpediente usExp = usuarioExpedienteCtrl.DataRowToUsuarioExpediente(ds.Tables[0].Rows[i]);
                    docente = licenciaEscuelaCtrl.RetrieveUsuarioOrientador(dctx, new Usuario { UsuarioID = usExp.UsuarioID });

                    if (userSession.CurrentUniversidad.Docentes.FirstOrDefault(x => x.DocenteID == docente.DocenteID) != null)
                    {
                        Usuario usuarioUniversidad = licenciaEscuelaCtrl.RetrieveUsuarios(ConnectionHlp.Default.Connection, docente).Where(x => x.UniversidadId == userSession.CurrentUniversidad.UniversidadID).FirstOrDefault();
                        if (usExp.UsuarioID == usuarioUniversidad.UsuarioID && usExp.AlumnoID == alumno.AlumnoID)                        
                            docente = eFDocenteCtrl.RetrieveWithRelationship(new Docente { DocenteID = docente.DocenteID }, false).FirstOrDefault();                        
                    }
                    else                    
                        docente = null;                    
                }
            }
            else             
                docente = null;
            
            #endregion

            if (docente != null)            
                txtOrientador.Text = docente.NombreCompletoDocente;            
            else             
                txtOrientador.Text = "Sin orientador";
            

            // obtener ubicacion
            LoadUbicacion(alumno);

            // Datos de Usuario
            Usuario usuario = usuarioCtrl.LastDataRowToUsuario(usuarioCtrl.Retrieve(dctx, usuarioAlumno));
            txtEmail.Text = usuario.Email;
            txtTelCasa.Text = usuario.TelefonoCasa;
            txtTelReferencia.Text = usuario.TelefonoReferencia;

            // Obtener interes
            List<InteresAspirante> interesesAspirante = expedienteEscolarCtrl.RetrieveInteresesAspirante(dctx, alumno, pruebaDinamica).Distinct().ToList();
            gvIntereses.DataSource = interesesAspirante.ToList();
            gvIntereses.DataBind();

            // Para obtener areas de conocmiento
            foreach (InteresAspirante clas in interesesAspirante)
            {
                if (arrAreaConocimiento.IndexOf(clas.clasificador.ClasificadorID) == -1)
                {
                    arrAreaConocimiento.Add(clas.clasificador.ClasificadorID);
                    areasConocimiento.Add(clas.clasificador);
                }
            }
            gvAreasConocimiento.DataSource = areasConocimiento.ToList();
            gvAreasConocimiento.DataBind();

            // Obtener pruebas
            List<RespuestaAlumno> respuestaAlumno = respuestaAlumnoCtrl.RetrieveRespuestaAlumno(dctx, alumno, pruebaDinamica, EEstadoPrueba.CERRADA).Distinct().ToList();

            sinPruebas.Visible = (respuestaAlumno.Count > 0) ? false : true;

            if (respuestaAlumno.Count > 0)
            {
                gvPruebasGratis.DataSource = respuestaAlumno;
                gvPruebasGratis.DataBind();
            }
            else
            {
                gvPruebasGratis.DataSource = null;
                gvPruebasGratis.DataBind();
            }
        }

        private void LoadCiudades(Ubicacion filter)
        {
            if (filter == null || filter.Ciudad == null)
                return;
            DataSet ds = ciudadCtrl.Retrieve(dctx, filter.Ciudad);
            ddlMunicipio.DataSource = ds;
            ddlMunicipio.DataValueField = "CiudadID";
            ddlMunicipio.DataTextField = "Nombre";
            ddlMunicipio.DataBind();
            ddlMunicipio.Items.Insert(0, new ListItem("", "0"));
        }
               
        private void LoadUbicacion(Alumno alumno)
        {

            if (alumno.Ubicacion.UbicacionID != null)
            {
                alumno.Ubicacion = ubicacionCtrl.LastDataRowToUbicacion(ubicacionCtrl.Retrieve(dctx, alumno.Ubicacion));

                LoadPaises(new Ubicacion { Pais = new Pais { PaisID = alumno.Ubicacion.Pais.PaisID } });
                ddlPais.SelectedValue = alumno.Ubicacion.Pais != null ? alumno.Ubicacion.Pais.PaisID.ToString() : null;

                LoadEstados(new Ubicacion { Estado = new Estado { Pais = alumno.Ubicacion.Pais } });
                ddlEstado.SelectedValue = alumno.Ubicacion.Estado != null ? alumno.Ubicacion.Estado.EstadoID.ToString() : null;

                LoadCiudades(new Ubicacion { Ciudad = new Ciudad { Estado = alumno.Ubicacion.Estado } });
                ddlMunicipio.SelectedValue = alumno.Ubicacion.Ciudad != null ? alumno.Ubicacion.Ciudad.CiudadID.ToString() : null;
            }

            else
            {
                LoadPaises(new Ubicacion { Pais = new Pais() });
            }
        }

        private void LoadEstados(Ubicacion filter)
        {
            if (filter == null || filter.Estado == null)
                return;
            DataSet ds = estadoCtrl.Retrieve(dctx, filter.Estado);

            ddlEstado.DataSource = ds;
            ddlEstado.DataValueField = "EstadoID";
            ddlEstado.DataTextField = "Nombre";
            ddlEstado.DataBind();
            ddlEstado.Items.Insert(0, new ListItem("", "0"));
        }

        private void LoadPaises(Ubicacion filter)
        {
            if (filter == null || filter.Pais == null)
                return;
            DataSet ds = paisCtrl.Retrieve(dctx, filter.Pais);
            ddlPais.DataSource = ds;
            ddlPais.DataValueField = "PaisID";
            ddlPais.DataTextField = "Nombre";
            ddlPais.DataBind();
            ddlPais.Items.Insert(0, new ListItem("", "0"));
        }
        #endregion
        #region Autorizacion de la pagina
        protected override void DisplayCreateAction()
        {
            throw new NotImplementedException();
        }
        protected override void DisplayReadAction()
        {
            divContenido.Visible = true;
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