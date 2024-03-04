using System.Data.Entity.ModelConfiguration;
using POV.ConfiguracionActividades.BO;

namespace POV.Modelo.Mapping.ConfiguracionActividades
{
	public class TareaReactivoMapping:EntityTypeConfiguration<TareaReactivo>
	{
		public TareaReactivoMapping()
		{

			HasRequired(t => t.Reactivo).WithMany().HasForeignKey(t => t.ReactivoId);
			
		}
	}
}
