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
    public class CompraProductoMapping : EntityTypeConfiguration<CompraProducto>
    {
        public CompraProductoMapping() 
        {
            ToTable("CompraProducto");
            HasKey(x => x.CompraProductoID);
            Property(x => x.CompraProductoID).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.CostoCompra).IsRequired();
            Property(x=>x.FechaCompra).IsOptional();
            Property(x=>x.CodigoCompra).IsOptional();
            Property(x=>x.UniversidadID).IsOptional();
        }
    }
}
