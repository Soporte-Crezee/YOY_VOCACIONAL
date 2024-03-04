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
    public class NotaCompraMapping : EntityTypeConfiguration<NotaCompra>
    {
        public NotaCompraMapping() 
        {
            ToTable("NotaCompra");
            HasKey(x => x.NotaCompraID);
            Property(x => x.NotaCompraID).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.TutorID);
            Property(x => x.AlumnoID);
            Property(x => x.FechaCompra).IsRequired();
            Property(x => x.CostoProductoID).IsRequired();
            Property(x => x.Cantidad).IsOptional();
            Property(x => x.ConceptoCompra).IsRequired();
        }
    }
}
