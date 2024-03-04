using System;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using POV.CentroEducativo.BO;
using POV.Logger.Service;
using POV.Expediente.BO;
using POV.Prueba.Diagnostico.Dinamica.BO;
using POV.Prueba.Diagnostico.Service;
using Framework.Base.DataAccess;
using POV.Web.Portal.Pruebas.Helper;
using POV.Licencias.Service;
using POV.Licencias.BO;
using POV.Prueba.BO;

namespace POV.Web.Portal.Pruebas.Pages
{
    public partial class Final : System.Web.UI.Page
    {

        #region *** Propiedades ***
        private IDataContext dctx = ConnectionHlp.Default.Connection;
        private Alumno Session_Alumno
        {
            get
            {
                Alumno alumno = null;
                if (Session["Alumno"] != null)
                    alumno = this.Session["Alumno"] as Alumno;
                return alumno;
            }
            set
            {
                Session["Alumno"] = value;
            }
        }
        private AResultadoPrueba Session_ResultadoPrueba
        {
            get { return (AResultadoPrueba)this.Session["ResultadoPrueba"]; }
            set { this.Session["ResultadoPrueba"] = value; }
        }
        private long? Session_AsignacionActividadId
        {
            get { return HttpContext.Current.Session["AsignacionActividadId"] as long?; }
            set { HttpContext.Current.Session["AsignacionActividadId"] = value; }
        }

        private bool? Session_EsPruebaPivote
        {
            get { return this.Session["Session_EsPruebaPivote"] as bool?; }
            set { this.Session["Session_EsPruebaPivote"] = value; }
        }

        private bool? Session_EsPruebaBullying
        {
            get { return this.Session["Session_EsPruebaBullying"] as bool?; }
            set { this.Session["Session_EsPruebaBullying"] = value; }
        }

