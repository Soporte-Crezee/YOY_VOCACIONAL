using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Framework.Base.Exceptions;
using POV.CentroEducativo.BO;
using POV.Seguridad.BO;
using POV.Web.PortalUniversidad.AppCode.Page;
using POV.CentroEducativo.Services;
using POV.Logger.Service;

namespace POV.Web.PortalUniversidad.Eventos
{
    public partial class BuscarEventos : CatalogPage
    {
        private EventoUniversidadCtrl eventoUniversidadCtrl;

        #region *** propiedades de clase ***
        public DataSet DsEventoUniversidadsEscuela
        {
            get { return Session["EventoUniversidad"] != null ? Session["EventoUniversidad"] as DataSet : null; }
            set { Session["EventoUniversidad"] = value; }
        }
        public EventoUniversidad LastObject
        {
            get { return Session["LastEvento"] != null ? (EventoUniversidad)Session["LastEvento"] : null; }
            set { Session["LastEvento"] = value; }
        }
        #endregion

        public BuscarEventos()
        {
            eventoUniversidadCtrl = new EventoUniversidadCtrl(null); 
        }

        #region *** eventos de pagina ***
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                LastObject = null;
                if (userSession != null && userSession.CurrentUniversidad != null && userSession.CurrentCicloEscolar != null)
                {
                    if (!IsPostBack)
                        LoadEventosUniversidad(new EventoUniversidad());
                }
                else
                {
                    redirector.GoToLoginPage(true);
                }
            }
            catch (Exception ex)
            {
                this.ShowMessage(ex.Message, MessageType.Error);
                LoggerHlp.Default.Error(this, ex);
            }
        }
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                EventoUniversidad EventoUniversidad = UserInterfaceToData();
                LoadEventosUniversidad(EventoUniversidad);
            }
            catch (Exception ex)
            {
                this.ShowMessage(ex.Message, MessageType.Error);
            }
        }
        protected void grdEventosUniversidad_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                switch (e.CommandName)
                {
                    case "editar":
                        var eventoUniversidad = new EventoUniversidad { EventoUniversidadId = long.Parse(e.CommandArgument.ToString()) };
                        LastObject = eventoUniversidadCtrl.Retrieve(new EventoUniversidad { EventoUniversidadId = long.Parse(e.CommandArgument.ToString()) }, false).FirstOrDefault();
                        redirector.GoToEditarEvento(true);
                        break;
                    case "eliminar":
                        LastObject = new EventoUniversidad { EventoUniversidadId = long.Parse(e.CommandArgument.ToString()) };
                        DoDelete(LastObject);
                        LastObject = null;
                        LoadEventosUniversidad(new EventoUniversidad());
                        break;

                }
            }
            catch (Exception ex)
            {

                ShowMessage(ex.Message, MessageType.Error);
            }
        }
        #endregion

        #region *** validaciones ***

        #endregion

        #region *** Data to UserInterface ***
        private void LoadEventosUniversidad(EventoUniversidad EventoUniversidadfilter)
        {
            //Formar DataSet
            DataSet dsCompose = new DataSet();
            dsCompose.Tables.Add(new DataTable());

            dsCompose.Tables[0].Columns.Add(new DataColumn("EventoUniversidadID", typeof(int)));
            dsCompose.Tables[0].Columns.Add(new DataColumn("Nombre", typeof(string)));
            dsCompose.Tables[0].Columns.Add(new DataColumn("FechaInicio", typeof(string)));
            dsCompose.Tables[0].Columns.Add(new DataColumn("FechaFin", typeof(string)));

            if(userSession.CurrentUniversidad.EventosUniversidad != null)
            {
                foreach (EventoUniversidad evento in userSession.CurrentUniversidad.EventosUniversidad)
                {
                    DataRow dr = dsCompose.Tables[0].NewRow();
                    dr.SetField("EventoUniversidadID", evento.EventoUniversidadId);
                    dr.SetField("Nombre", evento.Nombre);
                    dr.SetField("FechaInicio", string.Format("{0:dd/MM/yyyy}", evento.FechaInicio));
                    dr.SetField("FechaFin", string.Format("{0:dd/MM/yyyy}", evento.FechaFin));
                    dsCompose.Tables[0].Rows.Add(dr);
                }
            }
            if (EventoUniversidadfilter != null)
                if (!string.IsNullOrEmpty(EventoUniversidadfilter.Nombre))
                {
                    //agregar filtros.

                    string strFilter = string.Empty;
                    if (!string.IsNullOrEmpty(EventoUniversidadfilter.Nombre))
                        strFilter += string.Format("AND Nombre LIKE '%{0}%' ", EscapeLike(EventoUniversidadfilter.Nombre));

                    if (strFilter.StartsWith("AND"))
                        strFilter = strFilter.Substring(4);

                    if (strFilter.Trim().Length > 0)
                    {
                        DataView dataView = new DataView(dsCompose.Tables[0]);
                        dataView.RowFilter = strFilter;
                        strFilter = string.Empty;
                        dsCompose.Tables.Clear();
                        dsCompose.Tables.Add(dataView.ToTable());
                    }
                }
            grdEventoUniversidades.DataSource = dsCompose; 
            grdEventoUniversidades.DataBind();
        }
        #endregion

        #region *** UserInterface to Data ***
        private EventoUniversidad UserInterfaceToData()
        {
            EventoUniversidad obj = new EventoUniversidad();

            if (!string.IsNullOrEmpty(txtNombre.Text.Trim()))
                obj.Nombre = txtNombre.Text;
            
            obj.UniversidadId = userSession.CurrentUniversidad.UniversidadID;

            return obj;
        } 
        #endregion

        #region *** metodos auxiliares ***
        private void DoDelete(EventoUniversidad eventoUniversidad)
        {

            try
            {
                var evento = eventoUniversidadCtrl.Retrieve(LastObject, true).FirstOrDefault();
                eventoUniversidadCtrl.Delete(evento);

                // Se recarga las relaciones de la universidad-carrera
                UniversidadCtrl sessionUpdCtrl = new UniversidadCtrl(null);
                userSession.CurrentUniversidad = sessionUpdCtrl.RetrieveWithRelationship(userSession.CurrentUniversidad, false).FirstOrDefault();
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

            grdEventoUniversidades.Visible = true;
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
            List<Permiso> permisos = userSession.CurrentPrivilegiosUniversidad.GetPermisos();

            bool acceso = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.PUACCESOEVENTOS) != null;
            bool lectura = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.PUACCESOEVENTOS) != null;
            bool creacion = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.PUACCESOEVENTOS) != null;
            bool delete = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.PUACCESOEVENTOS) != null;
            bool edit = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.PUACCESOEVENTOS) != null;

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