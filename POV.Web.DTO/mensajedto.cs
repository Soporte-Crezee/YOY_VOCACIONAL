using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POV.Web.DTO
{
   public class mensajedto
    {
        public string mensajeid { get; set; }
        public string contenido { get; set; }
        public string asunto { get; set; }
        public bool? espadre { get; set; }
        public string guidconversacion { get; set; }
        public long? usuariosocialid { get; set; }
        public string usuariosocialnombre { get; set; }
        public string usuariosocialurlperfil{get; set;}

        public long? remitenteid { get; set; }
        public string remitentenombre { get; set; }
        public string remitenteurlperfil{get; set;}
        public string fechamensaje { get; set; }
        public string fechaformated { get; set; }
       

        public short? tipo { get; set; }

        public List<contactodto> destinatarios { get; set; }
        public List<mensajedto> respuestas { get; set; }
        public string destinatariosstring { get; set; }

       public bool? renderdelete { get; set; }

        public bool? success { get; set; }
        public string error { get; set; }

        public int? estatus { get; set; }
        public int? totalrespuestas{get; set;}
        public bool? complete{get; set;}
        public int? recordcount { get; set; }
    }
}
