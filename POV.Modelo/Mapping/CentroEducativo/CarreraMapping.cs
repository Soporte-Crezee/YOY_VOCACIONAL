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
    public class CarreraMapping: EntityTypeConfiguration<Carrera>
    {
        public CarreraMapping()
        {
            ToTable("Carrera");
            HasKey(x => x.CarreraID);
            Property(x => x.CarreraID).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.NombreCarrera).IsRequired().HasMaxLength(100);
            Property(x => x.Descripcion).IsRequired().HasMaxLength(254);
            Property(x => x.Activo).IsRequired();
            HasRequired(x => x.Clasificador).WithMany().HasForeignKey(x => x.ClasificadorID);
        }
    }
}
