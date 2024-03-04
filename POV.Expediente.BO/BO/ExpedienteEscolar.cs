using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Data;
using POV.CentroEducativo.BO;

namespace POV.Expediente.BO
{ 
   /// <summary>
   /// Expediente Escolar de un alumno
   /// </summary>
    public class ExpedienteEscolar : ICloneable
    { 
      private long? expedienteEscolarID;
      /// <summary>
      /// Identificador del expediente
      /// </summary>
      public long? ExpedienteEscolarID{
         get{ return this.expedienteEscolarID; }
         set{ this.expedienteEscolarID = value; }
      }
      private Alumno alumno;
      /// <summary>
      /// Alumno asociado al expediente
      /// </summary>
      public Alumno Alumno{
         get{ return this.alumno; }
         set{ this.alumno = value; }
      }
      private List<DetalleCicloEscolar> detallesCicloEscolar;
      /// <summary>
      /// Detalles de lo ciclos escolares
      /// </summary>
      public List<DetalleCicloEscolar> DetallesCicloEscolar{
         get{ return this.detallesCicloEscolar; }
         set{ this.detallesCicloEscolar = value; }
      }
      private bool? activo;
      /// <summary>
      /// Activo
      /// </summary>
      public bool? Activo{
         get{ return this.activo; }
         set{ this.activo = value; }
      }
      private DateTime? fechaRegistro;
      /// <summary>
      /// Fecha de Registro
      /// </summary>
      public DateTime? FechaRegistro{
         get{ return this.fechaRegistro; }
         set{ this.fechaRegistro = value; }
      }
      public string Apuntes { get; set; }

       public object Clone()
      {
          return this.MemberwiseClone();
      }
   } 
}
