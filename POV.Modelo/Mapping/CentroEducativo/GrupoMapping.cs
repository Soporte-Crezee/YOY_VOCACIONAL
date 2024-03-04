using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using POV.CentroEducativo.BO;

namespace POV.Modelo.Mapping.CentroEducativo
{
    public class GrupoMapping:EntityTypeConfiguration<Grupo>
    {
        public GrupoMapping()
        {
            ToTable("Grupo");
            HasKey(x => x.GrupoID);
            Property(x => x.GrupoID)
                .IsRequired()
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(x => x.Nombre)
                .IsRequired()
                .HasMaxLength(20);

            Property(x => x.Grado)
                .IsRequired();

        }
    }
}
