using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Core.Operaciones.Interfaces;
using POV.Modelo.BO;
using POV.Modelo.Service;
using POV.Prueba.Diagnostico.Dinamica.BO;
using POV.Prueba.Diagnostico.Dinamica.Service;
using POV.Prueba.Diagnostico.Service;
using POV.Web.PortalOperaciones.AppCode.Page;
using POV.Prueba.BO;
using POV.Web.PortalOperaciones.Helper;
using POV.Comun.BO;
using POV.Seguridad.BO;

namespace POV.Web.PortalOperaciones.Pruebas
{
    public partial class ConfigurarMetodoClasificacion : PageBase
    {
        private IDataContext dctx = ConnectionHlp.Default.Connection;
        private CatalogoPruebaCtrl catalogoPruebaCtrl;
        private PruebaDinamicaCtrl pruebaDinamicaCtrl;
        public ConfigurarMetodoClasificacion()
        {
            catalogoPruebaCtrl = new CatalogoPruebaCtrl();
            pruebaDinamicaCtrl = new PruebaDinamicaCtrl();
        }
        #region Propiedades de la clase.
        private DataTable DtRutas
        {
            set { this.Session["DtRutas"] = value; }
            get
            {
                return Session["DtRutas"] != null ? Session["DtRutas"] as
                  DataTable : null;
            }
        }
        private List<AEscalaDinamica> ListaEscalasDinamicas
        {
            get { return Session["ListaEscalasEdit"] != null ? Session["ListaEscalasEdit"] as List<AEscalaDinamica> : null; }
            set { Session["ListaEscalasEdit"] = value; }
        }
        public APrueba LastObject
        {
            set { Session["lastPruebas"] = value; }
            get { return Session["lastPruebas"] != null ? Session["lastPruebas"] as APrueba : null; }
        }
        #endregion
        #region Eventos de la página.
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    if (LastObject != null)
                    {
                        if (LastObject is PruebaDinamica)
                        {
                            ListaEscalasDinamicas = new List<AEscalaDinamica>();

                            LastObject = catalogoPruebaCtrl.RetrieveComplete(dctx, LastObject, false);
                            ModeloDinamico mod = (ModeloDinamico)LastObject.Modelo;
                            if (mod.MetodoCalificacion == EMetodoCalificacion.CLASIFICACION)
                            {
                                //Lista temporal de escalas.
                                if (LastObject.ListaPuntajes != null)
                                    foreach (AEscalaDinamica escala in LastObject.ListaPuntajes)
                                        if (escala.Activo == true)
                                            //Si la escala está activa, clonar y agregar a la lista.
                                            ListaEscalasDinamicas.Add((AEscalaDinamica) escala.CloneAll());

                                LoadClasificadores();
                                LoadInfoPrueba();
                                LoadListaEscalas(ListaEscalasDinamicas);
                            }
                            else
                            {
                                txtRedirect.Value = "BuscarPruebas.aspx";
                                ShowMessage("La prueba seleccionada no tiene el método de calificación por clasificación.", MessageType.Error);
                            }
                        }
                        else
                        {
                            txtRedirect.Value = "BuscarPruebas.aspx";
                            ShowMessage("La prueba seleccionada no sigue el modelo de prueba Prueba Dinámica", MessageType.Error);
                        }
                    }
                    else
                    {
                        txtRedirect.Value = "BuscarPruebas.aspx";
                        ShowMessage("No se ha seleccionado un elemento", MessageType.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Service.LoggerHlp.Default.Error(this, ex);
                redirector.GoToHomePage(true);
            }
        }

