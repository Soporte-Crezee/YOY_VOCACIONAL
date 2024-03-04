using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using POV.ContenidosDigital.BO;

namespace POV.Modelo.Mapping.ContenidosDigital
{
    public class URLContenidoMapping : EntityTypeConfiguration<URLContenido>
    {
        public URLContenidoMapping()
        {
            ToTable("URLContenido");
            HasKey(a => a.URLContenidoID);
            Property(a => a.URLContenidoID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(a => a.URL).HasMaxLength(1000).IsRequired();
            Property(a => a.Nombre).HasMaxLength(500).IsRequired();
            Property(a => a.EsPredeterminada).IsRequired();
            Property(a => a.FechaRegistro).IsRequired();
            Property(a => a.Activo).IsRequired();  
        }
    }
}

