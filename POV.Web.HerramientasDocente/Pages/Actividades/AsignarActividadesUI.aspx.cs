using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using POV.AppCode.Page;
using POV.ConfiguracionActividades.BO;
using POV.Content.MasterPages;
using POV.ServiciosActividades.Controllers;
using POV.Licencias.Service;
using POV.CentroEducativo.Service;
using POV.CentroEducativo.BO;
using POV.Web.Helper;
using POV.Licencias.BO;

namespace POV.Web.HerramientasDocente.Pages.Actividades
{
	public partial class AsignarActividadesUI : PageBase
	{
		#region Propiedades
		private DataTable AlumnosAAsignar
		{
			get
			{
				return Session["dtSeleccionados"] as DataTable;
			}
		}

		private List<ActividadDocente> Actividades
		{
			get
			{
                return (List<ActividadDocente>)Session["ListaActividades"];
			}
			set
			{
				Session["ListaActividades"] = value;
			}
		}

        private ActividadDocente ActividadSeleccionada
		{
			get
			{
                return (ActividadDocente)Session["ActividadSeleccionada"];
			}
			set
			{
                Session["ActividadSeleccionada"] = value;
			}
		}
		
		private int EscuelaId
		{
			get
			{
				return (int) Session["EscuelaId"];
			}
		}

		private List<AsignacionActividad> ActividadesPreAsignadas
		{
			get
			{
				return (List<AsignacionActividad>) Session["ListaActividadesPreAsignadas"];
			}
			set
			{
				Session["ListaActividadesPreAsignadas"] = value;
			}
		}

        private int? ClasificadorID
        {
            get { return Session["ClasificadorID"] as int?; }
            set { Session["ClasificadorID"] = value; }
        }

		private AsignarActividadesDocenteController _controller;
		#endregion

        //LA: Decalred 
        private ContratoCtrl contratoCtrl;
        private CicloContratoCtrl cicloContratoCtrl;
        private CicloEscolarCtrl cicloEscolarCtrl;
        private CicloEscolar cicloEscolar;

		/// <summary>
		/// Método que inicia la carga de la interfaz de usuario
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void Page_Load(object sender, EventArgs e)
		{
			_controller = new AsignarActividadesDocenteController(null);

            contratoCtrl = new ContratoCtrl();
            cicloContratoCtrl = new CicloContratoCtrl();
            cicloEscolarCtrl = new CicloEscolarCtrl();
            cicloEscolar = new CicloEscolar();
			if (!IsPostBack)
			{
				try
				{
					PopulateAlumnos();
					PopulateActividades();
					ActividadesPreAsignadas = new List<AsignacionActividad>();
				}
				catch (Exception exception)
				{
					ShowMessage("Carga: " + exception.Message, Content.MasterPages.Site.EMessageType.Error);
				}

			}
		}

		#region Populate
		/// <summary>
		/// Método que carga los alumnos a la interfaz de usuario
		/// </summary>
		private void PopulateAlumnos()
		{
			try
			{
				var resultado = from c in AlumnosAAsignar.AsEnumerable()
								group c by new
								{
									Grado = c["Grado"],
									Grupo = c["NombreGrupo"]
								}
									into grupo
									select new
									{
										GradoGrupo = grupo.Key,
										Alumnos = (from a in grupo
												   select new
												   {
													  AlumnoID= a["AlumnoID"],
													   Nombre = a["NombreCompletoAlumno"]
												   })
									};


				ltvGrado.DataSource = resultado;
				ltvGrado.DataBind();

				ScriptManager.RegisterStartupScript(this, typeof(Page), "acordeon", "cargarAcordeon();", true);
			}
			catch (Exception exception)
			{
				ShowMessage("No fue posible presentar los alumnos en la tabla " + exception.Message, Content.MasterPages.Site.EMessageType.Error);
			}
		}