        private long? Session_TareaRealizadaId
        {
            get { return HttpContext.Current.Session["TareaRealizadaId"] as long?; }
            set { HttpContext.Current.Session["TareaRealizadaId"] = value; }
        }
        #endregion
        #region *** Eventos ***
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session_EsPruebaPivote == true)
                {
                    divPivote.Visible = true;
                    divNoPivote.Visible = false;                    
                }
                else {
                    divPivote.Visible = false;
                    divNoPivote.Visible = true;
                    if (Session_ResultadoPrueba.Prueba.TipoPruebaPresentacion == ETipoPruebaPresentacion.Allport)
                        NombrePrueba.Text = "Prueba de Valores de Allport";
                    else if (Session_ResultadoPrueba.Prueba.TipoPruebaPresentacion == ETipoPruebaPresentacion.Chaside)
                        NombrePrueba.Text = "Prueba de Orientación Vocacional CHASIDE";
                    else if (Session_ResultadoPrueba.Prueba.TipoPruebaPresentacion == ETipoPruebaPresentacion.Cleaver)
                        NombrePrueba.Text = "Prueba de Personalidad de Cleaver";
                    else if (Session_ResultadoPrueba.Prueba.TipoPruebaPresentacion == ETipoPruebaPresentacion.Dominos)
                        NombrePrueba.Text = "Prueba de Dominós";
                    else if (Session_ResultadoPrueba.Prueba.TipoPruebaPresentacion == ETipoPruebaPresentacion.FrasesIncompletasSacks)
                        NombrePrueba.Text = "Prueba de Frases Incompletas de SACKS";
                    else if (Session_ResultadoPrueba.Prueba.TipoPruebaPresentacion == ETipoPruebaPresentacion.HabitosEstudio)
                        NombrePrueba.Text = "Prueba de Hábitos de Estudio";
                    else if (Session_ResultadoPrueba.Prueba.TipoPruebaPresentacion == ETipoPruebaPresentacion.Kuder)
                        NombrePrueba.Text = "Prueba de Preferencia Vocacional de Kuder";
                    else if (Session_ResultadoPrueba.Prueba.TipoPruebaPresentacion == ETipoPruebaPresentacion.TermanMerrill)
                        NombrePrueba.Text = "Prueba de Terman Merrill";
                    else if (Session_ResultadoPrueba.Prueba.TipoPruebaPresentacion == ETipoPruebaPresentacion.Rotter)
                        NombrePrueba.Text = "Prueba de Locus de control Rotter";
                    else if (Session_ResultadoPrueba.Prueba.TipoPruebaPresentacion == ETipoPruebaPresentacion.Raven)
                        NombrePrueba.Text = "Prueba de Raven";
                    else if (Session_ResultadoPrueba.Prueba.TipoPruebaPresentacion == ETipoPruebaPresentacion.FrasesIncompletasVocacionales)
                        NombrePrueba.Text = "Prueba de Frases Incompletas Vocacionales";
                    else if (Session_ResultadoPrueba.Prueba.TipoPruebaPresentacion == ETipoPruebaPresentacion.Zavic)
                        NombrePrueba.Text = "Prueba de Zavic";
                    else if (Session_ResultadoPrueba.Prueba.TipoPruebaPresentacion == ETipoPruebaPresentacion.EstilosdeAprendizaje)
                        NombrePrueba.Text = "Prueba Estilos de Aprendizaje";
                    else if (Session_ResultadoPrueba.Prueba.TipoPruebaPresentacion == ETipoPruebaPresentacion.InteligengiasMultiples)
                        NombrePrueba.Text = "Prueba Inteligencias Multiples";
                    #region BateriaBullying
                    #region Bloque 1 : Victimario
                    #region Autoconcepto
                    else if (Session_ResultadoPrueba.Prueba.TipoPruebaPresentacion == ETipoPruebaPresentacion.Autoconcepto)
                    {
                        NombrePrueba.Text = "Prueba de Bullying - Autoconcepto"; Session_EsPruebaBullying = true;
                    }
                    #endregion
                    #region Actitudes
                    else if (Session_ResultadoPrueba.Prueba.TipoPruebaPresentacion == ETipoPruebaPresentacion.Actitudes)
                    {
                        NombrePrueba.Text = "Prueba de Bullying - Actitudes"; Session_EsPruebaBullying = true;
                    }
                    #endregion
                    #region Empatia
                    else if (Session_ResultadoPrueba.Prueba.TipoPruebaPresentacion == ETipoPruebaPresentacion.Empatia)
                    {
                        NombrePrueba.Text = "Prueba de Bullying - Empatía"; Session_EsPruebaBullying = true;
                    }
                    #endregion
                    #region Humor
                    else if (Session_ResultadoPrueba.Prueba.TipoPruebaPresentacion == ETipoPruebaPresentacion.Humor)
                    {
                        NombrePrueba.Text = "Prueba de Bullying - Humor"; Session_EsPruebaBullying = true;
                    }
                    #endregion
                    #endregion
                    #region Bloque 2 : Victima
                    #region Victimizacion
                    else if (Session_ResultadoPrueba.Prueba.TipoPruebaPresentacion == ETipoPruebaPresentacion.Victimizacion)
                    {
                        NombrePrueba.Text = "Prueba de Bullying - Victimización"; Session_EsPruebaBullying = true;
                    }
                    #endregion
                    #region Ciberbullying
                    else if (Session_ResultadoPrueba.Prueba.TipoPruebaPresentacion == ETipoPruebaPresentacion.Ciberbullying)
                    {
                        NombrePrueba.Text = "Prueba de Bullying - Ciberbullying"; Session_EsPruebaBullying = true;
                    }
                    #endregion
                    #region Bullying
                    else if (Session_ResultadoPrueba.Prueba.TipoPruebaPresentacion == ETipoPruebaPresentacion.Bullying)
                    {
                        NombrePrueba.Text = "Prueba de Bullying - Bullying"; Session_EsPruebaBullying = true;
                    }
                    #endregion
                    #endregion
                    #region Bloque 3 : Funcionamiento Familiar
                    #region Violencia
                    else if (Session_ResultadoPrueba.Prueba.TipoPruebaPresentacion == ETipoPruebaPresentacion.Violencia)
                    {
                        NombrePrueba.Text = "Prueba de Bullying - Violencia"; Session_EsPruebaBullying = true;
                    }
                    #endregion
                    #region Comunicacion
                    else if (Session_ResultadoPrueba.Prueba.TipoPruebaPresentacion == ETipoPruebaPresentacion.Comunicacion)
                    {
                        NombrePrueba.Text = "Prueba de Bullying - Comunicación"; Session_EsPruebaBullying = true;
                    }
                    #endregion
                    #region Imagen Corporal
                    else if (Session_ResultadoPrueba.Prueba.TipoPruebaPresentacion == ETipoPruebaPresentacion.ImagenCorporal)
                    {
                        NombrePrueba.Text = "Prueba de Bullying - Imagen coporal"; Session_EsPruebaBullying = true;
                    }
                    #endregion
                    #region Ansiedad
                    else if (Session_ResultadoPrueba.Prueba.TipoPruebaPresentacion == ETipoPruebaPresentacion.Ansiedad)
                    {
                        NombrePrueba.Text = "Prueba de Bullying - Ansiedad"; Session_EsPruebaBullying = true;
                    }
                    #endregion
                    #region Depresion
                    else if (Session_ResultadoPrueba.Prueba.TipoPruebaPresentacion == ETipoPruebaPresentacion.Depresion)
                    {
                        NombrePrueba.Text = "Prueba de Bullying - Depresión"; Session_EsPruebaBullying = true;
                    }
                    #endregion
                    #endregion
                    #endregion
                    else if (Session_ResultadoPrueba.Prueba.TipoPruebaPresentacion == ETipoPruebaPresentacion.Socioeconomico)
                        NombrePrueba.Text = "Encuesta Socioeconómica";
                }   

