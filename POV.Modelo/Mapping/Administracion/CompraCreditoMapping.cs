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
    public class CompraCreditoMapping : EntityTypeConfiguration<CompraCredito>
    {
        public CompraCreditoMapping() 
        {
            ToTable("CompraCredito");
            HasKey(x => x.CompraCreditoID);
            Property(x => x.CompraCreditoID).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.CostoCompra).IsRequired();
            Property(x => x.FechaCompra).IsRequired();
            Property(x => x.CodigoCompra).IsRequired().HasMaxLength(50);
            Property(x => x.AlumnoID).IsOptional();
            Property(x => x.TutorID).IsOptional();
        }
    }
}
