using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POV.Web.DTO
{
    public class notificaciondto
    {
        public string notificacionid { get; set; }
        public string emisorname { get; set; }
        public long? emisorid { get; set; }
        public bool? renderemisorlink { get; set; }
        public string receptorname{get; set;}
        public long? receptorid{get; set;}
        public string textonotificacion { get; set; }
        public string notificableid { get; set; }
        public string textonotificable { get; set; }
        public string urllabel { get; set; }
        public string url { get; set; }
        public short? estatus { get; set; }
        public short? tiponotificacion { get; set; }
        public string fecharegistro { get; set; }

    }
}