                ValidarEstadoPrueba();
                if (!IsPostBack)
                {
                    this.DestruirPrueba();
                }
            }
            catch (Exception ex)
            {
                Logger.Service.LoggerHlp.Default.Error(this, ex);
            }
           
        }

        protected void lnkBotonTerminar_Click(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                Logger.Service.LoggerHlp.Default.Error(this, ex);
            }
        }

        protected void lnkRealizarPruebaBateriaBullying_Click(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                Logger.Service.LoggerHlp.Default.Error(this, ex);
            }
        }
        #endregion
        #region *** Validate Data***

        private EEstadoPrueba ConsultarEstadoPruebaDinamica(RegistroPruebaDinamica registroPruebaDinamica)
        {


            if (registroPruebaDinamica.ListaRespuestaReactivos != null && registroPruebaDinamica.ListaRespuestaReactivos.All(reactivo => reactivo.GetType() == typeof(RespuestaReactivoDinamica)))
            {
                 
                int count = registroPruebaDinamica.ListaRespuestaReactivos.Count;
                int countComplete = registroPruebaDinamica.ListaRespuestaReactivos.Count(x => x.EstadoReactivo == EEstadoReactivo.CERRADO);

                if (count == countComplete)
                    return EEstadoPrueba.CERRADA;

                if (countComplete >= 1 && count > countComplete)
                    return EEstadoPrueba.ENCURSO;
            }

            return EEstadoPrueba.NOINICIADA;

        }
        private void ValidarEstadoPrueba()
        {
            //Obteniendo la ruta base del login
            var pathBase = this.GetLoginURL();
            RegistroPruebaDinamica registroPruebaDinamica = null;

            EEstadoPrueba estadoPrueba = new EEstadoPrueba(); ;
            //Validando el estado actual de la sesión
            if (this.Session_ResultadoPrueba != null && this.Session_Alumno != null)
            {
                if (Session_ResultadoPrueba is ResultadoPruebaDinamica)
                {
                    #region
                    ResultadoPruebaDinamica resultadoPrueba = Session_ResultadoPrueba as ResultadoPruebaDinamica;
                    if (resultadoPrueba.RegistroPrueba != null)
                        if (resultadoPrueba.RegistroPrueba is RegistroPruebaDinamica)
                        {
                            registroPruebaDinamica = resultadoPrueba.RegistroPrueba as RegistroPruebaDinamica;
                            estadoPrueba = this.ConsultarEstadoPruebaDinamica(registroPruebaDinamica);
                        }
                }
                if (estadoPrueba != null)
                    if (estadoPrueba == EEstadoPrueba.CERRADA)
                    {
                        var tokenUrl = this.GenerarTokenYUrl(this.Session_Alumno);
                        lnkBotonTerminar.PostBackUrl = pathBase + tokenUrl;
                        lnkBotonTerminarOp2.PostBackUrl = pathBase + tokenUrl;

                        if (Session_AsignacionActividadId != null && Session_TareaRealizadaId != null)
                        {
                            lnkBotonTerminar.Text = "Regresar";
                            lnkBotonTerminarOp2.Text = "Regresar";
                        }
                        if (Session_EsPruebaBullying == true)
                        {
                            lnkRealizarPruebaBateriaBullying.Visible = true;
                            var tokenUrlBullying = this.GenerarTokenYUrl(this.Session_Alumno, true);
                            lnkRealizarPruebaBateriaBullying.PostBackUrl = pathBase + tokenUrlBullying;
                            //lnkBotonTerminarOp2.PostBackUrl = pathBase + tokenUrlBullying;
                        }
                        return;
                    }
                    #endregion
            }
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", String.Format("document.location='{0}';", pathBase), true);
        }
        #endregion
        #region*** Métodos ***
        
        private void DestruirPrueba()
        {
            this.Session_Alumno = null;
            this.Session_ResultadoPrueba = null;
            this.Session_AsignacionActividadId = null;
            this.Session_TareaRealizadaId = null;
        }
        private string GenerarTokenYUrl(Alumno alumno, bool esBullying = false)
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
            token = Server.UrlEncode(token);
            // se agrega identificador de portal &portal
            if (Session_EsPruebaPivote == true)
                strUrlAutoLogin = "?alumno=" + alumno.Curp + "&fechahora=" + fecha.ToString(formatoFecha) + "&token=" + token;
            else
            {
                if (Session_EsPruebaBullying == true && esBullying)
                    strUrlAutoLogin = "?alumno=" + alumno.Curp + "&fechahora=" + fecha.ToString(formatoFecha) + "&token=" + token + "&portal=bullying";
                else
                    strUrlAutoLogin = "?alumno=" + alumno.Curp + "&fechahora=" + fecha.ToString(formatoFecha) + "&token=" + token + "&portal=pruebas";
            }
            return strUrlAutoLogin;
        }
        private String GetLoginURL() 
        {
            string ruta = Server.MapPath("~/redireccion.txt");
            StreamReader objReader = new StreamReader(ruta);
            string sLine = "";
            ArrayList arrText = new ArrayList();
            while (sLine != null)
            {
                sLine = objReader.ReadLine();
                if (sLine != null)
                    arrText.Add(sLine);
            }
            string dominio = "";
            dominio = String.Format("{0}://{1}:{2}/", HttpContext.Current.Request.Url.Scheme, HttpContext.Current.Request.Url.Host, HttpContext.Current.Request.Url.Port);
            var pathBase = dominio + (string)arrText[0];

            return pathBase;
        }
        #endregion
    }
}