using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Framework.Base.DataAccess;
using Framework.Base.Exceptions;
using POV.Comun.BO;
using POV.Core.Operaciones.Interfaces;
using POV.Modelo.BO;
using POV.Modelo.Service;
using POV.Prueba.BO;
using POV.Prueba.Diagnostico.Dinamica.BO;
using POV.Prueba.Diagnostico.Dinamica.Service;
using POV.Prueba.Diagnostico.Service;
using POV.Seguridad.BO;
using POV.Web.PortalOperaciones.AppCode.Page;
using POV.Web.PortalOperaciones.Helper;

namespace POV.Web.PortalOperaciones.Pruebas
{
    public partial class ConfigurarMetodoPorcentaje : PageBase
    {
		private IDataContext dctx = ConnectionHlp.Default.Connection;
		private CatalogoPruebaCtrl catalogoPruebaCtrl;
		private PruebaDinamicaCtrl pruebaDinamicaCtrl;
		private IRedirector redirector;

        public ConfigurarMetodoPorcentaje()
        {
			catalogoPruebaCtrl = new CatalogoPruebaCtrl();
			pruebaDinamicaCtrl = new PruebaDinamicaCtrl();
        }

		#region *** Propiedades de Clase ***
		private DataSet DsEscalaDinamica
		{
			set { this.Session["escaladinamica"] = value; }
			get { return (DataSet)this.Session["escaladinamica"]; }
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
		#region *** Eventos de Pagina
		protected void Page_Load(object sender, EventArgs e)
		{
			try
			{
				if (!IsPostBack)
				{
					if (LastObject != null && LastObject.PruebaID != null)
					{
						APrueba prueba = catalogoPruebaCtrl.RetrieveComplete(dctx, LastObject, true);
						if (prueba != null && prueba is PruebaDinamica)
						{
							if ((prueba.Modelo as ModeloDinamico).MetodoCalificacion == EMetodoCalificacion.PORCENTAJE)
							{
								LastObject = prueba;
								ListaEscalasDinamicas = new List<AEscalaDinamica>();

								//Se almacena temporal las listas de las escalas
								foreach (AEscalaDinamica escala in LastObject.ListaPuntajes)
								{
									if (escala.Activo == true) //Si la escala está activa, clonar y agregar a la lista.
										ListaEscalasDinamicas.Add((AEscalaDinamica)escala.CloneAll());
								}

								LoadClasificadores();
								LoadInfoPrueba();
								LoadListaEscalas(ListaEscalasDinamicas);
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
			catch (Exception ex)
			{
				Logger.Service.LoggerHlp.Default.Error(this, ex);
				redirector.GoToHomePage(true);
			}
		}

		protected void BtnAgregarRango_Click(object sender, EventArgs e)
		{
			if (this.grdRangos.EditIndex < 0)
			{
				string sError = ValidarEscala();

				if (string.IsNullOrEmpty(sError))
				{
					AEscalaDinamica escala = GetEscalaDinamicaFromUI();

					try
					{
						PoliticaEscalaPorcentaje politica = new PoliticaEscalaPorcentaje();
						if (politica.Validar(new PruebaDinamica(this.ListaEscalasDinamicas), escala))
						{
							ListaEscalasDinamicas.Add(escala);
							LoadListaEscalas(ListaEscalasDinamicas);

							//Limpiar campos
							this.txtPorcentajeInicial.Text = "";
							this.txtPorcentajeFinal.Text = "";
							this.chbRangoPredominante.Checked = false;
							this.txtNombre.Text = "";
							this.txtaDescripcion.Value = "";
						}
						else
						{
							throw new Exception(String.Format(
							"Los porcentajes del rango seleccionado son incorrectos:\n" + 
							"Los rangos no deben traslaparse, ni existir conjuntos de porcentajes no asignados entre dos rangos.\n\n Rango: {0} - {1}",
							escala.PuntajeMinimo.ToString(), escala.PuntajeMaximo.ToString()));
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
		}

		protected void BtnAceptar_Click(object sender, EventArgs e)
		{
			if (this.grdRangos.EditIndex < 0)
			{
				PruebaDinamica pruebaDinamica = GetPruebaPuntajesFromUI();

				try
				{
					if (pruebaDinamica.ListaPuntajes.Where(x => x.EsPredominante == true).Count() != 1)
						throw new Exception("Es necesario que exista un único rango predominante");
					
					pruebaDinamicaCtrl.UpdateListEscalas(dctx, pruebaDinamica, (LastObject as PruebaDinamica));
					Response.Redirect("BuscarPruebas.aspx");
				}
				catch (Exception ex)
				{
					ShowMessage(ex.Message, MessageType.Error);
				}
			}
		}

		protected void BtnCancelar_Click(object sender, EventArgs e)
		{
			Response.Redirect("BuscarPruebas.aspx");
		}

		#region Eventos de gridview Rangos
		protected void grdRangos_RowDataBound(object sender, GridViewRowEventArgs e)
		{
			try
			{
				if (e.Row.RowType == DataControlRowType.DataRow)
				{
					if ((e.Row.RowState & DataControlRowState.Edit) > 0)
					{
						//Cargar los datos del dropDown ddlClasificadores					
						DropDownList ddl = (DropDownList)e.Row.FindControl("ddlClasificador");

						ddl.DataSource = LoadDatasetClasificadores().Tables[0];
						ddl.DataTextField = "Nombre";
						ddl.DataValueField = "ClasificadorID";
						ddl.DataBind();

						AEscalaDinamica escala = ListaEscalasDinamicas.ElementAt(e.Row.RowIndex);
						ddl.SelectedIndex = ddl.Items.IndexOf(ddl.Items.FindByText(escala.Clasificador.Nombre));

						//Aplicar el estado del checkbox de la fila
						CheckBox chk = (CheckBox)e.Row.FindControl("chkEsPredominante");
						chk.Checked = escala.EsPredominante.Value;

						//Poner la Descripción en el textArea
						HtmlTextArea txta = (HtmlTextArea)e.Row.FindControl("txtaDescripcion");
						txta.Value = escala.Descripcion;

						// Si la prueba está liberada, sólo se puede editar el NOMBRE y la DESCRIPCIÓN
						if (LastObject.EstadoLiberacionPrueba == EEstadoLiberacionPrueba.LIBERADA)
						{
							e.Row.Cells[0].Enabled = false;
							e.Row.Cells[1].Enabled = false;
							e.Row.Cells[2].Enabled = false;
							e.Row.Cells[3].Enabled = false;
						}
					}
					else
					{
						e.Row.Cells[6].Controls[2].Visible = (LastObject.EstadoLiberacionPrueba != EEstadoLiberacionPrueba.LIBERADA);
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
			string descripcion = (grdRangos.Rows[e.RowIndex].Cells[5].Controls[1] as HtmlTextArea).Value.Trim();

			string sError = string.Empty;

			decimal valorMin = 0;
			if (string.IsNullOrEmpty(puntajeMinimo))
				sError += ", rango inicial es requerido ";
			else if (puntajeMinimo.Length > 8)
				sError += ", rango inicial no puede tener más de 8 caracteres ";
			else if (!decimal.TryParse(puntajeMinimo, out valorMin))
				sError += ", rango inicial debe ser numérico ";
			else if (valorMin < 0)
			{
				sError += ", rango inicial no puede ser menor a cero";
			}
			else if (valorMin > 100)
			{
				sError += ", rango inicial no puede ser mayor a cien";
			}

			decimal valorMax = 0;
			if (string.IsNullOrEmpty(puntajeMaximo))
				sError += ", rango final es requerido ";
			else if (puntajeMaximo.Length > 8)
				sError += ", rango final no puede tener más de 8 caracteres ";
			else if (!decimal.TryParse(puntajeMaximo, out valorMax))
				sError += ", rango final debe ser numérico ";
			else if (valorMax < 0)
			{
				sError += ", rango final no puede ser menor a cero";
			}
			else if (valorMax > 100)
			{
				sError += ", rango final no puede ser mayor a cien";
			}

			if (string.IsNullOrEmpty(sError) && valorMin >= valorMax)
				sError += ", el rango inicial no puede ser mayor o igual a el rango final ";

			if (string.IsNullOrEmpty(nombre))
				sError += ", nombre es requerido";
			else if (nombre.Length > 100)
				sError += ", nombre no puede tener más de 100 caracteres ";

			if (descripcion.Length > 500)
				sError += ", descripción no puede tener más de 500 caracteres ";

			if (string.IsNullOrEmpty(sError))
			{
				AEscalaDinamica escalaAnterior = ListaEscalasDinamicas.ElementAt(e.RowIndex);

				AEscalaDinamica escalaValidar;
				escalaValidar = new EscalaPorcentajeDinamica();
				escalaValidar.PuntajeMinimo = valorMin;
				escalaValidar.PuntajeMaximo = valorMax;
				escalaValidar.EsPredominante = esPredominante;
				escalaValidar.Clasificador = clasificador;
				escalaValidar.Nombre = nombre;

				try
				{
					PoliticaEscalaPorcentaje politica = new PoliticaEscalaPorcentaje();
					if (politica.Validar(new PruebaDinamica(this.ListaEscalasDinamicas.Where(x => x != escalaAnterior)), escalaValidar))
					{
						escalaAnterior.PuntajeMinimo = valorMin;
						escalaAnterior.PuntajeMaximo = valorMax;
						escalaAnterior.EsPredominante = esPredominante;
						escalaAnterior.Clasificador = clasificador;
						escalaAnterior.Nombre = nombre;
						escalaAnterior.Descripcion = descripcion;

						ListaEscalasDinamicas[e.RowIndex] = escalaAnterior;
						grdRangos.EditIndex = -1;

						LoadListaEscalas(ListaEscalasDinamicas);
					}
					else
					{
						throw new Exception(String.Format(
						"Los porcentajes del rango seleccionado son incorrectos:\n" +
						"Los rangos no deben traslaparse, ni existir conjuntos de porcentajes no asignados entre dos rangos.\n\n Rango: {0} - {1}",
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
				LastObject.PuntajeEliminar(escala);

			ListaEscalasDinamicas.RemoveAt(e.RowIndex);
			grdRangos.EditIndex = -1;

			LoadListaEscalas(ListaEscalasDinamicas);

		}

		private void LoadListaEscalas(List<AEscalaDinamica> listaEscalas)
		{

			DataTable dtRutas = new DataTable();

			dtRutas.Columns.Add("PuntajeMinimo", typeof(string));
			dtRutas.Columns.Add("PuntajeMaximo", typeof(string));
			dtRutas.Columns.Add("EsPredominante", typeof(string));
			dtRutas.Columns.Add("Clasificador", typeof(string));
			dtRutas.Columns.Add("Nombre", typeof(string));
			dtRutas.Columns.Add("Descripcion", typeof(string));

			foreach (AEscalaDinamica escala in listaEscalas)
			{
				DataRow dr = dtRutas.NewRow();
				dr[0] = String.Format("{0:N2}", escala.PuntajeMinimo);
				dr[1] = String.Format("{0:N2}", escala.PuntajeMaximo);
				dr[2] = escala.EsPredominante == true ? "Sí" : "No";
				dr[3] = escala.Clasificador.Nombre;
				dr[4] = escala.Nombre;
				dr[5] = escala.Descripcion;
				dtRutas.Rows.Add(dr);
			}
			grdRangos.DataSource = dtRutas;
			grdRangos.DataBind();
		}
		#endregion
		#endregion
		#region *** Data To UserInterface ***
		private void LoadInfoPrueba()
		{
			this.txtClavePrueba.Text = LastObject.Clave;
			this.txtModeloPrueba.Text = LastObject.Modelo.Nombre;
			this.txtMetodoCalificacion.Text = (LastObject.Modelo as ModeloDinamico).NombreMetodoCalificacion;
			this.txtEstadoLiberacion.Text = LastObject.EstadoLiberacionPrueba.ToString();
		}

		private void LoadClasificadores()
		{
			ddlClasificador.DataSource = LoadDatasetClasificadores().Tables[0];
			ddlClasificador.DataTextField = "Nombre";
			ddlClasificador.DataValueField = "ClasificadorID";
			ddlClasificador.SelectedIndex = 0;
			ddlClasificador.DataBind();
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
			ruta = new EscalaPorcentajeDinamica();

			ruta.PuntajeMinimo = decimal.Parse(this.txtPorcentajeInicial.Text);
			ruta.PuntajeMaximo = decimal.Parse(this.txtPorcentajeFinal.Text);
			ruta.EsPredominante = this.chbRangoPredominante.Checked;
			ruta.Clasificador = GetClasificadorFromUI(this.ddlClasificador);
			ruta.Nombre = this.txtNombre.Text.Trim();
			ruta.Descripcion = this.txtaDescripcion.Value.Trim();
			ruta.Activo = true;
			ruta.EsPorcentaje = true;

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

			decimal valorMin = 0;
			if (string.IsNullOrEmpty(this.txtPorcentajeInicial.Text))
				sError += ", porcentaje inicial es requerido ";
			else if (this.txtPorcentajeInicial.Text.Length > 8)
				sError += ", porcentaje inicial no puede tener más de 8 caracteres ";
			else if (!decimal.TryParse(this.txtPorcentajeInicial.Text, out valorMin))
				sError += ", porcentaje inicial debe ser numérico ";
			else if (valorMin < 0)
			{
				sError += ", porcentaje inicial no puede ser menor a cero";
			}
			else if (valorMin > 100)
			{
				sError += ", porcentaje inicial no puede ser mayor a cien";
			}

			decimal valorMax = 0;
			if (string.IsNullOrEmpty(this.txtPorcentajeFinal.Text))
				sError += ", porcentaje final es requerido ";
			else if (this.txtPorcentajeFinal.Text.Length > 8)
				sError += ", porcentaje final no puede tener más de 8 caracteres ";
			else if (!decimal.TryParse(this.txtPorcentajeFinal.Text, out valorMax))
				sError += ", porcentaje final debe ser numérico ";
			else if (valorMax < 0)
			{
				sError += ", porcentaje final no puede ser menor a cero";
			}
			else if (valorMax > 100)
			{
				sError += ", porcentaje final no puede ser mayor a cien";
			}

			if (string.IsNullOrEmpty(sError) && valorMin >= valorMax)
				sError += ", porcentaje inicial no puede ser mayor o igual a Porcentaje final ";

			if (string.IsNullOrEmpty(this.txtNombre.Text.Trim()))
				sError = ", nombre es requerido";
			else if (this.txtNombre.Text.Trim().Length > 100)
				sError += ", nombre no puede tener más de 100 caracteres ";

			if (this.txtaDescripcion.Value.Trim().Length > 500)
				sError += ", descripción no puede tener más de 500 caracteres ";

			if (GetClasificadorFromUI(this.ddlClasificador).ClasificadorID == null)
				sError += ", clasificador es requerido ";

			return sError;

		}
		#endregion
		#region *** Metodos Auxiliares ***
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