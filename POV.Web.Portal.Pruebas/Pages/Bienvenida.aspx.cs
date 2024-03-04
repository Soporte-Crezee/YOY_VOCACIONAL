using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Framework.Base.Exceptions;
using Framework.Base.DataAccess;
using POV.CentroEducativo.BO;
using POV.CentroEducativo.Service;
using POV.Logger.Service;
using POV.Expediente.BO;
using POV.Prueba.BO;
using POV.Licencias.BO;
using POV.Licencias.Service;
using System.Configuration;

namespace POV.Web.Portal.Pruebas.Pages
{
    public partial class Bienvenida : System.Web.UI.Page
    {
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

        private AResultadoPrueba Session_RegistroPrueba
        {
            get { return (AResultadoPrueba)this.Session["ResultadoPrueba"]; }
            set { this.Session["ResultadoPrueba"] = value; }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (this.Session_Alumno == null)
                {
                    this.RedirigirRedSocial("Serás enviado al portal.");
                    return;
                }
                if (this.Session_RegistroPrueba == null)
                {
                    this.RedirigirRedSocial("Serás enviado al portal.");
                    return;
                }
                if (this.Session_RegistroPrueba.Prueba == null)
                {
                    this.RedirigirRedSocial("Serás enviado al portal.");
                    //this.RedirigirRedSocial("Serás enviado a N&#257;tware.");
                }
                if (this.Session_RegistroPrueba != null)
                {
                    ValidatePrueba();
                }
                if (!Page.IsPostBack)
                {
                    this.lblNombreAlumno.Text = string.Format("{0} {1} {2}", this.Session_Alumno.Nombre, this.Session_Alumno.PrimerApellido, this.Session_Alumno.SegundoApellido);
                    this.lblInstrucciones.Text = this.Session_RegistroPrueba.Prueba.Instrucciones;

                    if (this.Session_Alumno != null)
                    {
                        if (this.Session_RegistroPrueba.Prueba.TipoPruebaPresentacion != ETipoPruebaPresentacion.Dinamica)
                        {
                            instruccionesInicial.Visible = false;
                        }
                        else
                        {
                            lblInstrucciones.Visible = false;
                            instruccionesInicial.Visible = true;
                        }

                        #region Instrucciones Personalizadas de las pruebas
                        #region Dominos
                        if (this.Session_RegistroPrueba.Prueba.TipoPruebaPresentacion != ETipoPruebaPresentacion.Dominos) pruebaDominosExample.Visible = false;
                        else
                        {
                            lblInstrucciones.Visible = false;
                            instruccionesInicial.Visible = false;
                            pruebaDominosExample.Visible = true;

                            string urlImage = @ConfigurationManager.AppSettings["POVRutaImgReactivosDinamico"];
                            string urlImagePregunta = @ConfigurationManager.AppSettings["POVNomImgPreguntaDominos"];
                            string urlImageRespuesta = @ConfigurationManager.AppSettings["POVNomImgRespuestaDominos"];
                            string urlImageExtension = @ConfigurationManager.AppSettings["POVNomImgExtensionDominos"];

                            imgPregunta.Src = urlImage + urlImagePregunta + urlImageExtension;
                            imgRespuesta0.Src = urlImage + urlImageRespuesta + "00" + urlImageExtension;
                            imgRespuesta1.Src = urlImage + urlImageRespuesta + "01" + urlImageExtension;
                            imgRespuesta2.Src = urlImage + urlImageRespuesta + "02" + urlImageExtension;
                            imgRespuesta3.Src = urlImage + urlImageRespuesta + "03" + urlImageExtension;
                            imgRespuesta4.Src = urlImage + urlImageRespuesta + "04" + urlImageExtension;
                            imgRespuesta5.Src = urlImage + urlImageRespuesta + "05" + urlImageExtension;
                            imgRespuesta6.Src = urlImage + urlImageRespuesta + "06" + urlImageExtension;
                            imgRespuesta7.Src = urlImage + urlImageRespuesta + "00" + urlImageExtension;
                            imgRespuesta8.Src = urlImage + urlImageRespuesta + "01" + urlImageExtension;
                            imgRespuesta9.Src = urlImage + urlImageRespuesta + "02" + urlImageExtension;
                            imgRespuesta10.Src = urlImage + urlImageRespuesta + "03" + urlImageExtension;
                            imgRespuesta11.Src = urlImage + urlImageRespuesta + "04" + urlImageExtension;
                            imgRespuesta12.Src = urlImage + urlImageRespuesta + "05" + urlImageExtension;
                            imgRespuesta13.Src = urlImage + urlImageRespuesta + "06" + urlImageExtension;
                        }
                        #endregion

                        #region Terman
                        if (this.Session_RegistroPrueba.Prueba.TipoPruebaPresentacion != ETipoPruebaPresentacion.TermanMerrill) pruebaTermanExample.Visible = false;
                        else
                        {
                            lblInstrucciones.Visible = false;
                            instruccionesInicial.Visible = false;
                            pruebaTermanExample.Visible = true;
                        }
                        #endregion

                        #region Sacks
                        if (this.Session_RegistroPrueba.Prueba.TipoPruebaPresentacion != ETipoPruebaPresentacion.FrasesIncompletasSacks) pruebaSacksExample.Visible = false;
                        else 
                        {
                            lblInstrucciones.Visible = false;
                            instruccionesInicial.Visible = false;
                            pruebaSacksExample.Visible = true;
                        }
                        #endregion

                        #region Cleaver
                        if (this.Session_RegistroPrueba.Prueba.TipoPruebaPresentacion != ETipoPruebaPresentacion.Cleaver) pruebaCleaverExample.Visible = false;
                        else 
                        {
                            lblInstrucciones.Visible = false;
                            instruccionesInicial.Visible = false;
                            pruebaCleaverExample.Visible = true;
                        }
                        #endregion

                        #region Chaside
                        if (this.Session_RegistroPrueba.Prueba.TipoPruebaPresentacion != ETipoPruebaPresentacion.Chaside) pruebaChasideExample.Visible = false;
                        else 
                        {
                            lblInstrucciones.Visible = false;
                            instruccionesInicial.Visible = false;
                            pruebaChasideExample.Visible = true;
                        }
                        #endregion

                        #region Allport
                        if (this.Session_RegistroPrueba.Prueba.TipoPruebaPresentacion != ETipoPruebaPresentacion.Allport) pruebaAllportExample.Visible = false;
                        else
                        {
                            lblInstrucciones.Visible = false;
                            instruccionesInicial.Visible = false;
                            pruebaAllportExample.Visible = true;

                            string urlImage = @ConfigurationManager.AppSettings["POVRutaImgReactivosDinamico"];
                            string urlImageParteDos = @ConfigurationManager.AppSettings["POVNomImgPreguntaAllportPartDos"];
                            string urlImagePregunta = @ConfigurationManager.AppSettings["POVNomImgPreguntaAllport"];
                            string urlImageRespuesta = @ConfigurationManager.AppSettings["POVNomImgRespuestaInteres"];
                            string urlImageExtension = @ConfigurationManager.AppSettings["POVNomImgExtensionInteres"];

                            imgInstruccionesPart2.Src = urlImage + urlImageParteDos + urlImageExtension;
                            imgpreguntaAllport_A1.Src = urlImage + urlImageRespuesta + "A1" + urlImageExtension;
                            imgpreguntaAllport_A2.Src = urlImage + urlImageRespuesta + "A2" + urlImageExtension;
                            imgpreguntaAllport_A3.Src = urlImage + urlImageRespuesta + "A3" + urlImageExtension;
                            imgpreguntaAllport_A4.Src = urlImage + urlImageRespuesta + "A4" + urlImageExtension;
                            imgpreguntaAllport_B1.Src = urlImage + urlImageRespuesta + "B1" + urlImageExtension;
                            imgpreguntaAllport_B2.Src = urlImage + urlImageRespuesta + "B2" + urlImageExtension;
                            imgpreguntaAllport_B3.Src = urlImage + urlImageRespuesta + "B3" + urlImageExtension;
                            imgpreguntaAllport_B4.Src = urlImage + urlImageRespuesta + "B4" + urlImageExtension;
                            imgpreguntaAllport_C1.Src = urlImage + urlImageRespuesta + "C1" + urlImageExtension;
                            imgpreguntaAllport_C2.Src = urlImage + urlImageRespuesta + "C2" + urlImageExtension;
                            imgpreguntaAllport_C3.Src = urlImage + urlImageRespuesta + "C3" + urlImageExtension;
                            imgpreguntaAllport_C4.Src = urlImage + urlImageRespuesta + "C4" + urlImageExtension;
                            imgpreguntaAllport_D1.Src = urlImage + urlImageRespuesta + "D1" + urlImageExtension;
                            imgpreguntaAllport_D2.Src = urlImage + urlImageRespuesta + "D2" + urlImageExtension;
                            imgpreguntaAllport_D3.Src = urlImage + urlImageRespuesta + "D3" + urlImageExtension;
                            imgpreguntaAllport_D4.Src = urlImage + urlImageRespuesta + "D4" + urlImageExtension;
                        }
                        #endregion

                        #region Kuder
                        if (this.Session_RegistroPrueba.Prueba.TipoPruebaPresentacion != ETipoPruebaPresentacion.Kuder) pruebaKuderExample.Visible = false;
                        else 
                        {
                            lblInstrucciones.Visible = false;
                            instruccionesInicial.Visible = false;
                            pruebaKuderExample.Visible = true;
                        }
                        #endregion

                        #region Habitos
                        if (this.Session_RegistroPrueba.Prueba.TipoPruebaPresentacion != ETipoPruebaPresentacion.HabitosEstudio) pruebaHabitosExample.Visible = false;
                        else 
                        {
                            lblInstrucciones.Visible = false;
                            instruccionesInicial.Visible = false;
                            pruebaHabitosExample.Visible = true;
                        }
                        #endregion

                        #region Rotter
                        if (this.Session_RegistroPrueba.Prueba.TipoPruebaPresentacion != ETipoPruebaPresentacion.Rotter) pruebaRotterExample.Visible = false;
                        else 
                        {
                            lblInstrucciones.Visible = false;
                            instruccionesInicial.Visible = false;
                            pruebaRotterExample.Visible = true;
                        }
                        #endregion

                        #region Raven
                        if (this.Session_RegistroPrueba.Prueba.TipoPruebaPresentacion != ETipoPruebaPresentacion.Raven) pruebaRavenExample.Visible = false;
                        else
                        {
                            lblInstrucciones.Visible = false;
                            instruccionesInicial.Visible = false;
                            pruebaRavenExample.Visible = true;

                            string urlImage = @ConfigurationManager.AppSettings["POVRutaImgReactivosDinamico"];
                            string urlImagePregunta = @ConfigurationManager.AppSettings["POVNomImgPreguntaRaven"];
                            string urlImageRespuesta = @ConfigurationManager.AppSettings["POVNomImgRespuestaRaven"];
                            string urlImageExtension = @ConfigurationManager.AppSettings["POVNomImgExtensionRaven"];

                            imgpregravenexample.Src = urlImage + urlImagePregunta + urlImageExtension;
                            respraven01.Src = urlImage + urlImageRespuesta + "01" + urlImageExtension;
                            respraven02.Src = urlImage + urlImageRespuesta + "02" + urlImageExtension;
                            respraven03.Src = urlImage + urlImageRespuesta + "03" + urlImageExtension;
                            respraven04.Src = urlImage + urlImageRespuesta + "04" + urlImageExtension;
                            respraven05.Src = urlImage + urlImageRespuesta + "05" + urlImageExtension;
                            respraven06.Src = urlImage + urlImageRespuesta + "06" + urlImageExtension;
                        }
                        #endregion

                        #region frases incompletas vocacionales
                        if (this.Session_RegistroPrueba.Prueba.TipoPruebaPresentacion != ETipoPruebaPresentacion.FrasesIncompletasSacks) pruebaSacksExample.Visible = false;
                        else
                        {
                            lblInstrucciones.Visible = false;
                            instruccionesInicial.Visible = false;
                            pruebaFrasesVocacionalesExample.Visible = true;
                        }
                        #endregion

                        #region Zavic
                        if (this.Session_RegistroPrueba.Prueba.TipoPruebaPresentacion != ETipoPruebaPresentacion.Zavic) pruebaZavicExample.Visible = false;
                        else
                        {
                            lblInstrucciones.Visible = false;
                            instruccionesInicial.Visible = false;
                            pruebaZavicExample.Visible = true;

                            string urlImage = @ConfigurationManager.AppSettings["POVRutaImgReactivosDinamico"];
                            string urlImageParteDos = @ConfigurationManager.AppSettings["POVNomImgPreguntaZavic"];
                            string urlImageRespuesta = @ConfigurationManager.AppSettings["POVNomImgRespuestaInteres"];
                            string urlImageExtension = @ConfigurationManager.AppSettings["POVNomImgExtensionInteres"];

                            imginstruccioneszavic.Src = urlImage + urlImageParteDos + urlImageExtension;
                            imgpreguntaZavic_A1.Src = urlImage + urlImageRespuesta + "A1" + urlImageExtension;
                            imgpreguntaZavic_A2.Src = urlImage + urlImageRespuesta + "A2" + urlImageExtension;
                            imgpreguntaZavic_A3.Src = urlImage + urlImageRespuesta + "A3" + urlImageExtension;
                            imgpreguntaZavic_A4.Src = urlImage + urlImageRespuesta + "A4" + urlImageExtension;
                            imgpreguntaZavic_B1.Src = urlImage + urlImageRespuesta + "B1" + urlImageExtension;
                            imgpreguntaZavic_B2.Src = urlImage + urlImageRespuesta + "B2" + urlImageExtension;
                            imgpreguntaZavic_B3.Src = urlImage + urlImageRespuesta + "B3" + urlImageExtension;
                            imgpreguntaZavic_B4.Src = urlImage + urlImageRespuesta + "B4" + urlImageExtension;
                            imgpreguntaZavic_C1.Src = urlImage + urlImageRespuesta + "C1" + urlImageExtension;
                            imgpreguntaZavic_C2.Src = urlImage + urlImageRespuesta + "C2" + urlImageExtension;
                            imgpreguntaZavic_C3.Src = urlImage + urlImageRespuesta + "C3" + urlImageExtension;
                            imgpreguntaZavic_C4.Src = urlImage + urlImageRespuesta + "C4" + urlImageExtension;
                            imgpreguntaZavic_D1.Src = urlImage + urlImageRespuesta + "D1" + urlImageExtension;
                            imgpreguntaZavic_D2.Src = urlImage + urlImageRespuesta + "D2" + urlImageExtension;
                            imgpreguntaZavic_D3.Src = urlImage + urlImageRespuesta + "D3" + urlImageExtension;
                            imgpreguntaZavic_D4.Src = urlImage + urlImageRespuesta + "D4" + urlImageExtension;
                        }
                        #endregion

                        #region Estilos de Aprendizaje
                   
                        if (this.Session_RegistroPrueba.Prueba.TipoPruebaPresentacion != ETipoPruebaPresentacion.EstilosdeAprendizaje) pruebaEstilosdeApredizajeExample.Visible = false;
                    else
                        {
                            lblInstrucciones.Visible = false;
                            instruccionesInicial.Visible = false;
                            pruebaEstilosdeApredizajeExample.Visible = true;
                        }
                      
                        #endregion

                        #region Inteligencias Multiples
                        if (this.Session_RegistroPrueba.Prueba.TipoPruebaPresentacion != ETipoPruebaPresentacion.InteligengiasMultiples) pruebaInteligenciasMultiplesExample.Visible = false;
                        else
                        {
                            lblInstrucciones.Visible = false;
                            instruccionesInicial.Visible = false;
                            pruebaInteligenciasMultiplesExample.Visible = true;
                        }
                        #endregion

                        #region Inventario de Intereses
                        if (this.Session_RegistroPrueba.Prueba.TipoPruebaPresentacion != ETipoPruebaPresentacion.InventarioDeIntereses) InventariodeInteresesExample.Visible = false;
                        else
                        {
                            lblInstrucciones.Visible = false;
                            instruccionesInicial.Visible = false;
                            InventariodeInteresesExample.Visible = true;
                        }
                        #endregion

                        #region InventarioHerreraMontes
                        if (this.Session_RegistroPrueba.Prueba.TipoPruebaPresentacion != ETipoPruebaPresentacion.InventarioHerreraMontes) pruebaInventarioHerreraMontes.Visible = false;
                        else
                        {
                            lblInstrucciones.Visible = false;
                            instruccionesInicial.Visible = false;
                            InventariodeInteresesExample.Visible = true;
                        }
                     

                        #endregion

                        #region Bateria Bullying
                        if (this.Session_RegistroPrueba.Prueba.TipoPruebaPresentacion == ETipoPruebaPresentacion.Bullying || this.Session_RegistroPrueba.Prueba.TipoPruebaPresentacion == ETipoPruebaPresentacion.Autoconcepto ||
                            this.Session_RegistroPrueba.Prueba.TipoPruebaPresentacion == ETipoPruebaPresentacion.Actitudes || this.Session_RegistroPrueba.Prueba.TipoPruebaPresentacion == ETipoPruebaPresentacion.Empatia ||
                            this.Session_RegistroPrueba.Prueba.TipoPruebaPresentacion == ETipoPruebaPresentacion.Humor || this.Session_RegistroPrueba.Prueba.TipoPruebaPresentacion == ETipoPruebaPresentacion.Victimizacion ||
                            this.Session_RegistroPrueba.Prueba.TipoPruebaPresentacion == ETipoPruebaPresentacion.Ciberbullying || this.Session_RegistroPrueba.Prueba.TipoPruebaPresentacion == ETipoPruebaPresentacion.Violencia ||
                            this.Session_RegistroPrueba.Prueba.TipoPruebaPresentacion == ETipoPruebaPresentacion.Comunicacion || this.Session_RegistroPrueba.Prueba.TipoPruebaPresentacion == ETipoPruebaPresentacion.ImagenCorporal ||
                            this.Session_RegistroPrueba.Prueba.TipoPruebaPresentacion == ETipoPruebaPresentacion.Ansiedad || this.Session_RegistroPrueba.Prueba.TipoPruebaPresentacion == ETipoPruebaPresentacion.Depresion)
                        {
                            lblInstrucciones.Visible = false;
                            instruccionesInicial.Visible = false;
                            bateriabullying.Visible = true;
                        }
                        else
                        {
                            bateriabullying.Visible = false;
                        }
                        #endregion

                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Service.LoggerHlp.Default.Error(this, ex);
                this.RedirigirRedSocial("Ocurrió un error al iniciar. Serás enviado al portal");
            }
        }


        #region Métodos privados
        private void ValidatePrueba()
        {
            if ((this.Session_RegistroPrueba != null))
            {
                //consultar estado de la prueba en sesión en base a los 

                EEstadoPrueba estadoPrueba = (EEstadoPrueba)(this.Session_RegistroPrueba as IResultadoPrueba).RegistroPrueba.EstadoPrueba;

                if (estadoPrueba == EEstadoPrueba.CERRADA)
                    Response.Redirect("Final.aspx");
            }
        }

        private void RedirigirRedSocial(string mesaje)
        {
            this.LimpiarSesion();

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
            finally
            {
                objReader.Close();
            }

            string dominio = String.Format("{0}://{1}:{2}/", HttpContext.Current.Request.Url.Scheme, HttpContext.Current.Request.Url.Host, HttpContext.Current.Request.Url.Port);
            this.Redirect(dominio + (string)arrText[0]);
            this.ShowMessage(mesaje, "3");
        }

        private void LimpiarSesion()
        {
            this.Session_Alumno = null;
            this.Session_GrupoCicloEscolar = null;
            this.Session_RegistroPrueba = null;
        }

        private void Redirect(string url)
        {
            if (Page.Master == null) return;
            Control m = Page.Master.FindControl("txtRedirect");

            if (m == null)
                throw new Exception("No se puede realizar correctamente el redirect.\nNo se encontró el control 'txtRedirect' en la MasterPage.");

            if (m.GetType() != typeof(TextBox))
                throw new Exception("No se pudo realizar correctamente el redirect.\nEl control de la MasterPage para el redirect no es TextBox.");

            ((TextBox)m).Text = url;
        }
        #endregion

        #region *****Message  Showing*****
        /// <summary>
        /// Despliega el mensaje de error/advertencia/información en la UI
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
        /// Despliega el mensaje de error/advertencia/información en la UI
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