		/// <summary>
		/// Método que carga las actividades a la interfaz de usuario a partir de las aptitudes de los alumnos
		/// </summary>
		private void PopulateActividades()
		{
			var criteriaActividad = new ActividadDocente();

            var lstActividades = new List<ActividadDocente>();

            criteriaActividad.EscuelaId = userSession.CurrentEscuela.EscuelaID;
			criteriaActividad.DocenteId = userSession.CurrentDocente.DocenteID;
            criteriaActividad.UsuarioId = userSession.CurrentUser.UsuarioID;
            
            //criteriaActividad.ClasificadorID = ClasificadorID;
            
            var lstActividadesTmp = _controller.ConsultarActividadesDocente(criteriaActividad).Where(x=>x.ClasificadorID==ClasificadorID);

			lstActividades.AddRange(lstActividadesTmp);
            
			Actividades = lstActividades;
            

			grvActividades.DataSource = lstActividades;
			grvActividades.DataBind();

			grvDetalleActividad.DataSource = null;
			grvDetalleActividad.DataBind();
		}
		#endregion

		#region Métodos Genéricos
		/// <summary>
		/// Mantiene el seleccionado del grid
		/// </summary>
		/// <param name="grid"></param>
		public void KeepSelection(GridView grid)
		{
			//
			// se obtienen los id de las actividades seleccionadas de la pagina actual
			List<int> checkedAlumnos = (from item in grid.Rows.Cast<GridViewRow>()
										let check = (CheckBox) item.FindControl("cbSeleccionado")
										where check.Checked
										select Convert.ToInt32(grid.DataKeys[item.RowIndex].Value)).ToList();

			//
			// se recupera de session la lista de seleccionados previamente
			//
			List<int> listaSeleccionados = Session["ListaSeleccionados"] as List<int>;

			if (listaSeleccionados == null)
				listaSeleccionados = new List<int>();

			//
			// se cruzan todos los registros de la pagina actual del gridview con la lista de seleccionados,
			// si algun item de esa pagina fue marcado previamente no se devuelve
			//
			listaSeleccionados = (from item in listaSeleccionados
								  join item2 in grid.Rows.Cast<GridViewRow>()
									 on item equals Convert.ToInt32(grid.DataKeys[item2.RowIndex].Value) into g
								  where !g.Any()
								  select item).ToList();

			//
			// se agregan los seleccionados
			//
			listaSeleccionados.AddRange(checkedAlumnos);

			Session["ListaSeleccionados"] = listaSeleccionados;

		}

		/// <summary>
		/// Metodo para mantener el seleccionado del grid
		/// </summary>
		/// <param name="grid"></param>
		public void RestoreSelection(GridView grid)
		{

			List<int> listaSeleccionados = Session["ListaSeleccionados"] as List<int>;

			if (listaSeleccionados == null)
				return;

			//
			// se comparan los registros de la pagina del grid con los recuperados de la Session
			// los coincidentes se devuelven para ser seleccionados
			//
			List<GridViewRow> result = (from item in grid.Rows.Cast<GridViewRow>()
										join item2 in listaSeleccionados
										on Convert.ToInt32(grid.DataKeys[item.RowIndex].Value) equals item2 into g
										where g.Any()
										select item).ToList();

			//
			// se recorre cada item para marcarlo
			//
			result.ForEach(x => ((CheckBox) x.FindControl("cbSeleccionado")).Checked = true);

		}

		/// <summary>
		/// Método para comparar las fechas que se quieren asignar
		/// </summary>
		/// <returns>Mensaje del resultdo</returns>
		public String ValidateDate()
		{
			var mensaje = String.Empty;
			var fechaInicio = new DateTime();
			var fechaFin = new DateTime();
			return mensaje;
		}

		/// <summary>
		/// Método que valida los campos requeridos de la interfaz
		/// </summary>
		/// <returns>Mensaje con el resultado de la validación</returns>
		public String ValidateFields()
		{
			var mensaje = String.Empty;

			var listActividadesId = (List<int>) Session["ListaSeleccionados"];

			if (listActividadesId.Count == 0)
				mensaje = mensaje + "Actividad,";
			if (AlumnosAAsignar.Rows.Count == 0)
				mensaje = mensaje + "Alumnos,";

			if (!mensaje.Equals(String.Empty))
				mensaje = "Los siguientes campos son requeridos: " + mensaje + " favor de completar.";

			return mensaje;
		}

