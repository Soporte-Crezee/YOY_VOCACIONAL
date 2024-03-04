using System.Collections.Generic;

namespace POV.Prueba.Reportes.BO
{
   public class PruebaEstandarizadaDetail
    {
        public int? EscuelaId { get; set; }
        public string NombreEscuela { get; set; }
        public string TurnoEscuela { get; set; }
        public string NombreCicloEscolar { get; set; }
        public string NombrePrueba { get; set; }
        public string NombreModelo { get; set; }
        public string MetodoCalificacion { get; set; }
        public string ClavePrueba { get; set; }
        public List<ResultadoEstandarizadoGrupoDetail> ListaResultadoGrupo { get; set; }

    }
}
