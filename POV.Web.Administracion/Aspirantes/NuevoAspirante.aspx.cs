using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Administracion.Service;
using POV.CentroEducativo.BO;
using POV.CentroEducativo.Service;
using POV.Core.Administracion.Implements;
using POV.Core.Administracion.Interfaces;
using POV.Seguridad.BO;
using POV.Web.Administracion.Helper;
using POV.Web.Administracion.AppCode.Page;
using POV.Comun.Service;
using POV.Logger.Service;
using System.Net.Mail;
using System.Configuration;
using System.IO;
using POV.Localizacion.Service;
using POV.Localizacion.BO;
using POV.Comun.BO;
using POV.CentroEducativo.Services;
using POV.Seguridad.Utils;
using POV.Modelo.Context;
using POV.Expediente.BO;
using POV.Expediente.Services;
using POV.Licencias.Service;
using POV.Seguridad.Service;

namespace POV.Web.Administracion.Aspirantes
{
    public partial class NuevoAspirante : System.Web.UI.Page //: PageBase
    {
        #region *** propiedades de clase ***
        private CatalogoAlumnosCtrl catalogoAlumnosCtrl;
        private AlumnoCtrl alumnoCtrl;
        private CatalogoTutoresCtrl catalogoTutoresCtrl;

        private UbicacionCtrl ubicacionCtrl;
        private PaisCtrl paisCtrl;
        private EstadoCtrl estadoCtrl;
        private CiudadCtrl ciudadCtrl;

        private AsignacionUniversidadCtrl universidadesCtrl;
        private DocenteCtrl docenteCtrl;

        private UsuarioCtrl usuarioCtrl;

        private readonly IDataContext dctx = ConnectionHlp.Default.Connection;

        public Universidad UniversidadSession
        {
            get { return Session["SessionUniversidad"] != null ? (Universidad)Session["SessionUniversidad"] : null; }
            set { Session["SessionUniversidad"] = value; }
        }
        #endregion

        public NuevoAspirante()
        {
            catalogoAlumnosCtrl = new CatalogoAlumnosCtrl();
            alumnoCtrl = new AlumnoCtrl();

            catalogoTutoresCtrl = new CatalogoTutoresCtrl();

            ubicacionCtrl = new UbicacionCtrl();
            paisCtrl = new PaisCtrl();
            estadoCtrl = new EstadoCtrl();
            ciudadCtrl = new CiudadCtrl();

            universidadesCtrl = new AsignacionUniversidadCtrl();
            docenteCtrl = new DocenteCtrl();

            usuarioCtrl = new UsuarioCtrl();
        }

