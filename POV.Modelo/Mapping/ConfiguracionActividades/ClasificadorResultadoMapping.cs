using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using POV.ConfiguracionActividades.BO;

namespace POV.Modelo.Mapping.ConfiguracionActividades
{
    public class ClasificadorResultadoMapping : EntityTypeConfiguration<ClasificadorResultado>
	{
        public ClasificadorResultadoMapping()
		{
            ToTable("ClasificadorResultadoActividad");
			HasKey(t => t.ClasificadorResultadoId);
			Property(t => t.ClasificadorResultadoId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
			Property(t => t.FechaRegistro).IsRequired();
			Property(t => t.Version).IsRowVersion().HasColumnName("RowVersion");
            HasRequired(t => t.Modelo).WithMany().HasForeignKey(t => t.ModeloId);
            Map<ClasificadorResultadoDinamica>(m => m.Requires("Discriminator").HasValue("ClasificadorResultadoDinamica"));
		}

    }
}
