using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POV.Web.DTO
{
   public class reporteabusodto
    {
       public long? usuariosocialcurrent { get; set; }
       public string reporteabusoid { get; set; }
       

       public long? reportadoid { get; set; }
       public string reportadonombre { get; set; }

       public long? reportanteid { get; set; }
       public string reportantenombre { get; set; }

       public short? tiporeporte { get; set; }

       public string reportableid { get; set; }
       public string contenido { get; set; }
       public string url { get; set; }
       public string textonotificacion { get; set; }
       public short? estatusreporte { get; set; }
       
       public DateTime? fechainicio { get; set; }
       public DateTime? fechafin { get; set; }
       public string fechainicioformated { get; set; }
       public string fechafinformated { get; set; }
       public string error { get; set; }
       public bool? rendereportadolink { get; set; }
       public bool? success { get; set; }

    }
}
