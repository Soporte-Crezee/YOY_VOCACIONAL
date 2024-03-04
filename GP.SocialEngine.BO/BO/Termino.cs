// Clase motor de red social
using System;
namespace GP.SocialEngine.BO { 
   /// <summary>
   /// Catalogo de sistemas disponibles
   /// </summary>
   public class Termino { 
      private long? terminoID;
      /// <summary>
      /// Identificador autonumerico de sistema para el usuario social
      /// </summary>
      public long? TerminoID{
         get{ return this.terminoID; }
         set{ this.terminoID = value; }
      }
      private string cuerpo;
      /// <summary>
      /// Email del usuario social
      /// </summary>
      public string Cuerpo{
         get{ return this.cuerpo; }
         set{ this.cuerpo = value; }
      }
      private DateTime? fechaCreacion;
      /// <summary>
      /// Nick del usuario social
      /// </summary>
      public DateTime? FechaCreacion{
         get{ return this.fechaCreacion; }
         set{ this.fechaCreacion = value; }
      }
      private bool? estatus;
      /// <summary>
      /// Nombre que mostrará el usuario
      /// </summary>
      public bool? Estatus{
         get{ return this.estatus; }
         set{ this.estatus = value; }
      }
   } 
}
