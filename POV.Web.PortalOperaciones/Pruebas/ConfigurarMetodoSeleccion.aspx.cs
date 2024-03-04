using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Framework.Base.Exceptions;
using Framework.Base.DataAccess;
using POV.Seguridad.BO;
using POV.Web.PortalOperaciones.Helper;
using POV.Web.PortalOperaciones.AppCode.Page;
using POV.Prueba.BO;
using POV.Prueba.Diagnostico.Dinamica.BO;
using POV.Prueba.Diagnostico.Service;
using POV.Modelo.BO;
using POV.Modelo.Service;
using POV.Prueba.Diagnostico.Dinamica.Service;

namespace POV.Web.PortalOperaciones.Pruebas
{
    public partial class ConfigurarMetodoSeleccion : PageBase
    {
        private IDataContext dctx = ConnectionHlp.Default.Connection;

        private CatalogoPruebaCtrl catalogoPruebaCtrl;
        private ModeloCtrl modeloCtrl;
        public APrueba LastObject
        {
            set { Session["lastPruebas"] = value; }
            get { return Session["lastPruebas"] != null ? Session["lastPruebas"] as APrueba : null; }
        }

        private List<APuntaje> ListaPuntajes
        {
            get { return Session["listEscalasDinamica"] != null ? Session["listEscalasDinamica"] as List<APuntaje> : null; }
            set { Session["listEscalasDinamica"] = value; }

        }

        public ConfigurarMetodoSeleccion()
        {
            catalogoPruebaCtrl = new CatalogoPruebaCtrl();
            modeloCtrl = new ModeloCtrl();
        }

