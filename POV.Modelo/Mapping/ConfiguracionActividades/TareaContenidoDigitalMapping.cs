using System.Data.Entity.ModelConfiguration;
using POV.ConfiguracionActividades.BO;

namespace POV.Modelo.Mapping.ConfiguracionActividades
{
    public class TareaContenidoDigitalMapping : EntityTypeConfiguration<TareaContenidoDigital>
    {
        public TareaContenidoDigitalMapping()
        {

            HasRequired(t => t.ContenidoDigital).WithMany().HasForeignKey(t => t.ContenidoDigitalDocenteId);

        }
    }
} 
