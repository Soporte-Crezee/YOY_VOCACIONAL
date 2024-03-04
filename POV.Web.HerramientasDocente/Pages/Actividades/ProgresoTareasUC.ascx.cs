using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Framework.Base.DataAccess;
using POV.ConfiguracionActividades.BO;
using POV.Prueba.BO;
using POV.Prueba.Calificaciones.BO;
using POV.Prueba.Diagnostico.Service;

namespace POV.Web.Pages.Actividades
{
	public partial class ProgresoTareasUC : System.Web.UI.UserControl
	{

		#region Propiedades

		DataTable _resultados
		{
			get
			{
				return Session["ResultadosTareaRealizada"] as DataTable;
			}
			set
			{
				Session["ResultadosTareaRealizada"] = value;
			}
		}
        IDataContext dataContext = new DataContext((new DataProviderFactory()).GetDataProvider("POV"));
		#endregion

		#region eventos
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack) _resultados = null;
		}
		#endregion

		#region metodos

		public void DataToUserInterface(List<TareaRealizada> tareas)
		{
			const string nombre = "Nombre", tipo = "Tipo", estatus = "Estatus", fechaInicio = "FechaInicio",
						fechaFin = "FechaFin", resultadoPrueba = "ResultadoPrueba";
			DataTable dtTareas = new DataTable();
			dtTareas.Columns.Add(nombre);
			dtTareas.Columns.Add(tipo);
			dtTareas.Columns.Add(estatus);
			dtTareas.Columns.Add(fechaInicio);
			dtTareas.Columns.Add(fechaFin);
            dtTareas.Columns.Add(resultadoPrueba);

			foreach (TareaRealizada realizada in tareas)
			{
				DataRow dr = dtTareas.NewRow();
				dr[nombre] = realizada.Tarea.Nombre;
                if (realizada.Tarea.GetTypeDescription() == "Reactivo")
                {
                    dr[tipo] = "Ejercicio de práctica";
                }
                else
				dr[tipo] = realizada.Tarea.GetTypeDescription();

				dr[estatus] = realizada.Estatus.ToString().Replace("_", " ");
				dr[fechaInicio] = realizada.FechaInicio != null ? ((DateTime) realizada.FechaInicio).ToShortDateString() : " ";
				dr[fechaFin] = realizada.FechaFin != null ? ((DateTime) realizada.FechaFin).ToShortDateString() : " ";
                
				dtTareas.Rows.Add(dr);
			}

			_resultados = dtTareas;
			gvTareas.DataSource = dtTareas;
			gvTareas.DataBind();
			


		}

		public void SetTitle(string titulo)
		{
			lblTitulo.Text = string.Format("Avance {0}", titulo);
		}

		public void SetVigenciaActividad(DateTime fechaInicio, DateTime fechaFin)
		{
			lblVigenciaInicio.Text = string.Format("Inicio Vigencia: {0}", fechaInicio.ToShortDateString());
			lblVIgenciaFin.Text = string.Format("Fin Vigencia: {0}", fechaFin.ToShortDateString());
		}

		#endregion

		protected void gvTareas_PageIndexChanging(object sender, GridViewPageEventArgs e)
		{
			gvTareas.DataSource = _resultados;
			gvTareas.PageIndex = e.NewPageIndex;
			gvTareas.DataBind();
			
		}
	}
}