using POV.Blog.BO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POV.Modelo.Mapping.Blog
{
    public class PostFavoritoMapping : EntityTypeConfiguration<PostFavorito>
    {
        public PostFavoritoMapping()
        {
            ToTable("PostAspiranteFavorito");
            HasKey(x => x.PostFavoritoAspiranteId);
            Property(x => x.PostFavoritoAspiranteId).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.AlumnoId).IsRequired();
            Property(x => x.BlogId).IsRequired();
            Property(x => x.PostId).IsRequired();
            Property(x => x.Categorias).IsRequired();
        }
    }
}
