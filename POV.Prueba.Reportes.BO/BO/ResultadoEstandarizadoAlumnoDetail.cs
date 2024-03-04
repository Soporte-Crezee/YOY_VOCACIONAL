using System.Collections.Generic;

namespace POV.Prueba.Reportes.BO
{
  public  class ResultadoEstandarizadoAlumnoDetail
    {
      public long? AlumnoId { get; set; }
      public string NombreAlumno { get; set; }
      public string Sexo { get; set; }
      public string Edad { get; set; }
      public int? ResultadoPruebaId { get; set; }

      public List<ResultadoAreaConocimientoAlumnoDetail> ListaResultadoAreaConocimiento { get; set; }
      
    }
}
