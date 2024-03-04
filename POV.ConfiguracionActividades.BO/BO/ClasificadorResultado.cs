using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POV.Modelo.BO;

namespace POV.ConfiguracionActividades.BO
{
    public abstract class ClasificadorResultado
    {
        public int? ClasificadorResultadoId { get; set; }

        public virtual AModelo Modelo { get; set; }

        public int? ModeloId { get; set; }

        public DateTime? FechaRegistro { get; set; }

        public byte[] Version { get; set; }

        public abstract string GetNombreClasificador();
    }
}
