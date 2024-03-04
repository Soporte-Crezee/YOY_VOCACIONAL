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
using POV.Core.Administracion.Implements;
using POV.Core.Administracion.Interfaces;
using POV.Licencias.Service;
using POV.Seguridad.BO;
using POV.Seguridad.Service;
using POV.Web.Administracion.Helper;
using POV.Administracion.Service;
using POV.Web.Administracion.AppCode.Page;

namespace POV.Web.Administracion.Docentes
{
    public partial class EditarDocente : PageBase
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

        public  EditarDocente()
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
                if (userSession == null || !userSession.IsLogin())
                    redirector.GoToLoginPage(true);

                if (userSession.CurrentEscuela == null || userSession.CurrentCicloEscolar == null)
                    redirector.GoToError(true);

                if (!IsPostBack)
                {
                    if (LastObject != null)
                        LoadDocenteEscuela();
                    else
                        redirector.GoToConsultarDocente(true);
                }
            }
            catch (Exception ex)
            {
                this.ShowMessage(ex.Message, MessageType.Error);
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
                sError += " ,Primer Apellido";
            if (txtFechaNacimiento.Text.Trim().Length <= 0)
                sError += " ,Fecha Nacimiento";
            if (txtCorreo.Text.Trim().Length <= 0)
                sError += " ,Correo";
            if (CbxSexo.SelectedIndex == -1 || CbxSexo.SelectedValue.Length <= 0)
                sError += " ,Sexo";

            // CG
            //Valores Requeridos.
            if (txtSkype.Text.Trim().Length <= 0)
                sError += " ,Usuario skype";
            if (txtCedula.Text.Trim().Length <= 0)
                sError += " ,Cedula";
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
                sError += " ,Primer Apellido";
            if (txtCorreo.Text.Trim().Length > 100)
                sError += " ,Correo Electrónico";

            if (sError.Trim().Length > 0)
            {
                sError = sError.Substring(2);
                throw new Exception(string.Format("Los siguientes parámetros son inválidos :{0}", sError));
            }

            if (!ValidateCurpRegex(txtCurp.Text.Trim()))
                sError += " ,Curp";

            if (!ValidateEmailRegex(txtCorreo.Text.Trim()))
                sError += " ,Correo Electrónico";

            DateTime fn;
            if (!DateTime.TryParseExact(txtFechaNacimiento.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out fn))
                sError += " ,Fecha Nacimiento";
            
            //Valores con Incorrectos.
            if (txtSkype.Text.Trim().Length > 100)
                sError += " ,Usuario skype";
            if (txtCedula.Text.Trim().Length > 10)
                sError += " ,Cedula";

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

        private bool ValidateCurpRegex(string curp)
        {
            curp = curp.ToUpper();
            string patternLenient = @"[A-Z]{1}[AEIOUX]{1}[A-Z]{2}[0-9]{2}"
                + "(0[1-9]|1[0-2])(0[1-9]|1[0-9]|2[0-9]|3[0-1])" +
                "[HM]{1}" +
                "(AS|BC|BS|CC|CS|CH|CL|CM|DF|DG|GT|GR|HG|JC|MC|MN|MS|NT|NL|OC|PL|QT|QR|SP|SL|SR|TC|TS|TL|VZ|YN|ZS|NE)" +
                "[B-DF-HJ-NP-TV-Z]{3}" +
                "[0-9A-Z]{1}[0-9]{1}$";
            Regex reLenient = new Regex(patternLenient);

            bool match = reLenient.IsMatch(curp);
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
            Usuario usuario = licenciaEscuelaCtrl.RetrieveUsuario(ConnectionHlp.Default.Connection, new Docente { Curp = docente.Curp });
            usuario = usuarioCtrl.LastDataRowToUsuario(usuarioCtrl.Retrieve(ConnectionHlp.Default.Connection, usuario));

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

            txtCorreo.Text = usuario.Email;
            txtCorreoEditar.Text = usuario.Email;
            txtNombreUsuario.Text = usuario.NombreUsuario;

            // CG
            txtSkype.Text = docente.UsuarioSkype;
            if (docente.EsPremium == true)
                CheckBox1.Checked = true;
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
            docente.EsPremium = bool.Parse(CheckBox1.Checked ? "True" : "False");
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

                redirector.GoToConsultarDocente(false);
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
        #endregion

        #region AUTORIZACION DE LA PAGINA

        protected override void AuthorizeUser()
        {
            //ejemplificando la ejecucion
            List<Permiso> permisos = userSession.CurrentPrivilegiosDirector.GetPermisos();

            bool acceso = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.PAACCESODOCENTES) != null;
            bool lectura = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.PACONSULTARDOCENTES) != null;
            bool edit = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.PAEDITARDOCENTES) != null;

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