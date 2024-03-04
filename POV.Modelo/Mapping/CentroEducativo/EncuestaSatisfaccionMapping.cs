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
    public class EncuestaSatisfaccionMapping : EntityTypeConfiguration<EncuestaSatisfaccion>
    {
        public EncuestaSatisfaccionMapping()
        {
            ToTable("EncuestaSatisfaccion");
            HasKey(x => x.EncuestaID);
            Property(x => x.EncuestaID).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.PreguntaUno).IsOptional();
            Property(x => x.RespuestaUno).IsOptional();

            Property(x => x.PreguntaDos).IsOptional();
            Property(x => x.RespuestaDos).IsOptional();

            Property(x => x.PreguntaTres).IsOptional();
            Property(x => x.RespuestaTres).IsOptional();

            Property(x => x.PreguntaCuatro).IsOptional();
            Property(x => x.RespuestaCuatro).IsOptional();

            Property(x => x.PreguntaCinco).IsOptional();
            Property(x => x.RespuestaCinco).IsOptional();

            Property(x => x.AlumnoID).IsOptional();
            Property(x => x.DocenteID).IsOptional();
            Property(x => x.SesionOrientacionID).IsOptional();
        }
    }
}
