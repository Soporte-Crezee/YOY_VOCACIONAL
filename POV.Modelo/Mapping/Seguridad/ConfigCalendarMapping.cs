using GP.SocialEngine.BO;
using POV.Seguridad.BO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace POV.Modelo.Mapping.Seguridad
{
    public class ConfigCalendarMapping : EntityTypeConfiguration<ConfigCalendar>
    {
        public ConfigCalendarMapping()
        {
            ToTable("ConfigCalendar");
            HasKey(x => x.ConfigCalendarID);
            Property(x => x.ConfigCalendarID).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(x => x.DiasLaborales).IsRequired();
            Property(x => x.InicioTrabajo).IsRequired();
            Property(x => x.FinTrabajo).IsRequired();
            Property(x => x.InicioDescanso).IsOptional();
            Property(x => x.FinDescanso).IsOptional();
            
            HasRequired(x => x.Usuario).WithMany(x => x.ConfigCalendar);
        }
    }
}
