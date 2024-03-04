using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Framework.Base.DataAccess;
using POV.ConfiguracionActividades.BO;
using System.Globalization;
using POV.Expediente.BO;
using POV.Modelo.BO;
using POV.Prueba.BO;
using POV.Prueba.Calificaciones.BO; 
using POV.Prueba.Diagnostico.Service;

namespace POV.Web.Pages.Actividades
{
	public partial class ProgresoActividadUC : System.Web.UI.UserControl
	{
		#region Propiedades

		DataTable _resultados
		{
			get
			{
				return Session["ResultadosActividades"] as DataTable;
			}
			set
			{
				Session["ResultadosActividades"] = value;
			}
		}

		List<AsignacionActividad> _listAsignaciones
		{
			get
			{
				return Session["ListAsignaciones"] as List<AsignacionActividad>;
			}
			set
			{
				Session["ListAsignaciones"] = value;
			}
		}

        IDataContext dataContext = new DataContext((new DataProviderFactory()).GetDataProvider("POV"));
		#endregion

		#region Eventos
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				lblErrorBusqueda.Text = "";
				txtFechaFin.Text = "";
				txtFechaInicio.Text = "";
				_listAsignaciones = null;
				_resultados = null;
			}
		}
		#endregion

		#region Métodos

		public void DataToUserInterface(List<AsignacionActividad> asignaciones)
		{
			txtFechaFin.Text = "";
			txtFechaInicio.Text = "";
			lblErrorBusqueda.Text = "";
			_listAsignaciones = asignaciones;

			LlenarGridActividadesAsignadas(asignaciones);

		}

		private void LlenarGridActividadesAsignadas(List<AsignacionActividad> asignaciones)
		{
			const string nombre = "Nombre",
				inicioVigencia = "InicioVigencia",
				finVigencia = "FinVigencia",
				porcentajeAvance = "PorcentajeAvance",
				resultadoPrueba = "ResultadoPrueba", asignacionId = "AsignacionActividadId";
			DataTable dtResultados = new DataTable();
			dtResultados.Columns.Add(asignacionId);
			dtResultados.Columns.Add(nombre);
			dtResultados.Columns.Add(inicioVigencia);
			dtResultados.Columns.Add(finVigencia);
			dtResultados.Columns.Add(porcentajeAvance);
			dtResultados.Columns.Add(resultadoPrueba);
			foreach (AsignacionActividad asignacion in asignaciones)
			{
				DataRow drResultado = dtResultados.NewRow();
				drResultado[asignacionId] = asignacion.AsignacionActividadId.ToString();
				drResultado[nombre] = asignacion.Actividad.Nombre;
				drResultado[inicioVigencia] = ((DateTime) asignacion.FechaInicio).ToShortDateString();
				drResultado[finVigencia] = ((DateTime) asignacion.FechaFin).ToShortDateString();
				int totalTareas = asignacion.TareasRealizadas.Count;
				int totalTerminadas = asignacion.TareasRealizadas.Count(t => t.Estatus == EEstatusTarea.Finalizado);
				int porcentaje = totalTareas <= 0 ? 0 : (totalTerminadas * 100 / totalTareas);
				drResultado[porcentajeAvance] = string.Format("{0} %", porcentaje);

                dtResultados.Rows.Add(drResultado);
			}
			_resultados = dtResultados;

			gvActividades.DataSource = _resultados;
			gvActividades.DataBind();
		}
		#endregion

		protected void btnBuscarActividades_OnClick(object sender, EventArgs e)
		{
			lblErrorBusqueda.Text = "";
			string sError = string.Empty;
			DateTime fechaInicio = DateTime.Now;
			DateTime fechaFin = DateTime.Now;
			//si son vacias se consulta todas
			if (txtFechaInicio.Text.Trim().Length == 0 || txtFechaFin.Text.Trim().Length == 0)
			{
				LlenarGridActividadesAsignadas(_listAsignaciones);
			} //si se puede parsear correctamente se filtra con base a las fechas
			else if (DateTime.TryParseExact(txtFechaInicio.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out fechaInicio) &&
				DateTime.TryParseExact(txtFechaFin.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out fechaFin))
			{

				fechaInicio = DateTime.ParseExact(txtFechaInicio.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
				fechaFin = DateTime.ParseExact(txtFechaFin.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
				if (fechaInicio > fechaFin)
				{
					lblErrorBusqueda.Text = "El parámetro Fecha de inicio no puede ser mayor al de Fecha final";
					return;

				}

				List<AsignacionActividad> asignaciones = _listAsignaciones.Where(a => a.FechaInicio >= fechaInicio && a.FechaFin <= fechaFin).ToList();

				LlenarGridActividadesAsignadas(asignaciones);
			}
			else
			{
				lblErrorBusqueda.Text = "El formato de fecha esperado debe ser: dia/mes/año, por ejemplo 25/03/1990";
			}



		}
		protected void gvActividades_RowCommand(object sender, GridViewCommandEventArgs e)
		{
			long asignacionId = Convert.ToInt64(e.CommandArgument);

			AsignacionActividad asignacion = _listAsignaciones.FirstOrDefault(t => t.AsignacionActividadId == asignacionId);
			if (asignacion != null)
			{
				UCProgresoTareas.DataToUserInterface(asignacion.TareasRealizadas);
				UCProgresoTareas.SetTitle(asignacion.Actividad.Nombre);
				UCProgresoTareas.SetVigenciaActividad((DateTime) asignacion.FechaInicio, (DateTime) asignacion.FechaFin);
				hdnDialogo.Value = "1";
			}
		}

		protected void gvActividades_PageIndexChanging(object sender, GridViewPageEventArgs e)
		{

			gvActividades.DataSource = _resultados;
			gvActividades.PageIndex = e.NewPageIndex;
			gvActividades.DataBind();
		}
	}
}