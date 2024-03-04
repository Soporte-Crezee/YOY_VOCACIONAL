using System;
using System.Text;

namespace POV.CentroEducativo.BO { 
   /// <summary>
   /// Centro de Computo de una escuela
   /// </summary>
   public class CentroComputo { 
      private int? centroComputoID;
      /// <summary>
      /// CentroComputoID
      /// </summary>
      public int? CentroComputoID{
         get{ return this.centroComputoID; }
         set{ this.centroComputoID = value; }
      }
      private bool? tieneCentroComputo;
      /// <summary>
      /// TieneCentroComputo
      /// </summary>
      public bool? TieneCentroComputo{
         get{ return this.tieneCentroComputo; }
         set{ this.tieneCentroComputo = value; }
      }
      private bool? tieneInternet;
      /// <summary>
      /// TieneInternet
      /// </summary>
      public bool? TieneInternet{
         get{ return this.tieneInternet; }
         set{ this.tieneInternet = value; }
      }
      private decimal? anchoBanda;
      /// <summary>
      /// AnchoBanda
      /// </summary>
      public decimal? AnchoBanda{
         get{ return this.anchoBanda; }
         set{ this.anchoBanda = value; }
      }
      private string nombreProveedor;
      /// <summary>
      /// NombreProveedor
      /// </summary>
      public string NombreProveedor{
         get{ return this.nombreProveedor; }
         set{ this.nombreProveedor = value; }
      }
      private string tipoContrato;
      /// <summary>
      /// TipoContrato
      /// </summary>
      public string TipoContrato{
         get{ return this.tipoContrato; }
         set{ this.tipoContrato = value; }
      }
      private string responsable;
      /// <summary>
      /// Responsable
      /// </summary>
      public string Responsable{
         get{ return this.responsable; }
         set{ this.responsable = value; }
      }
      private long? telefonoResponsable;
      /// <summary>
      /// TelefonoResponsable
      /// </summary>
      public long? TelefonoResponsable{
         get{ return this.telefonoResponsable; }
         set{ this.telefonoResponsable = value; }
      }

      private int? numeroComputadoras;
      /// <summary>
      /// TelefonoResponsable
      /// </summary>
      public int? NumeroComputadoras
      {
          get { return this.numeroComputadoras; }
          set { this.numeroComputadoras = value; }
      }
      private DateTime? fechaRegistro;
      /// <summary>
      /// FechaRegistro
      /// </summary>
      public DateTime? FechaRegistro{
         get{ return this.fechaRegistro; }
         set{ this.fechaRegistro = value; }
      }
      private bool? activo;
      /// <summary>
      /// Activo
      /// </summary>
      public bool? Activo{
         get{ return this.activo; }
         set{ this.activo = value; }
      }
   } 
}
