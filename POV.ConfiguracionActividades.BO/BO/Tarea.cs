using System;

namespace POV.ConfiguracionActividades.BO
{
	public abstract class Tarea
	{
		private long? tareaId;
		private string nombre;
		private string instruccion;
		private byte[] version;

		public long? TareaId
		{
			get { return tareaId; }
			set { tareaId = value; }
		}

		public string Nombre
		{
			get { return nombre; }
			set { nombre = value; }
		}

		public string Instruccion
		{
			get { return instruccion; }
			set { instruccion = value; }
		}
		public byte[] Version
		{
			get { return version; }
			set { version = value; }
		}

	    public abstract String GetTypeDescription();

		abstract public string  GetIdentificador();

		public abstract String GetUrl();

	}
}
