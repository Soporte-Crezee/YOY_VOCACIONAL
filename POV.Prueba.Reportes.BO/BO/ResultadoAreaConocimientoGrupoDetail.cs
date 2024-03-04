using System.Collections.Generic;

namespace POV.Prueba.Reportes.BO
{
    public class ResultadoAreaConocimientoGrupoDetail
    {
        public int? AreaId { get; set; }
        public string NombreAreaConocimiento { get; set; }
        public List<ResultadoEstandarizadoNivelDesempenoDetail> ListaResultadoNivelDesempeno { get; set; } 
    }
}
