using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using POV.CentroEducativo.BO;
using POV.Logger.Service;
using POV.Prueba.Diagnostico.Dinamica.Service;
using POV.Web.Portal.Pruebas.Helper;
using Framework.Base.DataAccess;
using POV.Licencias.Service;
using POV.Prueba.BO;
using System.Data;
using POV.Licencias.BO;
using POV.Prueba.Diagnostico.Dinamica.BO;
using POV.CentroEducativo.Service;
using POV.Seguridad.Utils;
using System.IO;
using System.Collections;
using Framework.Base.Exceptions;
using System.Globalization;
using POV.Prueba.Diagnostico.Service;
using POV.Expediente.BO;

namespace POV.Web.Portal.Pruebas.Pages
{
    public partial class AccesoPrueba : System.Web.UI.Page
    {
        #region QueryString
        private string QS_CURP
        {
            get { return this.Request.QueryString["alumno"]; }
        }
        private string QS_FechaHora
        {
            get { return this.Request.QueryString["fechahora"]; }
        }
        private string QS_EscuelaID
        {
            get { return this.Request.QueryString["escuela"]; }
        }
        private string QS_GrupoCicloEscolarID
        {
            get { return this.Request.QueryString["grupo"]; }
        }
        
        private string QS_Token
        {
            get { return this.Request.QueryString["token"]; }
        }
        private string QS_PruebaID
        {
            get { return this.Request.QueryString["prueba"]; }
        }
        #region Ajuste 2
        private string QS_Prueba
        {
            get { return this.Request.QueryString["prueba"]; }
        }
        private string QS_Asignacion
        {
            get { return this.Request.QueryString["actividad"]; }
        }
        private string QS_Tarea
        {
            get { return this.Request.QueryString["tarea"]; }
        }
        private string QS_Talentos
        {
            get { return this.Request.QueryString["cmd"]; }
        }
        private string QS_ResultadoPrueba
        {
            get { return this.Request.QueryString["resultado"]; }
        }
        #endregion
        #endregion

        #region Session
        private Alumno Session_Alumno
        {
            get { return (Alumno)this.Session["Alumno"]; }
            set { this.Session["Alumno"] = value; }
        }

        private GrupoCicloEscolar Session_GrupoCicloEscolar
        {
            get { return (GrupoCicloEscolar)this.Session["GrupoCicloEscolar"]; }
            set { this.Session["GrupoCicloEscolar"] = value; }
        }

        private AResultadoPrueba Session_ResultadoPrueba
        {
            get { return (AResultadoPrueba)this.Session["ResultadoPrueba"]; }
            set { this.Session["ResultadoPrueba"] = value; }
        }
        private APrueba Session_Prueba
        {
            get { return (APrueba)this.Session["Prueba"]; }
            set { this.Session["Prueba"] = value; }
        }
        private long? Session_AsignacionActividadId
        {
            get { return this.Session["AsignacionActividadId"] as long?; }
            set { this.Session["AsignacionActividadId"] = value; }
        }

        private long? Session_TareaRealizadaId
        {
            get { return this.Session["TareaRealizadaId"] as long?; }
            set { this.Session["TareaRealizadaId"] = value; }
        }

        private LicenciaEscuela Session_LicenciaEscuela
        {
            get { return (LicenciaEscuela)this.Session["Session_LicenciaEscuela"]; }
            set { this.Session["Session_LicenciaEscuela"] = value; }
        }

        private bool? Session_EsPruebaPivote
        {
            get { return this.Session["Session_EsPruebaPivote"] as bool?; }
            set { this.Session["Session_EsPruebaPivote"] = value; }
        }

		#endregion

        private IDataContext dctx = ConnectionHlp.Default.Connection;
        PruebaDiagnosticoCtrl pruebaDiagnosticoCtrl;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                pruebaDiagnosticoCtrl = new PruebaDiagnosticoCtrl();

                string errores = "";

                if (!this.EsValidoQueryString(out errores))
                {
                    this.RedirigirRedSocial("Información incorrecta.");
                    return;
                }

                try
                {
                    if (!this.EsValidoToken())
                    {
                        this.RedirigirRedSocial("Información incorrecta.");
                        return;
                    }
                }
                catch (Exception ex)
                {
                    LoggerHlp.Default.Error(this, ex);
                    this.RedirigirRedSocial("No es posible validar la información del Usuario, intentalo nuevamente.");
                    return;
                }

