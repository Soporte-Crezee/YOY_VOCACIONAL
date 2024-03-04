using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using POV.CentroEducativo.BO;

namespace POV.Modelo.Mapping.CentroEducativo
{
    public class CicloEscolarMapping:EntityTypeConfiguration<CicloEscolar>
    {
		public CicloEscolarMapping()
        {
            ToTable("CicloEscolar");
            HasKey(x => x.CicloEscolarID);
            Property(x => x.CicloEscolarID)
                .IsRequired()
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
			Ignore(x => x.UbicacionID);

        }
    }
}
