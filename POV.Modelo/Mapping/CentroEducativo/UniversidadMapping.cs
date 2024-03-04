using POV.CentroEducativo.BO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POV.Modelo.Mapping.CentroEducativo
{
    class UniversidadMapping : EntityTypeConfiguration<Universidad>
    {
        public UniversidadMapping()
        {
            ToTable("Universidad");
            HasKey(x => x.UniversidadID);
            Property(x => x.UniversidadID).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.NombreUniversidad).IsRequired().HasMaxLength(250);
            Property(x => x.Descripcion).IsOptional().HasMaxLength(254);
            Property(x => x.Direccion).IsRequired().HasMaxLength(500);
            HasRequired(x => x.Ubicacion).WithMany().HasForeignKey(x => x.UbicacionID);
            Property(x => x.PaginaWEB).IsOptional().HasMaxLength(500);
            Property(x => x.CoordinadorCarrera).IsOptional().HasMaxLength(100);
            Property(x => x.Activo).IsRequired();
            Property(x => x.ClaveEscolar).IsOptional().HasMaxLength(50);
            Property(x => x.Siglas).IsOptional().HasMaxLength(15);
            Property(x => x.NivelEscolar).IsOptional();
            HasMany(u => u.Carreras).
                WithMany(c => c.Universidades).
                Map(m =>
                {
                    m.ToTable("UniversidadCarrera");
                    m.MapLeftKey("UniversidadID");
                    m.MapRightKey("CarreraID");
                });
            HasMany(u => u.Docentes).
                WithMany(c => c.Universidades).
                Map(m =>
                {
                    m.ToTable("UniversidadDocente");
                    m.MapLeftKey("UniversidadID");
                    m.MapRightKey("DocenteID");
                });
            HasMany(u => u.Alumnos).
                WithMany(a => a.Universidades).
                Map(m =>
                {
                    m.ToTable("UniversidadAlumno");
                    m.MapLeftKey("UniversidadID");
                    m.MapRightKey("AlumnoID");
                });
            HasMany(o => o.EventosUniversidad).WithRequired(o => o.Universidad);
        }
    }
}
