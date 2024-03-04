using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using POV.ConfiguracionesPlataforma.BO;

namespace POV.Modelo.Mapping.ConfiguracionPlataforma
{
    class PlantillaLudicaMapping: EntityTypeConfiguration<PlantillaLudica>
    {
        public PlantillaLudicaMapping()
        {
            ToTable("PlantillaLudica");
            HasKey(x => x.PlantillaLudicaId);
            Property(x => x.PlantillaLudicaId).
                IsRequired().
                HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(x => x.ImagenFondo).IsRequired();
            Property(x => x.ImagenPendiente).IsRequired();
            Property(x => x.ImagenIniciado).IsRequired();
            Property(x => x.ImagenFinalizado).IsRequired();
            Property(x => x.ImagenNoDisponible).IsRequired();
            Property(x => x.ImagenFlechaArriba).IsRequired();
            Property(x => x.ImagenFlechaAbajo).IsRequired();
            Property(x => x.EsPredeterminado).IsRequired();
            Property(x => x.FechaRegistro).IsRequired();
            Property(x => x.Activo).IsRequired();

            HasMany(x => x.PosicionesActividades)
                .WithRequired().Map(m => m.MapKey("PlantillaLudicaId"));

            Property(x => x.Version)
                    .HasColumnName("RowVersion")
                    .IsRowVersion();
        }
    }
}
