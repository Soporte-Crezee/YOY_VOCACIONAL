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
    public class PaquetePremiumMapping : EntityTypeConfiguration<PaquetePremium>
    {
        public PaquetePremiumMapping() 
        {
            ToTable("PaquetePremium");
            HasKey(x => x.PaquetePremiumID);
            Property(x => x.PaquetePremiumID).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Nombre).IsRequired().HasMaxLength(250);
            Property(x => x.CostoPaquete).IsRequired();
            Property(x => x.HorasTutor).IsRequired();
            Property(x => x.FechaRegistro).IsRequired();
            Property(x => x.Estatus).IsRequired();
            HasMany(c => c.CompraPremium).WithRequired(p => p.PaquetePremium);
        }
    }
}