		/// <summary>
		/// Método que realiza el insert de las asignaciones de las actividades
		/// </summary>
		public void DoInsert()
		{
            var actividadesAsignadas = new List<ActividadDocente>();
			var listActividadesId = (List<int>) Session["ListaSeleccionados"];

			try
			{
				var mensaje = String.Empty;

                //LA: Asignacion de Ultimo contrato registrado para la obtencion del ciclo escolar
                var SS_LastObject = (contratoCtrl.LastDataRowToContrato(contratoCtrl.Retrieve(ConnectionHlp.Default.Connection, new Contrato() { })));
                SS_LastObject = contratoCtrl.LastDataRowToContrato(contratoCtrl.Retrieve(ConnectionHlp.Default.Connection, SS_LastObject));
                DataSet ds = cicloContratoCtrl.Retrieve(ConnectionHlp.Default.Connection, SS_LastObject, new CicloContrato { Activo = true });
                CicloContrato cicloContrato = cicloContratoCtrl.LastDataRowToCicloContrato(ds);
                cicloEscolar = new CicloEscolar { CicloEscolarID = cicloContrato.CicloEscolar.CicloEscolarID };
                cicloEscolar = cicloEscolarCtrl.LastDataRowToCicloEscolar(cicloEscolarCtrl.Retrieve(ConnectionHlp.Default.Connection, cicloEscolar));
                var hdFechaInicio = cicloEscolar.InicioCiclo.ToString().Split(' ')[0];//(HiddenField)row.FindControl("hdFechaInicio");
                var hdFechaFin = cicloEscolar.FinCiclo.ToString().Split(' ')[0];//(HiddenField)row.FindControl("hdFechaFin");
				foreach (var idSeleccionado in listActividadesId)
				{
					actividadesAsignadas.Add(
						Actividades.Where(x => x.ActividadID == idSeleccionado).ToList().FirstOrDefault());

					foreach (DataRow asignacionEnfasis in AlumnosAAsignar.Rows)
					{
						var asignacion = new AsignacionActividad
						{
                            FechaInicio = DateTime.ParseExact(hdFechaInicio.Trim() + " 00:00", "dd/MM/yyyy HH:mm",
                                CultureInfo.InvariantCulture),
                            FechaFin = DateTime.ParseExact(hdFechaFin.Trim() + " 23:59", "dd/MM/yyyy HH:mm",
                                CultureInfo.InvariantCulture),
							FechaCreacion = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0,
								0, 0),
							ActividadId = actividadesAsignadas.Where(x => x.ActividadID == idSeleccionado).ToList().FirstOrDefault().ActividadID,
							AlumnoId = Convert.ToInt64(asignacionEnfasis["AlumnoID"]),
                            EsManual = true,
							TareasRealizadas = new List<TareaRealizada>(),
                            AsignadoPor=2
						};

						
                        foreach (var tarea in actividadesAsignadas.First(a => a.ActividadID == idSeleccionado).Tareas)
						{
							var tareaRealizada = new TareaRealizada
							{
								TareaId = tarea.TareaId,
								Estatus = EEstatusTarea.No_Iniciado,
								Acumulado = 0
							};

							asignacion.TareasRealizadas.Add(tareaRealizada);
						}

						asignacion = _controller.InsertAsignacionActividad(asignacion);
						_controller.LoadActividadAsignacion(asignacion);
						ActividadesPreAsignadas.Add(asignacion);						
					}
				}
			}
			catch (Exception exception)
			{
				ShowMessage("No fue posible realizar la asignación. " + exception.Message, Content.MasterPages.Site.EMessageType.Error);
			}
		}

		/// <summary>
		/// Limpia los campos de la interfaz de usuario
		/// </summary>
		public void ClearFields()
		{
            //hdFechaInicio.Value = String.Empty;
            //hdFechaFin.Value = String.Empty;

			GridViewRowCollection rows = grvActividades.Rows;
			foreach (GridViewRow row in rows)
			{
				CheckBox selectedItem = (CheckBox) row.FindControl("cbSeleccionado");
				if (selectedItem.Checked)
				{
					selectedItem.Checked = false;
				}
			}

			Session["ListaSeleccionados"] = null;
		}
		#endregion

		#region Mensajes
		/// <summary>
		/// Método que presenta mensajes error, informacion u otros, en la interfaz de usuario
		/// </summary>
		/// <param name="message"></param>
		/// <param name="messageType"></param>
		private void ShowMessage(string message, Site.EMessageType messageType)
		{
			Site site = (Site) Page.Master;
			site.ShowMessage(message, messageType);
		}
		#endregion

