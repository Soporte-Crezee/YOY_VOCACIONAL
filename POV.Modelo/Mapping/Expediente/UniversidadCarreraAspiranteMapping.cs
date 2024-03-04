using POV.Expediente.BO;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POV.Modelo.Mapping.Expediente
{
    public class UniversidadCarreraAspiranteMapping : EntityTypeConfiguration<UniversidadCarreraAspirante>
    {
        public UniversidadCarreraAspiranteMapping()
        {
            ToTable("UniversidadCarreraAspirante");
            HasKey(x => new { x.ExpedienteEscolarID, x.UniversidadCarreraID });

            Property(X=>X.ExpedienteEscolarID).IsRequired();
            Property(x=>x.UniversidadCarreraID).IsRequired();

            Ignore(x => x.Expediente);
            Ignore(x => x.UniversidadCarrera);
        }

    }
}
