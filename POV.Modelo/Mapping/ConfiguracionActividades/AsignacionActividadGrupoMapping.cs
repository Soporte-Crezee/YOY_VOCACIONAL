using System.ComponentModel.DataAnnotations.Schema;
using POV.ConfiguracionActividades.BO;
using System.Data.Entity.ModelConfiguration;

namespace POV.Modelo.Mapping.ConfiguracionActividades
{
    public class AsignacionActividadGrupoMapping : EntityTypeConfiguration<AsignacionActividadGrupo>
    {
        public AsignacionActividadGrupoMapping()
        {
            ToTable("AsignacionActividadGrupo");
            HasKey(a => a.AsignacionActividadGrupoID);
            Property(a => a.AsignacionActividadGrupoID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(a => a.FechaInicio).IsRequired();
            Property(a => a.FechaFin).IsRequired();
            Property(a => a.FechaCreacion).IsRequired();

            HasRequired(a => a.GrupoCicloEscolar).WithMany().HasForeignKey(a => a.GrupoCicloEscolarID);
            HasRequired(a => a.ActividadDocente).WithMany().HasForeignKey(a => a.ActividadID);
            HasMany(a => a.AsignacionesActividades)
                .WithMany()
                .Map(
                    t =>
                        t.MapLeftKey(new string[] { "AsignacionActividadGrupoID" })
                            .MapRightKey(new string[] { "AsignacionActividadID" })
                            .ToTable("AsignacionActividadGrupoAsignacionesActividades"));
        }
    }
}
