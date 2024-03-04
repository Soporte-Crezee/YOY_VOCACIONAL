using Framework.Base.Exceptions;
using POV.CentroEducativo.BO;
using POV.CentroEducativo.Services;
using POV.Core.RedSocial.Implement;
using POV.Core.RedSocial.Interfaces;
using POV.Logger.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace POV.Web.PortalSocial.PortalAlumno
{
    public partial class CarrerasEventosUniversidadUI : System.Web.UI.Page
    {
        private IUserSession userSession;
        private IRedirector redirector;

        private List<Universidad> ListaUnidversidades
        {
            get
            {
                return (List<Universidad>)Session["ListaUniversidades"];
            }
            set
            {
                Session["ListaUniversidades"] = value;
            }
        }
        private Universidad UniversidadSelect
        {
            get
            {
                return (Universidad)Session["UniversidadSelect"];
            }
            set
            {
                Session["UniversidadSelect"] = value;
            }
        }

        public CarrerasEventosUniversidadUI()
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
                    if (!userSession.IsLogin())
                    {
                        redirector.GoToLoginPage(true);
                    }
                    else
                    {
                        EFAlumnoCtrl alumnoCtrl = new EFAlumnoCtrl(null);
                        Alumno alumno = alumnoCtrl.Retrieve(new Alumno { AlumnoID = userSession.CurrentAlumno.AlumnoID }, false).FirstOrDefault();
                        
                        if ((bool)alumno.CorreoConfirmado)
                        {
                            ((PortalAlumno)this.Master).LoadControlsAlumnoMaster((long)userSession.CurrentUsuarioSocial.UsuarioSocialID, false);

                            this.hdnSessionSocialHubID.Value = userSession.SocialHub.SocialHubID.ToString();
                            this.hdnSessionUsuarioSocialID.Value = userSession.CurrentUsuarioSocial.UsuarioSocialID.ToString();
                            hdnSocialHubID.Value = userSession.SocialHub.SocialHubID.ToString();
                            hdnUsuarioSocialID.Value = userSession.CurrentUsuarioSocial.UsuarioSocialID.ToString();

                            PopulateUniversidades();
                        }
                        else
                            redirector.GoToHomeAlumno(true);
                    }
                }
                else
                {
                    if (!userSession.IsLogin()) //es alumno
                    {
                        redirector.GoToLoginPage(true);
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerHlp.Default.Error(this, ex);
            }
        }

        private void PopulateUniversidades()
        {

            try
            {
                UniversidadCtrl universidadCtrl = new UniversidadCtrl(null);
                UniversidadSelect = universidadCtrl.RetrieveWithRelationship(new Universidad { UniversidadID = userSession.CurrentAlumno.Universidades[0].UniversidadID }, false).FirstOrDefault();
                
                lbUniversidades.Text = UniversidadSelect.NombreUniversidad;
                grvEventos.DataSource = UniversidadSelect.EventosUniversidad;
                grvEventos.DataBind();
            }
            catch (Exception exception)
            {
                ShowMessage("Ha ocurrido un error inesperado, no fue posible presentar el listado de universidades",MessageType.Error);
            }
        }

        #region Eventos
        /// <summary>
        /// Método que maneja el cambio de página de las tareas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grvEventos_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grvEventos.DataSource = UniversidadSelect.Carreras;
            grvEventos.PageIndex = e.NewPageIndex;
            grvEventos.DataBind();

        }

        protected void grvEventos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName.Trim())
            {
                case "Ver_Descripcion":
                    {
                        var EventoId = long.Parse(e.CommandArgument.ToString());
                        var eventoUniversidad = UniversidadSelect.EventosUniversidad.Where(x => x.EventoUniversidadId == EventoId).FirstOrDefault();
                        divTitulo.InnerText = eventoUniversidad.Nombre;
                        txtDescripcion.Text = eventoUniversidad.Descripcion.ToString().Trim();
                        break;
                    }
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "viewModal();", true);
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
            Control m = Page.Master.Master.FindControl("hdnLastMessage");
            Control t = Page.Master.Master.FindControl("hdnShowMessage");

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