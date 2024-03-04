using System.Collections.Generic;
using POV.Reactivos.BO;

namespace POV.Operaciones.BO
{
   public class ReactivoTemp
    {
      
       public Reactivo Reactivo { get; set; }
       public RecursoImagenTemp ImagenReactivo { get; set; }
       public List<PreguntaTemp> ListaPregunta { get; set; }
       public int NumeroPreguntas { get; set; }
       public PreguntaTemp NewPreguntaTemp(ETipoReactivo tipoReactivo,Pregunta pregunta,RecursoImagenTemp recursoImagenTemp)
       {
           if(ListaPregunta==null)
               ListaPregunta= new List<PreguntaTemp>();

           PreguntaTemp p = new PreguntaTemp().CreatePreguntaTemp(pregunta, tipoReactivo, NumeroPreguntas + 1, recursoImagenTemp);
          
           return p;
       }
    }
}
