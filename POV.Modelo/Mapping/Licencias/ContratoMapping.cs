using POV.Licencias.BO;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace POV.Modelo.Mapping.Licencias
{
    class ContratoMapping : EntityTypeConfiguration<Contrato>
    {
        public ContratoMapping()
        {
            ToTable("Contrato");
            HasKey(x => x.ContratoID);
            Property(x => x.ContratoID).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Clave).IsRequired();
            Property(x => x.FechaContrato).IsRequired();
            Property(x => x.InicioContrato).IsRequired();
            Property(x => x.FinContrato).IsRequired();
            Property(x => x.LicenciasLimitadas).IsRequired();
            Property(x => x.NumeroLicencias).IsOptional();
            Property(x => x.FechaRegistro).IsRequired();

            Ignore(x => x.Cliente);
            Ignore(x => x.Ubicacion);
            Ignore(x => x.UsuarioRegistro);
            Ignore(x => x.ProfesionalizacionContrato);
            Ignore(x => x.CiclosContrato);
        }
    }
}
