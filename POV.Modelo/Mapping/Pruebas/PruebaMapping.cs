using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using POV.Prueba.BO;
using POV.Prueba.Diagnostico.Dinamica.BO;


namespace POV.Modelo.Mapping.Pruebas
{
	public class PruebaMapping : EntityTypeConfiguration<APrueba>
	{
		public PruebaMapping()
		{
			ToTable("Prueba");
			Property(p => p.EstadoLiberacionPrueba).HasColumnName("EstadoLiberacion");
			HasKey(p => p.PruebaID);
			Property(p => p.PruebaID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

			Ignore(p => p.ListaPuntajes);
            HasRequired(p => p.Modelo).WithMany().Map(m => m.MapKey("ModeloID")); ;
			Ignore(p => p.TipoPrueba);

			Map<PruebaDinamica>(m => { m.ToTable("Prueba"); m.Requires("Tipo").HasValue(0); });
		}
	}
}
