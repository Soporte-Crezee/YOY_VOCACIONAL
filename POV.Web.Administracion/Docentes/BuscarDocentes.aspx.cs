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
using POV.Seguridad.BO;
using POV.Modelo.Context;
using POV.CentroEducativo.Services;
using POV.Expediente.BO;
using POV.Expediente.Services;

namespace POV.Web.Administracion.Docentes
{
    public partial class BuscarDocentes : CatalogPage
    {
        private DocenteCtrl docenteCtrl;
        private EscuelaCtrl escuelaCtrl;
        private UsuarioCtrl usuarioCtrl;
        private LicenciaEscuelaCtrl licenciaEscuelaCtrl;
        private CatalogoDocentesCtrl catalogoDocentesCtrl;

        #region *** propiedades de clase ***
        public DataSet DsDocentesEscuela
        {
            get { return Session["docentesescuela"] != null ? Session["docentesescuela"] as DataSet : null; }
            set { Session["docentesescuela"] = value; }
        }
        public AsignacionDocenteEscuela LastObject
        {
            get { return Session["LastAsignacionDocente"] != null ? (AsignacionDocenteEscuela)Session["LastAsignacionDocente"] : null; }
            set { Session["LastAsignacionDocente"] = value; }
        }
        #endregion

        public BuscarDocentes()
        {
            docenteCtrl = new DocenteCtrl();
            escuelaCtrl = new EscuelaCtrl();
            usuarioCtrl = new UsuarioCtrl();
            licenciaEscuelaCtrl = new LicenciaEscuelaCtrl();
            catalogoDocentesCtrl = new CatalogoDocentesCtrl();
 
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
                        LoadDocentes(new Docente());
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
                Docente docente = UserInterfaceToData();
                LoadDocentes(docente);
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
                        LastObject = new AsignacionDocenteEscuela { AsignacionDocenteEscuelaID = long.Parse(e.CommandArgument.ToString()) };
                        redirector.GoToEditarDocente(true);
                        break;
                    case "eliminar":
                        LastObject = new AsignacionDocenteEscuela { AsignacionDocenteEscuelaID = long.Parse(e.CommandArgument.ToString()) };
                        LastObject = LoadAsignacionDocenteEscuela();
                        DoDelete(LastObject);
                        LastObject = null;
                        LoadDocentes(new Docente());
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
        private void LoadDocentes(Docente docentefilter)
        {
            Escuela escuela = escuelaCtrl.RetrieveComplete(ConnectionHlp.Default.Connection, userSession.CurrentEscuela);


            //Formar DataSet
            DataSet dsCompose = new DataSet();
            dsCompose.Tables.Add(new DataTable());

            dsCompose.Tables[0].Columns.Add(new DataColumn("DocenteID", typeof(int)));
            dsCompose.Tables[0].Columns.Add(new DataColumn("UsuarioID", typeof(int)));
            dsCompose.Tables[0].Columns.Add(new DataColumn("NombreCompleto", typeof(string)));
            dsCompose.Tables[0].Columns.Add(new DataColumn("Nombre", typeof(string)));
            dsCompose.Tables[0].Columns.Add(new DataColumn("PrimerApellido", typeof(string)));
            dsCompose.Tables[0].Columns.Add(new DataColumn("SegundoApellido", typeof(string)));
            dsCompose.Tables[0].Columns.Add(new DataColumn("Curp", typeof(string)));
            dsCompose.Tables[0].Columns.Add(new DataColumn("FechaNacimiento", typeof(string)));
            dsCompose.Tables[0].Columns.Add(new DataColumn("Sexo", typeof(string)));
            dsCompose.Tables[0].Columns.Add(new DataColumn("Correo", typeof(string)));
            dsCompose.Tables[0].Columns.Add(new DataColumn("AsignacionDocenteEscuelaID", typeof(long)));

            if (escuela.AsignacionDocentes.Any())
                foreach (AsignacionDocenteEscuela rowDocente in escuela.AsignacionDocentes)
                {
                    if ((bool)(!rowDocente.Activo))
                        continue;

                    Docente ddocente = docenteCtrl.RetrieveComplete(ConnectionHlp.Default.Connection, new Docente { DocenteID = rowDocente.Docente.DocenteID });
                    Usuario usuario = licenciaEscuelaCtrl.RetrieveUsuario(ConnectionHlp.Default.Connection, new Docente { Curp = ddocente.Curp });
                    LicenciaEscuela licenciaDocente = RetrieveLicenciaDocente(usuario);

                    if (licenciaDocente == null)
                        continue;

                    DataRow dr = dsCompose.Tables[0].NewRow();
                    dr.SetField("DocenteID", ddocente.DocenteID);
                    dr.SetField("NombreCompleto", string.Format("{0} {1} {2}", ddocente.Nombre, ddocente.PrimerApellido, ddocente.SegundoApellido));
                    dr.SetField("Nombre", ddocente.Nombre);
                    dr.SetField("PrimerApellido", ddocente.PrimerApellido);
                    dr.SetField("SegundoApellido", ddocente.SegundoApellido);
                    dr.SetField("Curp", ddocente.Curp);
                    dr.SetField("FechaNacimiento", string.Format("{0:dd/MM/yyyy}", ddocente.FechaNacimiento));
                    dr.SetField("Sexo", ddocente.Sexo != null && (bool)ddocente.Sexo ? "HOMBRE" : "MUJER");
                    dr.SetField("AsignacionDocenteEscuelaID", rowDocente.AsignacionDocenteEscuelaID);

                    usuario = usuarioCtrl.LastDataRowToUsuario(usuarioCtrl.Retrieve(ConnectionHlp.Default.Connection, usuario));
                    dr.SetField("UsuarioID", usuario.UsuarioID);
                    dr.SetField("Correo", usuario.Email);

                    dsCompose.Tables[0].Rows.Add(dr);
                }

            if (docentefilter != null)
                if (!string.IsNullOrEmpty(docentefilter.Nombre) || !string.IsNullOrEmpty(docentefilter.Curp) || !string.IsNullOrEmpty(docentefilter.PrimerApellido) || !string.IsNullOrEmpty(docentefilter.SegundoApellido) || !string.IsNullOrEmpty(docentefilter.Correo) || docentefilter.Sexo != null)
                {
                    //agregar filtros.

                    string strFilter = string.Empty;
                    if (!string.IsNullOrEmpty(docentefilter.Nombre))
                        strFilter += string.Format("AND Nombre LIKE '%{0}%' ", EscapeLike(docentefilter.Nombre));

                    if (!string.IsNullOrEmpty(docentefilter.PrimerApellido))
                        strFilter += string.Format("AND PrimerApellido LIKE '%{0}%' ", EscapeLike(docentefilter.PrimerApellido));

                    if (!string.IsNullOrEmpty(docentefilter.SegundoApellido))
                        strFilter += string.Format("AND SegundoApellido LIKE '%{0}%' ", EscapeLike(docentefilter.SegundoApellido));

                    if (!string.IsNullOrEmpty(docentefilter.Correo))
                        strFilter += string.Format("AND Correo LIKE '%{0}%' ", EscapeLike(docentefilter.Correo));

                    if (!string.IsNullOrEmpty(docentefilter.Curp))
                        strFilter += string.Format("AND Curp = '{0}' ", EscapeLike(docentefilter.Curp));

                    if (docentefilter.Sexo != null)
                        strFilter += string.Format("AND Sexo = '{0}' ", EscapeLike((bool)docentefilter.Sexo ? "HOMBRE" : "MUJER"));

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

            grdDocentesEscuela.DataSource = dsCompose;
            grdDocentesEscuela.DataBind();
            DsDocentesEscuela = dsCompose;

        }
        #endregion

        #region *** UserInterface to Data ***
        private Docente UserInterfaceToData()
        {
            Docente docente = new Docente();
            if (!string.IsNullOrEmpty(txtCurp.Text.Trim()))
                docente.Curp = txtCurp.Text;
            if (!string.IsNullOrEmpty(txtNombre.Text.Trim()))
                docente.Nombre = txtNombre.Text;
            if (!string.IsNullOrEmpty(txtPrimerApellido.Text.Trim()))
                docente.PrimerApellido = txtPrimerApellido.Text.Trim();
            if (!string.IsNullOrEmpty(txtSegundoApellido.Text.Trim()))
                docente.SegundoApellido = txtSegundoApellido.Text.Trim();
            if (!string.IsNullOrEmpty(txtCorreoElectronico.Text.Trim()))
                docente.Correo = txtCorreoElectronico.Text.Trim();

            if (CbxSexo.SelectedIndex != -1)
            {
                bool sx;
                if (bool.TryParse(CbxSexo.SelectedItem.Value, out sx))
                    docente.Sexo = sx;
                else
                    docente.Sexo = null;
            }
            else
                docente.Sexo = null;

            return docente;
        } 
        #endregion

        #region *** metodos auxiliares ***
        private void DoDelete(AsignacionDocenteEscuela asignacionDocente)
        {

            try
            {
                Escuela escuela = (Escuela)userSession.CurrentEscuela.Clone();
                List<Usuario> userOrientador = licenciaEscuelaCtrl.RetrieveUsuarios(ConnectionHlp.Default.Connection, new Docente { DocenteID = asignacionDocente.Docente.DocenteID, Curp = asignacionDocente.Docente.Curp });

                if (userOrientador.Count > 0) 
                {
                    foreach (Usuario usruni in userOrientador)
                    {
                        catalogoDocentesCtrl.DeleteDocenteEscuela(ConnectionHlp.Default.Connection, escuela, new CicloEscolar { CicloEscolarID = userSession.CurrentCicloEscolar.CicloEscolarID }, asignacionDocente.Docente, null, (long)usruni.UniversidadId);
                        #region *** Desvincular Orientador-Universidad
                        var objeto = new object();
                        var contexto = new Contexto(objeto);

                        UniversidadCtrl universidadCtrl = new UniversidadCtrl(contexto);
                        var universidadSave = universidadCtrl.RetrieveWithRelationship(new Universidad { UniversidadID = usruni.UniversidadId }, true).FirstOrDefault();

                        EFDocenteCtrl efDocenteCtrl = new EFDocenteCtrl(contexto);

                        Docente docenteSeleccionado = efDocenteCtrl.Retrieve(asignacionDocente.Docente, true).FirstOrDefault();
                        UsuarioExpedienteCtrl usuarioExpedienteCtrl = new UsuarioExpedienteCtrl();
                        #region eliminar Relacion Usuario-Expediente del Docente
                        UsuarioExpediente usuarioExpediente = new UsuarioExpediente();
                        usuarioExpediente.UsuarioID = usruni.UsuarioID;

                        DataSet ds = usuarioExpedienteCtrl.Retrieve(ConnectionHlp.Default.Connection, usuarioExpediente);
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                UsuarioExpediente usExp = usuarioExpedienteCtrl.DataRowToUsuarioExpediente(ds.Tables[0].Rows[i]);
                                usuarioExpedienteCtrl.Delete(ConnectionHlp.Default.Connection, usExp);
                            }
                        }
                        #endregion

                        if (universidadSave.Docentes.FirstOrDefault(x => x.DocenteID == docenteSeleccionado.DocenteID) != null)
                        {
                            universidadSave.Docentes.Remove(docenteSeleccionado);
                        }
                        universidadCtrl.Update(universidadSave);
                        contexto.Commit(objeto);
                        contexto.Dispose();

                        #endregion
                    }
                }
                
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

        private LicenciaEscuela RetrieveLicenciaDocente(Usuario usrdocente)
        {

            //Licencia de la escuela
            LicenciaEscuela licEscuela = new LicenciaEscuela { CicloEscolar = userSession.CurrentCicloEscolar, Escuela = userSession.CurrentEscuela, Activo = true };

            DataSet ds = licenciaEscuelaCtrl.Retrieve(ConnectionHlp.Default.Connection, licEscuela);
            if (ds.Tables[0].Rows.Count == 0)
                throw new Exception("La escuela no cuenta con licencia para el ciclo escolar especificado.");

            licEscuela = licenciaEscuelaCtrl.LastDataRowToLicenciaEscuela(ds);

            //Licencias del docente
            List<LicenciaEscuela> lscenciasDoc = licenciaEscuelaCtrl.RetrieveLicencia(ConnectionHlp.Default.Connection, usrdocente);
            if (!lscenciasDoc.Any())
                return null;


            foreach (LicenciaEscuela licdocente in lscenciasDoc)
            {
                if (licdocente.Activo == true && licdocente.LicenciaEscuelaID == licEscuela.LicenciaEscuelaID)
                    return licdocente;
            }
            return null;
        }

        private AsignacionDocenteEscuela LoadAsignacionDocenteEscuela()
        {
            //Carga Datos Escuela y Ciclo Escolar
            Escuela escuela = escuelaCtrl.RetrieveComplete(ConnectionHlp.Default.Connection, userSession.CurrentEscuela);


            if (LastObject == null || LastObject.AsignacionDocenteEscuelaID == null)
                throw new Exception("BuscarDocentes: Ocurrió un error al procesar su solicitud");

            if (escuela.AsignacionDocentes.Any())
            {

                AsignacionDocenteEscuela asignacion = (escuela.AsignacionDocentes.Where(asig => asig.Activo == true && asig.AsignacionDocenteEscuelaID == LastObject.AsignacionDocenteEscuelaID)).First();

                return asignacion;
            }
            return null;
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

            grdDocentesEscuela.Visible = true;
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

            bool acceso = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.PAACCESODOCENTES) != null;
            bool lectura = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.PACONSULTARDOCENTES) != null;
            bool creacion = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.PAINSERTARDOCENTES) != null;
            bool delete = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.PAELIMINARDOCENTES) != null;
            bool edit = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.PAEDITARDOCENTES) != null;

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