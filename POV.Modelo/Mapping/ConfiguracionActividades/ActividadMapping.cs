using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using POV.ConfiguracionActividades.BO;

namespace POV.Modelo.Mapping.ConfiguracionActividades
{
	internal class ActividadMapping:EntityTypeConfiguration<Actividad>
	{
		public ActividadMapping()
		{
			ToTable("Actividades");
			HasKey(a => a.ActividadID);
			Property(a => a.ActividadID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
			Property(a => a.Nombre).HasMaxLength(100).IsRequired();
			Property(a => a.Descripcion).HasMaxLength(500).IsRequired();
			Property(a => a.Activo).IsRequired();
			Property(a => a.FechaCreacion).IsRequired();
			Property(a => a.Version).IsRowVersion().HasColumnName("RowVersion");
            //Property(a => a.DocenteId).IsOptional();
            //Property(a => a.UsuarioId).IsOptional();
			HasRequired(a => a.Escuela).WithMany().HasForeignKey(a => a.EscuelaId);
            HasOptional(a => a.Clasificador).WithMany().HasForeignKey(a => a.ClasificadorID);
			HasMany(a => a.Tareas).WithRequired().Map(m => m.MapKey("ActividadId"));
            HasMany(a => a.ClasificadoresResultados).WithRequired().Map(m => m.MapKey("ActividadId"));
            HasOptional(c => c.BloqueActividad).WithMany().HasForeignKey(a => a.BloqueActividadId);
		}
	}
}
