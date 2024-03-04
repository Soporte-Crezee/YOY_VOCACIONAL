using System.Data.Entity.ModelConfiguration;
using System.ComponentModel.DataAnnotations.Schema;
using POV.ContenidosDigital.BO; 

using System.ComponentModel.DataAnnotations.Schema;
namespace POV.Modelo.Mapping.ContenidosDigital
{

	public class ContenidoDigitalMapping : EntityTypeConfiguration<ContenidoDigital>
	{
		public ContenidoDigitalMapping()
		{
			ToTable("ContenidoDigital");
			HasKey(c => c.ContenidoDigitalID); 
			Ignore(c => c.InstitucionOrigen);
            Ignore(c => c.ListaURLContenido);
            Ignore(c => c.TipoDocumento);
            Property(a => a.ContenidoDigitalID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(a => a.Clave).HasMaxLength(30).IsRequired();
            Property(a => a.Nombre).HasMaxLength(200).IsRequired();
            Property(a => a.EsInterno).IsRequired();
            Property(a => a.EsDescargable).IsRequired();
            Property(a => a.Tags).IsOptional();
            Property(a => a.FechaRegistro).IsRequired();
            Property(a => a.EstatusContenido).IsRequired();

            Property(a => a.TipoDocumentoId).IsRequired(); 

            HasMany(a => a.URLContenidosProxy).WithRequired().Map(m => m.MapKey("ContenidoDigitalID"));

		}
	}
}
