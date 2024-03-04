using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POV.ContenidosDigital.BO;

namespace POV.ConfiguracionActividades.BO
{
    public class TareaContenidoDigital : Tarea
    {
        public virtual ContenidoDigital ContenidoDigital { get; set; }

        public long? ContenidoDigitalDocenteId { get; set; }

        public override string GetTypeDescription()
        {
            return "Contenido digital";
        }

        public override string GetIdentificador()
        {
            return ContenidoDigital != null && ContenidoDigital.ContenidoDigitalID != null ? ContenidoDigital.ContenidoDigitalID.ToString() : null;
        }

        public override string GetUrl()
        {
            return "";
        }
    }
}
