using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POV.Web.DTO
{
    public class publicaciondto
    {
        public string publicacionid { get; set; }
        public string contenido { get; set; }
        public long? usuariosocialid { get; set; }
        public string usuariosocialnombre { get; set; }
        public long? destinatarioid { get; set; }
        public string destinatarionombre { get; set; }
        public string fechapublicacion { get; set; }
        public string fechaformated { get; set; }
        public long? socialhubid { get; set; }

        public string rankingid { get; set; }
        public int? vote { get; set; }

        public int? numvotes1 { get; set; }
        public int? numvotes2 { get; set; }
        public int? numvotes3 { get; set; }

        public int? ratingvotes { get; set; }

        public short? tipo { get; set; }
        public reactivodto reactivo { get; set; }
        public List<comentariodto> comentarios { get; set; }
        public int? totalcomentarios { get; set; }
        public bool? complete { get; set; }
        
        public bool? renderdelete { get; set; }
        public bool? renderlink { get; set; }
        public bool? renderfor { get; set; }
        public bool? renderforlink { get; set; }
        public bool? renderreporteabuso { get; set; }
        public bool? success { get; set; }
        public string error { get; set; }

        public string usuariosocialurl { get; set; }

        public string tipocompartido { get; set; }
        public string compartidoid { get; set; }

        public recursocompartidodto recursocompartido { get; set; }
    }
}
