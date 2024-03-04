using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Framework.Base.Exceptions;
using POV.Administracion.Service;
using POV.CentroEducativo.BO;
using POV.CentroEducativo.Service;
using POV.Core.Administracion.Implements;
using POV.Core.Administracion.Interfaces;
using POV.Licencias.BO;
using POV.Licencias.Service;
using POV.Seguridad.BO;
using POV.Seguridad.Service;
using POV.Web.Administracion.Helper;
using POV.Web.Administracion.AppCode.Page;
using POV.CentroEducativo.Services;
using Framework.Base.DataAccess;

namespace POV.Web.Administracion.Universidades
{
    public partial class BuscarUniversidad : CatalogPage
    {
        private UniversidadCtrl universidadCtrl;
        private CatalogoUniversidadesCtrl catalogoUniversidadesCtrl;
        private readonly IDataContext dctx = ConnectionHlp.Default.Connection;

        #region *** propiedades de clase ***
        public List<Universidad> ListUniversidad
        {
            get { return Session["universidad"] != null ? Session["universidad"] as List<Universidad> : null; }
            set { Session["universidad"] = value; }
        }
        public Universidad LastObject
        {
            get { return Session["LastUniversidad"] != null ? (Universidad)Session["LastUniversidad"] : null; }
            set { Session["LastUniversidad"] = value; }
        }
        #endregion

        public BuscarUniversidad()
        {
            // CG
            universidadCtrl = new UniversidadCtrl(null);
            catalogoUniversidadesCtrl = new CatalogoUniversidadesCtrl();
        }

        #region *** eventos de pagina ***
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                LastObject = null;
                if (userSession != null && userSession.CurrentDirector != null && userSession.CurrentCicloEscolar != null)
                {
                    if (!IsPostBack)
                        LoadUniversidades(new Universidad { Activo = true });
                }
                else
                {
                    redirector.GoToLoginPage(true);
                }
            }
            catch (Exception ex)
            {
                this.ShowMessage(ex.Message, MessageType.Error);
            }
        }
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                Universidad universidad = UserInterfaceToData();
                LoadUniversidades(universidad);
            }
            catch (Exception ex)
            {
                this.ShowMessage(ex.Message, MessageType.Error);
            }
        }
        protected void grdDocentesEscuela_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                switch (e.CommandName)
                {
                    case "editar":
                        LastObject = new Universidad { UniversidadID = long.Parse(e.CommandArgument.ToString()) };
                        redirector.GoToEditarUniversidad(true);
                        break;
                    case "eliminar":
                        LastObject = new Universidad { UniversidadID = long.Parse(e.CommandArgument.ToString()) };
                        DoDelete(LastObject);
                        LastObject = null;
                        LoadUniversidades(new Universidad { Activo = true });
                        break;

                }
            }
            catch (Exception ex)
            {

                ShowMessage(ex.Message, MessageType.Error);
            }
        }
        #endregion

        #region *** Data to UserInterface ***
        private void LoadUniversidades(Universidad universidadFilter)
        {
            List<Universidad> listaUniversidad = universidadCtrl.Retrieve(universidadFilter, false);

            grdUniversidad.DataSource = listaUniversidad.ToList();
            grdUniversidad.DataBind();
            ListUniversidad = listaUniversidad;

        }
        #endregion

        #region *** UserInterface to Data ***
        private Universidad UserInterfaceToData()
        {
            Universidad universidad = new Universidad();
            universidad.Activo = true;
            if (!string.IsNullOrEmpty(txtNombreUniversidad.Text.Trim()))
                universidad.NombreUniversidad = txtNombreUniversidad.Text;
            if (!string.IsNullOrEmpty(txtDireccion.Text.Trim()))
                universidad.Direccion = txtDireccion.Text;
            return universidad;
        }
        #endregion

        #region *** metodos auxiliares ***
        private void DoDelete(Universidad universidadFilter)
        {

            try
            {
                Universidad universidad = universidadCtrl.Retrieve(universidadFilter, true).First();
                Escuela escuela = (Escuela)userSession.CurrentEscuela.Clone();
                catalogoUniversidadesCtrl.DeleteUniversidadEscuela(dctx, escuela, new CicloEscolar { CicloEscolarID = userSession.CurrentCicloEscolar.CicloEscolarID }, universidad);
            }
            catch (Exception ex)
            {

                this.ShowMessage(ex.Message, MessageType.Error);
            }



        }

        private string EscapeLike(string valueWithoutWildcards)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < valueWithoutWildcards.Length; i++)
            {
                char c = valueWithoutWildcards[i];
                if (c == '*' || c == '%' || c == '[' || c == ']')
                    sb.Append("[").Append(c).Append("]");
                else if (c == '\'')
                    sb.Append("''");
                else
                    sb.Append(c);
            }
            return sb.ToString();
        }

        #endregion

        #region AUTORIZACION DE LA PAGINA


        protected void gv_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (RenderEdit)
                {
                    ImageButton btnEdit = (ImageButton)e.Row.FindControl("btnEdit");
                    btnEdit.Visible = true;
                }

                if (RenderDelete)
                {
                    ImageButton btnDelete = (ImageButton)e.Row.FindControl("btnDel");
                    btnDelete.Visible = true;
                }
            }
        }

        private bool RenderEdit = false;
        private bool RenderDelete = false;

        protected override void DisplayCreateAction()
        {
            PnlCreate.Visible = true;
        }

        protected override void DisplayReadAction()
        {

            grdUniversidad.Visible = true;
        }

        protected override void DisplayUpdateAction()
        {
            RenderEdit = true;
        }

        protected override void DisplayDeleteAction()
        {
            RenderDelete = true;
        }

        protected override void AuthorizeUser()
        {
            //ejemplificando la ejecucion
            List<Permiso> permisos = userSession.CurrentPrivilegiosDirector.GetPermisos();

            bool acceso = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.PAACCESOUNIVERSIDAD) != null;
            bool lectura = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.PAACCESOUNIVERSIDAD) != null;
            bool creacion = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.PAACCESOUNIVERSIDAD) != null;
            bool delete = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.PAACCESOUNIVERSIDAD) != null;
            bool edit = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.PAACCESOUNIVERSIDAD) != null;

            if (!acceso)
                redirector.GoToHomePage(true);
            if (lectura)
                DisplayReadAction();
            if (creacion)
                DisplayCreateAction();
            if (delete)
                DisplayDeleteAction();
            if (edit)
                DisplayUpdateAction();
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