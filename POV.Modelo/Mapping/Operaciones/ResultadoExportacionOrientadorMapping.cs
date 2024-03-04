using POV.Operaciones.BO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POV.Modelo.Mapping.Operaciones
{
    public class ResultadoExportacionOrientadorMapping : EntityTypeConfiguration<ResultadoExportacionOrientador>
    {
        public ResultadoExportacionOrientadorMapping() 
        {
            ToTable("ResultadoExportacionOrientador");
            HasKey(x => x.ResultadoExportacionID);
            Property(x => x.ResultadoExportacionID).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Nombre).IsRequired().HasMaxLength(50);
            Property(x => x.PrimerApellido).IsRequired().HasMaxLength(50);
            Property(x => x.SegundoApellido).IsOptional().HasMaxLength(50);
            Property(x => x.Curp).IsRequired().HasMaxLength(18);
            Property(x => x.Sexo).IsRequired();
            Property(x => x.FechaNacimiento).IsRequired();
            Property(x => x.Correo).IsRequired().HasMaxLength(50);
            Property(x => x.FechaRegistro).IsRequired();
            Property(x => x.Incosistencia).IsOptional();
        }
    }
}
