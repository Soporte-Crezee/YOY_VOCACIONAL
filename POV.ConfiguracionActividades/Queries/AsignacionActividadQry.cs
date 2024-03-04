using System;
using System.Linq;
using System.Linq.Expressions;
using Framework.Base.Entity;
using Framework.Base.Entity.Queries;
using POV.ConfiguracionActividades.BO;
using POV.CentroEducativo.BO;
namespace POV.ConfiguracionActividades.Queries
{
	internal class AsignacionActividadQry :IQuery<AsignacionActividad>
	{
        /// <summary>
        /// Propiedad a la que se asignan los criterios de búsqueda
        /// </summary>
		private readonly AsignacionActividad _criteria;

        /// <summary>
        /// Constructor no nulo que recibe los criterios de búsqueda
        /// </summary>
        /// <param name="criteria">Asignación de Actividad que provee los criterios de búsqueda</param>
		public AsignacionActividadQry(AsignacionActividad criteria)
		{
			_criteria = criteria ?? new AsignacionActividad();
		}

        /// <summary>
        /// Clase implementada del Framework
        /// </summary>
        /// <param name="filters">Filtros del criterio de búsqueda</param>
        /// <returns>Expresión que contendra los filtros de búsqueda</returns>
		public Expression<Func<AsignacionActividad, bool>> Action(params Expression<Func<AsignacionActividad, bool>>[] filters)
		{
			Expression<Func<AsignacionActividad, bool>> exp = x => true;

			if (_criteria == null) return filters.Aggregate(exp, (current, e) => current.And(e));

			if (_criteria.AsignacionActividadId != null)
				exp = exp.And(a => a.AsignacionActividadId == _criteria.AsignacionActividadId);
				
			if (_criteria.Actividad != null && _criteria.Actividad.ActividadID != null)
				exp = exp.And(a => a.Actividad.ActividadID == _criteria.Actividad.ActividadID);
            else if (_criteria.ActividadId != null)
                exp = exp.And(a => a.ActividadId == _criteria.ActividadId);

			if (_criteria.Alumno != null && _criteria.Alumno.AlumnoID != null)
				exp = exp.And(a => a.Alumno.AlumnoID == _criteria.Alumno.AlumnoID);
            else if (_criteria.AlumnoId != null)
                exp = exp.And(a => a.AlumnoId == _criteria.AlumnoId);

			if (_criteria.FechaCreacion != null)
				exp = exp.And(a => a.FechaCreacion == _criteria.FechaCreacion);

            if (_criteria.FechaFin != null)
                exp = exp.And(a => a.FechaFin == _criteria.FechaFin);

            if (_criteria.FechaInicio != null)
                exp = exp.And(a => a.FechaInicio == _criteria.FechaInicio);

			exp = filters.Aggregate(exp, (current, e) => current.And(e));

            return exp;
		}

        public Expression<Func<AsignacionActividad, bool>> Action(Alumno alumno, ActividadDocente actividad)
        {
            Expression<Func<AsignacionActividad, bool>> exp = x => true;

            if (_criteria != null)
            {
                if (_criteria.AsignacionActividadId != null)
                    exp = exp.And(a => a.AsignacionActividadId == _criteria.AsignacionActividadId);

                if (_criteria.Actividad != null && _criteria.Actividad.ActividadID != null)
                    exp = exp.And(a => a.Actividad.ActividadID == _criteria.Actividad.ActividadID);
                else if (_criteria.ActividadId != null)
                    exp = exp.And(a => a.ActividadId == _criteria.ActividadId);

                if (_criteria.EsManual != null)
                    exp = exp.And(a => a.EsManual == _criteria.EsManual);

            }

            if (actividad != null)
            {
                if (actividad.BloqueActividadId != null)
                    exp = exp.And(a => a.Actividad.BloqueActividadId == actividad.BloqueActividadId);
                if (actividad.BloqueActividad != null && actividad.BloqueActividad.CicloEscolarId != null)
                    exp = exp.And(a => a.Actividad.BloqueActividad.CicloEscolarId == actividad.BloqueActividad.CicloEscolarId);

                if (actividad.EscuelaId != null && actividad.EscuelaId != null)
                    exp = exp.And(a => a.Actividad.EscuelaId == actividad.EscuelaId);
                if (!string.IsNullOrEmpty(actividad.Nombre))
                    exp = exp.And(a => a.Actividad.Nombre.Contains(actividad.Nombre));
                if (actividad.DocenteId != null)
                    exp = exp.And(a => a.Actividad.DocenteId == actividad.DocenteId);
                if (actividad.UsuarioId != null)
                    exp = exp.And(a => a.Actividad.UsuarioId == actividad.UsuarioId);

            }

            if (alumno != null)
            {
                if (alumno.AlumnoID != null)
                    exp = exp.And(a => a.AlumnoId == alumno.AlumnoID);

                string nombreCompleto = string.Empty;

                if (!string.IsNullOrEmpty(alumno.Nombre))
                    nombreCompleto += alumno.Nombre;

                if (!string.IsNullOrWhiteSpace(nombreCompleto.Trim()))
                    //exp = exp.And(a => nombreCompleto.Contains(a.Alumno.Nombre) || nombreCompleto.Contains(a.Alumno.PrimerApellido) || nombreCompleto.Contains(a.Alumno.SegundoApellido));
                    exp = exp.And(a => a.Alumno.Nombre.Contains(nombreCompleto) || a.Alumno.PrimerApellido.Contains(nombreCompleto) || a.Alumno.SegundoApellido.Contains(nombreCompleto));

            }
            return exp;
        }
    }
}
