using Framework.Base.Exceptions;
using POV.CentroEducativo.BO;
using POV.CentroEducativo.Services;
using POV.Core.PadreTutor.Implements;
using POV.Core.PadreTutor.Interfaces;
using POV.Logger.Service;
using POV.Prueba.BO;
using POV.Prueba.Diagnostico.Dinamica.BO;
using POV.Prueba.Diagnostico.Dinamica.Service;
using POV.Web.PortalTutor.Helper;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace POV.Web.PortalTutor.Pages.Reportes
{
    public partial class ResultadoPruebaSacksTutorado : System.Web.UI.Page
    {
        private IUserSession userSession;
        private IRedirector redirector;

        public ResultadoPruebaSacksTutorado()
        {
            userSession = new UserSession();
            redirector = new Redirector();

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    if (userSession.IsLogin())
                    {
                        if ((bool)userSession.CurrentTutor.CorreoConfirmado && (bool)userSession.CurrentTutor.DatosCompletos)
                            btnBuscarAlumno_Click(sender, e);
                        else
                            redirector.GoToHomePage(true);
                    }
                    else
                    {
                        redirector.GoToLoginPage(true);
                    }
                }
                else
                {
                    if (!userSession.IsLogin())
                    {
                        redirector.GoToLoginPage(true);
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerHlp.Default.Error(this, ex);
                throw ex;
            }
        }

        protected void btnBuscarAlumno_Click(object sender, EventArgs e)
        {
            LlenarAlumnosUsuarios(new Tutor { TutorID = userSession.CurrentTutor.TutorID }, new PruebaDinamica { PruebaID = 12 }, "%" + TxtNombreTutorado.Text.Trim() + "%");
        }

        protected void gvResultadoPruebaSacksTutorado_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void gvResultadoPruebaSacksTutorado_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName.Trim())
            {
                case "VerReporte":
                    {
                        try
                        {
                            Response.Redirect("ReporteAlumnoResultadoSACKS.aspx?num=" + e.CommandArgument.ToString());
                        }
                        catch (Exception ex)
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alert('Ha ocurrido un error el resutado de la prueba del estudiante');", true);
                            LoggerHlp.Default.Error(this, ex);
                        }
                        break;
                    }
                default: { ShowMessage("Comando no encontrado", MessageType.Error); break; }

            }
        }

        private void LlenarAlumnosUsuarios(Tutor tutor, APrueba prueba, String strNombre)
        {
            ResultadoPruebaDinamicaCtrl ctrl = new ResultadoPruebaDinamicaCtrl();
            gvResultadoPruebaSacksTutorado.DataSource = ctrl.RetrieveAlumnosPruebasSacks(ConnectionHlp.Default.Connection, tutor, prueba, strNombre);
            gvResultadoPruebaSacksTutorado.DataBind();
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