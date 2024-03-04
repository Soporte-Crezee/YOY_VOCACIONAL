using POV.ConfiguracionesPlataforma.BO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POV.Modelo.Mapping.ConfiguracionPlataforma
{
    internal class PreferenciaUsuarioMapping: EntityTypeConfiguration<PreferenciaUsuario>
    {
        public PreferenciaUsuarioMapping()
        {
            ToTable("PreferenciasUsuario");

            HasKey(k => k.PreferenciaUsuarioId);
            Property(x => x.PreferenciaUsuarioId).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(X => X.UsuarioId).IsRequired();
            
            Property(x => x.PlantillaLudicaId).IsOptional();
            HasOptional(x => x.PlantillasLudicas).WithMany().HasForeignKey(x => x.PlantillaLudicaId);

            Property(x => x.FechaRegistro).IsRequired();
            
            Property(x => x.Version).IsRowVersion().HasColumnName("RowVersion");

            Ignore(x => x.Usuarios);
        }
    }
}
