using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using POV.ConfiguracionesPlataforma.BO;

namespace POV.Modelo.Mapping.ConfiguracionPlataforma
{
    class PosicionActividadMapping : EntityTypeConfiguration<PosicionActividad>
    {
        public PosicionActividadMapping()
        {
            ToTable("PosicionActividad");
            HasKey(a => a.PosicionActividadId);
            Property(a => a.PosicionActividadId).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(a => a.PosicionX).IsRequired();
            Property(a => a.PosicionY).IsRequired();
            Property(a => a.Orden).IsRequired();
            Property(a => a.Version).IsRowVersion().HasColumnName("RowVersion");
        }
    }
}
