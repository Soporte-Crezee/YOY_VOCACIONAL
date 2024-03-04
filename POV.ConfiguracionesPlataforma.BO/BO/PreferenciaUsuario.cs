using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POV.Seguridad.BO;

namespace POV.ConfiguracionesPlataforma.BO
{
    public class PreferenciaUsuario
    {
        public long? PreferenciaUsuarioId { get; set; }
        virtual public PlantillaLudica PlantillasLudicas { get; set; }
        public int? PlantillaLudicaId { get; set; }
        virtual public Usuario Usuarios { get; set; }
        public int? UsuarioId { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public byte[] Version { get; set; }
    }
}