                try
                {
                    if (QS_Talentos == null)
                    {
                        var pruebaPendiente = this.TienePruebaPendiente();
                        if (this.TienePruebaPendiente() == null)
                        {
                            this.RedirigirRedSocial("La prueba está finalizada.");
                            return;
                        }
                    }
                    else
                    {

                        btnAccesoPrueba_Click(sender, e);
                    }
                }
                catch (Exception ex)
                {
                    if (ex is System.Threading.ThreadAbortException)
                        return;
                    LoggerHlp.Default.Error(this, ex);
                        this.RedirigirRedSocial("No es posible iniciar ó continuar la prueba, intentalo nuevamente.");
                    return;
                }


                this.lblNombreAlumnoMensaje.Text = this.Session_Alumno.Nombre + " " + this.Session_Alumno.PrimerApellido + ((this.Session_Alumno.SegundoApellido != null && this.Session_Alumno.SegundoApellido.Trim().Length != 0) ? " " + this.Session_Alumno.SegundoApellido.Trim() : "");
                this.lblMensajeAcceso.Text = "Tienes una evaluación diagnóstica pendiente por favor ingresa tu \"Código\"  para comenzar";
                
                //*******  LA: Btn validar código para acceso a la encuesta  **********
                btnAccesoPrueba_Click(sender, e);
            }
        }
        private APrueba TienePruebaPendiente()
        {
            PruebaDiagnosticoCtrl pruebaDiagnosticaCtrl = new PruebaDiagnosticoCtrl();

            LicenciaEscuela licenciaEscuela = ConsultarLicenciaEscuela();
            Session_LicenciaEscuela = licenciaEscuela;

            List<APrueba> pruebasP = null;
            APrueba prueba=null;
            if (string.IsNullOrEmpty(QS_PruebaID))
            {
                pruebasP = pruebaDiagnosticaCtrl.RetrievePruebaPendiente(dctx, licenciaEscuela.Contrato, this.Session_Alumno, this.Session_GrupoCicloEscolar.Escuela, this.Session_GrupoCicloEscolar, true);
                prueba = pruebasP[0];
            }

            else
            {
                int pruebaID = Convert.ToInt32(QS_PruebaID);
                pruebasP = pruebaDiagnosticaCtrl.RetrievePruebaPendiente(dctx, licenciaEscuela.Contrato, this.Session_Alumno, this.Session_GrupoCicloEscolar.Escuela, this.Session_GrupoCicloEscolar, false);
                prueba = pruebasP.FirstOrDefault(itm => itm.PruebaID == pruebaID);
            }
             
            
            #region ***Consultamos la prueba pivote del contrato***
            ContratoCtrl contratoCtrl = new ContratoCtrl();
            List<PruebaContrato> pruebas = contratoCtrl.RetrievePruebasAsignadoContrato(dctx, licenciaEscuela.Contrato, new CicloContrato { CicloEscolar = Session_GrupoCicloEscolar.CicloEscolar });

            PruebaContrato pruebaPivoteContrato = pruebas.FirstOrDefault(item => item.TipoPruebaContrato == ETipoPruebaContrato.Pivote);
            if (pruebaPivoteContrato != null && pruebaPivoteContrato.Activo.Value) //si existe una prueba pivote en el contrato activa se verifica si esta PENDIENTE
            {
                APrueba pruebaPivote = null;
                if (pruebaPivoteContrato.Activo != null)
                    if (pruebaPivoteContrato.Activo.Value)
                    {
                        pruebaPivote = pruebaPivoteContrato.Prueba;
                        pruebaPivote = pruebaDiagnosticoCtrl.TienePruebaPendiente(pruebaPivote, dctx, Session_Alumno, Session_LicenciaEscuela.Escuela, Session_GrupoCicloEscolar, ETipoResultadoPrueba.PRUEBA_DIAGNOSTICA);
                        if (pruebaPivote != null)
                        {
                            Session_EsPruebaPivote = true;
                        }
                        else
                        {
                            Session_EsPruebaPivote = false;
                        }
                    }
            }
            #endregion
            
            if (prueba is PruebaDinamica)
            {
                Session_Prueba = prueba as PruebaDinamica;
                return prueba;
            }

            return null;
        }

        private LicenciaEscuela ConsultarLicenciaEscuela()
        {
            LicenciaEscuelaCtrl licenciaCtrl = new LicenciaEscuelaCtrl();
            DataSet dslicencia = licenciaCtrl.Retrieve(dctx, new LicenciaEscuela { Escuela = this.Session_GrupoCicloEscolar.Escuela, CicloEscolar = this.Session_GrupoCicloEscolar.CicloEscolar, Activo = true });
            LicenciaEscuela licenciaEscuela = null;
            if (dslicencia.Tables[0].Rows.Count > 0)
                licenciaEscuela = licenciaCtrl.LastDataRowToLicenciaEscuela(dslicencia);
            else throw new Exception("AccesoPrueba.aspx:TienePruebaPendiente. La consulta no trajo licencias");

            return licenciaEscuela;
 
        }
        /// <summary>
        /// Valida que el token recibido en la página es correcto o no
        /// </summary>
        /// <returns>Una bandera que indica si el token es válido o no</returns>
        public bool EsValidoToken()
        {
            bool valido = false;

            #region validacion de identificadores
            int escuelaID;
            if (!int.TryParse(this.QS_EscuelaID, out escuelaID))
                return false;            

            Guid grupoCicloEscolarID;
            if (!Guid.TryParse(this.QS_GrupoCicloEscolarID, out grupoCicloEscolarID))
                return false;
            #endregion

            Alumno alumno = new Alumno();
            alumno.Curp = this.QS_CURP;
            alumno.Estatus = true;

            AlumnoCtrl alumnoCtrl = new AlumnoCtrl();
            DataSet alumnos = alumnoCtrl.Retrieve(dctx, alumno);
            bool existeAlumno = alumnos.Tables[0].Rows.Count > 0;

            EscuelaCtrl escuelaCtrl = new EscuelaCtrl();
            DataSet escuelas = escuelaCtrl.Retrieve(dctx, new Escuela { EscuelaID = escuelaID });
            existeAlumno = existeAlumno && escuelas.Tables[0].Rows.Count > 0;

            GrupoCicloEscolarCtrl grupoCicloEscolarCtrl = new GrupoCicloEscolarCtrl();
            DataSet grupos = grupoCicloEscolarCtrl.Retrieve(dctx, new GrupoCicloEscolar { GrupoCicloEscolarID = grupoCicloEscolarID });
            existeAlumno = existeAlumno && grupos.Tables[0].Rows.Count > 0;


            if (existeAlumno)
            {
                this.Session_Alumno = alumnoCtrl.LastDataRowToAlumno(alumnos);
                this.Session_GrupoCicloEscolar = grupoCicloEscolarCtrl.LastDataRowToGrupoCicloEscolar(grupos);
                this.Session_GrupoCicloEscolar.Escuela = escuelaCtrl.LastDataRowToEscuela(escuelas);                                 

                string cadenaToken = this.Session_Alumno.FechaNacimiento.Value.ToString("yyyyMMddHHmmss.fff") + this.Session_Alumno.Nombre.Trim() + this.Session_Alumno.PrimerApellido.Trim() + this.Session_Alumno.Curp.Trim() + this.QS_FechaHora;
                byte[] bytes = EncryptHash.SHA1encrypt(cadenaToken);
                string token = EncryptHash.byteArrayToStringBase64(bytes);
                string tokenEncode = Server.UrlEncode(token);

                DateTime fechaHoraSolicitud = DateTime.ParseExact(this.QS_FechaHora, "yyyyMMddHHmmss.fff", CultureInfo.InvariantCulture);
                bool vigente = DateTime.Now.Subtract(fechaHoraSolicitud).TotalSeconds <= 5000;

                valido = this.QS_Token.CompareTo(token) == 0 && vigente;
            }

            return valido;
        }
        /// <summary>
        /// Verifica que la url recibida en la página es correcta
        /// </summary>
        /// <param name="errores">La cadena que sirve de parámetro de entrada, para validar</param>
        /// <returns>Si es válido o no la url</returns>
        public bool EsValidoQueryString(out string errores)
        {
            bool valido = true; errores = "";

            if (this.QS_CURP == null || this.QS_CURP.Trim().Length == 0)
                errores += ",QS_CURP";
            if (this.QS_FechaHora == null || this.QS_FechaHora.Trim().Length == 0)
                errores += ",QS_FechaHora";
            if (this.QS_Token == null || this.QS_Token.Trim().Length == 0)
                errores += ",QS_Token";
            if (this.QS_EscuelaID == null || this.QS_EscuelaID.Trim().Length == 0)
                errores += ",QS_EscuelaID";
            if (this.QS_GrupoCicloEscolarID == null || this.QS_GrupoCicloEscolarID.Trim().Length == 0)
                errores += ",QS_GrupoCicloEscolarID";
            #region Ajuste 1
            if (this.QS_Talentos != null)
            {
                if (this.QS_Asignacion == null)
                    errores += ",QS_Asignacion";
                if (this.QS_Prueba == null)
                    errores += ",QS_Prueba";
                if (this.QS_Tarea == null)
                    errores += ",QS_Tarea";
                if (this.QS_ResultadoPrueba == null)
                    errores += ",QS_ResultadoPrueba";
            }
            #endregion
            if (errores.Length > 0)
                valido = false;

            return valido;
        }
        /// <summary>
        /// Valida el código para acceder a la prueba
        /// </summary>
        /// <param name="codigo">Código a ser validado</param>
        /// <returns>Si el código es válido o no</returns>
        public bool ValidarCodigo(string codigo)
        {
            bool codigoValido = false;

            GrupoCicloEscolarCtrl gpoCicloEscolarCtrl = new GrupoCicloEscolarCtrl();
            GrupoCicloEscolar grupoCicloEscolar = gpoCicloEscolarCtrl.LastDataRowToGrupoCicloEscolar(gpoCicloEscolarCtrl.Retrieve(dctx, this.Session_GrupoCicloEscolar)); //.RetrieveGrupoCicloEscolar(dctx, this.Session_Alumno, new CicloEscolar());

            if (codigoValido = grupoCicloEscolar.Clave.CompareTo(codigo) == 0)
                this.Session_GrupoCicloEscolar = grupoCicloEscolar;

            return codigoValido;
        }
        /// <summary>
        /// Redirige a prueba diagnostica
        /// </summary>
        public void RedirigirPruebaDiagnostica()
        {
            HttpContext.Current.Response.Redirect("PruebaDiagnostica.aspx");
        }
        /// <summary>
        /// Redirige a bienvenida, para que el alumno pueda contestar la prueba.
        /// </summary>
        public void RedirigirBienvenida()
        {
            HttpContext.Current.Response.Redirect("Bienvenida.aspx");
        }
        /// <summary>
        /// Redirige a la red social, debido a que la  prueba ya está finalizada, o el alumno no tiene pruebas pendientes
        /// </summary>
        /// <param name="message"></param>
        public void RedirigirRedSocial(string message)
        {
            this.LimpiarSession();

            StreamReader objReader = new StreamReader(Server.MapPath("~/redireccion.txt"));
            ArrayList arrText = new ArrayList();
            try
            {
                string sLine = "";
                while (sLine != null)
                {
                    sLine = objReader.ReadLine();
                    if (sLine != null)
                        arrText.Add(sLine);
                }
            }
            catch (Exception ex)
            {
                LoggerHlp.Default.Error(this, ex);
            }
            finally
            {
                objReader.Close();
            }

            string dominio = String.Format("{0}://{1}:{2}/", HttpContext.Current.Request.Url.Scheme, HttpContext.Current.Request.Url.Host, HttpContext.Current.Request.Url.Port);
            this.txtRedirect.Text = dominio + (string)arrText[0];
            this.ShowMessage(message, "3");
        }

        private void LimpiarSession()
        {
            this.Session_Alumno = null;
            this.Session_GrupoCicloEscolar = null;
            this.Session_ResultadoPrueba = null;
        }
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
            //Si el HiddenField del mensaje de error ya tiene un mensaje guardado, se da un 'enter' y se concatena el nuevo mensaje (errores acumulados)
            //En caso contrario, se pone el encabezado y se concatena el nuevo mensaje
            if (hdnLastMessage.Value != null && hdnLastMessage.Value.Trim().CompareTo("") != 0)
                hdnLastMessage.Value += "<br />";

            hdnLastMessage.Value += message.Replace("\n", "<br />");
            hdnShowMessage.Value = typeNotification;
        }
        #endregion

        protected void btnAccesoPrueba_Click(object sender, EventArgs e)
        {
            this.txtRedirect.Text = "";

            if (this.Session_Alumno == null)
            {
                this.RedirigirRedSocial("Serás enviado al portal.");//N&#257;
                return;
            }

            bool valido = true;//false;
            try
            {
                if (QS_Talentos == null)
                    valido = true;//this.ValidarCodigo(this.txtAccesoPrueba.Text);
                else
                {
                    valido = true;
                }
            }
            catch (Exception ex)
            {
                LoggerHlp.Default.Error(this, ex);
                this.ShowMessage("No fue posible validar el código de la prueba diagnóstica, intentalo nuevamente.", "3");
                return;
            }

            if (!valido)
            {
                this.lblCodigoFail.Text = "Código incorrecto";
                return;
            }
            try
            {
                PruebaDiagnosticoCtrl pruebaDiagnosticaCtrl = new PruebaDiagnosticoCtrl();

                if (QS_Talentos == null)
                {

                    Session_ResultadoPrueba = pruebaDiagnosticaCtrl.CreateResultadoPruebaPendiente(dctx, Session_Prueba,
                        this.Session_Alumno, this.Session_GrupoCicloEscolar.Escuela, this.Session_GrupoCicloEscolar,
                        ETipoResultadoPrueba.PRUEBA_DIAGNOSTICA);
                }
                else
                {
                    GrupoCicloEscolarCtrl gpoCicloEscolarCtrl = new GrupoCicloEscolarCtrl();
                    GrupoCicloEscolar grupoCicloEscolar = gpoCicloEscolarCtrl.LastDataRowToGrupoCicloEscolar(gpoCicloEscolarCtrl.Retrieve(dctx, this.Session_GrupoCicloEscolar));
                    this.Session_GrupoCicloEscolar = grupoCicloEscolar;

                    CatalogoPruebaCtrl catalogoPruebaCtrl = new CatalogoPruebaCtrl();
                    DataSet ds = catalogoPruebaCtrl.Retrieve(dctx, int.Parse(QS_Prueba.ToString()),
                        EEstadoLiberacionPrueba.LIBERADA, null);

                    Session_AsignacionActividadId = long.Parse(QS_Asignacion);
                    Session_TareaRealizadaId = long.Parse(QS_Tarea);

                    Session_LicenciaEscuela = ConsultarLicenciaEscuela();

                    if (ds.Tables[0].Rows.Count != 0)
                    {
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            if (int.Parse(row["Tipo"].ToString()) == (int) ETipoPrueba.Dinamica)
                            {
                                PruebaDinamicaCtrl pruebaDinamicaCtrl = new PruebaDinamicaCtrl();
                                Session_Prueba = pruebaDinamicaCtrl.DataRowToPruebaDinamica(row);
                            }
                        }

                        int resultadoPruebaId = int.Parse(QS_ResultadoPrueba);
                        AResultadoPrueba resultadoPruebaFiltro = null;
                        switch (Session_Prueba.TipoPrueba)
                        {
                            case ETipoPrueba.Dinamica:
                                resultadoPruebaFiltro = new ResultadoPruebaDinamica
                                {
                                    ResultadoPruebaID = resultadoPruebaId,
                                    Tipo = ETipoResultadoPrueba.PRUEBA_TAREA,
                                    Prueba = Session_Prueba
                                };
                                break;
                        }
                        Session_ResultadoPrueba = pruebaDiagnosticaCtrl.RetrieveCompleteResultadoPrueba(dctx, resultadoPruebaFiltro);


                    }
                }

                this.RedirigirBienvenida();
            }
            catch (Exception ex)
            {
                if (ex is System.Threading.ThreadAbortException)
                    return;
                LoggerHlp.Default.Error(this, ex);
                this.ShowMessage("No fue posible validar el código de la prueba diagnóstica, intentalo nuevamente.", "3");
                return;
            }
        }

    }
}