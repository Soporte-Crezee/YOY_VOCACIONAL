using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POV.Profesionalizacion.DTO.BO
{
   public  class cursodto
    {
       public int? cursoid{get;set;}
       public string cursonombre { get; set; }
       public string cursotema { get; set; }
       public short cursopresencial { get; set; }
       public byte cursoestatus { get; set; }
       public string cursoinformacion { get; set; }
    }
}
