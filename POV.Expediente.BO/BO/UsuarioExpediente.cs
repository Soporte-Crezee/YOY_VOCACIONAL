using POV.CentroEducativo.BO;
using POV.Seguridad.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POV.Expediente.BO
{
    public class UsuarioExpediente
    {
        public int? UsuarioID { get; set; }
        public long? AlumnoID { get; set; }
    }
}