		#region Eventos
		/// <summary>
		/// Método que maneja el cambio de pagina de las actividades
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void grvActividades_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
		{
			try
			{
				KeepSelection((GridView) sender);
				grvActividades.DataSource = Actividades;
				grvActividades.PageIndex = e.NewPageIndex;
				grvActividades.DataBind();

				grvDetalleActividad.DataSource = null;
				grvDetalleActividad.DataBind();
			}
			catch (Exception exception)
			{
				ShowMessage("Error al realizar el páginado los resultados de la consulta " + exception.Message, Content.MasterPages.Site.EMessageType.Error);
			}
		}

		/// <summary>
		/// Método que presenta el cambio de página de las actividades
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void grvActividades_OnPageIndexChanged(object sender, EventArgs e)
		{
			try
			{
				RestoreSelection((GridView) sender);
			}
			catch (Exception exception)
			{
                ShowMessage("Error en el páginado del grid " + exception.Message, Content.MasterPages.Site.EMessageType.Error);
			}
		}

		/// <summary>
		/// Método que ejecuta el botón de detalle en las actividades
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void grvActividades_OnRowCommand(object sender, GridViewCommandEventArgs e)
		{
			try
			{
				if (e.CommandName == "Page") return;

				switch (e.CommandName)
				{
					case "Detalle":
						try
						{

							var actividads = _controller.ConsultarTareasActividad(new ActividadDocente
							{
								ActividadID =
									(Actividades.Where(
										x => x.ActividadID == int.Parse(Convert.ToInt32(e.CommandArgument).ToString(CultureInfo.InvariantCulture)))
										.ToList()).FirstOrDefault().ActividadID
							});

                            ActividadSeleccionada = actividads.FirstOrDefault();

                            var tareas = from t in ActividadSeleccionada.Tareas
											 select new
											 {
												 t.TareaId,
												 t.Nombre,
												 t.Instruccion,
												 Tipo = t.GetTypeDescription()
											 };
							grvDetalleActividad.DataSource = tareas.ToList();
							grvDetalleActividad.DataBind();
							
						}
						catch (Exception exception)
						{
                            ShowMessage("No fue posible presentar el detalle. " + exception.Message, Content.MasterPages.Site.EMessageType.Error);
						}
						break;
				}

			}
			catch (Exception ex)
			{
                ShowMessage(ex.Message, Content.MasterPages.Site.EMessageType.Error);
			}
		}

		/// <summary>
		/// Método que guarda la selección de las actividades cuando se seleccionan
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void grvActividades_OnRowDataBound(object sender, GridViewRowEventArgs e)
		{
			try
			{
				if (e.Row.RowType == DataControlRowType.DataRow)
				{
					var rowViewActividad = (ActividadDocente) e.Row.DataItem;
					var actividadId = rowViewActividad.ActividadID.ToString();
					var cbSelectedCheckBox = (CheckBox) e.Row.FindControl("cbSeleccionado");
					cbSelectedCheckBox.Attributes.Add("ActividadId", actividadId);
				}
			}
			catch (Exception)
			{
                ShowMessage("No fue posible presentar las actividades en la tabla", Content.MasterPages.Site.EMessageType.Error);
			}
		}

		/// <summary>
		/// Método que maneja el cambio de página de las tareas
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void grvDetalleActividad_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
		{
			try
			{
				
                var tareas = from t in ActividadSeleccionada.Tareas
                             select new
                             {
                                 t.TareaId,
                                 t.Nombre,
                                 t.Instruccion,
                                 Tipo = t.GetTypeDescription()
                             };
                grvDetalleActividad.DataSource = tareas.ToList();
				grvDetalleActividad.PageIndex = e.NewPageIndex;
				grvDetalleActividad.DataBind();
			}
			catch (Exception exception)
			{
                ShowMessage("Error al realizar el páginado los resultados de la consulta " + exception.Message, Content.MasterPages.Site.EMessageType.Error);
			}
		}

		/// <summary>
		/// Método que presenta el cambio de página de las tareas
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void grvDetalleActividad_OnPageIndexChanged(object sender, EventArgs e)
		{
			
		}

