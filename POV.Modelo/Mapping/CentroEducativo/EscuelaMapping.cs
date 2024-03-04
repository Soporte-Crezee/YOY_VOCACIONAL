using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using POV.CentroEducativo.BO;

namespace POV.Modelo.Mapping.CentroEducativo
{
    public class EscuelaMapping:EntityTypeConfiguration<Escuela>
    {
        public EscuelaMapping()
        {
            ToTable("Escuela");
            HasKey(x => x.EscuelaID);
            Property(x => x.EscuelaID)
                .IsRequired()
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(x => x.Clave)
                .IsRequired();

            Property(x => x.NombreEscuela)
                .IsRequired();

            Property(x => x.Estatus)
                .IsRequired();

            Property(x => x.FechaRegistro)
                .IsRequired();

            Ignore(x => x.Ubicacion);
            Ignore(x => x.Turno);
            Ignore(x => x.ToShortTurno);
            Ignore(x => x.Ambito);
            Ignore(x => x.ToShortAmbito);
            Ignore(x => x.TipoServicio);
            Ignore(x => x.ZonaID);
            Ignore(x => x.Control);
            Ignore(x => x.ToShortControl);
            Ignore(x => x.CentroComputo);
            Ignore(x => x.DirectorID);
            Ignore(x => x.AsignacionDocentes);
            Ignore(x => x.Grupos);
        }
    }
}
