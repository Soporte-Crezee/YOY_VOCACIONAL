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
    public class TutorMapping : EntityTypeConfiguration<Tutor>
    {
        public TutorMapping()
        {
            ToTable("Tutor");
            HasKey(x=>x.TutorID);
            Property(x => x.TutorID).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Nombre).IsRequired().HasMaxLength(80);
            Property(x => x.PrimerApellido).IsRequired().HasMaxLength(50);
            Property(x => x.SegundoApellido).IsOptional().HasMaxLength(50);
            Property(x => x.FechaNacimiento).IsOptional();
            Property(x => x.Direccion).IsOptional().HasMaxLength(250);
            Property(x => x.Estatus).IsOptional();
            Property(x => x.FechaRegistro).IsOptional();
            Property(x => x.Sexo).IsRequired();
            Property(x => x.CorreoConfirmado).IsRequired();
            Property(x => x.DatosCompletos).IsOptional();
            Property(x => x.Codigo).IsRequired().HasMaxLength(10);
            Property(x => x.DatosCompletos).IsOptional();
            Property(x => x.Telefono).IsOptional();
            Property(x => x.CorreoElectronico).IsRequired().HasMaxLength(250);
            Property(x => x.EstatusIdentificacion).IsRequired();
            Property(x => x.Curp).IsOptional().HasMaxLength(18); ;
            Property(x => x.NotificacionPago).IsRequired();

            Ignore(x => x.Ubicacion);
            Ignore(x => x.Parentesco);

            Property(x => x.Credito).IsOptional();
            Property(x => x.CreditoUsado).IsOptional();
            Property(x => x.Saldo).IsOptional().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
        }
    }
}
