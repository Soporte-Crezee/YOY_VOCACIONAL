using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using POV.ConfiguracionActividades.BO;

namespace POV.Modelo.Mapping.ConfiguracionActividades
{
	public class TareaMapping : EntityTypeConfiguration<Tarea>
	{
		public TareaMapping()
		{
			ToTable("Tareas");
			HasKey(t => t.TareaId);
			Property(t => t.TareaId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
			Property(t => t.Instruccion).HasMaxLength(200).IsRequired();
			Property(t => t.Version).IsRowVersion().HasColumnName("RowVersion");

			Map<TareaEjeTematico>(m => m.Requires("Discriminator").HasValue("TareaEjeTematico"));
			Map<TareaPrueba>(m => m.Requires("Discriminator").HasValue("TareaPrueba"));
			Map<TareaReactivo>(m => m.Requires("Discriminator").HasValue("TareaReactivo"));
            Map<TareaContenidoDigital>(m => m.Requires("Discriminator").HasValue("TareaContenidoDigital"));
		}
	}
}
