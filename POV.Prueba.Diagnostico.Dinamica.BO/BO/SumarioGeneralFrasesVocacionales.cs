using POV.CentroEducativo.BO;
using POV.Prueba.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POV.Prueba.Diagnostico.Dinamica.BO
{
    public class SumarioGeneralFrasesVocacionales
    {
        public int? SumarioGeneralFrasesID { get; set; }
        public APrueba Prueba { get; set; }
        public Alumno Alumno { get; set; }
        public string SumarioOrganizacionPersonalidad { get; set; }
        public string SumarioPerspectivaOpciones { get; set; }
        public string SumarioFuentesConflicto { get; set; }
        public DateTime? FechaRegistro { get; set; }
    }
}
