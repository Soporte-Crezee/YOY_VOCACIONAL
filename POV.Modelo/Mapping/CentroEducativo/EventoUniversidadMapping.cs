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
    public class EventoUniversidadMapping : EntityTypeConfiguration<EventoUniversidad> 
    {
        public EventoUniversidadMapping()
        {
            ToTable("EventoUniversidad");
            HasKey(x => x.EventoUniversidadId);
            Property(x => x.EventoUniversidadId).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Nombre).IsRequired();
            Property(x => x.Descripcion).IsRequired();
            Property(x => x.FechaInicio).IsRequired();
            Property(x => x.FechaFin).IsRequired();
            Property(x => x.UniversidadId).IsRequired();
        }
    }
}
