using POV.Blog.BO;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POV.Modelo.Blog.Mapping
{
    public class PostBlogEngineMapping : EntityTypeConfiguration<PostBlogEngine>
    {
        public PostBlogEngineMapping()
        {
            ToTable("ViewBlogPost");
            HasKey(x => x.PostId);
            Property(x => x.BlogId).IsRequired();
            Property(x => x.PostId).IsRequired();
            Property(x => x.CategoryId).IsRequired();
            Property(x => x.Title).IsRequired().HasMaxLength(255);
            Property(x => x.Description).IsRequired();
            Property(x => x.DateCreated).IsRequired();
            Property(x => x.Author).IsRequired().HasMaxLength(50);
            Property(x => x.Categoria).IsRequired().HasMaxLength(50);
            Property(x => x.RutaPost).IsRequired().HasMaxLength(300);
            Property(x => x.IsDeleted).IsRequired();
            Property(x => x.IsPublished).IsRequired();
            HasKey(x => new { x.BlogId, x.PostId, x.CategoryId });
        }
    }
}
