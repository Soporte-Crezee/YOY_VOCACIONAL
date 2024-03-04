using System;
using System.Collections.Generic;

namespace POV.Prueba.Reportes.BO
{
   public class ResultadoEstandarizadoGrupoDetail
    {

       public Guid? GrupoId { get; set; }
       public string NombreGrupo { get; set; }
      

       public List<ResultadoEstandarizadoAlumnoDetail> ListaResultadoAlumno { get; set; }
       public List<ResultadoAreaConocimientoGrupoDetail> ListaResultadoAreaConocimientoGrupo { get; set; } 
    }
}
