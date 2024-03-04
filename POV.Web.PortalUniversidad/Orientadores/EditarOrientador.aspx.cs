using System;
using System.Data;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;
using Framework.Base.Exceptions;
using POV.CentroEducativo.BO;
using POV.CentroEducativo.Service;
using POV.Licencias.Service;
using POV.Seguridad.BO;
using POV.Seguridad.Service;
using POV.Web.PortalUniversidad.Helper;
using POV.Administracion.Service;
using POV.Web.PortalUniversidad.AppCode.Page;
using POV.CentroEducativo.Services;
using POV.Logger.Service;

namespace POV.Web.PortalUniversidad.Orientadores
{
    public partial class EditarOrientador : PageBase
    {
        private DocenteCtrl docenteCtrl;
        private EscuelaCtrl escuelaCtrl;
        private LicenciaEscuelaCtrl licenciaEscuelaCtrl;
        private UsuarioCtrl usuarioCtrl;

        #region *** propiedades de clase ***
        public AsignacionDocenteEscuela LastObject
        {
            get { return Session["LastAsignacionDocente"] != null ? (AsignacionDocenteEscuela)Session["LastAsignacionDocente"] : null; }
            set { Session["LastAsignacionDocente"] = value; }
        }
        #endregion

        public  EditarOrientador()
        {
            docenteCtrl = new DocenteCtrl();
            escuelaCtrl = new EscuelaCtrl();
            licenciaEscuelaCtrl = new LicenciaEscuelaCtrl();
            usuarioCtrl = new UsuarioCtrl();
        }

        #region *** eventos de pagina ***
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                HabilitarInputs(false);
                if (userSession == null || !userSession.IsLogin())
                    redirector.GoToLoginPage(true);

                if (userSession.CurrentEscuela == null || userSession.CurrentCicloEscolar == null)
                    redirector.GoToError(true);

