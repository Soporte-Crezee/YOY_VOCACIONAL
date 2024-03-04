using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using POV.ConfiguracionActividades.BO;

namespace POV.Modelo.Mapping.ConfiguracionActividades
{
	public class AsignacionActividadMapping : EntityTypeConfiguration<AsignacionActividad>
	{
		public AsignacionActividadMapping()
		{
			
			ToTable("AsignacionesActividades");
			HasKey(a => a.AsignacionActividadId);
			Property(a => a.AsignacionActividadId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(a => a.FechaInicio).IsOptional();
            Property(a => a.FechaFin).IsOptional();
            Property(a => a.FechaCreacion).IsRequired();
            Property(a => a.EsManual).IsRequired();
            Property(a => a.AsignadoPor).IsOptional();
			Property(a => a.Version).IsRowVersion().HasColumnName("RowVersion");

			HasRequired(a => a.Actividad).WithMany().HasForeignKey(a => a.ActividadId);
			HasRequired(a => a.Alumno).WithMany().HasForeignKey(a => a.AlumnoId);
		    HasMany(a => a.TareasRealizadas)
		        .WithMany()
		        .Map(
		            t =>
		                t.MapLeftKey(new string[] {"AsignacionActividadId"})
		                    .MapRightKey(new string[] {"TareaRealizadaId"})
		                    .ToTable("AsignacionTareaRealizada"));


		}
	}
}
