using System.Data.Entity.ModelConfiguration;
using POV.ConfiguracionActividades.BO;

namespace POV.Modelo.Mapping.ConfiguracionActividades
{
	public class TareaPruebaMapping:EntityTypeConfiguration<TareaPrueba>
	{
		public TareaPruebaMapping()
		{

			HasRequired(t => t.Prueba).WithMany().HasForeignKey(t => t.PruebaId);
			
		}
	}
}
