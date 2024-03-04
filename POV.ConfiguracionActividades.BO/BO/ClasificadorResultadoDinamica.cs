using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POV.Modelo.BO;

namespace POV.ConfiguracionActividades.BO
{
    public class ClasificadorResultadoDinamica : ClasificadorResultado
    {
        public virtual Clasificador Clasificador { get; set; }

        public int? ClasificadorId { get; set; }

        public override string GetNombreClasificador()
        {
            if (Clasificador != null)
                return Clasificador.Nombre;

            return string.Empty;
        }
    }
}
