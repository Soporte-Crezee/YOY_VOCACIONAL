using System;
using System.Linq;
using System.Linq.Expressions;
using Framework.Base.Entity;
using Framework.Base.Entity.Queries;
using POV.Prueba.BO;

namespace POV.ConfiguracionActividades.Queries
{
	internal class PruebaQry : IQuery<APrueba>
	{
		/// <summary>
		/// Propiedad a la que se asignan los criterios de búsqueda
		/// </summary>
		private readonly APrueba _criteria;

		/// <summary>
		/// Constructor no nulo que recibe los criterios de búsqueda
		/// </summary>
		/// <param name="criteria">Prueba que provee los criterios de búsqueda</param>
		public PruebaQry(APrueba criteria)
		{
			_criteria = criteria;
		}

		/// <summary>
		/// Clase implementada del Framework
		/// </summary>
		/// <param name="filters">Filtros del criterio de búsqueda</param>
		/// <returns>Expresión que contendra los filtros de búsqueda</returns>
		public Expression<Func<APrueba, bool>> Action(params Expression<Func<APrueba, bool>>[] filters)
		{
			Expression<Func<APrueba, bool>> exp = x => true;

			if (_criteria == null) return filters.Aggregate(exp, (current, e) => current.And(e));
			if (_criteria.PruebaID != null) exp = exp.And(a => a.PruebaID == _criteria.PruebaID);
			if (_criteria.Clave != null) exp = exp.And(a => a.Clave.Contains(_criteria.Clave));
			if (_criteria.Nombre != null) exp = exp.And(a => a.Nombre.Contains(_criteria.Nombre));
			exp.And(a => a.TipoPrueba == _criteria.TipoPrueba);
			if (_criteria.EstadoLiberacionPrueba != null) exp = exp.And(a => a.EstadoLiberacionPrueba == _criteria.EstadoLiberacionPrueba);

			return exp;

		}
	}
}
