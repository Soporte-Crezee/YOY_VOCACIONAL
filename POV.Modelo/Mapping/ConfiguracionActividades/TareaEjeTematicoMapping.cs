using System.Data.Entity.ModelConfiguration;
using POV.ConfiguracionActividades.BO;

namespace POV.Modelo.Mapping.ConfiguracionActividades
{
	public class TareaEjeTematicoMapping : EntityTypeConfiguration<TareaEjeTematico>
	{
		public TareaEjeTematicoMapping()
		{

			HasRequired(t => t.EjeTematico).WithMany().HasForeignKey(t => t.EjeTematicoId);
			HasRequired(t => t.ContenidoDigital).WithMany().HasForeignKey(t => t.ContenidoDigitalId);

		}
	}
}
