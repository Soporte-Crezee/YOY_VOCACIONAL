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
    public class ProductoCosteoMapping : EntityTypeConfiguration<ProductoCosteo>
    {
        public ProductoCosteoMapping() 
        {
            ToTable("Productos");
            HasKey(x => x.ProductoID);
            Property(x => x.ProductoID).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Nombre).IsRequired().HasMaxLength(250);
            Property(x => x.Descripcion).IsRequired().HasMaxLength(500);
            Property(x => x.TipoProducto).IsRequired();
            HasMany(x => x.CostoProducto).WithRequired(p => p.Producto);
            Property(x => x.PruebaID).IsOptional();
        }
    }
}
