using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using POV.ConfiguracionActividades.BO;

namespace POV.Modelo.Mapping.ConfiguracionActividades
{
	class TareaRealizadaMapping : EntityTypeConfiguration<TareaRealizada>
	{
		public TareaRealizadaMapping()
		{
			ToTable("TareasRealizadas");
			HasKey(a => a.TareaRealizadaId);
			Property(a => a.TareaRealizadaId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
			Property(a => a.FechaInicio).IsOptional();
            Property(a => a.FechaFin).IsOptional();
			Property(a => a.Estatus).IsRequired().HasColumnName("EstatusTarea");
			Property(a => a.Acumulado).IsOptional();
            Property(a => a.ResultadoPruebaId).IsOptional();

			HasRequired(a => a.Tarea).WithMany().HasForeignKey(a => a.TareaId);
		   
            Property(a => a.Version).IsRowVersion().HasColumnName("RowVersion");

		}
	}
}
