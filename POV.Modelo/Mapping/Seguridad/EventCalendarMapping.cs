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
    public class EventCalendarMapping : EntityTypeConfiguration<EventCalendar>
    {
        public EventCalendarMapping() 
        {
            ToTable("EventCalendar");
            HasKey(x => x.EventCalendarID);
            Property(x => x.EventCalendarID).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(x => x.Asunto).IsRequired();
            Property(x => x.Fecha).IsRequired();
            Property(x => x.HrsInicio).IsRequired();
            Property(x => x.HrsFin).IsRequired();
            Property(x => x.UsuarioID).IsRequired();

            //Alumno
            Property(x => x.AlumnoID).IsOptional();
            Property(x => x.NombreCompletoAlumno).IsOptional();

            Property(x => x.CantidadHoras).IsOptional();

            //HasMany(x => x.ConfigCalendar).WithOptional(x => x.ConfigCalendar);
            HasOptional(x => x.ConfigCalendar).WithMany(x => x.EventCalendar);
            
        }
    }
}