        #region *** eventos de pagina ***
        protected void Page_Load(object sender, EventArgs e)
        {            
            if (!IsPostBack)
            {
                LoadPaises(new Ubicacion { Pais = new Pais() });
                formTutor.Visible = false;
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
                ShowMessage(ex.Message, MessageType.Error, string.Empty);
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect(ConfigurationManager.AppSettings["POVUrlPortalSocial"]);
        }

        protected void btnValidarUsuario_Click(object sender, EventArgs e)
        {

        }

        protected void ddlPais_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlPais.SelectedIndex > 0)
                {
                    ddlEscuela.DataSource = null;
                    ddlEscuela.DataBind();
                    ddlEscuela.Items.Clear();
                    ddlEscuela.ClearSelection();
                    formTutor.Visible = false;
                    UniversidadSession = null;
                    LoadEstados(new Ubicacion { Estado = new Estado { Pais = new Pais { PaisID = int.Parse(ddlPais.SelectedItem.Value) } } });
                }
                else
                {
                    ddlEstado.DataSource = null;
                    ddlEstado.DataBind();
                    ddlEstado.ClearSelection();
                    ddlEstado.Items.Clear();
                    ddlMunicipio.DataSource = null;
                    ddlMunicipio.DataBind();
                    ddlMunicipio.ClearSelection();
                    ddlMunicipio.Items.Clear();
                    ddlEscuela.DataSource = null;
                    ddlEscuela.DataBind();
                    ddlEscuela.ClearSelection();
                    ddlEscuela.Items.Clear();
                    formTutor.Visible = false;
                    UniversidadSession = null;
                }
                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "reloadUbicacion(1);", true);
            }
            catch (Exception ex)
            {
                POV.Logger.Service.LoggerHlp.Default.Error(this, ex);
            }
        }

        protected void ddlEstado_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlEstado.SelectedIndex > 0)
                {
                    ddlEscuela.DataSource = null;
                    ddlEscuela.DataBind();
                    ddlEscuela.Items.Clear();
                    ddlEscuela.ClearSelection();
                    formTutor.Visible = false;
                    UniversidadSession = null;
                    LoadCiudades(new Ubicacion { Ciudad = new Ciudad { Estado = new Estado { EstadoID = int.Parse(ddlEstado.SelectedItem.Value) } } });
                }
                else
                {
                    ddlMunicipio.DataSource = null;
                    ddlMunicipio.DataBind();
                    ddlMunicipio.Items.Clear();
                    ddlMunicipio.ClearSelection();
                    ddlEscuela.DataSource = null;
                    ddlEscuela.DataBind();
                    ddlEscuela.Items.Clear();
                    ddlEscuela.ClearSelection();
                    formTutor.Visible = false;
                    UniversidadSession = null;
                }
                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "reloadUbicacion(2);", true);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        protected void ddlMunicipio_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlMunicipio.SelectedIndex > 0)
            {
                ddlEscuela.DataSource = null;
                ddlEscuela.DataBind();
                ddlEscuela.Items.Clear();
                ddlEscuela.ClearSelection();
                formTutor.Visible = false;
                UniversidadSession = null;
                LoadEscuelas(new Universidad { Activo = true });
            }
            else
            {
                ddlEscuela.DataSource = null;
                ddlEscuela.DataBind();
                ddlEscuela.Items.Clear();
                ddlEscuela.ClearSelection();
                formTutor.Visible = false;
                UniversidadSession = null;
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "reloadUbicacion(3);", true);
        }

        protected void ddlEscuela_SelectedIndexChanged(object sender, EventArgs e)
        {
            UniversidadSession = null;
            UniversidadCtrl universidadCtrl = new UniversidadCtrl(null);
            Universidad universidad;
            if (ddlMunicipio.SelectedIndex > 0)
            {
                universidad = universidadCtrl.Retrieve(new Universidad { UniversidadID = ddlEscuela.SelectedIndex > 0 ? int.Parse(ddlEscuela.SelectedItem.Value) : (int?)null }, true).FirstOrDefault();
                if (universidad.UniversidadID != null)
                {
                    if (universidad.NivelEscolar == ENivelEscolar.Superior)
                        formTutor.Visible = false;
                    else
                        formTutor.Visible = true;
                    UniversidadSession = universidad;
                }
            }
            else
            {
                throw new Exception("Debes seleccionar una escuela.");
            }

            ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "reloadUbicacion(22);", true);
        }
        #endregion

        #region *** metodos auxiliares ***
        #region *** validaciones ***
        #region *** Estudiante ***
        private void AlumnoUbicacionValidateData()
        {
            string sError = string.Empty;
            if (ddlPais.SelectedIndex == 0 || ddlPais.SelectedValue.Length <= 0)
                sError += " ,País";
            if ((ddlPais.SelectedIndex > 0) && ddlEstado.SelectedIndex == 0)
                sError += " ,Estado";

            if (sError.Trim().Length > 0)
            {
                sError = sError.Substring(2);
                throw new Exception(string.Format("Los siguientes parámetros son requeridos: {0}", sError));
            }

        }
        private void AlumnoValidateData()
        {
            //Campos Requeridos
            string sError = string.Empty;

            // Escuela
            if (ddlPais.SelectedIndex == -1 || ddlPais.SelectedValue.Length <= 0)
                sError += " ,País";

            if (ddlEstado.SelectedIndex == -1 || ddlEstado.SelectedValue.Length <= 0)
                sError += " ,Estado";

            if (ddlMunicipio.SelectedIndex == -1 || ddlMunicipio.SelectedValue.Length <= 0)
                sError += " ,Cuidad";

            if (txtNombre.Text.Trim().Length <= 0)
                sError += " ,Nombre";
            if (txtPrimerApellido.Text.Trim().Length <= 0)
                sError += " ,Primer Apellido";
            if (CbSexo.SelectedIndex == -1 || CbSexo.SelectedValue.Length <= 0)
                sError += " ,Sexo";
            if (sError.Trim().Length > 0)
            {
                sError = sError.Substring(2);
                throw new Exception(string.Format("Los siguientes parámetros son requeridos para estudiante: {0}", sError));
            }

            // Valores incorrectos
            if (txtNombre.Text.Trim().Length > 80)
                sError += " ,Nombre";
            if (txtPrimerApellido.Text.Trim().Length > 50)
                sError += " ,Primer Apellido";
            if (txtSegundoApellido.Text.Trim().Length > 50)
                sError += " ,Segundo Apellido";
            if (txtFechaNacimiento.Text.Trim().Length <= 0)
                sError += " ,Fecha Nacimiento";

            if (sError.Trim().Length > 0)
            {
                sError = sError.Substring(2);
                throw new Exception(string.Format("Los siguientes parámetros son inválidos para estudiante: {0}", sError));
            }
            if (txtCurp.Text.Trim().Length > 0 && !ValidateCurpRegex(txtCurp.Text.Trim()))
                sError += " ,Curp";

            // Formatos incorrectos
            DateTime fn;
            if (!DateTime.TryParseExact(txtFechaNacimiento.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out fn))
                sError += " ,Fecha Nacimiento";

            if (sError.Trim().Length > 0)
            {
                sError = sError.Substring(2);
                throw new Exception(string.Format("Los siguientes parámetros tienen un formato no valido para estudiante: {0}", sError));
            }
        }
        private void UsuarioValidateData()
        {
            string sError = string.Empty;
            //Campos Validos 
            if (txtCorreoElectronico.Text.Trim().Length > 100)
                sError += " ,Correo";
            if (txtNombreUsuario.Text.Trim().Length > 50)
                sError += " ,Nombre de Usuario";
            if (txtPassword.Text.Trim().Length > 50)
                sError += " ,Contraseña";

            if (txtConfirmarCorreoElectronico.Text.Trim() != txtCorreoElectronico.Text.Trim())
                sError += " ,Confirmar correo";
            if (txtConfirmarPassword.Text.Trim() != txtPassword.Text.Trim())
                sError += " ,Confirmar Contraseña";


            if (sError.Trim().Length > 0)
            {
                sError = sError.Substring(2);
                throw new Exception(string.Format("Los siguientes parámetros son inválidos para usuario estudiante: {0}", sError));
            }
            //Formato Incorrecto           
            if (txtCorreoElectronico.Text.Trim().Length > 0 && !ValidateEmailRegex(txtCorreoElectronico.Text.Trim()))
                sError += " ,Correo";
            if (txtConfirmarCorreoElectronico.Text.Trim().Length > 0 && !ValidateEmailRegex(txtConfirmarCorreoElectronico.Text.Trim()))
                sError += " ,Confirmar correo";

            if (txtCurpTutor.Text.Trim().Length > 0 && !ValidateCurpRegex(txtCurpTutor.Text.Trim()))
                sError += " ,Curp";

            if (sError.Trim().Length > 0)
            {
                sError = sError.Substring(2);
                throw new Exception(string.Format("Los siguientes parámetros tienen un formato no valido para usuario estudiante: {0}", sError));
            }

            Usuario usrAlumno = new Usuario();
            usrAlumno.Email = txtConfirmarCorreoElectronico.Text.Trim();
            if (!EmailDisponible(dctx, usrAlumno))
            sError += " ,Correo electrónico";

            if (sError.Trim().Length > 0)
            {
                sError = sError.Substring(2);
                throw new Exception(string.Format("Los siguientes parámetros no se encuentran disponibles para usuario estudiante: {0}", sError));
            }
        }
        #endregion

        #region *** Padre/Madre ***
        private void TutorValidateData()
        {
            //Campos Requeridos
            string sError = string.Empty;

            if (txtNombreTutor.Text.Trim().Length <= 0)
                sError += " ,Nombre";
            if (txtPrimerApellidoTutor.Text.Trim().Length <= 0)
                sError += " ,Primer Apellido";
            if (txtFechaNacimientoTutor.Text.Trim().Length <= 0)
                sError += " ,Fecha Nacimiento";
            if (ddlSexoTutor.SelectedIndex == -1 || ddlSexoTutor.SelectedValue.Length <= 0)
                sError += " ,Sexo";
            if (sError.Trim().Length > 0)
            {
                sError = sError.Substring(2);
                throw new Exception(string.Format("Los siguientes parámetros son requeridos para padre/madre: {0}", sError));
            }

            // Valores incorrectos
            if (txtNombre.Text.Trim().Length > 80)
                sError += " ,Nombre";
            if (txtPrimerApellido.Text.Trim().Length > 50)
                sError += " ,Primer Apellido";
            if (txtSegundoApellido.Text.Trim().Length > 50)
                sError += " ,Segundo Apellido";

            if (sError.Trim().Length > 0)
            {
                sError = sError.Substring(2);
                throw new Exception(string.Format("Los siguientes parámetros son inválidos para padre/madre: {0}", sError));
            }
        }
        private void UsuarioTutorValidateData()
        {
            string sError = string.Empty;
            //Campos Validos 
            if (txtEmailTutor.Text.Trim().Length > 100)
                sError += " ,Correo";
            if (txtNombreUsuarioTutor.Text.Trim().Length > 50)
                sError += " ,Nombre de Usuario";
            if (txtPasswordTutor.Text.Trim().Length > 50)
                sError += " ,Contraseña";

            if (txtConfirmarEmailTutor.Text.Trim() != txtEmailTutor.Text.Trim())
                sError += " ,Confirmar Correo";
            if (txtConfirmarPasswordTutor.Text.Trim() != txtPassword.Text.Trim())
                sError += " ,Confirmar Contraseña";

            if (sError.Trim().Length > 0)
            {
                sError = sError.Substring(2);
                throw new Exception(string.Format("Los siguientes parámetros son inválidos para usuario padre/madre: {0}", sError));
            }
            //Formato Incorrecto
            DateTime fn;
            if (!DateTime.TryParseExact(txtFechaNacimientoTutor.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out fn))
                sError += " ,Fecha Nacimiento";

            if (txtEmailTutor.Text.Trim().Length > 0 && !ValidateEmailRegex(txtEmailTutor.Text.Trim()))
                sError += " ,Correo";
            if (txtConfirmarEmailTutor.Text.Trim().Length > 0 && !ValidateEmailRegex(txtConfirmarEmailTutor.Text.Trim()))
                sError += " ,Confirmar Correo";

            if (sError.Trim().Length > 0)
            {
                sError = sError.Substring(2);
                throw new Exception(string.Format("Los siguientes parámetros tienen un formato no valido para usuario padre/madre: {0}", sError));
            }

            Usuario usrTutor = new Usuario();
            usrTutor.Email = txtConfirmarCorreoElectronico.Text.Trim();
            if (!EmailDisponible(dctx, usrTutor))
                sError += " ,Correo electrónico";

            if (sError.Trim().Length > 0)
            {
                sError = sError.Substring(2);
                throw new Exception(string.Format("Los siguientes parámetros no se encuentran disponibles para usuario padre/madre: {0}", sError));
            }
        }

        private void UsuarioAlumnoTutorValidateData() 
        {
            string sError = string.Empty;

            if (txtCorreoElectronico.Text.Trim() == txtEmailTutor.Text.Trim())
                sError += " ,Correo electrónico";
            if (txtNombreUsuario.Text.Trim() == txtNombreUsuarioTutor.Text.Trim())
                sError += " ,Nombre usuario";

            if (sError.Trim().Length > 0)
            {
                sError = sError.Substring(2);
                throw new Exception(string.Format("No puedes porporcionar los mismo datos para el usuario padre/madre y estudiante: {0}", sError));
            }
        }
        #endregion

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

        public bool EmailDisponible(IDataContext dctx, Usuario usuario)
        {
            if (usuario == null || string.IsNullOrEmpty(usuario.Email))
                throw new ArgumentException("Email requerido", "usuario");

            DataSet ds = usuarioCtrl.Retrieve(dctx, new Usuario { Email = usuario.Email, EsActivo = true });
            int index = ds.Tables[0].Rows.Count;
            if (index == 1 && usuario.UsuarioID != null)
            {
                Usuario usr = usuarioCtrl.LastDataRowToUsuario(ds);
                return usr.UsuarioID == usuario.UsuarioID;
            }

            if (index <= 0)
                return true;

            return false;
        }
        #endregion

        #region *** UserInterface to Data ***
        #region *** Estudiante ***
        private Alumno AlumnoUserInterfaceToData()
        {
            Alumno alumno = new Alumno();

            //DateTime dateval;
            bool boolval;
            alumno.Nombre = !string.IsNullOrEmpty(txtNombre.Text.Trim()) ? txtNombre.Text.Trim() : string.Empty;
            alumno.PrimerApellido = !string.IsNullOrEmpty(txtPrimerApellido.Text.Trim()) ? txtPrimerApellido.Text.Trim() : string.Empty;
            alumno.SegundoApellido = !string.IsNullOrEmpty(txtSegundoApellido.Text.Trim()) ? txtSegundoApellido.Text.Trim() : string.Empty;

            string curpGen = Guid.NewGuid().ToString();
            char[] curpItem = curpGen.ToCharArray();
            curpGen = string.Empty;
            foreach (var item in curpItem)
            {
                if (curpGen.Length >= 15) break;
                if (item != '-') curpGen += item.ToString();
            }
            curpGen += new Random().Next(0, 999);
            alumno.Curp = !string.IsNullOrEmpty(txtCurp.Text.Trim()) ? txtCurp.Text.Trim() : string.Empty;

            string matriculaGen = Guid.NewGuid().ToString();
            char[] matriculaItem = curpGen.ToCharArray();
            matriculaGen = string.Empty;
            foreach (var item in matriculaItem)
            {
                if (matriculaGen.Length >= 45) break;
                if (item != '-') matriculaGen += item.ToString();
            }
            matriculaGen += new Random().Next(0, 99999);
            alumno.Matricula = matriculaGen;

            if (CbSexo.SelectedIndex != -1 && !string.IsNullOrEmpty(CbSexo.SelectedItem.Value))
                if (bool.TryParse(CbSexo.SelectedValue, out boolval)) alumno.Sexo = boolval;

            string fechaNacimiento = !string.IsNullOrEmpty(txtFechaNacimiento.Text.Trim()) ? txtFechaNacimiento.Text.Trim() : string.Empty;
            DateTime tmpFecha = new DateTime();
            tmpFecha = Convert.ToDateTime(fechaNacimiento);
            fechaNacimiento = tmpFecha.ToShortDateString();
            alumno.FechaNacimiento = DateTime.ParseExact(fechaNacimiento, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            alumno.Premium = false;

            return alumno;

        }
        private Usuario UsuarioUserInterfaceToData()
        {
            Usuario usuario = new Usuario();
            usuario.Email = !string.IsNullOrEmpty(txtCorreoElectronico.Text) ? txtCorreoElectronico.Text.Trim() : string.Empty;
            usuario.NombreUsuario = !string.IsNullOrEmpty(txtNombreUsuario.Text) ? txtNombreUsuario.Text.Trim() : string.Empty;

            return usuario;
        }
        #endregion

        #region *** Padre/Madre
        private Tutor TutorUserInterfaceToData()
        {
            Tutor tutor = new Tutor();

            //DateTime dateval;
            bool boolval;
            tutor.Nombre = !string.IsNullOrEmpty(txtNombreTutor.Text.Trim()) ? txtNombreTutor.Text.Trim() : string.Empty;
            tutor.PrimerApellido = !string.IsNullOrEmpty(txtPrimerApellidoTutor.Text.Trim()) ? txtPrimerApellidoTutor.Text.Trim() : string.Empty;
            tutor.SegundoApellido = !string.IsNullOrEmpty(txtSegundoApellidoTutor.Text.Trim()) ? txtSegundoApellidoTutor.Text.Trim() : string.Empty;
            tutor.Curp = !string.IsNullOrEmpty(txtCurpTutor.Text.Trim()) ? txtCurpTutor.Text.Trim() : string.Empty;
            string fechaNacimiento = !string.IsNullOrEmpty(txtFechaNacimientoTutor.Text.Trim()) ? txtFechaNacimientoTutor.Text.Trim() : string.Empty;
            DateTime tmpFecha = new DateTime();
            tmpFecha = Convert.ToDateTime(fechaNacimiento);
            fechaNacimiento = tmpFecha.ToShortDateString();
            tutor.FechaNacimiento = DateTime.ParseExact(fechaNacimiento, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            if (ddlSexoTutor.SelectedIndex != -1 && !string.IsNullOrEmpty(ddlSexoTutor.SelectedValue))
                if (bool.TryParse(ddlSexoTutor.SelectedValue, out boolval)) tutor.Sexo = boolval;

            tutor.CorreoElectronico = txtEmailTutor.Text.Trim();
            tutor.EstatusIdentificacion = false;
            tutor.Estatus = true;

            return tutor;

        }
        private Usuario UsuarioTutorUserInterfaceToData()
        {
            Usuario usuario = new Usuario();
            string password = !string.IsNullOrEmpty(txtPasswordTutor.Text) ? txtPasswordTutor.Text.Trim() : string.Empty;
            usuario.Email = !string.IsNullOrEmpty(txtEmailTutor.Text) ? txtEmailTutor.Text.Trim() : string.Empty;
            usuario.NombreUsuario = !string.IsNullOrEmpty(txtNombreUsuarioTutor.Text) ? txtNombreUsuarioTutor.Text.Trim() : string.Empty;
            usuario.Password = EncryptHash.SHA1encrypt(password);

            return usuario;
        }
        #endregion
        #endregion

        private void DoInsert()
        {
            try
            {
                #region Validaciones
                try
                {
                    AlumnoUbicacionValidateData();
                    AlumnoValidateData();
                    UsuarioValidateData();
                    if (UniversidadSession.UniversidadID != null)
                    {
                        if (UniversidadSession.NivelEscolar != ENivelEscolar.Superior)
                        {
                            TutorValidateData();
                            UsuarioTutorValidateData();
                            UsuarioAlumnoTutorValidateData();
                        }
                    }
                }
                catch (Exception ex)
                {                    
                    ShowMessage(ex.Message, MessageType.Error, string.Empty);
                    return;
                }
                #endregion

                #region Inicializar Conexion a BD
                // Entity
                object firma = new object();
                var contexto = new Contexto(firma);

                // Framework Anterior
                object con = new object();
                try
                {
                    dctx.OpenConnection(con);
                }
                catch (Exception ex)
                {
                    throw new Exception("Inconsistencias al conectarse a la base de datos.");
                }
                #endregion

                try
                {
                    object tran = new object();
                    try
                    {
                        dctx.BeginTransaction(tran);
                        try
                        {
                            EFAlumnoCtrl efAlumnoCtrl = new EFAlumnoCtrl(contexto);
                            UsuarioExpedienteCtrl usuarioExpedienteCtrl = new UsuarioExpedienteCtrl();
                            LicenciaEscuelaCtrl licenciaEscuelaCtrl = new LicenciaEscuelaCtrl();
                            TutorCtrl efTutorCtrl = new TutorCtrl(contexto);

                            // Estudiante
                            Alumno alumno = AlumnoUserInterfaceToData();

                            if (UniversidadSession.UniversidadID != null)
                                alumno.NivelEscolar = UniversidadSession.NivelEscolar;
                            else
                                throw new Exception("Error al consultar la escuela.");

                            Alumno alumnoRegistroCorrecto = new Alumno();

                            // Usuario estudiante
                            Usuario usuario = UsuarioUserInterfaceToData();
                            string password = txtPassword.Text;

                            alumnoRegistroCorrecto = catalogoAlumnosCtrl.InsertAlumno(dctx, alumno, usuario, password);

                            #region Vicular Alumno-Escuela
                            Universidad escuela = universidadesCtrl.LastDataRowToUniversidad(universidadesCtrl.RetrieveUniversidad(dctx, new Universidad { UniversidadID = UniversidadSession.UniversidadID }));
                            UniversidadAlumno universidadAlumno = new UniversidadAlumno() { UniversidadID = escuela.UniversidadID, AlumnoID = alumnoRegistroCorrecto.AlumnoID };
                            
                            UniversidadAlumno universidadAlumnoRegistrado = universidadesCtrl.LastDataRowToUniversidadAlumno(universidadesCtrl.RetrieveUniversidadAlumno(dctx, universidadAlumno));
                            if (universidadAlumnoRegistrado == null)
                                universidadesCtrl.InsertUniversidadAlumno(dctx, universidadAlumno);
                            #endregion

                            #region Vicular Alumno-Orientador
                            Docente orientador = new Docente();                            
                            Usuario userOrientador = licenciaEscuelaCtrl.RetrieveUsuarios(dctx, new Docente { Estatus = true }, false).Where(x => x.UniversidadId == escuela.UniversidadID && x.EsActivo == true).FirstOrDefault();

                            if (userOrientador != null && userOrientador.UsuarioID != null)
                            {
                                UsuarioExpediente usuarioExpediente = new UsuarioExpediente() { AlumnoID = alumnoRegistroCorrecto.AlumnoID, UsuarioID = userOrientador.UsuarioID };
                                
                                UsuarioExpediente usuarioExpedienteRegistrado = usuarioExpedienteCtrl.LastDataRowToUsuarioExpediente(usuarioExpedienteCtrl.Retrieve(dctx, usuarioExpediente));
                                if (usuarioExpedienteRegistrado == null)
                                    usuarioExpedienteCtrl.Insert(dctx, usuarioExpediente);
                            }
                            else
                                throw new Exception("La escuela seleccionada no cuenta con orientadores para su vinculación");
                            #endregion
                            dctx.CommitTransaction(tran);

                            bool correoAlumno = false;
                            bool correoTutor = false;

                            if (alumnoRegistroCorrecto.AlumnoID != null)
                                correoAlumno = true;

                            Tutor tutor = null;
                            Usuario usuarioTutor = null;

                            if (UniversidadSession.NivelEscolar != ENivelEscolar.Superior)
                            {
                                // Padre/madre
                                tutor = TutorUserInterfaceToData();
                                // Usuario padre/madre
                                usuarioTutor = UsuarioTutorUserInterfaceToData();
                                Tutor exiteTutor = efTutorCtrl.Retrieve(new Tutor { Curp = tutor.Curp }, true).FirstOrDefault();
                                if (exiteTutor == null)
                                {
                                    var tutorRegistroCorrecto = catalogoTutoresCtrl.InsertTutor(dctx, tutor, usuarioTutor);
                                    if (tutorRegistroCorrecto)
                                        correoTutor = true;
                                }
                                else
                                {                                    
                                    ShowMessage("Ocurrió un error inesperado al registrar al usuario, inténtelo más tarde", MessageType.Error, string.Empty);                                 
                                }
                            }

                            if (correoAlumno)
                                enviarCorreo(usuario, alumnoRegistroCorrecto);
                            if (correoTutor)
                                enviarCorreo(usuarioTutor, tutor);

                            #region Vincular Alumno-Tutor
                            if (alumno.NivelEscolar != ENivelEscolar.Superior)
                            {
                                Alumno alumnoSelect = efAlumnoCtrl.Retrieve(new Alumno { Curp = alumno.Curp }, true).FirstOrDefault();
                                //Hacemos la vinculación
                                int parentesco = 0;
                                switch ((bool)tutor.Sexo)
                                {
                                    case true:
                                        parentesco = 1;
                                        break;
                                    case false:
                                        parentesco = 2;
                                        break;
                                }

                                TutorAlumno tutorAlumnoFind = new TutorAlumno();
                                Tutor tutorSelect = efTutorCtrl.Retrieve(new Tutor { Curp = tutor.Curp }, true).FirstOrDefault();
                                if (tutorSelect.TutorID != null)
                                {
                                    tutorAlumnoFind.Tutor = tutorSelect;
                                    tutorAlumnoFind.TutorID = tutorSelect.TutorID;
                                    tutorAlumnoFind.Parentesco = (Int16)parentesco;
                                    if (alumnoSelect.Tutores.FirstOrDefault(x => x.TutorID == tutorAlumnoFind.TutorID) == null)
                                    {
                                        alumnoSelect.Tutores.Add(tutorAlumnoFind);
                                    }
                                }
                                else
                                {
                                    throw new Exception("Hubo problemas al recuperar los datos del padre/madre intente de nuevo.");
                                }
                            }
                            #endregion

                            contexto.Commit(firma);                            
                            ShowMessage("¡Registro exitoso! Revisa tu bandeja de entrada y/o bandeja de correo no deseado, serás redirigido al portal.!", MessageType.Information, ConfigurationManager.AppSettings["POVUrlLandingPage"]);

                        }
                        catch (Exception ex)
                        {                            
                            ShowMessage(ex.Message, MessageType.Error,string.Empty);
                            dctx.RollbackTransaction(tran);
                            POV.Logger.Service.LoggerHlp.Default.Error(this, ex);
                        }
                    }
                    catch (Exception ex)
                    {                        
                        ShowMessage(ex.Message, MessageType.Error, string.Empty);
                        dctx.RollbackTransaction(tran);
                        POV.Logger.Service.LoggerHlp.Default.Error(this, ex);                     
                    }
                }
                finally
                {
                    if (dctx.ConnectionState == ConnectionState.Open)
                        dctx.CloseConnection(con);
                    contexto.Dispose();
                }

            }
            catch (Exception ex)
            {                
                ShowMessage(ex.Message, MessageType.Error,string.Empty);
            }
        }

        //Enviar Correo de registro exitoso
        private void enviarCorreo(Usuario usuario, Alumno alumno)
        {
            CorreoCtrl correoCtrl = new CorreoCtrl();
            #region Variables
            string password = "";
            string urllogo = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgLogo"];
            string urlimgbackground = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgBackground"];
            string urlimgface = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgFace"];
            string urlimgtwit = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgTwit"];
            string urlimglinked = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgLinked"];
            string urlimginstagram = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgInstagram"];
            const string imgalt = "YOY - Estudiantes";
            const string titulo = "Registro exitoso";
            string dominio = String.Format("{0}://{1}:{2}/", HttpContext.Current.Request.Url.Scheme, HttpContext.Current.Request.Url.Host, HttpContext.Current.Request.Url.Port);
            string linkportal = System.Configuration.ConfigurationManager.AppSettings["POVUrlPortalSocial"];
            string linkAutologin = linkportal + GenerarTokenYUrl(alumno);
            #endregion

            string cuerpo = string.Empty;
            using (StreamReader reader = new StreamReader(Server.MapPath("~/Files/protocolo/EmailTemplateNewUser.html")))
            {
                cuerpo = reader.ReadToEnd();
            }
            cuerpo = cuerpo.Replace("{urllogo}", urllogo);
            cuerpo = cuerpo.Replace("{urlimgbackground}", urlimgbackground);
            cuerpo = cuerpo.Replace("{title}", titulo);
            cuerpo = cuerpo.Replace("{linkportal}", linkAutologin);
            cuerpo = cuerpo.Replace("{urlimgface}", urlimgface);
            cuerpo = cuerpo.Replace("{urlimgtwit}", urlimgtwit);
            cuerpo = cuerpo.Replace("{urlimglinked}", urlimglinked);
            cuerpo = cuerpo.Replace("{urlimginstagram}", urlimginstagram);

            List<string> tos = new List<string>();
            tos.Add(usuario.Email);
            AlternateView texto = null;
            List<string> archivos = new List<string>();
            List<string> copias = new List<string>();
            try
            {
                correoCtrl.sendMessage(tos, "YOY - Registro exitoso", cuerpo, texto, archivos, copias);                
            }
            catch (Exception ex)
            {                
                LoggerHlp.Default.Error(this, ex);
            }
        }

        //Enviar Correo de registro exitoso
        private void enviarCorreo(Usuario usuario, Tutor tutor)
        {
            CorreoCtrl correoCtrl = new CorreoCtrl();
            #region Variables;
            string urllogo = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgLogo"];
            string urlimgbackground = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgBackground"];
            string urlimgface = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgFace"];
            string urlimgtwit = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgTwit"];
            string urlimglinked = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgLinked"];
            string urlimginstagram = System.Configuration.ConfigurationManager.AppSettings["POVUrlImgInstagram"];
            const string altimg = "YOY - Email";
            const string titulo = "Registro exitoso";
            string dominio = String.Format("{0}://{1}:{2}/", HttpContext.Current.Request.Url.Scheme, HttpContext.Current.Request.Url.Host, HttpContext.Current.Request.Url.Port);
            string linkportal = System.Configuration.ConfigurationManager.AppSettings["POVUrlPortalTutor"];
            #endregion

            string cuerpo = string.Empty;
            using (StreamReader reader = new StreamReader(Server.MapPath("~/Files/protocolo/EmailTemplateNewUserTutor.html")))
            {
                cuerpo = reader.ReadToEnd();
            }
            cuerpo = cuerpo.Replace("{urllogo}", urllogo);
            cuerpo = cuerpo.Replace("{urlimgbackground}", urlimgbackground);
            cuerpo = cuerpo.Replace("{title}", titulo);
            cuerpo = cuerpo.Replace("{linkportal}", linkportal);
            cuerpo = cuerpo.Replace("{urlimgface}", urlimgface);
            cuerpo = cuerpo.Replace("{urlimgtwit}", urlimgtwit);
            cuerpo = cuerpo.Replace("{urlimglinked}", urlimglinked);
            cuerpo = cuerpo.Replace("{urlimginstagram}", urlimginstagram);

            List<string> tos = new List<string>();
            tos.Add(usuario.Email);
            AlternateView texto = null;
            List<string> archivos = new List<string>();
            List<string> copias = new List<string>();
            try
            {
                correoCtrl.sendMessage(tos, "YOY - Registro exitoso", cuerpo, texto, archivos, copias);                
            }
            catch (Exception ex)
            {                
                LoggerHlp.Default.Error(this, ex);
            }
        }

        private string GenerarTokenYUrl(Alumno alumno)
        {
            string strUrlAutoLogin;
            string formatoFecha = "yyyyMMddHHmmss.fff";
            DateTime fecha = System.DateTime.Now;
            string nombre = alumno.Nombre;
            nombre = nombre.Trim();
            string apellido = alumno.PrimerApellido;
            apellido = apellido.Trim();
            string curp = alumno.Curp;
            DateTime fechaNacimiento = (DateTime)alumno.FechaNacimiento;
            curp = curp.Trim();
            string token = fechaNacimiento.ToString(formatoFecha) + nombre + apellido + curp + fecha.ToString(formatoFecha);
            byte[] clave = POV.Seguridad.Utils.EncryptHash.SHA1encrypt(token);
            token = POV.Seguridad.Utils.EncryptHash.byteArrayToStringBase64(clave);
            token = System.Web.HttpUtility.UrlEncode(token);
            strUrlAutoLogin = "?alumno=" + alumno.Curp + "&fechahora=" + fecha.ToString(formatoFecha) + "&token=" + token;
            return strUrlAutoLogin;
        }

        private void LoadPaises(Ubicacion filter)
        {
            if (filter == null || filter.Pais == null)
                return;
            DataSet ds = paisCtrl.Retrieve(ConnectionHlp.Default.Connection, filter.Pais);
            ddlPais.DataSource = ds;
            ddlPais.DataValueField = "PaisID";
            ddlPais.DataTextField = "Nombre";
            ddlPais.DataBind();
            ddlPais.Items.Insert(0, new ListItem("Seleccionar", ""));

        }

        private void LoadEstados(Ubicacion filter)
        {
            if (filter == null || filter.Estado == null)
                return;
            DataSet ds = estadoCtrl.Retrieve(ConnectionHlp.Default.Connection, filter.Estado);
            ddlEstado.DataSource = ds;
            ddlEstado.DataValueField = "EstadoID";
            ddlEstado.DataTextField = "Nombre";
            ddlEstado.DataBind();
            ddlEstado.Items.Insert(0, new ListItem("Seleccionar", ""));
        }

        private void LoadCiudades(Ubicacion filter)
        {
            if (filter == null || filter.Ciudad == null)
                return;
            DataSet ds = ciudadCtrl.Retrieve(ConnectionHlp.Default.Connection, filter.Ciudad);
            ddlMunicipio.DataSource = ds;
            ddlMunicipio.DataValueField = "CiudadID";
            ddlMunicipio.DataTextField = "Nombre";
            ddlMunicipio.DataBind();
            ddlMunicipio.Items.Insert(0, new ListItem("Seleccionar", ""));
        }

        private void LoadEscuelas(Universidad filter)
        {
            if (filter == null)
                return;


            UniversidadCtrl universidadCtrl = new UniversidadCtrl(null);
            List<Universidad> listUniversidad = universidadCtrl.Retrieve(filter, false).ToList();
            List<Universidad> listFilter = new List<Universidad>();


            foreach (var item in listUniversidad)
            {
                if (item.ClaveEscolar != "YOY")
                {
                    item.Ubicacion = new Ubicacion();
                    item.Ubicacion.Pais = new Pais { PaisID = ddlPais.SelectedIndex > 0 ? int.Parse(ddlPais.SelectedItem.Value) : (int?)null };
                    item.Ubicacion.Estado = new Estado { EstadoID = ddlEstado.SelectedIndex > 0 ? int.Parse(ddlEstado.SelectedItem.Value) : (int?)null };
                    item.Ubicacion.Ciudad = new Ciudad { CiudadID = ddlMunicipio.SelectedIndex > 0 ? int.Parse(ddlMunicipio.SelectedItem.Value) : (int?)null };

                    Ubicacion ubicacion = new Ubicacion();
                    DataSet dsUbicacion = ubicacionCtrl.RetrieveExacto(dctx, item.Ubicacion);
                    int index = dsUbicacion.Tables["Ubicacion"].Rows.Count;
                    if (index == 1)
                        ubicacion = ubicacionCtrl.LastDataRowToUbicacion(dsUbicacion);

                    if (ubicacion.UbicacionID != null && item.UbicacionID == ubicacion.UbicacionID)
                    {
                        listFilter.Add(item);
                    }
                }
            }
            ddlEscuela.DataSource = listFilter;
            ddlEscuela.DataValueField = "UniversidadID";
            ddlEscuela.DataTextField = "NombreUniversidad";
            ddlEscuela.DataBind();
            ddlEscuela.Items.Insert(0, new ListItem("Seleccionar", ""));

        }
        #endregion

        #region *****Message  Showing*****
        /// <summary>
        /// Desplega el mensaje de error/advertencia/información en la UI
        /// </summary>
        /// <param name="message">Mensaje a desplegar</param>
        /// <param name="messageType">Tipo de mensaje</param>
        private void ShowMessage(string message, MessageType messageType, string redirection)
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

            ShowMessage(message, type, redirection);
        }
        /// <summary>
        /// Desplega el mensaje de error/advertencia/información en la UI
        /// </summary>
        /// <param name="message">Mensaje a desplegar</param>
        /// <param name="typeNotification">1: Error, 2: Advertencia, 3: Información</param>
        private void ShowMessage(string message, string typeNotification, string redirection)
        {
            //Se ubican los controles que manejan el desplegado de error/advertencia/información
            if (Page.Master == null) return;
            Control m = Page.Master.FindControl("hdnLastMessage");
            Control t = Page.Master.FindControl("hdnShowMessage");
            Control r = Page.Master.FindControl("hdnRedirection");

            if (m == null)
                throw new Exception("No se pudo desplegar correctamente el error.\nNo se encontró el control 'hdnLastMessage' en la MasterPage.\nEl error original es:\n" + message);
            if (t == null)
                throw new Exception("No se pudo desplegar correctamente el error.\nNo se encontró el control 'hdnShowMessage' en la MasterPage.\nEl error original es:\n" + message);
            
            if (r == null)
                throw new Exception("No se pudo desplegar correctamente el error.\nNo se encontró el control 'hdnRedirection' en la MasterPage.\nEl error original es:\n" + message);

            if (m.GetType() != typeof(HiddenField) || t.GetType() != typeof(HiddenField) || r.GetType() != typeof(HiddenField))
                throw new Exception("No se pudo desplegar correctamente el error.\nAlguno de los controles de la MasterPage para el manejo de errores no es HiddenField.\nEl error original es:\n" + message);

            //Si el HiddenField del mensaje de error ya tiene un mensaje guardado, se da un 'enter' y se concatena el nuevo mensaje (errores acumulados)
            //En caso contrario, se pone el encabezado y se concatena el nuevo mensaje
            if (((HiddenField)m).Value != null && ((HiddenField)m).Value.Trim().CompareTo("") != 0)
                ((HiddenField)m).Value += "<br />";


            ((HiddenField)m).Value += message.Replace("\n", "<br />");
            ((HiddenField)t).Value = typeNotification;
            ((HiddenField)r).Value = redirection;
        }
        #endregion

        protected void CbSexo_SelectedIndexChanged(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "reloadUbicacion(6);", true);
        }

        protected void ddlSexoTutor_SelectedIndexChanged(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "reloadUbicacion(15);", true);
        }


    }
}