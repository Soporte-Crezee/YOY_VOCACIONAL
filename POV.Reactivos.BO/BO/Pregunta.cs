using System;
using System.Collections.Generic;
using System.Text;

namespace POV.Reactivos.BO { 
   /// <summary>
   /// Pregunta
   /// </summary>
   public class Pregunta:ICloneable { 
      private int? preguntaID;
      /// <summary>
      /// identificador de la pregunta
      /// </summary>
      public int? PreguntaID{
         get{ return this.preguntaID; }
         set{ this.preguntaID = value; }
      }
      private int? orden;
      /// <summary>
      /// orden de la pregunta
      /// </summary>
      public int? Orden{
         get{ return this.orden; }
         set{ this.orden = value; }
      }
      private string descripcion;
      /// <summary>
      /// descripcion de la pregunta
      /// </summary>
      public string Descripcion{
         get{ return this.descripcion; }
         set{ this.descripcion = value; }
      }
      private decimal? valor;
      /// <summary>
      /// valor de la pregunta
      /// </summary>
      public decimal? Valor{
         get{ return this.valor; }
         set{ this.valor = value; }
      }
      private string textoPregunta;
      /// <summary>
      /// Texto de la pregunta
      /// </summary>
      public string TextoPregunta{
         get{ return this.textoPregunta; }
         set{ this.textoPregunta = value; }
      }
      private string plantillaPregunta;
      /// <summary>
      /// PlantillaPregunta de la pregunta
      /// </summary>
      public string PlantillaPregunta{
         get{ return this.plantillaPregunta; }
         set{ this.plantillaPregunta = value; }
      }
      private DateTime? fechaRegistro;
      /// <summary>
      /// Fecha de Registro de la pregunta
      /// </summary>
      public DateTime? FechaRegistro{
         get{ return this.fechaRegistro; }
         set{ this.fechaRegistro = value; }
      }
      private bool? puedeOmitir;
      /// <summary>
      /// Determinar si se Puede omitir la pregunta
      /// </summary>
      public bool? PuedeOmitir{
         get{ return this.puedeOmitir; }
         set{ this.puedeOmitir = value; }
      }
      private bool? activo;
      /// <summary>
      /// Determina si se encuentra activo el registro
      /// </summary>
      public bool? Activo{
         get{ return this.activo; }
         set{ this.activo = value; }
      }

      private bool? soloImagen;
      /// <summary>
      /// Determina si se encuentra activo el registro
      /// </summary>
      public bool? SoloImagen
      {
          get { return this.soloImagen; }
          set { this.soloImagen = value; }
      }
      private RespuestaPlantilla respuestaPlantilla;
      /// <summary>
      /// plantilla de la respuesta del reactivo
      /// </summary>
      public RespuestaPlantilla RespuestaPlantilla{
         get{ return this.respuestaPlantilla; }
         set{ this.respuestaPlantilla = value; }
      }

      /// <summary>
      /// PresentacionPlantilla de la pregunta
      /// </summary>
      private EPresentacionPlantilla? presentacionPlantilla;

      public EPresentacionPlantilla? PresentacionPlantilla
       {
           get { return this.presentacionPlantilla; }
           set { this.presentacionPlantilla = value; }
       }

      public virtual object Clone()
      {
          Pregunta nuevo = (Pregunta)this.MemberwiseClone();
          if(this.RespuestaPlantilla!=null)
                nuevo.RespuestaPlantilla = (RespuestaPlantilla)this.RespuestaPlantilla.Clone();
         
          return nuevo;
      }
   } 
}
