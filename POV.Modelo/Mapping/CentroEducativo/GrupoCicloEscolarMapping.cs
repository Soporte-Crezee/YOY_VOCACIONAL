using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using POV.CentroEducativo.BO;

namespace POV.Modelo.Mapping.CentroEducativo
{
    public class GrupoCicloEscolarMapping: EntityTypeConfiguration<GrupoCicloEscolar>
    {
        public GrupoCicloEscolarMapping()
        {
            ToTable("GrupoCicloEscolar");
            HasKey(x => x.GrupoCicloEscolarID);
            Property(x => x.GrupoCicloEscolarID)
                .IsRequired()
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(x => x.Clave)
                .IsRequired()
                .HasMaxLength(10);

            Property(x => x.Activo)
                .IsRequired();

            HasRequired(x => x.Grupo).WithMany().Map(m => m.MapKey("GrupoID"));
            HasRequired(x => x.Escuela).WithMany().Map(m => m.MapKey("EscuelaID"));
            
            Ignore(x => x.AsignacionAlumnos);
            Ignore(x => x.AsignacionMaterias);
            Ignore(x => x.CicloEscolar);
        }
    }
}
