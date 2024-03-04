using POV.CentroEducativo.BO;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POV.Modelo.Mapping.CentroEducativo
{
    public class UniversidadCarreraMapping : EntityTypeConfiguration<UniversidadCarrera>
    {
        public UniversidadCarreraMapping()
        {
            ToTable("UniversidadCarrera");

            HasKey(x => x.UniversidadCarreraID);

            Property(x => x.UniversidadID).IsRequired(); 
            Property(x => x.CarreraID).IsRequired();
        }
    }
}
