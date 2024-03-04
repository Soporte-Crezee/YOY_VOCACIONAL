using POV.Prueba.BO;

namespace POV.ConfiguracionActividades.BO
{
	public class PruebaProxy: APrueba
	{
		private ETipoPrueba _tipoPrueba;
		public override ETipoPrueba TipoPrueba
		{
			get { return _tipoPrueba; }
		}

		public void SetTipoPrueba(ETipoPrueba tipoPrueba)
		{
			_tipoPrueba = tipoPrueba;
		}
	}
}
