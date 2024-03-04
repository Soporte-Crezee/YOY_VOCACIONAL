using POV.CentroEducativo.BO;
using POV.Prueba.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POV.Prueba.Diagnostico.Dinamica.BO
{
    public class SumarioGeneralSacks
    {
        public int? SumarioGeneralSACKSID { get; set; }
        public APrueba Prueba { get; set; }
        public Alumno Alumno { get; set; }
        public string SumarioMadurez { get; set; }
        public string SumarioNivelRealida { get; set; }
        public string SumarioConflictoExpresados { get; set; }
        public DateTime? FechaRegistro { get; set; }
    }
}
