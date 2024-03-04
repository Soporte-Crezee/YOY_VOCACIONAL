using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POV.Operaciones.BO
{
    public class ResultadoExportacionOrientador
    {
        public int? ResultadoExportacionID { get; set; }
        public string Nombre { get; set; }
        public string PrimerApellido { get; set; }
        public string SegundoApellido { get; set; }
        public string Curp { get; set; }
        public bool Sexo { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public string Correo { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public string Incosistencia { get; set; }
    }
}
