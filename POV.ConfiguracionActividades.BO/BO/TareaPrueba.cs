using POV.Prueba.BO;

namespace POV.ConfiguracionActividades.BO
{
	public class TareaPrueba : Tarea
	{
		virtual public APrueba Prueba
		{
			get;
			set;
		}

		public int? PruebaId
		{
			get;
			set;
		}
		public override string GetIdentificador()
		{
			return Prueba.PruebaID.ToString();
		}

		public override string GetUrl()
		{
			return string.Empty;
		}

	    public override string GetTypeDescription()
	    {
            return "Prueba";
	    }
	}
}