                if (!IsPostBack)
                {
                    if (LastObject != null)
                        LoadDocenteEscuela();
                    else
                        redirector.GoToConsultarOrientador(true);
                }
            }
            catch (Exception ex)
            {
                this.ShowMessage(ex.Message, MessageType.Error);
                LoggerHlp.Default.Error(this, ex);
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
                this.ShowMessage(ex.Message, MessageType.Error);
            }
        }

        protected void btnCancelarPerfil_Click(object sender, EventArgs e)
        {
            redirector.GoToConsultarOrientador(false);
        }

        #endregion

        #region *** validaciones ***
        private void ValidateData()
        {
            string sError = string.Empty;

            //Valores Requeridos.
            if (txtCurp.Text.Trim().Length <= 0)
                sError += " ,CURP";
            if (txtNombre.Text.Trim().Length <= 0)
                sError += " ,Nombre";
            if (txtPrimerApellido.Text.Trim().Length <= 0)
                sError += " ,Primer apellido";
            if (txtFechaNacimiento.Text.Trim().Length <= 0)
                sError += " ,Fecha nacimiento";
            if (txtCorreo.Text.Trim().Length <= 0)
                sError += " ,Correo";
            if (CbxSexo.SelectedIndex == -1 || CbxSexo.SelectedValue.Length <= 0)
                sError += " ,Sexo";

            // CG
            //Valores Requeridos.
            if (txtSkype.Text.Trim().Length <= 0)
                sError += " ,Usuario skype";
            if (txtCedula.Text.Trim().Length <= 0)
                sError += " ,Cédula";
            if (ddlNivelEstudio.SelectedValue.Length <= 0)
                sError += " ,Nivel estudio";
            if (txtTitulo.Text.Trim().Length <= 0)
                sError += " ,Título";
            if (txtEspecialidades.Text.Length <= 0)
                sError += " ,Especialidades";
            if (txtExperiencia.Text.Length <= 0)
                sError += " ,Experiencia";
            if (txtCursos.Text.Length <= 0)
                sError += " ,Cursos";

            if (sError.Trim().Length > 0)
            {
                sError = sError.Substring(2);
                throw new Exception(string.Format("Los siguientes parámetros son requeridos:{0}", sError));
            }


            //Valores con Incorrectos.
            if (txtCurp.Text.Trim().Length > 18)
                sError += " ,CURP";
            if (txtNombre.Text.Trim().Length > 80)
                sError += " ,Nombre";
            if (txtPrimerApellido.Text.Trim().Length > 50)
                sError += " ,Primer apellido";
            if (txtCorreo.Text.Trim().Length > 100)
                sError += " ,Correo electrónico";
            if (this.txtCedula.Text.Trim().Length > 10)
                sError += " ,Cédula";
            if (txtSegundoApellido.Text.Trim().Length > 50)
                sError += " ,Segundo apellido";
            if (txtTitulo.Text.Trim().Length > 100)
                sError += " ,Título";

            if (sError.Trim().Length > 0)
            {
                sError = sError.Substring(2);
                throw new Exception(string.Format("Los siguientes parámetros son inválidos :{0}", sError));
            }


            if (!ValidateEmailRegex(txtCorreo.Text.Trim()))
                sError += " ,Correo electrónico";

            DateTime fn;
            if (!DateTime.TryParseExact(txtFechaNacimiento.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out fn))
                sError += " ,Fecha nacimiento";
            
            //Valores con Incorrectos.
            if (txtSkype.Text.Trim().Length > 100)
                sError += " ,Usuario skype";
            if (txtCedula.Text.Trim().Length > 10)
                sError += " ,Cédula";

            if (txtTitulo.Text.Trim().Length > 100)
                sError += " ,Título";
            
            if (sError.Trim().Length > 0)
            {
                sError = sError.Substring(2);
                throw new Exception(string.Format("Los siguientes parámetros tienen un formato no válido :{0}", sError));
            }
        }
        private bool ValidateEmailRegex(string email)
        {
            string patternLenient = @"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";
            Regex reLenient = new Regex(patternLenient);
            bool match = reLenient.IsMatch(email);
            return match;
        }
        #endregion

        #region *** Data to UserInterface ***
        private void LoadDocenteEscuela()
        {
            if (LastObject == null || LastObject.AsignacionDocenteEscuelaID == null)
                throw new Exception("EditarDocente LoadDocenteEscuela: Ocurrió un error al procesar su solicitud");

            AsignacionDocenteEscuela asignacion = LoadAsignacionDocenteEscuela();
            DataSet ds = docenteCtrl.Retrieve(ConnectionHlp.Default.Connection, new Docente { DocenteID = asignacion.Docente.DocenteID, Estatus = true });
            Docente docente = docenteCtrl.LastDataRowToDocente(ds);
            Usuario usuario = new Usuario();
            if (userSession.CurrentUniversidad.UniversidadID != null)
            {
                usuario = licenciaEscuelaCtrl.RetrieveUsuarios(ConnectionHlp.Default.Connection, docente).Where(x => x.UniversidadId == userSession.CurrentUniversidad.UniversidadID).FirstOrDefault();
                if (usuario == null)
                    usuario = licenciaEscuelaCtrl.RetrieveUsuario(ConnectionHlp.Default.Connection, docente);
            }
            else
                usuario = licenciaEscuelaCtrl.RetrieveUsuario(ConnectionHlp.Default.Connection, docente);
            
            asignacion.Docente = docente;
            LastObject = asignacion;
            DataToUserInterface(asignacion.Docente, usuario);
        }
        private void DataToUserInterface(Docente docente, Usuario usuario)
        {
            txtCurp.Text = docente.Curp;
            txtNombre.Text = docente.Nombre;
            txtPrimerApellido.Text = docente.PrimerApellido;
            txtSegundoApellido.Text = docente.SegundoApellido;
            txtFechaNacimiento.Text = docente.FechaNacimiento != null ? string.Format("{0:dd/MM/yyyy}", docente.FechaNacimiento.Value) : string.Empty;

            if (docente.Sexo != null)
                CbxSexo.SelectedValue = (bool)docente.Sexo ? "True" : "False";

            txtNombreUsuario.Text = usuario.NombreUsuario;
            txtNombreUsuarioEdit.Text = usuario.NombreUsuario;

            txtCorreo.Text = usuario.Email;
            txtCorreoEdit.Text = usuario.Email;

            txtSkype.Text = docente.UsuarioSkype;
            txtCedula.Text = docente.Cedula;
            if(docente.NivelEstudio!=null)
            ddlNivelEstudio.Items.FindByText(docente.NivelEstudio).Selected=true;
            txtTitulo.Text = docente.Titulo;
            txtEspecialidades.Text = docente.Especialidades;
            txtExperiencia.Text = docente.Experiencia;
            txtCursos.Text = docente.Cursos;
        }
        #endregion

        #region *** UserInterface to Data ***
        private Docente UserInterfaceToData()
        {
            Docente docente = new Docente();
            docente.Nombre = txtNombre.Text.Trim();
            docente.PrimerApellido = txtPrimerApellido.Text.Trim();
            docente.SegundoApellido = txtSegundoApellido.Text.Trim();
            docente.FechaNacimiento = DateTime.ParseExact(txtFechaNacimiento.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
            docente.Correo = txtCorreo.Text.Trim();
            docente.Sexo = bool.Parse(CbxSexo.SelectedValue);
            docente.Curp = txtCurp.Text.Trim();
            // CG
            docente.UsuarioSkype = txtSkype.Text.Trim();
            docente.EsPremium = false;
            docente.Cedula = txtCedula.Text.Trim();
            docente.NivelEstudio = ddlNivelEstudio.SelectedItem.Text;
            docente.Titulo = txtTitulo.Text.Trim();
            docente.Especialidades = txtEspecialidades.Text.Trim();
            docente.Experiencia = txtExperiencia.Text.Trim();
            docente.Cursos = txtCursos.Text.Trim();
            return docente;
        }
        #endregion

        #region *** metodos auxiliares ***
        private void DoUpdate()
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
                CatalogoDocentesCtrl catDocentesCtrl = new CatalogoDocentesCtrl();
                Docente docente = UserInterfaceToData();
                docente.DocenteID = LastObject.Docente.DocenteID;

                catDocentesCtrl.UpdateDocenteEscuela(ConnectionHlp.Default.Connection, userSession.CurrentEscuela, userSession.CurrentCicloEscolar, docente);
                LastObject = null;

                // Se recarga las relaciones de la universidad
                UniversidadCtrl universidadCtrl = new UniversidadCtrl(null);
                userSession.CurrentUniversidad = universidadCtrl.RetrieveWithRelationship(new Universidad() { UniversidadID = userSession.CurrentUniversidad.UniversidadID }, false).FirstOrDefault();

                redirector.GoToConsultarOrientador(false);
            }
            catch (Exception ex)
            {
                this.ShowMessage(ex.Message, MessageType.Error);
            }
        }

        private AsignacionDocenteEscuela LoadAsignacionDocenteEscuela()
        {
            //Carga Datos Escuela y Ciclo Escolar
            Escuela escuela = escuelaCtrl.RetrieveComplete(ConnectionHlp.Default.Connection, userSession.CurrentEscuela);


            if (LastObject == null || LastObject.AsignacionDocenteEscuelaID == null)
                throw new Exception("BuscarDocentes: Ocurrió un error al procesar su solicitud");

            if (escuela.AsignacionDocentes.Any())
            {

                AsignacionDocenteEscuela asignacion = (escuela.AsignacionDocentes.Where(asig => asig.Activo == true && asig.AsignacionDocenteEscuelaID == LastObject.AsignacionDocenteEscuelaID)).First();

                return asignacion;
            }
            return null;
        }

        private void HabilitarInputs(bool habilita)
        {
            if (habilita)
                ClearInputs();
            txtCurp.Enabled = habilita;
            txtNombre.Enabled = habilita;
            txtPrimerApellido.Enabled = habilita;
            txtSegundoApellido.Enabled = habilita;
            txtFechaNacimiento.Enabled = habilita;

            CbxSexo.Enabled = habilita;


            txtCorreo.Enabled = habilita;

            txtSkype.Enabled = habilita;
            txtCedula.Enabled = habilita;
            ddlNivelEstudio.Enabled = habilita;
            txtTitulo.Enabled = habilita;
            txtEspecialidades.Enabled = habilita;
            txtExperiencia.Enabled = habilita;
            txtCursos.Enabled = habilita;
        }
        private void ClearInputs()
        {
            txtNombre.Text = String.Empty;
            txtPrimerApellido.Text = String.Empty;
            txtSegundoApellido.Text = String.Empty;
            txtFechaNacimiento.Text = String.Empty;

            CbxSexo.Text = String.Empty;


            txtCorreo.Text = String.Empty;

            txtSkype.Text = String.Empty;
            txtCedula.Text = String.Empty;
            ddlNivelEstudio.Text = String.Empty;
            txtTitulo.Text = String.Empty;
            txtEspecialidades.Text = String.Empty;
            txtExperiencia.Text = String.Empty;
            txtCursos.Text = String.Empty;
        }

        #endregion

        #region AUTORIZACION DE LA PAGINA

        protected override void AuthorizeUser()
        {
            //ejemplificando la ejecucion
            List<Permiso> permisos = userSession.CurrentPrivilegiosUniversidad.GetPermisos();

            bool acceso = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.PUACCESOORIENTADOR) != null;
            bool lectura = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.PUACCESOORIENTADOR) != null;
            bool edit = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.PUACCESOORIENTADOR) != null;

            if (!acceso)
                redirector.GoToHomePage(true);
            if (!lectura)
                redirector.GoToHomePage(true);
            if (!edit)
                redirector.GoToHomePage(true);
        }
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
        #endregion        
    }
}