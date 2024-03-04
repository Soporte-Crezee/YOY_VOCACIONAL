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
    public class TutorAlumnoMapping : EntityTypeConfiguration<TutorAlumno>
    {
        public TutorAlumnoMapping()
        {
            ToTable("TutorAlumno");
            HasKey(x => new { x.TutorID, x.AlumnoID });
            Property(x => x.TutorID).IsRequired();
            Property(x => x.AlumnoID).IsRequired();
            Property(x => x.Parentesco).IsRequired();

            HasRequired(x => x.Alumno).WithMany(x=>x.Tutores).HasForeignKey(x => x.AlumnoID);
            HasRequired(x => x.Tutor).WithMany(x=>x.Alumnos).HasForeignKey(x=>x.TutorID);

            Ignore(x => x.DescripcionParentesco);
        }
    }
}