		/// <summary>
		/// Método que Asigna actividades a los alumnos seleccionados
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void btnAsignar_OnClick(object sender, EventArgs e)
		{
			try
			{
				KeepSelection(grvActividades);

				if (Session["ListaSeleccionados"] != null)
				{
					var mensajeError = ValidateFields();

					if (mensajeError.Equals(String.Empty))
					{
						var msjError = ValidateDate();

						if (msjError.Equals(String.Empty))
						{
							DoInsert();
							ClearFields();

							grvPreAsignaciones.DataSource = ActividadesPreAsignadas;
							grvPreAsignaciones.DataBind();
						}
						else
						{
							ShowMessage(msjError, Content.MasterPages.Site.EMessageType.Information);
						}
					}
					else
					{
                        ShowMessage(mensajeError, Content.MasterPages.Site.EMessageType.Information);
					}
				}
				else
				{
                    ShowMessage("No se han seleccionado actividades a asignar.", Content.MasterPages.Site.EMessageType.Information);
				}
			}
			catch (Exception exception)
			{
                ShowMessage(exception.Message, Content.MasterPages.Site.EMessageType.Information);
			}

		}

		/// <summary>
		/// Método que maneja el cambio de página de las asignaciones
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void grvPreAsignaciones_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
		{
			try
			{

				grvPreAsignaciones.DataSource = ActividadesPreAsignadas;
				grvPreAsignaciones.PageIndex = e.NewPageIndex;
				grvPreAsignaciones.DataBind();
			}
			catch (Exception exception)
			{
                ShowMessage("Error al realizar el páginado los resultados de la consulta " + exception.Message, Content.MasterPages.Site.EMessageType.Error);
			}
		}

		/// <summary>
		/// Método que presenta el cambio d página de las asiganciones
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void grvPreAsignaciones_OnPageIndexChanged(object sender, EventArgs e)
		{

		}

		/// <summary>
		/// Método que maneja la eliminación de las asignaciones de las actividades
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void grvPreAsignaciones_OnRowCommand(object sender, GridViewCommandEventArgs e)
		{
			try
			{
				if (e.CommandName == "Page") return;

				switch (e.CommandName)
				{
					case "Eliminar":
						try
						{
							var asignacion =
								(ActividadesPreAsignadas.Where(
									x =>
										x.AsignacionActividadId ==
										int.Parse(Convert.ToInt32(e.CommandArgument).ToString(CultureInfo.InvariantCulture)))
									.ToList()).FirstOrDefault().AsignacionActividadId;

							var asignacionActividad =
								_controller.ConsultarAsignacionActividades(new AsignacionActividad
								{
									AsignacionActividadId = asignacion
								}, true).FirstOrDefault();

							_controller.DeleteAsignacionActividad(asignacionActividad);

							ActividadesPreAsignadas.Remove(ActividadesPreAsignadas.First(x => x.AsignacionActividadId == asignacion));

							grvPreAsignaciones.DataSource = ActividadesPreAsignadas;
							grvPreAsignaciones.DataBind();

						}
						catch (Exception exception)
						{
                            ShowMessage("No fue posible actualizar la lista" + exception.Message, Content.MasterPages.Site.EMessageType.Error);
						}
						break;
				}

			}
			catch (Exception exception)
			{
                ShowMessage("Error: " + exception.Message, Content.MasterPages.Site.EMessageType.Error);
			}
		}

		/// <summary>
		/// Método que regresa a la selección de alumnos para asignarles tareas
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void btnAgregarMas_OnClick(object sender, EventArgs e)
		{
			try
			{
				Response.Redirect("~/Pages/Actividades/ConsultarAlumnosUI.aspx");

				ActividadesPreAsignadas = null;
				Actividades = null;
				_controller = null;
				Session["ListaSeleccionados"] = null;
			}
			catch (Exception ex)
			{
                ShowMessage(ex.Message, Content.MasterPages.Site.EMessageType.Information);
			}

		}

		/// <summary>
		/// Método que cancela la nueva asignación que se esté realizando, limpiando los campos de seleccion y redirigiendose a la pantalla de Consulta
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void btnCancelar_OnClick(object sender, EventArgs e)
		{
			ClearFields();
		}
		#endregion

		protected override void AuthorizeUser()
		{
			
		}
	}
}