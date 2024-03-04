using System;
using POV.Core.RedSocial.Implement;
using POV.Reactivos.BO;

namespace POV.ConfiguracionActividades.BO
{
	public class TareaReactivo:Tarea
	{
		private Reactivo reactivo;

		virtual public Reactivo Reactivo
		{
			get { return reactivo; }
			set { reactivo = value; }
		}

		public Guid? ReactivoId
		{
			get;
			set;
		}

		public override string GetIdentificador()
		{
			return Reactivo != null && Reactivo.ReactivoID != null ? Reactivo.ReactivoID.ToString() : null;
		}
		
		public override string GetUrl()
		{
			return UrlHelper.GetMuroReactivoURL(GetIdentificador());
		}

	    public override string GetTypeDescription()
	    {
            return "Ejercicio de práctica";
	    }
	}
}
