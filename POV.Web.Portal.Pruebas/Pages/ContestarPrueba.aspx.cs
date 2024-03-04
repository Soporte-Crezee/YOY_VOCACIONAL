using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using POV.Prueba.BO;
using POV.Expediente.BO;
using POV.Prueba.Diagnostico.Service;
using POV.Prueba.Diagnostico.Dinamica.BO;
using POV.Prueba.Diagnostico.Dinamica.Service;

using POV.Web.Portal.Pruebas.Helper;
using Framework.Base.DataAccess;
using POV.CentroEducativo.BO;
using POV.Logger;
using Framework.Base.Exceptions;
using System.IO;
namespace POV.Web.Portal.Pruebas.Pages
{
    public partial class ContestarPrueba : System.Web.UI.Page
    {
        private IDataContext dctx = ConnectionHlp.Default.Connection;
        private AResultadoPrueba Session_RegistroPrueba
        {
            get { return (AResultadoPrueba)this.Session["ResultadoPrueba"]; }
            set { this.Session["ResultadoPrueba"] = value; }
        }

        private Alumno Session_Alumno
        {
            get { return (Alumno)this.Session["Alumno"]; }
            set { this.Session["Alumno"] = value; }
        } 

        public string tiempoEsperaSesion;

        public ContestarPrueba() 
        {
            tiempoEsperaSesion = System.Configuration.ConfigurationManager.AppSettings["POVtiempoEsperaSesion"];
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
            if (Session_Alumno == null)
                {
                    RedirigirRedSocial("Ocurrió un error. Serás enviado al portal.");//N&#257;
                }
                if (this.Session_RegistroPrueba == null)
                {
                    RedirigirRedSocial("Ocurrió un error. Serás enviado al portal.");
                }

                if (!Page.IsPostBack)
                {
                    lblTipoAlumno.Text = "Estudiante: " ;
                    hdnAlumnoID.Value = this.Session_Alumno.AlumnoID.ToString();
                    hdnAlumnoFechaNacimiento.Value = this.Session_Alumno.FechaNacimiento.ToString();
                    hdnAlumnoNombre.Value = this.Session_Alumno.Nombre.ToString();
                    hdnAlumnoPrimerApellido.Value = this.Session_Alumno.PrimerApellido.ToString();
                    hdnAlumnoCurp.Value = this.Session_Alumno.Curp.ToString();

                    lblNombreAlumno.Text = string.Format("{0} {1} {2}", this.Session_Alumno.Nombre, this.Session_Alumno.PrimerApellido, this.Session_Alumno.SegundoApellido);

                    if (this.Session_RegistroPrueba.Prueba.TipoPruebaPresentacion != ETipoPruebaPresentacion.Dinamica)
                    {
                        btnSuspender.Visible = true;
                        progressBar.Visible = true;
                    }
                    else
                    {
                        btnSuspender.Visible = false;
                        progressBar.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                
                Logger.Service.LoggerHlp.Default.Error(this,ex);
            }

        }

        #region Metodos Privados

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
            Response.Redirect(dominio + (string)arrText[0]);
           
        }

        private void LimpiarSesion()
        {
            this.Session_Alumno = null;
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