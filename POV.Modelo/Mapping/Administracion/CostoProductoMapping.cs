using POV.Administracion.BO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POV.Modelo.Mapping.Administracion
{
    public class CostoProductoMapping : EntityTypeConfiguration<CostoProducto>
    {
        public CostoProductoMapping() 
        {
            ToTable("CostoProducto");
            HasKey(x => x.CostoProductoId);
            Property(x => x.CostoProductoId).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x=>x.Precio).IsRequired();
            Property(x=>x.FechaInicio).IsRequired();    
            Property(x=>x.FechaFin).IsOptional();
            Property(x => x.ProductoID).IsRequired();
        }
    }
}
