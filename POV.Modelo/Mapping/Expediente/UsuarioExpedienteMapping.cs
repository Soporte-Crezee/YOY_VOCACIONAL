using POV.Expediente.BO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POV.Modelo.Mapping.Expediente
{
    public class UsuarioExpedienteMapping : EntityTypeConfiguration<UsuarioExpediente>
    {
        public UsuarioExpedienteMapping()
        {
            ToTable("UsuarioExpediente");
            HasKey(u=>new 
            {
                u.UsuarioID,
                u.AlumnoID
            });

            Property(x => x.UsuarioID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None).IsRequired();
            Property(x => x.AlumnoID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None).IsRequired();
        }
    }
}
