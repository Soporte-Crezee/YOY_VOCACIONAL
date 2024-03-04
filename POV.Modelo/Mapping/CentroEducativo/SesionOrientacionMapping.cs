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
    public class SesionOrientacionMapping : EntityTypeConfiguration<SesionOrientacion>
    {
        public SesionOrientacionMapping() 
        {
            ToTable("SesionOrientacion");
            HasKey(x => x.SesionOrientacionID);
            Property(x => x.SesionOrientacionID).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Inicio).IsRequired();
            Property(x => x.Fin).IsRequired();
            Property(x => x.Fecha).IsRequired();
            Property(x => x.AsistenciaAspirante).IsRequired();
            Property(x => x.AsistenciaOrientador).IsRequired();
            Property(x => x.CantidadHoras).IsOptional();
            Property(x => x.EstatusSesion).IsRequired();
            Property(x => x.EncuestaContestada).IsRequired();
            Property(x => x.HoraFinalizado).IsOptional();

            Property(x => x.Version).IsRowVersion().HasColumnName("RowVersion");
        }
    }
}
