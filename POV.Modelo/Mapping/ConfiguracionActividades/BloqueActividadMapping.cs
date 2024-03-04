using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using POV.ConfiguracionActividades.BO;

namespace POV.Modelo.Mapping.ConfiguracionActividades
{
    public class BloqueActividadMapping : EntityTypeConfiguration<BloqueActividad>
    {
        public BloqueActividadMapping()
        {
            ToTable("BloqueActividad");
            HasKey(a => a.BloqueActividadId);
            Property(a => a.BloqueActividadId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(a => a.Nombre).IsRequired().HasMaxLength(200);
            Property(a => a.FechaInicio).IsRequired();
            Property(a => a.FechaFin).IsRequired();
            Property(a => a.FechaRegistro).IsRequired();
            Property(a => a.Activo).IsRequired();
            Property(a => a.Version).IsRowVersion().HasColumnName("RowVersion");

            HasRequired(a => a.Escuela).WithMany().HasForeignKey(a => a.EscuelaId);
            HasRequired(a => a.CicloEscolar).WithMany().HasForeignKey(a => a.CicloEscolarId);
        }
    }
}
