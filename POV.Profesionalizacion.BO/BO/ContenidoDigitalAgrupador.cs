using System;
using POV.ContenidosDigital.BO;

namespace POV.Profesionalizacion.BO { 
   /// <summary>
   /// Contenido Digital Agrupador
   /// </summary>
   public class ContenidoDigitalAgrupador { 
      private Int64? contenidoDigitalAgrupadorID;
      /// <summary>
      /// Identificador del Contenido Digital Agrupador
      /// </summary>
      public Int64? ContenidoDigitalAgrupadorID{
         get{ return this.contenidoDigitalAgrupadorID; }
         set{ this.contenidoDigitalAgrupadorID = value; }
      }
      private EjeTematico ejeTematico;
      /// <summary>
      /// Eje Temático
      /// </summary>
      public EjeTematico EjeTematico{
         get{ return this.ejeTematico; }
         set{ this.ejeTematico = value; }
      }
      private SituacionAprendizaje situacionAprendizaje;
      /// <summary>
      /// Situación de Aprendizaje
      /// </summary>
      public SituacionAprendizaje SituacionAprendizaje{
         get{ return this.situacionAprendizaje; }
         set{ this.situacionAprendizaje = value; }
      }
      private AAgrupadorContenidoDigital agrupadorContenidoDigital;
      /// <summary>
      /// Agrupador de Contenido Digital
      /// </summary>
      public AAgrupadorContenidoDigital AgrupadorContenidoDigital{
         get{ return this.agrupadorContenidoDigital; }
         set{ this.agrupadorContenidoDigital = value; }
      }
      private ContenidoDigital contenidoDigital;
      /// <summary>
      /// Contenido Digital
      /// </summary>
      public ContenidoDigital ContenidoDigital{
         get{ return this.contenidoDigital; }
         set{ this.contenidoDigital = value; }
      }
      private bool? activo;
      /// <summary>
      /// Estado de Activo/Inactivo
      /// </summary>
      public bool? Activo{
         get{ return this.activo; }
         set{ this.activo = value; }
      }
      private DateTime? fechaRegistro;
      /// <summary>
      /// Fecha de registro
      /// </summary>
      public DateTime? FechaRegistro{
         get{ return this.fechaRegistro; }
         set{ this.fechaRegistro = value; }
      }
   } 
}
