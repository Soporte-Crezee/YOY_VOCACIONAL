using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POV.Web.DTO
{
    public class postfavoritodto
    {
        public long? PostFavoritoAspiranteId { get; set; }
        public long? AlumnoId { get; set; }
        public Guid? BlogId { get; set; }
        public Guid? PostId { get; set; }
        public string Categorias { get; set; }
        public string Error { get; set; }
        public string Success { get; set; }
    }
}
