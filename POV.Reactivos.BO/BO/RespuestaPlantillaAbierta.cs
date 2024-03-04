using POV.Modelo.BO;
using System;
using System.Collections.Generic;
using System.Text;

namespace POV.Reactivos.BO { 
   /// <summary>
   /// RespuestaPlantillaAbierta
   /// </summary>
   public abstract class RespuestaPlantillaAbierta: RespuestaPlantilla {

       /// <summary>
       /// Ponderación de RespuestaPlantillaAbierta
       /// </summary>
       private decimal? ponderacion;

       public decimal? Ponderacion
       {
           get { return this.ponderacion; }
           set { this.ponderacion = value; }
       }

       /// <summary>
       /// ValorRespuesta de RespuestaPlantillaAbierta
       /// </summary>
       private string valorRespuesta;
       public string ValorRespuesta
       {
           get { return this.valorRespuesta; }
           set { this.valorRespuesta = value; }
       }

       private ModeloDinamico modelo;
       /// <summary>
       /// Modelo relacionado
       /// </summary>
       public ModeloDinamico Modelo
       {
           get { return modelo; }
           set { modelo = value; }
       }
       private Clasificador clasificador;
       /// <summary>
       /// Clasificador relacionado
       /// </summary>
       public Clasificador Clasificador
       {
           get { return clasificador; }
           set { clasificador = value; }
       }
   } 
}
