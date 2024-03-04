using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POV.Blog.BO
{
    public class PostBlogEngine
    {
        public Guid? BlogId { get; set; }
        public Guid? PostId { get; set; }
        public Guid? CategoryId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? DateCreated { get; set; }
        public string Author { get; set; }
        public string Categoria { get; set; }
        public string RutaPost { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? IsPublished { get; set; }
    }
}
