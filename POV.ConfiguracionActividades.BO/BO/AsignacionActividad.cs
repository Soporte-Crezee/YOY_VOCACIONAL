using System;
using System.Collections.Generic;
using POV.CentroEducativo.BO;

namespace POV.ConfiguracionActividades.BO
{
	public class AsignacionActividad
	{
		public long? AsignacionActividadId { get; set; }
		public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaFin { get; set; }
        public DateTime? FechaInicio { get; set; }
		virtual public ActividadDocente Actividad { get; set; }
		public long? ActividadId { get; set; }
		virtual public List<TareaRealizada> TareasRealizadas { get; set; }
        virtual public Alumno Alumno { get; set; }
        public long? AlumnoId { get; set; }

        public bool? EsManual { get; set; }

		public byte[] Version { get; set; }

		public int? AsignadoPor { get; set; }
	}
}