        #region *** eventos de pagina ***
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (LastObject != null && LastObject.PruebaID != null)
                {
                    APrueba prueba = catalogoPruebaCtrl.RetrieveComplete(dctx, LastObject, true);

                    if (prueba != null && (prueba is PruebaDinamica))
                    {
                        if ((prueba.Modelo as ModeloDinamico).MetodoCalificacion == EMetodoCalificacion.SELECCION)
                        {
                            LastObject = prueba;

                            
                            ListaPuntajes = new List<APuntaje>();


                            foreach (APuntaje puntaje in LastObject.ListaPuntajes)
                            {
                                if (puntaje.Activo.Value)
                                {
                                    ListaPuntajes.Add((puntaje as AEscalaDinamica).CloneAll() as APuntaje);
                                }
                            }

                            ListaPuntajes = ListaPuntajes.OrderBy(item => (item as AEscalaDinamica).Clasificador.ClasificadorID).ThenBy(item => item.PuntajeMinimo).ToList();
                            LoadInfoPrueba(prueba as PruebaDinamica);
                        }
                        else
                        {
                            txtRedirect.Value = "BuscarPruebas.aspx";
                            ShowMessage("El método de calificación de la prueba no corresponde con la configuración solicitada.", MessageType.Error);
                        }
                    }
                    else
                    {
                        txtRedirect.Value = "BuscarPruebas.aspx";
                        ShowMessage("El tipo de prueba no corresponde con la configuración solicitada.", MessageType.Error);
                    }
                }
                else
                {
                    txtRedirect.Value = "BuscarPruebas.aspx";
                    ShowMessage("No se ha seleccionado un elemento", MessageType.Error);
                }
            }

        }

        protected void BtnGuardar_Click(object sender, EventArgs e)
        {
           
                try
                {
                    PruebaDinamica prueba = GetPruebaDinamicaFromUI();
                    PruebaDinamicaCtrl pruebaDinamicaCtrl = new PruebaDinamicaCtrl();

                    pruebaDinamicaCtrl.UpdateListEscalas(dctx, prueba, prueba);

                    txtRedirect.Value = "BuscarPruebas.aspx";
                    ShowMessage("El método de calificación fue configurador con éxito.", MessageType.Information);
                }
                catch (Exception ex)
                {
                    txtRedirect.Value = "";
                    ShowMessage(ex.Message, MessageType.Error);
                }
           
        }

        protected void BtnCancelar_Click(object sender, EventArgs e)
        {
            LastObject = null;
            ListaPuntajes = null;
            Response.Redirect("BuscarPruebas.aspx", true);
        }

        protected void BtnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                string sError = ValidarEscalaPuntaje();

                if (string.IsNullOrEmpty(sError))
                {
                    APuntaje puntaje = GetPuntajeFromUI();

                    APoliticaEscalaDinamica politica = new PoliticaEscalaSeleccion();
                    PruebaDinamica prueba = new PruebaDinamica(ListaPuntajes);
                    politica.Validar(prueba, puntaje as AEscalaDinamica);

                    ListaPuntajes.Add(puntaje);

                    ListaPuntajes = ListaPuntajes.OrderBy(item => (item as AEscalaDinamica).Clasificador.ClasificadorID).ThenBy(item => item.PuntajeMinimo).ToList();
                    LoadPuntajes(ListaPuntajes);

                    ResetFormAgregarPuntaje();
                }
                else
                {
                    txtRedirect.Value = "";
                    ShowMessage("Errores en los campos: " + sError.Substring(2), MessageType.Error);
                }
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message, MessageType.Error);

            }
        }
        protected void grdRangos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList ddlClasificador = e.Row.FindControl("ddlClasificador") as DropDownList;
                if (ddlClasificador != null)
                {

                    DataRowView dr = e.Row.DataItem as DataRowView;


                    DataSet dsClasificadores = modeloCtrl.RetrieveClasificador(dctx, new Clasificador { Activo = true }, LastObject.Modelo as ModeloDinamico);

                    ddlClasificador.DataSource = dsClasificadores;
                    ddlClasificador.DataValueField = "ClasificadorID";
                    ddlClasificador.DataTextField = "Nombre";
                    ddlClasificador.DataBind();

                    ddlClasificador.SelectedValue = dr[8].ToString();
                    if (dr[0].ToString() != "")
                    {
                        if (LastObject.EstadoLiberacionPrueba == EEstadoLiberacionPrueba.LIBERADA)
                        {
                            ddlClasificador.Enabled = false;
                            (e.Row.Cells[0].Controls[0] as TextBox).Enabled = false;
                            (e.Row.Cells[1].Controls[0] as TextBox).Enabled = false;
                            (e.Row.Cells[2].Controls[1] as CheckBox).Enabled = false;
                        }
                    }
                }
            }
        }

        protected void grdRangos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
        }

        #region Eventos de gridview rutas
        protected void GrdRangos_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GrdRangos.EditIndex = e.NewEditIndex;


            LoadPuntajes(ListaPuntajes);


        }

        protected void GrdRangos_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GrdRangos.EditIndex = -1;

            LoadPuntajes(ListaPuntajes);
        }

        protected void GrdRangos_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            string sError = string.Empty;
            string nombre = string.Empty;
            string descripcion = string.Empty;
            string sPuntajeMinimo = string.Empty;
            string sPuntajeMaximo = string.Empty;
            bool esPredominante = false;
            string clasificadorID = string.Empty;
            string clasificadorNombre = string.Empty;

            int puntajeMinimo = 0;
            int puntajeMaximo = 0;

            APuntaje puntaje = ListaPuntajes.ElementAt(e.RowIndex);

            if (puntaje.PuntajeID != null)
            {
                if (LastObject.EstadoLiberacionPrueba != EEstadoLiberacionPrueba.LIBERADA)
                {
                    sPuntajeMinimo = (GrdRangos.Rows[e.RowIndex].Cells[0].Controls[0] as TextBox).Text.Trim();
                    sPuntajeMaximo = (GrdRangos.Rows[e.RowIndex].Cells[1].Controls[0] as TextBox).Text.Trim();
                    esPredominante = (GrdRangos.Rows[e.RowIndex].Cells[2].Controls[1] as CheckBox).Checked;
                    clasificadorID = (GrdRangos.Rows[e.RowIndex].Cells[3].Controls[1] as DropDownList).SelectedValue.Trim();
                    clasificadorNombre = (GrdRangos.Rows[e.RowIndex].Cells[3].Controls[1] as DropDownList).SelectedItem.Text;


                    if (!int.TryParse(sPuntajeMinimo, out puntajeMinimo))
                        sError += ", respuestas inicial requerido";
                    else if (puntajeMinimo < 0)
                        sError += ", respuestas inicial debe ser mayor o igual que cero";


                    if (!int.TryParse(sPuntajeMaximo, out puntajeMaximo))
                        sError += ", respuestas final requerido";
                    else if (puntajeMaximo <= 0)
                        sError += ", respuestas final debe ser mayor que cero";

                    if (puntajeMinimo >= 0 && puntajeMaximo > 0)
                    {
                        if (puntajeMaximo < puntajeMinimo)
                            sError += ", respuestas inicial debe ser menor que respuestas final";
                    }
                }
            }
            else
            {
                sPuntajeMinimo = (GrdRangos.Rows[e.RowIndex].Cells[0].Controls[0] as TextBox).Text.Trim();
                sPuntajeMaximo = (GrdRangos.Rows[e.RowIndex].Cells[1].Controls[0] as TextBox).Text.Trim();
                esPredominante = (GrdRangos.Rows[e.RowIndex].Cells[2].Controls[1] as CheckBox).Checked;
                clasificadorID = (GrdRangos.Rows[e.RowIndex].Cells[3].Controls[1] as DropDownList).SelectedValue.Trim();
                clasificadorNombre = (GrdRangos.Rows[e.RowIndex].Cells[3].Controls[1] as DropDownList).SelectedItem.Text;


                if (!int.TryParse(sPuntajeMinimo, out puntajeMinimo))
                    sError += ", respuestas inicial requerido";
                else if (puntajeMinimo < 0)
                    sError += ", respuestas inicial debe ser mayor o igual que cero";


                if (!int.TryParse(sPuntajeMaximo, out puntajeMaximo))
                    sError += ", respuestas final requerido";
                else if (puntajeMaximo <= 0)
                    sError += ", respuestas final debe ser mayor que cero";

                if (puntajeMinimo >= 0 && puntajeMaximo > 0)
                {
                    if (puntajeMaximo < puntajeMinimo)
                        sError += ", respuestas inicial debe ser menor que respuestas final";
                }
            }

            nombre = (GrdRangos.Rows[e.RowIndex].Cells[4].Controls[0] as TextBox).Text.Trim();
            descripcion = (GrdRangos.Rows[e.RowIndex].Cells[5].Controls[1] as TextBox).Text.Trim();




            if (string.IsNullOrEmpty(nombre))
                sError += ", Nombre es requerido";

            if (string.IsNullOrEmpty(sError))
            {
                try
                {
                    if (puntaje.PuntajeID == null)
                    {
                        EscalaSeleccionDinamica escalaNueva = new EscalaSeleccionDinamica();
                        escalaNueva.PuntajeMinimo = puntajeMinimo;
                        escalaNueva.PuntajeMaximo = puntajeMaximo;
                        escalaNueva.EsPredominante = esPredominante;
                        (escalaNueva as AEscalaDinamica).Nombre = nombre;
                        (escalaNueva as AEscalaDinamica).Descripcion = descripcion;
                        (escalaNueva as AEscalaDinamica).Clasificador = new Clasificador { ClasificadorID = int.Parse(clasificadorID), Nombre = clasificadorNombre };


                        List<APuntaje> puntajesTemporales = new List<APuntaje>();
                        for (int i = 0; i < ListaPuntajes.Count; i++)
                        {
                            if (i != e.RowIndex)
                            {
                                puntajesTemporales.Add(ListaPuntajes[i]);
                            }
                        }

                        APoliticaEscalaDinamica politica = new PoliticaEscalaSeleccion();
                        PruebaDinamica prueba = new PruebaDinamica(puntajesTemporales);
                        politica.Validar(prueba, escalaNueva as AEscalaDinamica);
                    }
                    

                    if (puntaje.PuntajeID != null)
                    {
                        if (LastObject.EstadoLiberacionPrueba != EEstadoLiberacionPrueba.LIBERADA)
                        {
                            puntaje.PuntajeMinimo = puntajeMinimo;
                            puntaje.PuntajeMaximo = puntajeMaximo;
                            puntaje.EsPredominante = esPredominante;
                            (puntaje as AEscalaDinamica).Clasificador = new Clasificador { ClasificadorID = int.Parse(clasificadorID), Nombre = clasificadorNombre };
                        }
                    }
                    else
                    {
                        puntaje.PuntajeMinimo = puntajeMinimo;
                        puntaje.PuntajeMaximo = puntajeMaximo;
                        puntaje.EsPredominante = esPredominante;
                        (puntaje as AEscalaDinamica).Clasificador = new Clasificador { ClasificadorID = int.Parse(clasificadorID), Nombre = clasificadorNombre };
                    }

                    (puntaje as AEscalaDinamica).Nombre = nombre;
                    (puntaje as AEscalaDinamica).Descripcion = descripcion;
                    ListaPuntajes[e.RowIndex] = puntaje;

                    GrdRangos.EditIndex = -1;
                    ListaPuntajes = ListaPuntajes.OrderBy(item => (item as AEscalaDinamica).Clasificador.ClasificadorID).ThenBy(item => item.PuntajeMinimo).ToList();

                    LoadPuntajes(ListaPuntajes);
                }
                catch (Exception ex)
                {
                    ShowMessage(ex.Message, MessageType.Error);
                }
            }
            else
            {
                txtRedirect.Value = "";
                ShowMessage("Errores en los campos: " + sError.Substring(2), MessageType.Error);
            }
        }

        protected void GrdRangos_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {


            APuntaje ruta = ListaPuntajes.ElementAt(e.RowIndex);

            if (ruta.PuntajeID != null)
            {
                if (LastObject.EstadoLiberacionPrueba == EEstadoLiberacionPrueba.LIBERADA)
                {
                    txtRedirect.Value = "";
                    ShowMessage("No se puede eliminar un rango de una prueba LIBERADA.", MessageType.Error);
                    return;
                }

                LastObject.PuntajeEliminar(ruta);
            }

                

            ListaPuntajes.RemoveAt(e.RowIndex);


            GrdRangos.EditIndex = -1;

            LoadPuntajes(ListaPuntajes);


        }

        #endregion
        #endregion

        #region *** validaciones ***

        private string ValidarEscalaPuntaje()
        {
            string sError = string.Empty;
            string descripcion = txtDescripcion.Text.Trim();
            string pinicial = txtSeleccionInicial.Text.Trim();
            string pfinal = txtSeleccionFinal.Text.Trim();
            string nombre = txtNombre.Text.Trim();
            Clasificador clasificador = GetClasificadorFromUI();

            if (string.IsNullOrEmpty(pinicial))
                sError += ", número respuestas inicial requerido";
            else if (pinicial.Length > 11)
                sError += ", número respuestas inicial máximo 11 caracteres";
            if (string.IsNullOrEmpty(pfinal))
                sError += ", número respuestas final requerido";
            else if (pfinal.Length > 11)
                sError += ", número respuestas final máximo 11 caracteres";

            if (string.IsNullOrEmpty(nombre))
                sError += ", nombre requerido";
            else if (nombre.Length > 100)
                sError += ", nombre máximo 100 caracteres";
            if (clasificador.ClasificadorID == null)
                sError += ", clasificador requerido";
            if (!string.IsNullOrEmpty(descripcion) && descripcion.Length > 500)
                sError += ", descripción máximo 500 caracteres";
            if (!string.IsNullOrEmpty(sError))
                return sError;

            int inicial = 0;
            int final = 0;

            if (!int.TryParse(pinicial, out inicial))
                sError += ", número respuestas inicial debe ser un número entero";
            if (!int.TryParse(pfinal, out final))
                sError += ", número respuestas final debe ser un número entero";

            return sError;
        }

        
        #endregion

        #region *** Data to UserInterface ***
        private void LoadInfoPrueba(PruebaDinamica prueba)
        {
            txtClavePrueba.Text = prueba.Clave;
            txtModeloPrueba.Text = prueba.Modelo.Nombre;
            txtMetodoCalificacion.Text = (prueba.Modelo as ModeloDinamico).NombreMetodoCalificacion;
            LoadClasificadoresFormAgregar(prueba.Modelo as ModeloDinamico);
            txtEstadoPrueba.Text = prueba.EstadoLiberacionPrueba.ToString();
            LoadPuntajes(ListaPuntajes);

        }

        private void LoadPuntajes(List<APuntaje> puntajes)
        {
            DataTable dtPuntajes = new DataTable();
            dtPuntajes.Columns.Add("PuntajeID", typeof(string));
            dtPuntajes.Columns.Add("PuntajeMinimo", typeof(decimal));
            dtPuntajes.Columns.Add("PuntajeMaximo", typeof(decimal));
            dtPuntajes.Columns.Add("EsPredominante", typeof(bool));
            dtPuntajes.Columns.Add("Clasificador", typeof(string));
            dtPuntajes.Columns.Add("Nombre", typeof(string));
            dtPuntajes.Columns.Add("Descripcion", typeof(string));
            dtPuntajes.Columns.Add("TextoPredominante", typeof(string));
            dtPuntajes.Columns.Add("ClasificadorID", typeof(int));
            dtPuntajes.Columns.Add("ReadOnly", typeof(bool));
            dtPuntajes.Columns.Add("VisibleCamposEdit", typeof(bool));
            dtPuntajes.Columns.Add("VisibleCamposInfo", typeof(bool));
            foreach (APuntaje puntaje in ListaPuntajes)
            {
                EscalaSeleccionDinamica escala = puntaje as EscalaSeleccionDinamica;

                DataRow drEscala = dtPuntajes.NewRow();

                drEscala[0] = escala.PuntajeID != null ? escala.PuntajeID.ToString() : "";
                drEscala[1] = decimal.ToInt32(escala.PuntajeMinimo.Value);
                drEscala[2] = decimal.ToInt32(escala.PuntajeMaximo.Value);
                drEscala[3] = escala.EsPredominante;
                drEscala[4] = escala.Clasificador.Nombre;
                drEscala[5] = escala.Nombre;
                drEscala[6] = escala.Descripcion;
                drEscala[7] = escala.EsPredominante.Value ? "Si" : "No";
                drEscala[8] = escala.Clasificador.ClasificadorID;
                if (LastObject.EstadoLiberacionPrueba == EEstadoLiberacionPrueba.LIBERADA)
                {
                    drEscala[9] = true;
                    drEscala[10] = false;
                    drEscala[11] = true;
                }
                else
                {
                    drEscala[9] = false;
                    drEscala[10] = true;
                    drEscala[11] = false;

                }

                dtPuntajes.Rows.Add(drEscala);
            }

            GrdRangos.DataSource = dtPuntajes;
            GrdRangos.DataBind();

        }

        private void LoadClasificadoresFormAgregar(ModeloDinamico modelo)
        {
            DataSet dsClasificadores = modeloCtrl.RetrieveClasificador(dctx, new Clasificador { Activo = true }, LastObject.Modelo as ModeloDinamico);

            ddlClasificadorForm.DataSource = dsClasificadores;
            ddlClasificadorForm.DataValueField = "ClasificadorID";
            ddlClasificadorForm.DataTextField = "Nombre";
            ddlClasificadorForm.DataBind();
            ddlClasificadorForm.Items.Insert(0, new ListItem("SELECCIONE...", ""));
        }

        private void ResetFormAgregarPuntaje()
        {
            txtSeleccionFinal.Text = "";
            txtSeleccionInicial.Text = "";
            txtNombre.Text = "";
            txtDescripcion.Text = "";
            chkRangoPredominante.Checked = false;
            LoadClasificadoresFormAgregar(LastObject.Modelo as ModeloDinamico);
        }
        #endregion

        #region *** UserInterface to Data ***

        private PruebaDinamica GetPruebaDinamicaFromUI()
        {
            PruebaDinamica prueba = new PruebaDinamica();
            prueba.PruebaID = LastObject.PruebaID;
            prueba.Modelo = LastObject.Modelo;

            foreach (APuntaje puntaje in ListaPuntajes)
            {
                if (puntaje.PuntajeID == null)
                {
                    LastObject.PuntajeAgregar(puntaje);
                }
                else
                {
                    APuntaje puntajeCopy = LastObject.ListaPuntajes.FirstOrDefault(item => item.PuntajeID == puntaje.PuntajeID);
                    if (puntajeCopy != null)
                    {
                        puntajeCopy.PuntajeMinimo = puntaje.PuntajeMinimo;
                        puntajeCopy.PuntajeMaximo = puntaje.PuntajeMaximo;
                        puntajeCopy.EsPredominante = puntaje.EsPredominante;
                        (puntajeCopy as AEscalaDinamica).Nombre = (puntaje as AEscalaDinamica).Nombre;
                        (puntajeCopy as AEscalaDinamica).Descripcion = (puntaje as AEscalaDinamica).Descripcion;
                        (puntajeCopy as AEscalaDinamica).Clasificador = (puntaje as AEscalaDinamica).Clasificador;

                    }

                }
            }

            return LastObject as PruebaDinamica;
        }
        private APuntaje GetPuntajeFromUI()
        {
            APuntaje puntaje = new EscalaSeleccionDinamica();
            puntaje.PuntajeMinimo = decimal.Parse(txtSeleccionInicial.Text);
            puntaje.PuntajeMaximo = decimal.Parse(txtSeleccionFinal.Text);

            puntaje.Activo = true;
            puntaje.EsPorcentaje = false;
            puntaje.EsPredominante = chkRangoPredominante.Checked;
            (puntaje as EscalaSeleccionDinamica).Nombre = txtNombre.Text;
            (puntaje as EscalaSeleccionDinamica).Descripcion = txtDescripcion.Text;
            (puntaje as EscalaSeleccionDinamica).Clasificador = GetClasificadorFromUI();



            return puntaje;
        }

        private Clasificador GetClasificadorFromUI()
        {
            Clasificador clasificador = new Clasificador();
            int clasificadorID = 0;
            string valorID = ddlClasificadorForm.SelectedValue;
            if (int.TryParse(valorID, out clasificadorID))
            {
                if (clasificadorID > 0)
                {
                    clasificador.ClasificadorID = clasificadorID;
                    clasificador.Nombre = ddlClasificadorForm.SelectedItem.Text;
                }
            }

            return clasificador;
        }
        #endregion

        #region *** metodos auxiliares ***
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

        #region AUTORIZACION DE LA PAGINA

        protected override void AuthorizeUser()
        {
            //ejemplificando la ejecucion
            List<Permiso> permisos = userSession.CurrentPrivilegios.GetPermisos();

            bool acceso = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POACCESOCATPRUEBAS) != null;
            bool edicion = permisos.FirstOrDefault(x => x.PermisoID == (int)EPermiso.POEDITARCATPRUEBA) != null;

            if (!acceso)
                redirector.GoToHomePage(true);
            if (!edicion)
                redirector.GoToHomePage(true);
        }
        #endregion
    }
}