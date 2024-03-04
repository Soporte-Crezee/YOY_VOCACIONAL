using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.CentroEducativo.BO;
using POV.CentroEducativo.Services;
using POV.Expediente.BO;
using POV.Expediente.Services;
using POV.Licencias.Service;
using POV.Logger.Service;
using POV.Seguridad.BO;
using POV.Seguridad.Service;
using POV.Web.PortalUniversidad.AppCode.Page;
using POV.Web.PortalUniversidad.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace POV.Web.PortalUniversidad.Pages
{
    public partial class VincularExpediente : CatalogPage
    {
        #region Propiedades de la clase
        private List<Alumno> Session_Alumnos
        {
            get { return Session["ListAlumno"] != null ? Session["ListAlumno"] as List<Alumno> : null; }
            set { Session["ListAlumno"] = value; }
        }

        private Usuario UsuarioDocente
        {
            get { return Session["UsuarioDocente"] != null ? Session["UsuarioDocente"] as Usuario : null; }
            set { Session["UsuarioDocente"] = value; }
        }

        private List<Alumno> AlumnosPreAsignados
        {
            get { return Session["AlumnosPreAsignados"] != null ? Session["AlumnosPreAsignados"] as List<Alumno> : null; }
            set { Session["AlumnosPreAsignados"] = value; }
        }

        private List<long> AlumnosSeleccionados
        {
            get { return Session["AlumnosSeleccionados"] != null ? Session["AlumnosSeleccionados"] as List<long> : null; }
            set { Session["AlumnosSeleccionados"] = value; }
        }

        private Docente LastObjDocente
        {
            get { return Session["LastObjDocente"] != null ? Session["LastObjDocente"] as Docente : null; }
            set { Session["LastObjDocente"] = value; }
        }

        private EFDocenteCtrl eFDocenteCtrl;
        private LicenciaEscuelaCtrl licenciaEscuelaCtrl;
        private UsuarioCtrl usuarioCtrl;
        private IDataContext dctx = ConnectionHlp.Default.Connection;
        private UsuarioExpedienteCtrl usuarioExpedienteCtrl;
        private EFAlumnoCtrl eFAlumnoctrl;
        #endregion

        public VincularExpediente()
        {
            eFDocenteCtrl = new EFDocenteCtrl(null);
            licenciaEscuelaCtrl = new LicenciaEscuelaCtrl();
            usuarioCtrl = new UsuarioCtrl();
            usuarioExpedienteCtrl = new UsuarioExpedienteCtrl();
            eFAlumnoctrl = new EFAlumnoCtrl(null);
        }
        #region Eventos de la pagina
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (userSession != null && userSession.CurrentUniversidad != null && userSession.CurrentCicloEscolar != null)
                    if (!IsPostBack)
                        LoadAsignaciones(userSession.CurrentUniversidad, LastObjDocente);
            }
            catch (Exception ex)
            {
                this.ShowMessage(ex.Message, MessageType.Error);
                LoggerHlp.Default.Error(this, ex);
            }
        }

        protected void grdAlumnos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                KeepSelection((GridView)sender);
                grdAlumnos.PageIndex = e.NewPageIndex;
                grdAlumnos.DataSource = Session_Alumnos;
                grdAlumnos.DataBind();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "reloadControlsGrid();", true);
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message, MessageType.Error);
            }
        }

        protected void grdAlumnos_PageIndexChanged(object sender, EventArgs e)
        {
            try
            {
                RestoreSelection((GridView)sender);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "reloadControlsGrid();", true);
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message, MessageType.Error);
            }
        }

        protected void grdPreAsignaciones_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdPreAsignaciones.DataSource = AlumnosPreAsignados;
                grdPreAsignaciones.PageIndex = e.NewPageIndex;
                grdPreAsignaciones.DataBind();
            }
            catch (Exception ex)
            {
                ShowMessage("Error al realizar el paginado de los resultados de la consulta " + ex.Message, MessageType.Error);
            }
        }

        protected void grdPreAsignaciones_PageIndexChanged(object sender, EventArgs e)
        {

        }

        protected void grdPreAsignaciones_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                switch (e.CommandName)
                {
                    case "Eliminar":
                        try
                        {
                            var asignacion = (AlumnosPreAsignados.Where(x => x.AlumnoID == long.Parse(Convert.ToInt64(e.CommandArgument).ToString(CultureInfo.InvariantCulture)))
                                .ToList()).FirstOrDefault().AlumnoID;
                            UsuarioExpediente usuarioExpediente = new UsuarioExpediente();
                            usuarioExpediente.AlumnoID = asignacion;
                            usuarioExpediente.UsuarioID = UsuarioDocente.UsuarioID;

                            UsuarioExpediente usExp = usuarioExpedienteCtrl.LastDataRowToUsuarioExpediente(usuarioExpedienteCtrl.Retrieve(dctx, usuarioExpediente));
                            usuarioExpedienteCtrl.Delete(dctx, usExp);
                            AlumnosPreAsignados.Remove(AlumnosPreAsignados.First(x => x.AlumnoID == asignacion));

                            grdPreAsignaciones.DataSource = AlumnosPreAsignados;
                            grdPreAsignaciones.DataBind();

                            Alumno alumno = new Alumno();
                            alumno.AlumnoID = asignacion;
                            alumno = eFAlumnoctrl.Retrieve(new Alumno { AlumnoID = alumno.AlumnoID }, false).FirstOrDefault();
                            Session_Alumnos.Add(alumno);
                            grdAlumnos.DataSource = Session_Alumnos;
                            grdAlumnos.DataBind();
                        }
                        catch (Exception ex)
                        {
                            ShowMessage("No fue posible actualizar la lista" + ex, MessageType.Error);
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Error" + ex, MessageType.Error);
            }
        }

        protected void btnNuevaSeleccion_Click(object sender, EventArgs e)
        {
            try
            {
                redirector.GoToConsultarOrientador(false);
                Session_Alumnos = null;
                AlumnosPreAsignados = null;
                AlumnosSeleccionados = null;
                Session["ListaSeleccionados"] = null;
                LastObjDocente = null;
                UsuarioDocente = null;
            }
            catch (Exception ex)
            {
                this.ShowMessage(ex.Message, MessageType.Error);
            }
        }

        /// <summary>
        /// Método que cancela la nueva asignacion que se esté realizando, limpiando los campos de seleccion y redigiriendose a la pantalla de consulta
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        protected void btnAsignar_Click(object sender, EventArgs e)
        {
            try
            {
                KeepSelection(grdAlumnos);
                var listSeleccionesId = (List<long>)Session["ListaSeleccionados"];
                if (listSeleccionesId.Count > 0)
                {
                    List<long> listAlumnosID = (List<long>)Session["ListaSeleccionados"];
                    List<Alumno> alumnos = new List<Alumno>();

                    foreach (var item in listAlumnosID)
                    {
                        Alumno alumno = new Alumno();
                        alumno.AlumnoID = Convert.ToInt64(item);
                        Alumno itemalumno = eFAlumnoctrl.Retrieve(alumno, false).First();
                        alumnos.Add(itemalumno);
                    }

                    foreach (Alumno al in alumnos)
                    {
                        UsuarioExpediente usuarioExpediente = new UsuarioExpediente();
                        usuarioExpediente.AlumnoID = al.AlumnoID;
                        usuarioExpediente.UsuarioID = UsuarioDocente.UsuarioID;
                        usuarioExpedienteCtrl.Insert(dctx, usuarioExpediente);
                        Session_Alumnos.Remove(Session_Alumnos.First(x => x.AlumnoID == usuarioExpediente.AlumnoID));
                        AlumnosPreAsignados.Add(al);
                    }
                    Session["ListaSeleccionados"] = null;
                    grdAlumnos.DataSource = Session_Alumnos;
                    grdAlumnos.DataBind();

                    grdPreAsignaciones.DataSource = AlumnosPreAsignados;
                    grdPreAsignaciones.DataBind();
                    redirector.GoToVincularExpediente(true);
                }
                else
                {
                    ShowMessage("Debes seleccionar al menos un estudiante", MessageType.Error);
                }
            }
            catch (Exception exception)
            {
                ShowMessage(exception.Message, MessageType.Error);
            }
        }
        #endregion

        #region Metodos Auxiliares
        private void LoadAsignaciones(Universidad universidad, Docente docente)
        {
            Session_Alumnos = universidad.Alumnos.GroupBy(test => test.AlumnoID).Select(grp => grp.First()).ToList();

            #region Docente
            Docente docUni = eFDocenteCtrl.Retrieve(docente, false).FirstOrDefault();
            Usuario usuario = licenciaEscuelaCtrl.RetrieveUsuarios(ConnectionHlp.Default.Connection, docUni).Where(x => x.UniversidadId == userSession.CurrentUniversidad.UniversidadID).FirstOrDefault();

            List<Docente> listDocente = eFDocenteCtrl.Retrieve(new Docente(), false);
            List<Usuario> listUsuario = new List<Usuario>();
            foreach (Docente doc in listDocente)
            {
                List<Usuario> usuarioUniversidad = licenciaEscuelaCtrl.RetrieveUsuarios(ConnectionHlp.Default.Connection, doc).ToList();
                foreach (Usuario us in usuarioUniversidad)
                {
                    if (us.UniversidadId == userSession.CurrentUniversidad.UniversidadID)
                    {
                        listUsuario.Add(us);
                    }
                }
            }

            UsuarioDocente = usuario;

            List<Alumno> listaPreAsignados = new List<Alumno>();
            List<UsuarioExpediente> listaUsuarioExpediente = new List<UsuarioExpediente>();
            DataSet ds = usuarioExpedienteCtrl.Retrieve(dctx, new UsuarioExpediente());
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    UsuarioExpediente usExp = usuarioExpedienteCtrl.DataRowToUsuarioExpediente(ds.Tables[0].Rows[i]);
                    // Obtener el alumno
                    Alumno alumno = eFAlumnoctrl.Retrieve(new Alumno { AlumnoID = usExp.AlumnoID }, false).FirstOrDefault();
                    if (alumno != null)
                    {
                        listaPreAsignados.Add(alumno);
                    }
                    foreach (Usuario us in listUsuario)
                    {
                        if (usExp.AlumnoID == alumno.AlumnoID && usExp.UsuarioID == us.UsuarioID)
                        {
                            listaUsuarioExpediente.Add(usExp);
                        }
                    }
                }
            }

            Session_Alumnos = (from item in Session_Alumnos
                               join item2 in listaUsuarioExpediente
                               on item.AlumnoID equals item2.AlumnoID into g
                               where !g.Any()
                               select item).ToList();


            listaUsuarioExpediente = listaUsuarioExpediente.Where(x => x.UsuarioID == usuario.UsuarioID).ToList();
            var lista = new List<Alumno>();
            foreach (UsuarioExpediente item in listaUsuarioExpediente)
            {
                Alumno alumno = eFAlumnoctrl.Retrieve(new Alumno { AlumnoID = item.AlumnoID }, false).FirstOrDefault();
                lista.Add(alumno);
            }

            AlumnosPreAsignados = lista;
            #endregion

            UserInterfaceToData(docUni, usuario);
            grdAlumnos.DataSource = Session_Alumnos;
            grdAlumnos.DataBind();

            grdPreAsignaciones.DataSource = AlumnosPreAsignados;
            grdPreAsignaciones.DataBind();
        }

        private void UserInterfaceToData(Docente docente, Usuario usuario)
        {
            lblNombre.Text = docente.Nombre;
            lblApellido.Text = docente.PrimerApellido + " " + docente.SegundoApellido;
            lblCorreo.Text = docente.Correo;
            lblUsuario.Text = usuario.NombreUsuario;
        }

        /// <summary>
        /// Limpia los campos de la interfaz de usuario
        /// </summary>
        public void ClearFields()
        {
            GridViewRowCollection rows = grdAlumnos.Rows;
            foreach (GridViewRow row in rows)
            {
                CheckBox selectedItem = (CheckBox)row.FindControl("cbSeleccionado");
                if (selectedItem.Checked)
                {
                    selectedItem.Checked = false;
                }
            }
            Session["ListaSeleccionados"] = null;
        }
        #endregion

        #region Paginacion
        /// <summary>
        /// Mantiene el seleccionado del grid
        /// </summary>
        /// <param name="grid"></param>
        public void KeepSelection(GridView grid)
        {
            // Se obtiene los id de los aspirantes seleccionados de la pagina actual
            List<long> checkedAlumnos = (from item in grid.Rows.Cast<GridViewRow>()
                                         let chek = (CheckBox)item.FindControl("cbSeleccionado")
                                         where chek.Checked
                                         select Convert.ToInt64(grid.DataKeys[item.RowIndex].Value)).ToList();

            // se recupera de session la lista de seleccionados previamente
            List<long> listaSeleccionados = Session["ListaSeleccionados"] as List<long>;
            if (listaSeleccionados == null)
                listaSeleccionados = new List<long>();

            // se cruzan todos los registros de la pagina actual del gridview con la lista de seleccionados,
            // si algun item de esa pagina fue marcado previamente no se devuelve
            listaSeleccionados = (from item in listaSeleccionados
                                  join item2 in grid.Rows.Cast<GridViewRow>()
                                  on item equals Convert.ToInt64(grid.DataKeys[item2.RowIndex].Value) into g
                                  where !g.Any()
                                  select item).ToList();

            // se agregan los seleccionados
            listaSeleccionados.AddRange(checkedAlumnos);

            Session["ListaSeleccionados"] = listaSeleccionados;
            AlumnosSeleccionados = listaSeleccionados;
        }

        public void RestoreSelection(GridView grid)
        {
            List<long> listaSeleccionados = Session["ListaSeleccionados"] as List<long>;
            if (listaSeleccionados == null)
                return;

            // Ce comparan los registro de la pagina del grid con los recuperados de la session
            // los coincidentes se devuelven para ser seleccionados
            List<GridViewRow> result = (from item in grid.Rows.Cast<GridViewRow>()
                                        join item2 in listaSeleccionados
                                        on Convert.ToInt64(grid.DataKeys[item.RowIndex].Value) equals item2 into g
                                        where g.Any()
                                        select item).ToList();

            // se recorre cada item para marcarlo
            result.ForEach(x => ((CheckBox)x.FindControl("cbSeleccionado")).Checked = true);
        }

        #endregion

        #region Autorizacion de la pagina
        private bool RenderEdit = false;
        private bool RenderDelete = false;
        protected void grdAlumnos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    string AlumnoID = e.Row.Cells[0].Text;
                    CheckBox cbSeleccionado = (CheckBox)e.Row.FindControl("cbSeleccionado");
                    cbSeleccionado.Attributes.Add("AlumnoID", AlumnoID);
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Error al llenar el grid de la consulta", MessageType.Error);
            }

        }

        protected void grdPreAsignaciones_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    ImageButton btnDelete = (ImageButton)e.Row.FindControl("btnEliminar");
                    btnDelete.Visible = true;
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Error al llenar el grid de la consulta", MessageType.Error);
            }
        }

        protected override void DisplayCreateAction()
        {
            contenidoPrincipal.Visible = true;
        }

        protected override void DisplayReadAction()
        {
            grdAlumnos.Visible = true;
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

            bool acceso = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.PUACCESOXPEDIENTE) != null;
            bool lectura = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.PUACCESOXPEDIENTE) != null;
            bool creacion = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.PUACCESOXPEDIENTE) != null;
            bool delete = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.PUACCESOXPEDIENTE) != null;
            bool edit = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.PUACCESOXPEDIENTE) != null;

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

        #region Message Showing
        /// <summary>
        /// Desplega el mensaje de error/advertencia/informacion en UI
        /// </summary>
        /// <param name="message"> Menesaje a desplegar </param>
        /// <param name="messageType"> Tipo de mesnsaje </param>
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
        /// Desplega el mensaje de error/advertencia/informacion en la UI
        /// </summary>
        /// <param name="message"> Mensaje a desplegar </param>
        /// <param name="typeNotification"> 1:Error, 2:Advertencia, 3:Informacion </param>
        private void ShowMessage(string message, string typeNotification)
        {
            // Se ubican los controles que manejan el desplegado de error/advertencia/informacion
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