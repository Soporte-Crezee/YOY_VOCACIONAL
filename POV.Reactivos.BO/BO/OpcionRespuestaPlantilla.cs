using System;
using System.Collections.Generic;
using System.Text;

namespace POV.Reactivos.BO { 
   /// <summary>
   /// Pregunta
   /// </summary>
   public class OpcionRespuestaPlantilla:ICloneable { 
      private int? opcionRespuestaPlantillaID;
      /// <summary>
      /// identificador de la OpcionRespuestaPlantilla
      /// </summary>
      public int? OpcionRespuestaPlantillaID{
         get{ return this.opcionRespuestaPlantillaID; }
         set{ this.opcionRespuestaPlantillaID = value; }
      }
      private string texto;
      /// <summary>
      /// texto de la OpcionRespuestaPlantilla
      /// </summary>
      public string Texto{
         get{ return this.texto; }
         set{ this.texto = value; }
      }
      private string imagenUrl;
      /// <summary>
      /// imagen de la OpcionRespuestaPlantilla
      /// </summary>
      public string ImagenUrl{
         get{ return this.imagenUrl; }
         set{ this.imagenUrl = value; }
      }
      private bool? esPredeterminado;
      /// <summary>
      /// predeterminado de la OpcionRespuestaPlantilla
      /// </summary>
      public bool? EsPredeterminado{
         get{ return this.esPredeterminado; }
         set{ this.esPredeterminado = value; }
      }
      private bool? esOpcionCorrecta;
      /// <summary>
      /// opcion correcta de la OpcionRespuestaPlantilla
      /// </summary>
      public bool? EsOpcionCorrecta{
         get{ return this.esOpcionCorrecta; }
         set{ this.esOpcionCorrecta = value; }
      }
      private bool? esInteres;
      /// <summary>
      /// es interes de la OpcionRespuestaPlantilla
      /// </summary>
      public bool? EsInteres
      {
          get { return this.esInteres; }
          set { this.esInteres = value; }
      }
      private Decimal? porcentajeCalificacion;
      /// <summary>
      /// Porcentaje de calificación asignado a esta OpcionRespuestaPlantilla
      /// </summary>
      public Decimal? PorcentajeCalificacion{
         get{ return this.porcentajeCalificacion; }
         set{ this.porcentajeCalificacion = value; }
      }

      private bool? activo;
      /// <summary>
      /// Bandera si la opcion esta Activa
      /// </summary>
      public bool? Activo
      {
          get { return this.activo; }
          set { this.activo = value; }
      }

      public virtual object Clone()
      {
          return this.MemberwiseClone();
      }
   } 
}
