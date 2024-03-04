using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POV.Web.DTO
{
    public class reactivodto
    {
        public string reactivoid { get; set; }
        public string nombrereactivo { get; set; }
        public string descripcion { get; set; }
        public string retroalimentacion { get; set; }
        public string plantilla { get; set; }
        public int? tipocomplejidadid { get; set; }
        public byte? presentacionplantilla { get; set; }
        public string nombrecomplejidad { get; set; }
        public int? areaaplicacionid { get; set; }
        public string areatitulo { get; set; }
        public string thumb { get; set; }

        //variables de control
        public bool? success { get; set; }
        public List<string> errors { get; set; }
        public List<preguntadto> preguntas { get; set; }
        //control suscripcion

        public bool? issuscrito { get; set; }
        public int? docenteid { get; set; }
        public bool? tipodocente { get; set; }
        public List<preguntadto> preguntasdeleted { get; set; }
        public List<opcionrespuestadto> opcionesdeleted { get; set; }

        public int? tipo { get; set; }
    }
}