        protected void BtnAgregarRango_Click(object sender, EventArgs e)
        {
            try
            {
                string sError = ValidarEscala();

                if (string.IsNullOrEmpty(sError))
                {
                    AEscalaDinamica escala = GetEscalaDinamicaFromUI();

                    APoliticaEscalaDinamica politica = new PoliticaEscalaClasificacion();
                    PruebaDinamica prueba = new PruebaDinamica(ListaEscalasDinamicas);
                    politica.Validar(prueba, escala);

                    ListaEscalasDinamicas.Add(escala);

                    LoadListaEscalas(ListaEscalasDinamicas);

                    //Limpiar campos
                    this.txtRangoInicial.Text = "";
                    this.txtRangoFinal.Text = "";
                    this.chbRangoPredominante.Checked = false;
                    this.txtNombre.Text = "";
                    this.txtDescripcion.Text = "";
                }
                else
                {
                    txtRedirect.Value = "";
                    ShowMessage("Errores en los campos: " + sError.Substring(2), MessageType.Error);
                }
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message,MessageType.Error);
            }
        }

        protected void BtnAceptar_Click(object sender, EventArgs e)
        {
            try
            {
                if (LastObject!= null)
                {
                    PruebaDinamica pruebaDinamica = GetPruebaPuntajesFromUI();
                    
                    pruebaDinamicaCtrl.UpdateListEscalas(dctx, pruebaDinamica, (LastObject as PruebaDinamica));
                    txtRedirect.Value = "BuscarPruebas.aspx";
                    ShowMessage("Actualización exitosa", MessageType.Information);

                }
                else
                {
                    txtRedirect.Value = "BuscarPruebas.aspx";
                    ShowMessage("La prueba dinámica no es de tipo Clasificación", MessageType.Information);
                }
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message, MessageType.Error);
            }
        }

        protected void BtnCancelar_Click(object sender, EventArgs e)
        {
            LastObject = null;
            ListaEscalasDinamicas = null;
            DtRutas = null;
            Response.Redirect("BuscarPruebas.aspx");
        }
        #region Eventos de gridview Rangos
        protected void grdRangos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    //Cargar los datos del dropDown ddlClasificadores					
                    DropDownList ddl = (DropDownList)e.Row.FindControl("ddlClasificador");

                    if (ddl!=null)
                    {
                        DataRowView dr = e.Row.DataItem as DataRowView;

                        ddl.DataSource = LoadDatasetClasificadores().Tables[0];
                        ddl.DataValueField = "ClasificadorID";
                        ddl.DataTextField = "Nombre";
                        ddl.DataBind();

                        ddl.SelectedValue = dr[8].ToString();

                        if (dr[0].ToString() != "")
                        {
                            if (LastObject.EstadoLiberacionPrueba == EEstadoLiberacionPrueba.LIBERADA)
                            {
                                ddl.Enabled = false;
                                (e.Row.Cells[0].Controls[0] as TextBox).Enabled = false;
                                (e.Row.Cells[1].Controls[0] as TextBox).Enabled = false;
                                (e.Row.Cells[2].Controls[1] as CheckBox).Enabled = false;
                            } 
                        } 
                    }
                }
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message, MessageType.Error);
            }
        }

        protected void grdRangos_RowEditing(object sender, GridViewEditEventArgs e)
        {
            grdRangos.EditIndex = e.NewEditIndex;

            LoadListaEscalas(ListaEscalasDinamicas);
        }

        protected void grdRangos_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            grdRangos.EditIndex = -1;

            LoadListaEscalas(ListaEscalasDinamicas);
        }

        protected void grdRangos_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {

            string puntajeMinimo = (grdRangos.Rows[e.RowIndex].Cells[0].Controls[0] as TextBox).Text.Trim();
            string puntajeMaximo = (grdRangos.Rows[e.RowIndex].Cells[1].Controls[0] as TextBox).Text.Trim();
            bool esPredominante = (grdRangos.Rows[e.RowIndex].Cells[2].Controls[1] as CheckBox).Checked;
            Clasificador clasificador = GetClasificadorFromUI(grdRangos.Rows[e.RowIndex].Cells[3].Controls[1] as DropDownList);
            string nombre = (grdRangos.Rows[e.RowIndex].Cells[4].Controls[0] as TextBox).Text.Trim();
            string descripcion = (grdRangos.Rows[e.RowIndex].Cells[5].Controls[1] as TextBox).Text.Trim();

            string sError = string.Empty;

            decimal valorMin = 0;
            if (!decimal.TryParse(puntajeMinimo, out valorMin))
                sError += ", puntaje inicial requerido ";
            else if (valorMin < 0)
            {
                sError += ", puntaje inicial debe ser positivo";
            }

            decimal valorMax = 0;
            if (!decimal.TryParse(puntajeMaximo, out valorMax))
                sError += ", puntaje final requerido ";
            else if (valorMax < 0)
            {
                sError += ", puntaje final debe ser positivo";
            }

            if (valorMin >= valorMax)
                sError += ", puntaje inicial no puede ser mayor o igual a Puntaje final ";

            if (string.IsNullOrEmpty(nombre))
                sError = ", nombre requerido";

            if (string.IsNullOrEmpty(sError))
            {
                AEscalaDinamica escalaant = ListaEscalasDinamicas.ElementAt(e.RowIndex);

                AEscalaDinamica escalaValidar;
                escalaValidar = new EscalaClasificacionDinamica();
                escalaValidar.PuntajeMinimo = valorMin;
                escalaValidar.PuntajeMaximo = valorMax;
                escalaValidar.EsPredominante = esPredominante;
                escalaValidar.Clasificador = clasificador;
                escalaValidar.Nombre = nombre;
                escalaValidar.Descripcion = descripcion;

                try
                {
                    PoliticaEscalaClasificacion politica = new PoliticaEscalaClasificacion();
                    if (politica.Validar(new PruebaDinamica(this.ListaEscalasDinamicas.Where(x => x != escalaant)),
                                         escalaValidar))
                    {
                        escalaant.PuntajeMinimo = valorMin;
                        escalaant.PuntajeMaximo = valorMax;
                        escalaant.EsPredominante = esPredominante;
                        escalaant.Clasificador = clasificador;
                        escalaant.Nombre = nombre;
                        escalaant.Descripcion = descripcion;

                        ListaEscalasDinamicas[e.RowIndex] = escalaant;
                        grdRangos.EditIndex = -1;

                        LoadListaEscalas(ListaEscalasDinamicas);
                    }
                    else
					{
						throw new Exception(String.Format(
						"Los puntajes del rango seleccionado son incorrectos:\nLos rangos no deben traslaparse, ni existir conjuntos de puntajes" + 
						" no asignados entre dos rangos.\n\n rango: {0} - {1}",
						escalaValidar.PuntajeMinimo.ToString(), escalaValidar.PuntajeMaximo.ToString()));
					}
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

        protected void grdRangos_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            AEscalaDinamica escala = ListaEscalasDinamicas.ElementAt(e.RowIndex);

            if (escala.PuntajeID != null)
            {
                if (LastObject.EstadoLiberacionPrueba == EEstadoLiberacionPrueba.LIBERADA)
                {
                    txtRedirect.Value = "";
                    ShowMessage("No se puede eliminar un rango de una prueba LIBERADA.",MessageType.Error);
                    return;
                }

                LastObject.PuntajeEliminar(escala);
            }

            ListaEscalasDinamicas.RemoveAt(e.RowIndex);
            grdRangos.EditIndex = -1;

            LoadListaEscalas(ListaEscalasDinamicas);

        }
        #endregion
        #endregion
        #region *** Data To UserInterface ***
        private void LoadListaEscalas(List<AEscalaDinamica> listaEscalas)
        {

            DtRutas = new DataTable();

            DtRutas.Columns.Add("PuntajeID", typeof(string));
            DtRutas.Columns.Add("PuntajeMinimo", typeof(decimal));
            DtRutas.Columns.Add("PuntajeMaximo", typeof(decimal));
            DtRutas.Columns.Add("EsPredominante", typeof(bool));
            DtRutas.Columns.Add("Clasificador", typeof(string));
            DtRutas.Columns.Add("Nombre", typeof(string));
            DtRutas.Columns.Add("Descripcion", typeof(string));
            DtRutas.Columns.Add("TextoPredominante", typeof(string));
            DtRutas.Columns.Add("ClasificadorID", typeof (int));

            foreach (AEscalaDinamica escala in listaEscalas)
            {
                DataRow dr = DtRutas.NewRow();
                dr[0] = escala.PuntajeID != null ? escala.PuntajeID.ToString() : "";
                dr[1] = decimal.ToInt32(escala.PuntajeMinimo.Value);
                dr[2] = decimal.ToInt32(escala.PuntajeMaximo.Value);
                dr[3] = escala.EsPredominante;
                dr[4] = escala.Clasificador.Nombre;
                dr[5] = escala.Nombre;
                dr[6] = escala.Descripcion;
                dr[7] = escala.EsPredominante.Value ? "Si" : "No";
                dr[8] = escala.Clasificador.ClasificadorID;
                DtRutas.Rows.Add(dr);
            }
            grdRangos.DataSource = DtRutas;
            grdRangos.DataBind();
        }
        private void LoadInfoPrueba()
        {
            ModeloDinamico modeloDinamico = (ModeloDinamico)LastObject.Modelo;

            this.txtClavePrueba.Text = LastObject.Clave;
            this.txtModeloPrueba.Text = LastObject.Modelo.Nombre;
            this.txtMetodoCalificacion.Text = modeloDinamico.NombreMetodoCalificacion;
            this.txtEstadoPrueba.Text = LastObject.EstadoLiberacionPrueba.ToString();
        }

        private void LoadClasificadores()
        {
            ddlClasificador.DataSource = LoadDatasetClasificadores().Tables[0];
            ddlClasificador.DataTextField = "Nombre";
            ddlClasificador.DataValueField = "ClasificadorID";
            ddlClasificador.DataBind();
            ddlClasificador.Items.Insert(0,new ListItem("SELECCIONE", ""));
        }
        /// <summary>
        /// Carga los clasificadores en el dropDownList del GridView
        /// </summary>
        private DataSet LoadDatasetClasificadores()
        {
            ModeloCtrl modeloCtrl = new ModeloCtrl();
            Clasificador clasificador = new Clasificador { Activo = true };

            DataSet dsClasificadores = modeloCtrl.RetrieveClasificador(dctx, clasificador, LastObject.Modelo as ModeloDinamico);

            return dsClasificadores;
        }
        #endregion
        #region *** User Interface To Data ***
        private AEscalaDinamica GetEscalaDinamicaFromUI()
        {
            AEscalaDinamica ruta;
            ruta = new EscalaClasificacionDinamica();

            ruta.PuntajeMinimo = decimal.Parse(this.txtRangoInicial.Text);
            ruta.PuntajeMaximo = decimal.Parse(this.txtRangoFinal.Text);
            ruta.EsPredominante = this.chbRangoPredominante.Checked;
            ruta.Clasificador = GetClasificadorFromUI(this.ddlClasificador);
            ruta.Nombre = this.txtNombre.Text.Trim();
            ruta.Descripcion = this.txtDescripcion.Text.Trim();
            ruta.Activo = true;
            ruta.EsPorcentaje = false;

            return ruta;
        }

        /// <summary>
        /// Obtiene un objecto Clasificador de la interfaz.
        /// </summary>
        /// <param name="ddl">DropDownList donde tomará los datos para generar el objeto</param>
        /// <returns>Objeto Clasificador generado a partir de la interfaz</returns>
        private Clasificador GetClasificadorFromUI(DropDownList ddl)
        {
            Clasificador clasificador = new Clasificador();
            int clasificadorID = 0;
            string valorID = ddl.SelectedValue;
            if (int.TryParse(valorID, out clasificadorID))
            {
                if (clasificadorID > 0)
                {
                    clasificador.ClasificadorID = clasificadorID;
                    clasificador.Nombre = ddl.SelectedItem.Text;
                }
            }
            return clasificador;
        }

        private PruebaDinamica GetPruebaPuntajesFromUI()
        {
            List<AEscalaDinamica> puntajes = new List<AEscalaDinamica>();

            foreach (AEscalaDinamica p in LastObject.ListaPuntajes.Where(w => w.Activo == true))
                puntajes.Add((AEscalaDinamica)p.CloneAll());

            PruebaDinamica pruebaDinamica = new PruebaDinamica(puntajes);
            pruebaDinamica.PruebaID = LastObject.PruebaID;
            pruebaDinamica.Modelo = LastObject.Modelo;

            List<APuntaje> escalasEliminadas = LastObject.ListaPuntajes.Where(item => EObjetoEstado.ELIMINADO == LastObject.PuntajeEstado(item)).ToList();

            foreach (APuntaje escala in escalasEliminadas)
            {
                pruebaDinamica.PuntajeEliminar(escala);
            }

            foreach (AEscalaDinamica escala in ListaEscalasDinamicas)
            {
                if (escala.PuntajeID != null)
                {
                    AEscalaDinamica escalaCopy = (AEscalaDinamica)pruebaDinamica.ListaPuntajes.FirstOrDefault(item => item.PuntajeID == escala.PuntajeID);
                    if (escalaCopy != null)
                    {

                        escalaCopy.PuntajeMaximo = escala.PuntajeMaximo;
                        escalaCopy.PuntajeMinimo = escala.PuntajeMinimo;
                        escalaCopy.EsPredominante = escala.EsPredominante;
                        escalaCopy.Clasificador = escala.Clasificador;
                        escalaCopy.Nombre = escala.Nombre;
                        escalaCopy.Descripcion = escala.Descripcion;
                        escalaCopy.Activo = escala.Activo;
                        escalaCopy.EsPorcentaje = escalaCopy.EsPorcentaje;

                    }
                }
                else
                {
                    pruebaDinamica.PuntajeAgregar(escala);
                }
            }
            return pruebaDinamica;
        }
        #endregion
        #region *** Validaciones ***
		private string ValidarEscala()
		{
			string sError = string.Empty;
		    short num;
            decimal valorMin = 0;
            decimal valorMax = 0;
		    string sNumI = this.txtRangoInicial.Text.Trim();
		    string sNumF = this.txtRangoFinal.Text.Trim();

            if (short.TryParse(sNumI,out num))
            {
                if (!decimal.TryParse(this.txtRangoInicial.Text, out valorMin))
                    sError += ", rango inicial requerido ";
                else if (valorMin < 0)
                {
                    sError += ", rango inicial debe ser un número mayor que cero.";
                } 
            }
            else
            {
                txtRedirect.Value = "";
                ShowMessage("Rango inicial: El valor debe ser un número entero", MessageType.Error);
            }

            if (short.TryParse(sNumF,out num))
            {   
                if (!decimal.TryParse(this.txtRangoFinal.Text, out valorMax))
                    sError += ", rango final requerido ";
                else if (valorMax < 0)
                {
                    sError += ", rango final debe ser positivo";
                } 
            }
            else
            {
                txtRedirect.Value = "";
                ShowMessage("Rango final: El valor debe ser un número entero", MessageType.Error);
            }

			if (valorMin >= valorMax)
				sError += ", rango inicial no puede ser mayor o igual a rango final ";

			if (string.IsNullOrEmpty(this.txtNombre.Text.Trim()))
				sError = ", nombre requerido";
			else if (this.txtNombre.Text.Trim().Length > 100)
				sError += ", nombre excede 100 caracteres ";
            if (this.txtDescripcion.Text.Trim().Length > 500)
                sError += ", descripción excede 500 caracteres ";
			if (GetClasificadorFromUI(this.ddlClasificador).ClasificadorID == null)
				sError += ", clasificador requerido ";
				
			return sError;

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
        #region *** Autorizacion de la Pagina ***
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