namespace POV.Prueba.Reportes.BO
{
  public class ResultadoEstandarizadoNivelDesempenoDetail
    {
        public int? AreaConocimientoId { get; set; }
        public string NombreAreaConocimiento { get; set; }
        public int? NivelDesempenoId { get; set; }
        public string NombreNivelDesempeno { get; set; }
        public string PuntajeRango { get; set; }
        public string TotalAlumnosNivelDesempeno { get; set; }
        public decimal? PuntajeMinimo { get; set; }
        public decimal? PuntajeMaximo { get; set; }
    }
}
