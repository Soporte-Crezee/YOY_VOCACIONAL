using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POV.Web.DTO
{
   public class usuariodto
    {
       public long? usuarioid { get; set; }
       public string nombreusuario { get; set; }
       public string nombreantusuario { get; set; }
       public string email { get; set; }
       public string telefono { get; set; }
       public bool? esactivo { get; set; }
       public long? universidadid { get; set; }
    }
}
