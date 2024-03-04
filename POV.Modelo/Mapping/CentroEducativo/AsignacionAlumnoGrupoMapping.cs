using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using POV.CentroEducativo.BO;

namespace POV.Modelo.Mapping.CentroEducativo
{
    public class AsignacionAlumnoGrupoMapping: EntityTypeConfiguration<AsignacionAlumnoGrupo>
    {
        public AsignacionAlumnoGrupoMapping()
        {
            ToTable("AsignacionAlumnoGrupo");
            HasKey(x => x.AsignacionAlumnoGrupoID);
            Property(x => x.AsignacionAlumnoGrupoID)
                .IsRequired()
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(x => x.FechaRegistro)
                .IsRequired();

            Property(x => x.FechaBaja)
                .IsOptional();

            Property(x => x.Activo)
                .IsRequired();

            HasRequired(x => x.Alumno).WithMany().Map(m=>m.MapKey("AlumnoID"));
        }
    }
}
