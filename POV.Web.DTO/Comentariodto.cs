using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POV.Web.DTO
{
    public class comentariodto
    {
        public string comentarioid { get; set; }
        public string contenidocom { get; set; }
        public string fechacom { get; set; }
        public string fechacomformated { get; set; }
        public int? estatuscom { get; set; }
        public long? usuariosocialidcom { get; set; }
        public string usuariosocialnombrecom { get; set; }
        public string publicacionid { get; set; }
        
        /// <summary>
        /// Determina que variable usar en el js para mostrar la opcion de eliminar
        /// </summary>
        public bool? renderdelete { get; set; }
        public bool? renderlink { get; set; }
        public bool? renderreporteabuso { get; set; }
        public string rankingid { get; set; }
        public int? ratingvotes { get; set; }
        public int? vote { get; set; }

        public int? numvotes1 { get; set; }
        public int? numvotes2 { get; set; }
        public int? numvotes3 { get; set; }
        public bool? success { get; set; }
        public string error { get; set; }

        public short? tipo { get; set; }
        public string tipocompartido { get; set; }
        public string compartidoid { get; set; }

        public recursocompartidodto recursocompartido { get; set; }
    }
}
