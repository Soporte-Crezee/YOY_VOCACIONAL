using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using POV.CentroEducativo.BO;

namespace POV.Modelo.Mapping.CentroEducativo
{
    public class AlumnoMapping: EntityTypeConfiguration<Alumno>
    {
        public AlumnoMapping()
        {
            ToTable("Alumno");
            HasKey(x => x.AlumnoID);
            Property(x => x.AlumnoID).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Curp).IsOptional().HasMaxLength(18);
            Property(x => x.Matricula).IsOptional().HasMaxLength(50);
            Property(x => x.Nombre).IsRequired().HasMaxLength(80);
            Property(x => x.PrimerApellido).IsRequired().HasMaxLength(50);
            Property(x => x.SegundoApellido).IsOptional().HasMaxLength(50);
            Property(x => x.FechaNacimiento).IsOptional();
            Property(x => x.Direccion).IsOptional().HasMaxLength(50);
            Property(x => x.NombreCompletoTutor).IsOptional().HasMaxLength(50);
            Property(x => x.Estatus).IsRequired();
            Property(x => x.FechaRegistro).IsOptional();
            Property(x => x.Sexo).IsRequired();
            Property(x => x.EstatusIdentificacion).IsOptional();
            Property(x => x.NombreCompletoTutorDos).IsOptional().HasMaxLength(50);
            Property(x => x.CorreoConfirmado).IsRequired();
            Property(x => x.Escuela).IsOptional().HasMaxLength(250);
            Property(x => x.Grado).IsOptional();
            Property(x => x.DatosCompletos).IsOptional();
            Property(x => x.CarreraSeleccionada).IsOptional();
            Property(x => x.Premium).IsOptional();
            Property(x => x.NivelEscolar).IsOptional();
            Property(x => x.EstatusPago).IsOptional();
            Property(x => x.IDReferenciaOXXO).IsOptional();
            Property(x => x.ReferenciaOXXO).IsOptional();
            Property(x => x.NotificacionPago).IsRequired();

            Ignore(x => x.Ubicacion);
            Ignore(x => x.AreasConocimiento);
            Ignore(x => x.NombreCompletoAlumno);

            Property(x => x.Credito).IsOptional();
            Property(x => x.CreditoUsado).IsOptional();
            Property(x => x.Saldo).IsOptional();

            HasMany(x => x.Sesiones).WithRequired(x => x.Alumno);
        }
    }
}
