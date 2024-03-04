using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POV.Web.DTO
{
   public class alumnodto
    {
       public long? alumnoid { get; set; }
       public long? usuarioid { get; set; }
       public string curp { get; set; }
       public string nombre { get; set; }
       public string primerapellido { get; set; }
       public string segundoapellido { get; set; }
       public string nombrecompleto { get; set; }
       public int? estatus { get; set; }
       public int? sexo { get; set; }
       public string email { get; set; }
       public string telefono { get; set; } 
       public string nombreusuario { get; set; }

       public string fechanacimiento { get; set; }
       public string redirect { get; set; }
    }
}
