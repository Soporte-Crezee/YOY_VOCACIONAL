using POV.CentroEducativo.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POV.Blog.BO
{
    public class PostFavorito
    {
        public long? PostFavoritoAspiranteId { get; set; }
        public long? AlumnoId { get; set; }
        public Guid? BlogId { get; set; }
        public Guid? PostId { get; set; }
        public string Categorias { get; set; }
    }
}
