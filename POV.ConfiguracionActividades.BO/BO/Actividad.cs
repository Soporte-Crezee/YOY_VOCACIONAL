using System;
using System.Collections.Generic;
using POV.CentroEducativo.BO;
using POV.Modelo.BO;

namespace POV.ConfiguracionActividades.BO
{
	public class Actividad
	{
		public long? ActividadID { get; set; }

		public bool? Activo { get; set; }

		public string Descripcion { get; set; }

		public DateTime? FechaCreacion { get; set; }

		public string Nombre { get; set; }

		public virtual List<Tarea> Tareas { get; set; }

		public byte[] Version { get; set; }

		public virtual Escuela Escuela { get; set; }

		public int? EscuelaId { get; set; }

        public virtual BloqueActividad BloqueActividad { get; set; }

        public int? BloqueActividadId { get; set; }

        public virtual List<ClasificadorResultado> ClasificadoresResultados { get; set; }

        public virtual Clasificador Clasificador { get; set; }

        public Int32? ClasificadorID { get; set; }
	}
}
