using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using POV.Modelo.BO;
using POV.Modelo.Estandarizado.BO;

namespace POV.Modelo.Mapping.ModeloDiagnostico
{
    internal class AModeloMapping: EntityTypeConfiguration<AModelo>
	{
        public AModeloMapping()
		{
			ToTable("Modelo");
			HasKey(p => p.ModeloID);
			Property(p => p.ModeloID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            
			Property(p => p.Estatus).HasColumnName("Activo");
            Ignore(p => p.TipoModelo);

            Map<ModeloDinamico>(m => { m.ToTable("Modelo"); m.Requires("TipoModelo").HasValue<byte>(0); });
		}
    }
}
