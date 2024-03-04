using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using POV.CentroEducativo.BO;

namespace POV.Modelo.Mapping.CentroEducativo
{
    public class DocenteMapping : EntityTypeConfiguration<Docente>
    {
        public DocenteMapping()
        {
            ToTable("Docente");
            HasKey(x => x.DocenteID);
            Property(x => x.DocenteID).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Curp).IsRequired().HasMaxLength(18);
            Property(x => x.Nombre).IsRequired().HasMaxLength(80);
            Property(x => x.PrimerApellido).IsRequired().HasMaxLength(50);
            Property(x => x.SegundoApellido).IsOptional().HasMaxLength(50);
            Property(x => x.Correo).IsOptional().HasMaxLength(100);
            Property(x => x.FechaNacimiento).IsRequired();
            Property(x => x.Estatus).IsRequired();
            Property(x => x.FechaRegistro).IsRequired();
            Property(x => x.Sexo).IsRequired();
            Property(x => x.EstatusIdentificacion).IsOptional();
            Property(x => x.PermiteAsignaciones).IsRequired();
            //HasMany(o => o.Alumnos).WithRequired(o => o.Docente);
            
            Property(x => x.Titulo).IsOptional().HasMaxLength(100);
            Property(x => x.Cedula).IsOptional();
            Property(x => x.NivelEstudio).IsOptional();
            Property(x => x.Especialidades).IsOptional();
            Property(x => x.Experiencia).IsOptional();
            Property(x => x.Cursos).IsOptional();
            Property(x => x.UsuarioSkype).IsOptional().HasMaxLength(100);
            Property(x => x.EsPremium).IsRequired();
            
            Ignore(x => x.Clave);
            Ignore(x => x.NombreUsuario);
            Ignore(x => x.NombreCompletoDocente);

            HasMany(x => x.Sesiones).WithRequired(x => x.Docente);
        }
    }
}
 
