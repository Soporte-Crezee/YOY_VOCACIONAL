using POV.Reactivos.BO;

namespace POV.Operaciones.BO
{
   public class PreguntaTemp
    {
     
       public Pregunta Pregunta { get; set; }
       public int? Orden { get; set; }
       public RecursoImagenTemp ImagenPregunta { get; set; }
    

       public PreguntaTemp CreatePreguntaTemp(Pregunta pregunta, ETipoReactivo tipoReactivo,int? orden,RecursoImagenTemp imagenTemp)
       {
           PreguntaTemp p = new PreguntaTemp();
           switch (tipoReactivo)
           {
               case ETipoReactivo.Final:
                   #region ***** Pregunta Tipo Final****
                   p.Pregunta = pregunta;
                   p.Orden = orden ?? 1;
                   p.ImagenPregunta = imagenTemp;
                   break;
                   #endregion
               case ETipoReactivo.ModeloGenerico:
                   #region ***Pregunta Tipo Generico ***
                   #endregion
                   break;
           }

           return p;
       }

    }
}
