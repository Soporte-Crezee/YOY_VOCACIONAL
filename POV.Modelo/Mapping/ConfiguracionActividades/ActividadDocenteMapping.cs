using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using POV.ConfiguracionActividades.BO;

namespace POV.Modelo.Mapping.ConfiguracionActividades
{
    internal class ActividadDocenteMapping : EntityTypeConfiguration<ActividadDocente>
    {
        public ActividadDocenteMapping()
        {
            ToTable("Actividades");
            HasKey(a => a.ActividadID);
            Property(a => a.ActividadID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(a => a.Nombre).HasMaxLength(100).IsRequired();
            Property(a => a.Descripcion).HasMaxLength(500).IsRequired();
            Property(a => a.Activo).IsRequired();
            Property(a => a.FechaCreacion).IsRequired();
            Property(a => a.Version).IsRowVersion().HasColumnName("RowVersion");
            HasRequired(a => a.Escuela).WithMany().HasForeignKey(a => a.EscuelaId);
            HasMany(a => a.Tareas).WithRequired().Map(m => m.MapKey("ActividadId"));
            HasMany(a => a.ClasificadoresResultados).WithRequired().Map(m => m.MapKey("ActividadId"));
            HasOptional(c => c.BloqueActividad).WithMany().HasForeignKey(a => a.BloqueActividadId);
            HasRequired(t => t.Docente).WithMany().HasForeignKey(t => t.DocenteId);
            HasRequired(t => t.Usuario).WithMany().HasForeignKey(t => t.UsuarioId);
        }
    }
}
