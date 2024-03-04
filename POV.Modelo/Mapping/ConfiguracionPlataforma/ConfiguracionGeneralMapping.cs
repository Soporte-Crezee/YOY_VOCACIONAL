using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using POV.ConfiguracionesPlataforma.BO;

namespace POV.Modelo.Mapping.ConfiguracionPlataforma
{
    class ConfiguracionGeneralMapping:EntityTypeConfiguration<ConfiguracionGeneral>
    {
        public ConfiguracionGeneralMapping()
        {
             ToTable("ConfiguracionGeneral");
             HasKey(x => x.ConfiguracionGeneralId);

             Property(x => x.ConfiguracionGeneralId).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
             Property(x => x.MaximoTopAlumnos).IsRequired();
             Property(x => x.RutaServidorContenido).IsRequired().HasColumnName("RutaServidorContenidos");
             Property(x => x.RutaPlantillas).IsRequired();
             Property(x => x.RutaInsignias).IsRequired();
             Property(x => x.Version).HasColumnName("RowVersion").IsRowVersion();
        }
    }
}
