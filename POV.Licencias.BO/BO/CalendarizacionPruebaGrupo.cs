using System;
using System.Collections.Generic;
using System.Text;
using POV.CentroEducativo.BO;
using POV.Prueba.BO;

namespace POV.Licencias.BO { 
   /// <summary>
   /// Clase Calendarizacion Prueba Grupo
   /// </summary>
   public class CalendarizacionPruebaGrupo { 
      private int? calendarizacionPruebaGrupoID;
      /// <summary>
      /// Identificador de CalendarizacionPruebaGrupo
      /// </summary>
      public int? CalendarizacionPruebaGrupoID{
         get{ return this.calendarizacionPruebaGrupoID; }
         set{ this.calendarizacionPruebaGrupoID = value; }
      }
      private bool? conVigencia;
      /// <summary>
      /// Determina si tiene vigencia la prueba
      /// </summary>
      public bool? ConVigencia{
         get{ return this.conVigencia; }
         set{ this.conVigencia = value; }
      }
      private DateTime? fechaInicioVigencia;
      /// <summary>
      /// Inicio de Vigencia
      /// </summary>
      public DateTime? FechaInicioVigencia{
         get{ return this.fechaInicioVigencia; }
         set{ this.fechaInicioVigencia = value; }
      }
      private DateTime? fechaFinVigencia;
      /// <summary>
      /// Fin de la Vigencia
      /// </summary>
      public DateTime? FechaFinVigencia{
         get{ return this.fechaFinVigencia; }
         set{ this.fechaFinVigencia = value; }
      }
      private DateTime? fechaRegistro;
      /// <summary>
      /// Fin de Registro
      /// </summary>
      public DateTime? FechaRegistro
      {
          get { return this.fechaRegistro; }
          set { this.fechaRegistro = value; }
      }
      private bool? activo;
      /// <summary>
      /// Determina si esta Activo o no
      /// </summary>
      public bool? Activo{
         get{ return this.activo; }
         set{ this.activo = value; }
      }
      private GrupoCicloEscolar grupoCicloEscolar;
      /// <summary>
      ///  GrupoCicloEscolar
      /// </summary>
      public GrupoCicloEscolar GrupoCicloEscolar{
         get{ return this.grupoCicloEscolar; }
         set{ this.grupoCicloEscolar = value; }
      }
      private PruebaContrato pruebaContrato;
      /// <summary>
      /// PruebaCOntrato
      /// </summary>
      public PruebaContrato PruebaContrato{
         get{ return this.pruebaContrato; }
         set{ this.pruebaContrato = value; }
      }
   } 
}
