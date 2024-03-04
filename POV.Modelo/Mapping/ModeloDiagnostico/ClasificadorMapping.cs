using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using POV.Modelo.BO;

namespace POV.Modelo.Mapping.ModeloDiagnostico
{
    internal class ClasificadorMapping : EntityTypeConfiguration<Clasificador>
    {
        public ClasificadorMapping()
        {
            ToTable("Clasificador");
            HasKey(p => p.ClasificadorID);
            Property(p => p.ClasificadorID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(p => p.Nombre).IsRequired();
             
            Ignore(p => p.ListaPropiedadClasificador);
        }
    }
}
