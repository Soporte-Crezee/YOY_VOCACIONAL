using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using POV.ConfiguracionActividades.Queries;
using POV.Modelo.Context;
using POV.Prueba.BO;
using POV.Modelo.BO;

namespace POV.ConfiguracionActividades.Services
{
	public class PruebaCtrl : IDisposable
	{
		/// <summary>
		/// Contexto interno de conexión a la base de datos
		/// </summary>
		private readonly Contexto _model;

		/// <summary>
		/// Firma de la conexión a la base de datos
		/// </summary>
		private readonly object _sing;

		/// <summary>
		/// Constructor encargado de crear la conexion interna a la base de datos
		/// </summary>
		/// <param name="contexto">Contexto de conexion a la base de datos</param>
		public PruebaCtrl(Contexto contexto)
		{
			_sing = new object();
			_model = contexto ?? new Contexto(_sing);
		}


		/// <summary>
		/// Consulta la actividades que existen en el sistema
		/// </summary>
		/// <param name="criteria">APrueba que provee el criterio de búsqueda</param>
		/// <param name="tracking">Permite saber si se rastrearán los objetos que se obtengan de la consulta</param>
		/// <returns>Actividades que satisfacen el criterio de búsqueda</returns>
		public List<APrueba> Retrieve(APrueba criteria, bool tracking)
		{
			DbQuery<APrueba> qryPruebas = (tracking) ? _model.Pruebas : _model.Pruebas.AsNoTracking();

			return qryPruebas.Where(new PruebaQry(criteria).Action()).Include(x => x.Modelo)
								.ToList();
		}

		/// <summary>
		/// Método para cerrar la conexión del controlador con la base de datos
		/// </summary>
		public void Dispose()
		{
			_model.Disposing(_sing);
		}
	}
}
