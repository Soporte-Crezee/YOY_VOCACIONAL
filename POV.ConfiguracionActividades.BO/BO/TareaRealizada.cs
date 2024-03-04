using System;

namespace POV.ConfiguracionActividades.BO
{
	public class TareaRealizada
	{
		public long? Acumulado { get; set; }
		public EEstatusTarea? Estatus { get; set; }
		public DateTime? FechaFin { get; set; }
        public DateTime? FechaInicio { get; set; }
		virtual public Tarea Tarea { get; set; }
		public long? TareaId { get; set; }
	    public long? TareaRealizadaId { get; set; }
        public byte[] Version { get; set; }
        public int? ResultadoPruebaId { get; set; }
	}
}
